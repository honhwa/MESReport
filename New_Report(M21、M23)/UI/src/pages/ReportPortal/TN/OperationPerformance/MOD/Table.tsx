import type { TableColumn } from '@/components/core/dynamic-table';
import { AddOperationParamsWithOut } from '@/store/module/operationDeatil';
import { ReportCommonWithOut } from '@/store/module/ReportCommon';
import { message } from 'ant-design-vue';
import router from '@/router';
import { AddCommonPageParams } from '@/store/module/commonpage';
// import { Avatar, Space, Tag } from 'ant-design-vue';
let topTableLabels = ["Operation", "Pass Q'ty", "To Rwk", "Rwk/Pass Rate", "Rwk Out", "To Be Scrap", "WIP", "WIP Hold", "WIP Non-Hold", "IN", "1'st yield", "Total yield"]
let topTableProps = ["WORK_CTR", "PASS_QTY", "TOTALRWK_QTY", "RWKRATIO", "RWKOUT_QTY", "TOBESCRAP_QTY", "WIP_QTY", "HOLD_QTY", "NOHOLD_QTY", "IN_QTY", "YIELD1", "TOTAL_YIELD"]
// 首頁上面可點擊列
let TopClick = ["PASS_QTY", "TOTALRWK_QTY", "RWKOUT_QTY", "WIP_QTY", "HOLD_QTY", "NOHOLD_QTY", "IN_QTY"]


let bottomTableLabels = ["Operation", "In", "Rework", "Rework(1)以上", "Rate", "To Be Scrap", "WIP", "WIP Hold", "WIP Non-Hold"]
let bottomTableProps = ["WORK_CTR", "RWK_IN", "PASS_QTY", "PASS2_QTY", "RWK_UP_ONE_RATE", "TOBESCRAP_QTY", "WIP_QTY", "HOLD_QTY", "NOHOLD_QTY"]
// 首頁下面可點擊列
let BottomClick = ["RWK_IN", "PASS_QTY", "PASS2_QTY", "TOBESCRAP_QTY", "WIP_QTY", "HOLD_QTY", "NOHOLD_QTY"]


// Top Pass Q'ty，一階頁面
let OperDetail3Label = ['機種名稱', '版本', '機台', '班别', '投產數', '良品數', 'Rework'];
let OperDetail3Prop = ['PROD_NBR', 'VER', 'EQUIP_NBR', 'SHIFT_ID', 'PASS_QTY', 'GOOD_PASS', 'RWK_QTY'];
let PassQtyClickColumn = ['PASS_QTY']

// Top Pass Q'ty，二階頁面
let PassQty2ndLabel = ['Prod Nbr', 'Entity No', 'Trans Date', 'Trans Type', 'Equip Nbr', 'Operator', 'Work Ctr', 'Output FG', 'To WC', 'Defect Code', 'Comment', 'Rework 次数', 'Dfct 次數', '合計片数']; 
let PassQty2ndProp = ['PROD_NBR', 'ENTITY_NO', 'TRANS_DATE', 'TRANS_TYPE', 'EQUIP_NBR', 'OPERATOR', 'MAIN_WC', 'OUTPUT_FG', 'TO_WC', 'DEFECT_CODE', 'COMMENT1', 'STIMES_TTL', 'ITEM_NBR', 'PASS_QTY'];

// Top To Rwk / Bottom To Be Scrap，一階頁面
let OperDetail4Label = ['Defect Code', 'Description', 'Qty'];
let OperDetail4Prop = ['CODE', 'CODE_DESC', 'QTY'];
let RwkQtyClickColumn = ['QTY']

// Top Rwk，二階頁面
let ToRwk2ndLabel = ['Prod Nbr', 'Entity No', 'Trans Date', 'Trans Type', 'Equip Nbr', 'Operator', 'Output FG', 'Work Ctr', 'To WC', 'Defect Code', 'LEVEL', 'WO Nbr',  'Rework 次数', 'Dfct 次數', '合計片数']; 
let ToRwk2ndProp = ['PROD_NBR', 'ENTITY_NO', 'TRANS_DATE', 'TRANS_TYPE', 'EQUIP_NBR', 'OPERATOR', 'OUTPUT_FG', 'OPER', 'TO_WC', 'DFCT_CODE', 'LEVEL', 'WO_NBR', 'STIMES_TTL', 'ITEM_NBR', 'PASS_QTY'];

// Bottom To Be Scrap，二階頁面
let Scrap2ndLabel = ['Prod Nbr', 'Entity No', 'Trans Date', 'Trans Type', 'Equip Nbr', 'Work Ctr', 'To WC', 'Scrap Code', 'WO Nbr',  'PC Cust', '合計片数']; 
let Scrap2ndProp = ['PROD_NBR', 'ENTITY_NO', 'TRANS_DATE', 'TRANS_TYPE', 'EQUIP_NBR', 'OPER', 'TO_WC', 'SCRAP_CODE', 'WO_NBR', 'PC_CUST', 'PASS_QTY'];


