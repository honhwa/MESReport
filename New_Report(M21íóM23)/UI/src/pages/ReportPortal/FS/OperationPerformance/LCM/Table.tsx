import type { TableColumn } from '@/components/core/dynamic-table';
import { AddOperationParamsWithOut } from '@/store/module/operationDeatil';
import { message } from 'ant-design-vue';
import router from '@/router';


let topTableLabels = ["Operation", "In", "Out", "WIP", "Hold", "Non Hold", "T/D ↓", "T/R ↑", "In(from rwk)", "Out(to rwk)", "1'st yield%", "Total yield%", "In(R)", "Out(R)", "WIP(R)", "Hold(R)", "Non Hold(R)", "T/D(R) ↓", "T/R(R) ↑", "Out(R)To rwk", "前→后", "后→前", "修理再發率%"
]
// 首页上面可点击列
let TopClick = ["PASS_QTY", "WIP_QTY", "RWK_OUTQTY", "TOTALRWK_QTY", "RWK_WIP_QTY", "MTO", "TOM"]
let topTableProps = ["WORK_CTR", "IN_QTY", "PASS_QTY", "WIP_QTY", "HOLD_WIP_QTY", "NHOLD_WIP_QTY", "TD", "TR", "RWK_OUTQTY", "TOTALRWK_QTY", "FirstYield", "TotalYield", "RWK_IN_QTY", "RWK_PASS_QTY", "RWK_WIP_QTY", "RWK_HOLDWIP_QTY", "RWK_NHOLDWIP_QTY", "TDR", "TRR", "RWK_TORWK_QTY", "MTO", "TOM", "RepairRate"]

let bottomTableLabels = ["Operation", "In", "Out", "WIP", "Hold", "Non Hold", "T/D↓", "T/R↑", "Out(to main)", "To be scrap", "Out rwk(1)以上", "Out rwk(1)以上%", "Wip(PCBI)", "Wip(A KEN)", "Wip(CD KEN)",]
let bottomTableProps = ["WORK_CTR", "IN_QTY", "PASS_QTY", "WIP_QTY", "HOLD_WIP_QTY", "NHOLD_WIP_QTY", "TD", "TR", "RWK_TOMAIN_QTY", "TOBESCRAP_QTY", "PASS2_QTY", "PASS2_QTYRate", "S1_WIP_QTY", "S2_WIP_QTY", "S3_WIP_QTY"]
// 下面可点击列
let BottomClick = ["IN_QTY", "WIP_QTY", "HOLD_WIP_QTY", "NHOLD_WIP_QTY", "TOBESCRAP_QTY", "PASS2_QTY", 'S1_WIP_QTY',
  'S2_WIP_QTY',
  'S3_WIP_QTY']

// 首页上表格的out，子页面
let OperDetail3Label = ['機種名稱', '版本', '機台', '班别', '投產數', '良品數', 'Rework'];
let OperDetail3Prop = ['PROD_NBR', 'VER', 'EQUIP_NBR', 'SHIFT_ID', 'PASS_QTY', 'GOOD_PASS', 'RWK_QTY'];
let PassQtyClickColumn = ['PASS_QTY']


// out to rwk的second页面
let OutTorwkLabel = [
  'prod_nbr',
  'entity_no',
  'trans_date',
  'trans_type',
  'equip_nbr',
  'operator',
  'output_fg',
  'work_ctr',
  'to_wc',
  'defect_code',
  'rework次数',
  'dfct次数',
  '合计片数'
]; let OutTorwkProp = [
  'PROD_NBR',
  'ENTITY_NO',
  'TRANS_DATE',
  'TRANS_TYPE',
  'EQUIP_NBR',
  'OPERATOR',
  'OUTPUT_FG',
  'WORK_CTR',
  'TO_WC',
  'DFCT_CODE',
  'STIMES_TTL',
  'ITEM_NBR',
  'PASS_QTY'
];
let WipQtyLabel = ['product', 'Reversion', 'Qty'];
let WipQtyProp = ['PROD_NBR', 'VER', 'COUNT'];
let WipClickColumn = ['COUNT'];

let LCM2WipQtyLabel = ['product', 'Reversion', 'Qty','綫體類型'];
let LCM2WipQtyProp = ['PROD_NBR', 'VER', 'COUNT','FLAG'];
let LCM2WipClickColumn = ['COUNT'];


