<script lang="tsx" setup>
import { PageParams } from '@/components/core/dynamic-table/src/dynamic-table';
import { useForm } from '@/components/core/schema-form';
import { useTable } from '@/components/core/dynamic-table';
import { reactive, onMounted, ref, h } from 'vue';
import { GetCommon, UploadCommon } from '@/api/pages/common';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { useLoading } from '@/components/feat/Loading';
import { cloneDeep } from 'lodash';
import dayjs from 'dayjs';
import UploadExcell from '@/components/upload/UploadCommon.vue';
import { ReportCommonWithOut } from '@/store/module/ReportCommon';

import { message, Popconfirm, TypographyLink } from 'ant-design-vue';
const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});
const ReportCommonParams = ReportCommonWithOut()
const UseParamsAdd = AddCommonPageParams();
let FactoryType = UseParamsAdd.Fab;
function getImport(site: any) {
    const {
        schemasdetail, optionsdata
    } = require(`@/pages/ReportPortal/FS/EntityStatusSummaryTactTimeSetup/${site}/form-schema`);
    const { detailbaseColumns } = require(`@/pages/ReportPortal/FS/EntityStatusSummaryTactTimeSetup/${site}/Table`);
    return { schemasdetail, optionsdata, detailbaseColumns };
}
let { schemasdetail, optionsdata, detailbaseColumns } = getImport("LCM1");

let PageSetting = reactive<BasePageParams.BasePageSetting>({
    List: [],
    APIColumns: [],
    Columns: [],
    moreData: []
});
const [DynamicTable] = useTable();
const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas: schemasdetail(FactoryType),
    actionColOptions: {
        span: 24
    },
    layout: 'vertical'
});

async function confirm(values) {
    openFullLoading();
    let baseParams = {
        apiName: 'EntityStaTactTimeImport',
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
        if (values["action"] == "Delete") {
            let { Message } = api01;
            if (Message.code == 200) {
                message.success(Message.result)

            } else {
                message.error(Message.result)
            }
        } else if (values["action"] == "Query") {
            PageSetting.Title = '(MA503) Entity Status Summary Tact Time Setup ';
            PageSetting.Columns = detailbaseColumns;
            PageSetting.moreData = api01['Data']['Table'];
            if (PageSetting.moreData.length == 0) {
                closeFullLoading();
                return message.warn('無數據!');
            }
        }

        closeFullLoading();
    } catch {
        closeFullLoading();
    }
}



// 打開文件上傳彈窗按鈕
let visible = ref(false);
let Months = "";
function upload(values) {
    // 獲取月份
    visible.value = true;
    Months = values["Months"];
    console.log("文件上傳獲取的參數", values);

}
// 關閉文件上傳彈窗
function closemodal() {
    visible.value = false;
}
const createElementVNode = (content: any) => {
    // 定义文本内容及样式（这里的样式延用的官方的）
    const innerText = h('span', { style: { color: '#262626', fontSize: '14px' } }, content);
    // 自定义关闭图标
    const innerIcon = h('span', {
        style: { marginLeft: '10px', color: '#000', cursor: 'pointer', 'font-size': '18px' },
        class: "removes",
        attrs: { type: 'close', title: "关闭" },
        onClick: () => {
            message.destroy()
        }
    }, "x");
    // 合到一个 VNode 节点中
    const container = h('span', { style: {} }, [innerText, innerIcon]);
    // 最终返回
    return container;
};
// 獲取文件上傳列表
async function uploadfile(fileList, formData) {
    console.log("輸出fileList", fileList);
    console.log("輸出formData", formData);
    let baseParams = {
        apiName: 'EntityStaTactTimeImport',
        FactoryType,
        G_FAC: ReportCommonParams.G_FAC_COM,
        action: "Import",
        Months
    };
    Object.keys(baseParams).forEach(key => {
        formData.append(key, baseParams[key])
    })
    try {
        let { Message, Data: { Table: Data } } = await UploadCommon(formData);
        if (Message.code == 200) {
            message.success(createElementVNode(Message.result));
            if (Data.length != 0) {
                PageSetting.moreData = Data;
                PageSetting.Columns = Object.keys(PageSetting.moreData[0]).map(item => {
                    return {
                        title: item,
                        align: 'center',
                        dataIndex: item,
                    }
                })
            }
        } else {
            message.error(createElementVNode(Message.result))
        }
    } catch (error: any) {
        message.error(error)
    }


}
// 確認
//实现无限滚动或者说懒加载
// InfiniteScroll(Tableref, PageSetting);
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
                result.push(`${year}${month}`);
            } else {
                result.push(`${year}0${month}`);
            }
            month++;
            if (month > 12) {
                year++;
                month = 1;
            }
        }
        return result;
    }
    let start = String(dayjs().year() - 1) + `-${dayjs().add(6, 'months').format('MM')}`;
    let end = String(dayjs().year()) + `-${dayjs().add(6, 'months').format('MM')}`;
    optionsdata.TimeData = getMonths(start, end).map((item) => {
        return { label: item, value: item };
    });
    /* console.log('输出当前月份', dayjs().format('MM'));
    console.log('输出index的时间处理', optionsdata.TimeData);
 */
}
</script>

<template>
    <div class="DayliMove">
        <a-card>
            <a-row>
                <a-col :span="24 - PageParams.TableSpan" class="ScrollForm" v-show="PageParams.ShowMenu">
                    <SchemaForm @submit="confirm" :showTextPosition="'center'" showUploadButton @upload="upload">
                    </SchemaForm>
                </a-col>
                <a-col :span="PageParams.TableSpan">
                    <div ref="Tableref" class="ScrollForm">
                        <DynamicTable sticky :PageParams="PageParams" showFavorite :header-title="PageSetting.Title"
                            :data-all="PageSetting.moreData" :columns="PageSetting.Columns" :key="PageSetting.Columns"
                            bordered :data-source="PageSetting.moreData" :pagination="{
                                pageSize: 40,
                                pageSizeOptions: ['40', '80', '120', '160']
                            }" export-file-name="导出" :row-class-name="PageSetting.Title">
                            <template #export-button>
                                <a-button type="success"> 导出 </a-button>
                            </template>
                        </DynamicTable>
                    </div>
                </a-col>
                <UploadExcell :visible="visible" @closemodal="closemodal" @uploadfile="uploadfile" :footer="null">
                </UploadExcell>
            </a-row>
        </a-card>
    </div>
</template>
