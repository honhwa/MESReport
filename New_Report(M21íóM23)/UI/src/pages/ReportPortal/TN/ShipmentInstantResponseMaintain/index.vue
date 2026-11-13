<template>
  <div class="ShipmentInstantResponseMaintain">
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
          <div class="swapper">
            <div>
              <font color="blue">產品資訊:</font><br />
              說明：<br />
              &emsp;1.CATE_CODE1：Project name <br />
              &emsp;2.SEQ：顯示順序<br />
              <br />
              <font color="blue">可出貨GRADE:</font><br />
              說明：<br />
              &emsp;1.CATE_CODE2：可出貨GRADE <br />
              <br />
              <font color="blue">每日Target:</font><br />
              說明：<br />
              &emsp;1.ACCT_DATE：日期 <br />
              &emsp;2.CATE_CODE3：出貨重點說明 <br />
              &emsp;3.DAILY_QTY：數量
              <br />
            </div>
          </div>
        </a-col>
        <a-col :span="PageSetting.TableSpan" class="ScrollTable">
          <div v-show="PageSetting.isImport">
            &nbsp;
            <button
              type="button"
              class="ant-btn ant-btn-primary ant-btn-sm mr-2"
              @click="state.visible = true"
            >
              匯入資料
            </button>
            <button
              type="button"
              class="ant-btn ant-btn-primary ant-btn-sm mr-2"
              @click="importData"
            >
              確定匯入
            </button>
            <button
              type="button"
              class="ant-btn ant-btn-primary ant-btn-sm mr-2"
              @click="cancelData"
            >
              取消
            </button>
          </div>
          <div>
            <DynamicTable
              row-key="id"
              header-title="Production Progress Maintain"
              :columns="PageSetting.Columns"
              :key="PageSetting.Columns"
              bordered
              size="small"
              :data-source="PageSetting.List"
              :data-all="PageSetting.moreData"
              :pagination="false"
              :scroll="PageSetting.TableSize"
              export-file-name="Export"
              :row-class-name="getRowClassNmae"
              :PageParams="PageParams"
            >
              <template #export-button>
                <a-button type="success"> Export </a-button>
              </template>
            </DynamicTable>
          </div>
        </a-col>
        <div hidden>{{ PageSetting.SQLHidden }}</div>
      </a-row>
    </a-card>
    <DraggableModal :visible="state.visible" :title="'匯入資料'">
      <label style="color: blue; white-space: pre-line">{{ PageSetting.importMsg }}</label> <br />
      <upexcel :beforeUpload="beforeUpload" :onSuccess="onSuccess"></upexcel><br />
      <label style="color: red"
        >※ 請注意：資料匯入前，系統會根據左側所選條件刪除原有資料，請確定選擇是否正確。</label
      >
      <br />
      <label style="color: red">{{ PageSetting.ErrMsg }}</label>
      <template #footer>
        <a-button @click="colse" danger>取消</a-button>
      </template>
    </DraggableModal>
  </div>
</template>

