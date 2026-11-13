import type { TableColumn } from '@/components/core/dynamic-table';
import {formatToDateTime} from '@/utils/dateUtil'

export type TableListItem = API.UserListResultItem;
export type TableColumnItem = TableColumn<TableListItem>;



export const RoleFunColumns: TableColumnItem[] = [
  {
    title: '路由ID',
    dataIndex: 'id',
    
  },
  {
    title: '名称',
    dataIndex: 'name',
    align: 'center',
    bodyCell: ({ record }) => <>{record['name'] && <title-i18n title={record['name']}/>}</>,
  },
  
  {
    title: '节点路由',
    dataIndex: 'router',
    align: 'center',
  },
];