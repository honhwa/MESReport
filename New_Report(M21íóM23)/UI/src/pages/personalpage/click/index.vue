<template>
    <div>
        <SchemaForm @submit="loadTableData">
            <template #remoteSearch="{ formModel, field }">
                <ApiSelect
                    v-model:value="formModel[field]"
                    :api="optionsListApi"
                    show-search
                    option-filter-prop="label"
                    result-field="Table"
                    label-field="SYSMODULE"
                    value-field="SYSMODULE"
                   
                />
            </template>
        </SchemaForm>
        <DynamicTable
            header-title="點擊率記錄"
            show-index
            :data-source="ClickData"
            :columns="baseColumns"
            :pagination="false"
            :scroll="{ y: 500 }"
        >
        </DynamicTable>
    </div>
</template>

<script setup lang="ts">
import { useForm, ApiSelect } from '@/components/core/schema-form';
import { userSchemas } from './formSchemas';
import { baseColumns } from './columns';
import { useTable } from '@/components/core/dynamic-table';
import { GetCommon } from '@/api/pages/common';
import { ref } from 'vue';

// defineOptions({
//   name: 'SystemUser',
// });
const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas: userSchemas,
    actionColOptions: {
        span: 24
    },
    layout: 'horizontal'
});
const [DynamicTable, dynamicTableInstance] = useTable();
let ClickData = ref([]);

async function loadTableData(e) {
    const { api01 } = await GetCommon({
        apiName: 'NGBAP_Number_Of_Hits_Api_Report',
        FactoryType: e['FactoryType'] ?? 'LCM1',
        FromDate: e['FromDate'] ?? '',
        ToDate: e['ToDate'] ?? ''
    });
    ClickData.value = api01['Data']['Table'];
}
async function optionsListApi() {
    const { api01 } = await GetCommon({
        apiName: 'NGBAP_Number_Of_Hits_Api_ShowFactory',
        SysName: `${process.env.VUE_APP_SITE}Report`
    });
    return {api01:api01['Data']};
}
</script>

<style></style>
