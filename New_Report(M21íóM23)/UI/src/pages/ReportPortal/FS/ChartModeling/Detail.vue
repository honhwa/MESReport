<template>
  <div class="StartSummaryReport">
    <a-card class="ScrollForm">
      <SchemaForm @submit="confirm" @submit1="confirm" showUploadButton @upload="DELETE" showcustomButton
        @custom="UPDATE_ALL"> </SchemaForm>
      <!-- <span>Update:{{ UpdateTime }}</span> -->
    </a-card>
  </div>
</template>


<script lang="tsx" setup>

import { AddStartSummaryReportWithOut } from '@/store/module/StartSummaryReport';
import { AddCommonPageParams } from '@/store/module/commonpage';
import router from '@/router';
import { reactive, watchEffect } from 'vue';
import { useForm } from '@/components/core/schema-form';
import { ref } from '@vue/reactivity';
import { onMounted } from '@vue/runtime-core';
import { cloneDeep } from 'lodash-es';
import { GetCommon } from '@/api/pages/common';
import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect';
import { ReportCommonWithOut } from '@/store/module/ReportCommon';
import { message } from 'ant-design-vue';

let useCommonParams = AddCommonPageParams();
// 導入table和form-schema的數據
const UseParamsAdd = ReportCommonWithOut();
let FactoryType: any = UseParamsAdd.SubFab;
let TableParams = useCommonParams.FirstTableParams;
function getImport(site: any) {
  const { schemas, optionsdata, getSizeData, getUpdatetime } = require(`@/pages/ReportPortal/FS/ChartModeling/${site}/form-updata`);
  return { schemas, optionsdata, getSizeData, getUpdatetime };
}

const { schemas, optionsdata, getSizeData } = getImport(FactoryType);
let UpdateTime = ref("");
const { InfiniteScroll, mountedowner, mountedOper } = useRemoteSelect()
// 給form-schema表單的下拉選擇框賦值
onMounted(async () => {
  if (FactoryType == "BEOL2") {
    await mountedowner(optionsdata, FactoryType);
    await mountedOper(optionsdata, FactoryType);
    optionsdata.worder_cateData = await getSizeData(GetCommon, 'WORDER_CATE', {
      name: "Date",
      apiName: "WorderType",
      FactoryType: FactoryType,
    });
  } else {
    optionsdata.worderTypeData = await getSizeData(GetCommon, 'WORDER_TYPE', {
      name: "Date",
      apiName: "WorderType",
      FactoryType: FactoryType,
    });
  }
  const { Date: { Table: Data } } = await GetCommon({
    name: "Date",
    apiName: "Update",
    FactoryType: FactoryType,
  });
  UpdateTime.value = Data[0]["T_STAMP"].substring(0, 19);


})

watchEffect(() => {
  //一阶表格监控

});
console.log(TableParams);

const [SchemaForm, dynamicFormRef] = useForm({
  labelWidth: 120,
  schemas: schemas(TableParams),
  actionColOptions: {
    span: 24
  },
  submitButtonOptions: {
    text: 'UPDATE'
  },
  uploadButtonOptions: {
    text: 'DELETE'
  },
  customButtonOptions: {
    text: 'UPDATE ALL'
  }
});



//定義vuex
let useStartSummaryReport = AddStartSummaryReportWithOut();
// 確認按鈕
async function confirm(values) {
  let baseParams = { name: 'Data', apiName: 'update_Chtmodel', FactoryType: FactoryType, G_FAC: 'A', ReportType: 'Updata' };
  baseParams = { ...baseParams, ...values };
        let {
          Data:api01
        } = await GetCommon({ ...baseParams, ...values });
  console.log(api01)
  if(api01.includes("Success")){
    message.success('success');
  }
  else{
    message.warn('failed');
  }
}
async function DELETE(values) {
  let baseParams = { name: 'Data', apiName: 'update_Chtmodel', FactoryType: FactoryType, G_FAC: 'A', ReportType: 'Delete' };
  baseParams = { ...baseParams, ...values };
        let {
          Data:api01
        } = await GetCommon({ ...baseParams, ...values });
  console.log(api01)
  if(api01.includes("Success")){
    message.success('success');
  }
  else{
    message.warn('failed');
  }
}
async function UPDATE_ALL(values) {
  let baseParams = { name: 'Data', apiName: 'update_Chtmodel', FactoryType: FactoryType, G_FAC: 'A', ReportType: 'Updata_ALL' };
  baseParams = { ...baseParams, ...values };
        let {
          Data:api01
        } = await GetCommon({ ...baseParams, ...values });
  console.log(api01)
  if(api01.includes("Success")){
    message.success('success');
  }
  else{
    message.warn('failed');
  }
}
</script>
<style scoped lang="less">
.desciprtion {
  background-color: #f0e68c;

  ul> {
    margin: 10px;
  }
}
</style>

