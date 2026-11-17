<template>
  <div class="ToBeScrapResponsibility">
    <a-row>
      <a-col :span="24 - PageParams.TableSpan" class="ScrollForm" v-show="PageParams.ShowMenu">
        <SchemaForm @submit="confirm"> </SchemaForm>
      </a-col>
      <a-col :span="PageParams.TableSpan">
        <a-button type="primary" @click="changeView" v-show="PageSetting.List.length != 0">{{
          PageSetting.buttonText
        }}</a-button>

        <V3chart
          width="100%"
          height="90%"
          :options="PageSetting.EchartOptions"
          isClick
          v-if="PageSetting.showPage == 0"
        ></V3chart>
        <a-row v-if="PageSetting.showPage == 1" style="height: calc(100% - 80px)">
          <a-col :span="12" style="height: 50%">
            <V3chart :options="PageSetting.PieChart_PROD" isClick></V3chart>
          </a-col>
          <a-col :span="12" style="height: 50%">
            <V3chart :options="PageSetting.PieChart_OWNER" isClick></V3chart>
          </a-col>
          <a-col :span="12" style="height: 50%">
            <V3chart :options="PageSetting.PieChart_LCM" isClick></V3chart>
          </a-col>
          <a-col :span="12" style="height: 50%">
            <V3chart :options="PageSetting.PieChart_LCD" isClick></V3chart>
          </a-col>
        </a-row>
        <div v-if="PageSetting.showPage == 2">
          <DynamicTable
            row-key="id"
            header-title="LCMA To Be Scrap 責任歸屬 ReportChart"
            showFavorite
            sticky
            :PageParams="PageParams"
            :columns="PageSetting.Columns"
            :key="PageSetting.Columns"
            bordered
            id="ToBeScrapResponsibility"
            :data-source="PageSetting.List"
            :data-all="PageSetting.List"
            export-type="Table"
            :pagination="false"
            export-file-name="导出"
            :row-class-name="getRowClassName"
            :scroll="PageSetting.TableSize"
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
  import { reactive } from '@vue/reactivity';
  import { useForm } from '@/components/core/schema-form';
  import { GetCommon } from '@/api/pages/common';
  import { message } from 'ant-design-vue';
  import V3chart from '@/components/charts/V3chart.vue';
  import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect';
  import { useLoading } from '@/components/feat/Loading';
  import { ReportCommon } from '@/store/module/ReportCommon';
  import { mergeTableRow } from '@/pages/ReportPortal/utils/common';
  import { sum } from 'xe-utils';
  import { cloneDeep } from 'lodash-es';

  const UseParamsAdd = ReportCommon();
  let FactoryType: any = UseParamsAdd.Fab;
  function getImport(site: any) {
    const {
      schemas,
      optionsdata,
    } = require(`@/pages/ReportPortal/TN/ToBeScrapResponsibility/${site}/form-schema`);
    const {
      aheadColumns,
      rearColumns,
      getColumns,
    } = require(`@/pages/ReportPortal/TN/ToBeScrapResponsibility/${site}/Table`);
    return { schemas, optionsdata, aheadColumns, rearColumns, getColumns };
  }
  const { schemas, optionsdata, aheadColumns, rearColumns, getColumns } = getImport('MOD');

  // 頁面數據
  let pieOption = {
    title: {
      left: 'center',
    },
    tooltip: {
      trigger: 'item',
    },
    legend: {
      orient: 'vertical',
      left: 'left',
    },
    series: [
      {
        type: 'pie',
        radius: '50%',
        percentPrecision: 2,
        emphasis: {
          itemStyle: {
            shadowBlur: 10,
            shadowOffsetX: 0,
            shadowColor: 'rgba(0, 0, 0, 0.5)',
          },
        },
        label: {
          formatter: '{d}%',
        },
      },
    ],
  };
  let PageSetting = reactive({
    TableSpan: 21,
    showmenu: true,
    List: [] as any,
    Columns: [] as any,
    EchartOptions: {
      title: [
        {
          text: 'Scrape Chart',
          left: 'center',
        },
      ],
      tooltip: {
        trigger: 'item',
        axisPointer: {
          type: 'shadow',
        },
      },
      legend: {
        bottom: '16%',
        type: 'scroll',
        padding: [10, 0, 0, 0],
        textStyle: {
          color: 'rgba(36, 173, 254, 1)',
          fontSize: '1rem',
        },
        itemHeight: 5,
        itemWidth: 10,
      },
      grid: {
        left: '3%',
        top: '15%',
        right: '4%',
        bottom: '20%',
        containLabel: true,
      },
      xAxis: [
        {
          type: 'category',
        },
      ],
      yAxis: [
        {
          type: 'value',
          name: 'Scrap Qty',
          position: 'left',
          min: 0,
        },
        {
          type: 'value',
          name: 'Scrap Rate(%)',
          position: 'right',
          min: 0,
          max: 5,
        },
      ],
    } as any,

    PieChart_PROD: cloneDeep(pieOption) as any,
    PieChart_OWNER: cloneDeep(pieOption) as any,
    PieChart_LCM: cloneDeep(pieOption) as any,
    PieChart_LCD: cloneDeep(pieOption) as any,

    Message: {},
    showPage: 0,
    buttonText: '切换表格',
    TableSize: { x: 1000, y: -1 },
  });
  const [DynamicTable] = useTable();

  const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas: schemas(UseParamsAdd.Fab),
    actionColOptions: {
      span: 24,
    },
    layout: 'vertical',
  });

  function changeView() {
    PageSetting.showPage = (PageSetting.showPage + 1) % 3;
    if (PageSetting.showPage == 1) {
      PageSetting.buttonText = '切换表格';
    } else {
      PageSetting.buttonText = '切换图表';
    }
  }
  const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...',
  });
  function getRowClassName(record: Object) {
    if (Object.keys(record).length != 0) {
      for (const [key, value] of Object.entries(record)) {
        if (value == '小計') {
          return 'Green';
        } else if (['總計', 'Sum'].includes(value)) {
          return 'Orange';
        } else if ('sub-total' == value) {
          return 'sub-total';
        } else if ('total' == value) {
          return 'total';
        }
        if (['Shipping Qty', 'Total Yield'].includes(value)) {
          return 'DeepOrange';
        }
      }
    }
  }

  async function confirm(values) {
    PageSetting.EchartOptions.xAxis.data = [];
    PageSetting.EchartOptions.series = [];
    let baseParams = reactive({
      name: 'Data',
      apiName: 'TN_ToBeScrapResponsibility',
      FactoryType: FactoryType,
    });
    Object.keys(values).forEach((key) => {
      if (!values[key]) {
        values[key] = '';
      }
    });
    //給ParamsData賦值
    dynamicFormRef.validate().then(async () => {
      openFullLoading();
      //呼叫 api
      const {
        Data: {
          Data: { Table: Data },
          DataPie: { Table: DataPie },
          sql: sql,
        },
      } = await GetCommon({ ...baseParams, ...values });
      if (Data.length == 0) {
        closeFullLoading();
        return message.warn('无数据!');
      }
      UseParamsAdd.AddFormParams('ToBeScrapResponsibility' + UseParamsAdd.Fab, { ...values });
      //新增欄位
      let operationList = Object.keys(groupBy(Data, { key: ['SCRAP_DATE'] }));
      operationList = operationList.filter((x) => x != 'Total');
      //#region 圖表資料處理
      let series: any = [];
      let chartData = groupBy(
        Data.filter(
          (x) =>
            x.SCRAP_DATE != 'Total' &&
            x.RESP_OWNER != 'Shipping Qty' &&
            x.RESP_OWNER != '半成品狀態',
        ),
        {
          key: ['RESP_OWNER', 'SCRAP_DATE'],
          sum: ['QTY', 'DEFECTIVE'],
        },
      );
      for (let owner in chartData) {
        let bar: any = {};
        bar.name = owner;
        bar.type = 'bar';
        bar.stack = 'total';
        bar.barWidth = 20;
        bar.emphasis = { focus: 'series' };
        bar.yAxisIndex = 0;
        bar.data = [];
        operationList.forEach((dt) => {
          let val = chartData[owner][dt]?.sum.QTY || 0;
          bar.data.push(val);
        });
        series.push(bar);

        let line: any = {};
        line.name = owner;
        line.type = 'line';
        line.yAxisIndex = 1;
        line.data = [];
        operationList.forEach((dt) => {
          let val = chartData[owner][dt]?.sum.DEFECTIVE || 0;
          line.data.push(val);
        });
        series.push(line);
      }
      series.sort((a, b) => {
        if (a.name < b.name) {
          return -1;
        }
        if (a.name > b.name) {
          return 1;
        }
      });
      PageSetting.EchartOptions.xAxis[0].data = operationList;
      PageSetting.EchartOptions.series = series;

      let pieProd = DataPie.filter((x) => x.CHART == 'PROD').map((x) => {
        return { value: x.QTY, name: x.NAME };
      });
      PageSetting.PieChart_PROD.title.text = '報廢機種分析 Chart';
      PageSetting.PieChart_PROD.series[0].data = pieProd;
      let pieOwner = DataPie.filter((x) => x.CHART == 'OWNER').map((x) => {
        return { value: x.QTY, name: x.NAME };
      });
      PageSetting.PieChart_OWNER.title.text = '報廢責任歸屬分析 Chart';
      PageSetting.PieChart_OWNER.series[0].data = pieOwner;
      let pieLCM = DataPie.filter((x) => x.CHART == 'LCM4').map((x) => {
        return { value: x.QTY, name: x.NAME };
      });
      PageSetting.PieChart_LCM.title.text = 'LCM4 責任細分 Chart';
      PageSetting.PieChart_LCM.series[0].data = pieLCM;
      let pieLCD = DataPie.filter((x) => x.CHART == 'LCD').map((x) => {
        return { value: x.QTY, name: x.NAME };
      });
      PageSetting.PieChart_LCD.title.text = 'LCD 責任細分 Chart';
      PageSetting.PieChart_LCD.series[0].data = pieLCD;

      //#endregion

      //#region 表格資料處理
      let newDataDetail: any = [];
      let detailData = groupBy(Data, {
        key: ['RESP_OWNER', 'SCRAP_CODE', 'SCRAP_DATE'],
      });
      let allTotal: any = {
        RESP_OWNER: '總計',
        SCRAP_CODE: '',
        C_DESC: '',
        Sort: 'ZZZZ',
      };
      let totalYield: any = {
        RESP_OWNER: 'Total Yield',
        SCRAP_CODE: '',
        C_DESC: '',
        Sort: 'ZZZZZA',
      };
      Object.keys(detailData).forEach((owner) => {
        let ownerTotal: any = {
          RESP_OWNER: owner,
          SCRAP_CODE: '小計',
          C_DESC: '',
        };
        Object.keys(detailData[owner]).forEach((code) => {
          let detail: any = {
            RESP_OWNER: owner,
            SCRAP_CODE: code,
            C_DESC: '',
          };
          Object.keys(detailData[owner][code]).forEach((dt) => {
            let qty = detailData[owner][code][dt][0].QTY || 0;
            let defective = detailData[owner][code][dt][0].DEFECTIVE || 0;
            let c_desc = detailData[owner][code][dt][0].C_DESC || '';
            if (owner != 'Shipping Qty' && owner != '半成品狀態') {
              detail[dt] = qty;
              detail[dt + '-DFCT_CODE'] = defective;
              detail.C_DESC = c_desc;
              ownerTotal[dt] = ownerTotal[dt] + qty || qty;
              ownerTotal[dt + '-DFCT_CODE'] =
                ownerTotal[dt + '-DFCT_CODE'] + defective || defective;
              ownerTotal[dt + '-DFCT_CODE'] =
                Math.round(ownerTotal[dt + '-DFCT_CODE'] * 1000) / 1000;

              allTotal[dt] = allTotal[dt] + qty || qty;
              allTotal[dt + '-DFCT_CODE'] = allTotal[dt + '-DFCT_CODE'] + defective || defective;
              allTotal[dt + '-DFCT_CODE'] = Math.round(allTotal[dt + '-DFCT_CODE'] * 1000) / 1000;
              detail.Sort = (owner ? owner : '0') + code;
            } else {
              detail[dt] = qty;
              detail.C_DESC = c_desc;
              detail.Sort = (owner == '半成品狀態' ? 'ZZZZZF' : 'ZZZZZ') + code;
            }
          });
          newDataDetail.push(detail);
        });
        if (owner != 'Shipping Qty' && owner != '半成品狀態') {
          ownerTotal.Sort = (owner ? owner : '0') + 'ZZZZ';
          newDataDetail.push(ownerTotal);
        }
      });
      for (let dt in detailData['Shipping Qty']['']) {
        let qty = detailData['Shipping Qty'][''][dt][0].QTY || 0;
        let y = qty / (qty + allTotal[dt]);
        y = Math.round(y * 100000) / 1000;
        totalYield[dt] = y;
        totalYield[dt + '-DFCT_CODE'] = y;
      }
      newDataDetail.push(allTotal);
      newDataDetail.push(totalYield);
      newDataDetail.push({ RESP_OWNER: '　', Sort: 'ZZZZZB' });
      newDataDetail.sort((a, b) => {
        if (a.Sort < b.Sort) {
          return -1;
        }
      });
      //#endregion

      let operColumns: any = getColumns(operationList);
      let DateColumns: any = [...aheadColumns, ...operColumns, ...rearColumns];
      PageSetting.Columns = [...DateColumns]; //vue3或者adv无法监测push,shift等方法
      PageSetting.List = getTableData(newDataDetail);

      closeFullLoading();
    });
  }

  function getTableData(Data: any[]) {
    let combineCol: any = {
      data: Data,
      mergeColNames: ['RESP_OWNER'],
      firstMergeColNames: ['RESP_OWNER'],
      firstMerge: 'RESP_OWNER',
    };
    return mergeTableRow(combineCol);
  }

  const groupBy = (data, option) => {
    let deep: any = [];
    let rs = deep;
    data.forEach((item) => {
      option.key.forEach((k) => {
        rs[item[k]] = rs[item[k]] || [];
        rs = rs[item[k]];
      });
      rs.push(item);
      if (option.sum) {
        let sum: number[] = [];
        option.sum.forEach((k) => {
          rs.sum = rs.sum || [];
          rs.sum[k] = rs.sum[k] || 0;
          rs.sum[k] += item[k];
        });
      }
      if (option.avg) {
        option.avg.forEach((k) => {
          rs.avg = rs.avg || [];
          rs.avg[k] = rs.avg[k] || 0;
          if (rs.length != 0) {
            rs.avg[k] = (rs.avg[k] + item[k]) / rs.length;
          }
        });
      }
      rs = deep;
    });
    return deep;
  };
  function numberComma(num) {
    let comma = /\B(?<!\.\d*)(?=(\d{3})+(?!\d))/g;
    return num.toString().replace(comma, ',');
  }
</script>

<style lang="less" scoped>
  :deep(.ant-table) {
    height: calc(80vh - 3.8rem) !important;
  }

  :deep(.ant-table-container) {
    height: calc(80vh - 3.8rem) !important;
  }

  :deep(.ant-table-wrapper) {
    height: calc(80vh - 3.8rem) !important;
    // overflow: auto;
  }

  :deep(.ant-table-body) {
    height: calc(80vh - 3.8rem) !important;
    overflow: auto;
  }
  #app .tabs-view .tabs-view-content {
    height: calc(100% - 80px);
    padding: 10px 0 0 8px;
    overflow: hidden;
  }
</style>
