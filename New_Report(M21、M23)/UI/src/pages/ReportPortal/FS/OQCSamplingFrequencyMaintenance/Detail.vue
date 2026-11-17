<template>
    <div class='Report'>
        <a-card>
            <a-row>
                <a-col :span="24 - PageParams.TableSpan" class="ScrollForm" v-show="PageParams.ShowMenu">
                    <div :style="{ color: 'red', 'font-size': '16px' }">請選擇要維護的項目</div>
                    <a-button type="primary" size="small " :style="{ margin: '12px' }"
                        @click="SendQuery(1)">查詢或進出資料</a-button>
                    <a-button type="primary" size="small " @click="SendQuery(2)">進入資料</a-button>
                </a-col>
                <a-col :span='PageParams.TableSpan'>
                    <div class='ScrollTable' ref='report'>
                        <DynamicTable row-key='report' :header-title='PageSetting.Title' :columns='PageSetting.Columns'
                            showFavorite :key='PageSetting.Columns' bordered size='small' :PageParams="PageParams" sticky
                            :style="{ display: 'table', width: PageSetting.Columns.length > 8 ? '100% !important' : '70% !important' }"
                            :row-class-name='getRowClassNmae' :data-source='PageSetting.List'
                            :data-all='PageSetting.moreData' :pagination='false' :scroll='{ x: 600 }' export-file-name='导出'>
                            <template #export-button>
                                <a-button type='success'> 导出 </a-button>
                            </template>
                        </DynamicTable>
                    </div>
                </a-col>
                <!-- <UploadFile :visible="visible" @closemodal="closemodal" @uploadfile="uploadfile"  ></UploadFile> -->
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
import { ReportCommonWithOut } from '@/store/module/ReportCommon';
import { message } from 'ant-design-vue';
import { getRowClassNmae } from '@/pages/ReportPortal/utils/common'
import router from '@/router';
import UploadFile from './Upload.vue'
import axios from 'axios';
import UploadExcell from '@/components/upload/UploadCommon.vue';


const ReportCommonParams = ReportCommonWithOut()
let FactoryType: any = ReportCommonParams.Fab;

function getImport(site: any) {
    const { LCM1baseColumns } = require(`@/pages/ReportPortal/FS/OQCSamplingFrequencyMaintenance/LCM1/Table`);
    return { LCM1baseColumns };
}
let { LCM1baseColumns } = getImport(FactoryType);

const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});
const { InfiniteScroll } = useRemoteSelect()
const report = ref<HTMLElement | null>(null);


let PageSetting = reactive<BasePageParams.BasePageSetting>({
    List: [] as any,
    Columns: [] as any,
    Title: '抽驗頻率 Maintain ',
    moreData: [],
    SQLHidden: '',
    APIColumns: [] as any,
});
const [DynamicTable] = useTable();
//文件上傳彈窗visible
let visible = ref(false);
async function SendQuery(type: number) {
    if (type == 1) {
        openFullLoading()
        let baseParams = {
            apiName: 'OQCSamplingFrequencyMaintenance',
            FactoryType,
            G_FAC: ReportCommonParams.G_FAC_COM,
            ReportType: 'OQCInspFrequencyImport',
            action: 'Query',
            inputpassword:ReportCommonParams[`OQCInspFrequencyImport${FactoryType}`]["FormParams"]["inputpassword"]
        };
        try {
            let { api01 } = await GetCommon(baseParams);
            let Data = api01.Data.Table ?? [];
            if (Data.length == 0) {
                closeFullLoading();
                PageSetting.List = [];
                PageSetting.moreData = [];
                return message.warn("無數據!");
            }
            PageSetting.moreData = Data;
            let tempcolumns = cloneDeep(LCM1baseColumns);
            tempcolumns[2].title = "計算別(Y或N)";
            if (FactoryType == "LCM2") {
                tempcolumns.pop();
            }
            PageSetting.Columns = tempcolumns;
            if (PageSetting.moreData.length > 100) {
                //预先加载40条数据
                PageSetting.List = PageSetting.moreData.slice(0, 40);
            }
            else {
                PageSetting.List = PageSetting.moreData;
            }
            closeFullLoading();
        } catch (error) {
            closeFullLoading();
        }

    } else {
        if (ReportCommonParams[`OQCInspFrequencyImport${FactoryType}`]["FormParams"]) {
            if (["136858", "145058"].includes(ReportCommonParams[`OQCInspFrequencyImport${FactoryType}`]["FormParams"]["inputpassword"])) {
                visible.value = true
            } else {
                message.warn("權限不足！無法進入資料！")
            }
        } else {
            message.warn("權限不足！無法進入資料！")
        }

    }
}

const closemodal = () => {
    visible.value = false
}
const uploadfile = async (fileList, formData) => {
    let baseParams = {
        apiName: 'OQCSamplingFrequencyMaintenance',
        FactoryType,
        G_FAC: ReportCommonParams.G_FAC_COM,
        ReportType: 'OQCInspFrequencyImport',
        action: "Import",
        inputpassword:ReportCommonParams[`OQCInspFrequencyImport${FactoryType}`]["FormParams"]["inputpassword"]
    };
    Object.keys(baseParams).forEach(key => {
        formData.append(key, baseParams[key])
    })
    let data;
    try {
        let { Data:{Table:Data}, Message, result } = await UploadCommon(formData);
        /* data = await axios.post(process.env.VUE_APP_BASE_API+'SQLAgent/ApiWork', formData, {
        headers: { 'content-type': 'application/x-www-form-urlencoded' },
    }) */
        if (result != "Success") {
            return message.warn(result);
        }
        // const Data = data.data.Data.Table;
        PageSetting.moreData = Data;
        if (PageSetting.moreData.length > 100) {
            //预先加载40条数据
            PageSetting.List = PageSetting.moreData.slice(0, 40);
        }
        else {
            PageSetting.List = PageSetting.moreData;
        }
        let tempcolumns = cloneDeep(LCM1baseColumns);
        tempcolumns[2].title = "計算別(Y或N)";
        if (FactoryType == "LCM2") {
            tempcolumns.pop();
        }
        PageSetting.Columns = tempcolumns;
        message.success("文件上傳成功!");
    } catch (error) {
        return message.error(error + "上傳失敗!請檢查文件是否爲空/文件是否為excell!")

    }


}

InfiniteScroll(report, PageSetting);
onMounted(async () => {
})
</script>

<style lang="less" scoped></style>