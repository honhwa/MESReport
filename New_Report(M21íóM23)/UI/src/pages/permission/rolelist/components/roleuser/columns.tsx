import { Tag } from 'ant-design-vue';
import type { TableColumn } from '@/components/core/dynamic-table';
import { formatToDateTime } from '@/utils/dateUtil';
import { GetRoleList } from '@/api/system/menu/role';
import { ref } from 'vue';
import { Storage } from '@/utils/Storage';

export type TableListItem = API.UserListResultItem;
export type TableColumnItem = TableColumn<TableListItem>;
let Data = process.env.VUE_APP_SYSID
const UserParams = Storage.get('USERMESSAGE');

export let RoleList = ref<any[]>([])
export const baseColumns: TableColumnItem[] = [

  {
    title: '工号',//label
    width: 120,
    dataIndex: 'EMP_ID',//props
    align: 'center',
  },
  {
    title: '姓名',
    width: 120,
    align: 'center',
    dataIndex: 'EMP_NAME',
  },
  // {
  //   title: '密码',
  //   width: 120,
  //   align: 'center',
  //   dataIndex: 'PASSWORD',
  // },
  // {
  //   title: '部门',
  //   width: 120,
  //   align: 'center',
  //   dataIndex: 'DEPT_NAME',
  // },
  // {
  //   title: '备注',
  //   width: 120,
  //   align: 'center',
  //   dataIndex: 'REMARK',
  // },
  
  {
    title: '角色',
    align: 'center',
    dataIndex: 'EMP_PERMS',//EMP_PERMS需要由api獲取
    width: 120,
    formItemProps: {//作用同formSchemas,componentProps不可异步获取
      // slot: 'remoteSearch',
      component: 'Select',
      componentProps:  () => {
        return {
          options: RoleList.value
        }
      },
    },
  },
  {
    title: '状态',
    dataIndex: 'BACKUP',
    width: 100,
    formItemProps: {
      component: 'Select',
      componentProps: {
        options: [
          {
            label: '启用',
            value: 'Y',
          },
          {
            label: '禁用',
            value: 'N',
          },
        ],
      },
    },
    bodyCell: ({ record }) => {

      const isEnable = record.BACKUP === 'Y';
      return <Tag color={isEnable ? 'success' : 'red'}>{isEnable ? '启用' : '禁用'}</Tag>;
    },
  },
  {
    title: 'Token',
    dataIndex: 'Token',
    width: 200,
    
    bodyCell: ({ record }) => {
      let TokenData:string="";
      if(record.EMP_PERMS=='ApiToken')TokenData =btoa(record.EMP_ID ?? 'null')  
      else TokenData =  btoa(Data+record.EMP_ID+UserParams?.LoginDate ?? 'null')
      // return <span>{record.SHOW_TOKEN}</span>;
     if (record.SHOW_TOKEN) return <span>{TokenData}</span>;
     else return <>****</>
    },
  },
  {
    title: '创建时间',
    dataIndex: 'WRITE_DATE',
    width: 120,
    // formItemProps: {
    //   component: 'DatePicker',
    //   componentProps: {
    //     class: 'w-full',
    //   },
    // },
    hideInSearch: true,
    bodyCell: ({ record }) => <>{formatToDateTime(record['WRITE_DATE'])}</>,
  },
  // {
  //   title: '修改时间',
  //   dataIndex: 'UPDATE_TIME',
  //   width: 120,
  //   formItemProps: {
  //     component: 'DatePicker',
  //     componentProps: {
  //       class: 'w-full',
  //     },
  //   },
  // },
  {
    title: '修改时间',
    width: 120,
    align: 'center',
    dataIndex: 'UPDATE_TIME',
    hideInSearch: true,
    bodyCell: ({ record }) => <>{formatToDateTime(record['UPDATE_TIME'])}</>,
  },
];
