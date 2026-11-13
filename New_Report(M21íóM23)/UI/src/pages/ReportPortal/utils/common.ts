import type { TableColumn } from '@/components/core/dynamic-table';
import { AddCommonPageParamsWithOut } from '@/store/module/commonpage';
import { message } from 'ant-design-vue';
import router from '@/router';
import { cloneDeep } from 'lodash-es';
import { PageShowHidden } from '@/components/core/dynamic-table/src/types/setting';
import { Column } from 'vxe-table';
import { ColumnsType } from 'ant-design-vue/lib/table';

/**
 * @param PageParams 頁面所有配置
 */
export function showHidden(PageParams: PageShowHidden) {
  PageParams.ShowMenu = !PageParams.ShowMenu;
  if (PageParams.ShowMenu) PageParams.TableSpan = 20;
  else PageParams.TableSpan = 24;
}

/**
 *
 * @param count 結束行
 * @param count1 開始行
 * @param data 原始的縂數據
 * @param calculation 需要加總的列名
 * @returns 獲取加總的數據
 */
export function getSumData(count: number, count1: number, data: any[], calculation: any[]) {
  let SubTotal = {};
  for (let item of calculation) {
    let sum = 0;
    for (let i = count1; i < count; i++) {
      let a = isNaN(data[i][item]) ? 0 : data[i][item];
      sum = sum + a;
    }
    SubTotal[item] = sum;
  }
  return SubTotal;
}
/**
 *
 * @param record 给表格添加css样式
 * @returns
 */
export function getRowClassNmae(record: Object) {
  if (record && Object.keys(record).length != 0) {
      for (const [key, value] of Object.entries(record)) {
          if (
              typeof value == 'string' &&
              ['小计', 'BANK_SUM', 'WIP_SUM', '小計', '合計', 'Total', '品', 'Diff', 'TTL'].includes(value)
          ) {
              return 'YellowBold';
          } else if (
              typeof value == 'string' &&
              ['總計', 'SUM', 'RWK', '总计', 'Sum', '合计', 'Sub Total', 'total'].includes(value)
          ) {
              return 'GreenBold';
          } else if (typeof value == 'string' && value.includes('Sub Sum')) {
              return 'YellowBold';
          } else if (typeof value == 'string' && value.includes('品')) {
              return 'YellowBold';
          } else if (typeof value == 'string' && value.includes('subtotal')) {
              return 'GreenBold';
          } else {
              if (typeof value == 'object') {
              } else if (typeof value == 'number') {
              }
          }
      }
  }
}


/**
 * 
 * @param record vxetable row class
 * @returns 
 */
export function getRowClassNmaeVxe({row}) {
    let record = row
    if (record && Object.keys(record).length != 0) {
        for (const [key, value] of Object.entries(record)) {
            if (
                typeof value == 'string' &&
                ['小计', 'BANK_SUM', 'WIP_SUM', '小計', '合計', 'Total', '品', 'Diff', 'TTL'].includes(value)
            ) {
                return 'YellowBold';
            } else if (
                typeof value == 'string' &&
                ['總計', 'SUM', 'RWK', '总计', 'Sum', '合计', 'Sub Total', 'total'].includes(value)
            ) {
                return 'GreenBold';
            } else if (typeof value == 'string' && value.includes('Sub Sum')) {
                return 'YellowBold';
            } else if (typeof value == 'string' && value.includes('品')) {
                return 'YellowBold';
            } else if (typeof value == 'string' && value.includes('subtotal')) {
                return 'GreenBold';
            } else {
                if (typeof value == 'object') {
                } else if (typeof value == 'number') {
                }
            }
        }
    }
}


export function mergeTableRow(config: any) {
  //合并表格的方法
  let data = config['data'];
  const { mergeColNames, firstMerge } = config;
  if (!mergeColNames || mergeColNames.length === 0) {
    return data;
  }
  mergeColNames.forEach((item: string, mergeIndex: number) => {
    //遍历需要合并的列
    const mList = {};
    data = data.map((Source: any, index: number) => {
      const rowVal = Source[item]; //每行对应合并列的值
      if (mList[rowVal] && mList[rowVal].newIndex === index) {
        // mergeColNames.forEach((meta: string,metaIndex:number)=>{
        //     if(meta==item){没思路，改不了，等会改

                //     }
                // })
                if (item == firstMerge) {
                    data[mList[rowVal]['index']][item + '-span'].rowspan++;
                    Source[item + '-span'] = {
                        rowspan: 0,
                        colspan: 0
                    };
                    mList[rowVal]['num']++;
                    mList[rowVal]['newIndex']++;
                } else if (mergeIndex == 1 && data[index][firstMerge] == data[index - 1][firstMerge]) {
                    data[mList[rowVal]['index']][item + '-span'].rowspan++;
                    Source[item + '-span'] = {
                        rowspan: 0,
                        colspan: 0
                    };
                    mList[rowVal]['num']++;
                    mList[rowVal]['newIndex']++;
                } else if (
                    mergeIndex == 2 &&
                    data[index][firstMerge] == data[index - 1][firstMerge] &&
                    data[index][mergeColNames[mergeIndex - 1]] == data[index - 1][mergeColNames[mergeIndex - 1]]
                ) {
                    data[mList[rowVal]['index']][item + '-span'].rowspan++;
                    Source[item + '-span'] = {
                        rowspan: 0,
                        colspan: 0
                    };
                    mList[rowVal]['num']++;
                    mList[rowVal]['newIndex']++;
                } else {
                    mList[rowVal] = { num: 1, index: index, newIndex: index + 1 };
                    Source[item + '-span'] = {
                        rowspan: 1,
                        colspan: 1
                    };
                }
            } else {
                mList[rowVal] = { num: 1, index: index, newIndex: index + 1 };
                Source[item + '-span'] = {
                    rowspan: 1,
                    colspan: 1
                };
            }
            return Source;
        });

    });
    return data;
}
export function mergeTableRow1(config:any) {
    let data = config.data;
    const { mergeColNames, firstMergeColNames, firstMerge } = config;
    if (!mergeColNames || mergeColNames.length === 0) {
      return data;
    }
    mergeColNames.forEach((m) => {
      const mList = {};
      data = data.map((v, index) => {
        const rowVal = v[m];
        if (mList[rowVal] && mList[rowVal].newIndex === index) {
          const flag =
            firstMergeColNames.filter((f) => {
              return f === m;
            }).length !== 0;
          const mcFlag =
            mergeColNames.filter((mc) => {
              return mc === firstMerge;
            }).length === 0;
          if (
            (mcFlag && flag) ||
            (flag &&
              data[index][firstMerge + "-span"] &&
              data[index][firstMerge + "-span"].rowspan === 1)
          ) {
            v[m + "-span"] = {
              rowspan: 1,
              colspan: 1,
            };
          } else {
            data[mList[rowVal]["index"]][m + "-span"].rowspan++;
            v[m + "-span"] = {
              rowspan: 0,
              colspan: 0,
            };
            mList[rowVal]["num"]++;
            mList[rowVal]["newIndex"]++;
          }
        } else {
          mList[rowVal] = { num: 1, index: index, newIndex: index + 1 };
          v[m + "-span"] = {
            rowspan: 1,
            colspan: 1,
          };
        }
        return v;
      });
      
    });
    return data;
  }

