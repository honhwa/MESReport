// import type { TableColumn } from '@/components/core/dynamic-table';
import router from '@/router';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { ReportCommon } from '@/store/module/ReportCommon';
import { reactive } from 'vue';

// import { Avatar, Space, Tag } from 'ant-design-vue';
//import { AddCommonPageParamsWithOut } from '@/store/module/commonpage';
const useCommonParams = AddCommonPageParams();
let useReportCommon = ReportCommon();

let ReportItem = reactive({
    // 首頁後端返回的sql以及sql參數
        APIMessage:'',
        // 左側請求參數
        FormParams: {},
        // 二級頁面所需參數
        FirstTableParams: {},
        // 其餘頁面所需參數
        SecondTableParams:reactive({}),
        ThirdTableParams:{},
        ThirdTableClickParams:{},
        //廠別
        Fab:'',
        G_FAC_COM:'',
});

 useReportCommon.AddObject(`PanelWipStayNew-${useCommonParams.Fab}`,ReportItem);

export const PanelWipLabel = [ '0~1h', '1~2h', '2~3h', '3~4h', '4~8h', '8~12h', '12~24h', '24~36h', '36~48h', '2~3d','3~4d','4~5d','>5d','Total','>7d'];
export const PanelWipProps = ['QTY1', 'QTY2', 'QTY3', 'QTY4', 'QTY5', 'QTY6', 'QTY7', 'QTY8', 'QTY9', 'QTY10', 'QTY11', 'QTY12', 'QTY13','TOTAL_QTY','QTY15'
];
let PanelWipWidth= [20,20,20,20,20,20,20,20,20,20,20,20,20,20,20]

export const PanelWipStayNewDetailLabel = [
  'Prod_ID',
  'VER',
  'Panel ID',
  'Owner',
  'Oper',
  'Hold code',
  'WO_Nbr',
  'WO_Cust',
  'Last_trans_dt',
  'Commnet',
  'Rw_count',
]
const PanelWipStayNewDetailProps = [
  'PROD_NBR',
  'VER',
  'PNL_ID',
  'LCM_OWNER',
  'OPER',
  'HOLD_CODE',
  'WO_NBR', 
  'WO_CUST',
  'LAST_TRANS_DT',
  'COMMENT',
  'STIMES_TTL',
]

let PanelWipStayNewDetailWidth= [35,10,30,15,15,20,28,20,50,20,20]


function TableCustomCell(Label: string[], Prop: string[], ClickColumn: string[] = [],Width:number[]=[]) {
 
  return Prop.map((item: string, index: number) => {
    return {
      title: ClickColumn.includes(item) ? `${Label[index]}*` : Label[index],//可点击的栏位显示*号
      align: 'center' as any,
      dataIndex: item,
      width: Width[index],
      //@ts-ignore
      customCell: (record: Object, rowindex: number, column: Object) => {
        return {
          onDblclick: () => {
            useReportCommon.AddFirstTableParams(`PanelWipStayNew-${useCommonParams.Fab}`,{ ...record, ...column['__originColumn__'] }) 
            if (useCommonParams.Fab=='LCM1' ){
            if (item != 'WORK_CTR') {
              router.push({ name: `PanelWipStayNewDetail-MOD1` });
            }
          }
          else  if (useCommonParams.Fab=='LCM2' ){
            if (item != 'WORK_CTR') {
              router.push({ name: `PanelWipStayNewDetail-MOD1` });
            }
          }
          else  if (useCommonParams.Fab=='LCM3' ){
            if (item != 'WORK_CTR') {
              router.push({ name: `PanelWipStayNewDetail-MOD2` });
            }
          }
          else  if (useCommonParams.Fab=='LCM4' ){
            if (item != 'WORK_CTR') {
              router.push({ name: `PanelWipStayNewDetail-MOD2` });
            }
          }
          }, style: {
            cursor: ClickColumn.includes(item) ? 'pointer' : '',
          }
        }
      },
      className:['PROD_NBR','WORK_CTR'].includes(item)?'v-center':''
    }
  })

}
export const PanelWipColumns = TableCustomCell(PanelWipLabel, PanelWipProps, PanelWipProps,PanelWipWidth)
export const PanelWipDetailColumns = TableCustomCell(PanelWipStayNewDetailLabel, PanelWipStayNewDetailProps, [],PanelWipStayNewDetailWidth)
