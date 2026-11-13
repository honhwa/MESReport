<template>
    <div class="EquipmentUtilization">
        <a-card>
        <a-row>
            <a-col :span="24 - PageParams.TableSpan" class="SHScrollForm" v-show="PageParams.ShowMenu">
                <SchemaForm @submit="confirm"> </SchemaForm>
            </a-col>
            <a-col :span="PageParams.TableSpan">
               
                <!-- <a-button type="primary" @click="changeView" v-show="PageSetting.List.length != 0">{{
                    PageSetting.buttonText
                }}</a-button> 
                <V3chart
                    width="100%"
                    height="100%"
                    :options="PageSetting.EchartOptions"
                    isClick
                    v-if="!PageSetting.isShowTable"
                ></V3chart>
                <div class="table" v-else> -->
                <div class="ScrollTable">
                    <DynamicTable
                        row-key="id"
                        :header-title="PageSetting.Title"
                        :columns="PageSetting.Columns"
                        showFavorite
                        sticky
                        :PageParams="PageParams"
                        :key="PageSetting.Columns"
                        bordered
                        size="small"
                        :data-source="PageSetting.List"
                        :data-all="PageSetting.List"
                        :pagination="false"
                        export-file-name="EquipmentUtilization"
                        :row-class-name="getRowClassName_sel"
                        :flexwidth="-10"
                    >
                        <template #export-button>
                            <a-button type="success"> 导出 </a-button>
                        </template>
                    </DynamicTable>
                <!-- </div>  -->
            </div>
            </a-col>
        </a-row>
    </a-card>
    </div>
</template>
<script lang="tsx" setup>
import { useForm } from '@/components/core/schema-form';
import { useTable } from '@/components/core/dynamic-table';
import { reactive, onMounted, ref } from 'vue';
import { cloneDeep } from 'lodash-es';
import useRemoteSelect from '@/pages/ReportPortal/utils/SHuseRemoteSelect';
import { getrowsum, Inserttotalrow, sortArray } from '@/pages/ReportPortal/utils/SHcommon';
import { message } from 'ant-design-vue';
import type { TableColumn } from '@/components/core/dynamic-table';
import { GetCommon } from '@/api/pages/common';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { useRouter, useRoute } from 'vue-router';
import V3chart from "@/components/charts/V3chart.vue";
import { Console } from 'console';
import { useLoading } from '@/components/feat/Loading';
import { PageParams } from '@/components/core/dynamic-table/src/dynamic-table'
import { ReportCommon } from '@/store/module/ReportCommon';
import { PovitParams, getPovitData } from "@/utils/reportcommon/provit";

const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});
const useCommonParams = AddCommonPageParams();
let useReportCommon = ReportCommon();
let bigfab=useCommonParams.Fab.slice(0,useCommonParams.Fab.length-1);
const QualityTable = ref<HTMLElement | null>(null);


const router = useRouter();
let ReportItem = reactive({
    // 首頁後端返回的sql以及sql參數
        APIMessage:'',
        // 左側請求參數
        FormParams: {},
        // 二級頁面所需參數
        FirstTableParams: {},
        // 其餘頁面所需參數
        SecondTableParams:reactive({}),
        ThirdTableParams:{},
        ThirdTableClickParams:{},
        //廠別
        Fab:'',
        G_FAC_COM:'',
});
let ReportNameParams = `EquipmentUtilization-${useCommonParams.Fab}`;
useReportCommon.AddObject(ReportNameParams,ReportItem);


function getImport(site: any) {
    const { schemas,  } = require(`@/pages/ReportPortal/SH/EquipmentUtilization/${bigfab}/form-schema`);
    const { baseColumns } = require(`@/pages/ReportPortal/SH/EquipmentUtilization/${bigfab}/table`);
    return { schemas, baseColumns };
}
const { schemas, baseColumns} = getImport(useCommonParams.Fab);
// 頁面數據
let PageSetting = reactive({
    List: [] as any,
    Columns: [] as any,
    EchartOptions: {},
    Message: {},
    isShowTable: false,
    buttonText: "切换表格",
    Title: "",

});
const [DynamicTable] = useTable();
const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas:schemas(useCommonParams.Fab,reactive({})),
    actionColOptions: {
        span: 24
    },
    layout: "vertical"
});

// function changeView() {
//     PageSetting.isShowTable = !PageSetting.isShowTable;
//     if (PageSetting.isShowTable) {
//         PageSetting.buttonText = "切换图表"
//     } else {
//         PageSetting.buttonText = "切换表格"
//     }
// }

function getRowClassName_sel(record: Object) {
    if (record && Object.keys(record).length != 0) {
        for (const [key, value] of Object.entries(record)) {
            if (typeof (value) == "string" && value.includes("Sum")) {
                return 'Green';
            }
        }
    }
}
async function confirm(values) {
    openFullLoading();
    PageSetting.Title=`${useCommonParams.Fab} Equipment Utilization`
    PageSetting.List = [];
    PageSetting.Columns = [];
    let TopTableParams = { name: "Date", apiName: "EquUtilizationDataSet", FactoryType: useCommonParams.Fab, G_FAC: useCommonParams.G_FAC_COM, };

    Object.keys(values).forEach((key) => {
        if (!values[key]) {
            values[key] = '';
        }
    });
   
    if (["equipShift", "equipment"].includes(values.Interval)&& values.Equipment.length == 0) {
        closeFullLoading();
        return message.warning("請選擇機台,無法show 全部機台")
    }

    try {
        const {
            Date: {
                Data: { Table: Data }, //
                Data_2: { Table: Data_2 }, //
                SQL01,
                SQL02,
                ToDate,
            }
        } = await GetCommon( {...TopTableParams, ...values});

        console.log("SQL01",SQL01)
        console.log("SQL02",SQL02)
        let middleColumns: Object[] = [];

        for (let item in Data[0]) {
            if(item.slice(0,1)=="A"){
                    middleColumns.push({
                    title: item.slice(1).replaceAll('_', '-'),
                    align: 'center',
                    dataIndex: item,
                    key: item,
                });
            }            
        }     
        
        PageSetting.Columns = [
            ...baseColumns,
            ...middleColumns,
            {
                title: '總計',
                align: 'center',
                dataIndex: 'CODE_SUMTIME',
                key: 'CODE_SUMTIME'
            },
            {
                title: '百分比',
                align: 'center',
                dataIndex: 'RATE',
                key: 'RATE'
            }
        ];
        
        Data_2.forEach(item => {
            item["C_DESC"] = item["ENTITY_STATUS"]
            item["CODE_SUMTIME"] = item["ZJ"]
            item["RATE"] = item["RATE"]+"%"
        })

        Data.forEach(item => {
            item["RATE"] = item["RATE"]+"%"
        })

        PageSetting.List=[...Data,...Data_2]

       
        closeFullLoading();
    } catch (error) {
        console.log(error);
        message.error('數據處理失敗!');
        closeFullLoading();
    }
    
}


</script>

<style lang="less" scoped>
.btn-rows {
    button {
        margin-right: 12px;
    }
}

.swapper {
    display: flex;
    flex-direction: column;
}

.EquipmentUtilization :deep(.test) {
    background-color: #80ff80 !important;

}
</style>
