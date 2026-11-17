<script lang="ts" setup>
import { useTable } from '@/components/core/dynamic-table';
import { reactive, watchEffect } from 'vue';
import { GetCommon } from '@/api/pages/common';
import { ReportCommon } from '@/store/module/ReportCommon';
import { message } from 'ant-design-vue';

const UseParamsAdd = ReportCommon();
let FactoryType: string = UseParamsAdd.Fab;

/* function getImport(site: any) {
    const {
        detail1
    } = require(`@/pages/ReportPortal/FS/AssyKittingSummary/${site}/Table`);
    return { detail1 };
}
let { detail1 } = getImport("LCM1"); */


let PageSetting = reactive({
    moreData: [] as any,
    List: [] as any,
    Columns: [] as any,
    TopTitle: '',
});
const [DynamicTable] = useTable();

async function GetDetailData(TableParams: any) {
    let baseParams = {
        apiName: 'AssyKittingSummary',
        FactoryType: FactoryType,
        G_FAC: UseParamsAdd.G_FAC_COM,
        ReportType: "AssyKittingDetail"
    };
    let { api01 } = await GetCommon({...baseParams,...TableParams});
    let pushdata = api01['Data']['Table'];
    if (pushdata.length) {
        PageSetting.List = pushdata;
        let obj = {};
        PageSetting.Columns = Object.keys(pushdata[0]).map((item,index)=>{
            if(index == 0){
               obj[item] = "合計"
            }
            else if(index == 1){
               obj[item] = PageSetting.List.length;

            }
            return {
                title: item,
                align: 'center',
                dataIndex: item
            }
        })
        PageSetting.List.push(obj)
    } else {
        PageSetting.List = [];
        PageSetting.Columns = [];
        return message.warn("無數據")
    }
    PageSetting.TopTitle = 'LCMA Assy Kitting Summary';
}
let TableParams = UseParamsAdd["AssyKittingSummary" + FactoryType]["FirstTableParams"];
watchEffect(() => {
    TableParams = UseParamsAdd["AssyKittingSummary" + FactoryType]["FirstTableParams"];
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