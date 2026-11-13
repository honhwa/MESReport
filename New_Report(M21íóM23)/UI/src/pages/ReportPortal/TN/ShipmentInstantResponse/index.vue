<template>
    <div class="ShipmentInstantResponse">
        <Loading
            :loading="compState.loading"
            :absolute="compState.absolute"
            :theme="compState.theme"
            :background="compState.background"
            :tip="compState.tip"
        />
        <a-card>
            <a-row>
                <a-col :span="PageSetting.TableSpan" class="ScrollTable">
                    <DynamicTable
                        row-key="id"
                        :header-title="PageSetting.headerTitle"
                        :columns="PageSetting.Columns"
                        :key="PageSetting.Columns"
                        bordered
                        size="small"
                        :data-source="PageSetting.List"
                        :pagination="false"
                        :scroll="PageSetting.TableSize"
                        export-file-name="Export"
                        :row-class-name="getRowClassNmae"
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

<script lang="ts" setup>
import { useTable } from '@/components/core/dynamic-table';
import { reactive } from '@vue/reactivity';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { Loading, useLoading } from '@/components/feat/Loading';
import { GetCommon } from '@/api/pages/common';
import { onMounted } from 'vue';

const UseFormParamsAdd = AddCommonPageParams();

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

function getImport(site: any) {
    const { aheadColumns, rearColumns } = require(`@/pages/ReportPortal/TN/ShipmentInstantResponse/${site}/Table`);
    return { aheadColumns, rearColumns };
}
const { aheadColumns, rearColumns } = getImport('MOD');

let PageSetting = reactive({
    List: [] as any,
    TableSpan: 23,
    showmenu: true,
    Columns: [] as any,
    TableSize: { x: 1000, y: -1 }, //y = -1 => 配合畫面
    headerTitle: '出貨即時反應系統'
});
PageSetting.Columns = [...aheadColumns, ...rearColumns];
const [DynamicTable] = useTable();

let runTime = 0;
const getData = async () => {
    if (runTime == 0) {
        runTime = 1200;
        openFullLoading();
        let baseParams = { name: 'Date', apiName: 'TN_ShipmentInstantResponse', FactoryType: UseFormParamsAdd.Fab };

        const {
            Date: {
                Data: { Table: Data },
                Sql: Sql
            }
        } = await GetCommon({ ...baseParams });
        let dtColumns = new Array();
        for (let i = 0; i <= 5; i++) {
            let date = new Date();
            date.setDate(date.getDate() + i);
            let dt = date.toISOString().split('T')[0];
            let dtTitle = ('0' + (date.getMonth() + 1)).slice(-2) + '/' + ('0' + date.getDate()).slice(-2);
            let week = '日一二三四五六'.charAt(date.getDay());
            dtColumns.push({
                title: dtTitle,
                align: 'center',
                dataIndex: dt,
                className: 'v-center thOrange',
                children: [
                    {
                        title: week,
                        align: 'center',
                        dataIndex: dt,
                        className: 'v-center',
                        width: 50,
                        customCell: (record: Object, rowindex: number, column: Object) => {
                            let strToday = new Date().toISOString().split('T')[0];
                            let toDate = new Date(strToday);
                            let lastDate = new Date(column['dataIndex']);
                            let diffDays = (lastDate.getTime() - toDate.getTime()) / (1000 * 60 * 60 * 24);
                            let stock = record['STOCK_QTY'];
                            for (let diff = 0; diff <= diffDays; diff++) {
                                let diffDate = new Date();
                                diffDate.setDate(toDate.getDate() + diff);
                                let key = diffDate.toISOString().split('T')[0];
                                stock = stock - record[key];
                            }
                            let rs: any = {};
                            if (record[column['dataIndex']] != null && stock >= 0) {
                                rs.style = {
                                    'background-color': '#0f0'
                                };
                            }
                            return rs;
                        }
                    }
                ]
            });
        }
        PageSetting.Columns = [...aheadColumns, ...dtColumns, ...rearColumns]; //vue3或者adv无法监测push,shift等方法
        PageSetting.List = Data;
        closeFullLoading();
    } else {
        runTime = runTime - 1;
    }
    let time = new Date(runTime * 1000).toISOString().slice(14, 19);
    PageSetting.headerTitle = '出貨即時反應系統' + (runTime ? '(' + time + ')' : '');

    setTimeout(getData, 1000);
};

onMounted(() => {
    getData();
});

function getRowClassNmae(record: Object) {
    if (record && Object.keys(record).length != 0) {
        for (const [key, value] of Object.entries(record)) {
            if (key == 'DIFF_QTY') {
                return value > 0 ? 'diff-minus' : 'diff-positive';
            }
        }
    }
    return '';
}
</script>

<style lang="less" scoped>
.btn-rows {
    button {
        margin-right: 12px;
    }
}

.ShipmentInstantResponse :deep(.v-center) {
    border: 1px solid black;
}

.ShipmentInstantResponse :deep(th.thYellow) {
    background-color: #ff7;
}

.ShipmentInstantResponse :deep(th.thYellowGreen) {
    background-color: #cf9;
}
.ShipmentInstantResponse :deep(th.thRedColor) {
    color: #ff0000;
}

.ShipmentInstantResponse :deep(th.thOrange) {
    background-color: #fda;
}
</style>
