<template>
  <div class="Top5DefectCodeNew">
    <Loading
      :loading="compState.loading"
      :absolute="compState.absolute"
      :theme="compState.theme"
      :background="compState.background"
      :tip="compState.tip"
    />
    <a-card>
      <a-row>
        <a-col
          :span="24 - PageSetting.TableSpan"
          class="ScrollForm"
          Source-show="PageSetting.ShowMenu"
          v-show="PageParams.ShowMenu"
        >
          <div class="swapper">
            <SchemaForm @submit="confirm"> </SchemaForm>
          </div>
          <div>
            報表說明： <br/>
            1. Reworked : All<br/>
            2. 是否含快修: Yes (快修NG視為NG)<br/>
          </div>
        </a-col>
        <a-col :span="PageSetting.TableSpan">
          <div class="Top5DefectCodeNewTable" ref="Top5DefectCodeNewTable">
            <DynamicTable
            showFavorite
              row-key="id"
              header-title="Top 5 Defect Code New"
              :columns="PageSetting.Columns"
              :key="PageSetting.Columns"
              bordered
              size="small"
              :data-source="PageSetting.List"
              :data-all="PageSetting.moreData"
              :pagination="false"
              export-file-name="Export"
              :row-class-name="getRowClassNmae"
              sticky
              :scroll="PageSetting.TableSize"
              :explain="false"
              :show-header="false"
              :PageParams="PageParams"
              :OwnerParams="false"
            >
              <template #export-button>
                <a-button type="success"> Export </a-button>
              </template>
            </DynamicTable>

            <a-row>
              <a-col :span="12">
                <DynamicTable
                  row-key="Light"
                  header-title="Target ( All )"
                  :columns="PageSetting.LightColumns"
                  :key="PageSetting.LightColumns"
                  bordered
                  size="small"
                  :data-source="PageSetting.LightList"
                  :pagination="false"
                  export-file-name="Export"
                  :row-class-name="getRowClassNmae"
                  sticky
                  :scroll="{ x: -1, y: -1 }"
                  :explain="false"
                  :show-header="false"
                  :OwnerParams="false"
                  :OwnerFoldParams="false"
                >
                  <template #export-button>
                    <a-button type="success"> Export </a-button>
                  </template>
                </DynamicTable>
              </a-col>
              <a-col :span="12"></a-col>
              <a-col :span="8" :hidden="PageSetting.Top5Hidden">
                <DynamicTable
                  row-key="Light"
                  :header-title="PageSetting.PCBITitle"
                  :columns="PageSetting.Top5Columns"
                  :key="PageSetting.Top5Columns"
                  bordered
                  size="small"
                  :data-source="PageSetting.PCBIList"
                  :pagination="false"
                  export-file-name="Export"
                  :row-class-name="getRowClassNmae"
                  sticky
                  :scroll="{ x: -1, y: -1 }"
                  :explain="false"
                  :show-header="false"
                  :OwnerParams="false"
                  :OwnerFoldParams="false"
                >
                  <template #export-button>
                    <a-button type="success"> Export </a-button>
                  </template>
                </DynamicTable>
              </a-col>
              <a-col :span="8" :hidden="PageSetting.Top5Hidden">
                <DynamicTable
                  row-key="Light"
                  :header-title="PageSetting.LAMTitle"
                  :columns="PageSetting.Top5Columns"
                  :key="PageSetting.Top5Columns"
                  bordered
                  size="small"
                  :data-source="PageSetting.LAMList"
                  :pagination="false"
                  export-file-name="Export"
                  :row-class-name="getRowClassNmae"
                  sticky
                  :scroll="{ x: -1, y: -1 }"
                  :explain="false"
                  :show-header="false"
                  :OwnerParams="false"
                  :OwnerFoldParams="false"
                >
                  <template #export-button>
                    <a-button type="success"> Export </a-button>
                  </template>
                </DynamicTable>
              </a-col>
              <a-col :span="8" :hidden="PageSetting.Top5Hidden">
                <DynamicTable
                  row-key="Light"
                  :header-title="PageSetting.ACDPTitle"
                  :columns="PageSetting.Top5Columns"
                  :key="PageSetting.Top5Columns"
                  bordered
                  size="small"
                  :data-source="PageSetting.ACDPList"
                  :pagination="false"
                  export-file-name="Export"
                  :row-class-name="getRowClassNmae"
                  sticky
                  :scroll="{ x: -1, y: -1 }"
                  :explain="false"
                  :show-header="false"
                  :OwnerParams="false"
                  :OwnerFoldParams="false"
                >
                  <template #export-button>
                    <a-button type="success"> Export </a-button>
                  </template>
                </DynamicTable>
              </a-col>
            </a-row>
          </div>
        </a-col>
        <div hidden>{{ PageSetting.SQLHidden }}</div>
      </a-row>
    </a-card>
  </div>
