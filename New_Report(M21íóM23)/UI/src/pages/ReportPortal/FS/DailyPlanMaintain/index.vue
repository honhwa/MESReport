<template>
    <div class='Report'>
        <a-card>
            <a-row>
                <a-col :span='24 - PageParams.TableSpan' class='ScrollForm'>
                    <SchemaForm1 @submit='confirm1' v-show="!PageSetting.isshow"> </SchemaForm1>
                    <SchemaForm2 @submit='confirm2' v-show="PageSetting.isshow" :showTextPosition="'center'"
                        showUploadButton @upload="upload"> </SchemaForm2>
                </a-col>
                <a-col :span='PageParams.TableSpan'>
                    <div ref='report'>
                        <DynamicTable row-key='report' :header-title='PageSetting.Title' :columns='PageSetting.Columns'
                            showFavorite sticky :key='PageSetting.Columns' bordered :row-class-name='getRowClassNmae'
                            :data-source='PageSetting.moreData' :data-all='PageSetting.moreData' :pagination='false'
                            :flexwidth='1' :export-file-name='PageSetting.Title'>
                            <template #export-button>
                                <a-button type='success'> 导出 </a-button>
                            </template>
                        </DynamicTable>
                    </div>
                </a-col>
                <UploadExcell :visible="visible" @closemodal="closemodal" @uploadfile="uploadfile"></UploadExcell>

            </a-row>
        </a-card>
    </div>
</template>

<script lang='tsx' setup>
import { PageParams } from '@/components/core/dynamic-table/src/dynamic-table'
import { useLoading } from '@/components/feat/Loading';
import { GetCommon, UploadCommon } from '@/api/pages/common';
import { cloneDeep } from 'lodash';
import { onMounted } from '@vue/runtime-core';
import { reactive, ref } from '@vue/reactivity';
import { useForm } from '@/components/core/schema-form';
import { useTable } from '@/components/core/dynamic-table';
import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect';
import { ReportCommon } from '@/store/module/ReportCommon';
import { message } from 'ant-design-vue';
import { getRowClassNmae } from '@/pages/ReportPortal/utils/common'
import UploadExcell from '@/components/upload/UploadCommon.vue';

const UseParamsAdd = ReportCommon()
let FactoryType: string = UseParamsAdd.Fab;
const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});
const report = ref<HTMLElement | null>(null);
function getImport(site: string) {
    const { schemas1, schemas2 } = require(`@/pages/ReportPortal/FS/DailyPlanMaintain/${site}/form-schema`);
    const { baseColumns } = require(`@/pages/ReportPortal/FS/DailyPlanMaintain/${site}/Table`);
    return { schemas1, schemas2, baseColumns };
}
let { schemas1, schemas2, baseColumns } = getImport(FactoryType);

let PageSetting = reactive<BasePageParams.BasePageSetting>({
    List: [] as any,
    Columns: [] as any,
    Title: '(MA147-1) Daily Plan Maintain',
    moreData: [],
    SQLHidden: '',
    APIColumns: [] as any,
    isshow: false,
    uploadparams: {}
});
switch (FactoryType) {
    case '':
        PageSetting.Title = '';
        break;
    default:
        break;
}
const [DynamicTable] = useTable();
const [SchemaForm1] = useForm({
    labelWidth: 120,
    schemas: schemas1(),
    actionColOptions: {
        span: 24
    },
    layout: 'vertical'
});
const [SchemaForm2] = useForm({
    labelWidth: 120,
    schemas: schemas2(FactoryType),
    actionColOptions: {
        span: 24
    },
    layout: 'vertical'
});

async function confirm1(values) {
    let baseParams = {
        apiName: 'DailyPlanMaintain',
        FactoryType: FactoryType,
        G_FAC: UseParamsAdd.G_FAC_COM,
        form: 1,
    };
    try {
        let { api01: { result, message: apimessage } } = await GetCommon({ ...baseParams, ...values });
        if (result) {
            message.success(apimessage);
            PageSetting.isshow = true;
        } else {
            message.error(apimessage)
        }

    } catch (error) {
        console.log(error);

    }
}


async function gettableData(params:Object) {
    try {
        let { api01: { Data, sql } } = await GetCommon(params);
        if (Data.length == 0) {
            PageSetting.moreData = [];
            closeFullLoading();
            return message.warn('无数据!');
        }
        PageSetting.Columns = Object.keys(Data[0]).map(item => {
            return {
                title: item,
                align: 'center',
                dataIndex: item,
                // className: 'v-center'
            }
        })
        PageSetting.moreData = Data;
        closeFullLoading();
    } catch (error) {
        console.log(error)
        closeFullLoading();
    }

}
async function confirm2(values) {
    openFullLoading();
    let baseParams = {
        apiName: 'DailyPlanMaintain',
        FactoryType: FactoryType,
        G_FAC: UseParamsAdd.G_FAC_COM,
        form: 2,
        action: "Query"
    };
    Object.keys(values).forEach((key) => {
        if (!values[key]) {
            values[key] = '';
        }
    });
   await gettableData({ ...baseParams, ...values });
}
// 打開文件上傳彈窗按鈕
let visible = ref(false);
// 打开文件按钮
function upload(values) {
    // 獲取月份
    visible.value = true;
    PageSetting.uploadparams = values;
    console.log("文件上傳獲取的參數", values);

}
// 關閉文件上傳彈窗
function closemodal() {
    visible.value = false;
}
// 獲取文件上傳列表
async function uploadfile(fileList, formData) {
    console.log("輸出fileList", fileList);
    console.log("輸出formData", formData);
    let baseParams = {
        apiName: 'DailyPlanMaintain',
        FactoryType,
        G_FAC: UseParamsAdd.G_FAC_COM,
        form: 2,
        action: "Import",
    };
    let merger = { ...baseParams, ...PageSetting.uploadparams }
    Object.keys(merger).forEach(key => {
        formData.append(key, merger[key])
    })
    try {
        let { apimessage,result } = await UploadCommon(formData);
        if (result) {
            message.success(apimessage);
            await gettableData({...merger, action: "Query"});
        } else {
            message.error(apimessage)
        }
    } catch (error: any) {
        message.error(error)
    }


}


onMounted(async () => {
})
</script>

<style lang="less" scoped></style>