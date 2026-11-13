<template>
    <div class="DayliMove">
        <Loading
            :loading="compState.loading"
            :absolute="compState.absolute"
            :theme="compState.theme"
            :background="compState.background"
            :tip="compState.tip"
        />
        <a-card>
            <!--<a-row>
                <span class="menu-fold" @click="showHidden(PageSetting)">
                    <component :is="!PageParams.ShowMenu ? MenuUnfoldOutlined : MenuFoldOutlined" />
                </span>
            </a-row>-->
            <a-row>
                <a-col :span="24 - PageParams.TableSpan" class="ScrollForm"
                    Source-show="PageParams.ShowMenu">
                    <div class="swapper">
                        <SchemaForm @submit="confirm"> </SchemaForm>
                    </div>
                </a-col>
                <a-col :span="PageParams.TableSpan">
                    <div class="DaylimoveTable" ref="DaylimoveTable">
                        <DynamicTable row-key="id" header-title="Daily Move" :columns="PageSetting.Columns" sticky
                            :key="PageSetting.Columns" bordered size="small" :data-source="PageSetting.List"
                            :data-all="PageSetting.moreData" :pagination="false" export-file-name="Export"
                            :scroll="{x:1500,y:350}" :PageParams="PageParams"
                            :row-class-name="getRowClassNmae">
                            <template #export-button>
                                <a-button type="success"> Export </a-button>
                            </template>
                        </DynamicTable>
                    </div>
                </a-col>
                <div hidden>{{ PageSetting.SQLHidden }}</div>
            </a-row>
        </a-card>
    </div>
</template>


<script lang="tsx" setup>
import { MenuFoldOutlined, MenuUnfoldOutlined } from '@ant-design/icons-vue';import {PageParams} from '@/components/core/dynamic-table/src/dynamic-table'
// import { schemas } from './form-schema';
import { useForm } from '@/components/core/schema-form';
import { useTable } from '@/components/core/dynamic-table';
import { reactive, unref, ref, onMounted, watch, computed, inject, Ref } from 'vue';
import { GetDailyMove } from '@/api/pages/daylimove';
import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect';
import {
    cloneDeep,
    // isEqual, uniqWith, uniqBy
} from 'lodash-es';
import { getPovitData, PovitParams } from '@/utils/helper/povitHelper'
import { showHidden, getRowClassNmae, getSumData, mergeTableRow, getdiffdate } from '@/pages/ReportPortal/utils/common'
import { AddCommonPageParams } from '@/store/module/commonpage';
import { Loading, useLoading } from '@/components/feat/Loading';
//import {optionsdata} from '@/pages/ReportPortal/Static/TNCommon-Form-schema';

const UseParamsAdd = AddCommonPageParams();
const DaylimoveTable = ref<HTMLElement | null>(null)
//vue3的mixin
const { mounted, InfiniteScroll, mountedowner, mountedOper } = useRemoteSelect()
function getImport(site: any) {
    // let { optionsdata } = require('@/pages/ReportPortal/TN/PanelWipStayChart/MOD/form-schema');
    const { schemas } = require(`@/pages/ReportPortal/TN/daylimoveAddCustomer/${site}/form-schema`);
    const { baseColumns } = require(`@/pages/ReportPortal/TN/daylimoveAddCustomer/${site}/Table`);
    return { schemas, baseColumns };
}
const compState = reactive({
    absolute: true,
    loading: false,
    theme: 'dark',
    background: 'rgba(111,111,111,.7)',
    tip: '加载中...'
});
const [openFullLoading, closeFullLoading] = useLoading({
        tip: '數據加載中...',
      });
// 
let repeatData = ['WORK_CTR', 'WO_NBR', 'LCM_OWNER', 'SIZE1', 'PROD_NBR', 'CUSTOMER']

// let { schemas, baseColumns, optionsdata, } = getImport('LCM1');
let { schemas, baseColumns } = getImport('MOD');

let PageSetting = reactive({
    List: [] as any,
    TableSpan: 20,
    showmenu: true,
    APIColumns: [] as any,
    Columns: [] as any,
    moreData: [] as any,
    SQLHidden: ''
});
// let isLongData = ref(true)
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
/**
 * 提交表单,验证通过后进行数据获取与处理
 * @dynamicFormRef 表单的所有参数与属性
 * @ParamsData API接收的参数
 */
