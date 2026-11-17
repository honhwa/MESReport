<template>
    <div class="Top5DefectCode">
        <Loading
            :loading="compState.loading"
            :absolute="compState.absolute"
            :theme="compState.theme"
            :background="compState.background"
            :tip="compState.tip"
        />
        <a-card>
            <a-row>
                <a-col :span="24 - PageSetting.TableSpan" class="ScrollForm" Source-show="PageSetting.ShowMenu" v-show="PageParams.ShowMenu">
                    <div class="swapper">
                        <SchemaForm @submit="confirm"> </SchemaForm>
                    </div>
                </a-col>
                <a-col :span="PageSetting.TableSpan">
                    <div class="Top5DefectCodeTable" ref="Top5DefectCodeTable">
                        <DynamicTable
                            showFavorite
                            row-key="id"
                            header-title="Top5 Defect Code"
                            :columns="PageSetting.Columns"
                            :key="PageSetting.Columns"
                            bordered
                            size="small"
                            :data-source="PageSetting.List"
                            :data-all="PageSetting.moreData"
                            :pagination="false"
                            export-file-name="Export"
                            :row-class-name="getRowClassNmae"
                            sticky
                            :scroll="PageSetting.TableSize"
                            :show-header="false"
                            :PageParams="PageParams"
                        >
                            <template #export-button>
                                <a-button type="success"> Export </a-button>
                            </template>
                        </DynamicTable>
                    </div>
                </a-col>
                <div hidden>{{ PageSetting.SQLHidden }}</div>
            </a-row>
        </a-card>
    </div>
</template>

<script lang="tsx" setup>
import {PageParams} from '@/components/core/dynamic-table/src/dynamic-table'
import { useForm } from '@/components/core/schema-form';
import { useTable } from '@/components/core/dynamic-table';
import { reactive, unref, ref, onMounted } from 'vue';
import { GetCommon } from '@/api/pages/common';
import useRemoteSelect from '@/pages/ReportPortal/utils/TNuseRemoteSelect';
import { mergeTableRow } from '@/pages/ReportPortal/utils/common';
import { ReportCommon } from '@/store/module/ReportCommon';
import { Loading, useLoading } from '@/components/feat/Loading';

const UseParamsAdd = ReportCommon();
let fab = "MOD";

if(UseParamsAdd.Fab == "MOD1")
{
    fab = UseParamsAdd.Fab;
}
else if(UseParamsAdd.Fab == "MOD2" || UseParamsAdd.Fab == "MOD4")
{
    fab = "MOD24";
}

const Top5DefectCodeTable = ref<HTMLElement | null>(null);
const { mounted, InfiniteScroll } = useRemoteSelect();
function getImport(site: any) {
    const { schemas } = require(`@/pages/ReportPortal/TN/Top5DefectCode/${site}/form-schema`);
    const { aheadColumns, rearColumns, getColumns } = require(`@/pages/ReportPortal/TN/Top5DefectCode/MOD/Table`);
    return { schemas, aheadColumns, rearColumns, getColumns };
}
const compState = reactive({
    absolute: true,
    loading: false,
    theme: 'dark',
    background: 'rgba(111,111,111,.7)',
    tip: '加载中...'
});
const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});

let { schemas, aheadColumns, rearColumns, getColumns } = getImport(fab);

let PageSetting = reactive({
    List: [] as any,
    TableSpan: 20,
    ShowMenu: true,
    APIColumns: [] as any,
    Columns: [] as any,
    moreData: [] as any,
    SQLHidden: '',
    TableSize: { x: 1000 }
});
let Top5WorkCtr: any = [];

const [DynamicTable] = useTable();

const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas:schemas(UseParamsAdd.Fab),
    actionColOptions: {
        span: 24
    },
    layout: 'vertical'
});
/**
 * 提交表单,验证通过后进行数据获取与处理
 * @dynamicFormRef 表单的所有参数与属性
 * @ParamsData API接收的参数
 */
