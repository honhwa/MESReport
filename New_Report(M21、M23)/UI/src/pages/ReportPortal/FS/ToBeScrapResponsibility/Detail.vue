<script lang="ts" setup>
import { useTable } from '@/components/core/dynamic-table';
import { reactive, watchEffect, watch, computed } from 'vue';
import { GetCommon } from '@/api/pages/common';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { useVirtualList } from '@vueuse/core';
import { useLoading } from '@/components/feat/Loading';
import { message } from 'ant-design-vue';

const UseParamsAdd = AddCommonPageParams();

function getImport(site: any) {
    const { ToBeScrapDetailColumns } = require(`@/pages/ReportPortal/FS/ToBeScrapReport/${site}/Table`);
    return { ToBeScrapDetailColumns };
}
let { ToBeScrapDetailColumns } = getImport(UseParamsAdd.Fab);

let useCommonParams = AddCommonPageParams();
let TableParams = useCommonParams.FirstTableParams;
let PageSetting = reactive({
    moreData: [] as any,
    List: [] as any,
    Columns: ToBeScrapDetailColumns,
    TopTitle: ''
    // BottomTitle:''
});
const [openFullLoading, closeFullLoading] = useLoading({
  tip: '數據加載中...'
});
const [DynamicTable] = useTable();
function getRowClassName(record: Object) {
    if (record && Object.keys(record).length != 0) {
        for (const [key, value] of Object.entries(record)) {
            if (typeof value == 'string' && ['小计', '小計', '合計', 'Total', 'Summary'].includes(value)) {
                return 'Yellow';
            } else if (typeof value == 'string' && ['總計', 'SUM', '总计', 'Sum'].includes(value)) {
                return 'Green';
            }
        }
    }
}
async function GetDetailData(TableParams: any) {
    // let APIParams: any = useCommonParams.APIMessage
    let formParams = useCommonParams.FormParams;
    let { Data} = await GetCommon(TableParams);
    
    // let { api01 } = await GetCommon({
    //     ...formParams,
    //     ReportType: 'LCMScrapDetail',
    //     prod_nbr: TableParams.PROD_NBR,
    //     ver: TableParams.VER,
    //     scrap_code: TableParams['dataIndex'],
    //     report_type: 'scrap_code'
    // });
    let pushdata = Data['Data0']['Table'];
    if (pushdata.length == 0) {
    closeFullLoading();
    return message.warn('无数据!');
  }
    let ReasonData = Data['Reason'];
    let sum = 0;
    pushdata.forEach((item, index) => {
        if (ReasonData[`Data${index}`]['Table'].length > 0) {
            item['DFCT_REASON'] = ReasonData[`Data${index}`]['Table'][0]['DFCT_REASON'] ?? '';
            item['ERRM_DESCR'] = ReasonData[`Data${index}`]['Table'][0]['ERRM_DESCR'] ?? '';
        }
    });
    
    PageSetting.moreData = pushdata;
    let temp = '';
    for (let i = 0; i < PageSetting.moreData.length - 1; i++) {

        if (PageSetting.moreData[i]['ENTITY_NO'] == temp) {
            PageSetting.moreData[i]['TOBESCRAP_QTY'] = '';
        } else {
            PageSetting.moreData[i]['TOBESCRAP_QTY'] = 1;
        }
        temp = PageSetting.moreData[i]['ENTITY_NO'];
    }
    pushdata.forEach((item, index) => {
      if(item['TOBESCRAP_QTY'] != ""){
        sum += item['TOBESCRAP_QTY']
        }
    });
    console.log(sum);
    PageSetting.moreData.push({ ENTITY_NO: '合 計', TOBESCRAP_QTY: sum });

    // console.log(pushdata);
    // PageSetting.moreData.forEach((item: object) => {
    //     item['TRANS_DATE1'] = item['TRANS_DATE'].split(' ')[0];
    //     item['Time'] = item['TRANS_DATE'].split(' ')[1];
    // });

    switch (UseParamsAdd.Fab) {
        case 'LCM1':
            PageSetting.TopTitle = 'LCMA Scrap Detail ';
            break;
        case 'LCM2':
            PageSetting.TopTitle = 'LCMB Scrap Detail ';

            break;
    }
    
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
    TableParams = useCommonParams.FirstTableParams;

    GetDetailData(TableParams);
});
</script>

<template>
    <div>
        <a-card>
            <div>
                <DynamicTable
                    row-key="id"
                    :header-title="PageSetting.TopTitle"
                    :columns="PageSetting.Columns"
                    :key="PageSetting.TopTitle"
                    bordered
                    :flexwidth="1"
                    :data-source="PageSetting.moreData"
                    :data-all="PageSetting.moreData"
                    :pagination="false"
                    export-file-name="导出"
                    :row-class-name="getRowClassName"
                >
                    <template #export-button>
                        <a-button type="success"> 导出 </a-button>
                    </template>
                </DynamicTable>
            </div>
        </a-card>
    </div>
</template>
