import { HTMLAttributes, computed, ref, shallowRef, unref, watchEffect } from 'vue';
import { isBoolean, omit } from 'lodash-es';
import { Table } from 'ant-design-vue';
import type { Slots } from 'vue';
import type { DynamicTableProps } from '../dynamic-table';
import type { SchemaFormInstance } from '@/components/core/schema-form';
import type { TableProps } from 'ant-design-vue';
import type { TableColumn } from '../types/column';
import { DynamicColumnWidth } from '@/pages/ReportPortal/utils/common';
import { useWindowSize } from '@vueuse/core';
import type { ComponentPublicInstance } from 'vue';

const { width, height } = useWindowSize();

export type Pagination = TableProps['pagination'];
export type TableState = ReturnType<typeof useTableState>;

export type UseTableStateParams = {
  props: DynamicTableProps;
  slots: Slots;
};

export const useTableState = ({ props, slots }: UseTableStateParams) => {
  const tableRef = ref<InstanceType<typeof Table>>();
  //   const tableRef = shallowRef<ComponentPublicInstance<typeof Table>>()
  // 表格加载
  const loadingRef = ref<boolean>(!!props.loading);
  const tableData = ref<any[]>([]);
  const tableExportData = ref<any[]>([]);
  const queryFormRef = ref<SchemaFormInstance>();

  const innerPropsRef = ref<Partial<DynamicTableProps>>();

  // 分页配置参数
  const paginationRef = ref<NonNullable<Pagination>>(false);

  if (!Object.is(props.pagination, false)) {
    paginationRef.value = {
      current: 1,
      pageSize: 7,
      total: 0,
      pageSizeOptions: ['7', '15', '30', '100'],
      showQuickJumper: true,
      showSizeChanger: true, // 显示可改变每页数量
      showTotal: (total) => `共${total}条数据`, // 显示总数
      // onChange: (current, pageSize) => pageOption?.pageChange?.(current, pageSize),
      // onShowSizeChange: (current, pageSize) => pageOption?.pageChange?.(current, pageSize),
      ...props.pagination,
    };
  }

  const getProps = computed(() => {
    return { ...props, ...unref(innerPropsRef) };
  });

  const getBindValues = computed(() => {
    const props = unref(getProps);
    const columns = (props.columns as TableColumn[]).filter((n) => !n.hideInTable);
    columns.forEach((n) => {
      if (!n.className?.includes('addborder') && !props.dataRequest)
        n.className = `addborder ${n.className}`;
    });
    // 是否添加序号列
    if (props.showIndex) {
      columns.unshift({
        dataIndex: '$$index',
        title: '序号',
        width: 60,
        align: 'center',
        fixed: 'left',
        ...props.indexColumnProps,
        bodyCell: ({ index }) => {
          const getPagination = unref(paginationRef);
          if (isBoolean(getPagination)) {
            return <>{`${index + 1}`}</>;
          }
          const { current = 1, pageSize = 10 } = getPagination!;
          return <>{((current < 1 ? 1 : current) - 1) * pageSize + index + 1}</>;
        },
      });
    }
    let propsData: Recordable = {
      ...props,
      columns,
      rowKey: props.rowKey ?? 'id',
      loading: unref(loadingRef),
      // tableLayout: 'auto',
      pagination: unref(paginationRef), //设置分页
    };
    if (slots.expandedRowRender) {
      propsData = omit(propsData, 'scroll');
    }
    let TableWidth = ref(10);
    // 是否使用動態列寬
    try {
      if (props.flexwidth && props.flexwidth != 0) {
        DynamicColumnWidth(
          props.columns as any[],
          props.dataAll as any[],
          props.flexwidth,
          TableWidth,
        );
        TableWidth.value = Math.round(TableWidth.value);
        propsData.style = {
          width: TableWidth.value > 100 ? TableWidth.value + 'px' : '100%',
          overflow: 'auto',
          height: `${height.value - 140}px`,
        };
        let headertrlength = 1;
        if (tableRef.value?.$el) {
          // ant design vue表頭和表體是兩個不同的table
          let headerdom = tableRef.value?.$el.querySelectorAll('.ant-table-thead tr');
          headertrlength = headerdom.length;
        }
        propsData.scroll = {
          x: TableWidth.value,
          y: height.value - 160 - document.documentElement.clientHeight * 0.008 * headertrlength,
        };
        propsData = omit(propsData, TableWidth.value + 220 > width.value ? 'style' : 'scroll');
      }
    } catch (error) {
      console.log(error);
    }

    propsData = omit(propsData, ['class', 'onChange']);
    return propsData;
  });

  // 如果外界设置了dataSource，那就直接用外界提供的数据
  watchEffect(() => {
    if (props.dataSource) {
      tableData.value = props.dataSource;
    }
    if (props.dataAll) {
      tableExportData.value = props.dataAll;
    } else if (props.dataSource) {
      tableExportData.value = props.dataSource;
    }
  });

  return {
    tableRef,
    loadingRef,
    tableData,
    queryFormRef,
    innerPropsRef,
    getProps,
    getBindValues,
    paginationRef,
    tableExportData,
  };
};
