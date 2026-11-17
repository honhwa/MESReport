<template>
    <div class="OperationPerformance">
        <a-card>
       
            <a-row>
                <a-col :span="24 - PageParams.TableSpan" class="ScrollForm" v-show="PageParams.ShowMenu">
                    <SchemaForm @submit="confirm"> </SchemaForm>
                </a-col>
                <a-col :span="PageParams.TableSpan">
                    <!-- <div class="tablewrapper"> -->
                    <DynamicTable row-key="id" :header-title="PageSetting.TopTitle" :columns="PageSetting.TopColumns"
                        :flexwidth="1" :style="{ 'margin-bottom': '20px' }" showFavorite sticky :PageParams="PageParams"
                        :key="PageSetting.TopColumns" bordered :data-source="PageSetting.TopList"
                        :data-all="PageSetting.TopList" :pagination="false" export-file-name="导出"
                        :row-class-name="getRowClassNmae">
                        <template #export-button>
                            <a-button type="success"> 导出 </a-button>
                        </template>
                    </DynamicTable>
                    <!-- </div> -->
                    <!-- <div class="tablewrapper"> -->
                    <DynamicTable row-key="id" :header-title="PageSetting.BottomTitle" sticky
                        :columns="PageSetting.BottomColumns" :key="PageSetting.BottomColumns" bordered :flexwidth="1"
                        :style="{ 'margin-bottom': '10px' }" :data-source="PageSetting.BottomList" :pagination="false"
                        :data-all="PageSetting.BottomList" export-file-name="导出" :row-class-name="getRowClassNmae">
                        <template #export-button>
                            <a-button type="success"> 导出 </a-button>
                        </template>
                    </DynamicTable>
                    <!-- </div> -->



                </a-col>
            </a-row>
        </a-card>
    </div>
</template>

<script lang="ts" setup>
import {PageParams} from '@/components/core/dynamic-table/src/dynamic-table'
import { useForm } from '@/components/core/schema-form';
import { useTable } from '@/components/core/dynamic-table';
import { reactive, ref } from '@vue/reactivity';
import {  onMounted } from '@vue/runtime-core';
import { GetCommon } from '@/api/pages/common';
import { AddOperationParams } from '@/store/module/operationDeatil';
import { getRowClassNmae } from '@/pages/ReportPortal/utils/common'
import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { message } from 'ant-design-vue';


let PageSetting = reactive({
    TopTitle: "",
    BottomTitle: "",
    TopList: [] as any,
    BottomList: [] as any,
    TopColumns: [] as any,
    BottomColumns: [] as any,
    FactoryType: "",
    x:2000,
});
const UseParamsAdd = AddCommonPageParams()
let FactoryType: any = UseParamsAdd.Fab;
PageSetting.FactoryType = FactoryType;
let importurl = ref("LCM");
switch (FactoryType) {
    case "LCM1":
    case "LCM2":
    case "SET1":
        importurl.value = "LCM";
        PageSetting.TopTitle = "LCMA Operation Performance";
        PageSetting.BottomTitle = "LCMA Operation Performance Rework";
        /* if (FactoryType == "SET1") {
            PageSetting.TopTitle = "LCMA_TV  Operation Performance";
            PageSetting.BottomTitle = "LCMA_TV  Operation Performance Rework";
        } */
        if (FactoryType == "SET1") {
            PageSetting.TopTitle = "LCMA_TV  Operation Performance";
            PageSetting.BottomTitle = "LCMA_TV  Operation Performance Rework";
        }else if (FactoryType == "SET2") {
            PageSetting.TopTitle = "LCMA_TV  Operation Performance";
            PageSetting.BottomTitle = "LCMA_TV  Operation Performance Rework";
        }
        PageSetting.x = 2000;
        break;
    case "BLU1":
    case "BLU2":
        importurl.value = "BLU";
        PageSetting.TopTitle = "BLA Operation Performance ";
        PageSetting.BottomTitle = "BLA Operation Performance Rework "
        PageSetting.x = 600;
        break;
    default:
        break;
}
function getImport(site: any) {
    const { schemas, optionsdata } = require(`@/pages/ReportPortal/FS/OperationPerformance/${site}/form-schema`);
    const { handleindexTable } = require(`@/pages/ReportPortal/FS/OperationPerformance/${site}/index`);
    const { topBaseColumns, bottomBaseColumns, topTableProps, bottomTableProps ,pushFactoryType} = require(`@/pages/ReportPortal/FS/OperationPerformance/${site}/Table`);
    return { schemas, topBaseColumns, bottomBaseColumns, optionsdata, topTableProps, bottomTableProps, handleindexTable ,pushFactoryType};
}
const { schemas, topBaseColumns, bottomBaseColumns, optionsdata, topTableProps, bottomTableProps, handleindexTable ,pushFactoryType} = getImport(importurl.value);


