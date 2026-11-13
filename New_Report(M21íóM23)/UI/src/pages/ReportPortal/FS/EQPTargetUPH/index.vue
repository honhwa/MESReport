<template>
  <div>
    <a-row>
      <a-col :span="24 - PageParams.TableSpan" class="ScrollForm">
        <div class="LA501">
          <div>請輸入密碼:</div>
          <InputPassword v-model:value="PageSetting.PassWord" />
          <DatePicker valueFormat="YYYYMM" v-model:value="PageSetting.Month" picker="month" />
          <a-button @click="Submit" type="primary">查詢</a-button>
          <a-button @click="UploadExcel501" type="primary">匯入</a-button>
        </div>
      </a-col>
      <a-col :span="PageParams.TableSpan">
        <VexTable
          :PageParams="PageParams"
          :header-title="PageSetting.Title"
          :columns="PageSetting.Columns"
          :key="PageSetting.Columns"
          :tableData="PageSetting.moreData"
          :data="PageSetting.moreData"
          :export-file-name="PageSetting.Title"
          :flexwidth="10"
        >
          <template #export-button>
            <a-button type="success"> 导出 </a-button>
          </template>
        </VexTable>
      </a-col>
    </a-row>
    <upload-excel
      :maxNumber="1"
      :accept="['xlsx', 'xls']"
      v-model:visible="visible"
      :uploadParams="params"
    ></upload-excel>
  </div>
</template>

<script lang="tsx" setup>
  import { PageParams } from '@/components/core/dynamic-table/src/dynamic-table';
  import { useLoading } from '@/components/feat/Loading';
  import { GetCommon } from '@/api/pages/common';
  import { reactive, ref } from '@vue/reactivity';
  import { ReportCommon } from '@/store/module/ReportCommon';
  import { DatePicker, InputPassword } from 'ant-design-vue';
  import { formatToDate } from '@/utils/dateUtil';
  import { LA501BaseColumns } from './BEOL1/Table';
  import VexTable from '@/components/core/vex-virtual-table/src/vex-virtual-table.vue';
  import UploadExcel from '@/pages/updownfile/upexcel/directUploadExcel.vue';

  const UseParamsAdd = ReportCommon();
  let FactoryType: string = UseParamsAdd.Fab;
  const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...',
  });
  let visible = ref(false);
  let PageSetting = reactive({
    List: [] as any,
    Columns: [] as any,
    Title: '',
    moreData: [],
    SQLHidden: '',
    APIColumns: [] as any,
    PassWord: '',
    SelectPage: false,
    Month: formatToDate('YYYYMM'),
  });
  let params = reactive({
    PassWord: '',
    G_FAC: 'A',
    FactoryType,
    apiName: 'EQPTargetUPHDataSet',
    QueryType: 'Query',
    month: PageSetting.Month,
  });

  const Submit = async () => {
    openFullLoading();
    try {
      params.QueryType = 'Query';
      params.PassWord = PageSetting.PassWord;
      // if(FactoryType=='BEOL2'){
      //   params.QueryType = 'Query1';
      // }
      let { api01 } = await GetCommon(params);
      let ReturnData =FactoryType=='BEOL1'? api01['Data']['Table']:api01['Data']
      let ApiColumns: any[] = [
        { title: 'Tact_Time_Target', dataIndex: 'Tact_Time_Target', children: [] },
        { title: 'Tact_Time_Standard', dataIndex: 'Tact_Time_Standard', children: [] },
      ];
      ReturnData.forEach((item) => {
        for (let title in item) {
          // str = str.replace(/'/g, '');
          if (title.indexOf('_') == -1) {
            if (item['CATG_CD'] && item['CATG_CD'].trim() == '1TRG') {
              item[`s_${title}`] = item[title];
            } else {
              item[`e_${title}`] = item[title];
            }
            delete item[title];
          }
        }
      });
      api01['title'].forEach((item) => {
        if (item['field'].indexOf('s_') != -1) {
          ApiColumns[0]['children'].push(item);
        } else {
          ApiColumns[1]['children'].push(item);
        }
      });
      PageSetting.Columns = [...LA501BaseColumns, ...ApiColumns];
      const result = ReturnData.reduce((acc, cur) => {
        const item = acc.find(
          (item) => item['EQUIP_ID'] == cur['EQUIP_ID'] && item['PROC_ID'] == cur['PROC_ID'],
        );
        if (item) {
          Object.assign(item, cur);
        } else {
          acc.push(cur);
        }
        return acc;
      }, []);
      PageSetting.moreData = result;
      console.log(api01);
      
      closeFullLoading();
    } catch(error) {
      console.log(error);
      
      closeFullLoading();
    }
    // Data.fi
  };
  const UploadExcel501 = () => {
    params.QueryType = 'Update';
    params.PassWord = PageSetting.PassWord;
    visible.value = true;
  };
</script>

<style lang="less" scoped>
  .LA501 {
    display: flex;
    flex-direction: column; /* 子节点以 column 方向排列 */
  }

  .LA501 > * {
    margin: 0px 10px 10px 10px; /* 上下间距 20px */
  }
  .LA501 > *:last-child {
    margin-bottom: 0; /* 最后一个子节点不需要下边距 */
  }
  .LA501 > *:first-child {
    margin-top: 15px;
    color: red;
    font-size: 16px;
  }
</style>
