<template>
    <div class="OutputByGradeandDownGradeRatio">
        <a-card>
            <a-row>
                <a-col>
                    <DynamicTable row-key="id" :header-title="PageSetting.title" :columns="PageSetting.TopColumns"
                        :key="PageSetting.TopColumns" bordered size="small" :row-class-name="getRowClassNmae"
                        :style="{ 'margin-bottom': '20px' }" :flexwidth="1" :data-source="PageSetting.TopList"
                        :data-all="PageSetting.TopList" sticky :pagination="false" export-file-name="导出">
                        <template #export-button>
                            <a-button type="success"> 导出 </a-button>
                        </template>
                    </DynamicTable>
                    <DynamicTable row-key="id" :header-title="PageSetting.title" :columns="PageSetting.BottomColumns"
                        :key="PageSetting.BottomColumns" bordered size="small" :row-class-name="getRowClassNmae"
                        :flexwidth="1" :data-source="PageSetting.BottomList" :data-all="PageSetting.BottomList" sticky
                        :pagination="false" export-file-name="导出">
                        <template #export-button>
                            <a-button type="success"> 导出 </a-button>
                        </template>
                    </DynamicTable>
                </a-col>
            </a-row>
        </a-card>
    </div>
</template>


<script lang="tsx" setup>
import { useTable } from '@/components/core/dynamic-table';
import { reactive } from '@vue/reactivity';
import { GetCommon } from '@/api/pages/common';
import { inject, onMounted, watch } from '@vue/runtime-core';
import { message } from 'ant-design-vue';
import { cloneDeep } from 'lodash';
import { getRowClassNmae, getfinalsum, dclick } from '@/pages/ReportPortal/utils/common'
import { ReportCommonWithOut } from "@/store/module/ReportCommon";
import { watchEffect } from 'vue';
const ReportCommonParams = ReportCommonWithOut();

let FactoryType: string = ReportCommonParams.Fab;


let ALLFormParams = ReportCommonParams["OutputByGradeandDownGradeRatio" + FactoryType]["FirstTableParams"];

watch(
    () => ReportCommonParams["OutputByGradeandDownGradeRatio" + FactoryType]["FirstTableParams"],
    async (newvalue, oldvalue) => {
        ALLFormParams = newvalue;
        await getTableData()
    },
    {
        deep: true,
    }
)



function getImport(site: any) {
    const { detailColumns, detailColumns2 } = require(`@/pages/ReportPortal/FS/OutputByGradeandDownGradeRatio/${site}/table`);
    return { detailColumns, detailColumns2 };
}
const { detailColumns, detailColumns2 } = getImport(FactoryType);

// 頁面數據
let PageSetting = reactive({
    TableSpan: 20,
    showmenu: true,
    TopList: [] as any,
    TopColumns: [] as any,
    BottomList: [] as any,
    BottomColumns: [] as any,
    Message: {},
    title: "LCMA Output By Grade "


});
const [DynamicTable] = useTable();

async function getTableData() {
    PageSetting.TopList = [];
    PageSetting.TopColumns = [];
    const { Date: { Data0: { Table: Data }, Message: sqlmessage } } = await GetCommon(ALLFormParams);
    Data.push(getfinalsum(Data, ["QTY"], "PROD_NBR"));
    PageSetting.TopList = Data;
    let tempColumns = cloneDeep(detailColumns);
    dclick(tempColumns, ["QTY"], getbottomData)
    PageSetting.TopColumns = tempColumns;

}
async function getbottomData(record, rowindex, column, item, index) {
    let Bottomparams = cloneDeep(ALLFormParams);
    Bottomparams["row01"] = record["PROD_NBR"];
    Bottomparams["row11"] = record["PALLET"];
    if (record["PROD_NBR"] == "總計") {
        Bottomparams["row01"] = "合計";
        Bottomparams["row11"] = "";
        Bottomparams["row3"] = PageSetting.TopList[rowindex - 1]["PROD_NBR"]
    }
    Bottomparams["ReportType"] = "OutputByGradeDetail2";
    const { Date: { Data0: { Table: Data }, Message: sqlmessage } } = await GetCommon(Bottomparams);
    Data.push({ PROD_NBR: "總計", ENTITY_NO: Data.length })
    PageSetting.BottomColumns = detailColumns2;
    PageSetting.BottomList = Data;


}
onMounted(
    async () => {
        await getTableData();
    }
)

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
