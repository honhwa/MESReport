<script lang="ts" setup>
import { useTable } from '@/components/core/dynamic-table';
import { reactive, watchEffect, watch, computed } from 'vue';
import { GetCommon } from '@/api/pages/common';
import { AddCommonPageParams } from '@/store/module/commonpage';
// import { useVirtualList } from '@vueuse/core';



const UseParamsAdd = AddCommonPageParams();
function getImport(site: any) {
    const {
        OQCInspectionDetailColumns
    } = require(`@/pages/ReportPortal/FS/OQCInspectionQuantityControl/${site}/Table`);
    return { OQCInspectionDetailColumns};
}
let { OQCInspectionDetailColumns} = getImport(UseParamsAdd.Fab);

let useCommonParams = AddCommonPageParams();
let TableParams = useCommonParams.FirstTableParams;
let PageSetting = reactive({
    moreData: [] as any,
    List: [] as any,
    Columns: OQCInspectionDetailColumns,
    TopTitle: '',
    // BottomTitle:''
});
const [DynamicTable] = useTable();

async function GetDetailData(TableParams: any) {
    // let APIParams: any = useCommonParams.APIMessage
    let formParams = useCommonParams.FormParams;
    let { api01 } = await GetCommon({
        ...formParams,
        ReportType:"OQCInspQtyCtrlDetail",
        EQP_LINE:"",
        oper:TableParams['WORK_CTR'],
        product:TableParams['PROD_NBR'],
        floor:formParams['Floor']=='ALL'?"":formParams['Floor'],
        line_no:TableParams['LINE_NO']
    });
    let pushdata = api01['Data0']['Table'];
    pushdata.push({FAC_ID:'SUM',PNL_ID:TableParams['INSPECT_QTY']})
    PageSetting.List = pushdata;
    PageSetting.TopTitle = 'LCMA Top5 Defect Report ';
}
// let onMoreDataLoad = computed(() => {
//     if (PageSetting.moreData.length > 0) return PageSetting.moreData;
//     else return [];
// });
// const { list, containerProps, wrapperProps } = useVirtualList(onMoreDataLoad, {
//     itemHeight: 22
// });
// watch(
//     () => list.value,
//     () => {
//         let pushData: any[] = [];
//         PageSetting.List = [];
//         list.value.forEach((item) => {
//             if (item.index < PageSetting.moreData.length) {
//                 pushData.push(item.data);
//             }
//         });
//         PageSetting.List = pushData;
//     }
// );

watchEffect(() => {
    //一阶表格监控
    TableParams = useCommonParams.FirstTableParams;
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
                    sticky
                    :data-source="PageSetting.List"
                    :pagination="false"
                    export-file-name="导出"
                    :scroll={x:2500,y:600}
                >
                    <template #export-button>
                        <a-button type="success"> 导出 </a-button>
                    </template>
                </DynamicTable>
        </a-card>
    </div>
</template>