export function TableCustomCell(
  Label: string[],
  Prop: string[],
  ClickColumn: string[] = [],
  isMergeColumns: boolean = false,
  ToOtherpage?: string,
  FixedColumns?: string[],
) {
  return Prop.map((item: string, index: number) => {
    return {
      title: Label[index], //可点击的栏位显示*号
      align: 'center' as any,
      dataIndex: item,
      fixed: FixedColumns && FixedColumns.includes(item) ? 'left' : '',
      // width:30,
      // className: 'v-center',
      //@ts-ignore
      customCell: (record: Object, rowindex: number, column: Object) => {
        if (ClickColumn.includes(item)) {
          return {
            onDblclick: () => {
              const UseParamsAdd = AddCommonPageParamsWithOut();
              // UseParamsAdd.AddSecondTableParams({ ...record, ...column })
              UseParamsAdd.AddFirstTableParams({ ...record, ...column['__originColumn__'] });
              if (ToOtherpage) router.push({ name: ToOtherpage });
            },
            style: { cursor: 'pointer' },
          };
        } else if (isMergeColumns) {
          // Prop.forEach(item => {
          //     if (record[item] == '小计' || record[item] == '合计') {
          //         return {
          //             colSpan: 3,
          //             rowSpan: 1
          //         }
          //     }
          //     // else {
          //     //     return {
          //     //         rowSpan: record?.[`${item}-span`]?.['rowspan'] || 0,
          //     //         colSpan: 1
          //     //     }
          //     // }
          // })
          return {
            rowSpan: record?.[`${item}-span`]?.['rowspan'] || 0,
            colSpan: record?.[`${item}-span`]?.['colspan'] || 0,
            style: {},
          };
        } else {
          return {
            style: {},
            onDblclick: () => {
              message.warning('此栏位没有二阶数据');
            },
          };
        }
      },
    };
  });
}
/**
 *
 * @param stime 起始日期
 * @param etime 终止日期
 * @returns 获取两个日期中间的日期
 */
export function getdiffdate(stime: string, etime: string) {
  //初始化日期列表，数组
  var diffdate = new Array();
  var i = 0;
  //开始日期小于等于结束日期,并循环
  while (stime <= etime) {
    diffdate[i] = stime;

    //获取开始日期时间戳
    var stime_ts = new Date(stime).getTime();
    // console.log('当前日期：' + stime + '当前时间戳：' + stime_ts);

    //增加一天时间戳后的日期
    var next_date = stime_ts + 24 * 60 * 60 * 1000;

    //拼接年月日，这里的月份会返回（0-11），所以要+1
    var next_dates_y = new Date(next_date).getFullYear() + '-';
    var next_dates_m =
      new Date(next_date).getMonth() + 1 < 10
        ? '0' + (new Date(next_date).getMonth() + 1) + '-'
        : new Date(next_date).getMonth() + 1 + '-';
    var next_dates_d =
      new Date(next_date).getDate() < 10
        ? '0' + new Date(next_date).getDate()
        : new Date(next_date).getDate();

    stime = next_dates_y + next_dates_m + next_dates_d;

    //增加数组key
    i++;
  }
  return diffdate;
}
// 最后一行总计
export function getTotalData(data: any[], DateColumns: any[]) {
  //合計
  let obj = {};
  DateColumns.map((item: string) => {
    let sum = 0;
    data.map((n) => {
      sum += n[item];
    });
    obj[item] = sum;
  });
  return obj;
}
// 列总计
/**
 *
 * @param row
 * @param DateColumns
 * @returns
 */
