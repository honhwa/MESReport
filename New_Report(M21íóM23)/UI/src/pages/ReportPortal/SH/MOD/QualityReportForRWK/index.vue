<template>
  <div class="QualityReport">
    <a-card>
      <a-row>
        <a-col :span="24 - PageParams.TableSpan" Source-show="PageSetting.showmenu">
          <div class="SHScrollForm">
            <SchemaForm @submit="confirm"> </SchemaForm>
          </div>
        </a-col>
        <a-col :span="PageParams.TableSpan">
          <div class="ScrollTable" ref="QualityTable">
            <DynamicTable
              sticky
              showFavorite
              :PageParams="PageParams"
              id="QualityReport"
              export-type="Table"
              :header-title="PageSetting.Title"
              :columns="PageSetting.Columns"
              :key="PageSetting.Columns"
              bordered
              size="small"
              :scroll="{ x: PageSetting.x }"
              :data-source="PageSetting.List"
              :pagination="false"
              export-file-name="导出"
              :row-class-name="getRowClassName_Sel"
            >
              <template #export-button>
                <a-button type="success"> 导出 </a-button>
              </template>
            </DynamicTable>
          </div>
          <div hidden>{{ PageSetting.SQLHidden }}</div>
        </a-col>
      </a-row>
    </a-card>
  </div>
</template>

<script lang="tsx" setup>
  import { useForm } from '@/components/core/schema-form';
  import { useTable } from '@/components/core/dynamic-table';
  import { reactive, onMounted, ref } from 'vue';
  import { cloneDeep } from 'lodash-es';
  import useRemoteSelect from '@/pages/ReportPortal/utils/SHuseRemoteSelect';
  import { getrowsum, Inserttotalrow } from '@/pages/ReportPortal/utils/SHcommon';
  import { message } from 'ant-design-vue';
  import type { TableColumn } from '@/components/core/dynamic-table';
  import { GetCommon } from '@/api/pages/common';
  import { AddCommonPageParams } from '@/store/module/commonpage';
  import { useRouter, useRoute } from 'vue-router';
  import { Console } from 'console';
  import { useLoading } from '@/components/feat/Loading';
  import { PageParams } from '@/components/core/dynamic-table/src/dynamic-table';
  import { ReportCommon } from '@/store/module/ReportCommon';
  import { PovitParams, getPovitData } from '@/utils/reportcommon/provit';
