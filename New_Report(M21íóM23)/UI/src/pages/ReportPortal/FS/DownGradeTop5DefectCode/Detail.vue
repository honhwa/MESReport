<script lang="ts" setup>
import { useTable } from '@/components/core/dynamic-table';
import { reactive, watchEffect, watch, computed } from 'vue';
import { GetCommon } from '@/api/pages/common';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { getRowClassNmae, Insertrowcount } from '@/pages/ReportPortal/utils/common';
import { formatToDateTime } from '@/utils/dateUtil';
import { cloneDeep } from 'lodash';
import { deRepeat } from '@/utils/helper/povitHelper';

const UseParamsAdd = AddCommonPageParams();

function getImport(site: any) {
    const {
        Top5DetailColumns,
        Top5SecondColumns
    } = require(`@/pages/ReportPortal/FS/DownGradeTop5DefectCode/${site}/Table`);
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
        /* dfct_code:TableParams["DFCT_CODE"],
        errc_descr: TableParams[errc_descr], */
        errc_descr: TableParams["DFCT_CODE"],
        dfct_code: TableParams[errc_descr],
        // work_ctr: TableParams['title'],
        prod_5: TableParams['PROD'],
        apiName: "DownGradeTop5DefectCode",
        ReportType: 'DownGradeTop5DfctDetail',
    });
    let pushdata = api01['Data0']['Table'];
    pushdata.forEach((item) => {
        item['TRANS_DATE'] = formatToDateTime(item['TRANS_DATE']);
    });
    Insertrowcount(pushdata, ["PROD_NBR"], ["PASS_QTY"], "PROD_NBR");
    let QtyData = TableParams[TableParams['dataIndex']]
        ? TableParams[TableParams['dataIndex']].split('%')[1].replace(/[^0-9]/g, '')
        : 0;
    pushdata.push({ PROD_NBR: '總 計', PASS_QTY: QtyData });
    PageSetting.List = pushdata;
    PageSetting.TopTitle = 'LCMA Down Grade Top5 Defect Report ';
}
async function GetSecondTable(SecondTableParams: any) {
    let formParams = useCommonParams.FormParams;
    let errc_descr = TableParams['dataIndex'].split('-')[0];
    let params = {
        ...formParams,
        errc_descr: TableParams["DFCT_CODE"],
        dfct_code: TableParams[errc_descr],
        entity_no: "'" + SecondTableParams['ENTITY_NO'] + "'",
        prod_5: TableParams['PROD'],
        apiName: "DownGradeTop5DefectCode",
        ReportType: 'DownGradeTop5DfctDetail2',
    }
    /* if(SecondTableParams["PROD_NBR"] == "合計"){
        params["entity_no"] = PageSetting.List[SecondTableParams["rowindex"]]["PASS_QTY"];
    } */
    let { api01 } = await GetCommon(params);
    let pushdata = api01['Data0']['Table'];
    PageSetting.BottomList = HandleSecondData(pushdata);
    // HandleSecondData(pushdata);
    PageSetting.BottomTitle = 'LCMA Quality Report';
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
    let changeprops = ["ENTITY_NO", "EQUIP_NBR", "TRANS_DATE", "OPERATOR", "TRANS_TYPE"]
    let obj = {};
    OriginData.forEach(item => {
        changeprops.forEach(citem => {
            item[`${item["WORK_CTR"]}-${citem}`] = item[citem];
        })
        if (!obj[item["WORK_CTR"]]) {
            obj[item["WORK_CTR"]] = [item]
        } else {
            obj[item["WORK_CTR"]].push(item)
        }
    })
    // 取分组中长度最长的

    let tempobj = {
        key:"",
        count:0,
    };
    for(let i in obj){
        if(obj[i].length >= tempobj["count"]){
            tempobj.key = i;
            tempobj.count = obj[i].length
        }
    }
    let finalobj = cloneDeep(obj[tempobj.key]);
    delete obj[tempobj.key];
    for (let i = 0; i < finalobj.length; i++) {
        let temparr:any[] = [];
        for(let j in obj){
            temparr.push(obj[j][i])
        }
        temparr.forEach(item=>{
            if(typeof item != "undefined"){
                finalobj[i] = Object.assign(finalobj[i],item)
            }
        })
        
    }
    return deRepeat(finalobj,['ENTITY_NO','PROD_NBR'])
}
</script>


<template>
    <div>
        <a-card>
            <DynamicTable row-key="id" :header-title="PageSetting.TopTitle" :columns="PageSetting.Columns"
                :key="PageSetting.TopTitle" bordered size="small" :data-source="PageSetting.List" :pagination="false"
                :export-file-name="PageSetting.TopTitle" :scroll="{ x: 2500, y: 500 }"
                sticky
                :row-class-name="getRowClassNmae">
                <template #export-button>
                    <a-button type="success"> 导出 </a-button>
                </template>
            </DynamicTable>
            <DynamicTable row-key="id" :header-title="PageSetting.BottomTitle" :columns="PageSetting.BottomColumns"
                :key="PageSetting.BottomTitle" bordered size="small" :data-source="PageSetting.BottomList"
                sticky
                :pagination="false" :export-file-name="PageSetting.BottomTitle" :scroll="{ x: 5500, y: 500 }"
                :row-class-name="getRowClassNmae">
                <template #export-button>
                    <a-button type="success"> 导出 </a-button>
                </template>
            </DynamicTable>
        </a-card>
    </div>
</template>