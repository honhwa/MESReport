import type { TableColumn } from '@/components/core/dynamic-table';
import { AddOperationParamsWithOut } from '@/store/module/operationDeatil';
import { message } from 'ant-design-vue';
import router from '@/router';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { ReportCommon } from '@/store/module/ReportCommon';

// import { Avatar, Space, Tag } from 'ant-design-vue';
let useCommonParams = AddCommonPageParams();
let useReportCommon = ReportCommon();

//////////////////------首页上表格 work_CTR<2000--------------/////////////
let topTableLabels = ["Oper", "Pass Qty","To Rwk","Rwk/pass Rate%","To be MoveIn","WIP","WIP Hold","WIP Non Hold","In", "1'st yield%",  "Total yield%", "修理再發率%"]
let topTableProps = ["WORK_CTR", "PASS_QTY","TOTALRWK_QTY","RWKPass_Rate","TOBEMOVEIN_QTY", "WIP_QTY","HODE_QTY","NO_HODE_QTY","IN_QTY", "FirstYield", "TotalYield","RepairRate" ]
// 首页上面可点击列
let TopClick = ["PASS_QTY","TOTALRWK_QTY", "TOBEMOVEIN_QTY","WIP_QTY", "HODE_QTY"]
let TopWidth= [25,30,30,30,30,25,30,30,25,35,35,35]

// 首页上表格的pass qty，二階子页面 (三階頁面欄位自動生成的，不用定義)
let PassOperDetailLabel = ['機種名稱', '版本', '機台', '班别', '投產數', '良品數', 'Rework'];
let PassOperDetailProp = ['PROD_NBR', 'VER', 'EQUIP_NBR', 'SHIFT_ID', 'PASS_QTY', 'GOOD_PASS', 'RWK_QTY'];
let PassQtyClickColumn = ['PASS_QTY','GOOD_PASS', 'RWK_QTY']
let PassOperDetailWidth= [35,15,35,15,25,25,25]

///首页上表格的to rwk，二階子页面
let ToReworkLabel = ['Code', 'Description', "Qty"]
let ToReworkProp = ['CODE', 'CODE_DESC', 'COUNT']
////WipClickColumn = ['COUNT']

//首页上表格的to rwk，三階子页面
let ToReworkDetailLabel = ['prod_nbr',
  'entity_no',
  'trans_date',
  'trans_type',
  'eqp_nbr',
  'operators',
  'output_fg',
  'work_ctr',
  'to_wc',
  'defect_code',
  'rework次数',
  'dfct次数',
  '合计片数']
let ReworkDetailProp = ["PROD_NBR", "ENTITY_NO", "TRANS_DATE", "TRANS_TYPE", "EQUIP_NBR","OPERATOR","OUTPUT_FG", "WORK_CTR","TO_WC", "DFCT_CODE", "STIMES_TTL", "ITEM_NBR","PASS_QTY"]
let ReworkDetailWidth= [35,35,40,20,25,25,20,16,16,22,20,16,20]


///首页上表格的ToBeMoveIn，WIP 二階子页面
let ToBeMoveInLabel = ['product', 'Reversion', 'Cart_Flag','Qty','Hold Qty'];
let ToBeMoveInProp = ['PROD_NBR', 'VER','CART_FLAG','COUNT','HOLDQTY'];
let ToBeMoveInClickColumn = ['COUNT'];

let WipQtyLabel = ['product', 'Reversion','Cart_Flag', 'Qty'];
let WipQtyProp = ['PROD_NBR', 'VER', 'CART_FLAG','COUNT'];
let WipClickColumn = ['COUNT'];

///首页上表格的WIP 三階子页面
let AllWipLabel = ["prod_nbr",  "last_trans_dt", "pnl_id", "cart_id","CART_ID_CUST","last_eqp_nbr", "work_ctr",  "rework次数", "Hold_Code","是否模組","是否快修","合计片数"];
let AllWipLabelProp = ["PROD_NBR",  "LAST_TRANS_DT", "PNL_ID", "CART_ID","CART_ID_CUST","LAST_EQP_NBR", "OPER",  "RW_COUNT", "CUR_HOLD_CODE","RESERVED_21","FAST_RWK_FLG","QTY"];
let AllWipLabelWidth= [25,25,22,18,18,18,18,12,10,15,10,10,10]

