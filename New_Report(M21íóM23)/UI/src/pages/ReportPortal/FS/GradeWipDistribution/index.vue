<template>
    <div>
        <a-row>
            <a-col :span="24 - PageParams.TableSpan" class="ScrollForm" v-show="PageParams.ShowMenu">
                <SchemaForm @submit="confirm" class="ScrollForm"> </SchemaForm>
            </a-col>
            <a-col :span="PageParams.TableSpan">
                <DynamicTable
                    row-key="reportref"
                    header-title="LCMA Grade Wip Distribution"
                    :key="PageSetting.TopColumns"
                    showFavorite
                    sticky
                    export-type="Multi"
                    :PageParams="PageParams"
                    :columns="PageSetting.TopColumns"
                    :data-source="PageSetting.TopList"
                    :data-all="PageSetting.TopList"
                    :pagination="false"
                    export-file-name="LCMA Grade Wip Distribution"
                    :export-header="exportHeader"
                    :flexwidth="1"
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
import { reactive, ref, unref } from '@vue/reactivity';
import { useForm } from '@/components/core/schema-form';
import { GetCommon } from '@/api/pages/common';
import { onMounted } from '@vue/runtime-core';
import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { message } from 'ant-design-vue';
import { getPovitData, PovitParams } from '@/utils/helper/povitHelper';
import { useRouter } from 'vue-router';
import { useLoading } from '@/components/feat/Loading';
import { mergeTableRow1 } from '@/pages/ReportPortal/utils/common';

const UseParamsAdd = AddCommonPageParams();
const { mounted } = useRemoteSelect();
const router = useRouter();
const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});
function getImport(site: any) {
    let { optionsdata } = require(`@/pages/ReportPortal/FS/PanelWipStayChart/LCM1/form-schema`);
    const { schemas, getSchemaData } = require(`@/pages/ReportPortal/FS/GradeWipDistribution/${site}/form-schema`);
    return { schemas, optionsdata, getSchemaData };
}
let { schemas, optionsdata, getSchemaData } = getImport(UseParamsAdd.Fab);

const GradeWipLabel: string[] = ['Product', 'LCM Grade', 'WO CUST', 'PACK CUST'];
const GradeWipProps: string[] = ['PROD_NBR', 'LCM_GRADE', 'WO_CUST', 'CUSTOMER_GROUP'];
let basecolumns = GradeWipLabel.map((item: string, index: number) => {
    return {
        title: item,
        dataIndex: GradeWipProps[index],
        align: 'center',
        className:'v-center',
        customCell: (record: Object, rowIndex, column) => {
            return {
                rowSpan: record?.[`${column['dataIndex']}-span`]?.['rowspan'] || 0,
                colSpan: record?.[`${column['dataIndex']}-span`]?.['colspan'] || 0
            };
        }
    };
});
let exportHeader = [
    [
        'Product',
        'LCM Grade',
        'WO CUST',
        'PACK CUST',
        'Non Hold',
        'Non Hold',
        'Non Hold',
        'Non Hold',
        'Non Hold',
        'Non Hold',
        'Hold',
        'Hold',
        '合计'
    ]
];

// 頁面數據
let PageSetting = reactive({
    TopList: [] as any,
    TopColumns: [] as any,
    HoldFlag: [] as any,
    NonHoldLength: 0
});

const [DynamicTable] = useTable();
const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas: schemas(UseParamsAdd.Fab),
    actionColOptions: {
        span: 24
    },
    fieldMapToTime: [['fieldTime', ['vDate_s', 'vDate_e1'], 'YYYY-MM-DD']],
    layout: 'vertical'
});
async function confirm(values) {
    openFullLoading();

    /**values为所设定的所有参数,将values里为undefined的值赋为空*/
    Object.keys(values).forEach((key) => {
        if (!values[key]) {
            values[key] = '';
        }
    });
    let baseParams = { apiName: 'GradeWipDistDataSet', FactoryType: 'LCM1', G_FAC: 'A' };
    /**组装api所需参数并发起請求并解构赋值 */
    try {
        let { api01 } = await GetCommon({ ...baseParams, ...values });
        let Data = api01['Data']['Table'];
        let PanelFormParams = api01['Message'];
        if (Data.length <= 0) {
            closeFullLoading();
            return message.warning('當前條件下無數據!');
        }
        /**将API回传参数存进共用的vuex中*/
        UseParamsAdd.AddAPIResultMessage(PanelFormParams);
        /**将表单参数存进共用的vuex中*/
        UseParamsAdd.AddFormParams(values);
        HandleColumns(Data);
        HandleData(Data);
        closeFullLoading();
    } catch {
        closeFullLoading();
    }
}
/**
 *
 * @param Data 处理原始数据进行行列转换
 */
