
//import { TableCustomCell } from '@/pages/ReportPortal/utils/NGBcommon';
import router from '@/router';
import { reactive } from 'vue';
import { AddCommonPageParamsWithOut } from '@/store/module/commonpage';
//import { AddCommonPageParams} from '@/store/module/commonpage';
import { message } from 'ant-design-vue';
import { ReportCommon } from '@/store/module/ReportCommon';

const useCommonParams = AddCommonPageParamsWithOut();
let useReportCommon = ReportCommon();

// let ReportItem = reactive({
//     // 首頁後端返回的sql以及sql參數
//         APIMessage:'',
//         // 左側請求參數
//         FormParams: {},
//         // 二級頁面所需參數
//         FirstTableParams: {},
//         // 其餘頁面所需參數
//         SecondTableParams:reactive({}),
//         ThirdTableParams:{},
//         ThirdTableClickParams:{},
//         //廠別
//         Fab:'',
//         G_FAC_COM:'',
// });

 //useReportCommon.AddObject(`DownGrade-${useCommonParams.Fab}`,ReportItem);


//機種別,人員別
export let DownGradeLabel = ['累計檢驗數量', 'A', 'B', 'C', 'C2', 'C3', 'C4', 'C5', 'C6', 'D', 'E', 'W','K','G','L', 'S',
     'Down Grade數量', 'Down Grade比率', '送修數量', '送修比率', '檢驗良品率', 'Down Grade']
     let DownGradeWidth=[35,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,45,45,30,30,40,70]

let DownGradeProps =['TOTAL', 'A', 'B', 'C', 'C2', 'C3', 'C4', 'C5', 'C6', 'D', 'E', 'W','K','G','L', 'S',
     'DOWN', 'DOWN_GRADE_RATE', 'RWK', 'REPAIR_RATE', 'OK_RATE', 'DOWNGRADE']
    

     //Defect別
export let DownGradeDfctLabel = ['Dfct Desc','累計檢驗數量', 'A', 'B', 'C', 'C2', 'C3', 'C4', 'C5', 'C6', 'D', 'E', 'W','K','G','L',
'Down Grade數量', 'Down Grade比率', '送修數量', '送修比率', '檢驗良品率', 'Down Grade']
let DownGradeDfctWidth=[60,35,20,20,20,20,20,20,20,20,20,20,20,20,20,20,45,45,30,35,35,70]

let DownGradeDfctProps =['DFCT_DESC','TOTAL', 'A', 'B', 'C', 'C2', 'C3', 'C4', 'C5', 'C6', 'D', 'E', 'W','K','G','L',
     'DOWN', 'DOWN_GRADE_RATE', 'RWK', 'REPAIR_RATE', 'OK_RATE', 'DOWNGRADE']


//可點擊列
//let ClickProps = DownGradeLabel.filter(item => item != '累計檢驗數量')
let DownGradeClick =['TOTAL', 'A', 'B', 'C', 'C2', 'C3', 'C4', 'C5', 'C6', 'D', 'E', 'W','K','G','L', 'S',
     'DOWN',  'RWK',  'DOWNGRADE']

let DownGradeDfctClick =['TOTAL', 'A', 'B', 'C', 'C2', 'C3', 'C4', 'C5', 'C6', 'D', 'E', 'W','K','G','L',
     'DOWN',  'RWK',  'DOWNGRADE']

let DownOperatorSecondLabel = [ 'LCM Grade', 'Defect', 'Down Grade', 'Operator',"Qty"]
let DownOperatorSecondProps = [ 'LCM_GRADE', 'DFCT_CODE', "DOWN_GRADE","OPERATORS","QTY"]
let DownOperatorSecondClick =["QTY"]

let DownSecondLabel = [ 'LCM Grade', 'Defect', "Qty"]
let DownSecondProps = [ 'LCM_GRADE', 'DFCT_CODE', "QTY"]

let DownSecond2Label = [ 'Item','LCM Grade', 'Defect', "Qty"]
let DownSecond2Props = [ 'ITEM','LCM_GRADE', 'DEFECT_CODE', "QTY"]

let DownThridTableLabel = ["Panel Id",
    "Defect",
    "LCM Grade",
    "Trans Date",
    "Work Order",
    "Operator",
    "Oper",
    "To_oper",
    "Trans_type",
    "Lcd Grade",
    "Lcd Bright_flag",
    "Lcd fab",
    "X_AXIS",
    "Y_AXIS",
    ]

let DownThridTableProps=["PNL_ID","DFCT_CODE","LCM_GRADE","TRANS_DATE","WO_NBR","OPERATORS",
"OPER","TO_OPER","TRANS_TYPE","LCD_GRADE","LCD_BRIGHT_FLAG","LCD_FAB","X_AXIS","Y_AXIS"]
let DownThridTableWidth=[60,30,30,100,60,50,30,35,45,30,40,35,30,30]

let DownThridTable2Label = ["Panel Id",
    "Defect",
    "Product",
    "LCM Owner",
    "LCM Grade",
    "Work Order",
    "Operator",
    "Trans_type",
    "Output_fg",
    "Lcd Grade",
    "Lcd fab",
    ]

