<template>
  <div class='Report'>
    <a-card>
      <a-row>
        <a-col :span='24 - PageParams.TableSpan' class='ScrollForm'>
          <SchemaForm @submit='confirm'> </SchemaForm>
        </a-col>
        <a-col :span='PageParams.TableSpan'>
          <div ref='report'>
            <DynamicTable row-key='report' :header-title='PageSetting.Title' :PageParams='PageParams'
              :columns='PageSetting.Columns' export-type='Exceljs' showFavorite sticky :key='PageSetting.Columns' bordered
              :row-class-name='getRowClassNmae' :data-source='PageSetting.moreData' :data-all='PageSetting.moreData'
              :scroll="{ y: 350 }" :pagination='false' :export-file-name='PageSetting.Title'>
              <template #export-button>
                <a-button type='success'> 导出 </a-button>
              </template>
            </DynamicTable>
            <MixedChart height="350px" width="100%" :x-data="PageSetting.ChartXData" :seriesData="PageSetting.ChartData"
              title="LCMA OQC 檢驗狀況 Chart" :y-data="YDefine" />
          </div>
        </a-col>
      </a-row>
    </a-card>
  </div>
</template>

<script lang='tsx' setup>
import { PageParams } from '@/components/core/dynamic-table/src/dynamic-table'
import { useLoading } from '@/components/feat/Loading';
import { GetCommon } from '@/api/pages/common';
import { cloneDeep } from 'lodash';
import { onMounted } from '@vue/runtime-core';
import { reactive, ref } from '@vue/reactivity';
import { useForm } from '@/components/core/schema-form';
import { useTable } from '@/components/core/dynamic-table';
import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect';
import { ReportCommon } from '@/store/module/ReportCommon';
import { message } from 'ant-design-vue';
import { getRowClassNmae, TableRendering, antcustomCell, antdclick, antmegercell } from '@/pages/ReportPortal/utils/common'
import MixedChart from '@/components/charts/MixedChart.vue';
import { useRouter } from 'vue-router';

const router = useRouter();
const UseParamsAdd = ReportCommon()
let FactoryType: string = UseParamsAdd.Fab;
const [openFullLoading, closeFullLoading] = useLoading({
  tip: '數據加載中...'
});
const report = ref<HTMLElement | null>(null);
function getImport(site: string) {
  const { schemas, optionsdata } = require(`@/pages/ReportPortal/FS/OQCInspStaReport/${site}/form-schema`);
  const { baseColumns } = require(`@/pages/ReportPortal/FS/OQCInspStaReport/${site}/Table`);
  return { schemas, optionsdata, baseColumns };
}
let { schemas, optionsdata, baseColumns } = getImport(FactoryType);

let PageSetting = reactive<BasePageParams.BasePageSetting>({
  List: [] as any,
  Columns: [] as any,
  Title: '',
  moreData: [],
  SQLHidden: '',
  APIColumns: [] as any,
  ChartData: [] as any,
  ChartXData: [] as any
});
switch (FactoryType) {
  case '':
    PageSetting.Title = '';
    break;
  default:
    break;
}
const [DynamicTable] = useTable();
const [SchemaForm, dynamicFormRef] = useForm({
  labelWidth: 120,
  schemas: schemas(FactoryType),
  actionColOptions: {
    span: 24
  },
  layout: 'vertical'
});


async function confirm(values) {
  openFullLoading();
  let baseParams = {
    apiName: 'OQCInspStaReport',
    FactoryType: FactoryType,
    G_FAC: UseParamsAdd.G_FAC_COM,
    ReportType: ''
  };
  Object.keys(values).forEach((key) => {
    if (!values[key]) {
      values[key] = '';
    }
  });


  try {
    let { api01: { Data0: { Table: Data }, title, title2 } } = await GetCommon({ ...baseParams, ...values });

    if (Data.length == 0) {
      PageSetting.moreData = [];
      closeFullLoading();
      return message.warn('无数据!');
    }
    let temp1: Object[] = [];
    Object.keys(Data[0]).forEach((item) => {
      temp1.push(item);
    });
    let temp: Object[] = TableRendering(title, title2, temp1, true, [0]);
    temp.push({
      title: 'Down Grade', children: [{ title: '數量', dataIndex: 'DOWN_CNT', align: '', className: '', fixed: '' },
      { title: '比率(%)', dataIndex: 'DOWN_CNTNUM', align: '', className: '', fixed: '' }]
    })
    let ChartData1 = Data.map((item) => item.OKNUM);
    let ChartData2 = Data.map((item) => item.NG_CNTNUM);
    let ChartData3 = Data.map((item) => item.UP_CNTNUM);
    let ChartData4 = Data.map((item) => item.DOWN_CNTNUM);
    PageSetting.ChartData = [
      { name: 'OK比率', data: ChartData1, type: 'bar', barWidth: 20, stack: 'Ad' },
      { name: 'NG比率', data: ChartData2, type: 'bar', barWidth: 20, stack: 'Ad' },
      { name: 'Up Grade比率', data: ChartData3, type: 'bar', barWidth: 20, stack: 'Ad' },
      { name: 'Down Grade比率', data: ChartData4, type: 'bar', barWidth: 20, stack: 'Ad' },
    ];
    PageSetting.ChartXData = Data.map((item) => item.OPERATOR);
    PageSetting.moreData = Data;
    let clickarray: string[] = [];
    Object.keys(Data[0]).forEach((item, index) => {
      console.log(index);
      if (index > 0) {
        clickarray.push(item);
      }
    });
    PageSetting.Columns = temp;

    antcustomCell(PageSetting.Columns, ({ record, rowindex, column, item, index, paramsColumns }) => {

      return {
        // ...antmegercell({ record, rowindex, column }, ["責任歸屬"], PageSetting['TopList']),
        ...antdclick(column, clickarray, () => {
          let bottomparams = cloneDeep({ ...baseParams, ...values });
          bottomparams.ReportType = 'OQCInspStaDetail';
          bottomparams['col0'] = paramsColumns['title'];
          bottomparams['row0'] = record['OPERATOR'];
          UseParamsAdd.AddFirstTableParams("OQCInspStaReport" + FactoryType, bottomparams);
          switch (UseParamsAdd.Fab) {
            case "LCM1":
              router.push({ name: 'OQCInspStaDetailLCM1Detail' });
              break;
            case "LCM2":
              router.push({ name: 'OQCInspStaDetailLCM2Detail' });
              break;
            case "SET1":
              router.push({ name: 'OQCInspStaDetailSET1Detail' });
              break;
          }
        })
      }
    })

    closeFullLoading();
  } catch (error) {
    console.log(error)
    closeFullLoading();
  }

}
let YDefine = [
  {
    type: 'value',
    min: 0,
    name: '比率'
    // max: 1500
    // interval: 500
  }
];
onMounted(async () => {
})
</script>

<style lang="less" scoped></style>