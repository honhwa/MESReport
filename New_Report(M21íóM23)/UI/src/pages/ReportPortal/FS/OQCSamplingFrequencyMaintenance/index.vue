<script lang="ts" setup>
import { PageParams } from '@/components/core/dynamic-table/src/dynamic-table';
import { getRowClassNmae } from '@/pages/ReportPortal/utils/common';
import { useForm } from '@/components/core/schema-form';
import { useTable } from '@/components/core/dynamic-table';
import { reactive, onMounted, ref, unref } from 'vue';
import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect';
import { GetCommon } from '@/api/pages/common';
import { ReportCommonWithOut } from '@/store/module/ReportCommon';
import { useLoading } from '@/components/feat/Loading';
import { message } from 'ant-design-vue';
import { cloneDeep } from 'lodash';
import { useRouter } from 'vue-router';
const router = useRouter();

const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});

const { InfiniteScroll } = useRemoteSelect();
const ReportCommonParams = ReportCommonWithOut();

let FactoryType: string = ReportCommonParams.Fab;
const DownGrade = ref<HTMLElement | null>(null);
function getImport(site: any) {
    const { schemas } = require(`@/pages/ReportPortal/FS/OQCSamplingFrequencyMaintenance/LCM1/form-schema`);
    const {
        LCM1baseColumns,
        LCM2baseColumns,
        operaction,
        customdialog
    } = require(`@/pages/ReportPortal/FS/OQCSamplingFrequencyMaintenance/LCM1/Table`);
    return { schemas, LCM1baseColumns, LCM2baseColumns, operaction, customdialog };
}
let { schemas, LCM1baseColumns, LCM2baseColumns, operaction, customdialog } = getImport(FactoryType);

let PageSetting = reactive<BasePageParams.BasePageSetting>({
    List: [],
    APIColumns: [],
    Columns: [],
    moreData: [],
    x: null,
    Title: ''
});
let Form = ref({
    visible: false,
    editinfo: {},
    MType: 0
});
switch (FactoryType) {
    case 'LCM1':
        PageSetting.Title = '(MA408) OQC抽檢頻率維護';
        // PageSetting.Columns = LCM1baseColumns;
        break;
    case 'LCM2':
        PageSetting.Title = '(MB408) OQC抽檢頻率維護 ';
    // PageSetting.Columns = LCM2baseColumns;
    default:
        break;
}
const [DynamicTable,dynamicTableInstance] = useTable();
const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 220,
    schemas: schemas(FactoryType),
    actionColOptions: {
        span: 24
    },
    layout: 'vertical'
});
let fleshparams = reactive({});
let tablekey = ref(0);
// 修改按鈕
function update(text, record, index, column) {
    Form.value.editinfo = {
        PROD_NBR: record['PROD_NBR'],
        INSP_FREQUENCY: record['INSP_FREQUENCY'],
        PROD_FLAG: record['PROD_FLAG'],
        // 保存舊的old_insp_qty
        old_insp_qty: record['INSP_FREQUENCY'],
        CNT_FLAG: record['CNT_FLAG'],
        UP_DATE: record['UP_DATE']
    };
    Form.value.MType = 2;
    Form.value.visible = true;
}
// 刪除確認按鈕
async function cancel(text, record, index, column) {
    let deleteparams = cloneDeep(fleshparams);
    deleteparams['ReportType'] = 'OQCInspFrequencyMod';
    deleteparams['INSP_FREQUENCY'] = record['INSP_FREQUENCY'];
    deleteparams['act_type'] = 'D';
    deleteparams['PROD_NBR'] = record['PROD_NBR'];
    deleteparams['PROD_FLAG'] = record['PROD_FLAG'];
    deleteparams['UP_DATE'] = record['UP_DATE'];
    let { api01 } = await GetCommon(deleteparams);
    if (api01['result'] == 'Success') {
        await getTableData(fleshparams);
        dynamicTableInstance.reload();
        message.success('刪除成功!');
    } else {
        await getTableData(fleshparams);
        dynamicTableInstance.reload();
        message.error('刪除失敗!');
    }
}
async function getTableData(fleshparams) {
    try {
        let { api01 } = await GetCommon(fleshparams);
        tablekey.value ++;
        if (api01['Data']['Table'].length != 0) {
            PageSetting.moreData = api01['Data']['Table'];
            dynamicTableInstance.reload();
            let tempcolumns;
            switch (FactoryType) {
                case "LCM1":
                    PageSetting.Columns = LCM1baseColumns;
                    tempcolumns = LCM1baseColumns;
                    break;
                case "LCM2":
                    PageSetting.Columns = LCM2baseColumns;
                    tempcolumns = LCM2baseColumns;
                    break;
                default:
                    break;
            }
            if (api01['apimessage'] == '密碼正確!') {
                PageSetting.Columns = [...tempcolumns, operaction(cancel, update)];
            }
            closeFullLoading();
        } else {
            PageSetting.moreData = [];
            closeFullLoading();
            return message.warn('無數據!');
        }

    } catch (error) {
        console.log("輸出error", error);
        message.error((error as Error).message);
        closeFullLoading();
    }
}
async function confirm(values) {
    openFullLoading();
    let baseParams = {
        apiName: 'OQCSamplingFrequencyMaintenance',
        FactoryType,
        G_FAC: ReportCommonParams.G_FAC_COM,
        ReportType: ''
    };
    Object.keys(values).forEach((key) => {
        if (!values[key]) {
            values[key] = '';
        }
    });
    fleshparams = { ...values, ...baseParams };
    await getTableData(fleshparams);
}
// 關閉對話框
function closeDialog(data) {
    Form.value.visible = data;
}
// 新增按鈕
function Addhandle() {
    Form.value.editinfo = {
        PROD_NBR: '',
        INSP_FREQUENCY: '',
        PROD_FLAG: ''
    };
    Form.value.MType = 1;
    Form.value.visible = true;
}
// 新增對話框確認按鈕
async function addnew(MType) {
    let updateparams;
    // 新增是1，修改是2
    switch (MType) {
        case 1:
            updateparams = cloneDeep(fleshparams);
            updateparams.ReportType = 'OQCInspFrequencyMod';
            updateparams.act_type = 'A';
            let { api01: api } = await GetCommon({ ...updateparams, ...Form.value.editinfo });
            if (api['result'] == 'Success') {
                await getTableData(fleshparams);
                Form.value.visible = false;
                message.success('添加成功!');
            } else {
                Form.value.visible = false;
                message.error('添加失敗,已有重複資料!');
            }
            break;
        case 2:
            updateparams = cloneDeep(fleshparams);
            updateparams.ReportType = 'OQCInspFrequencyMod';
            updateparams.act_type = 'U';
            let { api01 } = await GetCommon({ ...updateparams, ...Form.value.editinfo });
            if (api01['result'] == 'Success') {
                await getTableData(fleshparams);
                Form.value.visible = false;
                message.success('修改成功!');
            } else {
                Form.value.visible = false;
                message.error('修改失敗!');
            }
            break;

        default:
            break;
    }
}
// 輸入框更新
function updata(type, value) {
    Form.value.editinfo[type] = value;
}
// 文件上傳
function upload(values) {
    // 存儲vux
    ReportCommonParams.AddFormParams(`OQCInspFrequencyImport${FactoryType}`, values);
    // 頁面跳轉
    router.push({ name: `OQCInspFrequencyImport${FactoryType}` });
}
//实现无限滚动或者说懒加载
// InfiniteScroll(DownGrade, PageSetting);
onMounted(async () => { });
</script>

