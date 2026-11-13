import type { TableColumn } from '@/components/core/dynamic-table';
export type TableListItem = API.DayliMoveThItem;
export type TableColumnItem = TableColumn<TableListItem>;
export const baseColumns1 = [

  {
    title: 'MAT_ID',//label
    align: 'center',
    dataIndex: 'MAT_ID',//prop屬性
    className: "v-center",
  },
  {
    title: 'PRODUCT_ID',
    align: 'center',
    dataIndex: 'PRODUCT_ID',
    className: "v-center",
  },
  {
    title: 'WARRANTY_LENGTH',
    align: 'center',
    dataIndex: 'WARRANTY_LENGTH',
    className: "v-center",
  },
  {
    title: 'UPDATE_DATE',
    align: 'center',
    dataIndex: 'UPDATE_DATE',
    className: "v-center",
  },
  
];
export const baseColumns2 = [

  {
    title: 'FIRST_FOUR_CODE',//label
    align: 'center',
    dataIndex: 'FIRST_FOUR_CODE',//prop屬性
    className: "v-center",
  },
  {
    title: 'MAT_TYPE',
    align: 'center',
    dataIndex: 'MAT_TYPE',
    className: "v-center",
  },
  {
    title: 'UPDATE_DATE',
    align: 'center',
    dataIndex: 'UPDATE_DATE',
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


