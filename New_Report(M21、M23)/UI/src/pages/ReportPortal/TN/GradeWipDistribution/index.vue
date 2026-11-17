<template>
    <div class="GradeWipDistribution">
        <Loading
            :loading="compState.loading"
            :absolute="compState.absolute"
            :theme="compState.theme"
            :background="compState.background"
            :tip="compState.tip"
        />
        <a-card>
            <!-- <a-row>
                <span class="menu-fold" @click="showHidden(PageSetting)">
                    <component :is="!PageSetting.showmenu ? MenuUnfoldOutlined : MenuFoldOutlined" />
                </span>
            </a-row> -->
            <a-row>
                <a-col :span="24 - PageSetting.TableSpan" class="ScrollForm" v-show="PageParams.ShowMenu" Source-show="PageSetting.showmenu">
                    <div class="swapper">
                        <SchemaForm @submit="confirm"> </SchemaForm>
                    </div>
                </a-col>
                <a-col :span="PageSetting.TableSpan">
                    <div class="GradeWipDistributionTable" ref="GradeWipDistributionTable">
                        <DynamicTable
                            row-key="id"
                            header-title="Grade Wip Distribution"
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
import { MenuFoldOutlined, MenuUnfoldOutlined } from '@ant-design/icons-vue';
// import { schemas } from './form-schema';
import { PageParams } from '@/components/core/dynamic-table/src/dynamic-table';
import { useForm } from '@/components/core/schema-form';
import { useTable } from '@/components/core/dynamic-table';
import { reactive, unref, ref, onMounted } from 'vue';
import { GetGradeWipDistribution } from '@/api/pages/gradewipdistribution';
import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect';
import { getRowClassNmae, mergeTableRow } from '@/pages/ReportPortal/utils/common';
import { ReportCommon } from '@/store/module/ReportCommon';
import { Loading, useLoading } from '@/components/feat/Loading';
import router from '@/router';
//import {optionsdata} from '@/pages/ReportPortal/Static/TNCommon-Form-schema';

