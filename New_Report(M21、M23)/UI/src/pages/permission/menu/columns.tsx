import type { TableColumn } from '@/components/core/dynamic-table';
import {formatToDateTime} from '@/utils/dateUtil'
import { ref } from 'vue';
// import { Avatar, Space, Tag } from 'ant-design-vue';

export type TableListItem = API.MenuListResultItem;
export type TableColumnItem = TableColumn<TableListItem>;
export let RoleList = ref<any[]>([])
/**
 * 将对应菜单类型转为字符串字意
 */
const getMenuType = (type) => {
  switch (type) {
    case 0:
      return '目录';
    case 1:
      return '菜单';
    case 2:
      return '权限';
    default:
      return '';
  }
};

export const baseColumns: TableColumnItem[] = [
  {
    title: '名称',
    dataIndex: 'name',
    bodyCell: ({ record }) => <>{record.name && <title-i18n title={record.name}/>}</>,
    fixed:'left'
  },
  {
    title: '图标',
    width: 80,
    dataIndex: 'icon',
    align: 'center',
    bodyCell: ({ record }) => <>{record.icon && <icon-font type={record.icon} size="22" />}</>,
  },
  {
    title: '类型',
    width: 80,
    align: 'center',
    dataIndex: 'type',
    bodyCell: ({ record }) => <>{getMenuType(record.type)}</>,
  },
  {
    title: '节点路由',
    dataIndex: 'router',
    align: 'center',
    width: 240,
  },
  {
    title: '路由缓存',
    dataIndex: 'keepalive',
    align: 'center',
    width: 80,
  },
  {
    title: '是否显示在侧边菜单',
    dataIndex: 'isShow',
    align: 'center',
    width: 120,
  },

  {
    title: '文件路径',
    width: 280,
    align: 'center',
    dataIndex: 'viewPath',
  },
  // {
  //   title: '权限',
  //   width: 300,
  //   align: 'center',
  //   dataIndex: 'perms',
  // },
  {
    title: 'Owner',
    width: 100,
    align: 'center',
    dataIndex: 'Owner',
  },
  {
    title: '排序号',
    width: 80,
    align: 'center',
    dataIndex: 'orderNum',
  },
  {
    title: '更新时间',
    width: 180,
    align: 'center',
    dataIndex: 'updateTime',
    bodyCell: ({ record }) => <>{formatToDateTime(record['updateTime'])}</>,
  },
  {
    title: '系統名稱',
    width: 80,
    align: 'center',
    dataIndex: 'SYSID',
    hideInTable:true
  },
  {
    title: '菜單名稱',
    width: 80,
    align: 'center',
    dataIndex: 'menuId',
    hideInTable:true
  },
  {
    title: '路由名稱',
    width: 80,
    align: 'center',
    dataIndex: 'routeName',
    hideInTable:true
  },
  {
    title: '説明文件',
    width: 80,
    align: 'center',
    dataIndex: 'Explain',
    hideInTable:true
  },
];
