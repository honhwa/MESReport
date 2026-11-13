import type { FormSchema } from '@/components/core/schema-form/';
// import { getRoleList } from '@/api/system/role';



export const userSchemas: FormSchema[] = [
  {
    field: 'ROLE',
    component: 'Input',
    label: '角色',
    required: true,
    // slot: 'remoteSearch',
  },
  {
    field: 'ROLELEVEL',
    component: 'Input',
    label: '角色等級',
  },
  {
    field: 'BACKUP',
    component: 'Input',
    label: '備注説明',
  },
  {
    field: 'PERSON',
    component: 'Input',
    label: '最後更改者',
    vIf:false
  },
];



