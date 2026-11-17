<template>
    <div>
        <a-card>
            <DynamicTable
                row-key="id"
                :header-title="PageSetting.TopTitle"
                sticky
                :columns="PageSetting.Columns"
                :key="PageSetting.Columns"
                bordered
                size="small"
                :data-source="PageSetting.List"
                :pagination="false"
                export-file-name="导出"
            >
                <template #export-button>
                    <a-button type="success"> 导出 </a-button>
                </template>
            </DynamicTable>
        </a-card>
    </div>
</template>

<script lang="ts" setup>
import { useTable } from '@/components/core/dynamic-table';
import { reactive, watchEffect } from 'vue';
import { GetCommon } from '@/api/pages/common';
import { ReportCommon } from '@/store/module/ReportCommon';
import {ZMFGOpDetailColumns} from './LCM1/Table'

let useCommonParams = ReportCommon();
let TableParams = useCommonParams['ZMFGOperatorLCM1'].FirstTableParams;


let PageSetting = reactive({
    moreData: [],
    List: [] as any,
    Columns: ZMFGOpDetailColumns,
    TopTitle: 'LCMA OQC 品質推移圖 Detail'
    // BottomTitle:''
});
const [DynamicTable] = useTable();

async function GetDetailData(TableParams: any) {
    let APIParams: any = useCommonParams['ZMFGOperatorLCM1'].APIMessage;
    let formParams = useCommonParams['ZMFGOperatorLCM1'].FormParams;
    let baseParams = {
        apiName: 'ZMFGOperatorDetail',
        FactoryType: 'LCM1',
        G_FAC: 'A',
        StrSql_w: APIParams.StrSql_w,
        StrSql_w1:APIParams.StrSql_w1,
        QType: formParams['QType'],
        operators:TableParams['QITEM']
    };
    let {api01} =await GetCommon(baseParams)
    console.log(api01,TableParams['QITEM']);
    PageSetting.List = api01['Data']['Table']
    // List
}




watchEffect(() => {
    //一阶表格监控
    TableParams = useCommonParams['ZMFGOperatorLCM1'].FirstTableParams;

    GetDetailData(TableParams);
});
</script>
