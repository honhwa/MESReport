<template>
    <div class="ProductionProgressSummary">
        <Loading
            :loading="compState.loading"
            :absolute="compState.absolute"
            :theme="compState.theme"
            :background="compState.background"
            :tip="compState.tip"
        />
        <a-card>
            <a-row>
                <a-col :span="24 - PageSetting.TableSpan" class="ScrollForm" v-show="PageParams.ShowMenu">
                    <SchemaForm @submit="confirm"> </SchemaForm>
                </a-col>
                <a-col :span="PageSetting.TableSpan" class="ScrollTable">
                    <DynamicTable
                        row-key="id"
                        header-title="Production Progress Summary"
                        :columns="PageSetting.TopColumns"
                        :key="PageSetting.TopColumns"
                        bordered
                        size="small"
                        :data-source="PageSetting.TopList"
                        :pagination="false"
                        :scroll="{ x: 800, y: 350 }"
                        export-file-name="Export"
                        :row-class-name="getRowClassNmae"
                        :PageParams="PageParams"
                    >
                        <template #export-button>
                            <a-button type="success"> Export </a-button>
                        </template>
                    </DynamicTable>
                    <DynamicTable
                        row-key="id"
                        header-title="Production Progress Detail"
                        :columns="PageSetting.BottomColumns"
                        :key="PageSetting.BottomColumns"
                        bordered
                        size="small"
                        :data-source="PageSetting.BottomList"
                        :pagination="false"
                        :scroll="{ x: 1200, y: 350 }"
                        export-file-name="Export"
                        :row-class-name="getRowClassNmae"
                        :PageParams="PageParams"
                    >
                        <template #export-button>
                            <a-button type="success"> Export </a-button>
                        </template>
                    </DynamicTable>
                </a-col>
            </a-row>
        </a-card>
    </div>
</template>

<script lang="ts" setup>
import { PageParams } from '@/components/core/dynamic-table/src/dynamic-table';
import { MenuFoldOutlined, MenuUnfoldOutlined } from '@ant-design/icons-vue';
import { useForm } from '@/components/core/schema-form';
import { useTable } from '@/components/core/dynamic-table';
import { reactive } from '@vue/reactivity';
import { onMounted } from '@vue/runtime-core';
import { getRowClassNmae, mergeTableRow } from '@/pages/ReportPortal/utils/common';
import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { Loading, useLoading } from '@/components/feat/Loading';
import { GetCommon } from '@/api/pages/common';

// defineOptions({
//   name: 'DemosFormDynamicForm',
// });

const UseFormParamsAdd = AddCommonPageParams();

const compState = reactive({
    absolute: true,
    loading: false,
    theme: 'dark',
    background: 'rgba(111,111,111,.7)',
    tip: '加载中...'
});

const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});

function getImport(site: any) {
    const { schemas, optionsdata } = require(`@/pages/ReportPortal/TN/ProductionProgressSummary/${site}/form-schema`);
    const {topBaseColumns,bottomBaseColumns} = require(`@/pages/ReportPortal/TN/ProductionProgressSummary/${site}/Table`);
    return { schemas, topBaseColumns, bottomBaseColumns, optionsdata };
}
const { schemas, topBaseColumns, bottomBaseColumns, optionsdata } = getImport('MOD');
const { mounted, mountedowner, mountedOper } = useRemoteSelect();

let PageSetting = reactive({
    TopList: [] as any,
    BottomList: [] as any,
    TableSpan: 20,
    ShowMenu: true,
    TopColumns: [] as any,
    BottomColumns: [] as any
});
PageSetting.TopColumns = topBaseColumns;
PageSetting.BottomColumns = bottomBaseColumns;
const [DynamicTable] = useTable();
const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas,
    actionColOptions: {
        span: 24
    },
    layout: 'vertical'
});

function confirm(values) {
    openFullLoading();
    let baseParams = { name: 'Date', apiName: 'TN_ProductionProgressSummary', FactoryType: UseFormParamsAdd.Fab };
    Object.keys(values).forEach((key) => {
        if (!values[key]) {
            values[key] = '';
        }
    });
    dynamicFormRef.validate().then(async () => {
        const {Date: {Data: { Table: Data_Summary }, Version ,SDate,EDate,Sql:Sql_Summary}} = await GetCommon({...baseParams, ...values});
        topBaseColumns[0]['children'].title = Version;
        topBaseColumns[1].title = "MTD("+SDate+"~"+EDate+")";
        topBaseColumns[2].title = EDate;
        PageSetting.TopColumns = [...topBaseColumns];
        Data_Summary.forEach(item => {item.OPER = item.OPER.substring(2)})
        //add input => Input=Bonding
        let bonding = Data_Summary.filter(x=>x.OPER == "Bonding")[0];
        if(bonding){
            let input:any = {};
            Object.assign(input,bonding)
            input.OPER = "Input";
            PageSetting.TopList = [input,...Data_Summary];
        }else{
            PageSetting.TopList = [...Data_Summary];
        }

        baseParams.apiName = 'TN_ProductionProgressSummaryDetail';
        const {Date: {Data: { Table: Data_Detail },Sql:Sql_Detail}} = await GetCommon({...baseParams, ...values});
        bottomBaseColumns[2].title = "MTD Input("+SDate+"~"+EDate+")";
        bottomBaseColumns[3].title = "MTD Onput("+SDate+"~"+EDate+")";
        PageSetting.BottomColumns = [...bottomBaseColumns];
        PageSetting.BottomList = getTableData(Data_Detail);

        closeFullLoading();
    });
}

function getTableData(Data: any[]) {
    /**表格合并所需参数
     * @data 数据源
     * @mergeColNames 需要合并的列
     * @firstMergeColNames 受到影响的列
     * @firstMerge 以哪列为基础进行合并
     */
    let combineCol1: any = {
        data: Data,
        mergeColNames: ['PROD_NBR'],
        firstMergeColNames: ['PROD_NBR'],
        firstMerge: 'PROD_NBR'
    };
    let combineCol2: any = {
        data: mergeTableRow(combineCol1),
        mergeColNames: ['CUSTOMER'],
        firstMergeColNames: ['CUSTOMER'],
        firstMerge: 'CUSTOMER'
    };
    return mergeTableRow(combineCol2);
}

function showHidden() {
    PageSetting.ShowMenu = !PageSetting.ShowMenu;
    if (PageSetting.ShowMenu) PageSetting.TableSpan = 20;
    else PageSetting.TableSpan = 24;
}

onMounted(async () => {
    await mounted(optionsdata, UseFormParamsAdd.Fab);
});
</script>

<style lang="less" scoped>
.btn-rows {
    button {
        margin-right: 12px;
    }
}

.ProductionProgressSummary :deep(.v-center) {
    background-color: #f0f0f0;
}

.ProductionProgressSummary :deep(.Yellow) {
    background-color: #80ff80;
}

.ProductionProgressSummary :deep(.YellowGreen) {
    background-color: #f39230;
}
</style>
