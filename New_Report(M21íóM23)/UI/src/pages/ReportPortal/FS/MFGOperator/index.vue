<script lang="ts" setup>
  import { PageParams } from '@/components/core/dynamic-table/src/dynamic-table';
  import { getRowClassNmae } from '@/pages/ReportPortal/utils/common';
  // import { schemas } from './form-schema';
  import { useForm } from '@/components/core/schema-form';
  import { useTable } from '@/components/core/dynamic-table';
  import { reactive, onMounted, ref, unref, computed } from 'vue';
  import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect';
  import { GetCommon } from '@/api/pages/common';
  import { AddCommonPageParams } from '@/store/module/commonpage';
  import { useLoading } from '@/components/feat/Loading';
  import { getPovitData, PovitParams } from '@/utils/helper/povitHelper';
  import { getrowsum } from '@/pages/ReportPortal/utils/common';
  import MixedChart from '@/components/charts/MixedChart.vue';
  import { message } from 'ant-design-vue';
  import VexTable from '@/components/core/vex-virtual-table/src/vex-virtual-table.vue';

  const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...',
  });

  // import { useInfiniteScroll, useVirtualList } from '@vueuse/core'
  const UseParamsAdd = AddCommonPageParams();
  const { mounted } = useRemoteSelect();
  const MFGOperator = ref<HTMLElement | null>(null);
  function getImport(site: any) {
    let { optionsdata } = require(`@/pages/ReportPortal/FS/MFGOperator/${site}/form-schema`);
    const { schemas } = require(`@/pages/ReportPortal/FS/MFGOperator/${site}/form-schema`);
    const { MFGOperatorColumns } = require(`@/pages/ReportPortal/FS/MFGOperator/${site}/Table.ts`);
    return { schemas, optionsdata, MFGOperatorColumns };
  }
  let { schemas, optionsdata, MFGOperatorColumns } = getImport(UseParamsAdd.Fab);
  let repeatData = ['QITEM', 'USER_NAME'];
  let PageSetting = reactive({
    List: [] as any,
    APIColumns: [] as any,
    Columns: [] as any,
    moreData: [] as any,
    ChartXData: [] as any,
    ChartData: [] as any,
    Title: '',
    QueryType: '',
  });
  const [DynamicTable] = useTable();
  const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas,
    actionColOptions: {
      span: 24,
    },
    layout: 'vertical',
  });

  async function confirm(values) {
    openFullLoading();
    PageSetting.Columns = [];
    // let dynamicColumns: TableColumn[] = []
    let baseParams = {
      apiName: 'MFGOperator',
      FactoryType: UseParamsAdd.Fab,
      G_FAC: UseParamsAdd.G_FAC_COM,
    };
    Object.keys(values).forEach((key) => {
      if (!values[key]) {
        values[key] = '';
      }
    });
    PageSetting.QueryType = values.QueryType;
    try {
      let ProdColumns: any[] = [];
      PageSetting.APIColumns = [];
      let { api01 } = await GetCommon({ ...values, ...baseParams });

      PageSetting.Title = 'MFG 人員作業';
      if (api01['Data']['Table']?.length <= 0) {
        closeFullLoading();
        return message.warning('無數據!');
      }
      api01['Data']['Table'].forEach((item) => {
        if (!PageSetting.APIColumns.includes(item['PROD_NBR'])) {
          PageSetting.APIColumns.push(item['PROD_NBR']);
          ProdColumns.push({
            title: item['PROD_NBR'],
            align: 'center',
            dataIndex: item['PROD_NBR'],
          });
        }
      });
      PageSetting.Columns = [...MFGOperatorColumns[values['QType']], ...ProdColumns];
      // PageSetting.Columns = MFGOperatorColumns;
      let config: PovitParams = {
        Data: api01['Data']['Table'],
        povitColumn: {
          PROD_NBR: 'QTY',
        },
        changeColumn: PageSetting.APIColumns,
        repeatColumn: repeatData,
      };
      let povitData = getPovitData(unref(config));
      getrowsum(povitData, PageSetting.APIColumns, 'Sum');
      PageSetting.moreData = povitData;
      if (isHaveChart) {
        let Data1: any[] = [];
        api01['HourData']['Table'].forEach((item) => {
          Data1.push(item['QTY']);
        });
        PageSetting.ChartData = [{ name: 'QTY', data: Data1, type: 'bar', barWidth: 20 }];
        PageSetting.ChartXData = PageSetting.APIColumns;
      }

      UseParamsAdd.AddFormParams({ ...baseParams, ...values });
      UseParamsAdd.AddAPIResultMessage(api01['Message']);
      closeFullLoading();
    } catch (error) {
      console.log('error:' + error);

      closeFullLoading();
    }
  }
  //实现无限滚动或者说懒加载
  onMounted(async () => {
    await GetOperationData();
    //获取Size,bigproduct,prodcut数据
    await GetProDuctData();
  });

  async function GetOperationData() {
    let { api01 } = await GetCommon({
      apiName: 'MFGOperatorWorkCtr',
      FactoryType: UseParamsAdd.Fab,
      G_FAC: 'A',
      Type:'WorkCtr'
    });
    optionsdata['OperationLabel'] = api01['Data']['Table'].map((item) => {
      return { label: `${item['WORK_CTR']}-${item['WORK_NAME']}`, value: item['WORK_CTR'] };
    });
    optionsdata['OperationLabel'].unshift({ label: 'ALL', value: 'ALL' });
  }
  async function GetProDuctData() {
    switch (UseParamsAdd.Fab) {
      case 'BLU1':
      case 'BLU2':
      case 'LCM2':
        getSizeData();
        break;
      case 'LCM1':
        await mounted(optionsdata, UseParamsAdd.Fab);
        break;
    }
  }
  async function getSizeData() {
    let { api01 } = await GetCommon({
      apiName: 'MFGOperatorWorkCtr',
      FactoryType: UseParamsAdd.Fab,
      G_FAC: UseParamsAdd.G_FAC_COM,
      Type:'Size'
    });

    optionsdata.SizeData.push({ label: 'ALL', value: 'ALL' });
    optionsdata.SizeData = api01['SizeData']['Table'].map((item) => {
      return { label: item['SIZE1'], value: item['SIZE1'] };
    });
    optionsdata.BigProductData1 = optionsdata.BigProductData = api01['BigProductData']['Table'].map(
      (item) => {
        return {
          label: `${item['PART']}(${item['SIZE1']})`,
          value: `${item['PART']}(${item['SIZE1']})`,
        };
      },
    );
    optionsdata.ProductData1 = optionsdata.ProductData = api01['ProductData']['Table'].map(
      (item) => {
        if (item['SOPTION'].includes('-') && item['SIZE1'] != '31.5') {
          return {
            label: `${item['SOPTION']}${item['SIZE1']}`,
            value: `${item['SOPTION']}${item['SIZE1']}`,
          };
        } else {
          return { label: `${item['SOPTION']}`, value: `${item['SOPTION']}` };
        }
      },
    );
  }
  let isHaveChart = computed(() => PageSetting.QueryType == 'Hour' && UseParamsAdd.Fab == 'LCM2');
