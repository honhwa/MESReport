<script lang="ts" setup>
import { PageParams } from '@/components/core/dynamic-table/src/dynamic-table'
import { getRowClassNmae ,megercell} from '@/pages/ReportPortal/utils/common';
import { useForm } from '@/components/core/schema-form';
import { useTable } from '@/components/core/dynamic-table';
import { reactive, onMounted, ref, unref } from 'vue';
import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect';
import { GetCommon } from '@/api/pages/common';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { useLoading } from '@/components/feat/Loading';
import { message } from 'ant-design-vue';
import { cloneDeep } from 'lodash';
const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});

const { InfiniteScroll, } = useRemoteSelect();
const UseParamsAdd = AddCommonPageParams();
let FactoryType: string = UseParamsAdd.Fab;
const DownGrade = ref<HTMLElement | null>(null);
function getImport(site: any) {
    const { schemas } = require(`@/pages/ReportPortal/FS/BL2InputQualityReport/BLU2/form-schema`);
    return { schemas };
}
let { schemas } = getImport(FactoryType);
let PageSetting = reactive<BasePageParams.BasePageSetting>({
    List: [],
    APIColumns: [],
    Columns: [],
    moreData: [],
    x: null,
    Title: "BLA Input Quality",

});
const [DynamicTable] = useTable();
const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 220,
    schemas: schemas(FactoryType),
    actionColOptions: {
        span: 24
    },
    layout: 'vertical'
});

async function confirm(values) {
    openFullLoading();
    let baseParams = {
        apiName: 'BL2InputQualityReport',
        FactoryType,
        G_FAC: UseParamsAdd.G_FAC_COM,
        ReportType: ''
    };
    Object.keys(values).forEach((key) => {
        if (!values[key]) {
            values[key] = '';
        }
    });
    try {
        let { api01 } = await GetCommon({ ...baseParams, ...values });
        let Data = api01.Data?.Table ? api01.Data.Table : [];
        if (Data.length == 0) {
            closeFullLoading();
            return message.warn("無數據!");
        }
        PageSetting.Columns = Object.keys(Data[0]).map(item => {
            let width;
            switch (item) {
                case "Size":
                    width = 60;
                    break;
                case "LCM_PROD":
                case "BL_PROD":
                    width = 150;
                    break;
                case "Bank qty":
                case "SUM":
                    width = 80;
                    break;
                default:
                    width = 100;
                    break;
            }
            return {
                title: item,
                align: 'center',
                dataIndex: item,
                width,
                className: item == "SUM" ? "YellowGray" : 'none',
            }
        })
        megercell(PageSetting.Columns,["Size","LCM_PROD","BL_PROD"],Data)
        PageSetting.x = null;
        if (PageSetting.Columns.length > 7) {
            PageSetting.x = 100 * PageSetting.Columns.length;
        }
        PageSetting.moreData = Data;
        if (PageSetting.moreData.length > 100) {
            PageSetting.List = PageSetting.moreData.slice(0, 40);
        } //预先加载40条数据
        else {
            PageSetting.List = PageSetting.moreData;
        }
        closeFullLoading();

    } catch (error) {
        closeFullLoading();
    }




}

//实现无限滚动或者说懒加载
InfiniteScroll(DownGrade, PageSetting);
onMounted(async () => {

});
</script>

<template>
    <div>
        <a-card>
            <a-row>
                <a-col :span="24 - PageParams.TableSpan" class="ScrollForm" v-show="PageParams.ShowMenu">
                    <SchemaForm @submit="confirm" :showTextPosition="'center'"> </SchemaForm>
                </a-col>
                <a-col :span="PageParams.TableSpan">
                    <div class="ScrollTable" ref="DownGrade">
                        <DynamicTable showFavorite :PageParams="PageParams" row-key="DownGrade"
                            :header-title="PageSetting.Title"
                            sticky
                            :columns="PageSetting.Columns"
                            :key="PageSetting.Columns"
                            bordered
                            :flexwidth="1"
                            :data-source="PageSetting.moreData"
                            :export-file-name="PageSetting.Title"
                            :data-all="PageSetting.moreData"
                            :row-class-name="getRowClassNmae"
                            :pagination="false"
                        >
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