///首页上表格的ToBeMoveIn 三階子页面
let AllToBeMoveInLabel = ["prod_nbr",  "last_trans_dt", "pnl_id", "cart_id","last_eqp_nbr", "work_ctr",  "rework次数","是否模組", "Hold_Code","Hold_Flag","合计片数"];
let AllToBeMoveInProp = ["PROD_NBR",  "LAST_TRANS_DT", "PNL_ID","CART_ID", "LAST_EQP_NBR", "WORK_CTR",  "STIMES_TTL", "RESERVED_21","HOLD_CODE","HOLD_FLAG","QTY"];
let AllToBeMoveInWidth= [30,35,30,20,22,20,16,20,20,22,20]

//////////////////------首页下表格 work_CTR>=2000--------------/////////////
let bottomTableLabels = ["Oper", "In", "Rework Out", "Rework Out(1)以上","Rework(1)以上%","To be scrap","WIP", "WIP Hold", "WIP Non Hold"]
let bottomTableProps = ["WORK_CTR", "IN_QTY", "PASS_QTY", "PASS2_QTY", "PASS2_QTYRate","TOBESCRAP_QTY","WIP_QTY","HODE_QTY", "NO_HODE_QTY"]
// 首页下面可点击列
let BottomClick = ["IN_QTY", "PASS_QTY", "PASS2_QTY","WIP_QTY","TOBESCRAP_QTY","HODE_QTY"]
let BottomWidth= [25,30,30,42,35,30,30,35,35]

///首页下表格in，rework， 二階子页面
let RwkIn2000Label = ['Operation', '數量'];
let RwkIn2000Prop = ['WORK_CTR', 'QTY'];
let RwkIn2000ClickColumn = ['QTY'];

///首页下表格in，rework， 三階子页面
let Rwkin2000DetailLabel = ['prod_nbr','acct_date','pnl_id',
'trans_type','trans_nbr','equip_nbr',
'operators','output_fg', 'work_ctr', 
'to_wc', 'rework次数',"是否模組",
'serial_no', 'rma_nbr','合计片数','Comments' ]
let Rwkin2000DetailProp = ["PROD_NBR", "ACCT_DATE","PNL_ID", 
"TRANS_TYPE","TRANS_NBR", "EQUIP_NBR", 
"OPERATORS","OUTPUT_FG","OPER",
"TO_OPER",  "STIMES_TTL", "RESERVED_21",
"SERIAL_NO","RMA_NBR","QTY","COMMENTS"]
let Rwkin2000DetailWidth= [30,20,30,20,15,20,20,18,15,15,20,20,35,20,20,25]

///首页下表格 ToBeScrap 二階子页面 (用to rwk，二階子页面欄位,三階自動產生)


export { topTableProps, bottomTableProps };
////------首頁上下表格----------////
export const topBaseColumns: TableColumn[] = TableCustomCell(topTableLabels, topTableProps, TopClick, true,TopWidth)
export const bottomBaseColumns: TableColumn[] = TableCustomCell(bottomTableLabels, bottomTableProps, BottomClick, true,BottomWidth)

///------首頁上表格detail ------------/////
///pass
export const PassOperDetailColumns: TableColumn[] = TableCustomCell(PassOperDetailLabel, PassOperDetailProp, PassQtyClickColumn,false,PassOperDetailWidth)
///to rwk
export const ToReworkColumns: TableColumn[] = TableCustomCell(ToReworkLabel, ToReworkProp, WipClickColumn)
export let ToReworkDetailColumns: TableColumn[] = TableCustomCell(ToReworkDetailLabel,ReworkDetailProp,[],false,ReworkDetailWidth)
///ToBeMoveIn
export const ToBeMoveInColumns: TableColumn[] = TableCustomCell(ToBeMoveInLabel, ToBeMoveInProp, ToBeMoveInClickColumn)
export const AllToBeMoveInColumns: TableColumn[] = TableCustomCell(AllToBeMoveInLabel, AllToBeMoveInProp,[],false,AllToBeMoveInWidth)
///WIP,HOLD
export const WipColumns: TableColumn[] = TableCustomCell(WipQtyLabel, WipQtyProp, WipClickColumn)
export const AllWipColumns: TableColumn[] = TableCustomCell(AllWipLabel, AllWipLabelProp,[],false,AllWipLabelWidth)

