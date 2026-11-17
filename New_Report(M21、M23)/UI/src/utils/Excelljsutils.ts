import { saveAs } from 'file-saver';
import ExcelJS from 'exceljs';
import { Style } from 'exceljs';
import dayjs from 'dayjs';
import { IsDate } from '@/pages/ReportPortal/utils/common';
import { handleColuns } from './Treeflat';
import { message } from 'ant-design-vue';

export interface Iheader {
  key: any[];
  title: any[];
}

export interface exceljsparams {
  Header: Iheader;
  // 多級表頭只需要title就好了，key可以在一級表頭裏面取
  MutiHeader?: any[];
  style: {
    key: string;
    style: Style;
  }[];
}
export function exportexcelljs({
  tableprops = [] as any[],
  tabledatas = [] as any[],
  exceljsparams = {} as exceljsparams,
  filename = '導出',
  bookType = 'xlsx',
} = {}) {
  // 创建excel
  let workbook = new ExcelJS.Workbook();
  // 创建工作表
  let workSheet = workbook.addWorksheet('sheet1');
  //  創建人信息
  workbook.creator = 'admin';
  workbook.lastModifiedBy = 'admin';
  workbook.created = new Date();
  workbook.modified = new Date();
  workbook.lastPrinted = new Date();
  workbook.properties.date1904 = false; // 设置日期系统
  /* console.log('輸出exceljsparams', exceljsparams);
    console.log("輸出tableprops",tableprops);
    console.log("輸出tabledatas",tabledatas); */

  if (exceljsparams) {
    let { Header, MutiHeader, style } = exceljsparams;
    /* 
        js的對象遍歷屬性都是無序的需要自己傳遞
        */
    if (Header && Header.key.length && Header.title.length) {
      // 第一行作爲表頭
      workSheet.columns = Header.key.map((item, index) => {
        return {
          header: Header.title[index],
          key: item,
          width: 10,
          style: style && style.length ? style.find((sitem) => sitem.key == item)?.style : {},
        };
      });
      if (MutiHeader && MutiHeader.length) {
        // 其餘行作爲表體
        MutiHeader.forEach((item) => {
          let key = Header.key;
          let title = item;
          let obj = {};

          for (let i = 0; i < key.length; i++) {
            obj[key[i]] = title[i];
          }
          workSheet.addRows([obj]);

          /* workSheet.addRows(
                item.key.map((keyitem, kindex) => {
                    return {
                        header: item.title[kindex],
                        key: keyitem,
                        width: 10
                    };
                })
            ); */
        });
      }
    }
  } else {
    //多级表头处理
    let { titlearr, keyarr } = handleColuns(tableprops);
    if (titlearr.length == 0 || keyarr.length == 0) {
      return message.error('excel處理錯誤!');
    }
    titlearr.forEach((item, index) => {
      if (index == 0) {
        // titlearr第一行作爲表頭,其餘行作爲表體，keyarr最爲key
        workSheet.columns = item.map((Titem, Tindex) => {
          return {
            header: Titem,
            key: keyarr[Tindex],
            width: 10,
          };
        });
      } else {
        // 其餘行作爲表體
        let obj = {};
        for (let i = 0; i < item.length; i++) {
          obj[keyarr[i]] = item[i];
        }
        workSheet.addRows([obj]);
      }
    });
  }

  // 遍歷單元格
  // rowCount，文档的总行数。 等于具有值的最后一行的行号。
  // columnCount 文档的总列数。 等于所有行的最大单元数。

  // 遍歷列,行和列都是從1開始
  /*  for (let i = 1; i < worksheet.columnCount; i++) {
    const dobCol = worksheet.getColumn(i);
    // 遍历此列中的所有当前单元格
    dobCol.eachCell(function (cell, rowNumber) {
      console.log("輸出當前的單元格cell", cell);
      console.log("輸出當前的行號rowNumber", rowNumber);
      console.log("輸出當前單元格的值value", cell.value);
    });
  } */

  // 添加數據源
  console.log('輸出tabledatas', tabledatas);
  workSheet.addRows(tabledatas);
  // 遍歷行
  for (let i = 1; i <= workSheet.rowCount; i++) {
    const row = workSheet.getRow(i);
    // 连续遍历所有非空单元格
    // 只需要讓表頭的日期更改就好，太多循環會很卡
    // if (i == 1) {
    row.eachCell(function (cell, colNumber) {
      // js時間轉成excell時間
      if (cell.value && IsDate(String(cell.value))) {
        cell.value = dayjs(String(cell.value)).add(dayjs().utcOffset(), 'minute').toDate();
        cell.numFmt = 'yyyy/m/d';
      }
      // 設置數字
      if (cell.value && /^[+-]?\d*(\.\d*)?(e[+-]?\d+)?$/.test(String(cell.value))) {
        cell.value = Number(cell.value);
      }
      // 設置百分比
      if (cell.value && String(cell.value).trim().length > 1 && String(cell.value).includes('%')) {
        cell.numFmt = '0.00%';
      }

      // console.log('Cell ' + colNumber + ' = ' + cell.value);
    });
    /*  } else {
      break;
    } */
  }

  console.log('输出worksheet', workSheet);

  // 导出文件
  workbook.xlsx.writeBuffer().then((buffer) => {
    saveAs(
      new Blob([buffer], {
        type: 'application/octet-stream',
      }),
      `${filename}.${bookType}`,
    );
  });
}
