<template>
  <div class='Report'>
    <a-card>
      <a-row>
        <a-col :span='24 - PageParams.TableSpan' class='ScrollTable' v-show='PageParams.ShowMenu'>
          <SchemaForm @submit='confirm'> </SchemaForm>
        </a-col>
        <a-col :span='PageParams.TableSpan'>
            <DynamicTable row-key='WipBL2Summary' :header-title='PageSetting.Title' 
            :PageParams="PageParams" sticky
            :columns='PageSetting.Columns'
              showFavorite 
              :key='PageSetting.Columns' bordered size='small' :row-class-name='getRowClassNmae'
              :flexwidth="1"
              :data-source='PageSetting.List' :data-all='PageSetting.moreData' :pagination='false' 
              export-file-name='导出'>
              <template #export-button>
                <a-button type='success'> 导出 </a-button>
              </template>
            </DynamicTable>
            <DynamicTable row-key='WipBL2Summary' :header-title='BottomPageSetting.Title' sticky
              :columns='BottomPageSetting.Columns' :key='BottomPageSetting.Columns' bordered size='small'
              :flexwidth="1"
              :row-class-name='getRowClassNmae' :data-source='BottomPageSetting.List'
              :data-all='BottomPageSetting.moreData' :pagination='false'  export-file-name='导出'>
              <template #export-button>
                <a-button type='success'> 导出 </a-button>
              </template>
            </DynamicTable>
        </a-col>
      </a-row>
    </a-card>
  </div>

</template>

