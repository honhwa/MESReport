<script lang="ts" setup>
import { useTable } from '@/components/core/dynamic-table';
import { reactive, watchEffect, watch, computed } from 'vue';
import { GetCommon } from '@/api/pages/common';
import { ReportCommonWithOut } from '@/store/module/ReportCommon';
import { message } from 'ant-design-vue';

const useCommonParams = ReportCommonWithOut();
console.log("輸出vuex",useCommonParams);

function getImport(site: any) {
    const {
        detail1
    } = require(`@/pages/ReportPortal/FS/EntityStatusSummary/${site}/Table`);
    return { detail1};
}
let FactoryType:string = useCommonParams.Fab;
let { detail1} = getImport("LCM1");

let TableParams = useCommonParams["EntityStatusSummary"+FactoryType]["FirstTableParams"];

let PageSetting = reactive({
    moreData: [] as any,
    List: [] as any,
    Columns: detail1,
    TopTitle: '',
});
const [DynamicTable] = useTable();

async function GetDetailData(TableParams: any) {
    let formParams = useCommonParams["EntityStatusSummary"+FactoryType]["FirstTableParams"];
    let { api01 } = await GetCommon(formParams);
    let pushdata = api01['Data0']['Table'];
    if(pushdata.length){
        PageSetting.List = pushdata;

    }else{
        return message.warn("無數據")
    }
    PageSetting.TopTitle = 'LCMA Entity Status Detail ';
}

watchEffect(() => {
    //一阶表格监控
    TableParams = useCommonParams["EntityStatusSummary"+FactoryType]["FirstTableParams"];
    GetDetailData(TableParams);
});


</script>


<template>
    <div>
        <a-card>
                <DynamicTable
                    row-key="id"
                    :header-title="PageSetting.TopTitle"
                    :columns="PageSetting.Columns"
                    :key="PageSetting.TopTitle"
                    bordered
                    size="small"
                    :flexwidth="1"
                    sticky
                    :data-source="PageSetting.List"
                    :data-all="PageSetting.List"
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