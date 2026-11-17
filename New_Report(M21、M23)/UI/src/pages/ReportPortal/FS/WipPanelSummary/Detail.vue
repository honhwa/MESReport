<script lang="ts" setup>
import { useTable } from '@/components/core/dynamic-table';
import { reactive, watchEffect, watch, computed } from 'vue';
import { GetCommon } from '@/api/pages/common';
import { useVirtualList } from '@vueuse/core';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { isEmpty } from '@/utils/common';
import dayjs from 'dayjs';





const UseParamsAdd = AddCommonPageParams()
console.log("輸出廠別", UseParamsAdd.Fab);


function getImport(site: any) {
    const { WipPanSumDetailCol } = require(`@/pages/ReportPortal/FS/WipPanelSummary/${site}/Table`);
    return { WipPanSumDetailCol };
}
let { WipPanSumDetailCol } = getImport(UseParamsAdd.Fab);




let useCommonParams = AddCommonPageParams();
let TableParams = useCommonParams.FirstTableParams;
// let formParams = 
let PageSetting = reactive({
    moreData: [],
    List: [] as any,
    Columns: WipPanSumDetailCol,
    TopTitle: '',
    // BottomTitle:''
});
const [DynamicTable] = useTable();


async function GetDetailData(TableParams: any) {
    // let APIParams: any = useCommonParams.APIMessage
    let formParams = useCommonParams.FormParams;
    let returnData ;
    switch (UseParamsAdd.Fab) {
        case "LCM1":
            let selText: any[] = []
            formParams['Colname'].forEach((item: string) => {
                let itemsplit: string = item.split('.')[1] ?? item.split('.')[0]
                selText.push(TableParams[itemsplit.toUpperCase()])
            })
            returnData= await GetCommon({ ...formParams, ReportType: 'WipPanelSumDetail', ...{ selText } });
            break;
        default:
            // 其餘廠別沒有在後端拼接，所以在前端拼接了
            console.log("輸出formParams['Colname']",formParams['Colname']);
            console.log("輸出TableParams",TableParams);
            let tempText = `AND a.wo_nbr = '${TableParams["WO_NBR"]}' AND a.LCD_PROD_NBR =  '${TableParams["LCD_PROD_NBR"]}'`;
            returnData= await GetCommon({ ...formParams, ReportType: 'WipPanelSumDetail',  selText:tempText });
            break;
    }
    let { api01 }  = returnData;
    let Data = api01['Data0']['Table'];
    Data.forEach(item => {
        if (isEmpty(item["TRANS_DATE"])) {
            item["rswvlhtxt18"] = 0
            item["rswvlhtxt19"] = ""
        } else {
            item["rswvlhtxt18"] = 0
            item["rswvlhtxt19"] = (dayjs(dayjs(new Date()).format("YYYY-MM-DD HH:mm:ss")).diff(dayjs(item["TRANS_DATE"].substr(0, 19).replace(/\./g, ":")), 'm') / 60).toFixed(2);
        }
        if (isEmpty(item["ISSUE_DATE"])) {
            item["rswvlhtxt23"] = 0
            item["rswvlhtxt24"] = ""
        } else {
            item["rswvlhtxt23"] = 0
            item["rswvlhtxt24"] = (dayjs(dayjs(new Date()).format("YYYY-MM-DD HH:mm:ss")).diff(dayjs(item["ISSUE_DATE"].substr(0, 19).replace(/\./g, ":")), 'm') / 60).toFixed(2);
        }
        //   替换换行和空格为""
        item["rswvlhtxt21"] = item["COMMENT1"].replace('"', "'").replace(" ", "")
        switch (item["PRIORITY"]) {
            case "1":
                item["rswvlhtxt22"] = "Normal"
                break;
            case "2":
                item["rswvlhtxt22"] = "Hot"
                break;
            case "3":
                item["rswvlhtxt22"] = "Super Hot"
                break;
            default:
                break;
        }


    })
    PageSetting.moreData = Data;
    PageSetting.TopTitle = 'Wip Panel Summary Detail'
}

let onMoreDataLoad = computed(() => {
    if (PageSetting.moreData.length > 0) return PageSetting.moreData
    else return []
})
const { list, containerProps, wrapperProps } = useVirtualList(
    onMoreDataLoad,
    {
        itemHeight: 22,
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
watchEffect(() => {
    //一阶表格监控
    TableParams = useCommonParams.FirstTableParams;
    GetDetailData(TableParams)
});
</script>

<template>
    <div>
        <a-card>
            <div v-bind="containerProps" style="height: 600px">
                <DynamicTable v-bind="wrapperProps" row-key="id" :header-title="PageSetting.TopTitle" sticky
                    :columns="PageSetting.Columns" :key="PageSetting.TopTitle" bordered size="small"
                    :data-source="PageSetting.List" :pagination="false" export-file-name="导出" :scroll="{ x: 2500 }">
                    <template #export-button>
                        <a-button type="success"> 导出 </a-button>
                    </template>
                </DynamicTable>
            </div>
        </a-card>
    </div>
</template>
