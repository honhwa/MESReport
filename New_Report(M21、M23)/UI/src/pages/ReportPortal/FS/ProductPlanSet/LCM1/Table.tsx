import type { TableColumn } from '@/components/core/dynamic-table';
export type TableListItem = API.DayliMoveThItem;
export type TableColumnItem = TableColumn<TableListItem>;
export const baseColumns = [

  {
    title: 'PLAN_DATE',//label
    align: 'center',
    dataIndex: 'PLAN_DATE',//prop屬性
    className: "v-center",
  },
  {
    title: 'PROD_NBR',
    align: 'center',
    dataIndex: 'PROD_NBR',
    className: "v-center",
  },
  {
    title: 'CUSTOMER',
    align: 'center',
    dataIndex: 'CUSTOMER',
    className: "v-center",
  },
  {
    title: 'PROD_TYPE',
    align: 'center',
    dataIndex: 'PROD_TYPE',
    className: "v-center",
  },
   {
    title: 'QTY',
    align: 'center',
    dataIndex: 'QTY',
    className: "v-center",

  }, {
    title: 'MONTH_CD',
    align: 'center',
    dataIndex: 'MONTH_CD',
    className: 'v-center',
  },{
    title: 'PLAN_VER',
    align: 'center',
    dataIndex: 'PLAN_VER',
    className: "v-center",

  },
];