function HandleData(Data: Object[]) {
    let config: PovitParams = {
        Data,
        povitColumn: {
            HOLD_FLAG: 'COUNT'
        },
        changeColumn: PageSetting.HoldFlag,
        repeatColumn: GradeWipProps
    };
    let povitData = getPovitData(unref(config)); //进行类行列转置的动作
    HandleSumData(povitData, PageSetting.HoldFlag, 'Total'); //总计
    HandleSumData(povitData, PageSetting.HoldFlag.slice(0, PageSetting.NonHoldLength), 'N小计'); //NonHold小计
    HandleSumData(
        povitData,
        PageSetting.HoldFlag.slice(PageSetting.NonHoldLength, PageSetting.HoldFlag.length),
        'Y小计'
    ); //Hold小计
    let combineCol1: any = {
        data: povitData,
        mergeColNames: ['PROD_NBR', 'LCM_GRADE','WO_CUST', 'CUSTOMER_GROUP'],
        firstMergeColNames: ['PROD_NBR'],
        firstMerge: 'PROD_NBR'
    };
    let Mergedata = mergeTableRow1(combineCol1);
    PageSetting.TopList = Mergedata;

    // PageSetting.moreData = getTableData(povitData)//进行表格合并
}
/**
 *
 * @param Data API返回的数据
 * 进行表头列数据处理,添加小计列
 */
function HandleColumns(Data: Object[]) {
    //添加的一级表头
    let FirstColumns = ['Non Hold', 'Hold'];
    //API返回的Hold与NonHold的列数
    PageSetting.HoldFlag = [];
    //获取API返回的Hold与NonHold的列数
    Data.forEach((item: any) => {
        if (!PageSetting.HoldFlag.includes(item['HOLD_FLAG'])) {
            PageSetting.HoldFlag.push(item['HOLD_FLAG']);
        }
    });
    //进行排序
    PageSetting.HoldFlag.sort();
    //最终要呈现在页面中的列
    let HoldFlagColumns: any = [];
    //添加一级表头
    FirstColumns.forEach((item: string) => {
        HoldFlagColumns.push({
            title: item,
            align: 'center',
            dataIndex: item,
            children: []
        });
    });
    //获取API返回的Hold与NonHold的列数
    PageSetting.HoldFlag.forEach((item) => (item[0] == 'N' ? (PageSetting.NonHoldLength += 1) : ''));
    //添加具体列与小计列
    PageSetting.HoldFlag.forEach((item: string, index: number) => {
        // item.substring(0,1)=='N'
        //第一个字符为N代表NonHold，为Y代表Hold
        let HoldNum = item[0] == 'N' ? 0 : 1;
        let isHold = item[0];
        HoldFlagColumns[HoldNum]['children'].push({
            title: item.replace(/\w/, ''),
            align: 'center',
            dataIndex: item,
            customCell: (record: Object, rowIndex: number, column: Object) => {
                return {
                    onDblclick: () => {
                        UseParamsAdd.AddFirstTableParams({ ...record, ...column['__originColumn__'] });
                        router.push({ name: 'GradeWipDisDetail' });
                    },
                    style: {
                        cursor: 'pointer'
                    }
                };
            }
        });
        //先添加具体列再添加小计列
        if (index == PageSetting.NonHoldLength - 1 || index == PageSetting.HoldFlag.length - 1) {
            HoldFlagColumns[HoldNum]['children'].push({
                title: '小计',
                align: 'center',
                dataIndex: `${isHold}小计`,
                className: 'v-lightblue',
                customCell: (record: Object, rowIndex: number, column: Object) => {
                    return {
                        ondblclick: () => {
                            UseParamsAdd.AddFirstTableParams({
                                ...record,
                                ...column['__originColumn__'],
                                ...{ WORK_CTR: '小計' }
                            });
                            router.push({ name: 'GradeWipDisDetail' });
                        },
                        style: {
                            cursor: 'pointer'
                        }
                    };
                }
            });
        }
    });
    PageSetting.TopColumns = [
        ...basecolumns,
        ...HoldFlagColumns,
        {
            title: '总计',
            align: 'center',
            dataIndex: 'Total',
            className: 'v-yellow',
            customCell: (record: Object, rowIndex: number, column: Object) => {
                return {
                    ondblclick: () => {
                        UseParamsAdd.AddFirstTableParams({
                            ...record,
                            ...column['__originColumn__'],
                            ...{ WORK_CTR: '合計' }
                        });
                        router.push({ name: 'GradeWipDisDetail' });
                    },
                    style: {
                        cursor: 'pointer'
                    }
                };
            }
        }
    ];
}
/**
 *
 * @param Data 原始数据
 * @param SumData 要进行计算的列
 * @param Title 列属性设置
 */
function HandleSumData(Data: any[], SumData: any[], Title: string) {
    Data.forEach((item: Object) => {
        item[Title] = getSum(item, SumData);
    });
    function getSum(columnData: Object, SumData: any[]) {
        let sum = 0;
        for (let i of SumData) {
            let a = isNaN(columnData[i]) ? 0 : columnData[i];
            sum = sum + a;
        }
        return sum;
    }
}

onMounted(async () => {
    optionsdata = await mounted(optionsdata, UseParamsAdd.Fab);
});
</script>

<style scoped lang="less">
:deep(.v-lightblue) {
    background-color: lightblue;
}

:deep(.v-yellow) {
    background-color: lightgreen;
}
</style>
