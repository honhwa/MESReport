<template>
    <div>
        <a-card>
                <DynamicTable
                    row-key="id"
                    :header-title="PageSetting.TopTitle"
                    :columns="PageSetting.Columns"
                    :key="PageSetting.TopTitle"
                    bordered :explain="false"
                    size="small"
                    sticky
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
import { reactive, watchEffect, watch, computed } from 'vue';
import { GetCommon } from '@/api/pages/common';
import { ReportCommon } from '@/store/module/ReportCommon';
import { AddCommonPageParams } from '@/store/module/commonpage';


let useCommonParams = AddCommonPageParams();
let useReportCommon = ReportCommon();
let ReportNameParams=`EntityStatusSummary-${useCommonParams.Fab}`;
let bigfab=useCommonParams.Fab.slice(0,useCommonParams.Fab.length-1)

let tableParams = useReportCommon[ReportNameParams].FirstTableParams;
let formParams= useReportCommon[ReportNameParams].FormParams;
let APIMessage=JSON.parse(useReportCommon[ReportNameParams].APIMessage);

function getImport(site: any) {
    const {detail1} = require(`@/pages/ReportPortal/NGB/EntityStatusSummary/${bigfab}/Table`);
    const { GetDetailData } = require(`@/pages/ReportPortal/NGB/EntityStatusSummary/${bigfab}/RenderFunction`);
    return { detail1,GetDetailData};
}


let { detail1,GetDetailData} = getImport(useCommonParams.Fab);

let TableParams = useCommonParams.FirstTableParams;
let PageSetting = reactive({
    moreData: [] as any,
    List: [] as any,
    Columns: detail1,
    TopTitle: '',
});
const [DynamicTable] = useTable();



watchEffect(() => {
    //一阶表格监控
    tableParams = useReportCommon[ReportNameParams].FirstTableParams;
    formParams= useReportCommon[ReportNameParams].FormParams;
    APIMessage=JSON.parse(useReportCommon[ReportNameParams].APIMessage);
    GetDetailData(tableParams,APIMessage,formParams,PageSetting);
});


</script>