const { mounted, InfiniteScroll, mountedowner, mountedOper, mountedLCDGrade, mountedLcmProdType } = useRemoteSelect()





const [DynamicTable] = useTable();
const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas:schemas(FactoryType),
    actionColOptions: {
        span: 24
    },
    layout: "vertical"
});
// 点击提交
function confirm() {
    pushFactoryType(PageSetting.FactoryType);
    const UseFormParamsAdd = AddOperationParams();
    const ParamsData: any = {
        name: "Date",
        apiName: "OperationPerformance",
        FactoryType: FactoryType,
        FacId: 'A',
        Building: 'A',
        DateFrom: '',
        DateTo: '',
        Shift: '',
        Floor: '',
        WorkOrder: '',
        LcmProductType: 'ALL',
        Size: 'ALL',
        BigProduct: ['ALL'],
        LcmOwner: [],
        LcdGrade: [],
        Product: [],
        ProdId: []
    };
    for (let i in dynamicFormRef.formModel) {
        // if (i == 'WorkOrder') ParamsData[i] = dynamicFormRef.formModel[i].split(',');
        if (i == 'WorkOrder' || i == "ProdId") ParamsData[i] = dynamicFormRef.formModel[i].split(/[\r\n\t]/).filter(item => item)
        else ParamsData[i] = dynamicFormRef.formModel[i];
    }
    dynamicFormRef.validate().then(async () => {
        const { Date: { Data: { Table: Data }, SQL01, SQLbu1, SQLbu2, StrSql_w, StrSql_w1, StrSql_w2, StrSql_w2e, StrSql_w3, StrSql_w4, StrSql_we, StrSql_ww, sql_m, sql_m1 } } = await GetCommon(ParamsData);
        let Message = {
            "SQL01": SQL01,
            "SQLbu1": SQLbu1,
            "SQLbu2": SQLbu2,
            "StrSql_w": StrSql_w,
            "StrSql_w1": StrSql_w1,
            "StrSql_w2": StrSql_w2,
            "StrSql_w2e": StrSql_w2e,
            "StrSql_w3": StrSql_w3,
            "StrSql_w4": StrSql_w4,
            "StrSql_we": StrSql_we,
            "StrSql_ww": StrSql_ww,
            "sql_m": sql_m,
            "sql_m1": sql_m1,
        }
        UseFormParamsAdd.AddAPIResultMessage(Message);
        UseFormParamsAdd.AddFormParams(ParamsData);
        if(Data.length == 0){
            return message.warn("無數據!");
        }
        handleindexTable(Data, PageSetting, topTableProps, bottomTableProps);
        console.log("輸出PageSetting.BottomList", PageSetting.BottomList[0]);

        PageSetting.BottomList = PageSetting.BottomList.filter(Boolean);

        console.log("輸出最終處理好的數據", PageSetting);
        // 清除為0或者0(0)的數據
        if (PageSetting.TopList.length) {
            let keyarr = Object.keys(PageSetting.TopList[0]);
            PageSetting.TopList.forEach((item, index) => {
                keyarr.forEach((kitem) => {
                    if ([0, '0(0)', '0.0000',"0.00",
                        "NaN"].includes(item[kitem])) {
                        item[kitem] = '';
                    }
                });
            });
        }

        if (PageSetting.BottomList.length) {
            let keyarr2 = Object.keys(PageSetting.BottomList[0]);
            PageSetting.BottomList.forEach((item, index) => {
                keyarr2.forEach((kitem) => {
                    if ([0, '0(0)', '0.0000',"0.00",
                        "NaN"].includes(item[kitem])) {
                        item[kitem] = '';
                    }
                });
            });
        }



    });
}





onMounted(async () => {
    await mountedowner(optionsdata, FactoryType)
    // await mounted(optionsdata, FactoryType);
    switch (FactoryType) {
        case "LCM1":
        case "LCM2":
            await mountedOper(optionsdata, FactoryType);
            await mountedLCDGrade(optionsdata, FactoryType);
            await mountedLcmProdType(optionsdata, FactoryType);
            break;
        case "BLU1":
        case "BLU2":
            break;
        default:
            break;
    }



});
</script>
<style lang="less" scoped>
:deep(.ant-table) {
    height: 35vh !important;
}

:deep(.ant-table-container) {
    height: 35vh !important;
}

:deep(.ant-table-wrapper) {
    height: 35vh !important;
    // overflow: auto;
}

:deep(.ant-table-body) {
    height: 35vh !important;
    overflow: auto;
}

/* .tablewrapper {
    height: 50vh;
    overflow: scroll;
 
} */
</style>


