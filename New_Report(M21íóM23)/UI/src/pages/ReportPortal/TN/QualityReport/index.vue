<template>
    <div class="QualityReport">
        <a-card>
            <a-row>
                <a-col :span="24 - PageParams.TableSpan" class="ScrollForm" v-show="PageParams.ShowMenu">
                    <SchemaForm @submit="confirm"> </SchemaForm>
                </a-col>
                <a-col :span="PageParams.TableSpan">
                    <!-- <div class="ScrollTable" ref="reportref">
                        <DynamicTable row-key="reportref" :header-title="PageSetting.Title" showFavorite
                        
                            sticky
                            :columns="PageSetting.TopColumns" :key="PageSetting.TopColumns"
                            bordered size="small" :row-class-name="getRowClassName" :data-source="PageSetting.TopList"
                            :pagination="false" :flexwidth="-8" :data-all='PageSetting.moreData'
                            export-file-name="Export">
                            <template #export-button>
                                <a-button type="success"> Export </a-button>
                            </template>
                        </DynamicTable>
                    </div> -->
                    <DynamicTable
                        row-key="reportref"
                        :header-title="PageSetting.Title"
                        :PageParams="PageParams"
                        id="reportref"
                        export-type="Table"
                        :columns="PageSetting.TopColumns"
                        showFavorite
                        :key="PageSetting.TopColumns"
                        bordered
                        :flexwidth="1"
                        :row-class-name="getRowClassName_Sel"
                        :data-source="PageSetting.moreData"
                        :data-all="PageSetting.moreData"
                        :pagination="false"
                        sticky
                        export-file-name="Export"
                    >
                        <template #export-button>
                            <a-button type="success"> Export </a-button>
                        </template>
                    </DynamicTable>
                </a-col>
            </a-row>
        </a-card>
    </div>
</template>


<script lang="tsx" setup>
import { PageParams } from '@/components/core/dynamic-table/src/dynamic-table'
import { useTable } from '@/components/core/dynamic-table';

import { useForm } from '@/components/core/schema-form';
import { GetCommon } from '@/api/pages/common';
import { reactive, unref, ref, onMounted, watch, computed, Ref } from 'vue';

import { message } from 'ant-design-vue';
import { getrowsum, Inserttotalrow } from "@/pages/ReportPortal/utils/common";
import { cloneDeep } from 'lodash';
import { PovitParams } from "@/utils/reportcommon/provit";
import useRemoteSelect from '@/pages/ReportPortal/utils/TNuseRemoteSelect';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { ReportCommon } from '@/store/module/ReportCommon';
import { getRowClassName } from '@/pages/ReportPortal/utils/TNcommon'
import router from '@/router';
import { useLoading } from '@/components/feat/Loading';
//import {optionsdata} from '@/pages/ReportPortal/Static/TNCommon-Form-schema';

const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});
const UseParamsAdd = AddCommonPageParams()
const useReportCommon = ReportCommon();
let ReportParamsItem = reactive({
    APIMessage:'',
    FormParams: {},
    FirstTableParams: {},
    SecondTableParams:reactive({}),
    ThirdTableParams:{},
    ThirdTableClickParams:{},
    Fab:'',
    G_FAC_COM:'',
});
let ReportNameParams = `QualityReportDetail_${UseParamsAdd.Fab}`;

let FactoryType: any = UseParamsAdd.Fab;

let fab = "MOD";
if(UseParamsAdd.Fab == "MOD1" || UseParamsAdd.Fab == "CARUX")
{
    fab = UseParamsAdd.Fab;
}

// 頁面數據
let PageSetting = reactive({
    TopList: [] as any,
    TopColumns: [] as any,
    Message: {},
    Title: "",
    moreData: [] as any,
});

PageSetting.Title=`${UseParamsAdd.Fab} Quality Report`

