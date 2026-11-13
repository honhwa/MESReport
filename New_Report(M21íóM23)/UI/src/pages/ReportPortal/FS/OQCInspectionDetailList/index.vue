<script lang="tsx" setup>
import {PageParams} from '@/components/core/dynamic-table/src/dynamic-table'
import { getRowClassNmae } from '@/pages/ReportPortal/utils/common';
// import { schemas } from './form-schema';
import { useForm } from '@/components/core/schema-form';
import { useTable } from '@/components/core/dynamic-table';
import { reactive, onMounted, ref } from 'vue';
import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect';
import { GetCommon } from '@/api/pages/common';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { useLoading } from '@/components/feat/Loading';
import { formatToDateTime } from '@/utils/dateUtil';

const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});

// import { useInfiniteScroll, useVirtualList } from '@vueuse/core'
const { InfiniteScroll } = useRemoteSelect();
const UseParamsAdd = AddCommonPageParams();
const OQCInspectionDetailList = ref<HTMLElement | null>(null);
function getImport(site: any) {
    const { schemas } = require(`@/pages/ReportPortal/FS/OQCInspectionDetailList/${site}/form-schema`);
    const {
        OQCInspectionDetailListColumns
    } = require(`@/pages/ReportPortal/FS/OQCInspectionDetailList/${site}/Table.ts`);
    return { schemas,  OQCInspectionDetailListColumns };
}
let { schemas,  OQCInspectionDetailListColumns } = getImport('LCM1');

let PageSetting = reactive<BasePageParams.BasePageSetting>({
    List: [],
    APIColumns: [],
    Columns: [],
    moreData: []
});
const [DynamicTable] = useTable();
const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas:schemas(UseParamsAdd.Fab),
    actionColOptions: {
        span: 24
    },
    layout: 'vertical'
});

async function confirm(values) {
    openFullLoading();
    PageSetting.Columns = [];
    // let dynamicColumns: TableColumn[] = []
    let baseParams = {
        apiName: 'OQCInspListDataSet',
        FactoryType: UseParamsAdd.Fab,
        G_FAC: UseParamsAdd.G_FAC_COM
    };

    Object.keys(values).forEach((key) => {
        if (!values[key]) {
            values[key] = '';
        }
    });

    values['FromDate'] = values['CompleteFromDate'].split(' ')[0];
    values['FromTime'] = values['CompleteFromDate'].split(' ')[1];
    values['ToDate'] = values['CompleteToDate'].split(' ')[0];
    values['ToTime'] = values['CompleteToDate'].split(' ')[1];
    try {
        let { api01 } = await GetCommon({ ...values, ...baseParams });
        api01['Data']['Table'].forEach(item=>item['TRANS_DATE']=item['TRANS_DATE'].replace('T',' '));
        console.log(api01['Data']['Table']);
        
        PageSetting.Title = 'OQC Inspection Detail List ';
        PageSetting.Columns = [
            {
                title: 'trans_date',
                align: 'center',
                dataIndex: 'TRANS_DATE',
                bodyCell: ({ record }) => <>{formatToDateTime(record['TRANS_DATE'],'YYYY-MM-DD A HH:mm:ss')}</>
            },
            ...OQCInspectionDetailListColumns
        ];
        PageSetting.moreData = api01['Data']['Table'];
        if (PageSetting.moreData.length > 100) PageSetting.List = PageSetting.moreData.slice(0, 40);
        else PageSetting.List = PageSetting.moreData;
        UseParamsAdd.AddFormParams({ ...baseParams, ...values });
        UseParamsAdd.AddAPIResultMessage(api01['Message']);
        PageSetting.SQLHidden = api01['Message']['sqlstr']
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
                    <SchemaForm @submit="confirm"> </SchemaForm>
                </a-col>
                <a-col :span="PageParams.TableSpan">
                    <div ref="OQCInspectionDetailList">
                        <DynamicTable
                            showFavorite
                            sticky
                            :PageParams="PageParams"
                            row-key="OQCInspectionDetailList"
                            :header-title="PageSetting.Title"
                            :columns="PageSetting.Columns"
                            :key="PageSetting.Columns"
                            bordered
                            size="small"
                            :data-source="PageSetting.List"
                            :pagination="false"
                            export-file-name="导出"
                            :row-class-name="getRowClassNmae"
                            :data-all="PageSetting.moreData"
                            :flexwidth="1"
                        >
                            <template #toolbar>
                                <!-- <a-button type="primary" @click="isShowExplain = !isShowExplain">{{
                                    isShowExplain ? '显示表格' : '显示说明文件'
                                }}</a-button> -->
                            </template>
                            <template #export-button>
                                <a-button type="success"> 导出 </a-button>
                            </template>
                        </DynamicTable>
                    </div>
                </a-col>
            </a-row>
            <div hidden>{{PageSetting.SQLHidden}}</div>
        </a-card>
    </div>
</template>
