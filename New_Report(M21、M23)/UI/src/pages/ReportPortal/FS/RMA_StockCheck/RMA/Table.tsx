import type { TableColumn } from '@/components/core/dynamic-table';
export type TableListItem = API.DayliMoveThItem;
export type TableColumnItem = TableColumn<TableListItem>;
export const baseColumns1 = [

  {
    title: '物料料號',//label
    align: 'center',
    dataIndex: 'ITEM_ID',//prop屬性
    className: "v-center",
  },
  {
    title: '數量',
    align: 'center',
    dataIndex: 'PRI_QTY',
    className: "v-center",
  },
  {
    title: '對應成品料號',
    align: 'center',
    dataIndex: 'PRODUCT_ID',
    className: "v-center",
  },
  {
    title: 'Model Name',
    align: 'center',
    dataIndex: 'MODEL_NAME',
    className: "v-center",
  },
  {
    title: '產品階段',
    align: 'center',
    dataIndex: 'STATE',
    className: "v-center",
  },
  {
    title: '產品階段Start_Day',
    align: 'center',
    dataIndex: 'STATE_START_DAY',
    className: "v-center",
  },
  {
    title: '保固期(月數)',
    align: 'center',
    dataIndex: 'BG_MONTHS',
    className: "v-center",
  },
  {
    title: '預計保固日期',
    align: 'center',
    dataIndex: 'BG_DAY',
    className: "v-center",
  },
  {
  
    title: '已issue未收料',
    align: 'center',
    dataIndex: 'UNRECEIVED_QTY',
    className: "v-center",
  },
  {
    title: '不良品倉庫存',
    align: 'center',
    dataIndex: 'PROD_QTY',
    className: "v-center",
  },
  {
    title: '上年下線量',
    align: 'center',
    dataIndex: 'BEF_YEAR_QTY',
    className: "v-center",
  },
  {
    title: '今年下線量',
    align: 'center',
    dataIndex: 'NOW_YEAR_QTY',
    className: "v-center",
  },
  {
    title: '延保機種',
    align: 'center',
    dataIndex: 'YB_PROD',
    className: "v-center",
  },
  {
    title: '未來返品量',
    align: 'center',
    dataIndex: 'RQ_PREDICT_QTY',
    className: "v-center",
  },
  {
    title: 'MP/EOL',
    align: 'center',
    dataIndex: 'JD_TY',
    className: "v-center",
  },
  {
    title: '庫齡',
    align: 'center',
    dataIndex: 'AGING_MEAS',
    className: "v-center",
  },
  {
    title: '成品是否EOS',
    align: 'center',
    dataIndex: 'P_EOS',
    className: "v-center",
  },
  {
    title: '物料去化評估',//物料是否EOS
    align: 'center',
    dataIndex: 'M_EOS',
    className: "v-center",
  },
  
];
export const baseColumns2 = [
//ACCT_DATE, FAB, ITEM_ID, MAT_TYPE, PRI_QTY, PRI_MONEY, KEFAN_QTY, TIDAI_ID
  {
    title: '檢核日期',//label
    align: 'center',
    dataIndex: 'ACCT_DATE',//prop屬性
    className: "v-center",
  },
  {
    title: '廠別',
    align: 'center',
    dataIndex: 'FAB',
    className: "v-center",
  },
  {
    title: '物料料號',
    align: 'center',
    dataIndex: 'ITEM_ID',
    className: "v-center",
  },
  {
    title: 'PART分類',
    align: 'center',
    dataIndex: 'MAT_TYPE',
    className: "v-center",
  },
  {
    title: '庫存量',
    align: 'center',
    dataIndex: 'PRI_QTY',
    className: "v-center",
  },
  {
    title: '庫存金額(NTD)',
    align: 'center',
    dataIndex: 'PRI_MONEY',
    className: "v-center",
  },
  {
    title: '未來四個月預估量',
    align: 'center',
    dataIndex: 'KEFAN_QTY',
    className: "v-center",
  },
  {
    title: '警戒線(現有待修需用料)',
    align: 'center',
    dataIndex: 'WARNING_QTY',
    className: "v-center",
  },
  {
    title: '待去化數量',
    align: 'center',
    dataIndex: 'WAITGO_QTY',
    className: "v-center",
  },
  {
    title: '待去化金額(NTD)',
    align: 'center',
    dataIndex: 'WAITGO_NTD',
    className: "v-center",
  },
  {
    title: '待備料數量',
    align: 'center',
    dataIndex: 'UNWAITGO_QTY',
    className: "v-center",
  },
  {
    title: '待備料金額(NTD)',
    align: 'center',
    dataIndex: 'UNWAITGO_NTD',
    className: "v-center",
  },
  {
    title: '替代料',
    align: 'center',
    dataIndex: 'TIDAI_ID',
    className: "v-center",
  },
  
];
export const baseColumns3 = [

  {
    title: 'FAB',//label
    align: 'center',
    dataIndex: 'FAB',//prop屬性
    className: "v-center",
  },
  {
    title: 'MAT_ID',
    align: 'center',
    dataIndex: 'MAT_ID',
    className: "v-center",
  },
  {
    title: 'QTY',
    align: 'center',
    dataIndex: 'QTY',
    className: "v-center",
  },
  {
    title: 'UPDATE_DATE',
    align: 'center',
    dataIndex: 'UPDATE_DATE',
    className: "v-center",
  },
  
];


