<template>
    <div>
        <a-card>
            <div v-bind="containerProps" style="height: 80vh">
                <DynamicTable v-bind="wrapperProps" row-key="id" :header-title="PageSetting.TopTitle" sticky
                    :columns="PageSetting.Columns" :key="PageSetting.Columns" bordered size="small"
                    :data-all="PageSetting.moreData" :data-source="PageSetting.List" :pagination="false" :flexwidth="1"
                    export-file-name="导出">
                    <template #export-button>
                        <a-button type="success"> 导出 </a-button>
                    </template>
                </DynamicTable>
            </div>
        </a-card>
    </div>
</template>

<script lang="ts" setup>
import { useTable } from '@/components/core/dynamic-table';
import { reactive, watchEffect, watch, computed, ref } from 'vue';
import { GetCommon } from '@/api/pages/common';
// import { AddCommonPageParams } from '@/store/module/commonpage';
import { ReportCommonWithOut } from '@/store/module/ReportCommon';

import { useVirtualList } from '@vueuse/core'
const UseParamsAdd = ReportCommonWithOut();
let FactoryType = UseParamsAdd.Fab;
let PageSetting = reactive({
    moreData: [],
    List: [] as any,
    Columns: [] as any,
    TopTitle: 'LCMA Panel Wip Stay Chart',
});
let importurl = ref("LCM");
switch (UseParamsAdd.Fab) {
    case 'LCM1':
        importurl.value = UseParamsAdd.Fab;
        PageSetting.TopTitle = "LCMA Panel Wip Stay Chart ";
        break;
    case 'LCM2':
        importurl.value = 'LCM1';
        PageSetting.TopTitle = "LCM2 Panel Wip Stay Chart ";
        break;
    case 'SET1':
        importurl.value = 'LCM1';
        PageSetting.TopTitle = "LCMA Panel Wip Stay Chart ";
        break;
    case 'SET2':
        importurl.value = 'LCM1';
        PageSetting.TopTitle = "LCM2_TV Panel Wip Stay Chart ";
        break;
    case 'BLU1':
        importurl.value = UseParamsAdd.Fab;
        PageSetting.TopTitle = "LCMA Panel Wip Stay Chart ";
        break;
    default:
        importurl.value = UseParamsAdd.Fab;
        break;
}

function getImport(site: any) {
    const { PanelWipDetailProdColumns, PanelWipDetailOperColumns, PanelWipDetailColumns, SET2DetailColumns, LCM2DetailColumns, LCM2DetailColumns2 } = require(`@/pages/ReportPortal/FS/PanelWipStayChart/${site}/Table`);
    return { PanelWipDetailProdColumns, PanelWipDetailOperColumns, PanelWipDetailColumns, SET2DetailColumns, LCM2DetailColumns, LCM2DetailColumns2 };
}
let { PanelWipDetailProdColumns, PanelWipDetailOperColumns, PanelWipDetailColumns, SET2DetailColumns, LCM2DetailColumns, LCM2DetailColumns2 } = getImport(importurl.value);

let useCommonParams = ReportCommonWithOut();
let TableParams = useCommonParams["PanelWipStayChart" + FactoryType]["FirstTableParams"];


const [DynamicTable] = useTable();


async function GetDetailData(TableParams: any) {
    // console.log("TableParams的值",TableParams);
    switch (TableParams["FactoryType"]) {
        case "LCM1":
        case "SET1":
            PageSetting.Columns = PanelWipDetailColumns;
            break;
        case "LCM2":
            PageSetting.Columns = LCM2DetailColumns2;
            if (TableParams["Operation"] <= 1000) {
                PageSetting.Columns = LCM2DetailColumns;
            }
            console.log("輸出1PageSetting.Columns",PageSetting.Columns);
            console.log("輸出LCM2DetailColumns2",LCM2DetailColumns2);
            console.log("輸出LCM2DetailColumns",LCM2DetailColumns);
            
            break;
        case "SET2":
            if (TableParams["category"] == 3) {
                PageSetting.Columns = PanelWipDetailColumns
            } else {

                PageSetting.Columns = SET2DetailColumns
            }
            break;
        case "BLU1":
            if (TableParams["category"] == 1) {
                PageSetting.Columns = PanelWipDetailOperColumns;
            } else {
                // 为2的时候
                PageSetting.Columns = PanelWipDetailProdColumns;
            }
            break;
        default:
            break;
    }
    let { api01 } = await GetCommon(TableParams);
    let pushdata = api01['Data']['Table']
    pushdata.push({ PROD_NBR: '合计', VER: pushdata.length })
    PageSetting.moreData = pushdata;
    console.log("輸出廠別",TableParams["FactoryType"]);
    
    console.log('輸出PageSetting.moreData', PageSetting.moreData);
    console.log('輸出PageSetting.Columns ', PageSetting.Columns);

}

let onMoreDataLoad = computed(() => {
    if (PageSetting.moreData.length > 0) return PageSetting.moreData
    else return []
})
const { list, containerProps, wrapperProps } = useVirtualList(
    onMoreDataLoad,
    {
        itemHeight: 23,
    },
)
watch(() => list.value,
    () => {
        let pushData: any[] = []
        PageSetting.List = []
        list.value.forEach(item => {
            if (item.index < PageSetting.moreData.length) {
                pushData.push(item.data)
            }
        })
        PageSetting.List = pushData
    })
/* watch(
    () => useCommonParams["PanelWipStayChart" + FactoryType]["FirstTableParams"],
    (newvalue) => {
        console.log("watch監聽更新了",newvalue);
        GetDetailData(newvalue)
    }, {
    deep: true,
    immediate: true,
}
) */
watchEffect(() => {
    //一阶表格监控
    TableParams = reactive(useCommonParams["PanelWipStayChart" + FactoryType]["FirstTableParams"]);
    GetDetailData(TableParams)
});

</script>


