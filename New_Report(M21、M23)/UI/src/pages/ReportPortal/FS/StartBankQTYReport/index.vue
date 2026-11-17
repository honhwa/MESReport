<script lang="ts" setup>
  import { PageParams } from '@/components/core/dynamic-table/src/dynamic-table';
  import { useForm } from '@/components/core/schema-form';
  import { useTable } from '@/components/core/dynamic-table';
  import { reactive, onMounted, ref } from 'vue';
  import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect';
  import { GetCommon } from '@/api/pages/common';
  import { ReportCommon } from '@/store/module/ReportCommon';
  import { mergeTableRow } from '@/pages/ReportPortal/utils/common';
  import { useLoading } from '@/components/feat/Loading';
  import { cloneDeep } from 'lodash-es';

  const { mounted } = useRemoteSelect();
  const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...',
  });
  const UseParamsAdd = ReportCommon();
  function getImport(site: any) {
    const {
      schemas,
      optionsdata,
    } = require(`@/pages/ReportPortal/FS/StartBankQTYReport/${site}/form-schema`);
    const {
      StartBankTable1Columns,
      StartBankTable2Columns,
    } = require(`@/pages/ReportPortal/FS/StartBankQTYReport/${site}/table`);
    return { schemas, optionsdata, StartBankTable1Columns, StartBankTable2Columns };
  }
  let { schemas, optionsdata, StartBankTable1Columns, StartBankTable2Columns } = getImport('LCM1');

  let PageSetting = reactive({
    List: [],
    APIColumns: [],
    Columns: StartBankTable1Columns,
    moreData: [],
    Table2Columns: StartBankTable2Columns,
    Table2Data: [] as any,
    Title: 'LCMA Start Bank Plan Quality',
    Title2: 'LCMA Start Bank No-Plan Quality ',
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
    // let dynamicColumns: TableColumn[] = []
    let baseParams = {
      apiName: 'StartBankPlanQualityDataSet',
      FactoryType: UseParamsAdd.Fab,
      G_FAC: 'A',
    };

    Object.keys(values).forEach((key) => {
      if (!values[key]) {
        values[key] = '';
      }
    });
    try {
      let { api01 } = await GetCommon({ ...baseParams, ...values });
      let Data = api01['Data']['Table'];
      let Data1 = Data.filter((item) => item['WO_NBR'] && item['WO_NBR'] != 'null');
      let NoPlanData = Data.filter((item) => !item['WO_NBR'] || item['WO_NBR'] == 'null');
      let sum = NoPlanData.reduce((prev, curr) => {
        return prev + curr.COUNT1;
      }, 0);
      let plansum = 0;
      Data1.forEach((item) => (plansum += item.COUNT1));

      NoPlanData.push({ COUNT1: sum, LCD_PROD: '總計' });
      PageSetting.Table2Data = NoPlanData;
      // megercell(PageSetting.Columns, ['WO_NBR', 'PROD_NBR'], Data);
      let combineCol1: any = {
        data: Data1,
        mergeColNames: ['WO_NBR', 'PROD_NBR'],
        firstMergeColNames: ['WO_NBR', 'PROD_NBR'],
        firstMerge: 'WO_NBR',
      };
      let pushdata = mergeTableRow(combineCol1);
      Insertrowcount(pushdata, ['WO_NBR'], 'WO_NBR');

      let findPanelData = cloneDeep(pushdata);
      // pushdata.forEach((i, index1) => {
      //     let tmp = i['LCD_PROD'];
      //     let m = pushdata.length;
      //     let tmpg = i['GRADE'];
      //     let test = '';
      //         let tmpw = '';
      //     if (tmp && tmp != '') {

      //         for (let k = index1; k < m; k++) {
      //             if (pushdata[k]['WO_NBR'].includes('小計')) {
      //                 tmpw = `(${pushdata[k]['WO_NBR'].split(' ')[1]})`;
      //                 break;
      //             }
      //             for (let j = 1; j < m; j++) {
      //             if (
      //                 i['PROD_NBR'] != pushdata[j]['PROD_NBR'] &&
      //                 pushdata[j]['LCD_PROD'] == tmp &&
      //                 pushdata[j]['GRADE'] == tmpg
      //             ) {
      //                 test =test+ tmpw + pushdata[j]['PROD_NBR'] + ';';
      //                 i['OTHERPROD'] = test;
      //                 console.log(test,'sss',pushdata[j]['PROD_NBR'] );
      //             }
      //         }
      //         }

      //     }
      // });
      for (let i = 1; i <= pushdata.length - 1; i++) {
        let other = '';
        if (pushdata[i]['LCD_PROD'] && pushdata[i]['LCD_PROD'] != '') {
          let tmp = pushdata[i]['LCD_PROD'];
          let m = pushdata.length;
          let tmpg = pushdata[i]['GRADE'];
          // let tmpw = '';
          // for (let k = i; k <= m; k++) {
          //     if (pushdata[k]['WO_NBR'].includes('小計'))  {
          //         tmpw = `(${pushdata[k]['WO_NBR'].split(' ')[1]})`;
          //     break;

          //     }
          // }
          for (let j = 1; j <= pushdata.length - 1; j++) {
            if (
              tmp != ' ' &&
              pushdata[i]['PROD_NBR'] != pushdata[j]['PROD_NBR'] &&
              pushdata[j]['LCD_PROD'] == tmp &&
              pushdata[j]['GRADE'] == tmpg
            ) {
              let tmpw = '';
              // for (let n = 0; n < pushdata[i]['WO_NBR-span']['rowspan']; n++) {
              if (pushdata[j + 1]['WO_NBR'].includes('小計')) {
                tmpw = `(${pushdata[j + 1]['WO_NBR'].split(' ')[1]})`;
                other += tmpw + pushdata[j]['PROD_NBR'] + ';';
              }
              // }
            }
          }
        }
        findPanelData[i]['OTHERPROD'] = other;
      }
      let SubName = findPanelData[findPanelData.length-1]['WO_NBR'].replace("小計","總計")
      SubName=SubName.replace("第","")
      findPanelData.push({
        COUNT1: plansum,
        WO_NBR: SubName,
        'WO_NBR-span': { rowspan: 1, colspan: 1 },
        'PROD_NBR-span': { rowspan: 1, colspan: 1 },
      });
    //   console.log(findPanelData);

      PageSetting.moreData = findPanelData;
      // PageSetting.moreData = Data.filter(item=>item['WO_NBR']&&item['WO_NBR']!='null')
      // PageSetting.Table2Data = Data.filter(item=>!item['WO_NBR']||item['WO_NBR']=='null');
      UseParamsAdd.AddFormParams(`StartBankQTYReport${UseParamsAdd.Fab}`, {
        ...baseParams,
        ...values,
      });
      UseParamsAdd.AddAPIResultMessage(`StartBankQTYReport${UseParamsAdd.Fab}`, api01['Message']);
      closeFullLoading();
    } catch (error) {
      console.error(error);
      closeFullLoading();
    }
  }
  // function handleData(){
  //     megercell(PageSetting.Columns, ['WO_NBR', 'PROD_NBR', 'E_QTY','NEED','BANKTARGET'], Data);
  // }
  function Insertrowcount(dataSource: any[], standard: any[], sumprops: string) {
    let temp: any = cloneDeep(dataSource[0]);
    let Data: any[] = cloneDeep(dataSource);
    let totalsum: any[] = [];
    let totalindex: any[] = [];
    let regArr: any[] = [];
    for (let i = 0; i < dataSource.length - 1; i++) {
      standard.forEach((item) => {
        regArr.push(dataSource[i][item] == dataSource[i + 1][item]);
      });
      if (regArr.includes(false)) {
        regArr = [];
        Object.keys(temp).forEach((item) => {
          temp[item] = '';
        });
        totalindex.push(i + 1);
        totalsum.push(temp);
        temp = cloneDeep(dataSource[i + 1]);
        continue;
      }
    }
    Object.keys(temp).forEach((item) => {
      temp[item] = '';
    });
    totalsum.push(temp);
    totalindex.push(dataSource.length);
    for (let i = 0; i < totalindex.length; i++) {
      totalsum[i][sumprops] = `小計:(第 ${i + 1} 張工單)`;
      let sum = 0;
      for (let j = totalindex[i - 1] ?? 0; j < totalindex[i]; j++) {
        sum += Data[j]['COUNT1'];
      }
      totalsum[i]['COUNT1'] = sum;
      dataSource.splice(totalindex[i] + i, 0, totalsum[i]);
    }
    return {
      insertrowList: totalsum,
      insertIndexList: totalindex,
    };
  }
  //实现无限滚动或者说懒加载
  onMounted(async () => {
    //获取Size,bigproduct,prodcut数据
    optionsdata = await mounted(optionsdata, UseParamsAdd.Fab);
  });
  const handleResizeColumn = (w, col) => {
    col.width = w;
  };
  function getRowClassNmae(record: Object) {
    if (record['WO_NBR'].includes('小計')) {
      return 'YellowBold';
    }
  }
</script>

<template>
  <a-row>
    <a-col :span="24 - PageParams.TableSpan" class="ScrollForm" v-show="PageParams.ShowMenu">
      <SchemaForm @submit="confirm"> </SchemaForm>
    </a-col>
    <a-col :span="PageParams.TableSpan">
      <DynamicTable
        showFavorite
        sticky
        :PageParams="PageParams"
        row-key="StartBankQTYReport"
        :header-title="PageSetting.Title"
        :columns="PageSetting.Columns"
        :key="PageSetting.Columns"
        export-type="Exceljs"
        bordered
        size="small"
        :data-source="PageSetting.moreData"
        :data-all="PageSetting.moreData"
        :row-class-name="getRowClassNmae"
        :pagination="false"
        export-file-name="导出"
        :scroll="{ y: 400 }"
        @resize-column="handleResizeColumn"
      >
        <!-- <template #toolbar>
                    <a-button type="primary" @click="openMenuModal({})"> 新增 </a-button>
                </template> -->
        <template #export-button>
          <a-button type="success"> 导出 </a-button>
        </template>
      </DynamicTable>
      <DynamicTable
        sticky
        :PageParams="PageParams"
        row-key="StartBankQTYReport"
        :header-title="PageSetting.Title2"
        :columns="PageSetting.Table2Columns"
        :key="PageSetting.Table2Columns"
        bordered
        size="small"
        :data-source="PageSetting.Table2Data"
        :data-all="PageSetting.Table2Data"
        :pagination="false"
        export-file-name="导出"
        :scroll="{ y: 200 }"
      >
        <!-- <template #toolbar>
                    <a-button type="primary" @click="openMenuModal({})"> 新增 </a-button>
                </template> -->
        <template #export-button>
          <a-button type="success"> 导出 </a-button>
        </template>
      </DynamicTable>
    </a-col>
  </a-row>
</template>