export function getAllSumData(row: any, DateColumns: any[]) {
  let sum = 0;
  DateColumns.map((item: any) => {
    row[item] = isNaN(row[item]) ? 0 : row[item];
    sum += row[item];
  });
  return sum;
}
// 表格共用方法
// 动态添加属性
// text:當前行的值     key：當前元素的列名    index:當前行的索引   dataSource:數據源
export function mergeCells(text: string, key: string, index: number, dataSource: any[]) {
  // 上一行该列数据是否一样
  if (index !== 0 && text === dataSource[index - 1][key]) {
    return 0;
  }
  let rowSpan = 1;
  // 判断下一行是否相等
  for (let i = index + 1; i < dataSource.length; i++) {
    if (text !== dataSource[i][key]) {
      break;
    }
    rowSpan++;
  }
  return rowSpan;
}
export function TableCustomCell0(
  Label: string[],
  Prop: string[],
  ClickColumn: string[] = [],
  Func: Function | null,
  mergeCellCol: any[] = [],
  dataSource: any[] = [],
  ColStyle: Object[] = [],
) {
  return Prop.map((item, index) => {
    return {
      // title: ClickColumn.includes(item) ? `${Label[index]}*` : Label[index],//可点击的栏位显示*号
      title: Label[index],
      align: 'center' as any,
      dataIndex: item,
      // className: 'v-center',
      customCell: (record: Object, rowindex: number, column: Object) => {
        // 合并行功能
        const obj = {
          children: record[item] !== null ? record[item] : '',
          attrs: {},
        };
        if (mergeCellCol.includes(item)) {
          obj.attrs['rowSpan'] = mergeCells(record[item], item, rowindex, dataSource);
        }
        let stylefunc = () => {
          let tempStyle = {};
          if (ClickColumn.includes(item)) {
            tempStyle = { cursor: 'pointer' };
          }
          // 单元格添加样式功能
          ColStyle.forEach((coloritem) => {
            if (item == coloritem['dataIndex']) {
              tempStyle = { ...tempStyle, ...coloritem['style'] };
            }
          });
          return { ...tempStyle };
        };
        return {
          onDblclick: async () => {
            if (ClickColumn.includes(item)) {
              if (Func) {
                Func(record, rowindex, column, item, index);
              }
            } else {
              message.warning('此栏位没有二阶数据');
            }
          },
          style: stylefunc(),
          rowSpan: obj.attrs['rowSpan'],
        };
      },
    };
  });
}
// 一级表头表格功能:包含双击，添加样式，合并列
export function TableCustomCell1(
  Columns: object[],
  ClickColumn: string[] = [],
  mergeCellCol: any[] = [],
  dataSource: any[] = [],
  ColStyle: Object[] = [],
  Func: Function,
) {
  Columns.forEach((item, index) => {
    // item["title"] = ClickColumn.includes(item["dataIndex"]) ? `${item["title"]}*` : item["title"];//可点击的栏位显示*号
    item['title'] = item['title']; //可点击的栏位显示*号
    item['customCell'] = (record: Object, rowindex: number, column: Object) => {
      // 合并行功能
      const obj = {
        children:
          !record[item['dataIndex']] &&
          typeof record[item['dataIndex']] != 'undefined' &&
          !isNaN(record[item['dataIndex']])
            ? record[item['dataIndex']]
            : '',
        attrs: {},
      };
      if (mergeCellCol.includes(item['dataIndex'])) {
        obj.attrs['rowSpan'] = mergeCells(
          record[item['dataIndex']],
          item['dataIndex'],
          rowindex,
          dataSource,
        );
      }
      let stylefunc = () => {
        let tempStyle = {};
        if (ClickColumn.includes(item['dataIndex'])) {
          tempStyle = { cursor: 'pointer' };
        }
        // 单元格添加样式功能
        ColStyle.forEach((coloritem) => {
          if (item['dataIndex'] == coloritem['dataIndex']) {
            tempStyle = { ...tempStyle, ...coloritem['style'] };
          }
        });
        return { ...tempStyle };
      };
      return {
        onDblclick: async () => {
          if (ClickColumn.includes(item['dataIndex'])) {
            Func(record, rowindex, column, item, index);
          } else {
            message.warning('此栏位没有二阶数据');
          }
        },
        style: stylefunc(),
        rowSpan: obj.attrs['rowSpan'],
      };
    };
  });
}

// 二阶表头表格功能
export function TableCustomCell2(
  Columns: object[],
  ClickColumn: string[] = [],
  mergeCellCol: any[] = [],
  dataSource: any[] = [],
  ColStyle: Object[] = [],
  Func: Function,
) {
  Columns.forEach((item) => {
    if (item['children']) {
      item['children'].forEach((citem: object, cindex: number) => {
        // citem["title"] = ClickColumn.includes(citem["dataIndex"]) ? `${citem["title"]}*` : citem["title"];//可点击的栏位显示*号
        citem['title'] = citem['title']; //可点击的栏位显示*号
        citem['customCell'] = (record: Object, rowindex: number, column: Object) => {
          // 合并行功能
          const obj = {
            children:
              !record[item['dataIndex']] &&
              typeof record[item['dataIndex']] != 'undefined' &&
              !isNaN(record[item['dataIndex']])
                ? record[item['dataIndex']]
                : '',
            attrs: {},
          };
          if (mergeCellCol.includes(citem['dataIndex'])) {
            obj.attrs['rowSpan'] = mergeCells(
              record[citem['dataIndex']],
              citem['dataIndex'],
              rowindex,
              dataSource,
            );
          }
          let stylefunc = () => {
            let tempStyle = {};
            if (ClickColumn.includes(citem['dataIndex'])) {
              tempStyle = { cursor: 'pointer' };
            }
            if (ColStyle.length != 0) {
              // 单元格添加样式功能
              ColStyle.forEach((coloritem) => {
                if (citem['dataIndex'] == coloritem['dataIndex']) {
                  tempStyle = { ...tempStyle, ...coloritem['style'] };
                }
              });
            }
            return { ...tempStyle };
          };
          return {
            onDblclick: async () => {
              if (ClickColumn.includes(citem['dataIndex'])) {
                Func(record, rowindex, column, citem, cindex, item);
              } else {
                message.warning('此栏位没有二阶数据');
              }
            },
            style: stylefunc(),
            rowSpan: obj.attrs['rowSpan'],
          };
        };
      });
    }
  });
}

