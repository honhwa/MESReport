<template>
    <div class="LCMWipSummaryReport">
        <a-card>
         
            <a-row>
                <a-col :span="24 - PageParams.TableSpan" class="ScrollForm" v-show="PageParams.ShowMenu">
                    <SchemaForm @submit="confirm"> </SchemaForm>
                </a-col>
                <a-col :span="PageParams.TableSpan">
                    <DynamicTable row-key="id" header-title="LCMA Wip Summary Reportall"
                    sticky
                    :PageParams="PageParams"
                        :columns="PageSetting.TopColumns" :key="PageSetting.TopColumns" bordered size="small"
                        :data-source="PageSetting.TopList" :pagination="false" :scroll="{ x: 3000, y: 500 }"
                        export-file-name="导出" >
                        <template #export-button>
                            <a-button type="success"> 导出 </a-button>
                        </template>
                    </DynamicTable>
                </a-col>
            </a-row>
        </a-card>
    </div>
</template>


<script lang="tsx" setup>
import {PageParams} from '@/components/core/dynamic-table/src/dynamic-table'
import { useTable } from '@/components/core/dynamic-table';
import { reactive } from '@vue/reactivity';
import { useForm } from '@/components/core/schema-form';
// import { cloneDeep } from 'lodash-es';
import { GetCommon } from '@/api/pages/common';
import {  onMounted } from '@vue/runtime-core';
import { getfinalsum, getrowsum} from "@/utils/reportcommon/Calculation";
import { cloneDeep } from 'lodash';
import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect';
import { AddCommonPageParams } from '@/store/module/commonpage';
const UseParamsAdd = AddCommonPageParams()
let FactoryType: any = UseParamsAdd.Fab;

function getImport(site: any) {
    const { schemas, optionsdata, } = require(`@/pages/ReportPortal/${site}/LCMWipSummaryReport/LCM1/form-schema`);
    const { baseColumns, allColumns,holdandnoholdColumns } = require(`@/pages/ReportPortal/${site}/LCMWipSummaryReport/LCM1/table`);
    return { schemas, optionsdata, baseColumns, allColumns,holdandnoholdColumns };
}
const { schemas, optionsdata, baseColumns, allColumns,holdandnoholdColumns } = getImport('FS');

// 頁面數據
let PageSetting = reactive({
    TopList: [] as any,
    TopColumns: [] as any,
    Message: {},


});
const [DynamicTable] = useTable();
const { mounted, InfiniteScroll, mountedowner, mountedOper } = useRemoteSelect()


const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas,
    actionColOptions: {
        span: 24
    },
    fieldMapToTime: [['fieldTime', ['vDate_s', 'vDate_e1'], 'YYYY-MM-DD']],
    layout: "vertical"
});

let TopTableParams = reactive({
    name: "Date",
    apiName: "PPWipSumDataSet",
    FactoryType: FactoryType,
    G_FAC: "A",
    vDate_e1: "",
    owner_code: [],
    EntyGroup: [],
    prod_nbr: [],
});