let DownThridTable2Props=["PNL_ID","DFCT_CODE","PROD_NBR","LCM_OWNER","LCM_GRADE","WO_NBR","OPERATOR","TRANS_TYPE",
"OUTPUT_FG","LCD_GRADE"]
let DownThridTable2Width=[60,40,60,40,40,60,50,40,40,40,]

export const DownGradeColumns = TableCustomCell(DownGradeLabel, DownGradeProps, DownGradeClick,DownGradeWidth,false,'DownGradeDetail')
export const DownGradeDfctColumns = TableCustomCell(DownGradeDfctLabel, DownGradeDfctProps, DownGradeDfctClick,DownGradeDfctWidth,false,'DownGradeDetail')

export const DownOperatorSecondColumns = TableCustomCell(DownOperatorSecondLabel,DownOperatorSecondProps,DownOperatorSecondClick)

export const DownSecondColumns = TableCustomCell(DownSecondLabel,DownSecondProps,DownOperatorSecondClick)

export const DownSecond2Columns = TableCustomCell(DownSecond2Label,DownSecond2Props,DownOperatorSecondClick)

export const DownThridColumns = TableCustomCell(DownThridTableLabel,DownThridTableProps,[],DownThridTableWidth)

export const DownThrid2Columns = TableCustomCell(DownThridTable2Label,DownThridTable2Props,[],DownThridTable2Width)


function TableCustomCell(Label: string[], Prop: string[], ClickColumn: string[] = [],Width:number[]=[],isMergeColumns: boolean = false, ToOtherpage?: string) {
   
     return Prop.map((item: string, index: number) => {
         return {
             title: ClickColumn.includes(item) ? `${Label[index]}*` : Label[index],//可点击的栏位显示*号
             align: 'center' as any,
             dataIndex: item,
             col:index,
             width: Width[index],
             // className: 'v-center',
             //@ts-ignore
             customCell: (record: Object, rowindex: number, column: Object) => {
                 if (ClickColumn.includes(item)) {
                     return {
                         onDblclick: () => {                            
                             if (ToOtherpage) 
                             {
                                 //useCommonParams.AddFirstTableParams({ ...record, ...column['__originColumn__'] })
                                 useReportCommon.AddFirstTableParams(`DownGrade-${useCommonParams.Fab}`,{ ...record, ...column['__originColumn__'] })
           
                                 router.push({ name: ToOtherpage+`-${useCommonParams.Fab}` });
                             }else{
                                  // 這裏判斷跳到三表格
                                  console.log("跳转表格3");
                                  if(record["ITEM"]=="小計"||record["ITEM"]=="合計"){
                                    message.warning('此栏位没有二阶数据')
                                     return;
                                  }  
                                  //useCommonParams.AddSecondTableParams({ ...record, ...column['__originColumn__'] })   
                                  useReportCommon.AddSecondTableParams(`DownGrade-${useCommonParams.Fab}`,{ ...record, ...column['__originColumn__'] })
                             }
                         }, style: { cursor: 'pointer',width:40 }
                     }
                 }
                 else if (isMergeColumns) {
                     return {
                         rowSpan: record?.[`${item}-span`]?.['rowspan'] || 0,
                         colSpan:record?.[`${item}-span`]?.['colspan'] || 0,
                          style: { width:40 }
                     }
                 }
                 else {
                     return {
                          style: { width:40 },
                         onDblclick: () => {
                             message.warning('此栏位没有二阶数据')
                         }
                     }
                 }
 
             }
         }
     })
 
 }

// function TableCustomCellWidth(Label: string[], Prop: string[], ClickColumn: string[] = [], AddPercent: string[], ToOtherpage?: string) {
//     return Prop.map((item: string, index: number) => {
//         return {
//             title: ClickColumn.includes(item) ? `${Label[index]}*` : Label[index],
//             align: 'center' as any,
//             dataIndex: item,
//             customRender: ({ text }) => {
//                 if (AddPercent.includes(item)) {
//                     return (<span>
//                         {text}%
//                     </span>)
//                 }
//             },
//             //@ts-ignore
//             customCell: (record: Object, rowindex: number, column: Object) => {
//                 if (ClickColumn.includes(item)) {
//                     return {
//                         ondblclick: () => {
//                             const UseParamsAdd = AddCommonPageParamsWithOut()
//                             if (AddPercent.length > 0) UseParamsAdd.AddFirstTableParams({ ...record, ...column['__originColumn__'] })
//                             else UseParamsAdd.AddSecondTableParams({ ...record, ...column['__originColumn__'] })
//                             if (ToOtherpage) router.push({ name: ToOtherpage })

//                         }, style: { cursor: 'pointer' }
//                     }
//                 }


//             }
//         }
//     })

// }

// function GetProdColumns(Label: string[], Prop: string[]) {
//     return Prop.map((item: string, index: number) => {
//         return {
//             title: Label[index],
//             align: 'center' as any,
//             dataIndex: item,
//             //@ts-ignore
//             customCell: (record: Object, rowindex: number, column: Object) => {
//                 if (item == 'Product') {
//                     return {
//                         rowSpan: record?.[`${item}-span`]?.['rowspan'],
//                         colSpan: record?.[`${item}-span`]?.['colspan']
//                     }
//                 }

//             }
//         }
//     })

// }