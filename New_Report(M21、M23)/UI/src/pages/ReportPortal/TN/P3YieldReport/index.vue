<template>
    <div class="P3YieldReport">
        <Loading
            :loading="compState.loading"
            :absolute="compState.absolute"
            :theme="compState.theme"
            :background="compState.background"
            :tip="compState.tip"
        />
        <a-card>
            <a-row>
                <a-col :span="24 - PageSetting.TableSpan" class="ScrollForm" v-show="PageParams.ShowMenu" Source-show="PageSetting.ShowMenu">
                    <div class="swapper">
                        <SchemaForm @submit="confirm"> </SchemaForm>
                    </div>
                </a-col>
                <a-col :span="PageSetting.TableSpan">
                    <div class="P3YieldReportTable" ref="P3YieldReportTable">
                        <DynamicTable
                            row-key="id"
                            header-title="P3 良率報表"
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
import { PageParams } from '@/components/core/dynamic-table/src/dynamic-table';
import { useForm } from '@/components/core/schema-form';
import { useTable } from '@/components/core/dynamic-table';
import { reactive, unref, ref, onMounted } from 'vue';
import { GetCommon } from '@/api/pages/common';
import useRemoteSelect from '@/pages/ReportPortal/utils/TNuseRemoteSelect';
import { mergeTableRow } from '@/pages/ReportPortal/utils/common';
import { ReportCommon } from '@/store/module/ReportCommon';
import { Loading, useLoading } from '@/components/feat/Loading';

const UseParamsAdd = ReportCommon();
const P3YieldReportTable = ref<HTMLElement | null>(null);
const { mountedTNEquipment, mountedTNSize, mountedLCMOwner, mountedOper, InfiniteScroll } = useRemoteSelect();
function getImport(site: any) {
    const { schemas, optionsdata } = require(`@/pages/ReportPortal/TN/P3YieldReport/${site}/form-schema`);
    const { aheadColumns, rearColumns, getColumns } = require(`@/pages/ReportPortal/TN/P3YieldReport/${site}/Table`);
    return { schemas, optionsdata, aheadColumns, rearColumns, getColumns };
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

let { schemas, optionsdata, aheadColumns, rearColumns, getColumns } = getImport('MOD');

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

const [DynamicTable] = useTable();

const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas,
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
    let baseParams = { name: 'Date', apiName: 'TN_P3YieldReport', FactoryType: UseParamsAdd.Fab };
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
                sql: sql
            }
        } = await GetCommon({ ...baseParams, ...values });
        UseParamsAdd.AddFormParams('P3YieldReport' + UseParamsAdd.Fab, { ...values });

        //新增欄位
        let operationList = Object.keys(groupBy(Data, { key: ['WORK_CTR'] }));
        let operColumns: any = getColumns(operationList);
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

            //前10名詳細資料(空資料)
            for (let i = 1; i <= 10; i++) {
                newDataDetail.push({ RANK: '-', ISHEAD: false });
            }
            newData.push(...newDataDetail);

            //填入資料
            let totalSum = 0;
            let totalOUT_QTY = 0;
            let totalYield = 1;
            let allErr: number[] = [];
            Object.keys(groupData[prod]).forEach((work_ctr) => {
                //填入統計
                let SUM = groupData[prod][work_ctr].sum.QTY;
                let OUT_QTY = groupData[prod][work_ctr][0].OUT_QTY;
                let Yield = (OUT_QTY - SUM) / OUT_QTY;
                newData1[work_ctr] = groupData[prod][work_ctr][0].OUT_QTY;
                newData2[work_ctr] = (Yield * 100).toFixed(3) + '%';
                newData3[work_ctr] = ((1 - Yield) * 100).toFixed(3) + '%(' + SUM + ')';

                //填入詳細資料
                groupData[prod][work_ctr].forEach((item, idx) => {
                    let errc_descr = (item.ERRC_DESCR || '').trim();
                    let qty = item['QTY'];
                    let OutYield = ((qty * 100) / OUT_QTY).toFixed(3);
                    if (idx < 10) {
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
                totalSum += SUM;
                totalOUT_QTY += OUT_QTY;
                totalYield *= Yield;
            });
            //填入右方的總計
            //統計部分
            if (totalOUT_QTY) {
                newData1['TOTAL'] = totalOUT_QTY;
                newData2['TOTAL'] = (totalYield * 100).toFixed(3) + '%';
                newData3['TOTAL'] = ((1 - totalYield) * 100).toFixed(3) + '%(' + totalSum + ')';
            }
            //詳細資料部分
            let errList = Object.keys(allErr).map((err) => {
                return { name: err, qty: allErr[err] };
            });
            errList = errList.sort((a: any, b: any) => {
                return b.qty - a.qty;
            });
            let totalIdx = 0;
            for (let i = 0; i < errList.length && i < 10; i++) {
                let errc_descr = errList[i]['name'] || '';
                let qty = errList[i]['qty'] || 0;
                if (errc_descr) {
                    let OutYield = ((qty / totalSum) * ((totalSum * 100) / totalOUT_QTY)).toFixed(3);
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
            PageSetting.List = PageSetting.moreData.slice(0, 80);
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
const groupBy = (data, option) => {
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

InfiniteScroll(P3YieldReportTable, PageSetting);

onMounted(async () => {
    await mountedTNEquipment(optionsdata, UseParamsAdd.Fab);
    await mountedTNSize(optionsdata, UseParamsAdd.Fab);
    await mountedLCMOwner(optionsdata, UseParamsAdd.Fab);
    await mountedOper(optionsdata, UseParamsAdd.Fab);
    console.log(optionsdata);
});
</script>
<style lang="less" scoped>
.btn-rows {
    button {
        margin-right: 12px;
    }
}

.P3YieldReport :deep(.Source-center) {
    background-color: #f0f0f0;
}

:deep(.pro_auto-center) {
    border: 1px solid black;
}

.P3YieldReport :deep(.YellowGreen) {
    background-color: #80ff80;
}

:deep(.sum-center) {
    border: 1px solid black;
}

.P3YieldReportTable {
    height: 700px;
    overflow-y: auto;
}

.P3YieldReport :deep(.Yellow) {
    background-color: #fbf991;
}

.P3YieldReport :deep(.Blue) {
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
