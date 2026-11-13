<template>
  <div class="RMA_ParaUP">
    <a-row>
      <a-col :span="24 - PageParams.TableSpan" class="baseModel" v-show="PageParams.ShowMenu">
        <!-- <SchemaForm1 @submit="confirm1" v-if="showTable"  class="ScrollForm"> </SchemaForm1> -->
        <!-- <SchemaForm @submit="confirm" v-if="showSelect" :showTextPosition="'center'"> </SchemaForm> -->
        <!-- <div v-if="showSelect">
                    <a-button type="primary" size="small " @click="SendQuery(2)">進入資料</a-button>
                    </div> -->
        <SchemaForm
          class="ScrollForm"
          @submit="confirm"
          v-if="showSelect"
          :showTextPosition="'center'"
          showUploadButton
          @upload="upload"
        >
        </SchemaForm>
        <div v-if="showSelect">
          <div :style="{ color: 'red', 'font-size': '16px' }">注意事項：</div>
          <div :style="{ color: 'red', 'font-size': '16px' }">1. 計劃版本號請不超過30個字符</div>
          <div :style="{ color: 'red', 'font-size': '16px' }">2. 計劃版本號請避免使用特殊字符</div>
        </div>
      </a-col>
      <a-col :span="PageParams.TableSpan">
        <div class="ScrollTable" ref="report">
          <DynamicTable
            row-key="RMA_ParaUP"
            header-title="Export RMA_ParaUP Data"
            :columns="PageSetting.Columns"
            showFavorite
            :PageParams="PageParams"
            :key="PageSetting.TopColumns"
            bordered
            size="small"
            :data-source="PageSetting.List"
            :data-all="PageSetting.moreData"
            :pagination="false"
            :scroll="{ x: PageSetting.x, y: 350 }"
            export-file-name="导出"
            :row-class-name="getRowClassNmae"
          >
            <template #export-button>
              <a-button type="success"> 导出 </a-button>
            </template>
          </DynamicTable>
        </div>
      </a-col>
      <UploadFile :visible="visible" @closemodal="closemodal" @uploadfile="uploadfile"></UploadFile>
    </a-row>
  </div>
</template>

