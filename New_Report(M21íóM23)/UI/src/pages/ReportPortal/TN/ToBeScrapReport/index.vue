<template>
    <div class="ToBeScrap">
        <a-card>
  
            <a-row>
                <a-col :span="24 - PageParams.TableSpan" class="ScrollForm" v-show="PageParams.ShowMenu">
                    <SchemaForm @submit="confirm"> </SchemaForm>
                </a-col>
                <a-col :span="PageParams.TableSpan">
                    <DynamicTable
                        row-key="ToBeScrapReport"
                        :header-title="PageSetting.Title"
                        :PageParams="PageParams"
                        id="ToBeScrapReport"
                        export-type="Table"
                        :columns="PageSetting.Columns"
                        showFavorite
                        :key="PageSetting.Columns"
                        bordered
                        :flexwidth="1"
                        :row-class-name="getRowClassName"
                        :data-source="PageSetting.moreData"
                        :data-all="PageSetting.moreData"
                        :pagination="false"
                        sticky
                        export-file-name="Export"
                    >
                        <template #export-button>
                            <a-button type="success"> Export </a-button>
                        </template>
                    </DynamicTable>
                </a-col>
            </a-row>
        </a-card>
    </div>
</template>

<script lang="ts" setup>
import {PageParams} from '@/components/core/dynamic-table/src/dynamic-table'
import { getSumData } from '@/pages/ReportPortal/utils/TNcommon';
import { useForm } from '@/components/core/schema-form';
import { useTable } from '@/components/core/dynamic-table';
import { reactive, onMounted, ref, unref } from 'vue';
import useRemoteSelect from '@/pages/ReportPortal/utils/TNuseRemoteSelect';
import { GetCommon } from '@/api/pages/common';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { ReportCommon } from '@/store/module/ReportCommon';
import { useRouter } from 'vue-router';
import { getPovitData, PovitParams } from '@/utils/helper/povitHelper';
import { useLoading } from '@/components/feat/Loading';
//import {optionsdata} from '@/pages/ReportPortal/Static/TNCommon-Form-schema';

const router = useRouter();
const { mounted, InfiniteScroll } = useRemoteSelect();
const UseParamsAdd = AddCommonPageParams();
const useReportCommon = ReportCommon();
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
let ReportNameParams = `ToBeScrapReportDetail_${UseParamsAdd.Fab}`;

const ToBeScrap = ref<HTMLElement | null>(null);
function getImport(site: any) {
    //const { optionsdata } = require('@/pages/ReportPortal/Static/TNCommon-Form-schema');
    const { schemas } = require(`@/pages/ReportPortal/TN/ToBeScrapReport/MOD/form-schema`);
    const { ToBeScrapColumns } = require(`@/pages/ReportPortal/TN/ToBeScrapReport/MOD/Table`);
    return { schemas, ToBeScrapColumns };
}
let { schemas, ToBeScrapColumns } = getImport(UseParamsAdd.Fab);

const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});

let PageSetting = reactive({
    TableSpan: 20,
    showmenu: true,
    List: [] as any,
    Columns: [] as any,
    Message: {},
    moreData: [] as any,
    HeadTitle:'',
    SQLHidden:'',
    APIColumns: [] as any,
    Title:'',
});
let FactoryType: any = UseParamsAdd.Fab;
const [DynamicTable] = useTable();
const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas: schemas(FactoryType),
    actionColOptions: {
        span: 24
    },
    layout: 'vertical'
});
function getRowClassName(record: Object) {
    if (record && Object.keys(record).length != 0) {
        for (const [key, value] of Object.entries(record)) {
            if (typeof (value) == "string" &&  ['小计', '小計', '合計',"Total","Summary"].includes(value)) {
                return 'Yellow';
            } else if (typeof (value) == "string" && ['總計', 'SUM', '总计', 'Sum'].includes(value)) {
                return 'Green';
            } 
        }
    }
}

async function confirm(values) {
    openFullLoading();
    PageSetting.Columns = [];
    PageSetting.APIColumns = []
    let baseParams = { apiName: 'TN_ToBeScrapDataSet', FactoryType: UseParamsAdd.Fab, G_FAC: UseParamsAdd.G_FAC_COM };

    Object.keys(values).forEach((key) => {
        if (!values[key]) {
            values[key] = '';
        }
    });
    // PageSetting.Columns = MoveOutTimeColumns;
    PageSetting.Title=`${UseParamsAdd.Fab} To Be Scrap Report`
    let { api01 } = await GetCommon({ ...values, ...baseParams });
    let children: any[] = [];
    debugger
    api01['DataScrapDesc']['Table'].forEach((item) => {
        PageSetting.APIColumns.push(item['SCRAP_CODE']);
        children.push({
            title: item['SCRAP_CODE'],
            align: 'center',
            dataIndex: item['SCRAP_CODE'],
            children: [
                {
                    title: item['SCRAP_DESC'],
                    align: 'center',
                    dataIndex: item['SCRAP_CODE'],
                    customCell: (record: Object, rowIndex: number, column: Object) => {
                        return {
                            ondblclick: () => {
                                //UseParamsAdd.AddFirstTableParams({ ...record, ...column['__originColumn__'] });
                                useReportCommon.AddFirstTableParams(`ToBeScrapReportDetail_${UseParamsAdd.Fab}`,{ ...record, ...column['__originColumn__'] })
                                router.push({ name: `ToBeScrapDetail_${UseParamsAdd.Fab}`});
                                
                            },
                            style: {
                                cursor: 'pointer'
                            }
                        };
                    }
                }
            ]
        });
    });
    
    ToBeScrapColumns[2]['children'] = children;
    PageSetting.Columns = ToBeScrapColumns;
    let config: PovitParams = {
        Data: api01['Data']['Table'],
        povitColumn: {
            SCRAP_CODE: 'COUNT'
        },
        changeColumn: PageSetting.APIColumns,
        repeatColumn: ['PROD_NBR', 'VER']
    };

    let povitData = getPovitData(unref(config));
    
    povitData.forEach((data) => {
        let Total = 0;
        PageSetting.APIColumns.forEach((item) => {
            Total += isNaN(data[item]) ? 0 : data[item];
            // data['Total'] +=Total;
        });
        
        data['Yield'] =
            data['QTY'] / (data['QTY'] + Total) == Infinity
                ? '100%'
                : ((data['QTY'] * 100) / (data['QTY'] + Total)).toFixed(2) + '%';
                
        data['Total'] = Total;
    });
    let SumData = getSumData(povitData.length, 0, povitData, [...PageSetting.APIColumns,...['Total','QTY']]);
    let TotalYield = (SumData['QTY']*100/(SumData['QTY']+SumData['Total'])).toFixed(2)+'%'
    povitData.push({PROD_NBR:'Summary',...SumData,Yield:TotalYield})
    PageSetting.moreData = povitData;

    if (PageSetting.moreData.length > 100) PageSetting.List = PageSetting.moreData.slice(0, 40);
    else PageSetting.List = PageSetting.moreData;
    //UseParamsAdd.AddFormParams({ ...baseParams, ...values });
    //UseParamsAdd.AddAPIResultMessage(api01['Message']);
    closeFullLoading();
    ReportParamsItem.APIMessage = api01['SQLFilter'];
    ReportParamsItem.FormParams = { ...baseParams, ...values };
    useReportCommon.AddObject(ReportNameParams , ReportParamsItem)
}
//实现无限滚动或者说懒加载
InfiniteScroll(ToBeScrap, PageSetting);
onMounted(async () => {
    //await mounted(optionsdata,UseParamsAdd.Fab) 
});

</script>