<script lang='tsx' setup>
import {PageParams} from '@/components/core/dynamic-table/src/dynamic-table'
import { useLoading } from '@/components/feat/Loading';
import { GetCommon } from '@/api/pages/common';
import { cloneDeep } from 'lodash';
import { useForm } from '@/components/core/schema-form';
import { useTable } from '@/components/core/dynamic-table';
import { onMounted } from '@vue/runtime-core';
import { reactive, ref } from '@vue/reactivity';
import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { dclick, getRowClassNmae, } from '@/pages/ReportPortal/utils/common'
import { message } from 'ant-design-vue';
import dayjs from 'dayjs';
const UseParamsAdd = AddCommonPageParams()
let FactoryType: any = UseParamsAdd.Fab;
const [openFullLoading, closeFullLoading] = useLoading({
  tip: '數據加載中...'
});
const { InfiniteScroll, mountedOper, mountedFSBLU2 } = useRemoteSelect();
function getImport(site: any) {
  const { schemas, optionsdata, GrpOptions } = require(`@/pages/ReportPortal/FS/WipBL2Summary/${site}/form-schema`);
  const { detailColumns } = require(`@/pages/ReportPortal/FS/WipBL2Summary/${site}/Table`);
  return { schemas, optionsdata, GrpOptions, detailColumns };
}
let { schemas, optionsdata, GrpOptions, detailColumns } = getImport(FactoryType);
const TopTable = ref<HTMLElement | null>(null);
const BottomTable = ref<HTMLElement | null>(null);
let PageSetting = reactive<BasePageParams.BasePageSetting>({
  List: [] as any,
  Columns: [] as any,
  Title: 'BLA Wip backlight Summary ',
  moreData: [],
  SQLHidden: '',
  APIColumns: [] as any,
});
let BottomPageSetting = reactive<BasePageParams.BasePageSetting>({
  List: [] as any,
  Columns: [] as any,
  Title: 'LCMA Wip backlight Detail',
  moreData: [],
  SQLHidden: '',
  APIColumns: [] as any,
});
const [DynamicTable] = useTable();
const [SchemaForm, dynamicFormRef] = useForm({
  labelWidth: 120,
  schemas:schemas(FactoryType),
  actionColOptions: {
    span: 24
  },
  layout: 'vertical'
});
let bottomparams = {};
async function click(record, rowindex, column, item, index) {
  openFullLoading();
  // 二阶页面需要整行所有的记录转为json字符串
  bottomparams["ReportType"] = "WipBL2SummaryDetail";
  delete record["SUB"]
  bottomparams["sqlcond"] = JSON.stringify(record);
  try {
    let { Date: { Data0: { Table: Data } } } = await GetCommon(bottomparams);
    if(Data.length == 0){
      closeFullLoading();
      return message.warn("無數據!");
    }
    Data.forEach(item => {
      if (item["TRANS_DATE"]) {
        item["STAYHRS"] = 0;
        item["TRANS_DATE"] = "";
      } else {
        // item["TRANS_DATE"] = "";
        // 源代码：item["STAYHRS"] = 截取TRANS_DATE的前10位，和现在的时间相差多少分钟再除以60，保留两位小数
        item["STAYHRS"] = (dayjs(dayjs(new Date()).format("YYYY-MM-DD HH:mm:ss")).diff(dayjs(item["TRANS_DATE"].substr(0, 19).replace(/\./g, ":")), 'm') / 60).toFixed(2);
      }
      switch (item["PRIORITY"]) {
        case "1":
          item["PRIORITY"] = "Normal"
          break;
        case "2":
          item["PRIORITY"] = "Hot"
          break;
        case "3":
          item["PRIORITY"] = "Super Hot"
          break;
        default:
          break;
      }

    })
    detailColumns.forEach(item=>{
      item["width"] = 900 / detailColumns.length;
    })
    BottomPageSetting.Columns = detailColumns;
    BottomPageSetting.moreData = Data;
    if (BottomPageSetting.moreData.length > 100) {
      //预先加载40条数据
      BottomPageSetting.List = BottomPageSetting.moreData.slice(0, 40);
    }
    else {
      BottomPageSetting.List = BottomPageSetting.moreData;
    }
    closeFullLoading();
  } catch (error) {
    closeFullLoading();
  }

}
async function confirm(values) {
  openFullLoading();
  let tempParams = { name: 'Date', apiName: 'WipBL2Summary', FactoryType: UseParamsAdd.Fab, G_FAC: 'A', building: '' };
  Object.keys(values).forEach((key) => {
    if (!values[key]) {
      values[key] = '';
    }
  });
  let baseColumns = { ...tempParams, ...values };
  bottomparams = cloneDeep(baseColumns);
  if (baseColumns["Colname"].length == 0) {
    closeFullLoading();
    return message.warn("請輸入 顯示欄位")
  }
  // 合成columns
  let Columns: object[] = []
  values["Colname"].forEach(item => {
    Columns.push({
      title: GrpOptions.find(element => element.value == item).label,
      align: 'center',
      dataIndex: GrpOptions.find(element => element.value == item).value.toUpperCase(),
      key: GrpOptions.find(element => element.value == item).value.toUpperCase(),
    })
  })
  Columns = [...Columns, {
    title: 'SUB Qty',
    align: 'center',
    dataIndex: 'SUB',
    key: "SUB",
  }]
  try {
    let { Date: { Data0: { Table: Data } } } = await GetCommon(baseColumns);
    if (Data.length == 0) {
      closeFullLoading();
      return message.warn("无数据!");
    }
    dclick(Columns, [...Columns.map(item => { return item["dataIndex"] }), "ACCT_MONTH"], click);
    console.log("輸出最終的Columns", Columns);
    PageSetting.Columns = Columns;
    PageSetting.moreData = Data;
    if (PageSetting.moreData.length > 100) {
      //预先加载40条数据
      PageSetting.List = PageSetting.moreData.slice(0, 40);
    }
    else {
      PageSetting.List = PageSetting.moreData;
    }
    closeFullLoading();
  } catch (error) {
    closeFullLoading();
  }
}

InfiniteScroll(TopTable, PageSetting);
InfiniteScroll(BottomTable, BottomPageSetting);
onMounted(async () => {
  /* await mountedFSBLU2(optionsdata, FactoryType);
  await mountedOper(optionsdata, FactoryType); */
})
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