const UseParamsAdd = ReportCommon();
const GradeWipDistributionTable = ref<HTMLElement | null>(null);
//vue3的mixin
const { mounted, InfiniteScroll, mountedowner, mountedOper } = useRemoteSelect();
function getImport(site: any) {
    const { schemas } = require(`@/pages/ReportPortal/TN/GradeWipDistribution/${site}/form-schema`);
    const { baseColumns } = require(`@/pages/ReportPortal/TN/GradeWipDistribution/${site}/Table`);
    return { schemas, baseColumns };
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

let { schemas, baseColumns } = getImport('MOD');

let PageSetting = reactive({
    List: [] as any,
    TableSpan: 20,
    showmenu: true,
    APIColumns: [] as any,
    Columns: [] as any,
    moreData: [] as any,
    SQLHidden: ''
});

let FactoryType: any = UseParamsAdd.Fab;
const [DynamicTable] = useTable();

const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas: schemas(FactoryType),
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
    let baseParams = { name: 'Date', apiName: 'TN_GradeWipDistribution', FactoryType: UseParamsAdd.Fab };
    Object.keys(values).forEach((key) => {
        if (!values[key]) {
            values[key] = '';
        }
    });

    dynamicFormRef.validate().then(async () => {
        let now = new Date();
        let nowStr = now.toISOString().split('T')[0];
        console.log(values['Date']);
        console.log(nowStr);
        const {
            Date: {
                Data: { Table: Data },
                sql: Message,
                Hold_N: Hold_N,
                Hold_Y: Hold_Y
            }
        } = await GetGradeWipDistribution({ ...baseParams, ...values });
        let DateColumns: any = [...baseColumns];

        DateColumns[0]['customCell'] = (record: Object, rowindex: number, column: Object) => {
            if (values['Date'] == nowStr) {
                return {
                    onDblclick: () => {
                        UseParamsAdd.AddFirstTableParams('GradeWipDistribution' + UseParamsAdd.Fab, {
                            ...record,
                            ...column['__originColumn__']
                        });
                        router.push({ name: 'GradeWipDistributionDetail_' + UseParamsAdd.Fab });
                    }
                };
            }
        };
        DateColumns[1]['customCell'] = (record: Object, rowindex: number, column: Object) => {
            if (values['Date'] == nowStr) {
                return {
                    onDblclick: () => {
                        UseParamsAdd.AddFirstTableParams('GradeWipDistribution' + UseParamsAdd.Fab, {
                            ...record,
                            ...column['__originColumn__']
                        });
                        router.push({ name: 'GradeWipDistributionDetail_' + UseParamsAdd.Fab });
                    }
                };
            }
        };
        DateColumns[4]['customCell'] = (record: Object, rowindex: number, column: Object) => {
            if (values['Date'] == nowStr) {
                return {
                    onDblclick: () => {
                        UseParamsAdd.AddFirstTableParams('GradeWipDistribution' + UseParamsAdd.Fab, {
                            ...record,
                            ...column['__originColumn__']
                        });
                        router.push({ name: 'GradeWipDistributionDetail_' + UseParamsAdd.Fab });
                    }
                };
            }
        };

        DateColumns[2]['children'] = [];
        DateColumns[3]['children'] = [];
        Hold_N.map((item: any) => {
            DateColumns[2]['children'].push({
                title: item,
                align: 'center',
                dataIndex: item + '_N',
                className: 'pro_auto-center',
                customCell: (record: Object, rowindex: number, column: Object) => {
                    if (values['Date'] == nowStr) {
                        return {
                            onDblclick: () => {
                                UseParamsAdd.AddFirstTableParams('GradeWipDistribution' + UseParamsAdd.Fab, {
                                    ...record,
                                    ...column['__originColumn__']
                                });
                                router.push({ name: 'GradeWipDistributionDetail_' + UseParamsAdd.Fab });
                            }
                        };
                    }
                }
            });
        });
        DateColumns[2]['children'].push({
            title: '小計',
            align: 'center',
            dataIndex: '小計_N',
            className: 'pro_auto-center Blue',
            customCell: (record: Object, rowindex: number, column: Object) => {
                if (values['Date'] == nowStr) {
                    return {
                        onDblclick: () => {
                            UseParamsAdd.AddFirstTableParams('GradeWipDistribution' + UseParamsAdd.Fab, {
                                ...record,
                                ...column['__originColumn__']
                            });
                            router.push({ name: 'GradeWipDistributionDetail_' + UseParamsAdd.Fab });
                        }
                    };
                }
            }
        });

        Hold_Y.map((item: any) => {
            DateColumns[3]['children'].push({
                title: item,
                align: 'center',
                dataIndex: item + '_Y',
                className: 'pro_auto-center',
                customCell: (record: Object, rowindex: number, column: Object) => {
                    if (values['Date'] == nowStr) {
                        return {
                            onDblclick: () => {
                                UseParamsAdd.AddFirstTableParams('GradeWipDistribution' + UseParamsAdd.Fab, {
                                    ...record,
                                    ...column['__originColumn__']
                                });
                                router.push({ name: 'GradeWipDistributionDetail_' + UseParamsAdd.Fab });
                            }
                        };
                    }
                }
            });
        });
        DateColumns[3]['children'].push({
            title: '小計',
            align: 'center',
            dataIndex: '小計_Y',
            className: 'pro_auto-center Yellow',
            customCell: (record: Object, rowindex: number, column: Object) => {
                if (values['Date'] == nowStr) {
                    return {
                        onDblclick: () => {
                            UseParamsAdd.AddFirstTableParams('GradeWipDistribution' + UseParamsAdd.Fab, {
                                ...record,
                                ...column['__originColumn__']
                            });
                            router.push({ name: 'GradeWipDistributionDetail_' + UseParamsAdd.Fab });
                        }
                    };
                }
            }
        });

        debugger;
        PageSetting.Columns = [...DateColumns]; //vue3或者adv无法监测push,shift等方法

        PageSetting.moreData = getTableData(Data);
        if (PageSetting.moreData.length > 100) {
            PageSetting.List = PageSetting.moreData.slice(0, 40);
        } //预先加载40条数据
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
    let combineCol1: any = {
        data: Data,
        mergeColNames: ['PROD_NBR'],
        firstMergeColNames: ['PROD_NBR'],
        firstMerge: 'PROD_NBR'
    };
    let combineCol2: any = {
        data: mergeTableRow(combineCol1),
        mergeColNames: ['LCM_GRADE'],
        firstMergeColNames: ['LCM_GRADE'],
        firstMerge: 'LCM_GRADE'
    };
    return mergeTableRow(combineCol2);
}

InfiniteScroll(GradeWipDistributionTable, PageSetting);

onMounted(async () => {
    //await mountedowner(optionsdata, UseParamsAdd.Fab);
    //await mountedOper(optionsdata, UseParamsAdd.Fab);
    //await mounted(optionsdata, UseParamsAdd.Fab);
    //console.log(optionsdata);
});
</script>
<style lang="less" scoped>
.btn-rows {
    button {
        margin-right: 12px;
    }
}

.GradeWipDistribution :deep(.Source-center) {
    background-color: #f0f0f0;
}

:deep(.pro_auto-center) {
    border: 1px solid black;
}

.GradeWipDistribution :deep(.Yellow) {
    background-color: #80ff80;
}

:deep(.sum-center) {
    border: 1px solid black;
}

.GradeWipDistributionTable {
    height: 700px;
    overflow-y: auto;
}

.GradeWipDistribution :deep(.YellowGreen) {
    background-color: #f39230;
}

.GradeWipDistribution :deep(.Blue) {
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
