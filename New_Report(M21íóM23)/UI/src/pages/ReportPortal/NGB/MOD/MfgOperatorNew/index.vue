<template>
  <div class="MfgOperatorNew">
    <a-card>
      <!-- <a-row>
                <span class="menu-fold" @click="showHidden(PageSetting)">
                    <component :is="!PageSetting.showmenu ? MenuUnfoldOutlined : MenuFoldOutlined" />
                </span>
            </a-row> -->
      <a-row>
        <a-col :span="24 - PageParams.TableSpan" Source-show="PageSetting.showmenu">
          <div class="NGBScrollForm">
            <SchemaForm @submit="confirm"> </SchemaForm>
          </div>
        </a-col>
        <a-col :span="PageParams.TableSpan">
          <div class="ScrollTable" ref="MfgOperatorNewTable">
            <DynamicTable
              sticky
              showFavorite
              :PageParams="PageParams"
              row-key="id"
              :header-title="PageSetting.HeadTitle"
              :columns="PageSetting.Columns"
              :key="PageSetting.Columns"
              bordered
              size="small"
              :data-source="PageSetting.List"
              :pagination="false"
              export-file-name="导出"
              :data-all="PageSetting.moreData"
              :scroll="{ x: PageSetting.x }"
              :row-class-name="getRowClassName"
            >
              <!-- <template #toolbar>
                                <a-button type="primary" @click="isShowExplain = !isShowExplain">{{isShowExplain?'显示表格':'显示说明文件'}}</a-button>
                            </template>   -->
              <template #export-button>
                <a-button type="success"> 导出 </a-button>
              </template>
            </DynamicTable>
          </div>
        </a-col>
        <div hidden>{{ PageSetting.SQLHidden }}</div>
      </a-row>
    </a-card>
  </div>
</template>

