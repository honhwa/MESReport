<script lang="ts" setup>
import { useTable } from '@/components/core/dynamic-table';
import { reactive, watchEffect, watch, computed } from 'vue';
import { GetCommon } from '@/api/pages/common';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { getRowClassNmae, Insertrowcount } from '@/pages/ReportPortal/utils/common';
import { formatToDateTime } from '@/utils/dateUtil';
import { cloneDeep } from 'lodash';
import { deRepeat } from '@/utils/helper/povitHelper';
import { ReportCommon } from '@/store/module/ReportCommon';
import { useLoading } from '@/components/feat/Loading';

const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});

const UseParamsAdd = AddCommonPageParams();

function getImport(site: any) {
    const {
        Top5DetailColumns,
        Top5SecondColumns
    } = require(`@/pages/ReportPortal/TN/DownGradeTop5DefectCode/MOD/Table`);
    return { Top5DetailColumns, Top5SecondColumns };
}
let { Top5DetailColumns, Top5SecondColumns } = getImport(UseParamsAdd.Fab);

let useCommonParams = AddCommonPageParams();
let useReportCommon = ReportCommon();

let ReportNameParams = `DownGradeTop5DefectCodeDetail_${useCommonParams.Fab}`;
let tableParams = useReportCommon[ReportNameParams].FirstTableParams;
let APIMessage = JSON.parse(useReportCommon[ReportNameParams].APIMessage);
let formParams = useReportCommon[ReportNameParams].FormParams;

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
    openFullLoading();
    APIMessage=JSON.parse(useReportCommon[ReportNameParams].APIMessage); 
    formParams = useReportCommon[ReportNameParams].FormParams;
    debugger
    let { api01 } = await GetCommon({
        apiName: "TN_DownGradeTop5DefectCodeDataSetDetail",
        FactoryType: useCommonParams.Fab,
        StartTime:formParams["vDate_s"],
        EndTime:formParams["vDate_e1"],
        Operation:formParams["workGroup"],
        Prod:tableParams["PROD"],
        DefectCode:tableParams["DFCT_CODE"]
    });
    let pushdata = api01['Data']['Table'];
    // pushdata.forEach((item) => {
    //     item['TRANS_DATE'] = formatToDateTime(item['TRANS_DATE']);
    // });
    // Insertrowcount(pushdata, ["PROD_NBR"], ["PASS_QTY"], "PROD_NBR");
    // let QtyData = TableParams[TableParams['dataIndex']]
    //     ? TableParams[TableParams['dataIndex']].split('%')[1].replace(/[^0-9]/g, '')
    //     : 0;
    // pushdata.push({ PROD_NBR: '總 計', PASS_QTY: QtyData });
    PageSetting.List = pushdata;
    PageSetting.TopTitle = useCommonParams.Fab +' Down Grade Top5 Defect Report ';
    closeFullLoading();
}


watchEffect(() => {
    //一阶表格监控
    tableParams = useReportCommon[ReportNameParams].FirstTableParams;
    GetDetailData(tableParams);
});
watch(
    //二阶表格监控
    () => useCommonParams.SecondTableParams,
    (newValue) => {
    },
    {
        deep: true
    }
);

</script>


<template>
    <div>
        <a-card>
            <DynamicTable row-key="id" :header-title="PageSetting.TopTitle" :columns="PageSetting.Columns"
                :key="PageSetting.TopTitle" bordered size="small" :data-source="PageSetting.List" :pagination="false"
                :export-file-name="PageSetting.TopTitle" :scroll="{ x: 500, y: 500 }"
                sticky
                :row-class-name="getRowClassNmae">
                <template #export-button>
                    <a-button type="success"> Export </a-button>
                </template>
            </DynamicTable>
        </a-card>
    </div>
</template>