// 合并列功能
/**
 *
 * @param Columns 表格列的配置
 * @param mergeCellCol 要合并的列
 * @param dataSource 数据源
 */
export function megercell(Columns: object[], mergeCellCol: any[] = [], dataSource: any[] = []) {
  Columns.forEach((item, index) => {
    item['customCell'] = (record: Object, rowindex: number, column: Object) => {
      /*  console.log(record);
             console.log(rowindex);
             console.log(column);
             console.log(item);
             console.log(index); */
      // 合并行功能
      const obj = {
        children:
          !record[item['dataIndex']] &&
          typeof record[item['dataIndex']] != 'undefined' &&
          !isNaN(record[item['dataIndex']])
            ? record[item['dataIndex']]
            : '',
        attrs: {},
      };
      if (mergeCellCol.includes(item['dataIndex'])) {
        obj.attrs['rowSpan'] = mergeCells(
          record[item['dataIndex']],
          item['dataIndex'],
          rowindex,
          dataSource,
        );
      }
      return {
        rowSpan: obj.attrs['rowSpan'],
      };
    };
  });
}
export function megercellVxe(Columns: object[], mergeCellCol: any[] = [], dataSource: any[] = []) {
    let megercellData:any = []
    dataSource.forEach((record,index1)=>{
        Columns.forEach((item, index) => {
            let spanData = {colspan:1}
            if (mergeCellCol.includes(item['dataIndex'])) {
                spanData['rowspan'] = mergeCells(record[item['dataIndex']], item['dataIndex'], index1, dataSource);
                spanData['row'] = index1
                spanData['col'] = index
                megercellData.push(spanData)
            }
           
        
    });
    })
    
   return megercellData.filter(item=>item.rowspan!=0)
}

//封装customCell
/**
 *
 * @param Columns 表格列的配置
 * @param Func 函数返回执行的函数
 * @param 当前列包括父级
 */
export function antcustomCell(Columns: object[], Func: Function, paramsColumns: Object = {}) {
  Columns.forEach((item: any, index: number) => {
    if (item.children && item.children.length) {
      antcustomCell(item.children, Func, item);
    } else {
      item['customCell'] = (record: Object, rowindex: number, column: Object) => {
        return Func({ record, rowindex, column, item, index, paramsColumns });
      };
    }
  });
}
//

// 双击功能
/**
 *
 * @param Columns 当前列的配置
 * @param ClickColumn 可点击列数组
 * @param Func 点击列的操作
 */
export function antdclick(Columns: any, ClickColumn: string[] = [], Func: Function) {
  return {
    onDblclick: async () => {
      if (ClickColumn.includes(Columns['dataIndex'])) {
        Func();
        return { style: { cursor: 'pointer' } };
      } else {
        message.warning('此栏位没有二阶数据');
      }
    },
    // 不能使用箭头函数
    style: (function () {
      if (ClickColumn.includes(Columns['dataIndex'])) {
        return { cursor: 'pointer' };
      }
    })(),
  };
}
/**
 * @param Column 当前列的配置
 * @param mergeCellCol 需要合并列的数组
 * @param dataSource
 *
 */
export function antmegercell(Column: any, mergeCellCol: any[] = [], dataSource: any[] = []) {
  let { record, rowindex, column } = Column;
  // 合并行功能
  const obj = {
    children:
      !record[column['dataIndex']] &&
      typeof record[column['dataIndex']] != 'undefined' &&
      !isNaN(record[column['dataIndex']])
        ? record[column['dataIndex']]
        : '',
    attrs: {},
  };
  if (mergeCellCol.includes(column['dataIndex'])) {
    obj.attrs['rowSpan'] = mergeCells(
      record[column['dataIndex']],
      column['dataIndex'],
      rowindex,
      dataSource,
    );
  }
  return {
    rowSpan: obj.attrs['rowSpan'],
  };
}

// 双击功能功能
/**
 *
 * @param Columns 表格列的配置
 * @param ClickColumn 要点击的列
 * @param Func 点击之后执行的函数
 */
export function dclick(Columns: object[], ClickColumn: string[] = [], Func: Function) {
  //可点击的栏位显示*号
  Columns.forEach((item, index) => {
    let tempcustomCell = {};
    if (item['customCell']) {
      tempcustomCell = cloneDeep(item['customCell']());
    }
    item['customCell'] = (record: Object, rowindex: number, column: Object) => {
      let stylefunc = () => {
        let tempStyle = {};
        if (ClickColumn.includes(item['dataIndex'])) {
          tempStyle = { cursor: 'pointer' };
        }
        if (tempcustomCell && tempcustomCell['style']) {
          return { ...tempStyle, ...tempcustomCell['style'] };
        } else {
          return { ...tempStyle };
        }
      };
      return {
        ...tempcustomCell,
        onDblclick: async () => {
          if (ClickColumn.includes(item['dataIndex'])) {
            Func(record, rowindex, column, item, index);
          } else {
            message.warning('此栏位没有二阶数据');
          }
        },
        style: stylefunc(),
      };
    };
  });
}
// 添加樣式功能
/* 
StyleArr:對象數組
    dataIndex:屬性名
    [
        {dataIndex:"",style:{}}
    ]
*/
export function addstyle(Columns: object[], StyleArr: Object[]) {
  Columns.forEach((item, index) => {
    let tempcustomCell = {};
    if (item['customCell']) {
      tempcustomCell = cloneDeep(item['customCell']());
    }
    item['customCell'] = (record: Object, rowindex: number, column: Object) => {
      let stylefunc = () => {
        StyleArr.forEach((sitem) => {
          if (sitem['dataIndex'] == item['dataIndex']) {
            if (tempcustomCell['style']) {
              return { ...tempcustomCell['style'], ...sitem['style'] };
            } else {
              return sitem['style'];
            }
          } else {
          }
        });
      };
      return {
        ...tempcustomCell,
        style: stylefunc(),
      };
    };
  });
}