<script lang="tsx" setup>
  import { MenuFoldOutlined, MenuUnfoldOutlined } from '@ant-design/icons-vue';
  import { PageParams } from '@/components/core/dynamic-table/src/dynamic-table';
  import { useForm } from '@/components/core/schema-form';
  import { useTable } from '@/components/core/dynamic-table';
  import { reactive, unref, ref, onMounted, watch, computed, Ref } from 'vue';
  import { GetCommon } from '@/api/pages/common';
  import useRemoteSelect from '@/pages/ReportPortal/utils/NGBuseRemoteSelect';
  import { useLoading } from '@/components/feat/Loading';
  import { cloneDeep } from 'lodash-es';
  import type { TableColumn } from '@/components/core/dynamic-table';
  import { getPovitData, PovitParams } from '@/utils/helper/povitHelper';
  
  import { AddCommonPageParams } from '@/store/module/commonpage';
  import { ReportCommon } from '@/store/module/ReportCommon';
  import { useRouter, useRoute } from 'vue-router';
  import { Console } from 'console';
  import { message } from 'ant-design-vue';




  

  let useCommonParams = AddCommonPageParams();
  let useReportCommon = ReportCommon();
  let bigfab = useCommonParams.Fab.slice(0, useCommonParams.Fab.length - 1);
  let fab = 'LCM';
  if (useCommonParams.Fab == 'LCM1' || useCommonParams.Fab == 'LCM3') {
    fab = useCommonParams.Fab;
  }
  const router = useRouter();
  const route = useRoute();

  const MfgOperatorNewTable = ref<HTMLElement | null>(null);
  //vue3的mixin
  const { InfiniteScroll } = useRemoteSelect();
  function getImport(site: any) {
    //let { optionsdata } = require('@/pages/ReportPortal/Static/NGBCommon-Form-schema');
    const { schemas } = require(`@/pages/ReportPortal/NGB/MOD/MfgOperatorNew/${fab}/form-schema`);
    const { baseColumns } = require(`@/pages/ReportPortal/NGB/MOD/MfgOperatorNew/${bigfab}/Table`);
    return { schemas, baseColumns };
  }

  const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...',
  });

  let { schemas, baseColumns } = getImport(useCommonParams.Fab);

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
  let ReportNameParams = `MfgOperatorNew-${useCommonParams.Fab}`;
  useReportCommon.AddObject(ReportNameParams, ReportItem);

  let PageSetting = reactive({
    List: [] as any,
    TableSpan: 20,
    showmenu: true,
    APIColumns: [] as any,
    Columns: [] as any,
    SQLHidden: '',
    HeadTitle: '',
    x: '' as any,
    moreData: [] as any,
  });
  // let isLongData = ref(true)
  //PageSetting.HeadTitle = `${useCommonParams.Fab} MFG 人員作業 (MFG OPERATOR)`;
  const [DynamicTable] = useTable();

  const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 130,
    schemas: schemas(useCommonParams.Fab, reactive({})),
    actionColOptions: {
      span: 18,
    },
    layout: 'vertical',
  });

  const customCellRender = (record: Object, rowIndex: number, column: Object) => {
    return {
      ondblclick: () => {
        ReportItem.FirstTableParams = { ...record, ...column['__originColumn__'] };
        //useReportCommon.AddFirstTableParams(`MfgOperator-${useCommonParams.Fab}`,{ ...record, ...column['__originColumn__'] })
        if (useCommonParams.Fab == 'LCM1' || useCommonParams.Fab == 'LCM2') {
          router.push({ name: `MfgOperatorNewDetail-MOD1` });
        }
        if (useCommonParams.Fab == 'LCM3' || useCommonParams.Fab == 'LCM4') {
          router.push({ name: `MfgOperatorNewDetail-MOD2` });
        }
      
      },
      style: {
        cursor: 'pointer',
      },
    };
  };

  /**
   * 提交表单,验证通过后进行数据获取与处理
   * @dynamicFormRef 表单的所有参数与属性
   * @ParamsData API接收的参数
   */
  function confirm(values) {
    //table 初始化

    PageSetting.List = [];
    PageSetting.Columns = [];
    //加載防護罩開始
 
    openFullLoading();
 
    if(values.FAC=="LCM2")
{
    useCommonParams.Fab="LCM2"
    useCommonParams.G_FAC_COM="B" 
} 
if(values.FAC=="LCM1")
{
    useCommonParams.Fab="LCM1"
    useCommonParams.G_FAC_COM="C"

} 
if(values.FAC=="LCM4")
{
    useCommonParams.Fab="LCM4"
    useCommonParams.G_FAC_COM="D"
} 
if(values.FAC=="LCM3")
{
    useCommonParams.Fab="LCM3"
    useCommonParams.G_FAC_COM="E"

} 

closeFullLoading();

    //基礎query參數條件值
    let ApiNamePara='';
    if (bigfab==="LCM"){
        ApiNamePara="MFGOperatorDataSet_MOD";
    }
    let baseParams = {name: "Date",
        apiName: ApiNamePara,
        FactoryType: useCommonParams.Fab,
        G_FAC: useCommonParams.G_FAC_COM,
    };
    
    // let baseParams = {
    //   name: 'Date',
    //   apiName: 'MFGOperatorDataSet_MOD',
    //   FactoryType: useCommonParams.Fab,
    //   G_FAC: useCommonParams.G_FAC_COM,
    // };

    //獲取query頁面的條件值
    Object.keys(values).forEach((key) => {
      if (!values[key]) {
        values[key] = '';
      }
    });

    if (values.Prod_nbr && values.Prod_nbr.length > 1000) {
      closeFullLoading();
      return message.error('料號输入超過 “最大限制筆數:1000” ');
    }

    for (let i in dynamicFormRef?.formModel) {
      if (i == 'Wo_nbr') values[i] = dynamicFormRef.formModel[i].split('\n');
      else {
        values[i] = dynamicFormRef.formModel[i];
      }
    }
    //console.log("useReportCommon3",useCommonParams.Fab)
        PageSetting.HeadTitle = `${useCommonParams.Fab} MFG 人員作業 (MFG OPERATOR)`;    
       // console.log("useReportCommon2",useCommonParams.Fab) 

    //獲取數據綁定table
    dynamicFormRef.validate().then(async () => {
      try {
        const {
          Date: {
            Data: { Table: Data },
            SQL01,          
            sql_Shift,
            sql_Floor,
            sql_Product,
            sql_LcmOwner,
            sql_Operation,
            
          },
        } = await GetCommon({ ...baseParams, ...values });

        PageSetting.SQLHidden = 'MFGOperatorNewDataSet-SQLHidden:' + SQL01;
        //console.log("MFGOperatorDataSet-SQLHidden:",SQL01);
       
        
        let Message = {
          sql_Floor: sql_Floor,
          sql_Operation: sql_Operation,
          sql_Product: sql_Product,
          sql_Shift: sql_Shift,
          sql_LcmOwner: sql_LcmOwner,
        
        };

       
        
        if (Data.length != 0) {
          //循環查詢data，獲取table Colums 數據
          let ColumsData: any = [];
          let WidthPars = 50;
          let titlePars = ''; //label
          let classNamePars = '';
          let middleProps: string[] = []; //API提供轉置后數據（例如：A_A_2M270KCJ）

          for (let item in Data[0]) {
            if (item.slice(0, 2) == 'A_') {
              titlePars = item.slice(1).replaceAll('_', '');
              middleProps.unshift(item);
              WidthPars = 80;
            }

            if (item.slice(0, 2) == 'A_') {
              ColumsData.push({
                title: titlePars,
                align: 'center',
                dataIndex: item,
                width: WidthPars,
                className: classNamePars,
                customCell: customCellRender,
              });
            }
          }

          //修改添加bl

          if (values['QueryType'] === 'operators' || values['QueryType'] === 'operators_small') {
            PageSetting.Columns = [
              {
                title: 'Operator',
                align: 'left',
                dataIndex: 'OPERATORS',
                className: 'v-center',
                width: 60,
                fixed: 'left',
              },
              {
                title: 'Sum',
                align: 'left',
                dataIndex: 'SUM',
                className: 'sum-center',
                width: 45,
                customCell: customCellRender,
              },
              ...ColumsData,
            ];
          } else if (
            values['QueryType'] === 'equip_nbr' ||
            values['QueryType'] === 'equip_nbr_small'
          ) {
            PageSetting.Columns = [
              {
                title: 'Equipment',
                align: 'left',
                dataIndex: 'EQUIP_NBR',
                className: 'v-center',
                width: 70,
                fixed: 'left',
              },
              {
                title: 'Sum',
                align: 'left',
                dataIndex: 'SUM',
                className: 'sum-center',
                width: 45,
                customCell: customCellRender,
              },
              ...ColumsData,
            ];
          }

          //為加總提供要加總 計算的Columns
          PageSetting.APIColumns = [];
          PageSetting.APIColumns = middleProps;

          Data.forEach(
            (item) => (item['SUM'] = getAllSumData(item)), //在渲染的实际数组中加入总计数据
          );

          PageSetting.moreData = Data;

          if (PageSetting.moreData.length > 100) {
            PageSetting.List = PageSetting.moreData.slice(0, 40);
            // isLongData.value = true
          } //预先加载40条数据
          else {
            PageSetting.List = PageSetting.moreData;
            // isLongData.value = false
          }

          PageSetting.x = null;
          if (PageSetting.Columns.length > 7) {
            PageSetting.x = 150 * PageSetting.Columns.length;
          }

          ReportItem.APIMessage = JSON.stringify(Message);
          ReportItem.FormParams = { ...baseParams, ...values };

          //console.log("useReportCommon",useReportCommon)
        } else {
          message.warning('無數據！');
        }

        //加載防護罩結束
        closeFullLoading();
      } catch (error: any) {
        closeFullLoading();
        return Promise.reject(error);
      }
    });
  }

  function getAllSumData(row: any) {
    let sum = 0;
    PageSetting.APIColumns.map((item: any) => {
      row[item] = isNaN(row[item]) ? 0 : row[item];
      sum += row[item];
    });
    return sum;
  }

  InfiniteScroll(MfgOperatorNewTable, PageSetting);

  onMounted(async () => {
   
  });
</script>

<style lang="less" scoped>
  .btn-rows {
    button {
      margin-right: 12px;
    }
  }

  :deep(.sum-center) {
    background-color: #80ff80;
  }

  .swapper {
    display: flex;
    flex-direction: column;
  }
</style>