<template>
    <div>
        <a-row>
            <a-col :span="24 - PageParams.TableSpan" class="ScrollForm" v-show="PageParams.ShowMenu">
                <SchemaForm @submit="confirm" :showTextPosition="'center'" showUploadButton @upload="upload">
                </SchemaForm>
            </a-col>
            <a-col :span="PageParams.TableSpan">
                <DynamicTable showFavorite :PageParams="PageParams" row-key="id" :header-title="PageSetting.Title"
                    sticky :columns="PageSetting.Columns" :key="()=>{return Math.random()*10}" bordered
                    :data-source="PageSetting.moreData" export-file-name="导出" :data-all="PageSetting.moreData"
                    :row-class-name="getRowClassNmae" :pagination="false">
                    <template #headerCell="{ column, record, index }">
                        <template v-if="column.dataIndex === 'operation'">
                            <a-button type="primary" size="small" @click="Addhandle">新增</a-button>
                        </template>
                    </template>

                    <template #export-button>
                        <a-button type="success"> 导出 </a-button>
                    </template>
                </DynamicTable>
                <customdialog @closeDialog="closeDialog" @update:addnew="addnew" @update:updata="updata"
                    v-model:UpdateInfo="Form.editinfo" v-model:MType="Form.MType" v-model:visible="Form.visible"
                    v-model:FactoryType="FactoryType">
                </customdialog>
            </a-col>
        </a-row>
    </div>
</template>
<style scoped lang="less">
:deep(.ant-table) {
    height: calc(80vh - 3.8rem) !important;
}

:deep(.ant-table-container) {
    height: calc(80vh - 3.8rem) !important;

}

:deep(.ant-table-wrapper) {
    height: calc(80vh - 3.8rem) !important;
    // overflow: auto;

}

:deep(.ant-table-body) {
    height: calc(80vh - 3.8rem) !important;
    overflow: auto;
}

:deep(.ant-pagination) {
    margin-top: 26px !important;
}
</style>