// 最後一行總計，列計算
/* 
dataSource：數據源
calcuList：要計算的列
sumprops：給屬性賦值為總計
isNegative：是否轉爲負數
     */
export function getfinalsum(
  dataSource: any[],
  calcuList: any[],
  sumprops: string,
  sumname: string = '總計',
  isNegative: boolean = false,
) {
  if (Array.isArray(dataSource) && dataSource.length != 0) {
    const tempObj = cloneDeep(dataSource[0]);
    calcuList.forEach((el) => {
      for (let i = 1; i < dataSource.length; i++) {
        if (isNaN(tempObj[el])) {
          tempObj[el] = 0;
        }
        if (!isNaN(dataSource[i][el])) {
          tempObj[el] = Number(tempObj[el]);
          tempObj[el] += Number(dataSource[i][el]);
        }
      }
    });
    // 不包含在属性数组中的属性设置为空
    Object.keys(tempObj).forEach((el) => {
      if (!calcuList.includes(el)) {
        tempObj[el] = null;
      }
      if (el == sumprops) {
        tempObj[el] = sumname;
      }
      // 總計需要是負數的處理
      // 不包含在數組裏面的就是整數
      if (isNegative && calcuList.includes(el)) {
        tempObj[el] = -tempObj[el];
      }
    });

    return tempObj;
  }
}
// 行計算
/* 
dataSource：數據源
calcuList：要計算的列
sumprops：添加的屬性的key
isNegative：是否轉爲負數
*/
export function getrowsum(
  dataSource: any[],
  calcuList: any[],
  sumprop: string,
  isNegative: boolean = false,
) {
  if (dataSource.length != 0) {
    dataSource.forEach((item) => {
      item[sumprop] = 0;
      for (let i = 0; i < calcuList.length; i++) {
        if (!isNaN(item[calcuList[i]])) {
          item[calcuList[i]] = Number(item[calcuList[i]]);
          item[sumprop] += item[calcuList[i]];
        }
      }
      if (isNegative) {
        item[sumprop] = -item[sumprop];
      }
    });
  }
}
// 插入行属性计算
// 插入行合计,dataSource数据源，standard:相同的屬性才合計，calcuList要合计的列，是否是负数isNegative,合计属性sumprops,isInsert是否插入，不为空的栏位
export function Inserttotalrow(
  dataSource: any[],
  standard: any[],
  calcuList: any[],
  sumprops: string,
  sumname: string = '合計',
  isNegative: boolean = false,
  isInsert: boolean = true,
  isNotEnpty: string[] = [],
) {
  if (dataSource.length != 0) {
    let temp: any = cloneDeep(dataSource[0]);
    let totalsum: any[] = [];
    let totalindex: any[] = [];
    let regArr: any[] = [];
    for (let i = 0; i < dataSource.length - 1; i++) {
      standard.forEach((item) => {
        regArr.push(dataSource[i][item] == dataSource[i + 1][item]);
      });
      if (regArr.includes(false)) {
        regArr = [];
        Object.keys(temp).forEach((item) => {
          if (!calcuList.includes(item)) {
            if (!isNotEnpty.includes(item)) {
              temp[item] = '';
            }
          }
          if (item == sumprops) {
            temp[item] = sumname;
          }
          // 合計需要是負數的處理
          // 不包含在數組裏面的就是整數
          if (isNegative && calcuList.includes(item)) {
            temp[item] = -Math.ceil(temp[item]);
          }
        });
        totalindex.push(i + 1);
        totalsum.push(temp);
        temp = cloneDeep(dataSource[i + 1]);
        continue;
      } else {
        for (let j = 0; j < calcuList.length; j++) {
          if (isNaN(temp[calcuList[j]])) {
            temp[calcuList[j]] = 0;
          }
          if (!isNaN(dataSource[i + 1][calcuList[j]])) {
            temp[calcuList[j]] = Number(temp[calcuList[j]]);
            dataSource[i + 1][calcuList[j]] = Number(dataSource[i + 1][calcuList[j]]);
            temp[calcuList[j]] += dataSource[i + 1][calcuList[j]];
          }
        }
      }
    }
    Object.keys(temp).forEach((item) => {
      if (!calcuList.includes(item)) {
        if (!isNotEnpty.includes(item)) {
          temp[item] = '';
        }
      }

      if (item == sumprops) {
        temp[item] = sumname;
      }
      // 合計需要是負數的處理
      if (isNegative && calcuList.includes(item)) {
        temp[item] = -Math.ceil(temp[item]);
      }
    });
    totalsum.push(temp);
    totalindex.push(dataSource.length);
    if (isNotEnpty.length != 0) {
      for (let i = 0; i < totalindex.length; i++) {
        Object.keys(totalsum[i]).forEach((key, keyindex) => {
          if (isNotEnpty.includes(key)) {
            totalsum[i][key] = dataSource[totalindex[i] - 1][key];
          }
        });
      }
    }
    if (isInsert) {
      for (let i = 0; i < totalindex.length; i++) {
        dataSource.splice(totalindex[i] + i, 0, totalsum[i]);
      }
    }
    return {
      insertrowList: totalsum,
      insertIndexList: totalindex,
    };
  } else {
    return {
      insertrowList: [],
      insertIndexList: [],
    };
  }
}
// 插入行的行合计
export function Insertrowcount(
  dataSource: any[],
  standard: any[],
  calcuList: any[],
  sumprops: string,
  sumname: string = '合計',
  isNegative: boolean = false,
  isInsert: boolean = true,
) {
  let temp: any = cloneDeep(dataSource[0]);
  let totalsum: any[] = [];
  let totalindex: any[] = [];
  let regArr: any[] = [];
  for (let i = 0; i < dataSource.length - 1; i++) {
    standard.forEach((item) => {
      regArr.push(dataSource[i][item] == dataSource[i + 1][item]);
    });
    if (regArr.includes(false)) {
      regArr = [];
      Object.keys(temp).forEach((item) => {
        if (!calcuList.includes(item)) {
          temp[item] = '';
        }
        if (item == sumprops) {
          temp[item] = sumname;
        }
        // 合計需要是負數的處理
        // 不包含在數組裏面的就是整數
        if (isNegative && calcuList.includes(item)) {
          temp[item] = -Math.ceil(temp[item]);
        }
      });

      totalindex.push(i + 1);
      for (let j = 0; j < calcuList.length; j++) {
        if (isNaN(temp[calcuList[j]])) {
          temp[calcuList[j]] = 1;
        }
      }
      totalsum.push(temp);
      temp = cloneDeep(dataSource[i + 1]);
      for (let j = 0; j < calcuList.length; j++) {
        if (isNaN(temp[calcuList[j]])) {
          temp[calcuList[j]] = 1;
        }
      }
      continue;
    } else {
      for (let j = 0; j < calcuList.length; j++) {
        if (isNaN(temp[calcuList[j]])) {
          temp[calcuList[j]] = 1;
        }
        temp[calcuList[j]]++;
      }
    }
  }
  Object.keys(temp).forEach((item) => {
    if (!calcuList.includes(item)) {
      temp[item] = '';
    }

    if (item == sumprops) {
      temp[item] = sumname;
    }
    // 合計需要是負數的處理
    if (isNegative && calcuList.includes(item)) {
      temp[item] = -Math.ceil(temp[item]);
    }
  });
  totalsum.push(temp);
  totalindex.push(dataSource.length);
  if (isInsert) {
    for (let i = 0; i < totalindex.length; i++) {
      dataSource.splice(totalindex[i] + i, 0, totalsum[i]);
    }
  }
  return {
    insertrowList: totalsum,
    insertIndexList: totalindex,
  };
}

