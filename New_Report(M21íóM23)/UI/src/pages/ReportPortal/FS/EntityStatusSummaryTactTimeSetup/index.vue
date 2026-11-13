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
const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});
const userStore = useUserStore();

// let showupload = ref(userStore.perms == 'Admin');
const UseParamsAdd = AddCommonPageParams();
const OQCInspectionDetailList = ref<HTMLElement | null>(null);
function getImport(site: any) {
    const { schemas, optionsdata } = require(`@/pages/ReportPortal/FS/EntityStatusSummaryTactTimeSetup/${site}/form-schema`);
    const { baseColumns } = require(`@/pages/ReportPortal/FS/EntityStatusSummaryTactTimeSetup/${site}/Table`);
    return { schemas, optionsdata, baseColumns };
}
let { schemas, optionsdata, baseColumns } = getImport('LCM1');

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

const editableData = reactive({});
async function confirm(values) {
    openFullLoading();
    PageSetting.Columns = [];
    let baseParams = {
        apiName: 'EntityStaTactTimeSet',
        FactoryType: UseParamsAdd.Fab,
        G_FAC: UseParamsAdd.G_FAC_COM
    };
    Object.keys(values).forEach((key) => {
        if (!values[key]) {
            values[key] = '';
        }
    });
    try {
        let { api01 } = await GetCommon({ ...values, ...baseParams });
        PageSetting.Title = '(MA503) Entity Status Summary Tact Time Setup ';
        PageSetting.Columns = baseColumns;
        PageSetting.moreData = api01['Data']['Table'];
        if (PageSetting.moreData.length > 100) PageSetting.List = PageSetting.moreData.slice(0, 40);
        else PageSetting.List = PageSetting.moreData;
        PageSetting.List.forEach((item, index) => {
            item["key"] = index.toString()
        })
        closeFullLoading();
    } catch {
        closeFullLoading();
    }
}
const edit = (key: string) => {
    editableData[key] = cloneDeep(PageSetting.List.filter(item => key === item["key"])[0]);
};
const save = async (key: string) => {
    console.log('輸出修改的數據', editableData[key]);
    Object.assign(PageSetting.moreData.filter((item) => key === item['key'])[0], editableData[key]);
    // 發送到後端
    let { api01 } = await GetCommon({
        apiName: 'EntityStaTactTimeMod2',
        FactoryType: FactoryType,
        G_FAC: UseParamsAdd.G_FAC_COM,
        UpdateObj: JSON.stringify(editableData[key])
    });
    if (api01['Message'] && api01['Message']['code'] == 200) {
        // 請求數據
        // 關閉數據框
        delete editableData[key];
        return message.success(api01['Message']['result']);
    } else {
        delete editableData[key];
        return message.error(api01['Message']['result']);
    }
};
const cancel = (key: string) => {
    delete editableData[key];
};

// 打開文件上傳彈窗按鈕
function upload(values) {
    console.log('文件上傳獲取的參數', values);
    // 跳轉到detail頁面
    router.push({ name: 'EntityStatusSummaryTactTimeSetupDetail' + FactoryType });
}

// 確認
//实现无限滚动或者说懒加载
InfiniteScroll(OQCInspectionDetailList, PageSetting);
onMounted(async () => {
    await GetProDuctData();
});

async function GetProDuctData() {
    function getMonths(start: string, end: string) {
        let startyear = Number(start.split('-')[0]);
        let startMonth = Number(start.split('-')[1]);
        let endyear = Number(end.split('-')[0]);
        let endMonth = Number(end.split('-')[1]);
        let result: string[] = [];
        let year = startyear;
        let month = startMonth;
        while (year < endyear || (year === endyear && month <= endMonth)) {
            if (month >= 10) {
                result.push(`${year}${month}`)
            } else {
                result.push(`${year}0${month}`)
            }
            month++;
            if (month > 12) {
                year++;
                month = 1;
            }
        }
        return result;
    }
    let start = String(dayjs().year()-1) + `-${dayjs().add(6,'months').format("MM")}`;
    let end = String(dayjs().year() ) + `-${dayjs().add(6,'months').format("MM")}`;
    optionsdata.TimeData = getMonths(start, end).map(item => {
        return { label: item, value: item }
    })
    console.log("输出当前月份",dayjs().format("MM"));
    
    console.log("输出index的时间处理",optionsdata.TimeData);
    
    async function getschmaData(FactoryType: string, apiName: string, paramsname: string) {
        let { api01: { Data: { Table: k } } } = await GetCommon({
            G_FAC: "A",
            FactoryType: FactoryType,
            apiName: apiName
        })
        return k.map(item => {
            return { label: item[paramsname], value: item[paramsname] };
        })
    }
    optionsdata.SizeData = await getschmaData("LCM1", "EntityStaTact_Size", "SIZE1");
    optionsdata.SizeData.unshift({ label: "ALL", value: "ALL" })
    optionsdata.ProductData = await getschmaData("LCM1", "EntityStaTact_Product", "PART");
    optionsdata.ProductData.unshift({ label: "ALL", value: "ALL" })
    optionsdata.ProductData1 = cloneDeep(optionsdata.ProductData);
    optionsdata.EquipmentSizeData = await getschmaData("LCM1", "EntityStaTact_Code", "CODE");
    optionsdata.EquipmentSizeData.unshift({ label: "ALL", value: "ALL" })
    optionsdata.EquipmentGroupData = await getschmaData("LCM1", "EntityStaTact_Equip_nbr", "EQUIP_NBR");
    optionsdata.EquipmentGroupData1 = cloneDeep(optionsdata.EquipmentGroupData);

}
</script>

<template>
    <div class="DayliMove">
        <a-card>

            <a-row>
                <a-col :span="24 - PageParams.TableSpan" class="ScrollForm" v-show="PageParams.ShowMenu">
                    <SchemaForm
                        @submit="confirm"
                        :showTextPosition="'center'"
                        showUploadButton
                        @upload="upload"
                    >
                    </SchemaForm>
                </a-col>
                <a-col :span="PageParams.TableSpan">
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
                                pageSize: 40,
                                pageSizeOptions: ['40', '80', '120', '160']
                            }"
                            export-file-name="导出"
                            :row-class-name="PageSetting.Title"
                        >
                            <template #export-button>
                                <a-button type="success"> 导出 </a-button>
                            </template>
                            <template #bodyCell="{ column, text, record }">
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
                            </template>
                        </DynamicTable>
                </a-col>
            </a-row>
        </a-card>
    </div>
</template>
<style scoped lang="less">
:deep(.ant-table){
    height: 80vh;
    overflow: auto;
}
</style>