</script>

<template>
  <div class="DayliMove">
    <a-row>
      <a-col :span="24 - PageParams.TableSpan" class="ScrollForm" v-show="PageParams.ShowMenu">
        <SchemaForm @submit="confirm"> </SchemaForm>
      </a-col>
      <a-col :span="PageParams.TableSpan">
        <div>
          <DynamicTable
            :flexwidth="isHaveChart?0:1"
            showFavorite
            :PageParams="PageParams"
            row-key="MFGOperator"
            :header-title="PageSetting.Title"
            :columns="PageSetting.Columns"
            :key="PageSetting.Columns"
            bordered
            size="small"
            :data-source="PageSetting.moreData"
            :data-all="PageSetting.moreData"
            :pagination="false"
            :export-file-name="PageSetting.Title"
            :row-class-name="getRowClassNmae"
            :scroll="isHaveChart?{y:350}:{}"
          >
            <template #toolbar>
              <!-- <a-button type="primary" @click="isShowExplain = !isShowExplain">{{
                                    isShowExplain ? '显示表格' : '显示说明文件'
                                }}</a-button> -->
            </template>
            <template #export-button>
              <a-button type="success"> 导出 </a-button>
            </template>
          </DynamicTable>
          <MixedChart
            v-if="isHaveChart"
            height="350px"
            width="100%"
            :x-data="PageSetting.ChartXData"
            :seriesData="PageSetting.ChartData"
            title="MFG 人員作業图表"
          />
        </div>
      </a-col>
    </a-row>
  </div>
</template>