///------首頁下表格detail ------------/////
/// rework in，rework Out，rework Out(1)以上
export const RwkIn2000Columns: TableColumn[] = TableCustomCell(RwkIn2000Label, RwkIn2000Prop, RwkIn2000ClickColumn)
export let Rwkin2000DetailCoulmns: TableColumn[] = TableCustomCell(Rwkin2000DetailLabel, Rwkin2000DetailProp,[],false,Rwkin2000DetailWidth)




function TableCustomCell(Label: string[], Prop: string[], ClickColumn: string[] = [], ToOtherpage: Boolean = false,Width:number[]=[]) {
  let colcss="";
  return Prop.map((item: string, index: number) => {  
    if(index==0){
      colcss="v-center"
    }else{
      colcss=""
    }
    return {           
      title: ClickColumn.includes(item) ? `${Label[index]}*` : Label[index],//可点击的栏位显示*号
      align: 'center' as any,
      dataIndex: item,
      className: colcss,
      width: Width[index],
      col:index,
      //@ts-ignore
      customCell: (record: Object, rowindex: number, column: Object) => {
        
        if (ClickColumn.includes(item)) {  
          return {           
            onDblclick: () => {             
              const UseParamsAdd = AddOperationParamsWithOut()             
              
              // UseParamsAdd.AddSecondTableParams({ ...record, ...column })
              // 是否跳轉到二級頁面
              if (ToOtherpage) {
                console.log("跳转到第二层");
                useReportCommon.AddFirstTableParams(`OperationPerformanceNew-${useCommonParams.Fab}`,{ ...record, ...column['__originColumn__'] });
                         
                
                if (useCommonParams.Fab == 'LCM1' || useCommonParams.Fab == 'LCM2') {
                  router.push({ name: `OperationPerformanceNewDetail-MOD1` });
                }
                if (useCommonParams.Fab == 'LCM3' || useCommonParams.Fab == 'LCM4') {
                  router.push({ name: `OperationPerformanceNewDetail-MOD2` });
                }
              } else {
                console.log("輸出ClickColumn",ClickColumn);
                console.log("輸出record",record);
                if (UseParamsAdd.FirstTableParams["dataIndex"] == "IN_QTY" && ClickColumn.includes('QTY') && ["總計", "合計"].includes(record["OPER"])) {
                
                  // 跳轉到表格4
                  console.log("表格4");                       
                  //useReportCommon.AddThirdTableParams(`OperationPerformance-${useCommonParams.Fab}`,{ ...UseParamsAdd.ThirdTableClickParams[rowindex - 1], ...column['__originColumn__'] })
                } else {
                  // 這裏判斷跳到三表格
                  console.log("跳转表格3");                 
                  useReportCommon.AddSecondTableParams(`OperationPerformanceNew-${useCommonParams.Fab}`,{ ...record, ...column['__originColumn__'] })   
                  //console.log({ ...record, ...column['__originColumn__'] }.values());  
                  //console.log({ ...record, ...column['__originColumn__'] }.keys());    
                           
                }

              }
            }, style: { cursor: 'pointer'}
          }
        } else {
          return {
            onDblclick: () => {
              message.warning('此栏位没有二阶数据')
            }, style: { }
          }
        }

      }
    }
  })

}
let OperationColumsData = { PassOperDetailColumns, WipColumns, ToReworkColumns,AllToBeMoveInColumns,ToBeMoveInColumns,AllWipColumns,RwkIn2000Columns,Rwkin2000DetailCoulmns}
export default OperationColumsData