<template>
    <div class="WOOrderStatusReport">

            <a-row>
                <a-col :span="24 - PageSetting.TableSpan" class="ScrollForm" v-show="PageParams.ShowMenu">
                    <SchemaForm @submit="confirm" > </SchemaForm>
                </a-col>
                <a-col :span="PageSetting.TableSpan">
                    
                        <!-- :style="{ display: 'table' }" -->
                        <DynamicTable
                        row-key="WOOrderStatusReport"
                        :header-title="PageSetting.Title"
                        :PageParams="PageParams"
                        id="WOOrderStatusReport"
                        export-type="Table"
                        :columns="PageSetting.Columns"
                        showFavorite
                        :key="PageSetting.Columns"
                        bordered
                        :flexwidth="1"
                        :row-class-name="getRowClassNmae"
                        :data-source="PageSetting.moreData"
                        :data-all="PageSetting.moreData"
                        sticky
                        export-file-name="Export"
                        :pagination="{
                                pageSize: 30,
                                pageSizeOptions: ['30', '80', '120', '160']
                            }"
                    >
                        <template #export-button>
                            <a-button type="success"> Export </a-button>
                        </template>
                    </DynamicTable>
                  
                </a-col>
            </a-row>
    </div>
</template>


<script lang="tsx" setup>
import { useLoading } from '@/components/feat/Loading';
import { PageParams } from '@/components/core/dynamic-table/src/dynamic-table'
import { useTable } from '@/components/core/dynamic-table';
import { reactive, ref } from '@vue/reactivity';
import { useForm } from '@/components/core/schema-form';
import { GetCommon } from '@/api/pages/common';
import { onMounted } from '@vue/runtime-core';
import { message } from 'ant-design-vue';
import { getfinalsum } from "@/utils/reportcommon/Calculation";
import { cloneDeep } from 'lodash';
import useRemoteSelect from '@/pages/ReportPortal/utils/TNuseRemoteSelect';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { dclick } from '@/pages/ReportPortal/utils/common'
import { ReportCommonWithOut } from '@/store/module/ReportCommon';
import router from '@/router';
import { mergeTableRow,getSumData } from '@/pages/ReportPortal/utils/common'
import { getrowsum} from "@/utils/reportcommon/Calculation";
//import {optionsdata} from '@/pages/ReportPortal/Static/TNCommon-Form-schema';

const UseParamsAdd = AddCommonPageParams()
const useReportCommon = ReportCommonWithOut();
let ReportParamsItem = reactive({
    APIMessage:'',
    FormParams: {},
    FirstTableParams: {},
    SecondTableParams:reactive({}),
    ThirdTableParams:{},
    ThirdTableClickParams:{},
    Fab:'',
    G_FAC_COM:'',
});
let ReportNameParams = `WOOrderStatusReportDetail_${UseParamsAdd.Fab}`;

let fab = "MOD";
if(UseParamsAdd.Fab == "MOD1")
{
    fab = UseParamsAdd.Fab;
}

let FactoryType: any = UseParamsAdd.Fab;

const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});
const WOOrderStatusReport = ref<HTMLElement | null>(null);

// 頁面數據
let PageSetting = reactive({
    List: [] as any,
    TableSpan: 20,
    showmenu: true,
    Columns: [] as any,
    moreData: [] as any,
    Title:'',
    SQLHidden:'',
    APIColumns: [] as any,
    HeadTitle:'',
});

PageSetting.Title=`${UseParamsAdd.Fab} WO Order Status Report`

function getImport(site: any, fab: any) {
    const { schemas } = require(`@/pages/ReportPortal/TN/WOOrderStatusReport/${fab}/form-schema`);
    const { baseColumns, CalcuList, ClickColumn } = require(`@/pages/ReportPortal/TN/WOOrderStatusReport/MOD/table`);
    return { schemas, baseColumns, CalcuList, ClickColumn };
}
const { schemas, baseColumns, CalcuList, ClickColumn } = getImport("TN",fab);
const [DynamicTable] = useTable();
const { mounted, InfiniteScroll } = useRemoteSelect()


const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas: schemas(FactoryType),
    actionColOptions: {
        span: 24
    },
    fieldMapToTime: [['fieldTime', ['vDate_s', 'vDate_e1'], 'YYYY-MM-DD']],
    layout: "vertical"
});


let TopTableParams = reactive({
    name: "Date",
    apiName: "TN_WOOrderStatusDataSet",
    FactoryType: FactoryType
});
function getRowClassNmae(record: Object) {
    if (record && Object.keys(record).length != 0) {
        for (const [key, value] of Object.entries(record)) {
            if (typeof (value) == "string" && ['小計', '合計'].includes(value)) {
                return 'Yellow';
            } else if (typeof (value) == "string" && ['總計', 'SUM', 'Sum'].includes(value)) {
                return 'Green';
            }
        }
    }
}