function confirm(values) {
    openFullLoading()
    let baseParams = { name: "Date", apiName: "TN_DailyMoveAddCustomer", FactoryType: UseParamsAdd.Fab }
    Object.keys(values).forEach((key) => {
        if (!values[key]) {
            values[key] = '';
        }
    });
    console.log("value的值是:", values);
    switch (UseParamsAdd.Fab) {
        case "BLU1":
        case "BLU2":
            switch (values["floor"]) {
                case "1":
                    values["floor"] = " and floor='1'";
                    break;
                case "2":
                    values["floor"] = " and floor='2'";
                    break;
                case "3":
                    values["floor"] = " and floor='3'";
                    break;
                default:
                    break;
            }
            break;

        default:
            break;
    }

    dynamicFormRef.validate().then(async () => {
        const { Date: { Data: { Table: Data, sql: Message } } } = await GetDailyMove({ ...baseParams, ...values });
        PageSetting.APIColumns = getdiffdate(values.vDate_s, values.vDate_e1);
        PageSetting.SQLHidden = Message
        //PageSetting.APIColumns = [];
        Data.map((item: any) => {
            if (!PageSetting.APIColumns.includes(item['PD'])) {
                PageSetting.APIColumns.push(item['PD']);
            }
        });
        let DateColumns: any = [];
        PageSetting.APIColumns.map((item: any) => {
            DateColumns.push({
                title: item,
                align: 'center',
                dataIndex: item
            });
        });
        PageSetting.Columns = [
            ...baseColumns,
            ...DateColumns,
            {
                title: '總計',
                align: 'center',
                dataIndex: 'Total',
                // customRender: ({ record }) => {
                //     return <span>{getAllSumData(record)}</span>;
                // }
            }
        ]; //vue3或者adv无法监测push,shift等方法

        let config: PovitParams = {
            Data,
            povitColumn: {
                PD: 'QTY'
            },
            changeColumn: PageSetting.APIColumns,
            repeatColumn: repeatData
        }

        let povitData = getPovitData(unref(config))//进行类行列转置的动作
        PageSetting.moreData = getTableData(povitData)//进行表格合并
        if (PageSetting.moreData.length > 100) {
            PageSetting.List = PageSetting.moreData.slice(0, 40)
            // isLongData.value = true
        }//预先加载40条数据
        else {
            PageSetting.List = PageSetting.moreData
            // isLongData.value = false
        }
        closeFullLoading();
    });
}


/**
 * 进行数据处理,将表格进行行列转置并进行表格合并
 * @Data 表格数据源
 */
function getTableData(Data: any[]) {

    /**表格合并所需参数
     * @data 数据源
     * @mergeColNames 需要合并的列
     * @firstMergeColNames 受到影响的列
     * @firstMerge 以哪列为基础进行合并
     */
    let combineCol1: any = {
        data: Data,
        mergeColNames: ['WORK_CTR', 'SIZE1', 'LCM_OWNER'],
        firstMergeColNames: ['WORK_CTR', 'SIZE1'],
        firstMerge: 'WORK_CTR'
    };
    /**进行表格合并 */
    let Mergedata = mergeTableRow(combineCol1);
    let { pushdata, AllSum } = getMergeData(Mergedata, 'SIZE1');

    pushdata.push(Object.assign({ CUSTOMER: '合計' }, getTotalData(AllSum)));
    pushdata.forEach(item =>
        item['Total'] = getAllSumData(item)//在渲染的实际数组中加入總計数据
    );

    let combineCol2 = {
        //二次合并
        data: pushdata,
        mergeColNames: ['WORK_CTR'],
        firstMergeColNames: ['WORK_CTR'],
        firstMerge: 'WORK_CTR'
    };
    return mergeTableRow(combineCol2);
}
function getAllSumData(row: any) {
    let sum = 0;
    PageSetting.APIColumns.map((item: any) => {
        row[item] = isNaN(row[item]) ? 0 : row[item];
        sum += row[item];
    });
    return sum;
}

function getMergeData(data: any[], params: string) {
    let pushdata = cloneDeep(data);
    let count = 0; //加总的数组的终点
    let count1 = 0; //加总的数组的起点
    let count2 = 0; //误差调整,要计算之前已经插入的小計行
    let AllSum: any = []; //合計
    for (let i in data) {
        count += data[i][params + '-span']['rowspan']; //要加总几条数据
        if (data[i][params + '-span']['rowspan'] > 0) {
            const Sub = getSumData(count, count1, data, PageSetting.APIColumns);
            AllSum.push(Sub);
            pushdata.splice(count + count2, 0, Object.assign({ WORK_CTR: data[i]['WORK_CTR'], CUSTOMER: '小計' }, Sub));
            count2 += 1; //记录插入过几条数据
            count1 += data[i][params + '-span']['rowspan']; //已加总的条数作为下一次加总的起点
        }
    }

    return { pushdata, AllSum };
}

function getTotalData(data: any[]) {
    //合計
    let obj = {};
    PageSetting.APIColumns.map((item: string) => {
        let sum = 0;
        data.map((n) => {
            sum += n[item];
        });
        obj[item] = sum;
    });
    return obj;
}
InfiniteScroll(DaylimoveTable, PageSetting)

onMounted(async () => {
    //await mountedowner(optionsdata, UseParamsAdd.Fab)
    //await mountedOper(optionsdata, UseParamsAdd.Fab);
    //await mounted(optionsdata, UseParamsAdd.Fab)
    //console.log(optionsdata);


})

</script>
<!-- <script lang="tsx">
export default{
    name:'daylimove'
}
</script> -->
<style lang="less" scoped>
.btn-rows {
    button {
        margin-right: 12px;
    }
}

.DayliMove :deep(.Source-center) {
    background-color: #f0f0f0;
}
:deep(.pro_auto-center){
    border: 1px solid black;
}

.DayliMove :deep(.Yellow) {
    background-color: #80ff80;
}

.DaylimoveTable {
    height: 700px;
    overflow-y: auto;
}

.DayliMove :deep(.YellowGreen) {
    background-color: #f39230;
}

.swapper {
    display: flex;
    flex-direction: column;
}
</style>