async function confirm() {
    PageSetting.TopList = [];
    PageSetting.TopColumns = [];
    //給ParamsData賦值 
    let tempkeys = Object.keys(TopTableParams);
    for (let i in cloneDeep(dynamicFormRef.formModel)) {
        if (tempkeys.includes(i)) {
            TopTableParams[i] = cloneDeep(dynamicFormRef.formModel[i]);
        }
    }
    // Product和LCM product共用同一个参数，如果最后的LCM product有输入，LCM product覆盖Product
    if (dynamicFormRef.formModel.inputprod) {
        // TopTableParams["prod_nbr"] = dynamicFormRef.formModel.inputprod.split(',')
        TopTableParams["prod_nbr"] = dynamicFormRef.formModel.inputprod.split(/[\r\n\t]/).filter(item => item)
    }
    const {
        Date: {
            Data: { Table: Data },//no-hold的數據
            Data1: { Table: Data1 },//留給hold做合并對象
            Data2: { Table: Data2 },//hold的數據
            Data3: { Table: Data3 },
            Message: sqlmessage } } = await GetCommon(TopTableParams);
    // 处理non-hold
    let { non_hold: non_hold_columns, povitData: non_hold_data, calcuList: non_hold_calcuList } = handledata(Data, "non-hold", "WORK_CTR");
    // 处理hold,出現問題
    // Data2數據處理
    // 獲取Data2的數字
    let tempArr = Data2.map(item => item["WORK_CTR"])
    /* tempArr.sort(function (a, b) {
        return a - b;
    }); */
    tempArr.forEach(titem => {
        Data1.forEach(item => {
            item["WORK_CTR"] = titem;
            item["QTY"] = 0;
        })
    });
    let tempholdData = [...Data2,...Data1 ];
    tempholdData.forEach(item => {
        item["WORK_CTR1"] = `hold${item["WORK_CTR"]}`
    })
    let { non_hold: hold_columns, povitData: hold_data, calcuList: hold_calcuList } = handledata(tempholdData, "hold", "WORK_CTR1");
    switch (dynamicFormRef.formModel["type1"]) {
        case "all":
            // 数组合并
            for (let i = 0; i < non_hold_data.length; i++) {
                for (let j = 0; j < hold_data.length; j++) {
                    if (non_hold_data.length > 0) {
                        if (non_hold_data[i]["LCM_OWNER"] == hold_data[j]["LCM_OWNER"] && non_hold_data[i]["PROD_NBR"] == hold_data[j]["PROD_NBR"] && non_hold_data[i]["CUSTOMER"] == hold_data[j]["CUSTOMER"]) {
                            non_hold_data[i] = { ...non_hold_data[i], ...hold_data[j] }
                        }
                    }
                }
            }
            // scrap小計  scrap_sum
            // all小計  all_sum
            // 總計  all_totalsum = hold_sum + non-hold_sum
            getrowsum(non_hold_data, ["hold_sum", "non-hold_sum"], "all_totalsum");
            non_hold_data.push(getfinalsum(non_hold_data, [...hold_calcuList, ...non_hold_calcuList.map(item => String(item)),...["scrap_sum","all_sum","all_totalsum"]], "LCM_OWNER"));
            PageSetting.TopColumns = [...baseColumns,non_hold_columns, hold_columns,allColumns];
            PageSetting.TopList = non_hold_data;
            non_hold_data[non_hold_data.length-1]["hold_sum"] =  non_hold_data[non_hold_data.length-1]["holdfbsum"] + non_hold_data[non_hold_data.length-1]["holdreworksum"]
            non_hold_data.forEach(item=>{
                item["scrap_sum"]  = 0;
                item["all_sum"]  = 0;
            })
            break;
        case "hold":
            getrowsum(hold_data, ["hold_sum"], "all_totalsum");
            hold_data.push(getfinalsum(hold_data, [...hold_calcuList,...["all_totalsum"]], "LCM_OWNER"));
            hold_data[hold_data.length-1]["hold_sum"] = hold_data[hold_data.length-1]["all_totalsum"]
            PageSetting.TopColumns = [...baseColumns,hold_columns,holdandnoholdColumns];
            PageSetting.TopList = hold_data;
            break;
        case "non-hold":
            getrowsum(non_hold_data, ["non-hold_sum"], "all_totalsum");
            non_hold_data.push(getfinalsum(non_hold_data, [...(non_hold_calcuList.map(item => String(item))),...["all_totalsum"]], "LCM_OWNER"));
            non_hold_data[non_hold_data.length-1]["non-hold_sum"] = non_hold_data[non_hold_data.length-1]["all_totalsum"]
            PageSetting.TopColumns = [...baseColumns,non_hold_columns,holdandnoholdColumns];
            PageSetting.TopList = non_hold_data;
            break;
        default:
            break;
    }
}
onMounted(
    async () => {
        await mountedowner(optionsdata, FactoryType)
        await mountedOper(optionsdata, FactoryType);
        await mounted(optionsdata, FactoryType)
    }
)

</script>

<style lang="less" scoped>
.btn-rows {
    button {
        margin-right: 12px;
    }
}

.LCMWipSummaryReport :deep(.sub-total) {
    background-color: #ffb468 !important;
}

.LCMWipSummaryReport :deep(.total) {
    background-color: #ff80c0 !important;
}

.swapper {
    display: flex;
    flex-direction: column;
}
</style>