<script lang="tsx" setup>
  import { PageParams } from '@/components/core/dynamic-table/src/dynamic-table';
  import { MenuFoldOutlined, MenuUnfoldOutlined } from '@ant-design/icons-vue';
  import { useForm } from '@/components/core/schema-form';
  import { useTable } from '@/components/core/dynamic-table';
  import { reactive, ref, onMounted } from 'vue';
  import { GetCommon } from '@/api/pages/common';
  import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect';
  import { showHidden, getRowClassNmae, mergeTableRow } from '@/pages/ReportPortal/utils/common';
  import { AddCommonPageParams } from '@/store/module/commonpage';
  import { Loading, useLoading } from '@/components/feat/Loading';
  import { DraggableModal } from '@/components/core/draggable-modal';
  import upexcel from '@/pages/updownfile/upexcel/index.vue';
  import dayjs from 'dayjs';

  const useCommonParams = AddCommonPageParams();
  //vue3的mixin
  const { InfiniteScroll } = useRemoteSelect();
  function getImport(site: any) {
    const {
      schemas,
    } = require(`@/pages/ReportPortal/TN/ShipmentInstantResponseMaintain/${site}/form-schema`);
    const {
      baseColumns,
    } = require(`@/pages/ReportPortal/TN/ShipmentInstantResponseMaintain/${site}/Table`);
    return { schemas, baseColumns };
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

  let { schemas, baseColumns } = getImport('MOD');

  let PageSetting = reactive({
    List: [] as any,
    TableSpan: 20,
    ShowMenu: true,
    APIColumns: [] as any,
    Columns: [] as any,
    moreData: [] as any,
    SQLHidden: '',
    isImport: false,
    ErrMsg: '',
    importMsg: '',
    TableSize: { x: -1, y: -1 }, //y = -1 => 配合畫面
  });
  schemas[0].componentProps.onChange = async (e) => {
    selCategory = e.target.value;
    PageSetting.importMsg = importMsg[selCategory];
  };
  let selCategory = 'PRODINFO';
  let importMsg = {
    PRODINFO:
      '產品資訊：\r\n' +
      '　說明： \r\n' +
      '　　1.CATE_CODE1：Project name \r\n' +
      '　　2.SEQ：顯示順序',
    PRODGRADE: '可出貨GRADE：\r\n' + '　說明：\r\n' + '　　1.CATE_CODE\r\n' + '　　2：可出貨GRADE',
    DAILYTART:
      '每日Target：\r\n' +
      '　說明：\r\n' +
      '　　1.ACCT_DATE：日期\r\n' +
      '　　2.CATE_CODE3：出貨重點說明\r\n' +
      '　　3.DAILY_QTY：數量',
  };

  const [DynamicTable] = useTable();

  const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas,
    actionColOptions: {
      span: 24,
    },
    layout: 'vertical',
  });
  interface RT_USR_PROD {
    CATEGORY: string;
    ACCT_DATE: string;
    PROD_NBR: string;
    CATE_CODE1: string;
    CATE_CODE2: string;
    CATE_CODE3: string;
    CATE_CODE4: string;
    CATE_CODE5: number;
    DAILY_QTY: number;
    QTY: number;
    RATE_TGT: number;
    SEQ: number;
  }

  /**
   * 提交表单,验证通过后进行数据获取与处理
   * @dynamicFormRef 表单的所有参数与属性
   * @ParamsData API接收的参数
   */
  function confirm(values) {
    let baseParams = {
      name: 'Date',
      apiName: 'TN_ShipmentInstantResponseMaintain',
      FactoryType: useCommonParams.Fab,
    };
    Object.keys(values).forEach((key) => {
      if (!values[key]) {
        values[key] = '';
      }
    });
    let columns = baseColumns[selCategory];

    dynamicFormRef.validate().then(async () => {
      openFullLoading();
      const {
        Date: {
          Data: { Table: Data },
        },
      } = await GetCommon({ ...baseParams, ...values });
      closeFullLoading();

      PageSetting.Columns = [...columns];
      PageSetting.moreData = Data;
      if (PageSetting.moreData.length > 100) {
        PageSetting.List = PageSetting.moreData.slice(0, 40);
      } //预先加载40条数据
      else {
        PageSetting.List = PageSetting.moreData;
      }

      PageSetting.isImport = true;
    });
  }

  onMounted(async () => {});

  const state = reactive({
    visible: false,
  });

  function colse() {
    state.visible = false;
  }

  function beforeUpload(rawFile) {
    console.log(rawFile);
    //onOk();
    return true;
  }

  function onSuccess(excelData) {
    let chkColumn = {
      PRODINFO: ['PROD_NBR', 'CATE_CODE1', 'SEQ'],
      PRODGRADE: ['PROD_NBR', 'CATE_CODE2'],
      DAILYTART: ['PROD_NBR', 'CATE_CODE3', 'ACCT_DATE', 'DAILY_QTY'],
    };
    let isCheck = true;
    chkColumn[selCategory].forEach((item, index) => {
      if (item != excelData.header[index]) {
        isCheck = false;
        PageSetting.ErrMsg = '匯入欄位錯誤，請確認查詢的類別及匯入的工作表是否為第一個工作表';
        return;
      }
    });
    if (isCheck) {
      state.visible = false;
      openFullLoading();
      let insertData: RT_USR_PROD[] = [];
      excelData.results.forEach((item) => {
        let acctDate = item['ACCT_DATE'] ?? '1900-01-01';
        if (acctDate) {
          acctDate = dayjs(new Date(acctDate)).format('YYYY-MM-DD');
        }
        insertData.push({
          CATEGORY: selCategory,
          ACCT_DATE: acctDate,
          PROD_NBR: item['PROD_NBR'],
          CATE_CODE1: item['CATE_CODE1'],
          CATE_CODE2: item['CATE_CODE2'],
          CATE_CODE3: item['CATE_CODE3'],
          CATE_CODE4: item['CATE_CODE4'],
          CATE_CODE5: item['CATE_CODE5'],
          DAILY_QTY: item['DAILY_QTY'],
          QTY: item['QTY'],
          RATE_TGT: item['RATE_TGT'],
          SEQ: item['SEQ'],
        });
      });

      console.log('import data length：' + insertData.length);
      PageSetting.Columns = [...baseColumns[selCategory]];
      PageSetting.moreData = insertData;
      if (PageSetting.moreData.length > 100) {
        PageSetting.List = PageSetting.moreData.slice(0, 40);
      } //预先加载40条数据
      else {
        PageSetting.List = PageSetting.moreData;
      }
      PageSetting.isImport = true;
    }
    closeFullLoading();
  }
  function importData() {
    openFullLoading();
    let baseParams = {
      name: 'Date',
      apiName: 'TN_ShipmentInstantResponseMaintainImport',
      FactoryType: useCommonParams.Fab,
      Category: selCategory,
      jsonData: JSON.stringify(PageSetting.moreData),
    };

    dynamicFormRef.validate().then(async () => {
      const {
        Date: { addCount: addCount, addSql: addSql },
      } = await GetCommon({ ...baseParams });
      if (addCount > 0) {
        alert('匯入完成，共 ' + addCount + ' 筆');
        PageSetting.isImport = false;
      }
    });
    closeFullLoading();
  }
  function cancelData() {
    PageSetting.List = [];
    PageSetting.moreData = [];
    PageSetting.Columns = [];
    PageSetting.isImport = false;
  }
</script>
<style lang="less" scoped>
  .btn-rows {
    button {
      margin-right: 12px;
    }
  }
</style>
