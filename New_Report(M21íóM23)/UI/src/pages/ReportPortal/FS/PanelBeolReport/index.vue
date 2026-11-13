<template>
  <div class="Report">
    <a-row>
      <a-col :span="24 - PageParams.TableSpan" class="ScrollForm">
        <SchemaForm @submit="confirm"> </SchemaForm>
      </a-col>
      <a-col :span="PageParams.TableSpan">
        <div ref="report">
          <VexTable
            id="PanelBluReport"
            :header-title="PageSetting.Title"
            showFavorite
            :PageParams="PageParams"
            export-type="Multi"
            :columns="PageSetting.Columns"
            :key="PageSetting.Title"
            :row-class-name="getRowClassNmae"
            :tableData="PageSetting.moreData"
            :data="PageSetting.moreData"
            :export-file-name="PageSetting.Title"
            :flexwidth="10"
            :column-config="{ resizable: true }"
          >
            <template #export-button>
              <a-button type="success"> 导出 </a-button>
            </template>
          </VexTable>
          <!-- <DynamicTable
            row-key="report"
            :header-title="PageSetting.Title"
            :columns="PageSetting.Columns"
            showFavorite
            sticky
            bordered
            :row-class-name="getRowClassNmae"
            :data-source="PageSetting.moreData"
            :data-all="PageSetting.moreData"
            :pagination="{ pageSize: 20 }"
            :export-file-name="PageSetting.Title"
          >
            <template #export-button>
              <a-button type="success"> 导出 </a-button>
            </template>
          </DynamicTable> -->
        </div>
      </a-col>
    </a-row>
  </div>
</template>

<script lang="ts" setup>
  import { PageParams } from '@/components/core/dynamic-table/src/dynamic-table';
  import { useLoading } from '@/components/feat/Loading';
  import { GetCommon } from '@/api/pages/common';
  import { onMounted } from '@vue/runtime-core';
  import { reactive, ref } from '@vue/reactivity';
  import { useForm } from '@/components/core/schema-form';
  import { useTable } from '@/components/core/dynamic-table';
  import { ReportCommon } from '@/store/module/ReportCommon';
  import { message } from 'ant-design-vue';
  import { getRowClassNmae } from '@/pages/ReportPortal/utils/common';
  import VexTable from '@/components/core/vex-virtual-table/src/vex-virtual-table.vue';

  const UseParamsAdd = ReportCommon();
  let FactoryType: string = UseParamsAdd.Fab;
  const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...',
  });
  const report = ref<HTMLElement | null>(null);
  function getImport(site: string) {
    const {
      schemas,
      optionsdata,
    } = require(`@/pages/ReportPortal/FS/PanelBeolReport/${site}/form-schema`);
    const { baseColumns } = require(`@/pages/ReportPortal/FS/PanelBeolReport/${site}/Table`);
    return { schemas, optionsdata, baseColumns };
  }
  let { schemas, optionsdata, baseColumns } = getImport(FactoryType);

  let PageSetting = reactive<BasePageParams.BasePageSetting>({
    List: [] as any,
    Columns: [] as any,
    Title: '',
    moreData: [],
    SQLHidden: '',
    APIColumns: [] as any,
  });
  switch (FactoryType) {
    case 'LCM2':
      PageSetting.Title = 'PanelBluReport';
      break;
    default:
      break;
  }
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
    let baseParams = {
      apiName: 'PanelBluReport',
      FactoryType: FactoryType,
      G_FAC: UseParamsAdd.G_FAC_COM,
    };
    Object.keys(values).forEach((key) => {
      if (!values[key]) {
        values[key] = '';
      }
    });

    try {
      let { api01 } = await GetCommon({ ...baseParams, ...values });
      if (api01['Data']['Table']?.length <= 0) {
        closeFullLoading();
        return message.error('查無數據');
      }
      PageSetting.moreData = api01['Data']['Table'];
      PageSetting.Columns = baseColumns[values['Category']];

      closeFullLoading();
    } catch (error) {
      console.log(error);
      closeFullLoading();
    }
  }

  onMounted(async () => {});
</script>

<style lang="less" scoped></style>