</template>

<script lang="tsx" setup>
  import { PageParams } from '@/components/core/dynamic-table/src/dynamic-table';
  import { useForm } from '@/components/core/schema-form';
  import { useTable } from '@/components/core/dynamic-table';
  import { reactive, unref, ref, onMounted } from 'vue';
  import { GetCommon } from '@/api/pages/common';
  import useRemoteSelect from '@/pages/ReportPortal/utils/TNuseRemoteSelect';
  import { mergeTableRow } from '@/pages/ReportPortal/utils/common';
  import { ReportCommon } from '@/store/module/ReportCommon';
  import { Loading, useLoading } from '@/components/feat/Loading';

  const UseParamsAdd = ReportCommon();
  const Top5DefectCodeNewTable = ref<HTMLElement | null>(null);
  const { mountedTreeLCMOwner, mountedTNSize4, InfiniteScroll } = useRemoteSelect();
  function getImport(site: any) {
    const {
      schemas,
      optionsdata,
    } = require(`@/pages/ReportPortal/TN/Top5DefectCodeNew/${site}/form-schema`);
    const {
      aheadColumns,
      rearColumns,
      getColumns,
      LightColumns,
      TOP5Columns,
    } = require(`@/pages/ReportPortal/TN/Top5DefectCodeNew/${site}/Table`);
    return {
      schemas,
      optionsdata,
      aheadColumns,
      rearColumns,
      getColumns,
      LightColumns,
      TOP5Columns,
    };
  }
  const compState = reactive({
    absolute: true,
    loading: false,
    theme: 'dark',
    background: 'rgba(111,111,111,.7)',
    tip: '加载中...',
  });
  const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...',
  });

  let { schemas, optionsdata, aheadColumns, getColumns, LightColumns, TOP5Columns } =
    getImport('MOD');

  let PageSetting = reactive({
    List: [] as any,
    TableSpan: 20,
    ShowMenu: true,
    APIColumns: [] as any,
    Columns: [] as any,
    moreData: [] as any,
    SQLHidden: '',
    TableSize: { x: -1 },

    PCBITitle: 'PCBI',
    LAMTitle: 'LAM',
    ACDPTitle: 'ACDP',
    LightColumns: [] as any,
    Top5Columns: [] as any,
    LightList: [] as any,
    PCBIList: [] as any,
    LAMList: [] as any,
    ACDPList: [] as any,
    Top5Hidden: true,
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
  /**
   * 提交表单,验证通过后进行数据获取与处理
   * @dynamicFormRef 表单的所有参数与属性
   * @ParamsData API接收的参数
   */
  function confirm(values) {
    let baseParams = {
      name: 'Date',
      apiName: 'TN_Top5DefectCodeNew',
      FactoryType: UseParamsAdd.Fab,
    };
    Object.keys(values).forEach((key) => {
      if (!values[key]) {
        values[key] = '';
      }
    });

    dynamicFormRef.validate().then(async () => {
      openFullLoading();
      //呼叫 api
      const {
        Date: {
          Data: { Table: Data },
          Light: { Table: LightData },
          Top5: { Table: Top5Data },
          SQL01: sql01,
          SQLLight: sqlLight,
          SQLTop5: sqlTop5,
        },
      } = await GetCommon({ ...baseParams, ...values });
      UseParamsAdd.AddFormParams('Top5DefectCodeNew' + UseParamsAdd.Fab, { ...values });

      //新增欄位
      let colData = groupBy(Data, { key: ['SEQ', 'PERIOD'] });
      let columnsList: string[] = [];
      let firstRow = { KPI: 'Date', ISHERD: true };
      colData.forEach((seq) => {
        Object.keys(seq).forEach((kpi_id) => {
          if (!columnsList.includes(kpi_id)) {
            columnsList.push(kpi_id);
            firstRow[kpi_id] = kpi_id;
          }
        });
      });
      let operColumns: any = getColumns(columnsList);
      let DateColumns: any = [...aheadColumns, ...operColumns];

      let groupData = groupBy(Data, { key: ['PERIOD'] });
      let newDatas = new Array(21);
      Object.keys(groupData).forEach((period) => {
        groupData[period].forEach((item, idx) => {
          let isYield = item.KPI_ID.indexOf('YIELD') >= 0;
          let kpiName = isYield ? '量產FPY監控' : item.KPI_ID.replace(/DEFECT[0-4]_/g, '');
          let topName = '';
          switch (item.KPI_ID) {
            case 'YIELD0_TTL':
              topName = 'TTL Yield';
              break;
            case 'YIELD1_FPT':
              topName = 'FPT Yield';
              break;
            case 'YIELD2_PCBI':
              topName = 'PCBI';
              break;
            case 'YIELD3_LAM':
              topName = 'LAM';
              break;
            case 'YIELD4_ACDP':
              topName = 'ACDP';
              break;
            case 'YIELD5_SHIP':
              topName = '1910 Shipping Qty';
              break;
            default:
              topName = 'Top ' + item.TOP;
          }
          let rowData = { KPI: kpiName, TOP: topName, ISYIELD: isYield };
          newDatas[idx] = newDatas[idx] || { ...rowData };
          newDatas[idx][period] = Math.round(item.VAL * 1000) / 1000;
          newDatas[idx][period + '-DFCT_CODE'] = item.ERRC_DESCR;
          newDatas[idx][period + '-REMARK'] = item.REMARK;
          newDatas[idx][period + '-LIGHT'] = item.LIGHT;
          newDatas[idx][period + '-SEQ'] = item.SEQ;
        });
      });
      newDatas.unshift(firstRow);

      PageSetting.TableSize.x = 650 + DateColumns.length * 100;
      PageSetting.Columns = [...DateColumns]; //vue3或者adv无法监测push,shift等方法
      PageSetting.moreData = getTableData(newDatas);

      if (PageSetting.moreData.length > 100) {
        PageSetting.List = PageSetting.moreData.slice(0, 80);
      } //预先加载80条数据
      else {
        PageSetting.List = PageSetting.moreData;
      }

      PageSetting.LightColumns = [...LightColumns];
      LightData.unshift({
        DESC: 'Target說明',
        FPTYIELD: 'FPT Yield',
        PCBI: 'PCBI',
        LAM: 'LAM',
        ACDP: 'ACDP',
      });
      PageSetting.LightList = LightData;

      if (values['Interval'] == 'Date') {
        PageSetting.Top5Hidden = false;

        PageSetting.Top5Columns = [...TOP5Columns];
        Top5Data.unshift({
          KPI_ID: 'DEFECT2_PCBI',
          TOP: 'TOP',
          PROD_NBR: 'Product',
          ERRC_DESCR: 'Defect',
          QTY: 'Qty',
        });
        Top5Data.unshift({
          KPI_ID: 'DEFECT3_LAM',
          TOP: 'TOP',
          PROD_NBR: 'Product',
          ERRC_DESCR: 'Defect',
          QTY: 'Qty',
        });
        Top5Data.unshift({
          KPI_ID: 'DEFECT4_ACDP',
          TOP: 'TOP',
          PROD_NBR: 'Product',
          ERRC_DESCR: 'Defect',
          QTY: 'Qty',
        });
        PageSetting.PCBIList = Top5Data.filter((x) => x.KPI_ID == 'DEFECT2_PCBI');
        PageSetting.PCBITitle = 'PCBI input Qty : ' + (PageSetting.PCBIList[1]?.TOTAL_QTY || 0);
        PageSetting.LAMList = Top5Data.filter((x) => x.KPI_ID == 'DEFECT3_LAM');
        PageSetting.LAMTitle = 'LAM input Qty : ' + (PageSetting.LAMList[1]?.TOTAL_QTY || 0);
        PageSetting.ACDPList = Top5Data.filter((x) => x.KPI_ID == 'DEFECT4_ACDP');
        PageSetting.ACDPTitle = 'ACDP input Qty : ' + (PageSetting.ACDPList[1]?.TOTAL_QTY || 0);
      } else {
        PageSetting.Top5Hidden = true;
      }
      closeFullLoading();
    });
  }

  /**
   * 进行数据处理,将表格进行行列转置并进行表格合并
   * @Data 表格数据源
   */
  function getTableData(Data: any[]) {
    /**表格合并所需参数
     * @data 数据源
     * @mergeColNames 需要合并的列
     * @firstMergeColNames 受到影响的列
     * @firstMerge 以哪列为基础进行合并
     */
    let combineCol: any = {
      data: Data,
      mergeColNames: ['KPI'],
      firstMergeColNames: ['KPI'],
      firstMerge: 'KPI',
    };
    return mergeTableRow(combineCol);
  }
  function getRowClassNmae(record: Object) {
    if (record && Object.keys(record).length != 0) {
      if (record['CLASSNAME']) {
        return record['CLASSNAME'];
      }
    }
  }
  var groupBy = (data, option) => {
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

  InfiniteScroll(Top5DefectCodeNewTable, PageSetting);

  onMounted(async () => {
    await mountedTreeLCMOwner(optionsdata, UseParamsAdd.Fab);
    await mountedTNSize4(optionsdata, UseParamsAdd.Fab);
    
  });
</script>
<style lang="less">
  :deep(td.v-center) {
    border: 1px solid black !important;
  }

  .swapper {
    display: flex;
    flex-direction: column;
  }
  #app .tabs-view .tabs-view-content{
    height: calc(100% - 80px);
    padding: 10px 0 0 8px;
    overflow: hidden;
  }
</style>