import { tableProps } from 'ant-design-vue/lib/table';

  const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...',
  });
  const useCommonParams = AddCommonPageParams();
  let useReportCommon = ReportCommon();
  let bigfab = useCommonParams.Fab.slice(0, useCommonParams.Fab.length - 1);
  let fab = 'LCM';
  if (useCommonParams.Fab == 'LCM1' || useCommonParams.Fab == 'LCM3') {
    fab = useCommonParams.Fab;
  }
  const QualityTable = ref<HTMLElement | null>(null);

  const router = useRouter();
  let ReportItem = reactive({
    // 首頁後端返回的sql以及sql參數
    APIMessage: '',
    // 左側請求參數
    FormParams: {},
    // 二級頁面所需參數
    FirstTableParams: {},
    // 其餘頁面所需參數
    SecondTableParams: reactive({}),
    ThirdTableParams: {},
    ThirdTableClickParams: {},
    //廠別
    Fab: '',
    G_FAC_COM: '',
  });
  let ReportNameParams = `QualityReportForRWK-${useCommonParams.Fab}`;
  useReportCommon.AddObject(ReportNameParams, ReportItem);

  // 頁面數據
  let PageSetting = reactive({
    List: [] as any,
    Columns: [] as any,
    Message: {},
    Title: '',
    moreData: [] as any,
    x: '' as any,
    SQLHidden: '',
    APIColumns: [] as any,
  });

  function getImport(site: any) {
    //let { optionsdata } = require('@/pages/ReportPortal/Static/SHCommon-Form-schema');
    const {
      schemas,
      GrpOptions,
    } = require(`@/pages/ReportPortal/SH/MOD/QualityReportForRWK/${fab}/form-schema`);
    return { schemas, GrpOptions };
  }
  let { schemas, GrpOptions } = getImport(useCommonParams.Fab);
  const [DynamicTable] = useTable();
  const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 130,
    schemas: schemas(useCommonParams.Fab, reactive({})),
    actionColOptions: {
      span: 24,
    },
    layout: 'vertical',
  });
  const { InfiniteScroll } = useRemoteSelect();

  async function confirm(values) {

    if (values.FAC == 'LCM2') {
      useCommonParams.Fab = 'LCM2';
      useCommonParams.G_FAC_COM = 'B';
    }
    if (values.FAC == 'LCM1') {
      useCommonParams.Fab = 'LCM1';
      useCommonParams.G_FAC_COM = 'C';
    }
    if (values.FAC == 'LCM4') {
      useCommonParams.Fab = 'LCM4';
      useCommonParams.G_FAC_COM = 'D';
    }
    if (values.FAC == 'LCM3') {
      useCommonParams.Fab = 'LCM3';
      useCommonParams.G_FAC_COM = 'E';
    }

    openFullLoading();
    PageSetting.List = [];
    PageSetting.Columns = [];

    Object.keys(values).forEach((key) => {
      if (!values[key]) {
        values[key] = '';
      }
      if (['WorkOrderLike', 'Operator'].includes(key)) {
        values[key] = values[key].split(',');
      }
    });

    if (values.Prod_nbr && values.Prod_nbr.length > 1000) {
      closeFullLoading();
      return message.error('料號输入超過 “最大限制筆數:1000” ');
    }

    if (values.ReworkedOption > 0 && values.ReworkedOption.length > 0) {
      if (values.ReworkedTimes && values.ReworkedTimes.length > 0) {
        if (values.OperationForRWK.length !== 1) {
          closeFullLoading();
          return message.error('請選擇一個站點');
        }
      } else {
        if (values.OperationForRWK.length == 1) {
          closeFullLoading();
          return message.error('請選擇重工次數');
        } else {
          closeFullLoading();
          return message.error('請選擇一個站點和重工次數');
        }
      }
    }

    PageSetting.Title = `${useCommonParams.Fab} Quality Report`;

    let ApiNamePara = '';
    // if(values.ShowMode=="1300to1600"){
    //     ApiNamePara="Quality1300to1600DataSet"
    // }else{
    //     ApiNamePara="QualityDataSet"
    // }
    ApiNamePara = 'QualityForRWKDataSet_MOD';

    let TopTableParams = {
      name: 'Date',
      apiName: ApiNamePara,
      FactoryType: useCommonParams.Fab,
      G_FAC: useCommonParams.G_FAC_COM,
    };

    TopTableParams = { ...TopTableParams, ...values };
console.log('TopTableParams',TopTableParams)

    try {
      const {
        Date: {
          Data: { Table: Data }, //前6行的组成,表头的组成
          Data_2: { Table: Data_2 },

          SQL01,
          SQL03,
          cuttime,
          lstr_ShowMode,
          lstr_Operation,
          sql_WoType,
          FromDate,
          ToDate,
          lstr_PROD_NBR,
          ShowMode,
          StrSql_w,
          StrSql_w1,
          StrSql_wf,
          StrSql_w3,
          rework,
          guolv,
          stroper,
          FAST_RWK_FLG_type,
        },
      } = await GetCommon(TopTableParams);

      PageSetting.SQLHidden = ApiNamePara + '-SQLHidden SQL01:' + SQL01 + ';<END> SQL02:' + SQL03;

      if (Data.length == 0 && Data_2.length == 0) {
        closeFullLoading();
        return message.warn('无数据！');
      }

      let Message = {
        lstr_ShowMode: lstr_ShowMode,
        lstr_Operation: lstr_Operation,
        sql_WoType: sql_WoType,
        FromDate: FromDate,
        ToDate: ToDate,
        ShowMode: ShowMode,
        StrSql_w: StrSql_w,
        StrSql_w1: StrSql_w1,
        StrSql_wf: StrSql_wf,
        lstr_PROD_NBR: lstr_PROD_NBR,
        rework: rework,
        guolv: guolv,
        stroper: stroper,
        FAST_RWK_FLG_type: FAST_RWK_FLG_type,
        StrSql_w3: StrSql_w3,
      };

      let baseColumns: any[] = [
        {
          title: 'process name',
          align: 'left',
          dataIndex: `DFCT_CODE`,
          key: `DFCT_CODE`,
          className: 'v-center',
          fixed: 'left',
        },
        {
          title: 'process name',
          align: 'left',
          dataIndex: `ERRC_DESCR`,
          key: `ERRC_DESCR`,
          className: 'v-center',
          fixed: 'left',
        },
      ];
      let finalData: object[] = [];

      // PROD_NBR记录
      let prodobj = {
        DFCT_CODE: 'PROD_NBR',
        ERRC_DESCR: 'PROD_NBR',
        'TOTAL YIELD QTY': '+PD',
        'TOTAL YIELD QTY1': '+PD',
        'TOTAL YIELD RATE': '-PD',
        'TOTAL YIELD RATE1': '-PD',
      };

      // inspection qty记录
      let inspectionobj = {
        DFCT_CODE: 'INSPECTION QTY',
        ERRC_DESCR: 'INSPECTION QTY',
        'TOTAL YIELD QTY': '+PD',
        'TOTAL YIELD QTY1': '+PD',
        'TOTAL YIELD RATE': '-PD',
        'TOTAL YIELD RATE1': '-PD',
      };
      let good = {
        DFCT_CODE: 'GOOD',
        ERRC_DESCR: 'GOOD',
        'TOTAL YIELD QTY': 0,
        'TOTAL YIELD QTY1': 0,
        'TOTAL YIELD RATE': 0,
        'TOTAL YIELD RATE1': 0,
      };
      let ng = {
        DFCT_CODE: 'NG',
        ERRC_DESCR: 'NG',
        'TOTAL YIELD QTY': 0,
        'TOTAL YIELD QTY1': 0,
        'TOTAL YIELD RATE': 0,
        'TOTAL YIELD RATE1': 0,
      };
      let yieldrate = {
        DFCT_CODE: 'YIELD Rate',
        ERRC_DESCR: 'YIELD Rate',
        'TOTAL YIELD QTY': 1,
        'TOTAL YIELD QTY1': 1,
        'TOTAL YIELD RATE': 1,
        'TOTAL YIELD RATE1': 1,
      };
      let yieldloss = {
        DFCT_CODE: 'YIELD LOSS',
        ERRC_DESCR: 'YIELD LOSS',
        'TOTAL YIELD QTY': 1,
        'TOTAL YIELD QTY1': 1,
        'TOTAL YIELD RATE': 1,
        'TOTAL YIELD RATE1': 1,
      };
      let defectitem = {
        DFCT_CODE: 'defect ITEM',
        ERRC_DESCR: 'defect ITEM',
        'TOTAL YIELD QTY': 'QTY',
        'TOTAL YIELD QTY1': 'QTY',
        'TOTAL YIELD RATE': '%',
        'TOTAL YIELD RATE1': '%',
      };

      let totalyieldsum = 0;
      let middleProps: any[] = [];
      let middlerateProps: any[] = [];
      let middlesecondqtyprops: any[] = [];
      Data.forEach((item) => {
        if (values.ShowMode == 'equip_nbr') {
          middleProps.push(`${item['WORK_CTR']}-${item['PROD_NBR']}`);
          middlerateProps.push(`${item['WORK_CTR']}-${item['PROD_NBR']}RATE`);
          middlesecondqtyprops.push(`${item['WORK_CTR']}-${item['PROD_NBR']}-SECOND_QTY`);

          baseColumns.push(
            {
              title: item['WORK_NAME'],
              align: 'center',
              dataIndex: `${item['WORK_CTR']}-${item['PROD_NBR']}`,
              key: `${item['WORK_CTR']}-${item['PROD_NBR']}QTY`,
            },
            {
              title: item['WORK_NAME'],
              align: 'center',
              dataIndex: `${item['WORK_CTR']}-${item['PROD_NBR']}RATE`,
              key: `${item['WORK_CTR']}-${item['PROD_NBR']}RATE`,
            },
          );
          totalyieldsum += item['QTY'];

          prodobj[`${item['WORK_CTR']}-${item['PROD_NBR']}`] = item['PROD_NBR'];
          prodobj[`${item['WORK_CTR']}-${item['PROD_NBR']}RATE`] = item['PROD_NBR'];

          inspectionobj[`${item['WORK_CTR']}-${item['PROD_NBR']}`] = item['QTY'];
          inspectionobj[`${item['WORK_CTR']}-${item['PROD_NBR']}RATE`] = item['QTY'];
          ng[`${item['WORK_CTR']}-${item['PROD_NBR']}`] = item['REJECT_QTY'];
          ng[`${item['WORK_CTR']}-${item['PROD_NBR']}RATE`] = item['REJECT_QTY1'];

          good[`${item['WORK_CTR']}-${item['PROD_NBR']}`] = item['QTY'] - item['REJECT_QTY'];
          good[`${item['WORK_CTR']}-${item['PROD_NBR']}RATE`] = item['QTY'] - item['REJECT_QTY1'];
          yieldrate[`${item['WORK_CTR']}-${item['PROD_NBR']}`] = (
            ((item['QTY'] - item['REJECT_QTY']) / item['QTY']) *
            100
          ).toFixed(3);
          yieldrate[`${item['WORK_CTR']}-${item['PROD_NBR']}RATE`] = (
            ((item['QTY'] - item['REJECT_QTY1']) / item['QTY']) *
            100
          ).toFixed(3);
          yieldloss[`${item['WORK_CTR']}-${item['PROD_NBR']}`] = (
            (item['REJECT_QTY'] / item['QTY']) *
            100
          ).toFixed(3);
          yieldloss[`${item['WORK_CTR']}-${item['PROD_NBR']}RATE`] = (
            (item['REJECT_QTY1'] / item['QTY']) *
            100
          ).toFixed(3);
          defectitem[`${item['WORK_CTR']}-${item['PROD_NBR']}`] = 'QTY';
          defectitem[`${item['WORK_CTR']}-${item['PROD_NBR']}RATE`] = '%';

          good['TOTAL YIELD QTY'] += good[`${item['WORK_CTR']}-${item['PROD_NBR']}`];
          good['TOTAL YIELD RATE'] += good[`${item['WORK_CTR']}-${item['PROD_NBR']}RATE`];
          ng['TOTAL YIELD QTY'] += ng[`${item['WORK_CTR']}-${item['PROD_NBR']}`];
          ng['TOTAL YIELD RATE'] += ng[`${item['WORK_CTR']}-${item['PROD_NBR']}RATE`];

          good['TOTAL YIELD QTY1'] += good[`${item['WORK_CTR']}-${item['PROD_NBR']}`];
          good['TOTAL YIELD RATE1'] += good[`${item['WORK_CTR']}-${item['PROD_NBR']}RATE`];
          ng['TOTAL YIELD QTY1'] += ng[`${item['WORK_CTR']}-${item['PROD_NBR']}`];
          ng['TOTAL YIELD RATE1'] += ng[`${item['WORK_CTR']}-${item['PROD_NBR']}RATE`];
        } else {
          middleProps.push(item['WORK_CTR']);
          middlerateProps.push(`${item['WORK_CTR']}RATE`);
          middlesecondqtyprops.push(`${item['WORK_CTR']}-SECOND_QTY`);

          baseColumns.push(
            {
              title: item['WORK_NAME'],
              align: 'center',
              dataIndex: `${item['WORK_CTR']}`,
              key: `${item['WORK_CTR']}QTY`,
            },
            {
              title: item['WORK_NAME'],
              align: 'center',
              dataIndex: `${item['WORK_CTR']}RATE`,
              key: `${item['WORK_CTR']}RATE`,
            },
          );
          totalyieldsum += item['QTY'];
          inspectionobj[`${item['WORK_CTR']}`] = item['QTY'];
          inspectionobj[`${item['WORK_CTR']}RATE`] = item['QTY'];

          ng[`${item['WORK_CTR']}`] = item['REJECT_QTY'];
          ng[`${item['WORK_CTR']}RATE`] = item['REJECT_QTY1'];

          good[`${item['WORK_CTR']}`] = item['QTY'] - item['REJECT_QTY'];
          good[`${item['WORK_CTR']}RATE`] = item['QTY'] - item['REJECT_QTY1'];
          yieldrate[`${item['WORK_CTR']}`] = (
            ((item['QTY'] - item['REJECT_QTY']) / item['QTY']) *
            100
          ).toFixed(3);
          yieldrate[`${item['WORK_CTR']}RATE`] = (
            ((item['QTY'] - item['REJECT_QTY1']) / item['QTY']) *
            100
          ).toFixed(3);
          yieldloss[`${item['WORK_CTR']}`] = ((item['REJECT_QTY'] / item['QTY']) * 100).toFixed(3);
          yieldloss[`${item['WORK_CTR']}RATE`] = (
            (item['REJECT_QTY1'] / item['QTY']) *
            100
          ).toFixed(3);
          defectitem[`${item['WORK_CTR']}`] = 'QTY';
          defectitem[`${item['WORK_CTR']}RATE`] = '%';
          good['TOTAL YIELD QTY'] += good[`${item['WORK_CTR']}`];
          good['TOTAL YIELD RATE'] += good[`${item['WORK_CTR']}RATE`];
          ng['TOTAL YIELD QTY'] += ng[`${item['WORK_CTR']}`];
          ng['TOTAL YIELD RATE'] += ng[`${item['WORK_CTR']}RATE`];
          if (item['WORK_CTR'] !== 2610 && item['WORK_CTR'] !== 2110) {
            good['TOTAL YIELD QTY1'] += good[`${item['WORK_CTR']}`];
            good['TOTAL YIELD RATE1'] += good[`${item['WORK_CTR']}RATE`];
            ng['TOTAL YIELD QTY1'] += ng[`${item['WORK_CTR']}`];
            ng['TOTAL YIELD RATE1'] += ng[`${item['WORK_CTR']}RATE`];
          }
        }
      });

      middleProps.forEach((item) => {
        if (Number(yieldrate[item]) !== 0) {
          yieldrate['TOTAL YIELD QTY'] *= Number(yieldrate[item]) / 100;
          yieldrate['TOTAL YIELD QTY1'] *= Number(yieldrate[item]) / 100;
        }
      });

      middlerateProps.forEach((item) => {
        if (Number(yieldrate[item]) !== 0) {
          yieldrate['TOTAL YIELD RATE'] *= Number(yieldrate[item]) / 100;
          yieldrate['TOTAL YIELD RATE1'] *= Number(yieldrate[item]) / 100;
        }
      });

      yieldrate['TOTAL YIELD QTY'] = Number(yieldrate['TOTAL YIELD QTY'].toFixed(5)) * 100;
      yieldrate['TOTAL YIELD QTY'] = Number(yieldrate['TOTAL YIELD QTY'].toFixed(3));

      yieldrate['TOTAL YIELD RATE'] = Number(yieldrate['TOTAL YIELD RATE'].toFixed(5)) * 100;
      yieldrate['TOTAL YIELD RATE'] = Number(yieldrate['TOTAL YIELD RATE'].toFixed(3));

      yieldloss['TOTAL YIELD QTY'] = Number((100 - yieldrate['TOTAL YIELD QTY']).toFixed(3));
      yieldloss['TOTAL YIELD RATE'] = Number((100 - yieldrate['TOTAL YIELD RATE']).toFixed(3));
      // 统一添加百分号
      [...middleProps, ...middlerateProps, 'TOTAL YIELD QTY', 'TOTAL YIELD RATE'].forEach(
        (item) => {
          yieldrate[item] = yieldrate[item] + '%';
          yieldloss[item] = yieldloss[item] + '%';
        },
      );

      yieldrate['TOTAL YIELD QTY1'] = Number(yieldrate['TOTAL YIELD QTY1'].toFixed(5)) * 100;
      yieldrate['TOTAL YIELD QTY1'] = Number(yieldrate['TOTAL YIELD QTY1'].toFixed(3));

      yieldrate['TOTAL YIELD RATE1'] = Number(yieldrate['TOTAL YIELD RATE1'].toFixed(5)) * 100;
      yieldrate['TOTAL YIELD RATE1'] = Number(yieldrate['TOTAL YIELD RATE1'].toFixed(3));

      yieldloss['TOTAL YIELD QTY1'] = Number((100 - yieldrate['TOTAL YIELD QTY1']).toFixed(3));
      yieldloss['TOTAL YIELD RATE1'] = Number((100 - yieldrate['TOTAL YIELD RATE1']).toFixed(3));
      // 统一添加百分号
      ['TOTAL YIELD QTY1', 'TOTAL YIELD RATE1'].forEach((item) => {
        yieldrate[item] = yieldrate[item] + '%';
        yieldloss[item] = yieldloss[item] + '%';
      });

      if (values.ShowMode == 'equip_nbr') {
        finalData.push(prodobj, inspectionobj, good, ng, yieldrate, yieldloss, defectitem);
      } else {
        finalData.push(inspectionobj, good, ng, yieldrate, yieldloss, defectitem);
      }
      // Data_2添加属性
      Data_2.forEach((item) => {
        if (values.ShowMode == 'equip_nbr') {
          item['WORK_CTR'] = `${item['WORK_CTR']}-${item['PROD_NBR']}`;
        }
        item[`${item['WORK_CTR']}-SECOND_QTY`] = item['SECOND_QTY'];
      });

      let cc = Data_2;
      console.log('cc', cc);

      // Data_2處理,行列轉換
      // 表格数据
      let config: PovitParams = {
        Data: Data_2,
        povitColumn: {
          WORK_CTR: 'MAIN_QTY',
        },
        changeColumn: middleProps,
        repeatColumn: ['DFCT_CODE', 'ERRC_DESCR'],
      };

      let povitData = getPovitData(config);
      // console.log(povitData,'povitData');
      // // 計算屬於什麼品質
      povitData.forEach((item) => {
        item['condition'] = item['DFCT_CODE'].substr(2, 1);
      });
      let tempmiddleProps = [...middleProps, ...middlerateProps];
      for (let i = 0; i < tempmiddleProps.length; i++) {
        tempmiddleProps[i] = String(tempmiddleProps[i]);
      }

      //列计算
      getrowsum(povitData, middleProps, 'TOTAL YIELD QTY');
      getrowsum(povitData, middleProps, 'TOTAL YIELD QTY1');
      let { insertrowList, insertIndexList } = Inserttotalrow(
        povitData,
        ['condition'],
        [...tempmiddleProps, ...middlesecondqtyprops, 'TOTAL YIELD QTY1', 'TOTAL YIELD QTY'],
        'DFCT_CODE',
        '總計',
        false,
        false,
      );

      // 数组头部添加新的值
      insertIndexList.unshift(0);
      // 数组头部移除最后一位数字
      insertIndexList.pop();
      // 插入之后的位置
      let afterinsert: number[] = [];
      for (let i = 0; i < insertIndexList.length; i++) {
        povitData.splice(insertIndexList[i] + i, 0, insertrowList[i]);
        afterinsert.push(insertIndexList[i] + i);
      }

      afterinsert.forEach((item) => {
        povitData[item]['DFCT_CODE'] = `${povitData[item + 1]['condition']}品`;
        povitData[item]['ERRC_DESCR'] = `${povitData[item + 1]['condition']}品`;
      });

      povitData.forEach((item) => {
        // 當前行倒數第二列的括號計算方式
        let totalqty = 0;
        middleProps.forEach((titem, index) => {
          if (item[titem] != null) {
            // 计算从第6行开始的百分号那一列的计算
            if (values.ShowMode == 'equip_nbr') {
              item[middlerateProps[index]] = Number(
                ((item[titem] / finalData[1][titem]) * 100).toFixed(3),
              );
            } else {
              item[middlerateProps[index]] = Number(
                ((item[titem] / finalData[0][titem]) * 100).toFixed(3),
              );
            }

            // 添加括号（）
            if (item[`${titem}-SECOND_QTY`] != undefined) {
              item[titem] = `${item[titem]}(${item[`${titem}-SECOND_QTY`]})`;
              totalqty += Number(`${item[`${titem}-SECOND_QTY`]}`);
            } else {
              item[titem] = `${item[titem]}(0)`;
            }
          }
        });

        // 最後一列的計算公式
        item['TOTAL YIELD RATE'] =
          (item['TOTAL YIELD QTY'] / ng['TOTAL YIELD QTY']) *
          Number(String(yieldloss['TOTAL YIELD QTY']).replace('%', ''));
        item['TOTAL YIELD RATE'] = item['TOTAL YIELD RATE'].toFixed(3);
        item['TOTAL YIELD QTY'] = `${item['TOTAL YIELD QTY']}(${totalqty})`;
        if (item['WORK_CTR'] !== 2610 && item['WORK_CTR'] !== 2110) {
          item['TOTAL YIELD RATE1'] =
            (item['TOTAL YIELD QTY1'] / ng['TOTAL YIELD QTY1']) *
            Number(String(yieldloss['TOTAL YIELD QTY1']).replace('%', ''));
          item['TOTAL YIELD RATE1'] = item['TOTAL YIELD RATE1'].toFixed(3);
          item['TOTAL YIELD QTY1'] = `${item['TOTAL YIELD QTY1']}(${totalqty})`;
        }
      });

      finalData.push(...povitData);

      baseColumns.push(
        {
          title: 'TOTAL YIELD(不含2610,2110)',
          align: 'center',
          dataIndex: `TOTAL YIELD QTY1`,
          key: `TOTAL YIELD QTY1`,
        },
        {
          title: 'TOTAL YIELD(不含2610,2110)',
          align: 'center',
          dataIndex: `TOTAL YIELD RATE1`,
          key: `TOTAL YIELD RATE1`,
        },
        {
          title: 'TOTAL YIELD',
          align: 'center',
          dataIndex: `TOTAL YIELD QTY`,
          key: `TOTAL YIELD QTY`,
        },
        {
          title: 'TOTAL YIELD',
          align: 'center',
          dataIndex: `TOTAL YIELD RATE`,
          key: `TOTAL YIELD RATE`,
        },
      );

  //   function getRowClassName_Sel(record: Object) {
  //   if (record && Object.keys(record).length != 0) {
  //     for (const [key, value] of Object.entries(record)) {
  //       if (typeof value == 'string' && value.includes('品')) {
  //         return 'Green';
  //       } else if (typeof value == 'string' && value.includes('defect ITEM')) {
  //         return 'v-center';
  //       } else if (typeof value == 'string' && value.includes('INSPECTION QTY')) {
  //         return 'Purple';
  //       } else if (typeof value == 'string' && ['GOOD', 'YIELD Rate'].includes(value)) {
  //         return 'GrayBlue';
  //       } else if (typeof value == 'string' && ['NG', 'YIELD LOSS'].includes(value)) {
  //         return 'YellowGray';
  //       }
  //       let ClickColumn = (['NG', 'YIELD LOSS'].includes(value));
  //     }
  //   }
  // }
  //...tempmiddleProps
        
        let ClickColumn = [...tempmiddleProps,'TOTAL YIELD QTY', 'TOTAL YIELD RATE'];
      // 合并表头
      baseColumns.forEach((item, index) => {
        if (index % 2 === 0) {
          item['colSpan'] = 2;
        } else {
          item['colSpan'] = 0;
        }
        // item["title"] = ClickColumn.includes(item["dataIndex"]) ? `${item["title"]}*` : item["title"];//可点击的栏位显示*号
        item['customCell'] = (record: Object, rowIndex: number, column: Object) => {
          let obj = {};
          // console.log("record",record)
          // console.log("column",column)

          // 双击功能功能
          let stylefunc = () => {
            let tempStyle = {};
            // ng[`${item['WORK_CTR']}-${item['PROD_NBR']}`] = item['REJECT_QTY'];
           // if (ClickColumn.includes(item['dataIndex'])) {
            if (ClickColumn.includes(item['dataIndex'])) {
              tempStyle = { cursor: 'pointer' };
            } else {
              tempStyle = {};
            }
            return { ...tempStyle };
            
          };
         
          (obj['onDblclick'] = async () => {
            if ( ClickColumn.includes(item['dataIndex']) )           
            {
           
              ReportItem.APIMessage = JSON.stringify(Message);
              ReportItem.FirstTableParams = { ...record, ...column['__originColumn__'] };
              ReportItem.FormParams = TopTableParams;
              
             

              // router.push({ name: `QualityReportForRWKDetail-MOD1` });
              if (useCommonParams.Fab == 'LCM1' || useCommonParams.Fab == 'LCM2') {
                  router.push({ name: `QualityReportForRWKDetail-MOD1` });
                }
                if (useCommonParams.Fab == 'LCM3' || useCommonParams.Fab == 'LCM4') {
                  router.push({ name: `QualityReportForRWKDetail-MOD2` });
              }

              //handclick(record, rowIndex, column, item, index);
            
            }

            
            else {
              message.warning('此栏位没有二阶数据');
            }

            
          }),
            (obj['style'] = stylefunc());

          if (values.ShowMode == 'equip_nbr') {
            if (rowIndex < 6) {
              if (column['key'].includes('QTY') && !column['key'].includes('TOTAL')) {
                return {
                  colSpan: 2, // 跨几列合并，这里现在是跨两列
                  ...obj,
                };
              } else if (column['key'].includes('RATE') && !column['key'].includes('TOTAL')) {
                return {
                  colSpan: 0,
                  ...obj,
                };
              } else if (column['key'].includes('DFCT_CODE')) {
                return {
                  colSpan: 2,
                  ...obj,
                };
              } else if (column['key'].includes('ERRC_DESCR')) {
                return {
                  colSpan: 0,
                  ...obj,
                };
              } else {
                if (rowIndex == 2) {
                  if (column['key'].includes('TOTAL')) {
                    return {
                      ...obj,
                    };
                  }
                }
              }
            } else if (rowIndex == 6) {
              if (column['key'].includes('DFCT_CODE')) {
                return {
                  colSpan: 2,
                  ...obj,
                };
              } else if (column['key'].includes('ERRC_DESCR')) {
                return {
                  colSpan: 0,
                  ...obj,
                };
              }
            } else if (rowIndex > 5) {
              return {
                ...obj,
              };
            }
          } else {
            if (rowIndex < 5) {
              if (column['key'].includes('QTY') && !column['key'].includes('TOTAL')) {
                return {
                  colSpan: 2, // 跨几列合并，这里现在是跨两列
                  ...obj,
                };
              } else if (column['key'].includes('RATE') && !column['key'].includes('TOTAL')) {
                return {
                  colSpan: 0,
                  ...obj,
                };
              } else if (column['key'].includes('DFCT_CODE')) {
                return {
                  colSpan: 2,
                  ...obj,
                };
              } else if (column['key'].includes('ERRC_DESCR')) {
                return {
                  colSpan: 0,
                  ...obj,
                };
              } else {
                if (rowIndex == 2) {
                  if (column['key'].includes('TOTAL')) {
                    return {
                      ...obj,
                    };
                  }
                }
              }
            } else if (rowIndex == 5) {
              if (column['key'].includes('DFCT_CODE')) {
                return {
                  colSpan: 2,
                  ...obj,
                };
              } else if (column['key'].includes('ERRC_DESCR')) {
                return {
                  colSpan: 0,
                  ...obj,
                };
              }
            } else if (rowIndex > 5) {
              return {
                ...obj,
              };
            }
          }
        };



      });

      PageSetting.Columns = baseColumns;

      //PageSetting.moreData = finalData;
      PageSetting.List = finalData;

      // //console.log("最終的數據", finalData);
      // if (PageSetting.moreData.length > 100 && values.ShowMode!="work_ctr") {
      //     //预先加载40条数据
      //     PageSetting.List = PageSetting.moreData.slice(0, 30);
      // }
      // else {
      //     PageSetting.List = PageSetting.moreData;
      // }

      PageSetting.x = null;
      if (PageSetting.Columns.length > 7) {
        PageSetting.x = 90 * PageSetting.Columns.length;
      }
      closeFullLoading();
    } catch (error) {
      closeFullLoading();
    }
  }

  function getRowClassName_Sel(record: Object) {
    if (record && Object.keys(record).length != 0) {
      for (const [key, value] of Object.entries(record)) {
        if (typeof value == 'string' && value.includes('品')) {
          return 'Green';
        } else if (typeof value == 'string' && value.includes('defect ITEM')) {
          return 'v-center';
        } else if (typeof value == 'string' && value.includes('INSPECTION QTY')) {
          return 'Purple';
        } else if (typeof value == 'string' && ['GOOD', 'YIELD Rate'].includes(value)) {
          return 'GrayBlue';
        } else if (typeof value == 'string' && ['NG', 'YIELD LOSS'].includes(value)) {
          return 'YellowGray';
        }
      }
    }
  }

  //实现无限滚动或者说懒加载
  InfiniteScroll(QualityTable, PageSetting);
  onMounted(async () => {
    // if (bigfab==="LCM"){
    //     await mounted(optionsdata,useCommonParams.Fab)
    //     await mountedSourceFab(optionsdata, useCommonParams.Fab);
    //     await mountedOper(optionsdata, useCommonParams.Fab);
    //     await mountedWoTypeBySap(optionsdata, useCommonParams.Fab);
    //     await mountedLCMowner(optionsdata, useCommonParams.Fab);
    //     await mountedLCDowner(optionsdata, useCommonParams.Fab);
    //     await mountedLCMBigEquipment(optionsdata, useCommonParams.Fab);
    //     await mountedLCMDefectCode(optionsdata, useCommonParams.Fab);
    //     await mountedLCDGrade(optionsdata, useCommonParams.Fab);
    //     await mountedModuleData(optionsdata, useCommonParams.Fab);
    // }else{
    // }
  });
</script>

<style lang="less" scoped>
  .btn-rows {
    button {
      margin-right: 12px;
    }
  }

  .swapper {
    display: flex;
    flex-direction: column;
  }
</style>
