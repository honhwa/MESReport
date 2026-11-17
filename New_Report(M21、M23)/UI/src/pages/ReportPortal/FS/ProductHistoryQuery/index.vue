<template>
    <div class='Report'>
        <a-card>
            <a-row>
                <a-col :span='24 - PageParams.TableSpan' class='ScrollForm'>
                    <SchemaForm @submit='confirm'> </SchemaForm>
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
            </a-row>
        </a-card>
    </div>
</template>

<script lang='tsx' setup>
import { PageParams } from '@/components/core/dynamic-table/src/dynamic-table'
import { useLoading } from '@/components/feat/Loading';
import { GetCommon } from '@/api/pages/common';
import { cloneDeep } from 'lodash';
import { onMounted } from '@vue/runtime-core';
import { reactive, ref } from '@vue/reactivity';
import { useForm } from '@/components/core/schema-form';
import { useTable } from '@/components/core/dynamic-table';
import { ReportCommon } from '@/store/module/ReportCommon';
import { message } from 'ant-design-vue';
import { getRowClassNmae ,antcustomCell,antmegercell} from '@/pages/ReportPortal/utils/common'
const UseParamsAdd = ReportCommon()
let FactoryType: string = UseParamsAdd.Fab;
const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});
const report = ref<HTMLElement | null>(null);
function getImport(site: string) {
    const { schemas } = require(`@/pages/ReportPortal/FS/ProductHistoryQuery/${site}/form-schema`);
    const { baseColumns } = require(`@/pages/ReportPortal/FS/ProductHistoryQuery/${site}/Table`);
    return { schemas, baseColumns };
}
let { schemas } = getImport(FactoryType);

let PageSetting = reactive<BasePageParams.BasePageSetting>({
    List: [] as any,
    Columns: [] as any,
    Title: 'LCMA Panel history Query',
    moreData: [],
    SQLHidden: '',
    APIColumns: [] as any,
});
/* switch (FactoryType) {
    case '':
        PageSetting.Title = '';
        break;
    default:
        break;
} */
const [DynamicTable] = useTable();
const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas: schemas(FactoryType),
    actionColOptions: {
        span: 24
    },
    layout: 'vertical'
});


async function confirm(values) {
    openFullLoading();
    let baseParams = {
        apiName: 'ProductHistoryQuery',
        FactoryType: FactoryType,
        G_FAC: UseParamsAdd.G_FAC_COM,
        G_CLASS: UseParamsAdd.G_CLASS,
    };
    Object.keys(values).forEach((key) => {
        if (key == "pnl_ids") {
            if (values[key] == undefined) {
                values[key] = [];
            } else {
                values[key] = values[key].split(/[\r\n\t]/).filter(item => item)
            }
        }
        if (!values[key]) {
            values[key] = '';
        }
    });


    try {
        if (values["pnl_ids"].length == 0) {
            closeFullLoading();
            return message.warn("Please input pnl_id");
        }
        let { api01: { Data: { Table: Data } } } = await GetCommon({ ...baseParams, ...values });
        if (Data.length == 0) {
            PageSetting.moreData = [];
            closeFullLoading();
            return message.warn('无数据!');
        }

        PageSetting.moreData = Data;
        PageSetting.Columns = Object.keys(Data[0]).map((item,index) => {
            return {
                title: item,
                align: 'center',
                dataIndex: item,
                className: index <=2 ? 'v-center' : null,
            }
        });
        antcustomCell(PageSetting.Columns,({record, rowindex, column, item, index})=>{
            return {
                ...antmegercell({record, rowindex, column},["fac_id","panel_id"],PageSetting.moreData)
            }
        });

        closeFullLoading();
    } catch (error) {
        console.log(error)
        closeFullLoading();
    }

}

onMounted(async () => {
})
</script>

<style lang="less" scoped></style>