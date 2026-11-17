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
                        <DynamicTable showFavorite row-key="id" header-title="Daily Move" :columns="PageSetting.Columns"
                            sticky :PageParams="PageParams"
                            :key="PageSetting.Columns" bordered size="small" :data-source="PageSetting.List"
                            :data-all="PageSetting.moreData" :pagination="false" export-file-name="Export"
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
import useRemoteSelect from '@/pages/ReportPortal/utils/TNuseRemoteSelect';
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
const { mounted, InfiniteScroll} = useRemoteSelect()
function getImport(site: any) {
    // let { optionsdata } = require('@/pages/ReportPortal/TN/PanelWipStayChart/MOD/form-schema');
    const { schemas } = require(`@/pages/ReportPortal/TN/daylimove/${site}/form-schema`);
    const { baseColumns } = require(`@/pages/ReportPortal/TN/daylimove/${site}/Table`);
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

let { schemas, baseColumns } = getImport('MOD');
let FactoryType: any = UseParamsAdd.Fab;
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

const [DynamicTable] = useTable();

const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas:schemas(FactoryType),
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
    let baseParams = { name: "Date", apiName: "TN_DailyMove", FactoryType: UseParamsAdd.Fab }
    Object.keys(values).forEach((key) => {
        if (!values[key]) {
            values[key] = '';
        }
    });
   
    dynamicFormRef.validate().then(async () => {
        debugger
        const { Date: { Data: { Table: Data, sql: Message } } } = await GetDailyMove({ ...baseParams, ...values });
        PageSetting.APIColumns = getdiffdate(values.vDate_s, values.vDate_e1);
        PageSetting.APIColumns.push('總計');
        let DateColumns: any = [];
        PageSetting.APIColumns.map((item: any) => {
            DateColumns.push({
                title: item,
                align: 'center',
                dataIndex: item,
                className: "pro_auto-center"
            });
        });
        PageSetting.Columns = [
            ...baseColumns,
            ...DateColumns
        ]; //vue3或者adv无法监测push,shift等方法

        PageSetting.moreData = getTableData(Data)
        //PageSetting.moreData = Data
        debugger
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
    mergeColNames: ['WORK_CTR', 'SIZE1','LCM_OWNER'],
    firstMergeColNames: ['WORK_CTR', 'SIZE1'],
    firstMerge: 'WORK_CTR'
};
/**进行表格合并 */
let Mergedata = mergeTableRow(combineCol1);

return mergeTableRow(combineCol1);
}


InfiniteScroll(DaylimoveTable, PageSetting)

onMounted(async () => {
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
:deep(.sum-center){
    border: 1px solid black;
}


.DaylimoveTable {
    height: 700px;
    overflow-y: auto;
}

.DayliMove :deep(.YellowGreen) {
    background-color: #f39230;
}
:deep(.v-center){
    border: 1px solid black;
}

.swapper {
    display: flex;
    flex-direction: column;
}
</style>
