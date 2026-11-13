<script lang="ts" setup>
import { useTable } from '@/components/core/dynamic-table';
import { reactive, watchEffect, watch, computed } from 'vue';
import { GetCommon } from '@/api/pages/common';
import { ReportCommon } from '@/store/module/ReportCommon';
import { getRowClassNmae } from '@/pages/ReportPortal/utils/common';
import { StartBankDetail3Table1Columns } from './LCM1/table';
import { getfinalsum } from '@/utils/reportcommon/Calculation';

let useCommonParams = ReportCommon();
let FactoryType = useCommonParams.Fab;
let TableParams = useCommonParams['StartBankQTYReport' + FactoryType]['FirstTableParams'];
let FormParams = useCommonParams['StartBankQTYReport' + FactoryType]['FormParams'];
let ApiParams = useCommonParams['StartBankQTYReport' + FactoryType]['APIMessage'];
let PageSetting = reactive({
    moreData: [],
    List: [] as any,
    Columns: [] as any,
    TopTitle: "LCMA Operation Performance Pass Q'ty"
});
const [DynamicTable] = useTable();

async function GetDetailData(TableParams: any) {
    try {
        PageSetting.Columns = StartBankDetail3Table1Columns;
        let params = {
            ReportType: 'Detail3',
            workctr: '1100',
            lcm_prod: TableParams['PROD_NBR'],
            FactoryType: FactoryType,
            G_FAC: 'A',
            apiName: 'StartBankPlanQualityDetail',
            ToDate: FormParams['ToDate']
        };
        let { api01 } = await GetCommon(params);
        let Data = api01['Data']['Table'];
        Data.forEach((item) => {
            item['GOOD_QTY'] = parseInt(item['PASS_QTY']) - parseInt(item['RWK_QTY']) ?? 0;
        });
        let tempsum = getfinalsum(Data, ['PASS_QTY', 'GOOD_QTY', 'RWK_QTY'], 'PROD_NBR');
        Data.push(tempsum);
        if (Data.length > 0) PageSetting.List = Data;
    } catch (error) {
        console.error(error);
    }
}

watchEffect(() => {
    //一阶表格监控
    TableParams = useCommonParams['StartBankQTYReport' + FactoryType]['FirstTableParams'];
    // FormParams = useCommonParams["StartBankQTYReport"+FactoryType]["FormParams"]
    // ApiParams = useCommonParams["StartBankQTYReport"+FactoryType]["APIMessage"];
    GetDetailData(TableParams);
});
</script>

<template>
    <div>
        <a-card>
            <div style="height: 600px">
                <DynamicTable
                    row-key="id"
                    :header-title="PageSetting.TopTitle"
                    :columns="PageSetting.Columns"
                    :key="PageSetting.TopTitle"
                    bordered
                    size="small"
                    :data-source="PageSetting.List"
                    :data-all="PageSetting.List"
                    :pagination="false"
                    export-file-name="导出"
                    :row-class-name="getRowClassNmae"
                    :flexwidth="1"
                    sticky
                >
                    <template #export-button>
                        <a-button type="success"> 导出 </a-button>
                    </template>
                </DynamicTable>
            </div>
        </a-card>
    </div>
</template>
