// import MultipleCascader from './components/multiple-cascader/index.vue';
import type { FormSchema } from '@/components/core/schema-form/';
import IconsSelect from '@/components/basic/icons-select/index.vue';
// import { constantRouterComponents } from '@/router/asyncModules';
import { FormStatusWithOut } from '@/store/module/form';
const useFormStatus = FormStatusWithOut();
import {  RoleList } from './columns';

export const menuSchemas: FormSchema<API.MenuAddParams>[] = [
  {
    field: 'type',
    component: 'RadioGroup',
    label: '菜单类型',
    defaultValue: 0,
    rules: [{ required: true, type: 'number' }],
    componentProps: {
      options: [
        {
          label: '目录',
          value: 0,
        },
        {
          label: '菜单',
          value: 1,
        },
        {
          label: '权限',
          value: 2,
        },
        // {
        //   label: '路由界面',
        //   value: 3,
        // },
      ],
    },
  },
  {
    field: 'name',
    component: 'Input',
    label: '节点名称',
    rules: [{ required: true, type: 'string' }],
  },
  {
    field: 'REMARK',
    component: 'Input',
    label: '路由名称',
    rules: [{ required: true, type: 'string' }],
    helpMessage: ['路由名称', '应尽量与节点ID保持一致'],
    // vIf: ({ formModel }) => formModel['type'] == 1,
  },
  {
    field: 'parentId',
    component: 'TreeSelect',
    label: '上级节点',
    componentProps: {
      fieldNames: {
        label: 'name',
        value: 'id',
      },
      getPopupContainer: () => document.body,
    },
    dynamicDisabled: ({ formModel }) => formModel['type'] == 2,
    rules: [{ required: true, type: 'string' }],
  },
  {
    field: 'router',
    component: 'Input',
    label: '节点路由',
    vIf: ({ formModel }) => formModel['type'] !== 2,
    rules: [{ required: true, type: 'string' }],
  },
  // {
  //   field: 'role',//EMP_PERMS需要由api獲取
  //   label: '角色',
  //   component: 'Select',
  //   componentProps:  () => {
  //     return {
  //       options: RoleList.value,
  //       mode: 'multiple',
  //     }
  //   },
   
  //   vIf: ({ formModel }) => !!useFormStatus.isAddForm || formModel['type'] === 2,
  //   defaultValue:['Admin'],
  //   // rules: [{ required: true, message: '请选择权限' }],
  // },
  {
    field: 'viewPath',
    // component: 'Select',
    component: 'Input',
    label: '文件路径',
    vIf: ({ formModel }) => formModel['type'] === 1,
    // componentProps: {
    //   options: Object.keys(constantRouterComponents).map((n) => ({ label: n, value: n })),
    // },
    rules: [{ required: true, type: 'string' }],
  },
  {
    field: 'icon',
    component: () => IconsSelect,
    label: '节点图标',
    vIf: ({ formModel }) => formModel['type'] !== 2,

  },
  {
    field: 'id',
    component: 'Input',
    label: '节点ID',
    dynamicDisabled: ({ values }) => !useFormStatus.isAddForm && values != {},
    helpMessage: ['新建时请谨慎设置', '设定完成后不可编辑'],
  },
  {
    field: 'keepalive',
    component: 'Switch',
    label: '是否在页面显示',
    defaultValue: true,
    vIf: ({ formModel }) => formModel['type'] === 1,
  },
  {
    field: 'isShow',
    component: 'Switch',
    label: '是否在侧边显示',
    defaultValue: true,
    vIf: ({ formModel }) => formModel['type'] !== 2,
  },
  {
    field: 'orderNum',
    component: 'InputNumber',
    label: '排序号',
    defaultValue: 1110,
    dynamicDisabled: ({ formModel }) => !useFormStatus.isAddForm && formModel['type'] !== 2,
    componentProps: {
      style: {
        width: '100%',
      },
    },
  },
  {
    field: 'Owner',
    component: 'Input',
    label: 'Owner'
  },
  {
    field: 'Explain',
    component: 'Input',
    label: '説明文件',
    vIf: ({ formModel }) => formModel['name'] == 'DashBoard',
  }
];

export const explainSchemas: FormSchema[] = [
  {
    field: 'title',
    component: 'Input',
    label: '路由ID',
    rules: [{ required: true }],
    dynamicDisabled:true
  },
  {
    field: 'tinymce',
    component: 'Tinymce',
    label: '富文本编辑框',
    defaultValue: 'defaultValue',
    componentProps: ({ formModel, field }) => {
      return {
        onChange: (value: string) => {
          formModel[field] = value;
        }
      };
    }
  }
];
