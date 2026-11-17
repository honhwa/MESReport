<template>
    <div class="ProductionProgressMaintain">
        <Loading
            :loading="compState.loading"
            :absolute="compState.absolute"
            :theme="compState.theme"
            :background="compState.background"
            :tip="compState.tip"
        />
        <a-card>
            <a-row>
                <a-col :span="24 - PageSetting.TableSpan" class="ScrollForm" v-show="PageParams.ShowMenu" Source-show="PageSetting.ShowMenu">
                    <div class="swapper">
                        <SchemaForm @submit="confirm"> </SchemaForm>
                    </div>
                    <div class="swapper">
                        <button type="button" class="ant-btn ant-btn-primary mr-2" @click="state.visible = true">
                            匯入資料
                        </button>
                        <a href="/openfile/ProductionProgressExample.xlsx" download="Example.xlsx">範例下載</a>
                    </div>
                </a-col>
                <a-col :span="PageSetting.TableSpan">
                    <div v-show="PageSetting.isImport">
                        <button type="button" class="ant-btn ant-btn-primary mr-2" @click="importData">確定</button>
                        <button type="button" class="ant-btn ant-btn-primary mr-2" @click="cancelData">取消</button>
                    </div>
                    <div class="ProductionProgressMaintainTable" ref="ProductionProgressMaintainTable">
                        <DynamicTable
                            row-key="id"
                            header-title="Production Progress Maintain"
                            :columns="PageSetting.Columns"
                            :key="PageSetting.Columns"
                            bordered
                            size="small"
                            :data-source="PageSetting.List"
                            :data-all="PageSetting.moreData"
                            :pagination="false"
                            export-file-name="Export"
                            :row-class-name="getRowClassNmae"
                            :PageParams="PageParams"
                        >
                            <template #export-button>
                                <a-button type="success"> Export </a-button>
                            </template>
                        </DynamicTable>
                    </div>
                </a-col>
                <div hidden>{{ PageSetting.SQLHidden }}</div>
            </a-row>
        </a-card>
        <DraggableModal :visible="state.visible" :title="'匯入資料'">
            <upexcel :beforeUpload="beforeUpload" :onSuccess="onSuccess"></upexcel>
            <br />
            <label style="color: red">{{ PageSetting.ErrMsg }}</label>
            <template #footer>
                <a-button @click="colse" danger>取消</a-button>
            </template>
        </DraggableModal>
    </div>
</template>

<script lang="tsx" setup>
import { PageParams } from '@/components/core/dynamic-table/src/dynamic-table';
import { MenuFoldOutlined, MenuUnfoldOutlined } from '@ant-design/icons-vue';
import { useForm } from '@/components/core/schema-form';
import { useTable } from '@/components/core/dynamic-table';
import { reactive, ref, onMounted } from 'vue';
import { GetCommon } from '@/api/pages/common';
import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect';
import { showHidden, getRowClassNmae, mergeTableRow } from '@/pages/ReportPortal/utils/common';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { Loading, useLoading } from '@/components/feat/Loading';
import { DraggableModal } from '@/components/core/draggable-modal';
import upexcel from '@/pages/updownfile/upexcel/index.vue';
import { formatToDateTime } from '@/utils/dateUtil';
import dayjs from 'dayjs';

const useCommonParams = AddCommonPageParams();
const ProductionProgressMaintainTable = ref<HTMLElement | null>(null);
//vue3的mixin
const { mounted, InfiniteScroll, mountedowner, mountedOper } = useRemoteSelect();
function getImport(site: any) {
    const { schemas } = require(`@/pages/ReportPortal/TN/ProductionProgressMaintain/${site}/form-schema`);
    const { baseColumns } = require(`@/pages/ReportPortal/TN/ProductionProgressMaintain/${site}/Table`);
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
    tip: '數據加載中...'
});

let { schemas, baseColumns } = getImport('MOD');

