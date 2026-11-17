<template>
  <div class="ToBeScrapResponsibility">
    <a-row>
      <a-col :span="24 - PageParams.TableSpan" class="ScrollForm" v-show="PageParams.ShowMenu">
        <SchemaForm @submit="confirm"> </SchemaForm>
      </a-col>
      <a-col :span="PageParams.TableSpan">
        <a-button type="primary" @click="changeView" v-show="PageSetting.TopList.length != 0">{{
          PageSetting.buttonText
        }}</a-button>
        <V3chart width="100%" height="90%" :options="PageSetting.EchartOptions" isClick @callbackFun="callbackFun"
          v-if="!PageSetting.isShowTable"></V3chart>
        <div v-else>
          <DynamicTable row-key="id" header-title="LCMA To Be Scrap 責任歸屬 ReportChart" showFavorite sticky
            :PageParams="PageParams" :columns="PageSetting.TopColumns" :key="PageSetting.TopColumns" bordered
            id="ToBeScrapResponsibility" :data-source="PageSetting.TopList" :data-all="PageSetting.TopList"
            export-type="Table" :pagination="false" export-file-name="导出" :row-class-name="getRowClassName"
            :flexwidth="1">
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
import { onMounted } from '@vue/runtime-core';
import { message } from 'ant-design-vue';
import { getfinalsum, Inserttotalrow } from '@/utils/reportcommon/Calculation';
import { cloneDeep } from 'lodash';
import V3chart from '@/components/charts/V3chart.vue';
import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { useLoading } from '@/components/feat/Loading';
import { megercell, TableRendering, antcustomCell, antdclick, antmegercell } from '@/pages/ReportPortal/utils/common';
import { useRouter } from 'vue-router';

const router = useRouter();
const UseParamsAdd = AddCommonPageParams();
let FactoryType: any = UseParamsAdd.Fab;
function getImport(site: any) {
  const { schemas, optionsdata } = require(`@/pages/ReportPortal/FS/ToBeScrapResponsibility/${site}/form-schema`);
  const {
    baseColumns,
    WipDistDetail3Columns,
    WipDistDetail2Columns,
    WipDistDetail1Columns
  } = require(`@/pages/ReportPortal/FS/ToBeScrapResponsibility/${site}/table`);
  return { schemas, optionsdata, baseColumns, WipDistDetail3Columns, WipDistDetail2Columns, WipDistDetail1Columns };
}
const { schemas, optionsdata, baseColumns, WipDistDetail3Columns, WipDistDetail2Columns, WipDistDetail1Columns } =
  getImport(FactoryType);

// 頁面數據
let PageSetting = reactive({
  TableSpan: 21,
  showmenu: true,
  TopList: [] as any,
  TopColumns: [] as any,
  EchartOptions: {},
  Message: {},
  isShowTable: false,
  buttonText: '切换表格',
  BottomList: [] as any,
  BottomColumns: [] as any,
  BottomTitle: ''
});
const [DynamicTable] = useTable();
const { mounted, mountedowner, mountedOper, mountedLcmProdType } = useRemoteSelect();

const [SchemaForm, dynamicFormRef] = useForm({
  labelWidth: 120,
  schemas: schemas(),
  actionColOptions: {
    span: 24
  },
  fieldMapToTime: [['fieldTime', ['vDate_s', 'vDate_e1'], 'YYYY-MM-DD']],
  layout: 'vertical'
});

