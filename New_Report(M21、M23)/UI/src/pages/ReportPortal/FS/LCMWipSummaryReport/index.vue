<template>
    <div class="LCMWipSummaryReport">
        <a-row>
            <a-col :span="24 - PageParams.TableSpan" class="ScrollForm" v-show="PageParams.ShowMenu">
                <SchemaForm @submit="confirm" :showTextPosition="'center'"> </SchemaForm>
            </a-col>
            <a-col :span="PageParams.TableSpan">
                <!-- 
                    :export-type="'Table'"

                 -->
                <DynamicTable
                    row-key="id"
                    id="LCMWipSummaryReport"
                    export-type="Exceljs"
                    :header-title="PageSetting.title"
                    showFavorite
                    :PageParams="PageParams"
                    :columns="PageSetting.TopColumns"
                    :key="PageSetting.TopColumns"
                    bordered
                    sticky
                    :data-source="PageSetting.moreData"
                    :data-all="PageSetting.moreData"
                    :pagination="false"
                    :flexwidth="1"
                    :export-file-name="PageSetting.title"
                >
                    <template #export-button>
                        <a-button type="success"> 导出 </a-button>
                    </template>
                </DynamicTable>
            </a-col>
        </a-row>
    </div>
</template>

<script lang="tsx" setup>
import { PageParams } from '@/components/core/dynamic-table/src/dynamic-table';
import { useTable } from '@/components/core/dynamic-table';
import { reactive, ref } from '@vue/reactivity';
import { useForm } from '@/components/core/schema-form';
import { GetCommon } from '@/api/pages/common';
import { onMounted } from '@vue/runtime-core';
import { getrowsum } from '@/utils/reportcommon/Calculation';
import { sortArray } from '@/pages/ReportPortal/utils/common';
import { cloneDeep } from 'lodash';
import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { getfinalsum } from '../../utils/common';
import { useLoading } from '@/components/feat/Loading';
import { message } from 'ant-design-vue';
const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});
const UseParamsAdd = AddCommonPageParams();
let FactoryType: any = UseParamsAdd.Fab;

function getImport(site: any) {
    const { schemas, optionsdata } = require(`@/pages/ReportPortal/FS/LCMWipSummaryReport/${site}/form-schema`);
    const {
        baseColumns,
        allColumns,
        holdandnoholdColumns
    } = require(`@/pages/ReportPortal/FS/LCMWipSummaryReport/${site}/table`);
    const { handledata } = require(`@/pages/ReportPortal/FS/LCMWipSummaryReport/${site}/handledata`);
    return { schemas, optionsdata, baseColumns, allColumns, holdandnoholdColumns, handledata };
}
const { schemas, optionsdata, baseColumns, allColumns, holdandnoholdColumns, handledata } = getImport(FactoryType);
// 頁面數據
let PageSetting = reactive({
    TopList: [] as any,
    TopColumns: [] as any,
    moreData: [],
    Message: {},
    title: 'LCMA Wip Summary Reportall'
});
const reportref = ref<HTMLElement | null>(null);

const [DynamicTable] = useTable();
const { mounted, mountedowner, mountedOper, InfiniteScroll } = useRemoteSelect();
const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas: schemas(FactoryType),
    actionColOptions: {
        span: 24
    },
    fieldMapToTime: [['fieldTime', ['vDate_s', 'vDate_e1'], 'YYYY-MM-DD']],
    layout: 'vertical'
});

let TopTableParams = reactive({
    name: 'Date',
    apiName: 'PPWipSumDataSet',
    FactoryType: FactoryType,
    G_FAC: 'A',
    vDate_e1: '',
    owner_code: [],
    EntyGroup: [],
    prod_nbr: []
});

