
//import { TableCustomCell } from '@/pages/ReportPortal/utils/SHcommon';
import router from '@/router';
import { reactive } from 'vue';
import { AddCommonPageParamsWithOut } from '@/store/module/commonpage';
import { ReportCommonWithOut } from '@/store/module/ReportCommon';
//import { AddCommonPageParams} from '@/store/module/commonpage';
import { message } from 'ant-design-vue';

const useCommonParams = AddCommonPageParamsWithOut();
const useReportParams = ReportCommonWithOut();


//機種別,人員別
export let DownGradeLabel = ['累計檢驗數量', 'A', 'B', 'C', 'C2', 'C3', 'C4', 'C5', 'C6', 'C7', 'D', 'E','K','G', 'W', 'R', 'L', 'V',
     'Down Grade數量', 'Down Grade比率', '送修數量', '送修比率', '檢驗良品率', 'Down Grade']
     //let DownGradeWidth=[35,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,45,45,25,30,30,70]
     let DownGradeWidth=[]

let DownGradeProps =['TOTAL', 'A', 'B', 'C', 'C2', 'C3', 'C4', 'C5', 'C6', 'C7', 'D', 'E' ,'K','G','W','R', 'L', 'V',
     'DOWN', 'DOWN_GRADE_RATE', 'RWK', 'REPAIR_RATE', 'OK_RATE', 'DOWNGRADE']
    

//可點擊列
//let ClickProps = DownGradeLabel.filter(item => item != '累計檢驗數量')
let DownGradeClick =['A', 'B', 'C', 'C2', 'C3', 'C4', 'C5', 'C6', 'C7', 'D', 'E', 'K','G','W', 'R', 'L', 'V',
     'DOWN',  'DOWN_GRADE_RATE', 'RWK', 'REPAIR_RATE']     


let DownSecondLabel = [ 'Item', 'Defect', 'Explain', 'Grade', '數量']
let DownSecondProps = [ 'ITEM', 'DEFECT_CODE', 'ERRC_DESCR', 'LCM_GRADE', 'QTY']
let DownOperatorSecondClick =["QTY"]

let DownThridTableLabel = ["Panel Id",
    "Defect",
    "Level",
    "Explain",
    "Product",
    "LCM Owner",
    "LCM Grade",
    "Work Order",
    "Operator",
    "Trans Type",
    "Out Flag",
    "X Axis",
    "Y_Axis",
    "Equipment",
    "Pattern Code",
    ]

let DownThridTableProps=["PNL_ID","DFCT_CODE","LEVEL","ERRC_DESCR","PROD_NBR","LCM_OWNER",
"LCM_GRADE","WO_NBR","OPERATOR","TRANS_TYPE","OUTPUT_FG","X_AXIS","Y_AXIS","EQUIP_NBR","PATTERN_CODE"]
//let DownThridTableWidth=[60,30,30,100,60,50,30,35,45,30,40,35,30,30]
let DownThridTableWidth=[]


export const DownGradeColumns = TableCustomCell(DownGradeLabel, DownGradeProps, DownGradeClick,DownGradeWidth,false,'DownGradeDetail')

export const DownSecondColumns = TableCustomCell(DownSecondLabel,DownSecondProps,DownOperatorSecondClick)

export const DownThridColumns = TableCustomCell(DownThridTableLabel,DownThridTableProps,[],DownThridTableWidth)


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
                 
                //  if(column["dataIndex"] == "A")
                //  {
                //     if(record["A"] < 10)
                //     {
                //         let AA = "'background-color' : 'rgb(0,255,0)'"
                //         return {style: { 'background-color' : 'rgb(0,255,0)' }}
                //     }
                //     else if(record["A"] > 10 && record["A"] < 100)
                //     {
                //         return {style: { 'background-color' : 'rgb(255,255,0)' }}
                //     }
                //     else
                //     {
                //         return {style: { 'background-color' : 'rgb(255,150,150)' }}
                //     }
                //  }
                 
                 if (ClickColumn.includes(item)) {
                     return {
                         onDblclick: () => {                            
                             if (ToOtherpage) 
                             {
                                 //useCommonParams.AddFirstTableParams({ ...record, ...column['__originColumn__'] })
                                 useReportParams.AddFirstTableParams(`DownGradeDetail_${useCommonParams.Fab}`,{ ...record, ...column['__originColumn__'] })
                                 //useReportParams.AddObject(`DownGradeDetail_${useCommonParams.Fab}` , useCommonParams)
                                 router.push({ name: ToOtherpage+`_${useCommonParams.Fab}` });
                             }else{
                                  // 這裏判斷跳到三表格
                                  console.log("跳转表格3");
                                  if(record["ITEM"]=="小計"){
                                    message.warning('此栏位没有二阶数据')
                                    
                                     return;
                                  }  
                                  //useCommonParams.AddSecondTableParams({ ...record, ...column['__originColumn__'] })   
                                  useReportParams.AddSecondTableParams(`DownGradeDetail_${useCommonParams.Fab}`,{ ...record, ...column['__originColumn__'] })   
                                  //useReportParams.AddObject(`DownGradeDetail_${useCommonParams.Fab}` , useCommonParams)
                                  
                             
                             } 
                         }, 
                          style: { cursor: 'pointer',width:40}
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