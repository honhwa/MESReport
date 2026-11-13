<template>
    <div>
        <a-card>

            <a-row>
                <a-col :span="24 - PageParams.TableSpan" class="NGBScrollForm" v-show="PageParams.ShowMenu">
                    <SchemaForm
                        @submit="confirm"
                        :showTextPosition="'center'"
                        showUploadButton
                        @upload="upload"
                    >
                    </SchemaForm>
                </a-col>
                <a-col :span="PageParams.TableSpan">
                    <div class="ScrollTable">
                        <DynamicTable
                            sticky
                            :PageParams="PageParams"
                            showFavorite
                            :header-title="PageSetting.Title"
                            :data-all="PageSetting.moreData"
                            :columns="PageSetting.Columns"
                            :key="PageSetting.Columns"
                            bordered
                            :data-source="PageSetting.moreData"
                            :pagination="{
                                pageSize: 29,
                                pageSizeOptions: ['29', '150', '300', '600']
                            }"
                            export-file-name="导出"
                            :row-class-name="PageSetting.Title"
                        >
                            <template #export-button>
                                <a-button type="success"> 导出 </a-button>
                            </template>
                            <!-- <template #bodyCell="{ column, text, record }">
                                <template
                                    v-if="
                                        column &&
                                        [
                                            'TACT_TIME',
                                            'EQ_MODIFY',
                                            'OEE',
                                            'UPH',
                                            'UP',
                                            'RUN',
                                            'PSETUP',
                                            'ELOT'
                                        ].includes(column.dataIndex)
                                    "
                                >
                                    <div>
                                        <a-input
                                            v-if="editableData[record.key]"
                                            v-model:value="editableData[record.key][column.dataIndex]"
                                            style="margin: -5px 0"
                                        />
                                        <template v-else>
                                            {{ text }}
                                        </template>
                                    </div>
                                </template>
                                <template v-else-if="column.dataIndex === 'operation'">
                                    <div class="editable-row-operations">
                                        <span v-if="editableData[record.key]">
                                            <TypographyLink @click="save(record.key)" :style="{ margin: '4px' }"
                                                >保存
                                            </TypographyLink>
                                            <Popconfirm title="Sure to cancel?" @confirm="cancel(record.key)">
                                                <a>取消</a>
                                            </Popconfirm>
                                        </span>
                                        <span v-else>
                                            <a @click="edit(record.key)">編輯</a>
                                        </span>
                                    </div>
                                </template>
                            </template> -->
                        </DynamicTable>
                    </div>
                </a-col>
            </a-row>
        </a-card>
    </div>
</template>

<script lang="tsx" setup>
import {PageParams} from '@/components/core/dynamic-table/src/dynamic-table'
import {  getRowClassNmae } from '@/pages/ReportPortal/utils/common';
import { useForm } from '@/components/core/schema-form';
import { useTable } from '@/components/core/dynamic-table';
import { reactive, onMounted, ref } from 'vue';
import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect';
import { GetCommon } from '@/api/pages/common';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { useLoading } from '@/components/feat/Loading';
import { cloneDeep } from 'lodash';
import dayjs from 'dayjs';
import { message, Popconfirm, TypographyLink } from 'ant-design-vue';
import router from '@/router';
import { useUserStore } from '@/store/module/user';
import { ReportCommon } from '@/store/module/ReportCommon';
const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});
const userStore = useUserStore();

let useCommonParams = AddCommonPageParams();
let useReportCommon = ReportCommon();
let bigfab=useCommonParams.Fab.slice(0,useCommonParams.Fab.length-1)
let ReportItem = reactive({
    // 首頁後端返回的sql以及sql參數
        APIMessage:'',
        // 左側請求參數
        FormParams: {},
        // 二級頁面所需參數
        FirstTableParams: {},
        // 其餘頁面所需參數
        SecondTableParams:reactive({}),
        ThirdTableParams:{},
        ThirdTableClickParams:{},
        //廠別
        Fab:'',
        G_FAC_COM:'',
});
let ReportNameParams = `EntityStatusSummaryTactTimeSetup-${useCommonParams.Fab}`;
useReportCommon.AddObject(ReportNameParams,ReportItem);
let baseParams= {
        apiName: 'EntityStaTactTimeQuery',
        FactoryType: useCommonParams.Fab,
        G_FAC: useCommonParams.G_FAC_COM
    };

function getImport(site: any) {
    const { schemas,} = require(`@/pages/ReportPortal/NGB/EntityStatusSummaryTactTimeSetup/${bigfab}/form-schema`);
    const { detailbaseColumns } = require(`@/pages/ReportPortal/NGB/EntityStatusSummaryTactTimeSetup/${bigfab}/Table`);
    return { schemas,  detailbaseColumns};
}
let { schemas,  detailbaseColumns } = getImport(useCommonParams.Fab);

let PageSetting = reactive<BasePageParams.BasePageSetting>({
    List: [],
    APIColumns: [],
    Columns: [],
    moreData: []
});
const [DynamicTable] = useTable();
const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas:schemas(useCommonParams.Fab,reactive({})),
    actionColOptions: {
        span: 24
    },
    layout: 'vertical'
});

const editableData = reactive({});

async function confirm(values) {
    openFullLoading();
    PageSetting.Columns = [];    
    Object.keys(values).forEach((key) => {
        if (!values[key]) {
            values[key] = '';
        }
    });
    try {
        let { api01 } = await GetCommon({ ...values, ...baseParams });
        PageSetting.Title = `${useCommonParams.Fab} Entity Status Summary Tact Time Setup`;
        PageSetting.Columns = detailbaseColumns;
        PageSetting.moreData = api01['Data']['Table'];
        PageSetting.List = PageSetting.moreData;
        PageSetting.List.forEach((item, index) => {
            item["key"] = index.toString()
        })
        closeFullLoading();
    } catch {
        closeFullLoading();
    }
}
// const edit = (key: string) => {
//     editableData[key] = cloneDeep(PageSetting.List.filter(item => key === item["key"])[0]);
// };
// const save = async (key: string) => {
//     console.log('輸出修改的數據', editableData[key]);
//     Object.assign(PageSetting.moreData.filter((item) => key === item['key'])[0], editableData[key]);
//     // 發送到後端
//     let { api01 } = await GetCommon({
//         apiName: 'EntityStaTactTimeMod2',
//         FactoryType: useCommonParams.Fab,
//         G_FAC:useCommonParams.G_FAC_COM,
//         UpdateObj: JSON.stringify(editableData[key])
//     });
//     if (api01['Message'] && api01['Message']['code'] == 200) {
//         // 請求數據
//         // 關閉數據框
//         delete editableData[key];
//         return message.success(api01['Message']['result']);
//     } else {
//         delete editableData[key];
//         return message.error(api01['Message']['result']);
//     }
// };
// const cancel = (key: string) => {
//     delete editableData[key];
// };


// 打開文件上傳彈窗按鈕
function upload(values) {
    console.log('文件上傳獲取的參數2', values); 
    ReportItem.FormParams={ ...baseParams,...values }
    // 跳轉到detail頁面
    router.push({ name: `EntityStatusSummaryTactTimeSetupDetail-${useCommonParams.Fab}`});
}

</script>


<style scoped lang="less">
:deep(.ant-table){
    height: 80vh;
    overflow: auto;
}
</style>
