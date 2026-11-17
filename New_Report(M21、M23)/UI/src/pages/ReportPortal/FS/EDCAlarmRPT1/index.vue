<template>
    <div class="EDC_Report">
        <a-row>
            <a-col :span="24 - PageParams.TableSpan" class="ScrollForm" v-show="PageParams.ShowMenu">
                <SchemaForm @submit="confirm"> </SchemaForm>
            </a-col>
            <a-col :span="PageParams.TableSpan">
                <div  ref="EDC_Report">
                    <DynamicTable
                        row-key="EDC_Report"
                        header-title="EDC Report  "
                        showFavorite
                        sticky
                        :style="{ display: 'inline-block' }"
                        :PageParams="PageParams"
                        :columns="PageSetting.TopColumns"
                        :key="PageSetting.TopColumns"
                        bordered
                        size="small"
                        :data-source="PageSetting.TopList"
                        :pagination="false"
                        :row-class-name="getRowClassNmae"
                        :scroll="{ x: PageSetting.x, y: 650 }"
                        export-file-name="导出"
                    >
                        <template #export-button>
                            <a-button type="success"> 导出 </a-button>
                        </template>
                    </DynamicTable>
                </div>
            </a-col>
        </a-row>
    </div>
</template>

<script lang="tsx" setup>
import { PageParams } from '@/components/core/dynamic-table/src/dynamic-table';
import { useTable } from '@/components/core/dynamic-table';
import { reactive, ref } from '@vue/reactivity';
import { useForm } from '@/components/core/schema-form';
import { GetCommon } from '@/api/pages/common';
import { onMounted } from '@vue/runtime-core';
import { cloneDeep } from 'lodash';
// import { getRowClassNmae } from '@/pages/ReportPortal/utils/common';
import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect';
import { AddCommonPageParams } from '@/store/module/commonpage';

import { getRowClassNmae, megercell } from '@/pages/ReportPortal/utils/common';
// import { megercell, sortArray } from '@/pages/ReportPortal/utils/common';
import dayjs from 'dayjs';
import { message, Popconfirm, TypographyLink } from 'ant-design-vue';
const UseParamsAdd = AddCommonPageParams();
let FactoryType: any = UseParamsAdd.Fab;
const EDC_Report = ref<HTMLElement | null>(null);

function getImport(site: any) {
    const { schemas, optionsdata } = require(`@/pages/ReportPortal/FS/EDCAlarmRPT1/${site}/form-schema`);
    const { baseColumns } = require(`@/pages/ReportPortal/FS/EDCAlarmRPT1/${site}/table`);
    return { schemas, optionsdata, baseColumns };
}
const { schemas, optionsdata, baseColumns } = getImport(UseParamsAdd.Fab);
// 頁面數據
let PageSetting = reactive({
    TopList: [] as any,
    TopColumns: [] as any,
    Message: {},
    x: 300
});
const [DynamicTable] = useTable();

const { mounted, InfiniteScroll, mountedowner, mountedLCDGrade } = useRemoteSelect();
const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas:schemas(FactoryType),
    actionColOptions: {
        span: 24
    },
    fieldMapToTime: [['fieldTime', ['vDate_s', 'vDate_e1'], 'YYYY-MM-DD']],
    layout: 'vertical'
});
let TopTableParams = reactive({
    name: 'Date',
    apiName: 'EDCDataSet', //"EDCDataSet",
    FactoryType: FactoryType,
    G_FAC: 'A',
    FromDate: '',
    FromTime: '',
    ToDate: '',
    ToTime: '',
    EDCLineid: '',
    EDCUnit: '',
    wo_nbr: ''
});

