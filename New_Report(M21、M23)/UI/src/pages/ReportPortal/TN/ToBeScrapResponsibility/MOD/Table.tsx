import router from '@/router';
import { ReportCommon } from '@/store/module/ReportCommon';
const UseParamsAdd = ReportCommon();

export const aheadColumns = [
  {
    title: '責任歸屬',
    align: 'center',
    dataIndex: 'RESP_OWNER',
    className: 'pro_auto-center',
    width: 120,
    customCell: (record) => {
      let text = record['RESP_OWNER'];
      let obj = {
        text,
        style: {},
      };
      obj['rowSpan'] = record?.['RESP_OWNER-span']?.['rowspan'] || 0;
      return obj;
    },
  },
  {
    title: 'Scrap Code',
    align: 'center',
    dataIndex: 'SCRAP_CODE',
    className: 'pro_auto-center',
    width: 70,
  },
  {
    title: 'desc',
    align: 'center',
    dataIndex: 'C_DESC',
    className: 'pro_auto-center',
    width: 120,
  },
];
export const rearColumns = [
  {
    title: '總計',
    align: 'center' as any,
    dataIndex: 'Total',
    colSpan: 2,
    children: [
      {
        title: '總計',
        align: 'center',
        dataIndex: 'Total',
        width: 50,
        customCell: (record: Object, rowIndex: number, column: Object) => {
          let rs: any = {};
          if (record['RESP_OWNER'] != 'Shipping Qty' && record['RESP_OWNER'] != 'Total Yield') {
            if (record[column['dataIndex']]) {
              rs.style = {
                cursor: 'pointer',
              };
              rs.onDblclick = () => {
                UseParamsAdd.AddFirstTableParams('ToBeScrapResponsibility' + UseParamsAdd.Fab, {
                  ...record,
                  ...column['__originColumn__'],
                });
                router.push({ name: 'ToBeScrapResponsibilityDetail_' + UseParamsAdd.Fab });
              };
            }
          }
          return rs;
        },
      },
      {
        title: '不良率',
        align: 'center',
        dataIndex: 'Total-DFCT_CODE',
        width: 50,
        customCell: (record: Object, rowIndex: number, column: Object) => {
          let rs: any = {};
          if (record['RESP_OWNER'] != 'Shipping Qty' && record['RESP_OWNER'] != 'Total Yield') {
            if (record[column['dataIndex']]) {
              rs.style = {
                cursor: 'pointer',
              };
              rs.onDblclick = () => {
                UseParamsAdd.AddFirstTableParams('ToBeScrapResponsibility' + UseParamsAdd.Fab, {
                  ...record,
                  ...column['__originColumn__'],
                });
                router.push({ name: 'ToBeScrapResponsibilityDetail_' + UseParamsAdd.Fab });
              };
            }
          }
          return rs;
        },
      },
    ],
  },
];

export const getColumns = (Prop: string[]) => {
  return Prop.map((prop) => {
    return {
      title: prop,
      align: 'center' as any,
      dataIndex: prop,
      colSpan: 2,
      children: [
        {
          title: '數量',
          align: 'center',
          width: 50,
          dataIndex: `${prop}`,
          customCell: (record: Object, rowIndex: number, column: Object) => {
            let rs: any = {};
            if (record['RESP_OWNER'] != 'Shipping Qty' && record['RESP_OWNER'] != 'Total Yield') {
              if (record[column['dataIndex']]) {
                rs.style = {
                  cursor: 'pointer',
                };
                rs.onDblclick = () => {
                  UseParamsAdd.AddFirstTableParams('ToBeScrapResponsibility' + UseParamsAdd.Fab, {
                    ...record,
                    ...column['__originColumn__'],
                  });
                  router.push({ name: 'ToBeScrapResponsibilityDetail_' + UseParamsAdd.Fab });
                };
              }
            }
            return rs;
          },
        },
        {
          title: '%',
          align: 'center',
          width: 50,
          dataIndex: prop + '-DFCT_CODE',
          customCell: (record: Object, rowIndex: number, column: Object) => {
            let rs: any = {};
            if (record['RESP_OWNER'] != 'Shipping Qty' && record['RESP_OWNER'] != 'Total Yield') {
              if (record[column['dataIndex']]) {
                rs.style = {
                  cursor: 'pointer',
                };
                rs.onDblclick = () => {
                  UseParamsAdd.AddFirstTableParams('ToBeScrapResponsibility' + UseParamsAdd.Fab, {
                    ...record,
                    ...column['__originColumn__'],
                  });
                  router.push({ name: 'ToBeScrapResponsibilityDetail_' + UseParamsAdd.Fab });
                };
              }
            }
            return rs;
          },
        },
      ],
    };
  });
};

const detailColumnsProp = [
  'ENTITY_NO',
  'LCD_PROD_NBR',
  'PROD_NBR',
  'MAIN_WC',
  'RESP_OPER',
  'DFCT_CODE',
  'LEVEL',
  'ERRC_DESCR',
  'RESP_OWNER',
  'ACCT_DATE',
  'SCRAP_CODE',
  'C_DESC',
  'SFG_STATUS',
  'OPERATOR',
  'WO_NBR',
  'SCRAP_NBR',
  'MRWK_COMMENT',
  'MRWK_DATE',
  'P_COUNT'
];
const detailColumnsLabel = [
  'Panel Id',
  'Lcd Product',
  'Lcm Product',
  '送修站點',
  '報廢責任站點',
  'Defect Code',
  'LEVEL',
  'Defect',
  '責任歸屬',
  '報廢日期',
  '報廢代碼',
  '報廢原因',
  '報廢狀態',
  '報廢人員',
  '工單編號',
  '報廢單號',
  'MRWK COMMENT',
  'MRWK 時間',
  '合計片數'
];
const colWidth = [120, 120, 120, 60, 90, 120, 60, 120, 60, 110, 60, 120, 60, 90, 120, 120, 120, 120, 120];
export const detailColumns = detailColumnsProp.map((prop, idx) => {
  return {
    title: detailColumnsLabel[idx],
    align: 'center',
    dataIndex: prop,
    className: 'pro_auto-center',
    width: colWidth[idx],
  };
});