async function confirm(values) {
    openFullLoading();
    PageSetting.List = [];
    PageSetting.Columns = [];

    Object.keys(values).forEach((key) => {      
        if (!values[key]) {
            values[key] = '';
        }
       
    });

    for (let i in dynamicFormRef?.formModel) {
        if (i == 'WorkOrder') values[i] = dynamicFormRef.formModel[i].split(',');
        else {
            values[i] = dynamicFormRef.formModel[i];
        }
    }

    try {
        //let { api01 } = await GetCommon({...TopTableParams, ...values});
        const { Date: { Data: { Table: Data },Data_Non_Hold_Oper: { Table: Data_Non_Hold_Oper },Data_Hold_Oper: { Table: Data_Hold_Oper },SQLFilter, SQLWOFilter, Message: sqlmessage } } = await GetCommon({...TopTableParams, ...values});
        
        let children_Non_Hold: any[] = [];
        let children_Hold: any[] = [];

        let Message = {                
                SQLFilter: SQLFilter,
            };

        Data_Non_Hold_Oper.forEach((item) => {
            PageSetting.APIColumns.push(item['WORK_CTR']);
            children_Non_Hold.push({
                title: item['WORK_CTR'],
                align: 'center',
                dataIndex: item['WORK_DESC'],
                customCell: (record: Object, rowIndex: number, column: Object) => {
                            return {
                                ondblclick: () => {
                                    //UseParamsAdd.AddFirstTableParams({ ...record, ...column['__originColumn__'] });
                                    useReportCommon.AddFirstTableParams(`WOOrderStatusReportDetail_${UseParamsAdd.Fab}`,{ ...record, ...column['__originColumn__'] })
                                    router.push({ name: `WOOrderStatusReportDetail_${UseParamsAdd.Fab}`});
                                    
                                },
                                style: {
                                    cursor: 'pointer'
                                }
                            };
                        }
            });
        });

        Data_Hold_Oper.forEach((item) => {
            PageSetting.APIColumns.push(item['WORK_CTR']);
            children_Hold.push({
                title: item['WORK_CTR'],
                align: 'center',
                dataIndex: item['WORK_DESC'],
                customCell: (record: Object, rowIndex: number, column: Object) => {
                            return {
                                ondblclick: () => {
                                    //UseParamsAdd.AddFirstTableParams({ ...record, ...column['__originColumn__'] });
                                    useReportCommon.AddFirstTableParams(`WOOrderStatusReportDetail_${UseParamsAdd.Fab}`,{ ...record, ...column['__originColumn__'] })
                                    debugger
                                    router.push({ name: `WOOrderStatusReportDetail_${UseParamsAdd.Fab}`});
                                    
                                },
                                style: {
                                    cursor: 'pointer'
                                }
                            };
                        }
            });
        });
        
        baseColumns[31]['children'] = children_Non_Hold;
        baseColumns[32]['children'] = children_Hold;

        baseColumns[0]['customCell'] = (record: Object, rowIndex: number, column: Object) => {
                            return {
                                ondblclick: () => {
                                    //UseParamsAdd.AddFirstTableParams({ ...record, ...column['__originColumn__'] });
                                    useReportCommon.AddFirstTableParams(`WOOrderStatusReportDetail_${UseParamsAdd.Fab}`,{ ...record, ...column['__originColumn__'] })
                                    router.push({ name: `WOOrderStatusReportDetail_${UseParamsAdd.Fab}`});
                                    
                                },
                                style: {
                                    cursor: 'pointer'
                                }
                            };
                        }

        PageSetting.Columns = baseColumns;
        PageSetting.moreData = Data;
        PageSetting.List = PageSetting.moreData;

        if (PageSetting.moreData.length > 100) PageSetting.List = PageSetting.moreData.slice(0, 40);
        else PageSetting.List = PageSetting.moreData;
        closeFullLoading();

        ReportParamsItem.APIMessage = JSON.stringify(Message);
        ReportParamsItem.FormParams = { ...TopTableParams, ...values };
        useReportCommon.AddObject(ReportNameParams , ReportParamsItem)

    } catch (error) {
        closeFullLoading();
    }

}
InfiniteScroll(WOOrderStatusReport, PageSetting);
onMounted(
    async () => {
        // switch (FactoryType) {
        //     case "BEOL1":
        //     case "BEOL2":
        //         break;
        //     default:
        //         await mounted(optionsdata, FactoryType);
        //         break;
        // }

    }
)

</script>

<style lang="less" scoped>
.btn-rows {
    button {
        margin-right: 12px;
    }
}

.WOOrderStatusReport :deep(.sub-total) {
    background-color: #ffb468 !important;
}

.WOOrderStatusReport :deep(.total) {
    background-color: #ff80c0 !important;
}

.swapper {
    display: flex;
    flex-direction: column;
}
</style>