// Top rework out / Bottom In / Bottom Rework / Bottom Rework(1)以上，一階頁面
let OperDetail1Label = ['Operation', 'Quantity', 'MVIN'];
let OperDetail1Prop = ['WORK_CTR', 'RWKOUT_QTY', 'IN20_QTY'];
let ReworkOutClickColumn = ['RWKOUT_QTY']

// Top rework out / Bottom In / Bottom Rework / Bottom Rework(1)以上，二階頁面
let OutTorwkLabel = ['Prod Nbr', 'Acct Date', 'Trans Type', 'EquIp Nbr', 'Operator', 'Output FG', 'Work Ctr', 'To WC', 'WO Nbr', 'PC Cust', 'Rework 次数', '合計片数']; 
let OutTorwkProp = ['PROD_NBR', 'ACCT_DATE', 'TRANS_TYPE', 'EQUIP_NBR', 'OPERATOR', 'OUTPUT_FG', 'WORK_CTR', 'TO_WC', 'WO_NBR', 'PC_CUST', 'STIMES_TTL', 'QTY'];

// Top WIP，一階頁面
let WipQtyLabel = ['Product', 'Reversion', 'Qty'];
let WipQtyProp = ['PROD_NBR', 'VER', 'COUNT'];
let WipClickColumn = ['COUNT'];

// Top WIP ，二階頁面
let WIP2ndLabel = ['Prod Nbr', 'Acct Date', 'Last Trans Dt', 'Panel ID', 'Last EQP Nbr', 'Work Ctr', 'Next Work Ctr', 'WO Nbr', 'PC Cust', 'Rework 次數', '合計片數'];
let WIP2ndProp = ['PROD_NBR', 'ACCT_DATE', 'LAST_TRANS_DT', 'PNL_ID', 'LAST_EQP_NBR', 'OPER', 'NEXT_OPER', 'WO_NBR', 'PC_CUST', 'STIMES_TTL', 'QTY'];


// Top WIP hold / Top Non Hold，/ Bottom WIP hold / Bottom Non Hold一階頁面
let OperDetail9Label = ['Product', 'Revision', 'Panel ID', 'Owner', 'Operation', 'Size', 'Cassette ID', 'LCD Product', 'LCD Grade', 'Exp/RMA/Resorting No', 'Status', 'Hold', 'Rework', 'Stay Hrs', 'Date', 'Hold Category', 'Hold Comments', 'Priority'];
let OperDetail9Prop = ['PROD_NBR', 'VER', 'PNL_ID', 'LCM_OWNER', 'WORK_NAME', 'PANEL_SIZE', 'CST_ID', 'PANEL_PN', 'LCD_GRADE', 'WO_NBR', 'STATUS', 'HOLD_FLAG', 'STIMES_TTL', 'SATY_HOURS', 'TRANS_DATE', 'C_DESC', 'CUR_HOLD_COMMENT', 'PRIORITY'];

// Top IN，一階頁面
let OperDetailINLabel = ['機種名稱', '版本', '機台', '班別', 'Move IN 數'];
let OperDetailINProp = ['PROD_NBR', 'VER', 'EQUIP_NBR', 'SHIFT_ID', 'IN_QTY'];
let INClickColumn = ['IN_QTY']

// Top IN ，二階頁面
let IN2ndLabel = ['Prod Nbr', 'Panel ID', 'Trans Date', 'Trans Type', 'EQUIP Nbr', 'Operator', 'From WC', 'WO Ctr', 'Qty', 'Comment'];
let IN2ndProp = ['PROD_NBR', 'PNL_ID', 'TRANS_DATE', 'TRANS_TYPE', 'EQUIP_NBR', 'OPERATOR', 'MAIN_WC', 'TO_OPER', 'QTY', 'COMMENT'];


// Bottom Rwk WIP，子页面
let RwkWipDetailLabel = ['Prod Nbr', 'Acct Date', 'Last Trans Dt', 'Panel ID', 'Last EQP Nbr', 'Work Ctr', 'Next Work Ctr', 'WO Nbr', 'PC Cust', 'Rework 次數', '合計片數'];
let RwkWipDetailProp = ['PROD_NBR', 'ACCT_DATE', 'LAST_TRANS_DT', 'PNL_ID', 'LAST_EQP_NBR', 'OPER', 'NEXT_OPER', 'WO_NBR', 'PC_CUST', 'STIMES_TTL', 'QTY'];


export { topTableProps, bottomTableProps };
export const topBaseColumns: TableColumn[] = TableCustomCell(topTableLabels, topTableProps, TopClick, true)
export const bottomBaseColumns: TableColumn[] = TableCustomCell(bottomTableLabels, bottomTableProps, BottomClick, true)