async function confirm() {
    openFullLoading();
    try {
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
            TopTableParams['prod_nbr'] = dynamicFormRef.formModel.inputprod.split(/[\r\n\t]/).filter((item) => item);
        }
        const {
            Date: {
                Data: { Table: Data }, //表頭為no-hold的數據
                Data1: { Table: Data1 }, //留給hold做合并對象
                Data2: { Table: Data2 }, //表頭為hold的數據
                Data3: { Table: Data3 },
                Message: sqlmessage
            }
        } = await GetCommon(TopTableParams);
        if (Data.length == 0 && Data1.length == 0 && Data2 == 0) {
            closeFullLoading();
            return message.warn('無數據!');
        }

        // 处理non-hold
        let {
            non_hold: non_hold_columns,
            povitData: non_hold_data,
            calcuList: non_hold_calcuList
        } = handledata(Data, 'non-hold', 'WORK_CTR');
        // 处理hold,出現問題
        // Data2數據處理
        // 獲取Data2的數字
        let tempArr = Data2.map((item) => item['WORK_CTR']);
        /* tempArr.sort(function (a, b) {
        return a - b;
    }); */
        /* tempArr.forEach(titem => {
        Data1.forEach(item => {
            item["WORK_CTR"] = titem;
            item["QTY"] = 0;
        })
    }); */
        let tempholdData = [...Data2, ...Data1];
        tempholdData.forEach((item) => {
            item['WORK_CTR1'] = `hold${item['WORK_CTR']}`;
        });
        // console.log("測試", tempholdData);

        // 處理hold
        let {
            non_hold: hold_columns,
            povitData: hold_data,
            calcuList: hold_calcuList
        } = handledata(tempholdData, 'hold', 'WORK_CTR1');
        /* console.log('最終的hold—data', hold_data);
        console.log('最終的non_hold_data', non_hold_data); */

        switch (dynamicFormRef.formModel['type1']) {
            case 'all':
                {
                    // 数组合并
                    for (let i = 0; i < non_hold_data.length; i++) {
                        for (let j = 0; j < hold_data.length; j++) {
                            if (non_hold_data.length > 0) {
                                if (
                                    non_hold_data[i]['LCM_OWNER'] == hold_data[j]['LCM_OWNER'] &&
                                    non_hold_data[i]['PROD_NBR'] == hold_data[j]['PROD_NBR'] &&
                                    non_hold_data[i]['CUSTOMER'] == hold_data[j]['CUSTOMER']
                                ) {
                                    non_hold_data[i] = { ...non_hold_data[i], ...hold_data[j] };
                                }
                            }
                        }
                    }

                    // Data1合并Data2
                    // 上面的數組合并的

                    // 再次合并
                    /* for (let i = 0; i < non_hold_data.length; i++) {
                        for (let j = 0; j < Data2.length; j++) {
                            if (non_hold_data.length > 0) {
                                if (
                                    non_hold_data[i]['LCM_OWNER'] == Data2[j]['LCM_OWNER'] &&
                                    non_hold_data[i]['PROD_NBR'] == Data2[j]['PROD_NBR'] &&
                                    non_hold_data[i]['CUSTOMER'] == Data2[j]['CUSTOMER']
                                ) {
                                    non_hold_data[i] = { ...non_hold_data[i], ...Data2[j] };
                                }
                            }
                        }
                    } */

                    const result = hold_data.filter((item) => {
                        const flag = non_hold_data.some((data) => {
                            return (
                                data.LCM_OWNER === item.LCM_OWNER &&
                                data.PROD_NBR === item.PROD_NBR &&
                                data.CUSTOMER === item.CUSTOMER
                            );
                        });
                        if (!flag) {
                            return item;
                        }
                    });
                    result.forEach((item) => {
                        non_hold_data.push(item);
                    });

                    // scrap小計  scrap_sum
                    // all小計  all_sum
                    // 總計  all_totalsum = hold_sum + non-hold_sum
                    getrowsum(non_hold_data, ['hold_sum', 'non-hold_sum'], 'all_totalsum');
                    non_hold_data.push(
                        getfinalsum(
                            non_hold_data,
                            [
                                ...hold_calcuList,
                                ...non_hold_calcuList.map((item) => String(item)),
                                ...['scrap_sum', 'all_sum', 'all_totalsum']
                            ],
                            'LCM_OWNER'
                        )
                    );
                    sortArray(non_hold_data, 'PROD_NBR');
                    if (non_hold_data.length > 1) {
                        non_hold_data[non_hold_data.length - 1]['hold_sum'] =
                            non_hold_data[non_hold_data.length - 1]['holdfbsum'] +
                            non_hold_data[non_hold_data.length - 1]['holdreworksum'];
                        non_hold_data.forEach((item) => {
                            item['scrap_sum'] = 0;
                            item['all_sum'] = 0;
                        });
                    }

                    switch (FactoryType) {
                        /* 
                儅是LCM1和2的時候non-hold_sum = non-holdfbsum + non-holdreworksum
                 */
                        case 'LCM2':
                            if (non_hold_data.length > 1) {
                                non_hold_data[non_hold_data.length - 1]['non-hold_sum'] =
                                    non_hold_data[non_hold_data.length - 1]['non-holdfbsum'] +
                                    non_hold_data[non_hold_data.length - 1]['non-holdreworksum'];
                                non_hold_data[non_hold_data.length - 1]['hold_sum'] =
                                    non_hold_data[non_hold_data.length - 1]['holdbackwordsum'];
                            }
                            break;
                        case 'LCM1':
                            if (non_hold_data.length > 1) {
                                non_hold_data[non_hold_data.length - 1]['non-hold_sum'] =
                                    non_hold_data[non_hold_data.length - 1]['non-holdfbsum'] +
                                    non_hold_data[non_hold_data.length - 1]['non-holdreworksum'];
                            }
                            break;
                        default:
                            break;
                    }

                    PageSetting.TopColumns = [...baseColumns, non_hold_columns, hold_columns, allColumns];
                    // PageSetting.TopList = non_hold_data;
                    PageSetting.moreData = non_hold_data;
                   /*  console.log('輸出最終的數據:', PageSetting.moreData);
                    console.log('輸出最終TopColumns:', PageSetting.TopColumns); */
                    console.log('輸出最終TopColumns:', PageSetting.TopColumns); 

                }
                break;
            case 'hold':
                getrowsum(hold_data, ['hold_sum'], 'all_totalsum');
                hold_data.push(getfinalsum(hold_data, [...hold_calcuList, ...['all_totalsum']], 'LCM_OWNER'));
                hold_data[hold_data.length - 1]['hold_sum'] = hold_data[hold_data.length - 1]['all_totalsum'];
                PageSetting.TopColumns = [...baseColumns, hold_columns, holdandnoholdColumns];
                sortArray(hold_data, 'PROD_NBR');
                // PageSetting.TopList = hold_data;
                PageSetting.moreData = hold_data;
                if (PageSetting.moreData.length > 100) {
                    //预先加载40条数据
                    PageSetting.TopList = PageSetting.moreData.slice(0, 40);
                } else {
                    PageSetting.TopList = PageSetting.moreData;
                }
                /*  console.log("輸出最終處理的數據", hold_data);
             console.log("輸出最終的columsn", PageSetting.TopColumns); */

                // 刪除holdforwordsum，holdfbsum

                break;
            case 'non-hold':
                getrowsum(non_hold_data, ['non-hold_sum'], 'all_totalsum');
                non_hold_data.push(
                    getfinalsum(
                        non_hold_data,
                        [...non_hold_calcuList.map((item) => String(item)), ...['all_totalsum']],
                        'LCM_OWNER'
                    )
                );
                non_hold_data[non_hold_data.length - 1]['non-hold_sum'] =
                    non_hold_data[non_hold_data.length - 1]['all_totalsum'];
                PageSetting.TopColumns = [...baseColumns, non_hold_columns, holdandnoholdColumns];
                sortArray(non_hold_data, 'PROD_NBR');
                // PageSetting.TopList = non_hold_data;
                PageSetting.moreData = non_hold_data;
                if (PageSetting.moreData.length > 100) {
                    //预先加载40条数据
                    PageSetting.TopList = PageSetting.moreData.slice(0, 40);
                } else {
                    PageSetting.TopList = PageSetting.moreData;
                }
                break;
            default:
                break;
        }
        closeFullLoading();
    } catch (error) {
        console.log('輸出error', error);
        closeFullLoading();
    }
}
InfiniteScroll(reportref, PageSetting as any);

onMounted(async () => {
    await mountedowner(optionsdata, FactoryType);
    await mountedOper(optionsdata, FactoryType);
    await mounted(optionsdata, FactoryType);
});
</script>

<!-- <style scoped lang="less">
:deep(.ant-table) {
    height: 85vh !important;
}

:deep(.ant-table-container) {
    height: 85vh !important;
}

:deep(.ant-table-wrapper) {
    height: 85vh !important;
    // overflow: auto;
}

:deep(.ant-table-body) {
    height: 80vh !important;
    overflow: auto;
}
</style> -->
