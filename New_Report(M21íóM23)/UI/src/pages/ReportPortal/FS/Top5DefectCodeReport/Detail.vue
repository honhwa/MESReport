<script lang="ts" setup>
import { useTable } from '@/components/core/dynamic-table';
import { reactive, watchEffect, watch, computed } from 'vue';
import { GetCommon } from '@/api/pages/common';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { getRowClassNmae } from '@/pages/ReportPortal/utils/common';
import { formatToDateTime } from '@/utils/dateUtil';

const UseParamsAdd = AddCommonPageParams();

function getImport(site: any) {
    const {
        Top5DetailColumns,
        Top5SecondColumns
    } = require(`@/pages/ReportPortal/FS/Top5DefectCodeReport/${site}/Table`);
    return { Top5DetailColumns, Top5SecondColumns };
}
let { Top5DetailColumns, Top5SecondColumns } = getImport(UseParamsAdd.Fab);

let useCommonParams = AddCommonPageParams();
let TableParams = useCommonParams.FirstTableParams;
let SecondTableParams = useCommonParams.SecondTableParams;
let PageSetting = reactive({
    moreData: [] as any,
    List: [] as any,
    Columns: Top5DetailColumns,
    TopTitle: '',
    BottomTitle: '',
    BottomColumns: Top5SecondColumns,
    BottomList: [] as any
    // BottomTitle:''
});
const [DynamicTable] = useTable();

async function GetDetailData(TableParams: any) {
    // let APIParams: any = useCommonParams.APIMessage
    let formParams = useCommonParams.FormParams;
    let errc_descr = TableParams['dataIndex'].split('-')[0];
    
    let { api01 } = await GetCommon({
        ...formParams,
        errc_descr: TableParams[errc_descr],
        work_ctr: errc_descr,
        prod_nbr: TableParams['PROD'],
        ReportType: formParams['ShowType']=='A'?'Top5DfctDetail':'Top5DfctDetailx'
    });
    let pushdata = api01['Data0']['Table'];
    pushdata.forEach((item) => {
        item['TRANS_DATE'] = formatToDateTime(item['TRANS_DATE']);
    });

    let QtyData = TableParams[TableParams['dataIndex']]
        ? TableParams[TableParams['dataIndex']].split('%')[1].replace(/[^0-9]/g, '')
        : 0;
    pushdata.push({ PROD_NBR: '合 計', PASS_QTY: QtyData });
    PageSetting.List = pushdata;
    PageSetting.TopTitle = `${UseParamsAdd.Fab} Top5 Defect Report`;
}
async function GetSecondTable(SecondTableParams: any) {
    let formParams = useCommonParams.FormParams;

    let errc_descr = TableParams['dataIndex'].split('-')[0];
    let { api01 } = await GetCommon({
        ...formParams,
        errc_descr: TableParams[errc_descr],
        work_ctr: TableParams['title'],
        prod_nbr: TableParams['PROD'],
        ReportType: 'Top5DfctDetail2',
        entity_no: SecondTableParams['ENTITY_NO']
    });
    let pushdata = api01['Data0']['Table'];
    PageSetting.BottomList = HandleSecondData(pushdata);
    // HandleSecondData(pushdata);
    PageSetting.BottomTitle = `${UseParamsAdd.Fab} Quality Report`;
}

watchEffect(() => {
    //一阶表格监控
    TableParams = useCommonParams.FirstTableParams;
    GetDetailData(TableParams);
});
watch(
    //二阶表格监控
    () => useCommonParams.SecondTableParams,
    (newValue) => {
        GetSecondTable(newValue);
    },
    {
        deep: true
    }
);
function HandleSecondData(OriginData: any[]) {
    let WORKCTR: any[] = [];
    let alldata: any[] = [];
    let ReturnData:any[]=[];
    let MaxCount:number = 0;
    OriginData.forEach((item) => {
        if (!WORKCTR.includes(item['WORK_CTR'])) WORKCTR.push(item['WORK_CTR']);
        for (let Props in item) {
            item[`${item['WORK_CTR']}-${Props}`] = item[Props];
        }
    });
    WORKCTR.forEach((WORK) => {
        let GroupingData: any[] = [];
        OriginData.forEach((item1) => {
            if (item1['WORK_CTR'] == WORK) {
                GroupingData.push(item1);
            }
        });
        MaxCount = GroupingData.length>MaxCount?GroupingData.length:MaxCount
        alldata.push(GroupingData);
    });
    for (let j = 0; j < MaxCount; j++) {
        let ColumnData = {};
        for (let i = 1; i < alldata.length; i++) {
            if (alldata.length>1&&alldata[0][j]) {
                ColumnData = alldata[0][j];
                for (let i = 1; i < alldata.length; i++) {
                    if (alldata[i][j]) {
                        ColumnData = Object.assign(ColumnData, alldata[i][j]);
                    }
                }
            }else if (alldata.length == 1 && alldata[0][j]) {
                //否則直接賦值(如果有值)
                ColumnData = alldata[0][j];
            }
        }
        ReturnData.push(ColumnData)
    }
    // console.log(ReturnData);
    
    return ReturnData
}
</script>


<template>
    <div>
        <a-card>
            <DynamicTable
                row-key="id" sticky
                :header-title="PageSetting.TopTitle"
                :columns="PageSetting.Columns"
                :key="PageSetting.TopTitle"
                bordered
                size="small"
                :data-source="PageSetting.List"
                :pagination="false"
                :export-file-name="PageSetting.TopTitle"
                :scroll="{ x: 2500, y: 500 }"
                :row-class-name="getRowClassNmae"
            >
                <template #export-button>
                    <a-button type="success"> 导出 </a-button>
                </template>
            </DynamicTable>
            <DynamicTable sticky
                row-key="id"
                :header-title="PageSetting.BottomTitle"
                :columns="PageSetting.BottomColumns"
                :key="PageSetting.BottomTitle"
                bordered
                size="small"
                :data-source="PageSetting.BottomList"
                :pagination="false"
                :export-file-name="PageSetting.BottomTitle"
                :scroll="{ x: 5500, y: 500 }"
                :row-class-name="getRowClassNmae"
            >
                <template #export-button>
                    <a-button type="success"> 导出 </a-button>
                </template>
            </DynamicTable>
        </a-card>
    </div>
</template>