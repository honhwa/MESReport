<script lang="ts" setup>
import { PageParams } from '@/components/core/dynamic-table/src/dynamic-table';
import { getRowClassNmae, mergeTableRow } from '@/pages/ReportPortal/utils/common';
import { useForm } from '@/components/core/schema-form';
import { useTable } from '@/components/core/dynamic-table';
import { h, reactive, ref } from 'vue';
import { GetCommon } from '@/api/pages/common';
import { ReportCommon } from '@/store/module/ReportCommon';
import { useLoading } from '@/components/feat/Loading';
import { message } from 'ant-design-vue';
import { megercell, dclick } from '@/pages/ReportPortal/utils/common';
import { cloneDeep } from 'lodash';
import router from '@/router';

const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});

const UseParamsAdd = ReportCommon();
let FactoryType: string = UseParamsAdd.Fab;
let SubFab: string = UseParamsAdd.SubFab;

const EntityStatusSummary = ref<HTMLElement | null>(null);
function getImport(site: any) {
    const { schemas } = require(`@/pages/ReportPortal/TN/EntityStatusSummary/${site}/form-schema`);
    const { baseColumns } = require(`@/pages/ReportPortal/TN/EntityStatusSummary/${site}/Table`);
    return { schemas, baseColumns};
}
let { schemas, baseColumns} = getImport("MOD");

let PageSetting = reactive<BasePageParams.BasePageSetting>({
    List: [],
    APIColumns: [],
    Columns: [],
    moreData: []
});
const [DynamicTable] = useTable();
const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas: schemas(FactoryType),
    actionColOptions: {
        span: 24
    },
    layout: 'vertical'
});
async function click(record, rowindex, column, item, apiparams) {
    // 跳轉到detail2
    let detailparams = cloneDeep(apiparams);
    detailparams['ReportType'] = 'EntityStaSumDetail';
    detailparams['Equipment'] = record['Entity'];
    detailparams['equip'] = record['Group'];
    await GetCommon(detailparams);
    UseParamsAdd.AddFirstTableParams('' + FactoryType, detailparams);
    router.push({ name: '' + FactoryType });
}
async function confirm(values) {
    openFullLoading();
    PageSetting.Columns = [];
    let baseParams = {
        apiName: 'TN_EntityStatusSummaryDataSet',
        FactoryType,
        G_FAC: UseParamsAdd.G_FAC_COM
    };
    Object.keys(values).forEach((key) => {
        if (!values[key]) {
            values[key] = '';
        }
    });

    console.log("輸出參數", values);
   
    try {
        let { api01 } = await GetCommon({ ...values, ...baseParams });
        let Data = api01['Data']['Table'];
        if (Data.length) {
            PageSetting.Columns = baseColumns;
           
            megercell(PageSetting.Columns, ['Group', 'Entity'], Data);
            let clickkey = Object.keys(Data[0]).slice(4);
            PageSetting.Columns.forEach((item) => {
                if (item['children']) {
                    // 二階遍歷children
                    item['children'].forEach((c1item) => {
                        if (c1item['children']) {
                            c1item['children'].forEach((c2item) => {
                                c2item['customCell'] = (record: Object, rowindex: number, column: Object) => {
                                    let stylefunc = () => {
                                        let tempStyle = {};
                                        if (clickkey.includes(c2item['dataIndex'])) {
                                            tempStyle = { cursor: 'pointer' };
                                        }
                                        return { ...tempStyle };
                                    };
                                    return {
                                        onDblclick: async () => {
                                            if (clickkey.includes(c2item['dataIndex'])) {
                                                click(record, rowindex, column, c2item, { ...values, ...baseParams });
                                            } else {
                                                message.warning('此栏位没有二阶数据');
                                            }
                                        },
                                        style: stylefunc()
                                    };
                                };
                                c2item['customRender'] = ![' 1', 'SUM', ' 3', ' 8', 'SUM1'].includes(c2item.dataIndex)
                                    ? null
                                    : ({ text, record, index, column }) => {
                                        /*  console.log("自定義渲染text", text);
                                  console.log("自定義渲染record", record);
                                  console.log("自定義渲染index", index);
                                  console.log("自定義渲染column", column); */
                                        return h(
                                            'div',
                                            {
                                                style: {
                                                    color: 'red',
                                                    cursor: 'pointer'
                                                },
                                                class: 'name'
                                            },
                                            Number(text).toFixed(2)
                                        );
                                    };
                            });
                        }
                    });
                }
            });
            console.log(clickkey);

            console.log('最終的columsn', PageSetting.Columns);

            PageSetting.moreData = Data;
            if (PageSetting.moreData.length > 100) {
                PageSetting.List = PageSetting.moreData.slice(0, 40);
            } //预先加载40条数据
            else {
                PageSetting.List = PageSetting.moreData;
            }
        } else {
            closeFullLoading();
            return message.warn('無數據!');
        }

        closeFullLoading();
    } catch {
        closeFullLoading();
    }
}
//实现无限滚动或者说懒加载
// InfiniteScroll(EntityStatusSummary, PageSetting);
</script>

<template>
    <div class="DayliMove">
        <a-card>
            <a-row>
                <a-col :span="24 - PageParams.TableSpan" class="ScrollForm" v-show="PageParams.ShowMenu">
                    <SchemaForm @submit="confirm" ></SchemaForm>
                </a-col>
                <a-col :span="PageParams.TableSpan">
                        <DynamicTable showFavorite row-key="EntityStatusSummary" :PageParams="PageParams"
                        :flexwidth="1"
                            :header-title="PageSetting.Title" :columns="PageSetting.Columns" :key="PageSetting.Columns"
                            :data-all="PageSetting.moreData" bordered  :data-source="PageSetting.moreData"
                            :pagination="undefined" export-file-name="Export" :row-class-name="getRowClassNmae">
                            <template #export-button>
                                <a-button type="success"> Export </a-button>
                            </template>
                        </DynamicTable>
                </a-col>
            </a-row>
        </a-card>
    </div>
</template>