import { computed, ref, unref, watchEffect } from 'vue';
import { omit } from 'lodash-es';
import { Table } from 'ant-design-vue';
import type { Slots } from 'vue';
import type { VexVirtualTableProps } from '../vex-virtual-table';
import type { TableProps } from 'ant-design-vue';
import type { TableColumn } from '../types/column';
import { DynamicColumnWidth } from '@/pages/ReportPortal/utils/common';
import { useWindowSize } from '@vueuse/core';
import { getRowClassNmaeVxe } from '@/pages/ReportPortal/utils/common';

const { width, height } = useWindowSize()
export type Pagination = TableProps['pagination'];
export type TableState = ReturnType<typeof useTableState>;

export type UseTableStateParams = {
    props: VexVirtualTableProps;
    slots: Slots;
};

export const useTableState = ({ props, slots }: UseTableStateParams) => {
    const tableRef = ref<InstanceType<typeof Table>>();
    // 表格加载
    const tableData = ref<any[]>([]);
    const tableExportData = ref<any[]>([]);

    const innerPropsRef = ref<Partial<VexVirtualTableProps>>();

    const getProps = computed(() => {
        return { ...props, ...unref(innerPropsRef) };
    });

    const getBindValues = computed(() => {
        const props = unref(getProps);

        (props.columns as TableColumn[]).forEach((n) => {
            if (!n.field) n.field = n.dataIndex ?? ''
            if (!n.className?.includes('addborder')) n.className = `addborder ${n.className}`;
        });
        if (!props.headerRowClassName) {
            props.headerRowClassName = 'AddHeaderRowClass'
        }
        let mergeCells= [
            { row: 4, col: 2, rowspan: 3, colspan: 1 },
            { row: 30, col: 3, rowspan: 10, colspan: 1 },
          ]

          
        //因为VxeTable的应用场景是虚拟滚动，所以自动带上以下参数
        let propsData: Recordable = {
            ...props,
            scrollX:{ enabled: true },//开启横向虚拟滚动
            scrollY:{ enabled: true },//开启竖向虚拟滚动
            showOverflow:true,//设置所有内容过长时显示为省略号
            rowClassName:getRowClassNmaeVxe,//设置行class
            exportType:"Multi",//设置导出方式为多级表头导出
            columnConfig:{ resizable: true },//可拖动列
            headerRowClassName:"VxeTableaHeader"
            // mergeCells:mergeCells//表格合并
        };
        propsData = omit(propsData, ['class', 'onChange']);
        let TableWidth = ref(10);
        // 是否使用動態列寬
        if (props.flexwidth && props.flexwidth != 0) {
            DynamicColumnWidth(props.columns as any[], props.tableData as any[], props.flexwidth, TableWidth);
            // TableWidth.value = Math.round(TableWidth.value)
            // propsData.style={width:TableWidth.value>100?TableWidth.value+'px':'100%','overflow-y':'auto',height:`${height.value-140}px`,'overflow-x':'hidden'}
            // propsData.height = height.value-100;
            // propsData = omit(propsData,TableWidth.value+220>width.value?'style':'scroll');
        }
        return propsData;
    });

    // 如果外界设置了dataSource，那就直接用外界提供的数据
    watchEffect(() => {
        if (props.tableData) {
            tableData.value = props.tableData;
            tableExportData.value = props.tableData
        }

    });

    return {
        tableRef,
        tableData,
        innerPropsRef,
        getProps,
        getBindValues,
        tableExportData
    };
};
