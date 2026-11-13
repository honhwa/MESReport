<template>
    <div class="MTLConsume">
        <a-card>
         
            <a-row>
                <a-col :span="24 - PageParams.TableSpan" class="ScrollForm" v-show="PageParams.ShowMenu">
                    <SchemaForm @submit="confirm"> </SchemaForm>
                </a-col>
                <a-col :span="PageParams.TableSpan">
                    <DynamicTable
                        row-key="id"
                        header-title="LCMA Change Product Report "
                        showFavorite
                        sticky
                        :flexwidth="1"
                        :PageParams="PageParams"
                        :columns="PageSetting.TopColumns"
                        :key="PageSetting.TopColumns"
                        bordered
                        :row-class-name="getRowClassNmae"
                        :data-source="PageSetting.TopList"
                        :data-all="PageSetting.TopList"
                        :pagination="false"
                        export-file-name="LCMA Change Product Report "
                    >
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
import { reactive, ref } from '@vue/reactivity';
import { useForm } from '@/components/core/schema-form';
import { GetCommon } from '@/api/pages/common';
import {  onMounted } from '@vue/runtime-core';
import { getfinalsum} from "@/utils/reportcommon/Calculation";
import { cloneDeep } from 'lodash';
import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { baseColumns } from './LCM1/table';
import { getRowClassNmae } from '@/pages/ReportPortal/utils/common';
// import { dclick } from '@/utils/reportcommon/tableconmon';
import { dclick } from '@/pages/ReportPortal/utils/common';

import { MTLConsumeWithOut } from '@/store/module/MTLConsume';
import router from '@/router';
import { message } from 'ant-design-vue';

function getImport(site: any) {
    const { schemas, optionsdata } = require(`@/pages/ReportPortal/${site}/MTLConsume/LCM1/form-schema`);
    return { schemas, optionsdata, baseColumns };
}
const { schemas, optionsdata, } = getImport('FS');

// 頁面數據
let PageSetting = reactive({
    TopList: [] as any,
    TopColumns: [] as any,
    Message: {},


});
const [DynamicTable] = useTable();
const { mounted, InfiniteScroll, mountedLcmProdType, mountedOper, mountedEquip } = useRemoteSelect()


const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas:schemas(FactoryType),
    actionColOptions: {
        span: 24
    },
    fieldMapToTime: [['fieldTime', ['vDate_s', 'vDate_e1'], 'YYYY-MM-DD']],
    layout: "vertical"
});


let TopTableParams = reactive({
    name: "Date",
    apiName: "MaterialConsumption",
    ReportType: "MTLConsumeDataSet",
    FactoryType: FactoryType,
    G_FAC: "A",
    FromDate: "",
    ToDate: "",
    Shift: "",
    QueryType: "",
    WorkOrderlike: [],
    Product: [],
    Equipment: [],
    MAT_TYPE: [],
    PART_NBR: [],
    AlternativeMaterials: [],
});

async function clickfunc(record, rowindex, column, item, colindex) {
    let detailparams = cloneDeep(TopTableParams);
    detailparams["ReportType"] = "MTLConsumeDetail"
    UseMTL.AddALLFormParams({
        ...detailparams,
        "acct_Date": record["ACCT_DATE"],
        "wo_nbr":record["WO_NBR"] ,
        "mat_type": record["MAT_TYPE"],
        "mat_id": record["MAT_ID"],
        "prod_nbr": record["PROD_NBR"],
        "equip_nbr": record["EQUIP_NBR"],
    })
    router.push({name:"MTLConsumeDetail"});
   /*  console.log("record", record);
    console.log("rowindex", rowindex);
    console.log("column", column);
    console.log("item", item);
    console.log("colindex", colindex);
    await GetCommon({
        ...detailparams,
        "acct_Date": record["ACCT_DATE"],
        "wo_nbr":record["WO_NBR"] ,
        "mat_type": record["MAT_TYPE"],
        "mat_id": record["MAT_ID"],
        "prod_nbr": record["PROD_NBR"],
        "equip_nbr": record["EQUIP_NBR"],
        
    }) */


}
async function confirm() {
    PageSetting.TopList = [];
    PageSetting.TopColumns = [];
    let tempkeys = Object.keys(TopTableParams);
    for (let i in cloneDeep(dynamicFormRef.formModel)) {
        if (tempkeys.includes(i)) {
            if (i == "AlternativeMaterials" || i == "WorkOrderlike") {
                TopTableParams[i] = dynamicFormRef.formModel[i].split(",");
            } else {
                TopTableParams[i] = cloneDeep(dynamicFormRef.formModel[i]);
            }
        }
    }
    const {
        Date: {
            Data0: { Table: Data },
            Message: sqlmessage
        }
    } = await GetCommon(TopTableParams);
    if(Data.length ==0){
        PageSetting.TopList = [];
        return message.warn("無數據!")
    }
    Data.push(getfinalsum(Data, ['QTY', 'QTY0'], 'ACCT_DATE'));
    Data[Data.length - 1]['QTY'] = Data[Data.length - 1]['QTY'].toFixed(3);
    PageSetting.TopList = Data;
    let tempClolums = cloneDeep(baseColumns);
    dclick(tempClolums, ["WO_NBR","MAT_TYPE","MAT_ID","QTY","QTY0","PROD_NBR","EQUIP_NBR"], clickfunc);
    PageSetting.TopColumns = tempClolums;
}
async function getMatType(optionsdata: object, FactoryType: string) {
    const { Date: {
        Data: { Table: Data },
        Data1: { Table: Data1 }
    } } = await GetCommon({
        name: "Date",
        apiName: "Marterial",
        FactoryType: FactoryType,
        G_FAC: "A",
    })
    let MatTypeData: any[] = [];
    Data.forEach((item, index) => {
        if (index > 0) {
            MatTypeData.push({
                label: item["MAT_TYPE"],
                value: item["MAT_TYPE"],
            })
        }
    });
    MatTypeData.unshift({ label: "ALL", value: "ALL" });


    for (let i = Data1.length - 1; i >= 0; i--) {
        if (Data1[i]["MAT_TYPE"] == " ") {
            Data1.splice(i, 1);
        }
        delete Data1[i]["LEN"]
    }
    //源数据
    let BigMatTypeData = Data1;
    //展示的数据
    let BigMatTypeData1: any[] = [];
    Data1.forEach(item => {
        BigMatTypeData1.push(
            {
                label: item["PART_NBR"],
                value: item["PART_NBR"]
            }
        )
    })
    BigMatTypeData1.unshift({ label: "ALL", value: "ALL" })
    optionsdata["MatTypeData"] = MatTypeData;
    optionsdata["BigMatTypeData"] = BigMatTypeData;
    optionsdata["BigMatTypeData1"] = BigMatTypeData1;
    optionsdata["BigMatTypeData2"] = cloneDeep(BigMatTypeData1)
}
onMounted(async () => {
    // await mounted(optionsdata, FactoryType);
    await mountedOper(optionsdata, FactoryType);
    // await mountedEquip(optionsdata, FactoryType, 0);
    await getMatType(optionsdata, FactoryType);
});
</script>

<style lang="less" scoped>
.btn-rows {
    button {
        margin-right: 12px;
    }
}

.MTLConsume :deep(.sub-total) {
    background-color: #ffb468 !important;
}

.MTLConsume :deep(.total) {
    background-color: #ff80c0 !important;
}

.swapper {
    display: flex;
    flex-direction: column;
}
</style>
