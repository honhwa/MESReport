<template>
    <div class='Report'>
        <a-card>
            <a-row>
                <a-col :span='24 - PageParams.TableSpan' class='ScrollForm'>
                    <SchemaForm @submit='confirm'> </SchemaForm>
                </a-col>
                <a-col :span='PageParams.TableSpan'>
                    <div ref='report'>
                        <VexTable :PageParams="PageParams" export-type="Exceljs" :header-title="PageSetting.Title"
                            :columns="PageSetting.Columns" :key="PageSetting.Columns" :tableData="PageSetting.moreData"
                            :data="PageSetting.moreData" :export-file-name="PageSetting.Title" :flexwidth="10"
                            :mergeCells="PageSetting.mergecellData">
                            <template #export-button>
                                <a-button type="success"> 导出 </a-button>
                            </template>
                        </VexTable>
                    </div>
                </a-col>
            </a-row>
        </a-card>
    </div>
</template>

<script lang="tsx" setup>
import { PageParams } from '@/components/core/dynamic-table/src/dynamic-table'
import { useLoading } from '@/components/feat/Loading';
import { GetCommon } from '@/api/pages/common';
import { cloneDeep } from 'lodash';
import { onMounted } from '@vue/runtime-core';
import { reactive, ref } from '@vue/reactivity';
import { useForm } from '@/components/core/schema-form';
import { ReportCommon } from '@/store/module/ReportCommon';
import { message } from 'ant-design-vue';
import VexTable from '@/components/core/vex-virtual-table/src/vex-virtual-table.vue';
import { TableRendering } from '../../utils/common';

const UseParamsAdd = ReportCommon()
let FactoryType: string = UseParamsAdd.Fab;
const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});
const report = ref<HTMLElement | null>(null);
function getImport(site: string) {
    const { schemas, optionsdata } = require(`@/pages/ReportPortal/FS/WorkOrderDeploymentPlan/${site}/form-schema`);
    const { baseColumns } = require(`@/pages/ReportPortal/FS/WorkOrderDeploymentPlan/${site}/table`);
    return { schemas, optionsdata, baseColumns };
}
let { schemas, optionsdata, baseColumns } = getImport(FactoryType);

let PageSetting = reactive<BasePageParams.BasePageSetting>({
    List: [] as any,
    Columns: [] as any,
    Title: '',
    moreData: [],
    SQLHidden: '',
    APIColumns: [] as any,
});
switch (FactoryType) {
    case 'LCM2':
        PageSetting.Title = '工單展開計劃 Report';
        break;
    case 'BLU2':
        PageSetting.Title = '工單展開計劃 Report';
        break;
    case 'SET2':
        PageSetting.Title = '工單展開計劃 Report';
        break;
}
const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas: schemas(FactoryType),
    actionColOptions: {
        span: 24
    },
    layout: 'vertical'
});


async function confirm(values) {
    openFullLoading();
    let baseParams = {
        name: 'Data',
        apiName: 'WorkOrderDeploymentPlan',
        G_FAC: 'A',
        FactoryType
    };
    Object.keys(values).forEach((key) => {
        if (!values[key]) {
            values[key] = '';
        }
    });


    try {
        let { Data: {
            Data0: { Table: Data },
            title1,
            title2
        } } = await GetCommon({ ...baseParams, ...values });
        if (Data.length == 0) {
            PageSetting.moreData = [];
            closeFullLoading();
            return message.warn('无数据!');
        }

        let temp1: Object[] = [];
        Object.keys(Data[0]).forEach((item) => {
            temp1.push(item);
        });

        let temp: Object[] = TableRendering(title1, title2, temp1, true, [0]);
        PageSetting.moreData = Data;
        PageSetting.Columns = temp;
        closeFullLoading();
    } catch (error) {
        console.log(error)
        closeFullLoading();
    }

}

onMounted(async () => {
})
</script>

<style lang="less" scoped></style>