function getImport(site: any) {
    //const { optionsdata } = require('@/pages/ReportPortal/Static/TNCommon-Form-schema');
    const { schemas } = require(`@/pages/ReportPortal/TN/QualityReport/${fab}/form-schema`);
    return { schemas };
}
const { schemas } = getImport(fab);
const [DynamicTable] = useTable();
const { mounted, InfiniteScroll, mountedEQP} = useRemoteSelect()
const reportref = ref<HTMLElement | null>(null);

const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas: schemas(FactoryType),
    actionColOptions: {
        span: 24
    },
    fieldMapToTime: [['fieldTime', ['vDate_s', 'vDate_e1'], 'YYYY-MM-DD']],
    layout: "vertical"
});
let TopTableParams = reactive({
    name: "Date",
    apiName: "TN_QualityDataSet",
    ReportType: "QualityDataSet",
    FactoryType: FactoryType,
    G_FAC: "A",
    DisplayMode: '',
    EQPID: []
});
async function confirm(values) {
    openFullLoading();
    PageSetting.TopList = [];
    PageSetting.TopColumns = [];

    Object.keys(values).forEach((key) => {
        if (!values[key]) {
            values[key] = '';
        }
        if (["WorkOrderLike", "Operator"].includes(key)) {
            values[key] = values[key].split(',')
        }
    });

    //給ParamsData賦值
    let values1 = Object.keys(TopTableParams);
    for (let i in cloneDeep(dynamicFormRef.formModel)) {
        if (values1.includes(i)) {
            TopTableParams[i] = cloneDeep(dynamicFormRef.formModel[i]);
        }
    }
    //TopTableParams = { ...TopTableParams, ...values }
debugger
    if (['equip_nbr'].includes(TopTableParams['DisplayMode']) && TopTableParams['EQPID'].length == 0) {
        message.warning('請選擇機台!');
        closeFullLoading();
    } 
    else 
    {
        try {
            const { Date: {
                Data_Out: { Table: Data_Out },//前6行的组成,表头的组成
                Data_Defect: { Table: Data_Defect },
                SQLFilter,
                SQLFilter_Defect
                /*  Data2: { Table: Data2 = null},
                Data3: { Table: Data3 = null}, */
            } } = await GetCommon({ ...values, ...TopTableParams });
            if (Data_Out.length == 0 && Data_Defect.length == 0) {
                closeFullLoading();
                return message.warn("無數據")
            }
            let Message = {                
                    SQLFilter: SQLFilter,
                    SQLFilter_Defect: SQLFilter_Defect,
                };
            let baseColumns: any[] = [
                {
                    title: 'Process Name',
                    align: 'center',
                    dataIndex: `DFCT_CODE`,
                    key: `DFCT_CODE`,
                    className: "v-center",
                    fixed: 'left',
                },
                {
                    title: 'Process Name',
                    align: 'center',
                    dataIndex: `ERRC_DESCR`,
                    key: `ERRC_DESCR`,
                    className: "v-center",
                    fixed: 'left',
                },
            ];
            let finalData: object[] = [];
            // inspection qty记录
            let inspectionobj = {
                "DFCT_CODE": "INSPECTION QTY",
                "ERRC_DESCR": "INSPECTION QTY",
                "TOTAL YIELD QTY": "+PD",
                "TOTAL YIELD RATE": "-PD",
            };
            let good = {
                "DFCT_CODE": "GOOD",
                "ERRC_DESCR": "GOOD",
                "TOTAL YIELD QTY": 0,
                "TOTAL YIELD RATE": 0,
            };
            let ng = {
                "DFCT_CODE": "NG",
                "ERRC_DESCR": "NG",
                "TOTAL YIELD QTY": 0,
                "TOTAL YIELD RATE": 0,
            };
            let yieldrate = {
                "DFCT_CODE": "YIELD Rate",
                "ERRC_DESCR": "YIELD Rate",
                "TOTAL YIELD QTY": 1,
                "TOTAL YIELD RATE": 1,
            };
            let yieldloss = {
                "DFCT_CODE": "YIELD LOSS",
                "ERRC_DESCR": "YIELD LOSS",
                "TOTAL YIELD QTY": 1,
                "TOTAL YIELD RATE": 1,
            };
            let defectitem = {
                "DFCT_CODE": "Defect ITEM",
                "ERRC_DESCR": "Defect ITEM",
                "TOTAL YIELD QTY": "QTY",
                "TOTAL YIELD RATE": "%",
            };
            let tempsum = 0;
            let totalyieldsum = 0;
            let middleProps: any[] = [];
            let middlerateProps: any[] = [];
            let middlesecondqtyprops: any[] = [];
            Data_Out.forEach(item => {
                middleProps.push(item["WORK_CTR"]);
                middlerateProps.push(`${item["WORK_CTR"]}RATE`);
                middlesecondqtyprops.push(`${item["WORK_CTR"]}-SECOND_QTY`);
                if (item["WORK_NAME"] == "C INSPECTION") {
                    tempsum = item["REJECT_QTY"]
                }
                baseColumns.push(
                    {
                        title: item["WORK_NAME"],
                        align: 'center',
                        dataIndex: `${item["WORK_CTR"]}`,
                        key: `${item["WORK_CTR"]}QTY`,
                    },
                    {
                        title: item["WORK_NAME"],
                        align: 'center',
                        dataIndex: `${item["WORK_CTR"]}RATE`,
                        key: `${item["WORK_CTR"]}RATE`,
                    }
                );
                totalyieldsum += item["QTY"];
                inspectionobj[`${item["WORK_CTR"]}`] = item["QTY"];
                inspectionobj[`${item["WORK_CTR"]}RATE`] = item["QTY"];
                ng[`${item["WORK_CTR"]}`] = item["REJECT_QTY"];
                ng[`${item["WORK_CTR"]}RATE`] = item["REJECT_QTY"];
                good[`${item["WORK_CTR"]}`] = item["QTY"] - item["REJECT_QTY"];
                good[`${item["WORK_CTR"]}RATE`] = item["QTY"] - item["REJECT_QTY"];
                yieldrate[`${item["WORK_CTR"]}`] = ((((item["QTY"] - item["REJECT_QTY"]) / item["QTY"]) * 100).toFixed(3));
                yieldrate[`${item["WORK_CTR"]}RATE`] = ((((item["QTY"] - item["REJECT_QTY"]) / item["QTY"]) * 100).toFixed(3));
                yieldloss[`${item["WORK_CTR"]}`] = ((item["REJECT_QTY"] / item["QTY"]) * 100).toFixed(3);
                yieldloss[`${item["WORK_CTR"]}RATE`] = ((item["REJECT_QTY"] / item["QTY"]) * 100).toFixed(3);
                defectitem[`${item["WORK_CTR"]}`] = "QTY";
                defectitem[`${item["WORK_CTR"]}RATE`] = "%";
                good["TOTAL YIELD QTY"] += good[`${item["WORK_CTR"]}`];
                good["TOTAL YIELD RATE"] += good[`${item["WORK_CTR"]}RATE`];
                ng["TOTAL YIELD QTY"] += ng[`${item["WORK_CTR"]}`];
                ng["TOTAL YIELD RATE"] += ng[`${item["WORK_CTR"]}RATE`];
            });
            middleProps.forEach(item => {
                yieldrate["TOTAL YIELD QTY"] *= Number(yieldrate[item]) / 100;
            })
            yieldrate["TOTAL YIELD QTY"] = Number(yieldrate["TOTAL YIELD QTY"].toFixed(5)) * 100;
            yieldrate["TOTAL YIELD QTY"] = Number(yieldrate["TOTAL YIELD QTY"].toFixed(3));

            yieldrate["TOTAL YIELD RATE"] = yieldrate["TOTAL YIELD QTY"];
            yieldloss["TOTAL YIELD QTY"] = Number((100 - yieldrate["TOTAL YIELD QTY"]).toFixed(3));
            yieldloss["TOTAL YIELD RATE"] = yieldloss["TOTAL YIELD QTY"];
            // 统一添加百分号
            [...middleProps, ...middlerateProps, "TOTAL YIELD QTY", "TOTAL YIELD RATE"].forEach(item => {
                yieldrate[item] = yieldrate[item] + "%";
                yieldloss[item] = yieldloss[item] + "%";
            })
            finalData.push(inspectionobj, good, ng, yieldrate, yieldloss, defectitem);
            // Data1添加属性
            Data_Defect.forEach(item => {
                item[`${item["WORK_CTR"]}-SECOND_QTY`] = item["SECOND_QTY"]
            })

            // Data1處理,行列轉換
            // 表格数据
            let config: PovitParams = {
                Data: Data_Defect,
                povitColumn: {
                    "WORK_CTR": 'MAIN_QTY'
                },
                changeColumn: middleProps,
                repeatColumn: ["DFCT_CODE", "ERRC_DESCR"]
            }
            let povitData = TableMerge(config);
            function TableMerge(config: PovitParams) {
                // 步骤1：行转列
                config.Data.forEach(item => {
                    for (let i in config.povitColumn) {
                        // 遍历key
                        if (i.trim().split(" ").length > 1) {
                            let key = "";
                            i.trim().split(" ").forEach((Iitem, index) => {
                                if (index != i.trim().split(" ").length - 1) {
                                    key += item[Iitem] + " "
                                } else {
                                    key += item[Iitem]
                                }
                            })
                            item[key] = item[config.povitColumn[i]]
                        } else {
                            item[item[i]] = item[config.povitColumn[i]]
                        }
                    }
                })
                // 步骤2：进行合并，两个数组进行合并
                // Data1和Data2是比较数组
                let Data1 = cloneDeep(config.Data);
                let Data2 = cloneDeep(config.Data);
                // 保存不同的索引
                let Data3: number[] = [];
                let regArr: any[] = [];
                for (let i = 0; i < Data1.length; i++) {
                    for (let j = i + 1; j < Data2.length; j++) {
                        config.repeatColumn.forEach(item => {
                            regArr.push(Data1[i][item] == Data2[j][item])
                        });
                        // 如果不相同
                        if (regArr.includes(false)) {
                            regArr = [];
                            continue;
                        } else {
                            // 相同，合并并且删除Data1中和Data2相同索引
                            Data1[i] = { ...Data1[i], ...Data2[j] }
                            Data3.push(j);
                        }
                    }
                }
                // 需要倒叙删除，不然就使用用filter
                Data3 = [...new Set(Data3)];
                for (let i = Data1.length; i >= 0; i--) {
                    Data3.forEach(item => {
                        if (i == item) {
                            Data1.splice(i, 1)
                        }
                    })
                }
                Data1.forEach(Titem => {
                    // 步骤3：添加属性,不能一边删除一边添加属性
                    config.changeColumn.forEach(item => {
                        if (!Titem.hasOwnProperty(item)) {
                            Titem[item] = null;
                        }
                    })
                })
                return Data1;
            }
            // 計算？品質
            povitData.forEach(item => {
                item["condition"] = item["ERRI_NBR"]
            })
            let tempmiddleProps = [...middleProps, ...middlerateProps];
            for (let i = 0; i < tempmiddleProps.length; i++) {
                tempmiddleProps[i] = String(tempmiddleProps[i])
            }
            // 列计算
            getrowsum(povitData, middleProps, "TOTAL YIELD QTY");
            let { insertrowList, insertIndexList } = Inserttotalrow(povitData, ["condition"], [...tempmiddleProps, ...middlesecondqtyprops, "TOTAL YIELD QTY"], "DFCT_CODE", "總計", false, false);
            /*  console.log("插入行位置", insertrowList);
            console.log("插入行数据", insertIndexList); */
            // 数组头部添加新的值
            insertIndexList.unshift(0);
            // 数组头部移除最后一位数字
            insertIndexList.pop();
            // 插入之后的位置
            let afterinsert: number[] = [];
            for (let i = 0; i < insertIndexList.length; i++) {
                povitData.splice(insertIndexList[i] + i, 0, insertrowList[i]);
                afterinsert.push(insertIndexList[i] + i)
            }
            afterinsert.forEach(item => {
                povitData[item]["DFCT_CODE"] = `${povitData[item + 1]["condition"]}品`;
                povitData[item]["ERRC_DESCR"] = `${povitData[item + 1]["condition"]}品`;
                povitData[item]["ERRI_NBR"] = `品`;
            })
            povitData.forEach(item => {
                middleProps.forEach((titem, index) => {
                    if (item[titem] != null) {
                        // 计算从第6行开始的百分号那一列的计算
                        item[middlerateProps[index]] = Number(((item[titem] / finalData[0][titem]) * 100).toFixed(3));
                        // 添加括号（）
                        if (item[`${titem}-SECOND_QTY`] != undefined) {
                            item[titem] = `${item[titem]}(${item[`${titem}-SECOND_QTY`]})`
                        } else {
                            item[titem] = '';
                        }

                    }
                })

                // item["TOTAL YIELD RATE"] = (item["TOTAL YIELD QTY"] /tempsum).toFixed(3)
                item["TOTAL YIELD RATE"] = Math.floor((item["TOTAL YIELD QTY"] / tempsum) * 1000) / 1000

            })
            // getrowsum(povitData, middlerateProps, "TOTAL YIELD RATE");
            finalData.push(...povitData);
            /*  console.log("middleProps", middleProps);
            console.log("middlerateProps", middlerateProps);
            console.log("middlesecondqtyprops", middlesecondqtyprops); */
            baseColumns.push(
                {
                    title: "TOTAL YIELD",
                    align: 'center',
                    dataIndex: `TOTAL YIELD QTY`,
                    key: `TOTAL YIELD QTY`,
                },
                {
                    title: "TOTAL YIELD",
                    align: 'center',
                    dataIndex: `TOTAL YIELD RATE`,
                    key: `TOTAL YIELD RATE`,

                }
            )
            async function handclick(record, rowindex, column, item, index) {
                if (["BLU1", "BLU2"].includes(FactoryType)) {
                    if (rowindex <= 5) {
                        return message.warn("此欄位沒有第二層資料")
                    }
                }

                /* console.log("点击了");
                console.log("record", record);
                console.log("rowindex", rowindex);
                console.log("column", column);
                console.log("item", item);
                console.log("index", index); */
                let bottomparams = cloneDeep(TopTableParams);
                bottomparams.ReportType = "QualityDetail1";
                if (["BLU1", "BLU2"].includes(FactoryType)) {
                    bottomparams["opercond"] = item["dataIndex"].replace("RATE", '');

                } else {
                    bottomparams["opercond"] = isNaN(item["dataIndex"]) ? Number(item["dataIndex"].replace("RATE", '')) : Number(item["dataIndex"]);

                }
                if (item["dataIndex"] == "TOTAL YIELD QTY" || item["dataIndex"] == "TOTAL YIELD RATE") {
                    bottomparams["opercond"] = "TOTAL YIELD"
                }
                console.log("合成的opercond", bottomparams["opercond"]);

                bottomparams["defcond1"] = record["DFCT_CODE"];
                debugger
                //UseParamsAdd.AddFirstTableParams(bottomparams);
                useReportCommon.AddFirstTableParams(`QualityReportDetail_${UseParamsAdd.Fab}`,bottomparams)
                router.push({ name: `QualityReportDetail_` + FactoryType });
            }
            let ClickColumn = [...tempmiddleProps, "TOTAL YIELD QTY", "TOTAL YIELD RATE"];
            // 合并表头
            baseColumns.forEach((item, index) => {
                if ((index % 2) === 0) {
                    item["colSpan"] = 2
                } else {
                    item["colSpan"] = 0
                }
                // item["title"] = ClickColumn.includes(item["dataIndex"]) ? `${item["title"]}*` : item["title"];//可点击的栏位显示*号
                item["customCell"] = (record: Object, rowIndex: number, column: Object) => {
                    let obj = {};
                    // 双击功能功能
                    let stylefunc = () => {
                        let tempStyle = {};
                        if (ClickColumn.includes(item["dataIndex"])) {
                            tempStyle = { cursor: 'pointer' }
                        } else {
                            tempStyle = {};
                        }
                        return { ...tempStyle };
                    }
                    obj["onDblclick"] = async () => {
                        if (ClickColumn.includes(item["dataIndex"])) {
                            handclick(record, rowIndex, column, item, index);
                        } else {
                            message.warning('此栏位没有二阶数据');
                        }
                    },
                        obj["style"] = stylefunc()
                    if (rowIndex < 5) {
                        if (column["key"].includes("QTY") && !column["key"].includes("TOTAL")) {
                            return {
                                colSpan: 2,// 跨几列合并，这里现在是跨两列
                                ...obj
                            }
                        } else if (column["key"].includes("RATE") && !column["key"].includes("TOTAL")) {
                            return {
                                colSpan: 0,
                                ...obj
                            }
                        }
                        else if (column["key"].includes("DFCT_CODE")) {
                            return {
                                colSpan: 2,
                                ...obj
                            }
                        } else if (column["key"].includes("ERRC_DESCR")) {
                            return {
                                colSpan: 0,
                                ...obj
                            }
                        }
                    } else if (rowIndex == 5) {
                        if (column["key"].includes("DFCT_CODE")) {
                            return {
                                colSpan: 2,
                                ...obj
                            }
                        } else if (column["key"].includes("ERRC_DESCR")) {
                            return {
                                colSpan: 0,
                                ...obj
                            }
                        }
                    } else if (rowIndex > 5) {
                        return {
                            ...obj
                        }
                    }
                }
            })
            /*  console.log("输出basecolumns", baseColumns);
            console.log("Data0输出处理的数据", finalData);
            console.log("Data1處理之後的數據是：", povitData); */
            PageSetting.TopColumns = baseColumns;
            PageSetting.moreData = finalData;
            console.log("最終的數據", finalData);
            // if (PageSetting.moreData.length > 20) {
            //     //预先加载40条数据
            //     PageSetting.TopList = PageSetting.moreData.slice(0, 40);
            // }
            // else {
                PageSetting.TopList = PageSetting.moreData;
            // }
            closeFullLoading();

            ReportParamsItem.APIMessage = JSON.stringify(Message);
            ReportParamsItem.FormParams = { ...TopTableParams, ...values };
            useReportCommon.AddObject(ReportNameParams , ReportParamsItem)
        } catch (error) {
            closeFullLoading();
        }

    }
    

}

