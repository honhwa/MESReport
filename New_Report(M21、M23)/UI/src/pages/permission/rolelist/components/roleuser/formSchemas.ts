import type { FormSchema } from '@/components/core/schema-form/';
import {  RoleList } from './columns';
import { useI18n } from '@/hooks/useI18n';



const { t } = useI18n();
export const userSchemas: FormSchema<API.CreateUserParams>[] = [
  {
    field: 'EMP_ID',
    component: 'Input',
    label: '工号',
    rules: [{ required: true }],
    colProps: {
      span: 16,
    },
  },
  {
    field: 'EMP_NAME',
    component: 'Input',
    label: '姓名',
    colProps: {
      span: 16,
    },
    rules: [{ required: true }],
    // vIf: ({ formModel }) => formModel['type'] !== 2,
  },
  {
    field: 'EMP_PERMS',//EMP_PERMS需要由api獲取
    label: '角色',
    component: 'Select',
    componentProps:  () => {
        
      return {
        options: RoleList.value
      }
    },
    rules: [{ required: true, type: 'string', message: '请选择权限' }],
    colProps: {
      span: 16,
    },
  },
  
  {
    field: 'BACKUP',
    component: 'RadioGroup',
    label: '状态',
    defaultValue: 'N',
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
    colProps: {
      span: 16,
    },
  },
  // isAdvanced:true
];
/**
 * @description 更新用户密码
 */
export const updatePswSchemas: FormSchema[] = [
  {
    field: 'password',
    component: 'Input',
    label: '新密码',
    rules: [{ required: true, type: 'string' }],
  },
];