<script lang="ts" setup>
  import { PageParams } from '@/components/core/dynamic-table/src/dynamic-table';
  import { useForm } from '@/components/core/schema-form';
  import { GetCommon, UploadCommon } from '@/api/pages/common';
  import { useTable } from '@/components/core/dynamic-table';
  import { reactive, ref } from '@vue/reactivity';
  import { onMounted } from '@vue/runtime-core';
  // import { GetCommon } from '@/api/pages/common';
  import { ReportCommonWithOut } from '@/store/module/ReportCommon';
  import { useLoading } from '@/components/feat/Loading';
  import { getRowClassNmae } from '@/pages/ReportPortal/utils/common';
  import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect';
  import { AddCommonPageParams } from '@/store/module/commonpage';
  import { cloneDeep } from 'lodash';
  import { useRouter } from 'vue-router';
  import UploadFile from './Upload.vue';
  import dayjs from 'dayjs';
  import axios from 'axios';
  import UploadExcell from '@/components/upload/UploadCommon.vue';
  const router = useRouter();
  import { message, Popconfirm, TypographyLink } from 'ant-design-vue';

  const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...',
  });
  const { InfiniteScroll } = useRemoteSelect();
  const UseParamsAdd = AddCommonPageParams();
  const ReportCommonParams = ReportCommonWithOut();
  let FactoryType: string = ReportCommonParams.Fab;
  const OQCInspectionDetailList = ref<HTMLElement | null>(null);
  let showTable = ref(true);
  let showSelect = ref(true);
  //文件上傳彈窗visible
  let visible = ref(false);
  function getImport(site: any) {
    const {
      schemas2,
      schemas,
      optionsdata,
    } = require(`@/pages/ReportPortal/FS/RMA_ParaUP/RMA/form-schema`);
    const {
      baseColumns1,
      baseColumns2,
      baseColumns3,
    } = require(`@/pages/ReportPortal/FS/RMA_ParaUP/RMA/Table`);
    return { schemas2, schemas, optionsdata, baseColumns1, baseColumns2, baseColumns3 };
  }
  let { schemas2, schemas, optionsdata, baseColumns1, baseColumns2, baseColumns3 } =
    getImport('LCM1');

  let PageSetting = reactive<BasePageParams.BasePageSetting>({
    List: [],
    APIColumns: [],
    Columns: baseColumns1,
    moreData: [],
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

  const editableData = reactive({});
  let selectData;
  async function confirm(values) {
    console.log('输出当前斷點', dayjs().format('MM'));
    openFullLoading();
    let baseParams = {
      apiName: 'ParaUPLOAD',
      FactoryType: UseParamsAdd.Fab,
      //G_FAC: UseParamsAdd.G_FAC_COM
    };
    Object.keys(values).forEach((key) => {
      if (!values[key]) {
        values[key] = '';
      }
    });
    try {
      let { api01 } = await GetCommon({ ...values, ...baseParams });
      console.log('输出当前baseParams', values);
      PageSetting.Title = 'Export PC Product Plan Set Data ';
      if (values['data_typ'] == '2') {
        PageSetting.Columns = baseColumns2;
      } else if (values['data_typ'] == '3') {
        PageSetting.Columns = baseColumns3;
      } else {
        PageSetting.Columns = baseColumns1;
      }
      console.log('输出当前月份输出当前baseParams', values['data_typ']);
      console.log('输出当前月份', PageSetting.Columns);
      PageSetting.moreData = api01['Data0']['Table'];
      if (PageSetting.moreData.length > 100) PageSetting.List = PageSetting.moreData.slice(0, 40);
      else PageSetting.List = PageSetting.moreData;

      closeFullLoading();
    } catch {
      closeFullLoading();
    }
  }

  const edit = (key: string) => {
    editableData[key] = cloneDeep(PageSetting.List.filter((item) => key === item['key'])[0]);
  };
  const save = (key: string) => {
    Object.assign(PageSetting.List.filter((item) => key === item['key'])[0], editableData[key]);
    message.success('修改成功！');
    delete editableData[key];
  };
  const cancel = (key: string) => {
    delete editableData[key];
  };
  // 文件上傳
  function upload(values) {
    // 存儲vux
    // ReportCommonParams.AddFormParams(`ProductPlanDataImport${FactoryType}`,values)
    // 頁面跳轉
    // router.push({name:`ProductPlanDataImport${FactoryType}`})
    visible.value = true;
    selectData = values;
  }
  const closemodal = () => {
    visible.value = false;
  };
  const uploadfile = async (fileList, formData) => {
    console.log('输出当前上傳斷點', dayjs().format('MM'));
    let baseParams = {
      apiName: 'ParaUPLOAD',
      FactoryType: UseParamsAdd.Fab,
      // G_FAC: ReportCommonParams.G_FAC_COM,
      //ReportType: 'OQCInspFrequencyImport',
      action: 'Import',
    };
    let temp = { ...baseParams, ...selectData };
    Object.keys(temp).forEach((key) => {
      formData.append(key, temp[key]);
    });
    console.log('输出当前baseParams', selectData);
    // Object.keys(values).forEach((key) => {
    //     if (!values[key]) {
    //         values[key] = '';
    //     }
    // });
    // for (var value of formData.values()) {
    //     console.log(value);
    // }
    console.log('输出当前上傳斷點formData', formData);
     let data;
    try {
        let { Data0:{Table:Data0}, Message, result } = await UploadCommon(formData);
      // data = await axios.post(process.env.VUE_APP_BASE_API + 'SQLAgent/ApiWork', formData, {
      //     headers: { 'content-type': 'application/x-www-form-urlencoded' }
      // });
      console.log("上傳",result);

      // const result = data.data.result;

      if ( result != '匯入Success') {
        return message.warn(result);
      }
      // const Data = data.data.Data0.Table;
      PageSetting.moreData = Data0;
      if (PageSetting.moreData.length > 100) {
        //预先加载100条数据
        PageSetting.List = PageSetting.moreData.slice(0, 100);
      } else {
        PageSetting.List = PageSetting.moreData;
      }
      message.success('文件上傳成功!');
    } catch (error) {
      return message.error('上傳失敗!請檢查文件是否爲空/文件是否為excell!');
    }
  };

  //实现无限滚动或者说懒加载
  InfiniteScroll(OQCInspectionDetailList, PageSetting);
  onMounted(async () => {
    await GetProDuctData();
  });

  async function GetProDuctData() {
    function getMonths(start: string, end: string) {
      let startyear = Number(start.split('-')[0]);
      let startMonth = Number(start.split('-')[1]);
      let endyear = Number(end.split('-')[0]);
      let endMonth = Number(end.split('-')[1]);
      let result: string[] = [];
      let year = startyear;
      let month = startMonth;
      while (year < endyear || (year === endyear && month <= endMonth)) {
        if (month >= 10) {
          result.push(`${year}${month}`);
        } else {
          result.push(`${year}0${month}`);
        }
        month++;
        if (month > 12) {
          year++;
          month = 1;
        }
      }
      return result;
    }
    let start = String(dayjs().year() - 1) + `-${dayjs().add(6, 'months').format('MM')}`;
    let end = String(dayjs().year()) + `-${dayjs().add(6, 'months').format('MM')}`;
    optionsdata.TimeData = getMonths(start, end).map((item) => {
      return { label: item, value: item };
    });

    async function getschmaData(FactoryType: string, apiName: string, paramsname: string) {
      let {
        api01: {
          Data: { Table: k },
        },
      } = await GetCommon({
        G_FAC: 'A',
        FactoryType: FactoryType,
        apiName: apiName,
      });
      return k.map((item) => {
        return { label: item[paramsname], value: item[paramsname] };
      });
    }
  }
</script>
<style lang="less" scoped>
  .btn-rows {
    button {
      margin-right: 12px;
    }
  }

  .OperationPerformance :deep(.v-center) {
    background-color: #f0f0f0;
  }

  .OperationPerformance :deep(.Yellow) {
    background-color: #80ff80;
  }

  .OperationPerformance :deep(.YellowGreen) {
    background-color: #f39230;
  }
</style>