export const OperDetail4Columns: TableColumn[] = TableCustomCell(OperDetail4Label, OperDetail4Prop, RwkQtyClickColumn)
export const ToRwk2ndColumns: TableColumn[] = TableCustomCell(ToRwk2ndLabel, ToRwk2ndProp)
export const Scrap2ndColumns: TableColumn[] = TableCustomCell(Scrap2ndLabel, Scrap2ndProp)

export const OperDetail3Columns: TableColumn[] = TableCustomCell(OperDetail3Label, OperDetail3Prop, PassQtyClickColumn)
export const PassQty2ndColumns: TableColumn[] = TableCustomCell(PassQty2ndLabel, PassQty2ndProp)

export const OperDetail1Columns: TableColumn[] = TableCustomCell(OperDetail1Label, OperDetail1Prop, ReworkOutClickColumn)
export const OutTorwkColumns: TableColumn[] = TableCustomCell(OutTorwkLabel, OutTorwkProp)

export const WipColumns: TableColumn[] = TableCustomCell(WipQtyLabel, WipQtyProp, WipClickColumn)
export const WIP2ndColumns: TableColumn[] = TableCustomCell(WIP2ndLabel, WIP2ndProp)

export const OperDetail9Columns: TableColumn[] = TableCustomCell(OperDetail9Label, OperDetail9Prop)

export const OperDetailINColumns: TableColumn[] = TableCustomCell(OperDetailINLabel, OperDetailINProp, INClickColumn)
export const IN2ndColumns: TableColumn[] = TableCustomCell(IN2ndLabel, IN2ndProp)

export const RwkWipDetailColumns: TableColumn[] = TableCustomCell(RwkWipDetailLabel, RwkWipDetailProp)

// 
function TableCustomCell(Label: string[], Prop: string[], ClickColumn: string[] = [], ToOtherpage: Boolean = false) {
  return Prop.map((item: string, index: number) => {
    return {
      title: ClickColumn.includes(item) ? `${Label[index]}*` : Label[index],//可点击的栏位显示*号
      align: 'center' as any,
      dataIndex: item,
      //@ts-ignore
      customCell: (record: Object, rowindex: number, column: Object) => {
        if (ClickColumn.includes(item)) {
          return {
            onDblclick: () => {
              const UseParamsAdd = AddOperationParamsWithOut()
              const useReportParams = ReportCommonWithOut();
              const useCommonParams = AddCommonPageParams();
              let ReportNameParams = `OperationPerformanceDetail_${useCommonParams.Fab}`;
              debugger
              // UseParamsAdd.AddSecondTableParams({ ...record, ...column })
              // 是否跳轉到二級頁面
              
              if (ToOtherpage) {
                console.log("跳轉到第二層");
                //UseParamsAdd.AddFirstTableParams({ ...record, ...column['__originColumn__'] })
                useReportParams.AddFirstTableParams(ReportNameParams,{ ...record, ...column['__originColumn__'] })
                router.push({ name: 'OperationPerformanceDetail_'+useCommonParams.Fab });
              } else {
                console.log("輸出ClickColumn",ClickColumn);
                console.log("輸出record",record);
                //if (UseParamsAdd.FirstTableParams["dataIndex"] == "IN_QTY" && ClickColumn.includes('QTY') && ["總計", "合計"].includes(record["OPER"])) {
                if (useReportParams[ReportNameParams].FirstTableParams["dataIndex"] == "IN_QTY" && ClickColumn.includes('QTY') && ["總計", "合計"].includes(record["OPER"])) {
                 /*  if(){
                    
                  } */
                  // 跳轉到表格4
                  console.log("表格4");
                  //UseParamsAdd.AddThirdTableParams({ ...UseParamsAdd.ThirdTableClickParams[rowindex - 1], ...column['__originColumn__'] })
                  useReportParams.AddThirdTableParams(`OperationPerformanceDetail_${useCommonParams.Fab}`,{ ...useReportParams[ReportNameParams].ThirdTableClickParams[rowindex - 1], ...column['__originColumn__'] })
                } else {
                  // 這裏判斷跳到三表格
                  console.log("跳轉表格3");
                  //UseParamsAdd.AddSecondTableParams({ ...record, ...column['__originColumn__'] })
                  useReportParams.AddSecondTableParams(`OperationPerformanceDetail_${useCommonParams.Fab}`,{ ...record, ...column['__originColumn__'] })
                }

              }
            }, style: { cursor: 'pointer' }
          }
        } else {
          return {
            onDblclick: () => {
              message.warning('此欄位沒有二階數據')
            }, style: {}
          }
        }

      }
    }
  })

}
let OperationColumsData = { OperDetail3Columns, OperDetail4Columns, OperDetail1Columns, WipColumns, OperDetail9Columns, OperDetailINColumns, RwkWipDetailColumns, PassQty2ndColumns, ToRwk2ndColumns, WIP2ndColumns, IN2ndColumns, Scrap2ndColumns}
export default OperationColumsData