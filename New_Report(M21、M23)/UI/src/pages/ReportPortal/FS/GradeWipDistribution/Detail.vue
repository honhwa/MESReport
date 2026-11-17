<template>
    <div>
        <a-card>
            <div v-bind="containerProps" style="height: 600px">
                <DynamicTable v-bind="wrapperProps" row-key="id" :header-title="PageSetting.TopTitle"
                sticky
                    :columns="PageSetting.Columns" :key="PageSetting.Columns" bordered size="small"
                    :data-source="PageSetting.List" :pagination="false" export-file-name="导出">
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
import { reactive, watchEffect, watch, computed } from 'vue';
import { GetCommon } from '@/api/pages/common';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { useVirtualList } from '@vueuse/core'





let useCommonParams = AddCommonPageParams();
let TableParams = useCommonParams.FirstTableParams;
// let formParams = 


let basecolumns = ['PNL_ID',
    'PROD_NBR',
    'VER'].map((item: string, index: number) => {
        return { title: item, dataIndex: item, align: 'center' }
    })
let PageSetting = reactive({
    moreData: [],
    List: [] as any,
    Columns: basecolumns,
    TopTitle: 'LCMA Panel Wip Stay Chart',
    // BottomTitle:''
});
const [DynamicTable] = useTable();


async function GetDetailData(TableParams: any) {
    let APIParams: any = useCommonParams.APIMessage
    let formParams = useCommonParams.FormParams
    let baseParams = {
        apiName: 'GradeWipDistDetail', FactoryType: 'LCM1', G_FAC: 'A',
        StrSql_w: APIParams.StrSql_w, Current_Date: formParams['vDate_s'],
    }
    let col0: string = ''
    if (TableParams['dataIndex'][0] == 'N') col0 = 'Non Hold'
    else if (TableParams['dataIndex'][0] == 'Y') col0 = 'Hold'
    else col0 = ''
    let ClickParams = { col0: col0, col1: TableParams['WORK_CTR'], row0: TableParams['PROD_NBR'], row1: TableParams['LCM_GRADE'], row2: TableParams['WO_CUST'], row3: TableParams['CUSTOMER_GROUP'], }
    let { api01 } = await GetCommon({ ...baseParams, ...ClickParams });
    let pushdata = api01['Data']['Table']
    PageSetting.moreData = pushdata

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

