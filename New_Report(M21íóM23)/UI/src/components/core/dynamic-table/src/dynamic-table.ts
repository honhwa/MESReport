import { tableProps } from 'ant-design-vue/es/table';
import DynamicTable from './dynamic-table.vue';
import { PropType, ExtractPropTypes, reactive } from 'vue';
import type { BookType } from 'xlsx';
//@ts-ignore
import type { LoadDataParams, TableColumn, OnChangeCallbackParams } from './types/';
import type { SchemaFormProps } from '@/components/core/schema-form';
import { isBoolean } from '@/utils/is';
import type { FormSchema } from '@/components/core/schema-form/';
import type { PageShowHidden } from './types/setting';
import { useWindowSize } from '@vueuse/core'
import { exceljsparams } from "@/utils/Excelljsutils";

const { width } = useWindowSize()
export const dynamicTableProps = {//外部(相对于.vue文件)定义props
  ...tableProps(),
  /** 是否显示搜索表单 */
  search: {
    type: Boolean as PropType<boolean>,
    default: false,
  },
  /** 是否显示说明页面 */
  explain: {
    type: Boolean as PropType<boolean>,
    default: true,
  },
  /** 是否显示编辑说明按钮(按权限显示) */
  // EditExplain: {
  //   type: Boolean as PropType<boolean>,
  //   default: false,
  // },
  /** 表单属性配置 */
  formProps: {
    type: Object as PropType<SchemaFormProps>,
    default: () => ({}),
  },
  /** 表格列配置 */
  columns: {
    type: Array as PropType<TableColumn<any>[]>,
    required: true,
    default: () => [],
  },
  /** 表格宽度控制 */
  PageParams: {
    type: Object as PropType<PageShowHidden>,
    default: () => ({}),
  },

//Owner顯示控制
 OwnerParams:{
    type: Boolean as PropType<boolean>,
    default: true,
  },
  //Owner和收合顯示控制
  OwnerFoldParams:{
    type: Boolean as PropType<boolean>,
    default: true,
  },
 
  /** 表格数据请求函数 */
  // dataRequest: {
  //   type: Object
  // },
  dataRequest: {
    // 获取列表数据函数API
    type: Function as PropType<
      (
        params?: LoadDataParams,
        onChangeParams?: OnChangeCallbackParams,
      ) => Promise<API.TableListResult>
    >,
  },
  /** 当前显示的数据 */
  dataSource: {
    type: Array
  },
  /** 所有数据 */
  dataAll: {
    type: Array
  },
  // dataRequest: {
  //   // 获取列表数据函数API
  //   type: Function as PropType<
  //     (
  //       params?: LoadDataParams,
  //       onChangeParams?: OnChangeCallbackParams,
  //     ) => Promise<API.TableListResult>
  //   >,
  // },
  /** 是否显示索引号 */
  showIndex: {
    type: Boolean as PropType<boolean>,
    default: false,
  },
  /** 索引列属性配置 */
  indexColumnProps: {
    type: Object as PropType<Partial<TableColumn>>,
    default: () => ({}),
  },
  /** 是否显示表格工具栏 */
  showToolBar: {
    type: Boolean as PropType<boolean>,
    default: true,
  },
  /** 是否显示表格设置 */
  showTableSetting: {
    type: Boolean as PropType<boolean>,
    default: false,
  },
  /** 表格标题 */
  headerTitle: String as PropType<string>,
  /** 表格标题提示信息 */
  titleTooltip: [Array, String] as PropType<string[] | string>,
  // excel导出配置
  /** 导出的文件名 */
  exportFileName: {
    type: String as PropType<string>,
  },
  /** xlsx的booktype */
  exportBookType: {
    type: String as PropType<BookType>,
    default: 'xlsx',
  },
  /** 导出的表头 */
  exportHeader: {
    type: Array as PropType<any[]>,
    default: () => [],
  },
  /** 收藏本頁 */
  showFavorite: {
    type: Boolean as PropType<boolean>,
    default: false,
  },
  flexwidth:{
    type:Number,
    default:0,
  },

  /** 导出表头方式 */
  exportType:{
    type:String as PropType<string>,
    default:'default'
  }, 
  /* 
  通過api獲取excell的參數
   */
  excellparams:{
    type:Object,
    default:null
  },

  /* 
  Exceljs的參數
   */
  exceljsparams:{
    type:Object as PropType<Partial<exceljsparams>>,
    default:null
  }

  
} as const;

export type DynamicTableProps = ExtractPropTypes<typeof dynamicTableProps>;//通过ExtractPropTypes将props值定义转为类型定义再导出
//但没有逆运算，导致在定义共有 props 组件 (props 继承) 的时候十分难受

export const dynamicTableEmits = {//外部定义emit
  change: (...rest: OnChangeCallbackParams) => rest.length === 4,
  'toggle-advanced': (isAdvanced: boolean) => isBoolean(isAdvanced),
};
export const ExplainSchemas: FormSchema[] = [
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

export type DynamicTableEmits = typeof dynamicTableEmits;

export type DynamicTableEmitFn = EmitFn<DynamicTableEmits>;
// @ts-ignore
export type DynamicTableInstance = InstanceType<typeof DynamicTable>;

// 默认支持的插槽
export const defaultSlots = [
  'emptyText',
  'expandIcon',
  'title',
  'footer',
  'summary',
  'expandedRowRender',
  'customFilterIcon',
  'customFilterDropdown',
] as const;

export let PageParams = reactive({
  TableSpan: width.value>1700?21:19,
  // TableSpan: 20,
  ShowMenu: true,
})



