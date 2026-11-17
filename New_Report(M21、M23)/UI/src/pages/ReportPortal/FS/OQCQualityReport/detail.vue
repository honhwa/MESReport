<script lang="ts" setup>
import { useTable } from '@/components/core/dynamic-table';
import { reactive, watchEffect } from 'vue';
import { GetCommon } from '@/api/pages/common';
import { ReportCommon } from '@/store/module/ReportCommon';
import { message } from 'ant-design-vue';

const UseParamsAdd = ReportCommon();
let FactoryType: string = UseParamsAdd.Fab;




let PageSetting = reactive({
    moreData: [] as any,
    List: [] as any,
    Columns: [] as any,
    TopTitle: 'OQC 重缺風險批',
});
const [DynamicTable] = useTable();

async function GetDetailData(TableParams: any) {
    let baseParams = {
        apiName: 'OQCQuality_ADetail2',
        FactoryType: FactoryType,
        G_FAC: UseParamsAdd.G_FAC_COM,
        pnl_id:TableParams['pnl_id']

    };
    let { api01 } = await GetCommon(baseParams);
    let pushdata = api01['Data']['Table'];
    if (pushdata.length) {
        let sumobj = {FAC_ID:"合計",PNL_ID:pushdata.length+1};
        pushdata.push(sumobj);
        PageSetting.List = pushdata;
        PageSetting.Columns = Object.keys(pushdata[0]).map(item=>{
            return {
                title: item,
                align: 'center',
                dataIndex: item
            }
        })
    } else {
        return message.warn("無數據")
    }
}
let TableParams = UseParamsAdd["OQCQualityReport" + FactoryType]["FirstTableParams"];
watchEffect(() => {
    TableParams = UseParamsAdd["OQCQualityReport" + FactoryType]["FirstTableParams"];
    GetDetailData(TableParams);
});


</script>


<template>
    <div>
        <a-card>
            <DynamicTable row-key="id" :header-title="PageSetting.TopTitle" :columns="PageSetting.Columns"
                :key="PageSetting.TopTitle" bordered sticky :data-source="PageSetting.List" :data-all="PageSetting.List"
                :flexwidth="1" :pagination="false" export-file-name="导出">
                <template #export-button>
                    <a-button type="success"> 导出 </a-button>
                </template>
            </DynamicTable>
        </a-card>
    </div>
</template>