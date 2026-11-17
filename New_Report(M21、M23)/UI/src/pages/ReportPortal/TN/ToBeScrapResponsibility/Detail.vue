<template>
  <div>
    <a-card>
      <div v-bind="containerProps" style="height: 600px">
        <DynamicTable
          v-bind="wrapperProps"
          row-key="id"
          :header-title="PageSetting.TopTitle"
          :columns="PageSetting.Columns"
          :key="PageSetting.Columns"
          bordered
          size="small"
          :data-source="PageSetting.List"
          :pagination="false"
          export-file-name="Export"
          :scroll="PageSetting.TableSize"
          :row-class-name="getRowClassName"
        >
          <template #export-button>
            <a-button type="success"> Export </a-button>
          </template>
        </DynamicTable>
      </div>
    </a-card>
  </div>
</template>

<script lang="ts" setup>
  import { useTable } from '@/components/core/dynamic-table';
  import { reactive, watchEffect, watch, computed } from 'vue';
  import { GetCommon } from '@/api/pages/common';
  import { ReportCommon } from '@/store/module/ReportCommon';
  import { useVirtualList } from '@vueuse/core';

  function getImport(site: any) {
    const {
      detailColumns,
    } = require(`@/pages/ReportPortal/TN/ToBeScrapResponsibility/${site}/Table`);
    return { detailColumns };
  }
  let { detailColumns } = getImport('MOD');

  let useCommonParams = ReportCommon();
  let TableParams =
    useCommonParams['ToBeScrapResponsibility' + useCommonParams.Fab]['FirstTableParams'];

  let PageSetting = reactive({
    moreData: [],
    List: [] as any,
    Columns: detailColumns,
    TopTitle: 'To Be Scrap 責任歸屬 Report',
    TableSize: { x: 1000, y: -1 },
  });
  const [DynamicTable] = useTable();

  async function GetDetailData(TableParams: any) {
    let formParams = useCommonParams['ToBeScrapResponsibility' + useCommonParams.Fab]['FormParams'];
    let baseParams = {
      apiName: 'TN_ToBeScrapResponsibilityDetail',
      FactoryType: useCommonParams.Fab,
    };
    let DetailParams: any = {};
    DetailParams.detail_interval_date = TableParams.dataIndex?.replace('-DFCT_CODE', '') || '';
    DetailParams.detail_scrap_code = TableParams.SCRAP_CODE;
    DetailParams.detail_resp_owner = TableParams.RESP_OWNER;

    let { api01 } = await GetCommon({ ...baseParams, ...formParams, ...DetailParams });
    let pushdata = api01['Data']['Table'];
    let count = 0;
    pushdata.forEach((x) => {
      count += x.P_COUNT;
    });
    let allCount = {
        ENTITY_NO: '合計',
        P_COUNT : count
    }
    pushdata.push(allCount);
    PageSetting.moreData = pushdata;
  }


  function getRowClassName(record: Object) {
    if (Object.keys(record).length != 0) {
      for (const [key, value] of Object.entries(record)) {
        if (value == '合計') {
          return 'Green';
        } 
      }
    }
  }



  let onMoreDataLoad = computed(() => {
    if (PageSetting.moreData.length > 0) return PageSetting.moreData;
    else return [];
  });
  const { list, containerProps, wrapperProps } = useVirtualList(onMoreDataLoad, {
    itemHeight: 22,
  });
  watch(
    () => list.value,
    () => {
      let pushData: any[] = [];
      PageSetting.List = [];
      list.value.forEach((item) => {
        if (item.index < PageSetting.moreData.length) {
          pushData.push(item.data);
        }
      });
      PageSetting.List = pushData;
    },
  );
  watchEffect(() => {
    //一阶表格监控
    TableParams =
      useCommonParams['ToBeScrapResponsibility' + useCommonParams.Fab]['FirstTableParams'];
    GetDetailData(TableParams);
  });
</script>
