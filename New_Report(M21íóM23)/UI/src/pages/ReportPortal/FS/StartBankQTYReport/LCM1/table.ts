import type { TableColumn } from '@/components/core/dynamic-table';
import { SetColumnsStyle } from '@/pages/ReportPortal/utils/common';
import router from '@/router';
import { ReportCommonWithOut } from '@/store/module/ReportCommon';
import { message } from 'ant-design-vue';

export const StartBankTable1Columns: TableColumn[] = [
  {
    title: 'Work Order', dataIndex: 'WO_NBR',
    customCell: (record: Object, rowindex: number, column: Object) => {
      return {
        rowSpan: record['WO_NBR-span']['rowspan'],
        colSpan: record['WO_NBR-span']['colspan']
      }
    }
  },
  {
    title: 'LCM Product', dataIndex: 'PROD_NBR',
    customCell: (record: Object, rowindex: number, column: Object) => {
      return {
        rowSpan: record['PROD_NBR-span']['rowspan'],
        colSpan: record['PROD_NBR-span']['colspan']
      }
    }
  },
  {
    title: 'AOPS開線數', dataIndex: 'E_QTY',
    customCell: (record: Object, rowindex: number, column: Object) => {
      return {
        onDblclick: () => {
          if (record['E_QTY'] == 0) return message.warning('無數據')
          const UseParamsAdd = ReportCommonWithOut()
          let params = { ...record, ...column['__originColumn__'] }
          let FactoryType: any = UseParamsAdd.Fab;
          // let SubFab: any = UseParamsAdd.SubFab;
          UseParamsAdd.AddFirstTableParams(`StartBankQTYReport${FactoryType}`, params);
          router.push({ name: `StartBankQTYReportDetail${FactoryType}` });
          // UseParamsAdd.AddSecondTableParams({ ...record, ...column['__originColumn__'] })
        }, style: { cursor: 'pointer' }
      }
    }
  },
  // {
  //   title: 'W/O剩余投入量', dataIndex: 'NEED', customCell: (record: Object, rowindex: number, column: Object) => {
  //     let style = SetColumnsStyle([700,900,1000,2000],['black','green','grey','red','yellow'],record['NEED']);
  //     return style??{}
  //   }
  // },
  {
    title: 'W/O剩余投入量', dataIndex: 'NEED'},
  { title: 'Bank Target', dataIndex: 'BANKTARGET' },
  { title: 'Prod（PFCD/SKD）', dataIndex: 'LCD_PROD' },
  { title: 'ERP_PROD_ID', dataIndex: 'ERP_PROD_ID' },
  { title: 'LCD Grade', dataIndex: 'GRADE' },
  { title: '符合的panel數量', dataIndex: 'COUNT1' },
  {
    title: '可投此panel的其他幾種', dataIndex: 'OTHERPROD',
    resizable: true,
    width: 150,
    customCell: (record) => {
      return {
        style: {
          overflow: 'hidden',
          whiteSpace: 'nowrap',
          textOverflow: 'ellipsis',
          cursor: 'pointer'
        },
        // rowSpan: record['PROD_NBR-span']['rowspan'],
        // colSpan: record['PROD_NBR-span']['colspan']
      }
    }

  },
  { title: 'LCD_OWNER', dataIndex: 'LCD_OWNER' },
]
export const StartBankTable2Columns: TableColumn[] = [
  { title: 'LCD Product ID', dataIndex: 'LCD_PROD' },
  { title: 'LCD Grade', dataIndex: 'GRADE' },
  { title: 'panel數量', dataIndex: 'COUNT1' },
]

export const StartBankDetail3Table1Columns: TableColumn[] = [
  { title: '機種名稱', dataIndex: 'PROD_NBR' },
  { title: '版本', dataIndex: 'VER' },
  { title: '機臺', dataIndex: 'EQUIP_NBR' },
  { title: '班別', dataIndex: 'SHIFT_ID' },
  { title: '投產數', dataIndex: 'PASS_QTY' },
  { title: '良品數', dataIndex: 'GOOD_QTY' },
  { title: 'Rework', dataIndex: 'RWK_QTY' },
]
