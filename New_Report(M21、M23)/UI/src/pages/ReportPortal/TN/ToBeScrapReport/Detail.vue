<template>
    <div>
        <a-card>
            <div v-bind="containerProps" style="height: 600px">
                <DynamicTable
                    v-bind="wrapperProps"
                    row-key="id"
                    :header-title="PageSetting.TopTitle"
                    :columns="PageSetting.Columns"
                    :key="PageSetting.TopTitle"
                    bordered
                    size="small"
                    :data-source="PageSetting.List"
                    :pagination="false"
                    export-file-name="Export"
                    :scroll="{x:2500, y:500}"
                    :row-class-name="getRowClassName"
                >
                    <template #export-button>
                        <a-button type="success"> Export </a-button>
                    </template>
                </DynamicTable>
            </div>
        </a-card>
    </div>
</template>

<script lang="ts" setup>
import { useTable } from '@/components/core/dynamic-table';
import { reactive, watchEffect, watch, computed } from 'vue';
import { GetCommon } from '@/api/pages/common';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { ReportCommon } from '@/store/module/ReportCommon';
import { useVirtualList } from '@vueuse/core';

const UseParamsAdd = AddCommonPageParams();

function getImport(site: any) {
    const { ToBeScrapDetailColumns } = require(`@/pages/ReportPortal/TN/ToBeScrapReport/MOD/Table`);
    return { ToBeScrapDetailColumns };
}
let { ToBeScrapDetailColumns } = getImport(UseParamsAdd.Fab);

let useCommonParams = AddCommonPageParams();
let useReportCommon = ReportCommon();

let ReportNameParams = `ToBeScrapReportDetail_${useCommonParams.Fab}`;
let TableParams = useReportCommon[ReportNameParams].FirstTableParams;
let APIMessage = useReportCommon[ReportNameParams].APIMessage;
let formParams = useReportCommon[ReportNameParams].FormParams;	

let PageSetting = reactive({
    moreData: [] as any,
    List: [] as any,
    Columns: ToBeScrapDetailColumns,
    TopTitle: ''
    // BottomTitle:''
});
const [DynamicTable] = useTable();
function getRowClassName(record: Object) {
    if (record && Object.keys(record).length != 0) {
        for (const [key, value] of Object.entries(record)) {
            if (typeof (value) == "string" &&  ['小计', '小計', '合計',"Total","Summary"].includes(value)) {
                return 'Yellow';
            } else if (typeof (value) == "string" && ['總計', 'SUM', '总计', 'Sum'].includes(value)) {
                return 'Green';
            } 
        }
    }
}
async function GetDetailData(TableParams: any) {
    // let APIParams: any = useCommonParams.APIMessage
    APIMessage=useReportCommon[ReportNameParams].APIMessage; 
    let formParams = useReportCommon[ReportNameParams].FormParams;	

    const ParamsData: any = {
        name: "Date",
        apiName: "TN_ToBeScrapDataSetDetail",
        FactoryType: useCommonParams.Fab,
        G_FAC: useCommonParams.G_FAC_COM,
        prod_nbr:TableParams.PROD_NBR,
        ver:TableParams.VER,
        scrap_code:TableParams['dataIndex'],
        Sqlwhere:APIMessage,
     };

     const { Date: { Data: { Table: Data },
        SQL01,
        SQLFilter,
        
       } } = await GetCommon(ParamsData);

    let pushdata = Data;
    PageSetting.moreData = pushdata;
    PageSetting.moreData.push({ENTITY_NO:'合計',QTY:TableParams[TableParams['dataIndex']]})
    // console.log(pushdata);
    // PageSetting.moreData.forEach((item: object) => {
    //     item['TRANS_DATE1'] = item['TRANS_DATE'].split(' ')[0];
    //     item['Time'] = item['TRANS_DATE'].split(' ')[1];
    // });
    // switch(UseParamsAdd.Fab){
    //     case "LCM1":
    //         PageSetting.TopTitle = 'LCMA Scrap Detail ';
    //         break;
    //     case "LCM2":
    //         PageSetting.TopTitle = 'LCMB Scrap Detail ';
    //         break;
    // }
    PageSetting.TopTitle = UseParamsAdd.Fab + ' Scrap Detail ';
}

let onMoreDataLoad = computed(() => {
    if (PageSetting.moreData.length > 0) return PageSetting.moreData;
    else return [];
});
const { list, containerProps, wrapperProps } = useVirtualList(onMoreDataLoad, {
    itemHeight: 22
});
watch(
    () => list.value,
    () => {
        let pushData: any[] = [];
        PageSetting.List = [];
        list.value.forEach((item) => {
            if (item.index < PageSetting.moreData.length) {
                pushData.push(item.data);
            }
        });
        PageSetting.List = pushData;
    }
);
watchEffect(() => {
    //一阶表格监控
    TableParams = useReportCommon[ReportNameParams].FirstTableParams;

    GetDetailData(TableParams);
});
</script>


