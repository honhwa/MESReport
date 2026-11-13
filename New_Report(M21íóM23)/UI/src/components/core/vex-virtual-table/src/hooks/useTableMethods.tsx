import { unref } from 'vue';
import { isObject, isString } from 'lodash-es';
import type { VNode } from 'vue';
import type { VexVirtualTableProps,VxeTableEmits } from '../vex-virtual-table';
import type { OnChangeCallbackParams, TableColumn } from '../types/';
import type { TableState } from './useTableState';

export type TableMethods = ReturnType<typeof useTableMethods>;

export type UseTableMethodsContext = {
  state: TableState;
  props: VexVirtualTableProps;
  // emit:VxeTableEmits
};

export const useTableMethods = ({ state, props }: UseTableMethodsContext) => {
  const { innerPropsRef, tableData } = state;

  const setProps = (props: Partial<VexVirtualTableProps>) => {
    innerPropsRef.value = { ...unref(innerPropsRef), ...props };
  };

  const getComponent = (comp: VNode | string) => {
    return isString(comp) ? <>{comp}</> : comp;
  };

  /**
   * @description 表格查询
   */
  const handleSubmit = (params: any, page = 1) => {
    params.page = page;
    fetchData(params);
  };

  /**
   * @param {object} params 表格查询参数
   * @param {boolean} flush 是否将页数重置到第一页
   * @description 获取表格数据
   */
  //@ts-ignore
  const fetchData = async (params = {}, rest?: OnChangeCallbackParams): Promise<any> => {
    // 如果用户没有提供dataSource并且dataRequest是一个函数，那就进行接口请求
    if (
      Object.is(props.tableData, undefined) &&
      Object.prototype.toString.call(props.dataRequest).includes('Function')
    ) {
      // 是否启用了分页
      const queryParams = {
        ...params,
      };
   
      const data = await props?.dataRequest?.(queryParams, rest)
      // const data = props?.dataRequest
      if (Array.isArray(data?.list)) {
        tableData.value = data!.list;
      } else if (Array.isArray(data)) {
        tableData.value = data;
      } else {
        tableData.value = [];
      }
      

        
    }
    
    return tableData;
  };

  /**
   * @description 刷新表格
   */
  const reload = async (resetPageIndex = false) => {
    
    return await fetchData();
  };

  /**
   * @description 分页改变
   */


  // dataIndex 可以为 a.b.c
  // const getDataIndexVal = (dataIndex, record) => dataIndex.split('.').reduce((pre, curr) => pre[curr], record)

  // 获取表格列key
  const getColumnKey = (column: TableColumn) => {
    return column?.key || column?.field;
  };

  /**
   * @description当外部需要动态改变搜索表单的值或选项时，需要调用此方法获取dynamicFormRef实例
   */

  return {
    setProps,
    getComponent,
    handleSubmit,
    getColumnKey,
    fetchData,
    reload,
  };
};
