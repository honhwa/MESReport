<script lang="ts" setup>
import { PageParams } from '@/components/core/dynamic-table/src/dynamic-table'
import { getSumData } from '@/pages/ReportPortal/utils/common';
import { useForm } from '@/components/core/schema-form';
import { useTable } from '@/components/core/dynamic-table';
import { reactive, onMounted, ref, unref } from 'vue';
import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect';
import { GetCommon } from '@/api/pages/common';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { useRouter } from 'vue-router';
import { getPovitData, PovitParams } from '@/utils/helper/povitHelper';
import { GetLCMProductSizeData, GetLcdParamsData } from '@/pages/ReportPortal/utils/useFormParams'


const router = useRouter();
const { mounted, InfiniteScroll } = useRemoteSelect();
const UseParamsAdd = AddCommonPageParams();
const WipPanelSummary = ref<HTMLElement | null>(null);
function getImport(site: any) {
    let { optionsdata } = require('@/pages/ReportPortal/FS/PanelWipStayChart/LCM1/form-schema');
    const { schemas } = require(`@/pages/ReportPortal/FS/ToBeScrapReport/${site}/form-schema`);
    const { ToBeScrapColumns } = require(`@/pages/ReportPortal/FS/ToBeScrapReport/${site}/Table`);
    return { schemas, optionsdata, ToBeScrapColumns };
}
let { schemas, optionsdata, ToBeScrapColumns } = getImport(UseParamsAdd.Fab);

let PageSetting = reactive<BasePageParams.BasePageSetting>({
    List: [],
    APIColumns: [],
    Columns: [],
    moreData: []
});
const [DynamicTable] = useTable();
const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas,
    actionColOptions: {
        span: 24
    },
    layout: 'vertical'
});
function getRowClassName(record: Object) {
    if (record && Object.keys(record).length != 0) {
        for (const [key, value] of Object.entries(record)) {
            if (typeof (value) == "string" && ['小计', '小計', '合計', "Total", "Summary"].includes(value)) {
                return 'Yellow';
            } else if (typeof (value) == "string" && ['總計', 'SUM', '总计', 'Sum'].includes(value)) {
                return 'Green';
            }
        }
    }
}

async function confirm(values) {
    PageSetting.Columns = [];
    PageSetting.APIColumns = []
    let baseParams = { apiName: 'ToBeScrap', FactoryType: UseParamsAdd.Fab, G_FAC: UseParamsAdd.G_FAC_COM };

    Object.keys(values).forEach((key) => {
        if (!values[key]) {
            values[key] = '';
        }
    });
    // PageSetting.Columns = MoveOutTimeColumns;
    PageSetting.Title = 'To Be Scrap Report';
    let { api01 } = await GetCommon({ ...values, ...baseParams });
    let children: any[] = [];
    api01['Data1']['Table'].forEach((item) => {
        PageSetting.APIColumns.push(item['SCRAP_CODE']);
        children.push({
            title: item['SCRAP_CODE'],
            align: 'center',
            dataIndex: item['SCRAP_CODE'],
            customCell: (record: Object, rowIndex: number, column: Object) => {
                return {
                    ondblclick: () => {
                        UseParamsAdd.AddFirstTableParams({ ...record, ...column['__originColumn__'] });
                        switch (UseParamsAdd.Fab) {
                            case "LCM1":
                                router.push({ name: 'ToBecrapReportCM1MFGDetail' });
                                break;
                            case "LCM2":
                                router.push({ name: 'ToBecrapReportCM2MFGDetail' });
                                break;
                        }
                    },
                    style: {
                        cursor: 'pointer'
                    }
                };
            }
        });
    });
    ToBeScrapColumns[2]['children'] = children;
    PageSetting.Columns = ToBeScrapColumns;
    let config: PovitParams = {
        Data: api01['Data0']['Table'],
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
    let SumData = getSumData(povitData.length, 0, povitData, [...PageSetting.APIColumns, ...['Total', 'QTY']]);
    let TotalYield = (SumData['QTY'] * 100 / (SumData['QTY'] + SumData['Total'])).toFixed(2) + '%'
    povitData.push({ PROD_NBR: 'Summary', ...SumData, Yield: TotalYield })
    PageSetting.moreData = povitData;

    if (PageSetting.moreData.length > 100) PageSetting.List = PageSetting.moreData.slice(0, 40);
    else PageSetting.List = PageSetting.moreData;
    UseParamsAdd.AddFormParams({ ...baseParams, ...values });
    UseParamsAdd.AddAPIResultMessage(api01['Message']);
}
//实现无限滚动或者说懒加载
InfiniteScroll(WipPanelSummary, PageSetting);
onMounted(async () => {
    //获取Size,bigproduct,prodcut数据
    optionsdata = await mounted(optionsdata, UseParamsAdd.Fab);
    await GetLcdParamsData(optionsdata);
    optionsdata.LCMProductSize = await GetLCMProductSizeData('LCMProductSize_TBSR');
    optionsdata.LCMProductType = await GetLCMProductSizeData('LCMProductType');
});

</script>

<template>
    <div class="DayliMove">

        <a-row>
            <a-col :span="24 - PageParams.TableSpan" class="ScrollForm" v-show="PageParams.ShowMenu">
                <SchemaForm @submit="confirm"> </SchemaForm>
            </a-col>
            <a-col :span="PageParams.TableSpan">
                <div class="ScrollTable" ref="ToBeScrapReport">
                    <DynamicTable sticky showFavorite export-type="Table" :id="PageSetting.Title" :flexwidth="1"
                        :PageParams="PageParams" row-key="ToBeScrapReport" :header-title="PageSetting.Title"
                        :columns="PageSetting.Columns" :key="PageSetting.Columns" bordered :data-source="PageSetting.List"
                        :data-all="PageSetting.moreData" :pagination="false" :export-file-name="PageSetting.Title"
                        :row-class-name="getRowClassName">
                        <template #export-button>
                            <a-button type="success"> 导出 </a-button>
                        </template>
                    </DynamicTable>
                </div>
            </a-col>
        </a-row>
    </div>
</template>