function getRowClassName_Sel(record: Object) {
    if (record && Object.keys(record).length != 0) {
        for (const [key, value] of Object.entries(record)) {
            if (typeof (value) == "string" && value.includes("品")) {
                return 'Green';
            }else if(typeof (value) == "string" && value.includes("defect ITEM")) {
                return 'v-center';
            }else if(typeof (value) == "string" && value.includes("INSPECTION QTY")) {
                return 'Purple';
            }else if(typeof (value) == "string" && ["GOOD","YIELD Rate"].includes(value)) {
                return 'GrayBlue';
            }else if(typeof (value) == "string" && ["NG","YIELD LOSS"].includes(value)) {
                return 'YellowGray';
            }
           
        }
    }
}

//实现无限滚动或者说懒加载
InfiniteScroll(reportref, PageSetting as any);
onMounted(async () => {
        //await mounted(optionsdata,UseParamsAdd.Fab) 
        //await mountedDefectCode(optionsdata, UseParamsAdd.Fab, 'DefectCodeData');
        //await mountedEQP(optionsdata,UseParamsAdd.Fab, "Quality Report");
    });

</script>

<style lang="less" scoped>
.btn-rows {
    button {
        margin-right: 12px;
    }
}

.swapper {
    display: flex;
    flex-direction: column;
}
</style>