let WipDetailLabel = ['prod_nbr','acct_date','last_trans_dt','pnl_id','last_eqp_nbr','work_ctr','defect_code','pre_dfctcode','rework次数','合计片数']
let WipDetailProp = ["PROD_NBR", "ACCT_DATE", "LAST_TRANS_DT", "PNL_ID", "LAST_EQP_NBR", "WORK_CTR", "DFCT_CODE", "PRE_DFCTCODE", "STIMES_TTL", "QTY"]
let ReworkLabel = ['Defect Code', 'Description', "Qty"]
let ScrapLabel = ['Script Code', 'Description', "Qty"]

let ReworkProp = ['CODE', 'CODE_DESC', 'COUNT']

let MTOLabel = ['PROD_NBR', 'FROM_OPER',"前站點→后站點"]
let MTOProp = ['PROD_NBR', 'FR_ERP_OPER', 'TOM'];
let MTOClickColumn = ['TOM']

let TOMLabel = ['PROD_NBR', 'FROM_OPER',"后站點→前站點"]
let TOMProp = ['PROD_NBR', 'FR_ERP_OPER', 'TOM'];
let TOMClickColumn = ['TOM']

let AllWipLabel = ["prod_nbr", "acct_date", "last_trans_dt", "pnl_id", "last_eqp_nbr", "work_ctr", "defect_code", "pre_dfctcode", "rework次数", "合计片数"];
let AllWipLabelProp = ["PROD_NBR", "ACCT_DATE", "LAST_TRANS_DT", "PNL_ID", "LAST_EQP_NBR", "WORK_CTR", "DFCT_CODE", "PRE_DFCTCODE", "STIMES_TTL", "QTY"];

let InpassLabel = ['Operation', '合计'];
let Inpass2Prop = ['WORK_CTR', 'QTY'];
let InpassClickColumn = ['QTY'];


let MTOTomLabel = ["FAC_ID", "TRANS_NBR", "TRANS_DATE", "PROP_NBR", "VER", "PNL_ID", "OPERATORS", "TRANS_TYPE", "OPER", "COMMENTS", "MAT_TYPE", "MAT_ID", "LOT_NUMBER", "FROM_OPER", "TO_OPER", "WO_NBR", "EQUIP_NBR"];
let MTOTomProps = ["FAC_ID", "TRANS_NBR", "TRANS_DATE", "PROD_NBR", "VER", "PNL_ID", "OPERATORS", "TRANS_TYPE", "OPER", "COMMENTS", "MAT_TYPE", "MAT_ID", "LOT_NUMBER", "FR_ERP_OPER", "TO_ERP_OPER", "WO_NBR", "EQUIP_NBR"];

let SetMTOTomProps = ["FAC_ID", "TRANS_NBR", "TRANS_DATE", "PROD_NBR", "VER", "PNL_ID", "OPERATORS", "TRANS_TYPE", "OPER", "COMMENTS", "MAT_TYPE", "MAT_ID", "LOT_NUMBER", "OPER1", "TO_OPER", "WO_NBR", "EQUIP_NBR"];

let InqtyLabel = [
  'oper',
  'prod_nbr',
  'acct_date',
  'trans_type',
  'equip_nbr',
  'operator',
  'output_fg',
  'work_ctr',
  'to_wc',
  'rework次数',
  '合计片数'
];
let InqtyProp = [
  'OPER',
  'PROD_NBR',
  'ACCT_DATE',
  'TRANS_TYPE',
  'EQUIP_NBR',
  'OPERATOR',
  'OUTPUT_FG',
  'WORK_CTR',
  'TO_WC',
  'STIMES_TTL',
  'QTY'
];

let TobethildLabel = [
  'prod_nbr',
  'entity_no',
  'trans_date',
  'trans_type',
  'equip_nbr',
  'work_ctr',
  'to_wc',
  'scarp_code',
  '合计片数'
];
let TobethildProps = [
  'PROD_NBR',
  'ENTITY_NO',
  'TRANS_DATE',
  'TRANS_TYPE',
  'EQUIP_NBR',
  'WORK_CTR',
  'TO_WC',
  'DFCT_CODE',
  'PASS_QTY'
];
let ThirdProps = ["PNL_ID", "TRANS_DATE", "OPERATOR", "WORK_CTR", "TO_WC", "STIMES_TTL", "MAT_TYPE", "Mat ID", "LOT_NUMBER", "DFCT_CODE"]
// _this.calcuList = ["PASS_QTY"];
let ThirdLabel = ["Panel ID", "Trans_Date", "Operactor", "Work_ctr", "to_wc", "rework次數", "Mat Type", "Mat Id", "Lot Number", "Dfct Code"]
// _this.mergerlist = [];
// export type TableListItem = API.OperationPerformanceThItem;
// export type TableColumnItem = TableColumn<TableListItem>;
export { topTableProps, bottomTableProps };
export const topBaseColumns: TableColumn[] = TableCustomCell(topTableLabels, topTableProps, TopClick, true)
export const bottomBaseColumns: TableColumn[] = TableCustomCell(bottomTableLabels, bottomTableProps, BottomClick, true)

