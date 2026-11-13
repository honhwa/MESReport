<template>
    <div class='Report'>
        <a-card>
            <a-row>
                <a-col :span='24 - PageParams.TableSpan' class='ScrollForm'>
                    <SchemaForm @submit='confirm'> </SchemaForm>
                    <ul>
                        <p>注意事項：</p>
                        <li>
                            Abnormal Only means Abnormal flag1 or flag2 or flag5 has data
                        </li>
                        <li>
                            PPbox ID include PPbox ID and Cassette ID
                        </li>
                        <li>
                            9940站點為LCM RECV BANK,此資料並不屬於LCD WIP量,LCD WIP站點為0000-9900
                        </li>
                        <li>
                            查History資料時,要查歷史日期真實的WIP量應選Night.
                        </li>
                        <li>Update frequency: At last 30 Minutes </li>
                    </ul>
                </a-col>
                <a-col :span='PageParams.TableSpan'>
                    <div ref='report'>
                        <DynamicTable row-key='report' :header-title='PageSetting.Title' :PageParams='PageParams'
                            :columns='PageSetting.Columns' export-type='Exceljs' showFavorite sticky
                            :key='PageSetting.Columns' bordered :row-class-name='getRowClassNmae'
                            :data-source='PageSetting.moreData' :data-all='PageSetting.moreData' :pagination='false'
                            :flexwidth='1' :export-file-name='PageSetting.Title'>
                            <template #export-button>
                                <a-button type='success'> 导出 </a-button>
                            </template>
                        </DynamicTable>
                        <DynamicTable row-key='report' :header-title='PageSetting.BottomTitle' :PageParams='PageParams'
                            :columns='PageSetting.BottomColumns' export-type='Exceljs' showFavorite sticky
                            :key='PageSetting.BottomColumns' bordered :row-class-name='getRowClassNmae'
                            :data-source='PageSetting.BottommoreData' :data-all='PageSetting.BottommoreData'
                            :pagination='false' :flexwidth='1' :export-file-name='PageSetting.BottomTitle'>
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
import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect';
import { ReportCommon } from '@/store/module/ReportCommon';
import { message } from 'ant-design-vue';
import { getRowClassNmae, antcustomCell, antdclick } from '@/pages/ReportPortal/utils/common'
const UseParamsAdd = ReportCommon()
let FactoryType: string = UseParamsAdd.Fab;
const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});
const report = ref<HTMLElement | null>(null);
function getImport(site: string) {
    const { schemas, optionsdata } = require(`@/pages/ReportPortal/FS/WIPLotSummaryReport/${site}/form-schema`);
    const { baseColumns } = require(`@/pages/ReportPortal/FS/WIPLotSummaryReport/${site}/Table`);
    return { schemas, optionsdata, baseColumns };
}
let { schemas, optionsdata, baseColumns } = getImport(FactoryType);

let PageSetting = reactive<BasePageParams.BasePageSetting>({
    List: [] as any,
    Columns: [] as any,
    Title: 'NHLCD WIP Lot Summary ',
    moreData: [],
    SQLHidden: '',
    APIColumns: [] as any,

    BottomList: [] as any,
    BottomColumns: [] as any,
    BottommoreData: [] as any,
    BottomTitle: ""
});
switch (FactoryType) {
    case '':
        PageSetting.Title = '';
        break;
    default:
        break;

}
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
        apiName: "WIPLotSummaryReport",
        FactoryType: FactoryType,
        G_FAC: UseParamsAdd.G_FAC_COM,
        ReportType: "WIPLotDataSet"
    };
    Object.keys(values).forEach((key) => {
        if (!values[key]) {
            values[key] = '';
        }
    });


    try {
        let { api01: { Data: { Table: Data } } } = await GetCommon({ ...baseParams, ...values });
        if (Data.length == 0) {
            PageSetting.moreData = [];
            closeFullLoading();
            return message.warn('无数据!');
        }
        PageSetting.moreData = Data;
        PageSetting.Columns = Object.keys(PageSetting.moreData[0]).map(item => {
            return {
                title: item,
                dataIndex: item
            }
        });
        antcustomCell(PageSetting.Columns, ({ record, rowindex, column, item }) => {
            return {
                ...antdclick(column, Object.keys(PageSetting.moreData[0]), async () => {
                    try {
                        openFullLoading();
                        let { api01: { Data: { Table: Data } } } = await GetCommon({
                            apiName: "WIPLotSummaryReport",
                            FactoryType: FactoryType,
                            G_FAC: UseParamsAdd.G_FAC_COM,
                            ReportType: "WIPLotDetail",
                            DetailType: values["DetailType"],
                            Firstclickvalue: record[item["dataIndex"]],
                            Firstclickfield: item["dataIndex"],
                            // record
                            record: JSON.stringify(record)
                        });
                        if (Data.length != 0) {
                            PageSetting.BottomColumns = Object.keys(Data[0]).map(item => {
                                return {
                                    title: item,
                                    dataIndex: item
                                }
                            });
                            PageSetting.BottommoreData = Data;
                            closeFullLoading();
                            PageSetting.BottomTitle = "WIP Lot Detail of row " + (rowindex + 1)
                        } else {
                            PageSetting.BottommoreData = [];
                            PageSetting.BottomColumns = [];
                            PageSetting.BottomTitle = "";
                            closeFullLoading();
                            return message.warn("無數據!")
                        }
                    } catch (error: any) {
                        console.log(error);
                        message.error(error);
                        closeFullLoading();

                    }



                })
            }
        });
        PageSetting.BottommoreData = [];
        PageSetting.BottomColumns = [];
        PageSetting.BottomTitle = "";
        closeFullLoading();
    } catch (error) {
        console.log(error)
        PageSetting.moreData = [];
        closeFullLoading();
    }

}

onMounted(async () => {

    await async function getGrade() {
        let { api01: { Data: { Table: Data } } } = await GetCommon({
            apiName: 'WIPLotSummaryReport_Grade',
            FactoryType: FactoryType,
        });
        optionsdata["Grade"] = Data.map(item => {
            return {
                label: item["CODE"],
                value: item["CODE"]
            }
        })
        optionsdata["Grade"].unshift({
            label: "All",
            value: "All"
        })

    }()
    await async function getOperation() {
        let { api01: { Data: { Table: Data } } } = await GetCommon({
            apiName: 'WIPLotSummaryReport_Operation',
            FactoryType: FactoryType,
        });
        optionsdata["Operation"] = Data.map(item => {
            return {
                label: item["PROC_ID"],
                value: item["PROC_ID"]
            }
        })
        optionsdata["Operation"].unshift({
            label: "All",
            value: "All"
        })
        optionsdata["Operation"].push({
            label: "9900",
            value: "9900"
        })

    }()
    await async function getOperation() {
        let { api01: { PFCD: { Table: PFCD }, Prod: { Table: Prod } } } = await GetCommon({
            apiName: 'WIPLotSummaryReport_PFCD',
            FactoryType: FactoryType,
        });
        optionsdata["PFCD"] = PFCD.map(item => item["PFCD"])
        optionsdata["Prod"] = Prod.map(item => {
            return {
                label: item["PROD"],
                value: item["PROD"]
            }
        })
        optionsdata["Prod"].unshift({
            label: "All",
            value: "All"
        })
    }()

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