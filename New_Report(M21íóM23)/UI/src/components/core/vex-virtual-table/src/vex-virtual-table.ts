import { VxeTableConstructor, VxeTableProps, VxeTable } from 'vxe-table/types';
import vxeProps from 'vxe-table/es/table/src/props';
import { VxeTablePropTypes } from 'vxe-table/types/table'
import { PropType, ExtractPropTypes, reactive } from 'vue';
import type { PageShowHidden, VexTableColumn } from './types/setting';
import { exceljsparams } from '@/utils/Excelljsutils';
import VexTable from './vex-virtual-table.vue'
import type { LoadDataParams, TableColumn, OnChangeCallbackParams } from './types/';
import { isBoolean } from 'lodash-es';
import type { BookType } from 'xlsx';

function propsFromInterface<T extends Record<string, any>>(intf: T): Record<keyof T, { type: PropType<any>; default?: any }> {

  const props: Record<keyof T, { type: PropType<any>; default?: any }> = {} as any;
  for (const [key, value] of Object.entries(intf)) {
    props[key as keyof T] = { type: value.type, default: value.default };
  }
  return props
}
//vxetable未提供props，從VxeTableProps轉為props，避免手寫
const vexTable = propsFromInterface<VxeTableProps>(vxeProps)
// const vexTable={
//   props:propsFromInterface<VxeTableProps>({height:{type:number,default:700}})
// }

export const vexVirtualTableProps = {
  ...vexTable,
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
  dataRequest: {
    // 获取列表数据函数API
    type: Function as PropType<
      (
        params?: LoadDataParams,
        onChangeParams?: OnChangeCallbackParams,
      ) => Promise<API.TableListResult>
    >,
  },
  flexwidth:{
    type:Number,
    default:0,
  },
  showToolBar: {
    type: Boolean as PropType<boolean>,
    default: true,
  },
  headerTitle: String as PropType<string>,
  titleTooltip: [Array, String] as PropType<string[] | string>,
  // excel导出配置
  /** 导出的文件名 */
  exportFileName: {
    type: String as PropType<string>,
    default:'导出'
  },
  showFavorite: {
    type: Boolean as PropType<boolean>,
    default: true,
  },
  showIndex: {
    type: Boolean as PropType<boolean>,
    default: false,
  },
  PageParams: {
    type: Object as PropType<PageShowHidden>,
    default: () => ({}),
  },
  //Owner顯示控制
  OwnerParams: {
    type: Boolean as PropType<boolean>,
    default: true,
  },
  //Owner和收合顯示控制
  OwnerFoldParams: {
    type: Boolean as PropType<boolean>,
    default: true,
  },
  /** 是否显示表格设置 */
  showTableSetting: {
    type: Boolean as PropType<boolean>,
    default: false,
  },
  /** 是否显示说明页面 */
  explain: {
    type: Boolean as PropType<boolean>,
    default: true,
  },
  columns: {
    type: Array as PropType<VexTableColumn[]>
  },
  tableData: {
    type: Array as PropType<any[]>,
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
}
export const vxeTableEmits = {
  'toggle-advanced': (isAdvanced: boolean) => isBoolean(isAdvanced),
}
export type VxeTableEmits = typeof vxeTableEmits
//@ts-ignore
export type VexTableInstance = InstanceType<typeof VexTable>;
export type VexVirtualTableProps = ExtractPropTypes<typeof vexVirtualTableProps>;

