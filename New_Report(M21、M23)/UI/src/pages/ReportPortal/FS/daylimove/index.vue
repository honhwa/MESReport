<template>
    <div class="DayliMove">
      <a-row>
        <a-col :span="24 - PageParams.TableSpan" class="ScrollForm">
          <SchemaForm @submit="confirm"> </SchemaForm>
        </a-col>
        <a-col :span="PageParams.TableSpan">
          <div ref="DaylimoveTable">
           
            <!-- 
                 :excellparams="PageSetting.excellparams"
               export-type="Exceljs"

             -->
            <VexTable
              :PageParams="PageParams"
              export-type="Api"
              :header-title="PageSetting.Title"
              :excellparams="PageSetting.excellparams"
              :columns="PageSetting.Columns"
              :key="PageSetting.Columns"
              :tableData="PageSetting.moreData"
              :data="PageSetting.moreData"
              :export-file-name="PageSetting.Title"
              :flexwidth="10"
              :mergeCells="PageSetting.mergecellData"
            >
              <template #export-button>
                <a-button type="success"> 导出 </a-button>
              </template>
            </VexTable>
          </div>
        </a-col>
        <div hidden>{{ PageSetting.SQLHidden }}</div>
      </a-row>
    </div>
  </template>
  
  <script lang="tsx" setup>
    import { PageParams } from '@/components/core/dynamic-table/src/dynamic-table';
    import { useForm } from '@/components/core/schema-form';
    import { reactive, ref, onMounted } from 'vue';
    import { GetDailyMove } from '@/api/pages/daylimove';
    import { useLoading } from '@/components/feat/Loading';
    import { getdiffdate, megercellVxe } from '@/pages/ReportPortal/utils/common';
    import { ReportCommonWithOut } from '@/store/module/ReportCommon';
    import { useRouter } from 'vue-router';
    import { message } from 'ant-design-vue';
    import VexTable from '@/components/core/vex-virtual-table/src/vex-virtual-table.vue';
  
    const router = useRouter();
    const UseParamsAdd = ReportCommonWithOut();
    const DaylimoveTable = ref<HTMLElement | null>(null);
    //vue3的mixin
  
    function getImport(site: any) {
      const {
        schemas,
        optionsdata,
      } = require(`@/pages/ReportPortal/FS/daylimove/${site}/form-schema`);
      const {
        LCM1Columns,
        LCM2Columns,
        BLUColumns,
        SET1PCColumns,
        SET1TestColumns,
        SET1TestNewColumns,
        SET2PCColumns,
        SET2TestColumns,
        SET2TestNewColumns,
      } = require(`@/pages/ReportPortal/FS/daylimove/${site}/Table`);
      return {
        schemas,
        LCM1Columns,
        LCM2Columns,
        BLUColumns,
        SET1PCColumns,
        SET1TestColumns,
        SET1TestNewColumns,
        SET2PCColumns,
        SET2TestColumns,
        SET2TestNewColumns,
        optionsdata,
      };
    }
    const [openFullLoading, closeFullLoading] = useLoading({
      tip: '數據加載中...',
    });
    let importurl = ref('');
    switch (UseParamsAdd.Fab) {
      case 'BLU1':
      case 'BLU2':
        importurl.value = 'BLU';
        break;
      default:
        importurl.value = 'LCM';
        break;
    }
    let { schemas, LCM1Columns, BLUColumns, SET1PCColumns, SET1TestColumns, SET1TestNewColumns } =
      getImport(importurl.value);
  
    let PageSetting = reactive({
      List: [] as any,
      APIColumns: [] as any,
      Columns: [] as any,
      moreData: [] as any,
      SQLHidden: '',
      Title: '',
      baseColumns: [],
      excellparams: {},
      mergecellData: [] as any,
    });
  
    const [SchemaForm, dynamicFormRef] = useForm({
      labelWidth: 120,
      schemas: schemas(UseParamsAdd.Fab),
      actionColOptions: {
        span: 24,
      },
      layout: 'vertical',
      /* submitButtonOptions:{
            text: '查询333'
        } */
    });
    /**
     * 提交表单,验证通过后进行数据获取与处理
     * @dynamicFormRef 表单的所有参数与属性
     * @ParamsData API接收的参数
     */
    function confirm(values) {
      openFullLoading();
      let baseParams = {
        name: 'Date',
        apiName: 'DailyMove',
        FactoryType: UseParamsAdd.Fab,
        G_FAC: 'A',
        building: '',
      };
      Object.keys(values).forEach((key) => {
        if (!values[key]) {
          values[key] = '';
        } /* else if (key == 'bigProduct') {
                values['Product'] = values[key];
            } else if (key == 'Product') {
                values['prod_nbr'] = values[key];
            } */ else if (key == 'wo_num') {
          values[key] = values[key].split(/[\r\n\t]/).filter((item) => item);
        }
      });
      switch (UseParamsAdd.Fab) {
        case 'LCM2':
          PageSetting.Title = 'LCMA Daily Move ';
          break;
        case 'BLU1':
        case 'BLU2':
          if (UseParamsAdd.Fab == 'BLU2') {
          }
          switch (values['floor']) {
            case '1':
              values['floor'] = " and floor='1'";
              break;
            case '2':
              values['floor'] = " and floor='2'";
              break;
            case '3':
              values['floor'] = " and floor='3'";
              break;
            default:
              break;
          }
          if (UseParamsAdd.Fab == 'BLU1') {
            PageSetting.Title = 'BLA Daily Move ';
          } else {
            PageSetting.Title = 'BL2 Daily Move ';
          }
          break;
        case 'SET1':
          let routertitle = router.currentRoute.value.meta.title;
          if (routertitle == '(MV202) Daily Move For TW') {
            baseParams.FactoryType = 'SET1_TEST';
          } else if (routertitle == '(MV202) Daily Move For TW new') {
            baseParams.FactoryType = 'SET1_NEW_TEST';
          } else if (routertitle == '(MV202) Daily Move') {
            baseParams.FactoryType = 'SET1';
          }
          PageSetting.Title = 'LCMA_TV Daily Move ';
          break;
        case 'SET2':
          let routertitle2 = router.currentRoute.value.meta.title;
          if (routertitle2 == '(MV202) Daily Move For TW') {
            baseParams.FactoryType = 'SET2_TEST';
          } else if (routertitle2 == '(MV202) Daily Move For TW new') {
            baseParams.FactoryType = 'SET2_NEW_TEST';
          } else if (routertitle2 == '(MV202) Daily Move') {
            baseParams.FactoryType = 'SET2';
          }
          PageSetting.Title = 'LCM2_TV Daily Move ';
          break;
        default:
          PageSetting.Title = 'LCMA Daily Move ';
          break;
      }
      dynamicFormRef.validate().then(async () => {
        try {
          const {
            Date: {
              Data: { Table: Data, sql: Message },
            },
          } = await GetDailyMove({ ...baseParams, ...values });
          PageSetting.excellparams = { ...baseParams, ...values };
          if (Data.length == 0) {
            closeFullLoading();
            return message.warn('無數據!');
          }
          PageSetting.APIColumns = getdiffdate(values.vDate_s, values.vDate_e1);
  
          let showattr = '';
          switch (UseParamsAdd.Fab) {
            case 'BLU1':
            case 'BLU2':
              PageSetting.baseColumns = BLUColumns;
              showattr = 'PROD_NBR';
              break;
            case 'SET1':
            case 'SET2':
              let routertitle = router.currentRoute.value.meta.title;
              if (routertitle === '(MV202) Daily Move For TW') {
                PageSetting.baseColumns = SET1TestColumns;
  
                showattr = 'CUSTOMER';
              } else if (routertitle === '(MV202) Daily Move For TW new') {
                PageSetting.baseColumns = SET1TestNewColumns;
                showattr = 'EQUIP_NBR';
              } else {
                PageSetting.baseColumns = SET1PCColumns;
                showattr = 'CUSTOMER';
              }
              break;
            case 'LCM1':
            case 'LCM2':
              PageSetting.baseColumns = LCM1Columns;
              showattr = 'CUSTOMER';
            default:
              showattr = 'CUSTOMER';
              break;
          }
          PageSetting.Columns = Object.keys(Data[0]).map((item) => {
            return {
              title: item,
              align: 'center',
              dataIndex: item,
              resizable: true,
            };
          });
  
          PageSetting.moreData = Data; //进行表格合并
  
          // PageSetting.mergecellData =  megercellVxe(
          //     PageSetting.Columns,
          //     ['Operation', 'Work Order', 'Size', 'Owner', 'Customer', showattr],
          //     PageSetting.moreData,
          //   );
  
          closeFullLoading();
          // console.log('輸出最終的columns', PageSetting.Columns);
        } catch (error) {
          closeFullLoading();
        }
      });
    }
  
    let TableHeight = ref(0);
  
    onMounted(async () => {
      /*  await mountedowner(optionsdata, UseParamsAdd.Fab);
         await mountedOper(optionsdata, UseParamsAdd.Fab); 
         await mounted(optionsdata, UseParamsAdd.Fab);
         */
      TableHeight.value = Math.round(document.documentElement.clientHeight) - 200;
    });
  </script>
  