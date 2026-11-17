import type { TableColumn } from '@/components/core/dynamic-table';
import { formatToDateTime } from '@/utils/dateUtil'

export type TableListItem = API.RoleListItem;
export type TableColumnItem = TableColumn<TableListItem>;



export const RoleManageColumns: TableColumnItem[] = [
  {
    title: '角色名称',
    dataIndex: 'ROLE',
    align: 'center',
  },
  {
    dataIndex: 'BACKUP',
    align: 'center',
    title: '備注説明',
  },
  {
    title: '创建时间',
    dataIndex: 'WRITE_DATE',
    bodyCell: ({ record }) => <>{formatToDateTime(record['WRITE_DATE'])}</>,
  },
  {
    title: '修改时间',
    align: 'center',
    dataIndex: 'UPDATE_TIME',
    bodyCell: ({ record }) => <>{formatToDateTime(record['UPDATE_TIME'])}</>,
  },
  
  {
    title: '角色等級',
    dataIndex: 'ROLELEVEL',
    hideInTable: true
  },

]