/* // 數組對象根據屬性名（字符串）排序，prop:屬性名
export function compare(a, b) {
    // Use toUpperCase() to ignore character casing
    const bandA = a[c].toUpperCase();
    const bandB = b[c].toUpperCase();

    let comparison = 0;
    if (bandA > bandB) {
        comparison = 1;
    } else if (bandA < bandB) {
        comparison = -1;
    }
    return comparison;
} */

/**
 * 定义一个比较器
 * @param propertyName
 * @param sortType
 * @returns {function(...[*]=)}
 */
function compare(propertyName: string, sortType: string) {
  return function (object1, object2) {
    let value1;
    let value2;
    if (typeof object1[propertyName] == 'string' && typeof object2[propertyName] == 'string') {
      value1 = object1[propertyName].toUpperCase();
      value2 = object2[propertyName].toUpperCase();
    } else if (
      typeof object1[propertyName] == 'number' &&
      typeof object2[propertyName] == 'number'
    ) {
      value1 = object1[propertyName];
      value2 = object2[propertyName];
    }
    if (sortType === 'asc') {
      if (value2 > value1) {
        return -1;
      } else if (value2 < value1) {
        return 1;
      } else {
        return 0;
      }
    } else {
      if (value2 < value1) {
        return -1;
      } else if (value2 > value1) {
        return 1;
      } else {
        return 0;
      }
    }
  };
}

/**
 * 对数组使用元素里的一个属性值排序
 * @param arr
 * @param propertyName 属性名字,值为数字或者為字符串
 * @param sortType 默认desc
 * @returns {*}
 */
