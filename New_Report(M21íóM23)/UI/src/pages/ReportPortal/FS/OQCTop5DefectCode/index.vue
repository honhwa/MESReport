<template>
    <div class="OQCTop5DefectCode">
        <a-card>

            <a-row>
                <a-col :span="24 - PageParams.TableSpan" class="ScrollTable" v-show="PageParams.ShowMenu">
                    <SchemaForm @submit="confirm"> </SchemaForm>
                </a-col>
                <a-col :span="PageParams.TableSpan">
                    <div  ref="OQCTop5DefectCode">
                        <DynamicTable row-key="OQCTop5DefectCode" header-title="LCMA OQC Top 5 Defect Code"
                        showFavorite
                        sticky
                        :style="{ display: 'inline-block' }"
                        :PageParams="PageParams"
                            :columns="PageSetting.TopColumns" :key="PageSetting.TopColumns" bordered size="small"
                            :data-source="PageSetting.TopList" :pagination="false" :row-class-name="getRowClassNmae"
                            :scroll="{ x: PageSetting.x, y: 650 }" export-file-name="导出">
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

<script lang="tsx" setup>
import {PageParams} from '@/components/core/dynamic-table/src/dynamic-table'
import { useTable } from '@/components/core/dynamic-table';
import { reactive, ref } from '@vue/reactivity';
import { useForm } from '@/components/core/schema-form';
import { GetCommon } from '@/api/pages/common';
import { onMounted } from '@vue/runtime-core';
import { cloneDeep } from 'lodash';
import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { megercell, sortArray } from '@/pages/ReportPortal/utils/common'
import dayjs from 'dayjs';

const UseParamsAdd = AddCommonPageParams()
let FactoryType: any = UseParamsAdd.Fab;
const OQCTop5DefectCode = ref<HTMLElement | null>(null);

function getImport(site: any) {
    const { schemas, optionsdata } = require(`@/pages/ReportPortal/FS/OQCTop5DefectCode/${site}/form-schema`);
    const { baseColumns } = require(`@/pages/ReportPortal/FS/OQCTop5DefectCode/${site}/table`);
    return { schemas, optionsdata, baseColumns };
}
const { schemas, optionsdata, baseColumns } = getImport(UseParamsAdd.Fab);
// 頁面數據
let PageSetting = reactive({
    TopList: [] as any,
    TopColumns: [] as any,
    Message: {},
    x: 300,
});
const [DynamicTable] = useTable();

const { mounted, InfiniteScroll, mountedowner, mountedLCDGrade } = useRemoteSelect()
const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas,
    actionColOptions: {
        span: 24
    },
    fieldMapToTime: [['fieldTime', ['vDate_s', 'vDate_e1'], 'YYYY-MM-DD']],
    layout: "vertical"
});
let TopTableParams = reactive({
    name: "Date",
    apiName: "OQCTop5DfctDataSet",
    FactoryType: FactoryType,
    G_FAC: "A",
    FromDate: "",
    FromTime: "",
    ToDate: "",
    ToTime: "",
    Shift: "",
    Floor: "",
    Operation: "",
    Product: [],
    LCMOwner: [],
    Judgement: [],
    DefectGroup: [],
    LCDGrade: [],

});
function getRowClassNmae(record: Object) {
    if (record && Object.keys(record).length != 0) {
        if (record["ERRC_DESCR"] == "電氣(c)" || record["OQC_GROUP"] == "C") {
            return 'Purple';
        } else if (record["ERRC_DESCR"] == "品位(f)" || record["OQC_GROUP"] == "F") {
            return "YellowGray"
        } else if (record["ERRC_DESCR"] == "外觀(d)" || record["OQC_GROUP"] == "D") {
            return 'GreenBlue';
        }

    }
}

