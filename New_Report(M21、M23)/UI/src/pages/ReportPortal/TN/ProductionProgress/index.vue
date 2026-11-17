<template>
    <div class="ProductionProgress">
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
                        header-title="Production Progress"
                        :columns="PageSetting.Columns"
                        :key="PageSetting.Columns"
                        bordered
                        size="small"
                        :data-source="PageSetting.List"
                        :pagination="false"
                        :scroll="PageSetting.TableSize"
                        export-file-name="Export"
                        :row-class-name="getRowClassName_temp"
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
    const { schemas, optionsdata } = require(`@/pages/ReportPortal/TN/ProductionProgress/${site}/form-schema`);
    const {aheadColumns,rearColumns} = require(`@/pages/ReportPortal/TN/ProductionProgress/${site}/Table`);
    return { schemas, aheadColumns,rearColumns, optionsdata };
}
const { schemas, aheadColumns,rearColumns, optionsdata } = getImport('MOD');
const { mounted } = useRemoteSelect();

let PageSetting = reactive({
    List: [] as any,
    TableSpan: 20,
    ShowMenu: true,
    Columns: [] as any,
    TableSize:{x:1000,y:750}
});
PageSetting.Columns = [...aheadColumns,...rearColumns];
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
    let baseParams = { name: 'Date', apiName: 'TN_ProductionProgress', FactoryType: UseFormParamsAdd.Fab };
    Object.keys(values).forEach((key) => {
        if (!values[key]) {
            values[key] = '';
        }
    });
    dynamicFormRef.validate().then(async () => {
        const {Date: {Data: { Table: Data } ,Sql:Sql}} = await GetCommon({...baseParams, ...values});
        let s = new Date(values.sDate);
        let e = new Date(values.eDate);
        let dtColumns = new Array();
        do{
            let dt = new Date(s).toISOString().split("T")[0]
            dtColumns.push(
                {
                    title: dt,
                    align: 'center',
                    dataIndex: dt,
                    className: 'pro_auto-center'
                }
            );
            s.setDate(s.getDate()+1)
        }while(e>=s)
        PageSetting.Columns = [...aheadColumns,...dtColumns,...rearColumns]; //vue3或者adv无法监测push,shift等方法
        PageSetting.List = getTableData(Data);
        PageSetting.TableSize.x = 850 + (dtColumns.length * 100);
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
        mergeColNames: ['SIZE1'],
        firstMergeColNames: ['SIZE1'],
        firstMerge: 'SIZE1'
    };
    let combineCol2: any = {
        data: mergeTableRow(combineCol1),
        mergeColNames: ['PROD_NBR'],
        firstMergeColNames: ['PROD_NBR'],
        firstMerge: 'PROD_NBR'
    };
    let combineCol3: any = {
        data: mergeTableRow(combineCol2),
        mergeColNames: ['MPC_COMMENT'],
        firstMergeColNames: ['MPC_COMMENT'],
        firstMerge: 'MPC_COMMENT'
    };
    
    return mergeTableRow(combineCol3);
}

function showHidden() {
    PageSetting.ShowMenu = !PageSetting.ShowMenu;
    if (PageSetting.ShowMenu) PageSetting.TableSpan = 20;
    else PageSetting.TableSpan = 24;
}

onMounted(async () => {
    await mounted(optionsdata, UseFormParamsAdd.Fab);
    let txtPlanVersion:any = document.getElementById("form_item_PlanVersion") || {};
    txtPlanVersion.value = optionsdata.TN_PlanVersion
});



function getRowClassName_temp(record: Object) {
    if (record && Object.keys(record).length != 0) {
        for (const [key, value] of Object.entries(record)) {
            if (['合計'].includes(value)) {
                return 'YellowGreen';
            }else if(['MPS Diff','MPC Diff'].includes(value)){
                return 'Yellow';
            }
        }
    }
}
</script>

<style lang="less" scoped>
.btn-rows {
    button {
        margin-right: 12px;
    }
}

.ProductionProgress :deep(.v-center) {
    background-color: #f0f0f0;
}

.ProductionProgress :deep(.Yellow) {
    background-color: #ffff00;
}

.ProductionProgress :deep(.YellowGreen) {
    background-color: #f39230;
}
</style>
