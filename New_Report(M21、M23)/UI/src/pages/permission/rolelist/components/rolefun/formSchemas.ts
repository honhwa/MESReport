import type { FormSchema } from '@/components/core/schema-form/';
// import { getRoleList } from '@/api/system/role';



export const userSchemas: FormSchema[] = [
  {
    field: 'RoleType',
    component: 'Select',
    label: '角色',
    required: true,
    slot: 'remoteSearch',
    colProps: {
      span: 8,
    },
    defaultValue:'Admin'
  },
  // {
  //   field: 'Equipment',
  //   component: 'Select',
  //   label: 'Equipment',
  //   slot: 'remoteSearch',
  // }
];