function confirm(values) {
    openFullLoading();
    let baseParams = { name: 'Date', apiName: 'TN_Top5DefectCodeDataSet', FactoryType: UseParamsAdd.Fab };
    Object.keys(values).forEach((key) => {
        if (!values[key]) {
            values[key] = '';
        }
    });

    dynamicFormRef.validate().then(async () => {
        //呼叫 api
        const {
            Date: {
                Data: { Table: Data },
                Data_Oper: { Table: Data_Oper },
                SQLFilter: SQLFilter
            }
        } = await GetCommon({ ...baseParams, ...values });

        UseParamsAdd.AddFormParams('Top5DefectCode' + UseParamsAdd.Fab, { ...values });

        //新增橫軸欄位資訊
        Data_Oper.map((item: string, index: number) => {
            Top5WorkCtr[Data_Oper[index]['WORK_CTR']] = Data_Oper[index]['WORK_NAME']
        })

        //新增欄位
        let operationList = Object.keys(groupBy(Data, { key: ['WORK_CTR'] }));
        let operColumns: any = getColumns(operationList, Top5WorkCtr);
        let DateColumns: any = [...aheadColumns, ...operColumns, ...rearColumns];

        

        //資料處理
        let newData: any = [];
        let groupOption = { key: ['PROD', 'WORK_CTR'], sum: ['QTY'] };
        let groupData = groupBy(Data, groupOption);
        Object.keys(groupData).forEach((prod) => {
            let operHeader = { RANK: prod, ISHEAD: true, TOTAL: 'Total', CLASSNAME: 'YellowGreen' };
            operColumns.forEach((x) => {
                operHeader[x.dataIndex] = x.title;
            });
            //各 Oper 統計資料(空資料)
            newData.push(operHeader);
            let newData1 = { PROD: prod, RANK: prod, ISHEAD: true, CLASSNAME: 'Yellow' };
            let newData2 = { PROD: prod, RANK: prod, ISHEAD: true, CLASSNAME: 'Yellow' };
            let newData3 = { PROD: prod, RANK: prod, ISHEAD: true, DBCLICK: true, CLASSNAME: 'Yellow' };
            let newDataDetail: any = [];
            newData.push(newData1);
            newData.push(newData2);
            newData.push(newData3);

            //前5名詳細資料(空資料)
            for (let i = 1; i <= 5; i++) {
                newDataDetail.push({ RANK: '-', ISHEAD: false });
            }
            newData.push(...newDataDetail);

            //填入資料
            let totaSum = 0;
            let totaOUT_QTY = 0;
            let totalYield = 1;
            let allErr: number[] = [];
            Object.keys(groupData[prod]).forEach((work_ctr) => {
                //填入統計
                let SUM = groupData[prod][work_ctr].sum.QTY;
                let OUT_QTY = groupData[prod][work_ctr][0].OUT_QTY;
                let Yield = (OUT_QTY - SUM) / OUT_QTY;
                newData1[work_ctr] = groupData[prod][work_ctr][0].OUT_QTY;
                //newData2[work_ctr] = ((OUT_QTY - SUM) * 100 / OUT_QTY).toFixed(3) + '%';
                //newData3[work_ctr] = ((SUM * 100) / OUT_QTY).toFixed(3) + '%(' + SUM + ')';
                newData2[work_ctr] = (Yield * 100).toFixed(3) + '%';
                newData3[work_ctr] = ((1 - Yield) * 100).toFixed(3) + '%(' + SUM + ')';

                //填入詳細資料
                groupData[prod][work_ctr].forEach((item, idx) => {
                    let errc_descr = (item.ERRC_DESCR || '').trim();
                    let qty = item['QTY'];
                    let OutYield = ((qty * 100) / OUT_QTY).toFixed(3);
                    if (idx < 5) {
                        newDataDetail[idx].PROD = prod;
                        newDataDetail[idx].RANK = idx + 1;
                        newDataDetail[idx][work_ctr] = errc_descr;
                        newDataDetail[idx][work_ctr + '-DFCT_CODE'] = OutYield + '%(' + qty + ')';
                    }
                    //詳細資料統計
                    allErr[errc_descr] = allErr[errc_descr] || 0;
                    allErr[errc_descr] += qty;
                });

                //計算加總
                totaSum += SUM;
                totaOUT_QTY += OUT_QTY;
                totalYield *= Yield;
            });
            //填入右方的總計
            //統計部分
            if (totaOUT_QTY) {
                // newData1['TOTAL'] = totaOUT_QTY;
                // newData2['TOTAL'] = ((totaOUT_QTY - totaSum) * 100 / totaOUT_QTY).toFixed(3) + '%';
                // newData3['TOTAL'] = ((totaSum * 100) / totaOUT_QTY).toFixed(3) + '%(' + totaSum + ')';
                newData1['TOTAL'] = totaOUT_QTY;
                newData2['TOTAL'] = (totalYield * 100).toFixed(3) + '%';
                newData3['TOTAL'] = ((1 - totalYield) * 100).toFixed(3) + '%(' + totaSum + ')';
            }
            //詳細資料部分
            let errList = Object.keys(allErr).map((err) => {
                return { name: err, qty: allErr[err] };
            });
            errList = errList.sort((a: any, b: any) => {
                return b.qty - a.qty;
            });
            let totalIdx = 0;
            for (let i = 0; i < errList.length && i < 5; i++) {
                let errc_descr = errList[i]['name'] || '';
                let qty = errList[i]['qty'] || 0;
                if (errc_descr) {
                    let OutYield = (((qty) / totaSum)*((totaSum * 100) / totaOUT_QTY)).toFixed(3);
                    newDataDetail[totalIdx]['TOTAL'] = errc_descr;
                    newDataDetail[totalIdx]['TOTAL-DFCT_CODE'] = OutYield + '%(' + qty + ')';
                    totalIdx++;
                }
            }
        });

        PageSetting.TableSize.x = 650 + DateColumns.length * 100;
        PageSetting.Columns = [...DateColumns]; //vue3或者adv无法监测push,shift等方法
        PageSetting.moreData = getTableData(newData);

        if (PageSetting.moreData.length > 100) {
            PageSetting.List = PageSetting.moreData.slice(0, 40);
        } //预先加载80条数据
        else {
            PageSetting.List = PageSetting.moreData;
        }
        closeFullLoading();
    });
}