let PageSetting = reactive({
    List: [] as any,
    TableSpan: 20,
    ShowMenu: true,
    APIColumns: [] as any,
    Columns: [] as any,
    moreData: [] as any,
    SQLHidden: '',
    isImport: false,
    ErrMsg: ''
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
/**
 * 提交表单,验证通过后进行数据获取与处理
 * @dynamicFormRef 表单的所有参数与属性
 * @ParamsData API接收的参数
 */
function confirm(values) {
    openFullLoading();
    let baseParams = { name: 'Date', apiName: 'TN_ProductionProgressMaintain', FactoryType: useCommonParams.Fab };
    Object.keys(values).forEach((key) => {
        if (!values[key]) {
            values[key] = '';
        }
    });

    dynamicFormRef.validate().then(async () => {
        const {
            Date: {
                Data: { Table: Data },
                sql: Message,
                Hold_N: Hold_N,
                Hold_Y: Hold_Y
            }
        } = await GetCommon({ ...baseParams, ...values });

        PageSetting.Columns = [...baseColumns];
        PageSetting.moreData = Data;
        if (PageSetting.moreData.length > 100) {
            PageSetting.List = PageSetting.moreData.slice(0, 40);
        } //预先加载40条数据
        else {
            PageSetting.List = PageSetting.moreData;
        }

        closeFullLoading();
    });
}

InfiniteScroll(ProductionProgressMaintainTable, PageSetting);

onMounted(async () => {});

interface RT_USR_DAILYPLAN {
    VERSION: string;
    ACCT_DATE: string;
    OPER: string;
    PLAN_TYPE: string;
    PROJECT: string;
    CUSTOMER: string;
    PROD_MODULE: string;
    PROD_NBR: string;
    BL_TYPE: string;
    WO_TYPE: string;
    MPC_COMMENT: string;
    OWNER: string;
    DAILY_PLAN: Number;
}

const state = reactive({
    visible: false
});

function colse() {
    state.visible = false;
}

function beforeUpload(rawFile) {
    console.log(rawFile);
    //onOk();
    return true;
}

function onSuccess(excelData) {
    let chkColumn = [
        'OPER',
        'PLAN_TYPE',
        'PROJECT',
        'CUSTOMER',
        'PROD_MODULE',
        'PROD_NBR',
        'BL_TYPE',
        'WO_TYPE',
        'MPC_COMMENT',
        'OWNER',
        'VERSION'
    ];
    let isCheck = true;
    chkColumn.forEach((item, index) => {
        if (item != excelData.header[index]) {
            isCheck = false;
            PageSetting.ErrMsg = '匯入欄位錯誤，請確認匯入的工作表是否為第一個工作表';
            return;
        }
    });
    if (isCheck) {
        state.visible = false;
        openFullLoading();
        let insertData: RT_USR_DAILYPLAN[] = [];
        let acctDateList = excelData.header.filter((x) => new Date(x).toString() != 'Invalid Date');
        //VERSION = K2 = 第一筆
        let VERSION = excelData.results[0]['VERSION'];
        acctDateList.forEach((acctDate) => {
            excelData.results.forEach((item) => {
                insertData.push({
                    VERSION: VERSION.toString(),
                    ACCT_DATE: dayjs(new Date(acctDate)).format('YYYY-MM-DD'),
                    OPER: (item['OPER'] ?? '').toString(),
                    PLAN_TYPE: (item['PLAN_TYPE'] ?? '').toString(),
                    PROJECT: (item['PROJECT'] ?? '').toString(),
                    CUSTOMER: (item['CUSTOMER'] ?? '').toString(),
                    PROD_MODULE: (item['PROD_MODULE'] ?? '').toString(),
                    PROD_NBR: (item['PROD_NBR'] ?? '').toString(),
                    BL_TYPE: (item['BL_TYPE'] ?? '').toString(),
                    WO_TYPE: (item['WO_TYPE'] ?? '').toString(),
                    MPC_COMMENT: (item['MPC_COMMENT'] ?? '').toString(),
                    OWNER: (item['OWNER'] ?? '').toString(),
                    DAILY_PLAN: isNaN(Number(item[acctDate])) ? 0 : Number(item[acctDate])
                });
            });
        });
        console.log('import data length：' + insertData.length);
        PageSetting.Columns = [...baseColumns];
        PageSetting.moreData = insertData;
        if (PageSetting.moreData.length > 100) {
            PageSetting.List = PageSetting.moreData.slice(0, 40);
        } //预先加载40条数据
        else {
            PageSetting.List = PageSetting.moreData;
        }
        PageSetting.isImport = true;
    }
    closeFullLoading();
}
function importData() {
    openFullLoading();
    let baseParams = {
        name: 'Date',
        apiName: 'TN_ProductionProgressMaintainImport',
        FactoryType: useCommonParams.Fab,
        jsonData: JSON.stringify(PageSetting.moreData)
    };

    dynamicFormRef.validate().then(async () => {
        const {
            Date: { addCount: addCount, addSql: addSql }
        } = await GetCommon({ ...baseParams });
        if (addCount > 0) {
            alert('匯入完成，共 ' + addCount + ' 筆');
            PageSetting.isImport = false;
        }
    });
    closeFullLoading();
}
function cancelData() {
    PageSetting.List = [];
    PageSetting.moreData = [];
    PageSetting.Columns = [];
    PageSetting.isImport = false;
}
</script>
<style lang="less" scoped>
.btn-rows {
    button {
        margin-right: 12px;
    }
}

.ProductionProgressMaintain :deep(.Source-center) {
    background-color: #f0f0f0;
}

:deep(.pro_auto-center) {
    border: 1px solid black;
}

.ProductionProgressMaintain :deep(.Yellow) {
    background-color: #80ff80;
}

:deep(.sum-center) {
    border: 1px solid black;
}

.ProductionProgressMaintainTable {
    height: 700px;
    overflow-y: auto;
}

.ProductionProgressMaintain :deep(.YellowGreen) {
    background-color: #f39230;
}

.ProductionProgressMaintain :deep(.Blue) {
    background-color: #80ffff;
}

:deep(.v-center) {
    border: 1px solid black;
}

.swapper {
    display: flex;
    flex-direction: column;
}

:deep(.ant-table-small .ant-table-thead > tr > th) {
    background-color: #80ffff;
}
</style>