export function sortArray(arr: any[], propertyName: string, sortType: string = 'desc') {
  return arr.sort(compare(propertyName, sortType));
}
export function countStrWidth(text: string) {
  const canvas = document.createElement('canvas');
  const context = canvas.getContext('2d') as CanvasRenderingContext2D;
  // 设置字体样式，当然，也可以在这里给一个默认值
  context.font = '14px Microsoft YaHei'; // 字体一定要写，不能只写一个字体大小，不启用
  const dimension = context.measureText(text);
  return dimension.width;
 }
 /**
 *table設置動態列寬
 * @param prop 列的屬性名
 * @param tableData 表格數據
 * @param title 列的標題
 * @param num 列的長度
 * @param deeptitlelength上一級表頭的寬度
 * @returns
 */
 export function flexWidth(
  prop: string,
  tableData: any[],
  title: string,
  num: number = 0,
  deeptitlelength: number = 0,
 ) {
  // 1参数'prop'
  // 2参数 tableData
  // 3参数 'lable' 可不传
  if (!tableData || tableData.length === 0) {
    //表格没数据不做处理
    return;
  }
  let flexWidth = 0; //初始化表格列宽
  let columnContent = ''; //占位最宽的内容
  const canvas: HTMLCanvasElement = document.createElement('canvas');
  const context = canvas.getContext('2d') as CanvasRenderingContext2D;
  context.font = '14px Microsoft YaHei';
  if (prop === '' && title) {
    //标题长内容少的，取标题的值,
    columnContent = title;
  } else {
    // 获取该列中占位最宽的内容
    let index = 0;
    for (let i = 0; i < tableData.length; i++) {
      //判斷是否是undefined情況
      const now_temp = tableData[i][prop] ? String(tableData[i][prop]) : '';
      const max_temp = tableData[index][prop] ? String(tableData[index][prop]) : '';
      const now_temp_w = context.measureText(now_temp).width;
      const max_temp_w = context.measureText(max_temp).width;
      if (now_temp_w > max_temp_w) {
        index = i;
      }
    }
    columnContent = tableData[index][prop];
    //比较占位最宽的值跟标题、标题为空的留出四个位置
    const column_w = context.measureText(columnContent).width;
    const title_w = context.measureText(title).width;
    if (column_w < title_w) {
      columnContent = title || '留四个字';
    }
  }
  // 计算最宽内容的列宽
  let width = context.measureText(columnContent);
  flexWidth = width.width + 8 + num;
  if (deeptitlelength) {
    if (deeptitlelength + num > flexWidth + num) {
      flexWidth = deeptitlelength + num;
    }
  }
  return flexWidth + num;
 }
 /**
 *
 * 官網注意：需要指定 column 的 width 属性，否则列头和内容可能不对齐。如果指定 width 不生效或出现白色垂直空隙，请尝试建议留一列不设宽度以适应弹性布局，或者检查是否有超长连续字段破坏布局。
 * @param prop 表格的列配置
 * @param tableData  表格數據
 * @param num 表格列的寬度
 * @param deeptitlelength 嵌套表頭的title長度
 * @returns
 */
 export function DynamicColumnWidth(
  prop: any[],
  tableData: any[],
  num: number = -10,
  tablewidth: any,
  deeptitlelength: number = 0,
 ) {
  prop.forEach((item) => {
    // 有children同時children不爲0
    if (item.children && item.children.length) {
      let temptitlewidth = countStrWidth(item.title);
      DynamicColumnWidth(item.children, tableData, num, tablewidth, temptitlewidth);
    } else {
      item['width'] = Math.round(
        Number(flexWidth(item.dataIndex, tableData, item.title, num, deeptitlelength)),
      );
      tablewidth.value = tablewidth.value + item['width'];
    }
  });
 }

/**
 * 判斷字符串是否是日期格式，支持yyyy-mm-dd || yyyy/mm/dd ， dd-mm-yyyy || dd/mm/yyyy， mm-dd-yyyy || mm/dd/yyyy
 * @param dateval 日期
 * @returns
 */
export function IsDate(dateval: string) {
  let arr = new Array();
  if (dateval.indexOf('-') != -1) {
    arr = dateval.toString().split('-');
  } else if (dateval.indexOf('/') != -1) {
    arr = dateval.toString().split('/');
  } else {
    return false;
  }

  //yyyy-mm-dd || yyyy/mm/dd
  if (arr[0] && arr[0].length == 4) {
    let date = new Date(arr[0], arr[1] - 1, arr[2]);
    if (date.getFullYear() == arr[0] && date.getMonth() == arr[1] - 1 && date.getDate() == arr[2]) {
      return true;
    }
  }
  //dd-mm-yyyy || dd/mm/yyyy
  if (arr[2] && arr[2].length == 4) {
    var date = new Date(arr[2], arr[1] - 1, arr[0]);
    if (date.getFullYear() == arr[2] && date.getMonth() == arr[1] - 1 && date.getDate() == arr[0]) {
      return true;
    }
  }
  //mm-dd-yyyy || mm/dd/yyyy
  if (arr[2] && arr[2].length == 4) {
    var date = new Date(arr[2], arr[0] - 1, arr[1]);
    if (date.getFullYear() == arr[2] && date.getMonth() == arr[0] - 1 && date.getDate() == arr[1]) {
      return true;
    }
  }

  return false;
}

/**
 *
 * @param title1 一级标题
 * @param title2 二级标题
 * @param title3 数据索引
 * @param left_position 表格内容左对齐
 * @param left_column 表格内容需要左对齐的列
 * @param right_position 表格内容右对齐
 * @param right_column 表格内容需要右对齐的列
 * @param Fixed 是否固定
 * @param Fixed_column 需要固定的栏位
 * @returns
 */