async function confirm() {
    PageSetting.TopList = [];
    PageSetting.TopColumns = [];
    let tempkeys = Object.keys(TopTableParams);
    let tempdynamicFormRef = cloneDeep(dynamicFormRef.formModel);
    for (let i in tempdynamicFormRef) {
        if (tempkeys.includes(i)) {
            if (["FromTime", "ToTime"].includes(i)) {
                TopTableParams[i] = ` ${dayjs(tempdynamicFormRef[i]["$d"]).format("HH:mm")}`
            }
            else {
                TopTableParams[i] = tempdynamicFormRef[i];
            }
        }
    }

    let { Date: {
        Data: { Table: Data },
        Data1: { Table: Data1 },
    } } = await GetCommon(TopTableParams);
    //取第一列需要sub_prod:2v400
    let modelarr: string[] = [];
    let testobj = {};
    Data1.forEach(item => {
        if (!modelarr.includes(item["SUB_PROD"])) {
            modelarr.push(item["SUB_PROD"]);
        }

    });
    sortArray(Data, "SUB_PROD", "asc");
    //處理上面3個
    Data.forEach(item => {
        if (modelarr.includes(item["SUB_PROD"])) {
            if (!testobj[item["SUB_PROD"]]) {
                testobj[item["SUB_PROD"]] = [
                    { ERR_CODE: item["SUB_PROD"], ERRC_DESCR: "電氣(c)", OUT_QTY: item["OUT_QTY_1"], QTY: 0, Rate: `${item["OUT_QTY_1"]}%` },
                    { ERR_CODE: item["SUB_PROD"], ERRC_DESCR: "品位(f)", OUT_QTY: item["OUT_QTY_1"], QTY: 0, Rate: `${item["OUT_QTY_1"]}%` },
                    { ERR_CODE: item["SUB_PROD"], ERRC_DESCR: "外觀(d)", OUT_QTY: item["OUT_QTY_1"], QTY: 0, Rate: `${item["OUT_QTY_1"]}%` },
                ]
            } else {
                testobj[item["SUB_PROD"]].push(
                    { ERR_CODE: item["SUB_PROD"], ERRC_DESCR: "電氣(c)", OUT_QTY: item["OUT_QTY_1"], QTY: 0, Rate: `${item["OUT_QTY_1"]}%` },
                    { ERR_CODE: item["SUB_PROD"], ERRC_DESCR: "品位(f)", OUT_QTY: item["OUT_QTY_1"], QTY: 0, Rate: `${item["OUT_QTY_1"]}%` },
                    { ERR_CODE: item["SUB_PROD"], ERRC_DESCR: "外觀(d)", OUT_QTY: item["OUT_QTY_1"], QTY: 0, Rate: `${item["OUT_QTY_1"]}%` },);
            }
        }

    })

    // 處理下面5個
    Data1.forEach(item => {
        if (!testobj[item["SUB_PROD"]]) {
            testobj[item["SUB_PROD"]] = [item];

        } else {
            testobj[item["SUB_PROD"]].push(item);
        }
    })





    let finalData: object[] = [];
    for (let i in testobj) {
        for (let j = 3; j < testobj[i].length; j++) {
            switch (testobj[i][j]["OQC_GROUP"]) {
                case "D":
                    testobj[i][2]["QTY"] += testobj[i][j]["QTY"];
                    break;
                case "F":
                    testobj[i][1]["QTY"] += testobj[i][j]["QTY"];
                    break;
                case "C":
                    testobj[i][0]["QTY"] += testobj[i][j]["QTY"];
                    break;
                // case " ":
                //     testobj[i][0]["QTY"] += testobj[i][j]["QTY"];
                //     break;
                default:
                    break;
            }

        }
        testobj[i][0]["Rate"] = ((testobj[i][0]["QTY"] / testobj[i][0]["OUT_QTY"]) * 100).toFixed(4);
        testobj[i][1]["Rate"] = ((testobj[i][1]["QTY"] / testobj[i][1]["OUT_QTY"]) * 100).toFixed(4);
        testobj[i][2]["Rate"] = ((testobj[i][2]["QTY"] / testobj[i][2]["OUT_QTY"]) * 100).toFixed(4);
        let obj = {
            ERR_CODE: "Total Failure Rate",
            SUB_PROD: "",
            ERRC_DESCR: "",
            OUT_QTY: "",
            QTY: "",
            Rate: 0,
        }
        for (let j = 3; j < testobj[i].length; j++) {
            testobj[i][j]["Rate"] = ((testobj[i][j]["QTY"] / testobj[i][0]["OUT_QTY"]) * 100).toFixed(4);
            obj["Rate"] += Number(testobj[i][j]["Rate"]);
        }
        testobj[i].push(obj);
        for (let j = 0; j < testobj[i].length; j++) {
            testobj[i][j]["Rate"] = `${testobj[i][j]["Rate"]}%`;
        }
        finalData.push(...testobj[i]);
    }
    let tempColumns = cloneDeep(baseColumns);
    megercell(tempColumns, ["ERR_CODE"], finalData);
    PageSetting.TopColumns = tempColumns;
    PageSetting.TopList = finalData;
}
InfiniteScroll(OQCTop5DefectCode, PageSetting as any);
onMounted(
    async () => {
        await mounted(optionsdata, FactoryType);
        await mountedowner(optionsdata, FactoryType)
        await mountedLCDGrade(optionsdata, FactoryType)
    }
)
</script>

<style lang="less" scoped>
.btn-rows {
    button {
        margin-right: 12px;
    }
}

.OQCTop5DefectCode :deep(.sub-total) {
    background-color: #ffb468 !important;
}

.OQCTop5DefectCode :deep(.total) {
    background-color: #ff80c0 !important;
}

.swapper {
    display: flex;
    flex-direction: column;
}
</style>