// 添加合并列功能
function TableCustomCell(Columns: Object[], ClickColumn: string[] = [], ToOtherpage: Boolean = false) {
    Columns.forEach((item) => {
        if (item['children']) {
            item['children'].forEach((citem) => {
                if (ClickColumn.includes(citem.dataIndex)) {
                    citem['customCell'] = (record: Object, rowindex: number, column: Object) => {
                        return {
                            onDblclick: async () => {
                                // 新页面打开
                                if (ToOtherpage) {
                                } else {
                                    PageSetting[
                                        'BottomTitle'
                                    ] = `LCMA Wip Distribution -- ${column['__originColumn__']['title']}`;
                                    PageSetting.BottomColumns = [];
                                    PageSetting.BottomList = [];
                                    let BottomTableParams = cloneDeep(TopTableParams);
                                    switch (column['dataIndex']) {
                                        case 'MOVE_QTY':
                                            BottomTableParams['ReportType'] = 'WipDistDetail3';
                                            BottomTableParams['row0'] = record['PROD_NBR'];
                                            BottomTableParams['col1'] = record[citem['dataIndex']];
                                            const {
                                                Date: {
                                                    Data0: { Table: Data }
                                                }
                                            } = await GetCommon(BottomTableParams);
                                            if (Data.length != 0) {
                                                PageSetting.BottomColumns = WipDistDetail3Columns;
                                                PageSetting.BottomList = Data;
                                                PageSetting.BottomList.push(
                                                    getfinalsum(PageSetting.BottomList, ['QTY'], 'EQUIP_NBR')
                                                );
                                            } else {
                                                message.warning('无数据！');
                                            }
                                            break;
                                        case 'RETURN_QTY':
                                            BottomTableParams['ReportType'] = 'WipDistDetail2';
                                            BottomTableParams['row0'] = record['PROD_NBR'];
                                            BottomTableParams['col1'] = record[citem['dataIndex']];
                                            const {
                                                Date: {
                                                    Data0: { Table: detail2Data }
                                                }
                                            } = await GetCommon(BottomTableParams);
                                            if (detail2Data.length != 0) {
                                                PageSetting.BottomColumns = WipDistDetail2Columns;
                                                PageSetting.BottomList = detail2Data;
                                            } else {
                                                message.warning('无数据！');
                                            }

                                            break;
                                        case 'ALL_WIP':
                                        case 'ALL_HOLD_WIP':
                                            BottomTableParams['ReportType'] = 'WipDistDetail1';
                                            BottomTableParams['row0'] = record['PROD_NBR'];
                                            BottomTableParams['col1'] = record[citem['dataIndex']];
                                            const {
                                                Date: {
                                                    Data0: { Table: detail1Data }
                                                }
                                            } = await GetCommon(BottomTableParams);
                                            if (detail1Data.length != 0) {
                                                PageSetting.BottomColumns = WipDistDetail1Columns;
                                                PageSetting.BottomList = detail1Data;
                                                let temp = cloneDeep(PageSetting.BottomList[0]);
                                                for (let i in temp) {
                                                    if (i == 'WORK_CTR') {
                                                        temp[i] = '合計';
                                                    } else if (i == 'HOLD_MARK') {
                                                        temp[i] = PageSetting.BottomList.length;
                                                    } else {
                                                        temp[i] = '';
                                                    }
                                                }
                                                PageSetting.BottomList.push(temp);
                                            } else {
                                                message.warning('无数据！');
                                            }

                                            break;
                                        case 'RWK_WIP':
                                        default:
                                            break;
                                    }
                                }
                            },
                            style: { cursor: 'pointer' }
                        };
                    };
                } else {
                    return {
                        onDblclick: () => {
                            message.warning('此栏位没有二阶数据');
                        },
                        style: {}
                    };
                }
            });
        }
    });
}

async function confirm() {
    //判斷是否選著線別及單元
    let tempTopTableParams = cloneDeep(dynamicFormRef.formModel);
    if (
        Object.values(tempTopTableParams)[4] == 'ALL' ||
        Object.values(tempTopTableParams)[5] == 'ALL' ||
        Object.values(tempTopTableParams)[4] == '' ||
        Object.values(tempTopTableParams)[5] == ''
    ) {
        message.success('請選擇線別及單元');
        //return;
    } else {
        PageSetting.TopList = [];
        PageSetting.TopColumns = [];
        let tempkeys = Object.keys(TopTableParams);
        let tempdynamicFormRef = cloneDeep(dynamicFormRef.formModel);
        for (let i in tempdynamicFormRef) {
            if (tempkeys.includes(i)) {
                if (['FromTime', 'ToTime'].includes(i)) {
                    TopTableParams[i] = ` ${dayjs(tempdynamicFormRef[i]['$d']).format('HH:mm')}`;
                } else {
                    TopTableParams[i] = tempdynamicFormRef[i];
                }
            }
        }

        let {
            Date: {
                Data: { Table: Data }
            }
        } = await GetCommon(TopTableParams);
        if (Data.length != 0) {
            let tempaops = '';

            Data.forEach((item) => {
                tempaops = item['LINE_ID'];
            });

            let tempColumns = cloneDeep(baseColumns);
            PageSetting.TopList = Data;
            PageSetting.TopColumns = baseColumns(tempaops);

            PageSetting.TopColumns.forEach((item, index) => {
                if (index == 0) {
                    item.children.forEach((citem) => {
                        if (['UNIT_ID', 'ITEM_TYPE'].includes(citem.dataIndex)) {
                            megercell(item.children, ['UNIT_ID', 'ITEM_TYPE'], PageSetting.TopList);
                        }
                    });
                }
            });
            console.log('PageSetting:TopColumns', tempColumns);
            console.log('PageSetting.Data', Data);
        } else {
            //closeFullLoading();
            message.warn('无数据！');
        }
    }
}
InfiniteScroll(EDC_Report, PageSetting as any);
onMounted(async () => {
    await mounted(optionsdata, FactoryType);
    await mountedowner(optionsdata, FactoryType);
    await mountedLCDGrade(optionsdata, FactoryType);
});
</script>

<style lang="less" scoped>
.btn-rows {
    button {
        margin-right: 12px;
    }
}

.EDC_Report :deep(.sub-total) {
    background-color: #ffb468 !important;
}

.EDC_Report :deep(.total) {
    background-color: #ff80c0 !important;
}

.swapper {
    display: flex;
    flex-direction: column;
}
</style>
