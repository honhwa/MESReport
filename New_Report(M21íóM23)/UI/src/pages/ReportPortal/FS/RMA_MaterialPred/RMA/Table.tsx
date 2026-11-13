import type { TableColumn } from '@/components/core/dynamic-table';
export type TableListItem = API.DayliMoveThItem;
export type TableColumnItem = TableColumn<TableListItem>;
export const baseColumns1 = [
  //ACCT_DATE, FAB, MAT_ID, MAT_TYPE, QTY, PRI_QTY, GAP, 
  //PRODUCT_ID, MODEL_NAME, JD_TY, DW_QTY, HY_YIELD, 
  //BACK_QTY, TIDAI_ID
    {
      title: '年月',//label
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
      dataIndex: 'MAT_ID',
      className: "v-center",
    },
    {
      title: 'PART_NAME',
      align: 'center',
      dataIndex: 'MAT_TYPE',
      className: "v-center",
    },
    {
      title: '未來物料預估量',
      align: 'center',
      dataIndex: 'QTY',
      className: "v-center",
    },
    {
      title: '庫存量',
      align: 'center',
      dataIndex: 'PRI_QTY',
      className: "v-center",
    },
    {
      title: '缺口量',
      align: 'center',
      dataIndex: 'GAP',
      className: "v-center",
    },
    {
      title: 'PRODUCT_ID',
      align: 'center',
      dataIndex: 'PRODUCT_ID',
      className: "v-center",
    },
    {
      title: 'MODEL_NAME',
      align: 'center',
      dataIndex: 'MODEL_NAME',
      className: "v-center",
    },
    {
      title: '產品狀態',
      align: 'center',
      dataIndex: 'JD_TY',
      className: "v-center",
    },
    
  ];
export const baseColumns2 = [
//ACCT_DATE, FAB, MAT_ID, MAT_TYPE, QTY, PRI_QTY, GAP, 
//PRODUCT_ID, MODEL_NAME, JD_TY, DW_QTY, HY_YIELD, 
//BACK_QTY, TIDAI_ID
  {
    title: '年月',//label
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
    dataIndex: 'MAT_ID',
    className: "v-center",
  },
  {
    title: 'PART_NAME',
    align: 'center',
    dataIndex: 'MAT_TYPE',
    className: "v-center",
  },
  {
    title: '未來物料預估量',
    align: 'center',
    dataIndex: 'QTY',
    className: "v-center",
  },
  {
    title: '庫存量',
    align: 'center',
    dataIndex: 'PRI_QTY',
    className: "v-center",
  },
  {
    title: '缺口量',
    align: 'center',
    dataIndex: 'GAP',
    className: "v-center",
  },
  {
    title: 'PRODUCT_ID',
    align: 'center',
    dataIndex: 'PRODUCT_ID',
    className: "v-center",
  },
  {
    title: 'MODEL_NAME',
    align: 'center',
    dataIndex: 'MODEL_NAME',
    className: "v-center",
  },
  {
    title: '產品狀態',
    align: 'center',
    dataIndex: 'JD_TY',
    className: "v-center",
  },
  {
    title: '單位用量',
    align: 'center',
    dataIndex: 'DW_QTY',
    className: "v-center",
  },
  {
    title: '耗用率',
    align: 'center',
    dataIndex: 'HY_YIELD',
    className: "v-center",
  },
  {
    title: '未來返品預估量',
    align: 'center',
    dataIndex: 'BACK_QTY',
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