export function TableRendering(
  title1: any,
  title2: any,
  title3: any,
  Fixed = false,
  Fixed_column: any[] = [],
  left_position = false,
  right_position = false,
  left_column: any[] = [],
  right_column: any[] = [],
) {
  let temp_Table: Object[] = [];
  let position: string = 'center';
  let temp: number = 0;
  let children1: Object[] = [];
  let temp1 = false;
  let fixed: string = '';
  let classname: string = '';
  let width: any;
  title1.forEach((item, index) => {
    if (left_position) {
      for (var i = i + temp; i < left_column.length; i++) {
        if (index == left_column[i]) {
          temp = i;
          position = 'left';
        }
      }
    } else if (right_position) {
      for (var i = i + temp; i < right_column.length; i++) {
        if (index == right_column[i]) {
          temp = i;
          position = 'right';
        }
      }
    }
    if (Fixed) {
      Fixed_column.forEach((Fixed_temp) => {
        if (index == Fixed_temp) {
          (classname = 'v-center'), (fixed = 'left');
        }
      });
    }
    if (item == title2[index]) {
      temp_Table.push({
        title: item,
        dataIndex: title3[index],
        align: position,
        className: classname,
        fixed,
      });
      classname = '';
      fixed = '';
      position = '';
    } else {
      if (index < title1.length - 1) {
        if (item == title1[index + 1]) {
          children1.push({
            title: title2[index],
            dataIndex: title3[index],
            align: position,
            className: classname,
            fixed,
          });
          classname = '';
          fixed = '';
          position = '';
        } else {
          children1.push({
            title: title2[index],
            dataIndex: title3[index],
            align: position,
            className: classname,
            fixed,
          });
          temp_Table.push({
            title: item,
            children: children1,
          });
          children1 = [];
          classname = '';
          fixed = '';
          position = '';
        }
      } else if (index == title1.length) {
        children1.push({
          title: title2[index],
          dataIndex: title3[index],
          align: position,
          className: classname,
          fixed,
        });
        temp_Table.push({
          title: item,
          children: children1,
        });
        children1 = [];
        classname = '';
        fixed = '';
        position = '';
      }
    }
    // else{
    //   if(title1[index-1] == item){
    //     children1.push({
    //       title: title2[index-1],
    //       dataIndex: title2[index-1],
    //       align: position,
    //     });
    //     temp1 = true;
    //   }
    //   else if(title1[index-1] != title2[index-1]){
    //     children1.push({
    //       title: title2[index-1],
    //       dataIndex: title2[index-1],
    //       align: position,
    //     });
    //     temp_Table.push({
    //     title: title1[index-1],
    //     children: children1
    //     });
    //     temp1 = false;
    //   }
    //   if(index == title1.length-1){
    //     children1.push({
    //       title: title2[index],
    //       dataIndex: title2[index],
    //       align: position,
    //     });
    //     temp_Table.push({
    //     title: item,
    //     children: children1
    //     });
    //     temp1 = false;
    //   }
    // }
  });
  return temp_Table;
}
// 替代evil函數
export function evilFn(fn: string) {
  let Fn = Function;
  return new Fn(`return ${fn}`)();
}
/**
 * 取出小括号内的内容
 * @param text
 * @returns {string}
 */
export function getindex(text: string) {
  let re = /\([^\)]+\)/g;
  text = text.match(re)![0];
  text = text.substring(1, text.length - 1);
  return text;
}
// 合并數組對象，從上往下合并，與下一個做對比
/**
 *
 * @param {*} datasource 數據源
 * @param {*} stadand 根據相同屬性進行合并
 */
export function mergerobjList(dataSource: any[], standard: string[]) {
  if (dataSource.length == 0) {
    return [];
  } else {
    let temp = cloneDeep(dataSource[0]);
    let mergerlist: any[] = [];
    let regArr: any[] = [];
    for (let i = 0; i < dataSource.length - 1; i++) {
      // 相同項的屬性
      standard.forEach((item) => {
        regArr.push(dataSource[i][item] == dataSource[i + 1][item]);
      });
      if (regArr.includes(false)) {
        // 不相同push
        regArr = [];
        mergerlist.push(temp);
        temp = cloneDeep(dataSource[i + 1]);
        continue;
      } else {
        // 相同的話，就合并
        temp = { ...temp, ...dataSource[i + 1] };
      }
    }
    // push最後一個temp
    mergerlist.push(temp);
    return mergerlist;
  }
}
/**
 * 返回颜色样式，colorSet长度总是等于dataSet+1(也就是dataSet的区间数)
 * @param dataSet 數據設定值
 * @param colorSet 顔色值
 * @param record 當前值
 * @returns 樣式
 */
export function SetColumnsStyle(dataSet: number[], colorSet: string[], record: number) {
  dataSet.push(record);
  dataSet.sort((a, b) => a - b);
  let length = dataSet.indexOf(record);
  return { style: { color: colorSet[length] } };
}

/**
 * 获取数组对象的某一列
 * @param dataSource 表格数据源
 * @param key  要去除列的键
 * @param filtervalue 从数组中剔除值为xxx元素
 */
export function getObjArrayCol(dataSource: Object[], key: string, filtervalue: any[] = []) {
  let returnvalue = dataSource.map((v) => {
    return {
      label: v[key],
      value: v[key],
    };
  });
  if (filtervalue.length) {
    returnvalue = returnvalue.filter((item) => {
      return !filtervalue.includes(item.label);
    });
  }
  return returnvalue;
}

// 获取多级表头最后一个层级的dataindex和titile
/**
 *
 * @param Columns ant design vue表格列的配置
 * @param filtervalue 从数组中剔除值为xxx元素
 * @returns
 */
export function getLastColumns(Columns: PropType<ColumnsType<any>>, filtervalue: any[] = []) {
  let key: (string | number)[] = [];
  let title: (string | number)[] = [];
  function fillkey(Columns) {
    Columns.forEach((item) => {
      if (item.children) {
        fillkey(item.children);
      } else {
        key.push(item.dataIndex);
        title.push(item.title);
      }
    });
  }
  fillkey(Columns);
  let returnvalue = key.map((item, index) => {
    return {
      label: title[index],
      value: item,
    };
  });
  if (filtervalue.length) {
    returnvalue = returnvalue.filter((item) => {
      return !filtervalue.includes(item.label);
    });
  }
  return returnvalue;
}