export const OperDetail3Columns: TableColumn[] = TableCustomCell(OperDetail3Label, OperDetail3Prop, PassQtyClickColumn)

export const WipColumns: TableColumn[] = TableCustomCell(WipQtyLabel, WipQtyProp, WipClickColumn)
export const LCM2WipColumns: TableColumn[] = TableCustomCell(LCM2WipQtyLabel, LCM2WipQtyProp, LCM2WipClickColumn)
export let WipDetailCoulmns: TableColumn[] = TableCustomCell(WipDetailLabel, WipDetailProp)

export const ReworkColumns: TableColumn[] = TableCustomCell(ReworkLabel, ReworkProp, WipClickColumn)
export const ScrapColumns: TableColumn[] = TableCustomCell(ScrapLabel, ReworkProp, WipClickColumn)

export const TOMColumns: TableColumn[] = TableCustomCell(TOMLabel, TOMProp, TOMClickColumn)
export const MTOColumns: TableColumn[] = TableCustomCell(MTOLabel, MTOProp, MTOClickColumn)
export const AllWipColumns: TableColumn[] = TableCustomCell(AllWipLabel, AllWipLabelProp)
export const Inpass2Columns: TableColumn[] = TableCustomCell(InpassLabel, Inpass2Prop, InpassClickColumn)
export const OutTorwkColumns: TableColumn[] = TableCustomCell(OutTorwkLabel, OutTorwkProp)
export const MTOTom: TableColumn[] = TableCustomCell(MTOTomLabel, MTOTomProps)
export const SetMTOTom: TableColumn[] = TableCustomCell(MTOTomLabel, SetMTOTomProps)
export const InqtyColumns: TableColumn[] = TableCustomCell(InqtyLabel, InqtyProp, InpassClickColumn)
export const Inpass2secondColumns: TableColumn[] = TableCustomCell(InqtyLabel, InqtyProp)
export const TobethildColumns: TableColumn[] = TableCustomCell(TobethildLabel, TobethildProps)
export const ThirdColumns: TableColumn[] = TableCustomCell(ThirdLabel, ThirdProps)
let FactoryType = ""
export function pushFactoryType(sFactoryType:string) {
  FactoryType  = sFactoryType;
}


function TableCustomCell(Label: string[], Prop: string[], ClickColumn: string[] = [], ToOtherpage: Boolean = false) {
  return Prop.map((item: string, index: number) => {
    return {
      // title: ClickColumn.includes(item) ? `${Label[index]}*` : Label[index],//可点击的栏位显示*号
      title: Label[index],//可点击的栏位显示*号
      align: 'center' as any,
      dataIndex: item,
      className:index == 0 ? "v-center" : '',
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
                UseParamsAdd.AddFirstTableParams({ ...record, ...column['__originColumn__'] })
                router.push({ name: `OperationPerformanceDetail${FactoryType}` });
              } else {
                console.log("輸出ClickColumn",ClickColumn);
                console.log("輸出record",record);
                if (UseParamsAdd.FirstTableParams["dataIndex"] == "IN_QTY" && ClickColumn.includes('QTY') && ["總計", "合計"].includes(record["OPER"])) {
                  // 跳轉到表格4
                  console.log("表格4");
                  UseParamsAdd.AddThirdTableParams({ ...UseParamsAdd.ThirdTableClickParams[rowindex - 1], ...column['__originColumn__'] })
                } else {
                  // 這裏判斷跳到三表格
                  console.log("跳转表格3");
                  UseParamsAdd.AddSecondTableParams({ ...record, ...column['__originColumn__'] })
                }
              }
            }, style: { cursor: 'pointer' }
          }
        } else {
          return {
            onDblclick: () => {
              message.warning('此栏位没有二阶数据')
            }, style: {}
          }
        }

      }
    }
  })

}
let OperationColumsData = { OperDetail3Columns, WipColumns, WipDetailCoulmns, ReworkColumns, TOMColumns, MTOColumns }
export default OperationColumsData