/**
 * 进行数据处理,将表格进行行列转置并进行表格合并
 * @Data 表格数据源
 */
function getTableData(Data: any[]) {
    /**表格合并所需参数
     * @data 数据源
     * @mergeColNames 需要合并的列
     * @firstMergeColNames 受到影响的列
     * @firstMerge 以哪列为基础进行合并
     */
    let combineCol: any = {
        data: Data,
        mergeColNames: ['RANK'],
        firstMergeColNames: ['RANK'],
        firstMerge: 'RANK'
    };
    return mergeTableRow(combineCol);
}
function getRowClassNmae(record: Object) {
    if (record && Object.keys(record).length != 0) {
        if (record['CLASSNAME']) {
            return record['CLASSNAME'];
        }
    }
}
var groupBy = (data, option) => {
    let deep: any = [];
    let rs = deep;
    data.forEach((item) => {
        option.key.forEach((k) => {
            rs[item[k]] = rs[item[k]] || [];
            rs = rs[item[k]];
        });
        rs.push(item);
        if (option.sum) {
            let sum: number[] = [];
            option.sum.forEach((k) => {
                rs.sum = rs.sum || [];
                rs.sum[k] = rs.sum[k] || 0;
                rs.sum[k] += item[k];
            });
        }
        if (option.avg) {
            option.avg.forEach((k) => {
                rs.avg = rs.avg || [];
                rs.avg[k] = rs.avg[k] || 0;
                if (rs.length != 0) {
                    rs.avg[k] = (rs.avg[k] + item[k]) / rs.length;
                }
            });
        }
        rs = deep;
    });
    return deep;
};

InfiniteScroll(Top5DefectCodeTable, PageSetting);

onMounted(async () => {
    // await mounted(optionsdata, UseParamsAdd.Fab);
    // console.log(optionsdata);
});
</script>
<style lang="less" scoped>
.btn-rows {
    button {
        margin-right: 12px;
    }
}

.Top5DefectCode :deep(.Source-center) {
    background-color: #f0f0f0;
}

:deep(.pro_auto-center) {
    border: 1px solid black;
}

.Top5DefectCode :deep(.YellowGreen) {
    background-color: #80ff80;
}

:deep(.sum-center) {
    border: 1px solid black;
}

.Top5DefectCodeTable {
    height: 700px;
    overflow-y: auto;
}

.Top5DefectCode :deep(.Yellow) {
    background-color: #fbf991;
}

.Top5DefectCode :deep(.Blue) {
    background-color: #80ffff;
}

:deep(.v-center) {
    border: 1px solid black;
}

.swapper {
    display: flex;
    flex-direction: column;
}
</style>