function callbackFun(value: any) {
  console.log('点击了', value);
}
function changeView() {
  PageSetting.isShowTable = !PageSetting.isShowTable;
  if (PageSetting.isShowTable) {
    PageSetting.buttonText = '切换图表';
  } else {
    PageSetting.buttonText = '切换表格';
  }
}
let TopTableParams = reactive({
  name: 'Data',
  apiName: 'ToBeScrapResponsibility',
  ReportType: '',
  FactoryType: FactoryType,
  FacID: 'A',
});
const [openFullLoading, closeFullLoading] = useLoading({
  tip: '數據加載中...'
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
  PageSetting.BottomList = [];
  PageSetting.TopList = [];
  PageSetting.TopColumns = [];
  //給ParamsData賦值
  let {
    Data: {
      Data2: { Table: Data },
      Data5: { Table: xData },
      title1,
      title2,
      series
    }
  } = await GetCommon({ ...TopTableParams, ...values });
  let baseParams = { ...TopTableParams, ...values };
  if (Data.length == 0) {
    closeFullLoading();
    return message.warn('无数据!');
  }
  let temp1: Object[] = [];
  Object.keys(Data[0]).forEach((item) => {
    temp1.push(item);
  });
  let x_data: Object[] = [];
  Object.keys(xData[0]).forEach((item) => {
    if (item != "RESP_OWNER") {
      x_data.push(item);
    }
  });
  let bar_series: Object[] = [];
  let clickarray: string[] = [];
  Object.keys(Data[0]).forEach((item, index) => {
    console.log(index);
    if (index > 2) {
      clickarray.push(item);
    }
  });
  let temp: Object[] = TableRendering(title1, title2, temp1, true, [0, 1, 2]);
  PageSetting.TopList = Data;
  PageSetting.TopColumns = temp;
  let title_chart = "LCMA To Be Scrap 責任歸屬 ReportChart"
  if (FactoryType == "LCM2") {
    title_chart = "LCMB To Be Scrap 責任歸屬 ReportChart"
  }
  PageSetting.EchartOptions = {
    title: [
      {
        text: title_chart,
        left: 'center'
      }
    ],
    tooltip: {
      trigger: 'item',
      axisPointer: {
        type: 'shadow'
      }
    },
    legend: {
      // 柱状图的leggeng

      bottom: '16%',
      type: 'scroll',
      padding: [10, 0, 0, 0],
      textStyle: {
        color: 'rgba(36, 173, 254, 1)',
        fontSize: '1rem'
      },
      //图例标记的图形高度
      itemHeight: 5,
      //图例标记的图形宽度
      itemWidth: 10
    },
    grid: {
      left: '3%',
      top: '15%',
      right: '4%',
      bottom: '20%',
      containLabel: true
    },
    xAxis: [
      {
        type: 'category',
        data: x_data
      }
    ],
    yAxis: [
      {
        type: 'value'
      }
    ],
    series: series
  };
  console.log(clickarray);
  console.log(PageSetting.TopColumns);


  // PageSetting.TopColumns.forEach((item) => {
  //   if (item.children) {
  //     item.children.forEach((citem) => {
  //       citem['customCell'] = (record: Object, rowindex: number, column: Object) => {
  //         return {
  //           onDblclick: () => {
  //             console.log(column);
  //             console.log("父級", item)
  //             if (['數量', '總計', '不良率%', '%', '-'].includes(citem['dataIndex'])) {
  //               let bottomparams = cloneDeep(baseParams);
  //               bottomparams.ReportType = 'ToBeScrapDetail';
  //               bottomparams['resp_owner'] = record['責任歸屬'];
  //               if (record['責任歸屬'] == "小計") {
  //                 bottomparams['resp_owner'] = Data[rowindex - 1]['責任歸屬']
  //               }
  //               bottomparams['report_type'] = 'resp_owner';
  //               if (record['Scrap Code'] != "" && record['Scrap Code'] != null) {
  //                 bottomparams['scrap_code'] = record['Scrap Code'];
  //               }
  //               else {
  //                 bottomparams['scrap_code'] = ' ';
  //               }
  //               bottomparams['scrap_date'] = item['title'];
  //               UseParamsAdd.AddFirstTableParams(bottomparams);
  //               switch (UseParamsAdd.Fab) {
  //                 case "LCM1":
  //                   router.push({ name: 'ToBeScrapResponsibilityCM1MFGDetail' });
  //                   break;
  //                 case "LCM2":
  //                   router.push({ name: 'ToBeScrapResponsibilityCM2MFGDetail' });
  //                   break;
  //               }
  //             } else {
  //               message.warning('此栏位没有二阶数据');
  //             }
  //           }
  //         };
  //       };
  //     })
  //   } else {
  //     item['customCell'] = (record: Object, rowindex: number, column: Object) => {
  //       return {
  //         onDblclick: () => {
  //           console.log(column);

  //           if (['數量', '總計', '不良率%', '%', '-'].includes(item['dataIndex'])) {
  //             let bottomparams = cloneDeep(baseParams);
  //             bottomparams.ReportType = 'ToBeScrapDetail';
  //             bottomparams['resp_owner'] = record['責任歸屬'];
  //             if (record['責任歸屬'] == "小計") {
  //               bottomparams['resp_owner'] = Data[rowindex - 1]['責任歸屬']
  //             }
  //             bottomparams['report_type'] = 'resp_owner';
  //             if (record['Scrap Code'] != "" && record['Scrap Code'] != null) {
  //               bottomparams['scrap_code'] = record['Scrap Code'];
  //             }
  //             else {
  //               bottomparams['scrap_code'] = ' ';
  //             }
  //             bottomparams['scrap_date'] = column['dataIndex'];
  //             UseParamsAdd.AddFirstTableParams(bottomparams);
  //             switch (UseParamsAdd.Fab) {
  //               case "LCM1":
  //                 router.push({ name: 'ToBeScrapResponsibilityCM1MFGDetail' });
  //                 break;
  //               case "LCM2":
  //                 router.push({ name: 'ToBeScrapResponsibilityCM2MFGDetail' });
  //                 break;
  //             }
  //           } else {
  //             message.warning('此栏位没有二阶数据');
  //           }
  //         }
  //       };
  //     };
  //   }

  // });
  antcustomCell(PageSetting.TopColumns, ({ record, rowindex, column, item, index, paramsColumns }) => {

    return {
      ...antmegercell({ record, rowindex, column }, ["責任歸屬"], PageSetting['TopList']),
      ...antdclick(column, clickarray, () => {
        console.log("xxxxxxxx", paramsColumns);

        console.log("父級", item)
        let bottomparams = cloneDeep(baseParams);
        bottomparams.ReportType = 'ToBeScrapDetail';
        bottomparams['resp_owner'] = record['責任歸屬'];
        console.log("**",record['責任歸屬']);
        
        if (record['責任歸屬'] == "小計") {
          bottomparams['resp_owner'] = Data[rowindex - 1]['責任歸屬']
        }
        bottomparams['report_type'] = 'resp_owner';
        if (bottomparams['resp_owner'] == null) {
          bottomparams['resp_owner'] = ''
        }
        if (record['Scrap Code'] != "" && record['Scrap Code'] != null) {
          bottomparams['scrap_code'] = record['Scrap Code'];
        }
        else {
          bottomparams['scrap_code'] = ' ';
        }
        if (paramsColumns['title']) {
          bottomparams['scrap_date'] = paramsColumns['title'];
        }
        else if (item['title']) {
          if (item['title'] == "不良率%") {
            bottomparams['scrap_date'] = '總計'
          } else {
            bottomparams['scrap_date'] = item['title'];
          }
        }
        UseParamsAdd.AddFirstTableParams(bottomparams);
        switch (UseParamsAdd.Fab) {
          case "LCM1":
            router.push({ name: 'ToBeScrapResponsibilityCM1MFGDetail' });
            break;
          case "LCM2":
            router.push({ name: 'ToBeScrapResponsibilityCM2MFGDetail' });
            break;
        }
      })
    }
  })
  // megercell(PageSetting.TopColumns, ['責任歸屬'], PageSetting.TopList );
}
onMounted(async () => {
  /* await mountedowner(optionsdata, FactoryType);
  await mountedOper(optionsdata, FactoryType);
  await mounted(optionsdata, FactoryType);
  await mountedLcmProdType(optionsdata, FactoryType); */
});
</script>
 
<style lang="less" scoped>
/* :deep(.ant-table) {
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
 } */
</style>