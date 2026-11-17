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
                            :style="{ 'margin-bottom': '40px' }" showFavorite sticky :key='PageSetting.Columns' bordered
                            :row-class-name='getRowClassNmae' :data-source='PageSetting.moreData'
                            :data-all='PageSetting.moreData' :pagination='false' :flexwidth='1'
                            :export-file-name='PageSetting.Title'>
                            <template #export-button>
                                <a-button type='success'> 导出 </a-button>
                            </template>
                        </DynamicTable>
                        <DynamicTable row-key='report' :header-title='PageSetting.bottomtitle'
                        v-show="PageSetting.Type == 'LINE'"
                            :columns='PageSetting.bottomcolumns' sticky :key='PageSetting.bottomcolumns' bordered
                            :row-class-name='getRowClassNmae' :data-source='PageSetting.bottomList'
                            :data-all='PageSetting.bottomList' :pagination='false' :flexwidth='1'
                            :export-file-name='PageSetting.bottomtitle'>
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
import { getRowClassNmae, megercell, getfinalsum } from '@/pages/ReportPortal/utils/common'
const UseParamsAdd = ReportCommon()
let FactoryType: string = UseParamsAdd.Fab;
const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});
const report = ref<HTMLElement | null>(null);
function getImport(site: string) {
    const { schemas } = require(`@/pages/ReportPortal/FS/DailyReport/${site}/form-schema`);
    const { SumColumns, LineTopcolumns, LinebottomColumns } = require(`@/pages/ReportPortal/FS/DailyReport/${site}/Table`);
    return { schemas, SumColumns, LineTopcolumns, LinebottomColumns };
}
let { schemas, SumColumns, LineTopcolumns, LinebottomColumns } = getImport(FactoryType);

let PageSetting = reactive<BasePageParams.BasePageSetting>({
    List: [] as any,
    Columns: [] as any,
    Title: '',
    moreData: [],
    SQLHidden: '',
    APIColumns: [] as any,
    bottomList: [] as any,
    bottomcolumns: [] as any,
    bottomtitle: '',
    Type:'',
});

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
        apiName: "DailyReport",
        FactoryType: FactoryType,
        G_FAC: UseParamsAdd.G_FAC_COM
    };
    Object.keys(values).forEach((key) => {
        if (!values[key]) {
            values[key] = '';
        }
    });
    try {
        let Data;
        PageSetting.Type = values["type"];
        switch (values["type"]) {
            case 'SUM':
                let { api01: { Result: Data0 } } = await GetCommon({ ...baseParams, ...values });
                Data = Data0;
                if (Data.length == 0) {
                    PageSetting.moreData = [];
                    PageSetting.bottomList = [];
                    closeFullLoading();
                    return message.warn('无数据!');
                }
                PageSetting.Title = 'HA Daily Sum Report';
                PageSetting.Columns = SumColumns(values["vDate_s"]);
                PageSetting.moreData = Data;

                megercell(PageSetting.Columns, ["prod"], PageSetting.moreData);
                break;
            case 'LINE':
                let { api01: { Result: { Table: Data1 }, Result2: { Table: Data2 } } } = await GetCommon({ ...baseParams, ...values });
                Data = Data1;
                if (Data.length == 0) {
                    PageSetting.moreData = [];
                    closeFullLoading();
                    return message.warn('无数据!');
                }

                PageSetting.moreData = Data;
                // 分組計算
                let groupfloor = {};
                PageSetting.moreData.forEach(item => {
                    item["BOND_LINE"] = `#${item["BOND_LINE"]}`;
                    item["ASSY_LINE"] = `#${item["ASSY_LINE"]}`;
                    item["CDP_LINE"] = `#${item["CDP_LINE"]}`;
                    if (item["BOND_PASS_ACT"]) {
                        item["BOND_Diff"] = item["BOND_PASS_ACT"] - item["BOND_MPS_PLAN"]
                    }
                    if (item["CDP_PASS_ACT"]) {
                        item["CDP_Diff"] = item["CDP_PASS_ACT"] - item["CDP_MPS_PLAN"]
                    }
                    if (item["ASSY_PASS_ACT"]) {
                        item["ASSY_Diff"] = item["ASSY_PASS_ACT"] - item["ASSY_MPS_PLAN"];
                    }
                    if (groupfloor[item["FLOOR"]]) {
                        groupfloor[item["FLOOR"]].push(item)
                    } else {
                        groupfloor[item["FLOOR"]] = [];
                        groupfloor[item["FLOOR"]].push(item);

                    }
                })
                let calculist = [`BOND_MPS_PLAN`, `BOND_PASS_ACT`, `BOND_Diff`, 'BOND_SCRAP_QTY', 'BOND_WIP', 'ASSY_PASS_ACT', 'ASSY_Diff', 'ASSY_SCRAP_QTY', 'ASSY_WIP', 'CDP_MPS_PLAN', 'CDP_PASS_ACT', 'CDP_Diff', 'CDP_SCRAP_QTY', 'CDP_WIP'];
                let finalsum = getfinalsum(PageSetting.moreData, calculist, "BOND_LINE", "TTL");
                finalsum["FLOOR"] = "ALL";
                PageSetting.moreData = [];
                PageSetting.moreData.push(finalsum);
                for (const key in groupfloor) {
                    let temp = getfinalsum(groupfloor[key], calculist, "BOND_LINE", "TTL");
                    temp["FLOOR"] = groupfloor[key][groupfloor[key].length - 1]["FLOOR"]
                    groupfloor[key].push(temp);
                    PageSetting.moreData.push(...groupfloor[key]);
                }


                let bottomcalculist = ["BL_Diff", "BL_PASS_ACT", "BL_MPS_PLAN"];
                PageSetting.Columns = LineTopcolumns;
                megercell(PageSetting.Columns, ["FLOOR"], PageSetting.moreData);
                PageSetting.Title = 'LCMHA Daily Plan Report';
                PageSetting.bottomList = Data2;
                PageSetting.bottomList.forEach(item => {
                    if (item["BL_PASS_ACT"]) {
                        item["BL_Diff"] = item["BL_PASS_ACT"] - item["BL_MPS_PLAN"]
                    }
                    item["BL_LINE"] = `#${item["BL_LINE"]}`
                })
                let bottomfinalsum = getfinalsum(PageSetting.bottomList,bottomcalculist,"BL_LINE","TTL");
                bottomfinalsum["BL_Diff"] = bottomfinalsum["BL_PASS_ACT"] - bottomfinalsum["BL_MPS_PLAN"]
                PageSetting.bottomList.unshift(bottomfinalsum);
                PageSetting.bottomList.push(bottomfinalsum);
                
                PageSetting.bottomcolumns = LinebottomColumns;
                PageSetting.bottomcolumns.forEach(item => {
                    megercell(item.children, ["BL_WIP"], PageSetting.bottomList);
                }
                )
                PageSetting.bottomtitle = "BLUA Daily Plan Report"
                break;
            default:
                break;
        }
        closeFullLoading();
    } catch (error) {
        console.log(error);
        closeFullLoading();
    }
}

onMounted(async () => {
})
</script>

<style lang="less" scoped>
:deep(.ant-table) {
    height: 35vh !important;
}

:deep(.ant-table-container) {
    height: 35vh !important;
}

:deep(.ant-table-wrapper) {
    height: 35vh !important;
    // overflow: auto;
}

:deep(.ant-table-body) {
    height: 35vh !important;
    overflow: auto;
}
</style>