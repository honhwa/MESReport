import type { FormSchema } from '@/components/core/schema-form/';


export interface ButtonSetting{
    label:string,
    icon?:string,
    handle:Function,
    type:string,
    isShow:boolean
}


export const masterFlowSchemas:FormSchema<API.CreateMasterFlow>[] = [
  {
    field: 'remarks',
    component: 'Input',
    label: '备注',
    rules: [{ required: true, type: 'string' }],
    colProps: {
      span: 12,
    },
  },
  {
    field: 'remote',
    component: 'Input',
    label: '人员',
    rules: [{ required: true, type: 'string' }],
    colProps: {
      span: 12,
    },
  },
  {
    field: 'remote',
    component: 'Input',
    label: '选中',
    rules: [{ required: true, type: 'string' }],
    colProps: {
      span: 12,
    },
  },
  // {
  //   field: 'menus',
  //   component: 'Tree',
  //   label: '菜单权限',
  //   colProps: {
  //     span: 12,
  //   },
  //   componentProps: {
  //     checkable: true,
  //     vModelKey: 'checkedKeys',
  //     style: {
  //       height: '350px',
  //       paddingTop: '5px',
  //       overflow: 'auto',
  //       borderRadius: '6px',
  //       border: '1px solid #dcdfe6',
  //     },
  //   },
  // },
  // {
  //   field: 'depts',
  //   component: 'Tree',
  //   label: '部门权限',
  //   colProps: {
  //     span: 12,
  //   },
  //   componentProps: {
  //     checkable: true,
  //     vModelKey: 'checkedKeys',
  //     style: {
  //       height: '350px',
  //       paddingTop: '5px',
  //       overflow: 'auto',
  //       borderRadius: '6px',
  //       border: '1px solid #dcdfe6',
  //     },
  //   },
  // },
];


