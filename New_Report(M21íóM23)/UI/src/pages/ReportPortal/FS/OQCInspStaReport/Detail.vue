<template>
  <div>
      <a-card>
          <div>
              <DynamicTable
                  row-key="id"
                  :header-title="PageSetting.TopTitle"
                  :columns="PageSetting.Columns"
                  :key="PageSetting.TopTitle"
                  bordered
                  :flexwidth="15"
                  :data-source="PageSetting.moreData"
                  :data-all="PageSetting.moreData"
                  :pagination="false"
                  export-file-name="导出"
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

<script lang="ts" setup>
import { useTable } from '@/components/core/dynamic-table';
import { reactive, watchEffect } from 'vue';
import { GetCommon } from '@/api/pages/common';
import { TableRendering, antcustomCell, antdclick, antmegercell } from '@/pages/ReportPortal/utils/common'
import { ReportCommon } from '@/store/module/ReportCommon';

let useCommonParams = ReportCommon();
let FactoryType: string = useCommonParams.Fab;
let TableParams = useCommonParams['OQCInspStaReport' + FactoryType].FirstTableParams;


let PageSetting = reactive({
  moreData: [],
  List: [] as any,
  Columns: [] as any,
  TopTitle: 'LCMA OQC 品質推移圖 Detail'
  // BottomTitle:''
});
const [DynamicTable] = useTable();

async function GetDetailData(TableParams: any) {
  console.log("-*-", TableParams);
  let { api01: { Data0: { Table: Data }, title, title2 } } = await GetCommon(TableParams);
  PageSetting.moreData = Data
  let temp1: Object[] = [];
  Object.keys(Data[0]).forEach((item) => {
    temp1.push(item);
  });
  console.log(temp1);
  
  let temp: Object[] = TableRendering(title, title2, temp1, true, [0]);
  PageSetting.Columns = temp;
  // List
}




watchEffect(() => {
  //一阶表格监控
  TableParams = useCommonParams['OQCInspStaReport' + FactoryType].FirstTableParams;

  GetDetailData(TableParams);
});
</script>
