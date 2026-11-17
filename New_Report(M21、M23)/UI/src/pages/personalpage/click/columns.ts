import type { TableColumn } from '@/components/core/dynamic-table';


export const baseColumns: TableColumn[] = [
  
  // {
  //   title: '路由ID',
  //   align: 'center',
  //   dataIndex: 'REPORT_ID',
  // },
  {
    title: '廠別',
    dataIndex: 'SYSMODULE',
    align: 'center',
  },
  {
    title: '模塊',
    dataIndex: 'MODULE',
    align: 'center',
  },
  {
    title: '節點名稱',
    dataIndex: 'SYSFUN',
    align: 'center',
  },
  {
    title: '點擊次數',
    align: 'center',
    dataIndex: 'REPORT',
    sorter: {
      compare: (a, b) => a.REPORT - b.REPORT,
      multiple: 2,
    },
  },
];
