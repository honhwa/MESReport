<template>
  <div class="vxetable">
    <ToolBar
      v-if="showToolBar"
      :title="headerTitle ?? tableSetting.headerTitle"
      :title-tooltip="titleTooltip"
      :OwnerParams="OwnerParams"
      :OwnerFoldParams="OwnerFoldParams"
      :show-table-setting="showTableSetting"
      :owner="tableSetting.owner"
      :PageParams="PageParams"
    >
      <template v-if="$slots.headerTitle" #headerTitle>
        <slot name="headerTitle"></slot>
      </template>
      <span class="Space" v-if="explain">
        <a-button
          type="primary"
          @click="tableSetting.isShowExplain = !tableSetting.isShowExplain"
          >{{
            tableSetting.isShowExplain
              ? $t('component.table.showTable')
              : $t('component.table.showExplain')
          }}</a-button
        >
      </span>

      <span v-if="exportFileName" class="ml-6px" @click="exportData2Excel">
        <slot name="export-button">
          <a-button type="primary">{{ $t('component.excel.exportModalTitle') }}</a-button>
        </slot>
      </span>
      <span class="Space" v-if="showFavorite">
        <a-button type="primary" @click="AddFavorite">{{
          $t('component.table.addFavorite')
        }}</a-button>
      </span>
      <template v-if="$slots.toolbar" #toolbar>
        <Space>
          <slot name="toolbar"></slot>
        </Space>
      </template>
    </ToolBar>
    <div v-if="tableSetting.isShowExplain" v-html="tableSetting.Explain"></div>
    <vxe-grid v-else ref="xTable2" v-bind="getBindValues" :height="height - 150"> </vxe-grid>
  </div>
</template>

<script lang="ts" setup>
  import { reactive, useSlots, computed } from 'vue';
  import { VxeGrid } from 'vxe-table';
  // import VxeTableGroup from './components/VxeTableGroup.vue';
  import { vexVirtualTableProps } from './vex-virtual-table';
  import { useRoute } from 'vue-router';
  import { Storage } from '@/utils/Storage';
  import { GetCommon } from '@/api/pages/common';
  import { message, Space } from 'ant-design-vue';
  import { ToolBar } from '@/components/core/dynamic-table/src/components';
  import { useTableMethods, useExportData2Excel, useTableState } from './hooks';
  import { useWindowSize } from '@vueuse/core';

  const { height } = useWindowSize();
  const slots = useSlots();
  const props = defineProps(vexVirtualTableProps);
  const tableState = useTableState({ props, slots });
  const tableMethods = useTableMethods({ state: tableState, props });
  const { getBindValues } = tableState;
  
  // const { getColumnKey, setProps, fetchData, handleSubmit, reload, handleTableChange, getComponent } = tableMethods;
  const UserParams = Storage.get('UserMessage');

  const instance = {
    ...props,
    ...tableState,
    ...tableMethods,
  };
  const route = useRoute();
  const tableSetting = reactive({
    Explain: route?.meta?.Explain ?? '',
    owner: route?.meta?.Owner ?? '',
    showFileModal: false,
    isShowExplain: false as boolean,
    headerTitle: route?.meta?.title,
  });

  const { exportData2Excel } = useExportData2Excel({
    props,
    state: tableState,
    methods: tableMethods,
    isMerge: false,
  });
  const AddFavorite = async () => {
    let params = {
      EMP_ID: UserParams['MemID'],
      EMP_NAME: UserParams['MemName'],
      ROUTE: route.fullPath,
      ID: route?.name,
      REP_NAME: route?.meta?.title,
      SYSMODULE: process.env.VUE_APP_SYSID,
      apiName: 'Favorite_Insert',
    };
    let { api01 } = await GetCommon(params);
    if (api01 == 'Success!') message.success('收藏成功!');
    else message.warning('请勿重复收藏');
  };

  defineExpose(instance);
</script>

<style scoped lang="less">
  .Space {
    margin-left: 7px;
  }
 
</style>
