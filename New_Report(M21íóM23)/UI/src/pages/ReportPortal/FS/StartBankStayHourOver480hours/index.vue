<script lang="ts" setup>
import { PageParams } from '@/components/core/dynamic-table/src/dynamic-table';
import { getRowClassNmae, mergeTableRow } from '@/pages/ReportPortal/utils/common';
import { useForm } from '@/components/core/schema-form';
import { useTable } from '@/components/core/dynamic-table';
import { h, reactive, ref } from 'vue';
import { GetCommon } from '@/api/pages/common';
import { ReportCommon } from '@/store/module/ReportCommon';
import { useLoading } from '@/components/feat/Loading';
import { message } from 'ant-design-vue';
import { megercell, dclick } from '@/pages/ReportPortal/utils/common';
import { cloneDeep } from 'lodash';
import dayjs from 'dayjs';

const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});

const UseParamsAdd = ReportCommon();
let FactoryType: string = UseParamsAdd.Fab;
let SubFab: string = UseParamsAdd.SubFab;

const StartBankStayHourOver480hours = ref<HTMLElement | null>(null);
function getImport(site: any) {
    const { schemas } = require(`@/pages/ReportPortal/FS/StartBankStayHourOver480hours/${site}/form-schema`);
    const { baseColumnsYcolumns, baseColumnsNcolumns } = require(`@/pages/ReportPortal/FS/StartBankStayHourOver480hours/${site}/table`);
    return { schemas, baseColumnsYcolumns, baseColumnsNcolumns };
}
let { schemas, baseColumnsYcolumns, baseColumnsNcolumns } = getImport("LCM1");

let PageSetting = reactive<BasePageParams.BasePageSetting>({
    List: [],
    APIColumns: [],
    Columns: [],
    moreData: []
});
const [DynamicTable] = useTable();
const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 180,
    schemas: schemas(FactoryType),
    actionColOptions: {
        span: 24
    },
    layout: 'vertical'
});

async function confirm(values) {
    openFullLoading();
    PageSetting.Columns = [];
    let baseParams = {
        apiName: 'StartBank480DataSet',
        FactoryType,
        G_FAC: UseParamsAdd.G_FAC_COM
    };
    Object.keys(values).forEach((key) => {
        if (!values[key]) {
            values[key] = '';
        }
    });
    try {
        if(!values["StartHour"]){
            closeFullLoading();
            return message.warning("請輸入Stay Hours")
        }
        let { api01 } = await GetCommon({ ...values, ...baseParams });
        let Data = api01['Data']['Table'];
        if (Data.length) {
            Data.forEach(item=>{
                item["TRANS_DATE"] = dayjs(new Date(item["TRANS_DATE"])).format('YYYY/MM/DD HH:mm:ss')
            })
            PageSetting.moreData = Data;
            if (values["show_abn"] == "Y") {
                PageSetting.Columns = baseColumnsYcolumns;

            } else {
                PageSetting.Columns = baseColumnsNcolumns;

            }
            megercell(PageSetting.Columns, ['SIZE1'], Data);
           /*  if (PageSetting.moreData.length > 100) {
                PageSetting.List = PageSetting.moreData.slice(0, 40);
            } //预先加载40条数据
            else {
                PageSetting.List = PageSetting.moreData;
            } */
        } else {
            closeFullLoading();
            return message.warn('無數據!');
        }
        closeFullLoading();
    } catch {
        closeFullLoading();
    }
}

</script>

<template>
    <div class="DayliMove">
        <a-card>
            <a-row>
                <a-col :span="24 - PageParams.TableSpan" class="ScrollForm" v-show="PageParams.ShowMenu">
                    <SchemaForm @submit="confirm" :showTextPosition="'center'"></SchemaForm>
                </a-col>
                <a-col :span="PageParams.TableSpan">
                    <div ref="StartBankStayHourOver480hours">
                        <DynamicTable showFavorite row-key="StartBankStayHourOver480hours" :PageParams="PageParams"
                            :flexwidth="1" :header-title="PageSetting.Title" :columns="PageSetting.Columns"
                            :key="PageSetting.Columns" :data-all="PageSetting.moreData" bordered size="small"
                            :data-source="PageSetting.moreData" :pagination="false" export-file-name="导出"
                            :row-class-name="getRowClassNmae">
                            <template #export-button>
                                <a-button type="success"> 导出 </a-button>
                            </template>
                        </DynamicTable>
                    </div>
                </a-col>
            </a-row>
        </a-card>
    </div>
</template>
