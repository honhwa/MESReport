<template>
  <div class="Report">
    <a-row>
      <a-col :span="24 - PageParams.TableSpan" class="ScrollForm">
        <SchemaForm @submit="confirm">
          <template #uploadBefore>
            <a-button type="primary" class="newbutton" @click="showUploadModal">上傳</a-button>
          </template>
        </SchemaForm>
      </a-col>
      <a-col :span="PageParams.TableSpan">
        <div ref="report">
          <DynamicTable
            row-key="report"
            :header-title="PageSetting.Title"
            :columns="PageSetting.Columns"
            showFavorite
            sticky
            :key="PageSetting.Columns"
            bordered
            :row-class-name="getRowClassNmae"
            :data-source="PageSetting.moreData"
            :data-all="PageSetting.moreData"
            :pagination="false"
            :flexwidth="-10"
            :export-file-name="PageSetting.Title"
          >
            <template #export-button>
              <a-button type="success"> 导出 </a-button>
            </template>
          </DynamicTable>
        </div>
      </a-col>
    </a-row>
    <!-- <upload-excel
      :maxSize="20"
      :maxNumber="1"
      v-model:visible="visible"
      :uploadParams="uploadParams"
      exampfilename="test"
      examplefileurl="./openfile/ProductionProgressExample.xlsx"
    ></upload-excel> -->
    <upload-excel
      :maxNumber="1"
      :accept ="['xlsx','xls']"
      v-model:visible="visible"
      :uploadParams="uploadParams"
    ></upload-excel>
  </div>
</template>

<script lang="tsx" setup>
  import { PageParams } from '@/components/core/dynamic-table/src/dynamic-table';
  import { useLoading } from '@/components/feat/Loading';
  import { GetCommon } from '@/api/pages/common';
  import { onMounted } from '@vue/runtime-core';
  import { reactive, ref,isRef } from 'vue';
  import { useForm } from '@/components/core/schema-form';
  import { useTable } from '@/components/core/dynamic-table';
  import { ReportCommon } from '@/store/module/ReportCommon';
  import { message } from 'ant-design-vue';
  import { getRowClassNmae } from '@/pages/ReportPortal/utils/common';
  import UploadExcel from '@/pages/updownfile/upexcel/directUploadExcel.vue';

  const UseParamsAdd = ReportCommon();
  let visible = ref(false);
  let FactoryType: string = UseParamsAdd.Fab;
  const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...',
  });

  function getImport(FactoryType: string) {
    FactoryType = 'BLU1';
    const {
      schemas,
      optionsdata,
    } = require(`@/pages/ReportPortal/FS/CKENReport1370/${FactoryType}/form-schema`);
    const {
      CKENDataSetColumn,
    } = require(`@/pages/ReportPortal/FS/CKENReport1370/${FactoryType}/Table`);
    return { schemas, optionsdata, CKENDataSetColumn };
  }
  let { schemas, optionsdata, CKENDataSetColumn } = getImport(FactoryType);

  let PageSetting = reactive<BasePageParams.BasePageSetting>({
    List: [] as any,
    Columns: [] as any,
    Title: '',
    moreData: [],
    SQLHidden: '',
    APIColumns: [] as any,
  });
  let baseParams = {
    apiName: 'CKENDataSet',
    FactoryType: FactoryType,
    G_FAC: UseParamsAdd.G_FAC_COM,
  };
  let uploadParams = reactive({
    ...baseParams,
    ActionType:'',
    PassWord:''
  });

  const [DynamicTable] = useTable();
  const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas: schemas,
    actionColOptions: {
      span: 24,
    },
    layout: 'vertical',
  });

  async function confirm(values) {
    openFullLoading();

    Object.keys(values).forEach((key) => {
      if (!values[key]) {
        values[key] = '';
      }
    });
    if (values['PassWord'] != '') {
      values['ActionType'] = 'Query1';
      PageSetting.Columns = CKENDataSetColumn['Query1'];
    } else {
      values['ActionType'] = 'Query';
      PageSetting.Columns = [
        {
          title: values['QType'] == 'PROD_NBR' ? '機臺' : '機種',
          align: 'center',
          dataIndex: 'PROD_NBR',
          fixed: 'left',
          className: 'v-center',
        },
        ...CKENDataSetColumn['Query'],
      ];
    }

    try {
      let { api01 } = await GetCommon({ ...baseParams, ...values });

      if (api01['Table'].length == 0) {
        PageSetting.moreData = [];
        closeFullLoading();
        return message.warn('无数据!');
      } else {
        PageSetting.moreData = api01['Table'];
      }

      closeFullLoading();
    } catch (error) {
      console.log(error);
      closeFullLoading();
    }
  }
  function showUploadModal() {

    uploadParams.ActionType = 'Update'//uploadParams這種參數必須先定義才能傳遞給子組件
    uploadParams.PassWord = dynamicFormRef.formModel.PassWord
    
    visible.value = true;
  }
  onMounted(async () => {});
</script>

<style lang="less" scoped>
  .newbutton {
    margin-left: 30%;
  }
</style>
