<template>
    <div class="DayliMove">
        <a-row>
            <a-col :span="24 - PageParams.TableSpan" class="ScrollForm" v-show="PageParams.ShowMenu">
                <SchemaForm @submit="confirm"> </SchemaForm>
            </a-col>
            <a-col :span="PageParams.TableSpan">
                <div ref="WipPanelSummary">
                    <DynamicTable showFavorite row-key="WipPanelSummary" header-title="Wip Panel Summary " sticky
                        :PageParams="PageParams" :columns="PageSetting.Columns" :key="PageSetting.Columns" bordered
                        size="small" :data-source="PageSetting.moreData" :data-all="PageSetting.moreData"
                        :pagination="false" export-file-name="导出" :flexwidth="1">
                        <template #export-button>
                            <a-button type="success"> 导出 </a-button>
                        </template>
                    </DynamicTable>
                </div>
            </a-col>
        </a-row>
    </div>
</template>

<script lang="ts" setup>
import {PageParams} from '@/components/core/dynamic-table/src/dynamic-table'
import { useForm } from '@/components/core/schema-form';
import { useTable } from '@/components/core/dynamic-table';
import { reactive, onMounted, ref } from 'vue';
import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect';
import { message } from 'ant-design-vue';
import type { TableColumn } from '@/components/core/dynamic-table';
import { GetCommon } from '@/api/pages/common';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { useRouter, useRoute } from 'vue-router';
import { useLoading } from '@/components/feat/Loading';
import { isEmpty } from '@/utils/common';

// import { useInfiniteScroll, useVirtualList } from '@vueuse/core'

const router = useRouter();
const route = useRoute();
const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});
const { mounted, InfiniteScroll, mountedOper, mountedLcmProdType, mountedHoldCode } = useRemoteSelect();
const UseParamsAdd = AddCommonPageParams();
const WipPanelSummary = ref<HTMLElement | null>(null);
function getImport(site: any) {
    let { optionsdata, GrpOptions ,schemas} = require(`@/pages/ReportPortal/FS/WipPanelSummary/${site}/form-schema`);
    return { schemas, optionsdata, GrpOptions };
}
let { schemas, optionsdata, GrpOptions } = getImport(UseParamsAdd.Fab);

let PageSetting = reactive<BasePageParams.BasePageSetting>({
    List: [],
    APIColumns: [],
    Columns: [] as any,
    moreData: []
});
const [DynamicTable] = useTable();
const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas: schemas(UseParamsAdd.Fab),
    actionColOptions: {
        span: 24
    },
    layout: 'vertical'
});
const customCellRender = (record: Object, rowIndex: number, column: Object) => {
    return {
        ondblclick: () => {
            UseParamsAdd.AddFirstTableParams({ ...record, ...column['__originColumn__'] });
            router.push({ name: 'WipPanelSummaryDetailDetail' + UseParamsAdd.Fab });
        },
        style: {
            cursor: 'pointer'
        }
    };
};
async function confirm(values) {
    let dynamicColumns: TableColumn[] = [];
    let baseParams = {
        apiName: 'WipPanelSummary',
        FactoryType: UseParamsAdd.Fab,
        G_FAC: 'A',
        scolname: '',
        sqlcond: ''
    };
    let FGrpOptions = GrpOptions.filter((item) => item.label != 'ALL');
    if (values.Colname && values.Colname.length > 0) {
        values.Colname.forEach((item) => {
            let findItem = FGrpOptions.find(fitem => {
              return  fitem.value == item
            } );
            if (findItem) {
                dynamicColumns.push({
                    title: findItem.label,
                    dataIndex: findItem['dataIndex'],
                    align: 'center',
                    customCell: customCellRender
                });
            }

        });
        Object.keys(values).forEach((key) => {
            if (!values[key]) {
                values[key] = '';
            } else if (key == 'inputprod') {
                values['Product'] = values[key].split(/[\r\n\t]/).filter((item) => item);
            }
        });
    } else {
        return message.warn("請輸入展示欄位!")
    }
    let finalColumns = [
        {
            title: 'SUB_Qty',
            align: 'center',
            dataIndex: 'SUB',
            customCell: customCellRender
        }
    ];
    switch (UseParamsAdd.Fab) {
        case "LCM2":
            finalColumns.push({
                title: '在RW',
                align: 'center',
                dataIndex: 'ML',
                customCell: customCellRender
            })
            break;

        default:
            break;
    }
    PageSetting.Columns = [...dynamicColumns, ...finalColumns];


    try {
        closeFullLoading();
    } catch (error) {
        closeFullLoading();

    }
    let { api01 } = await GetCommon({ ...baseParams, ...values });
    PageSetting.moreData = api01['Data0']['Table'];
    if(PageSetting.moreData.length == 0){
        return message.warn("無數據!")
    }
    switch (UseParamsAdd.Fab) {
        case "LCM2":
        PageSetting.moreData.forEach(item=>{
            item["WO_NBR"] = item["NVL(RTRIM(WO_NBR),'')"];
            item["PROD_NBR"] = item["NVL(RTRIM(PROD_NBR),'')"];
        })
            break;

        default:
            break;
    }
    UseParamsAdd.AddFormParams({ ...baseParams, ...values });
    UseParamsAdd.AddAPIResultMessage(api01['Message']);
    if (PageSetting.moreData.length > 100) {
        PageSetting.List = PageSetting.moreData.slice(0, 40);
    } //预先加载40条数据
    else {
        PageSetting.List = PageSetting.moreData;
    }
}
//实现无限滚动或者说懒加载
InfiniteScroll(WipPanelSummary, PageSetting);
onMounted(async () => {
    //获取Size,bigproduct,prodcut数据
    // await mounted(optionsdata, UseParamsAdd.Fab);
    //  LCM2使用LCM1的operaction
    /*  switch (UseParamsAdd.Fab) {
         case 'LCM1':
         case 'LCM2':
             await mountedOper(optionsdata, 'LCM1');
             break;
         default:
             await mountedOper(optionsdata, UseParamsAdd.Fab);
             break;
     } */
    // await mountedLcmProdType(optionsdata, UseParamsAdd.Fab);
    // await mountedHoldCode(optionsdata, UseParamsAdd.Fab);
    TableHeight.value = Math.round(document.documentElement.clientHeight) - 220;
});
</script>
