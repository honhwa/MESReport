<template>
    <div>
        <SchemaForm
            v-if="search"
            ref="queryFormRef"
            class="bg-white dark:bg-black mb-16px pt-24px pr-24px"
            submit-on-reset
            v-bind="getFormProps"
            :table-instance="tableAction"
            @toggle-advanced="(e) => $emit('toggle-advanced', e)"
            @submit="handleSubmit"
            :showTextPosition="'right'"
        >
            <template v-for="item in getFormSlotKeys" #[replaceFormSlotKey(item)]="data">
                <slot :name="item" v-bind="data || {}"></slot>
            </template>
        </SchemaForm>
        <div class="bg-white dark:bg-black">
            <ToolBar
                v-if="showToolBar"
                :title="headerTitle??tableSetting.headerTitle"
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
                    <a-button type="primary" @click="tableSetting.isShowExplain = !tableSetting.isShowExplain">{{
                        tableSetting.isShowExplain ? '显示表格' : '显示说明文件'
                    }}</a-button>
                </span>
                <!-- <span class="Space"
                    ><a-button type="primary" @click="tableSetting.showFileModal = true" v-if="explain && EditExplain"
                        >编辑说明文件</a-button
                    ></span
                > -->
                <span v-if="exportFileName" class="ml-6px" @click="exportData2Excel">
                    <slot name="export-button">
                        <a-button type="primary">导出</a-button>
                    </slot>
                </span>
                <span class="Space" v-if="showFavorite">
                    <a-button type="primary" @click="AddFavorite">收藏本頁</a-button>
                </span>
                <template v-if="$slots.toolbar" #toolbar>
                    <Space>
                        <slot name="toolbar"></slot>
                    </Space>
                </template>
            </ToolBar>
            <div v-if="tableSetting.isShowExplain" v-html="tableSetting.Explain"></div>
            <Table v-else ref="tableRef" v-bind="getBindValues" :data-source="tableData" @change="handleTableChange">
                <template v-for="slotName in defaultSlotsData" #[slotName]="slotData" :key="slotName">
                    <slot :name="slotName" v-bind="slotData"></slot>
                </template>

                <!-- 个性化单元格 start -->
                <template v-for="slotName in ['bodyCell', 'headerCell']" #[slotName]="slotData" :key="slotName">
                    <slot :name="slotName" v-bind="slotData"></slot>
                    <!-- 表格行操作start -->
                    <template v-if="slotName === 'bodyCell' && getColumnKey(slotData.column) === '$action'">
                        <TableAction :actions="slotData.column.actions(slotData)" />
                    </template>
                    <!-- 表格行操作end -->
                    <template
                        v-for="slotItem in getBindValues.columns?.filter((item) => item[slotName])"
                        :key="getColumnKey(slotItem)"
                    >
                        <component
                            :is="getComponent(slotItem?.[slotName]?.(slotData))"
                            v-if="getColumnKey(slotData.column) === getColumnKey(slotItem)"
                        />
                    </template>
                </template>
                <!-- 个性化单元格 end -->
            </Table>
        </div>
        <!-- 为什么不使用useFormModal hooks用以生成modal?会有一个警告或者报错无法处理 -->
        <!-- 新修改:将编辑说明文件放在菜单管理中 <DraggableModal
            v-model:visible="tableSetting.showFileModal"
            title="富文本编辑"
            :keyboard="false"
            @cancel="() => (tableSetting.showFileModal = false)"
            :width="1200"
        >
            <Tinymce @change="ChangeModelValue" :model-value="tableSetting.Explain"></Tinymce>
            <template #footer>
                <a-button @click="() => (tableSetting.showFileModal = false)" danger>取消</a-button>
                <a-button @click="submitExplain" type="success">保存</a-button>
            </template>
        </DraggableModal> -->
    </div>
</template>

<script lang="tsx" setup>
import { useSlots, ref, computed, reactive } from 'vue';
import { Table, Space, message } from 'ant-design-vue';
import { useTableMethods, createTableContext, useExportData2Excel, useTableForm, useTableState } from './hooks';
import { TableAction, ToolBar } from './components';
import { dynamicTableProps, defaultSlots, dynamicTableEmits, ExplainSchemas, DynamicTableProps } from './dynamic-table';
import { TableActionType } from './types';
import { SchemaForm } from '@/components/core/schema-form';
import { useRoute } from 'vue-router';
// import { useFormModal } from '@/hooks/useModal/useFormModal';
import { Storage } from '@/utils/Storage';
import { GetCommon } from '@/api/pages/common';
import { useUserStore } from '@/store/module/user';

// import {toolBarProps} from '@/components/core/dynamic-table/src/components'

// defineOptions({//下载了unplugin-vue-define-options但是还会报错defineOptions,我猜也是因为vue脚手架版本不对
//   name: 'DynamicTable',
//   inheritAttrs: false,
// });
const userStore = useUserStore();
// const [showModal] = useFormModal();
const UserParams = Storage.get('UserMessage');
const route = useRoute();
const props = defineProps(dynamicTableProps);
// const ToolbarProps = defineProps(toolBarProps)
const emit = defineEmits(dynamicTableEmits);
const slots = useSlots();
let defaultSlotsData = defaultSlots.filter((name) => Reflect.has(slots, name));
// 表格内部状态
const tableState = useTableState({ props, slots });
const { tableRef,tableData, queryFormRef, getBindValues } = tableState;


// 表格内部方法
const tableMethods = useTableMethods({ state: tableState, props, emit });
const { getColumnKey, setProps, fetchData, handleSubmit, reload, handleTableChange, getComponent } = tableMethods;

// 搜索表单
const { getFormProps, replaceFormSlotKey, getFormSlotKeys } = useTableForm({
    tableState,
    tableMethods,
    slots
});

const tableSetting = reactive({
    Explain: route?.meta?.Explain ?? '',
    owner:  route?.meta?.Owner ?? '',
    showFileModal: false,
    isShowExplain: false as boolean,
    headerTitle:route?.meta?.title,
});


// 当前组件所有的状态和方法
const instance = {
    ...props,
    ...tableState,
    ...tableMethods
};


const AddFavorite = async () => {
    let params = {
        EMP_ID: UserParams['MemID'],
        EMP_NAME: UserParams['MemName'],
        ROUTE: route.fullPath,
        ID: route?.name,
        REP_NAME: route?.meta?.title,
        SYSMODULE: process.env.VUE_APP_SYSID,
        apiName: 'Favorite_Insert'
    };
    let { api01 } = await GetCommon(params);
    if (api01 == 'Success!') message.success('收藏成功!');
    else message.warning('请勿重复收藏');
};

// 表单导出
const { exportData2Excel } = useExportData2Excel({
    props,
    state: tableState,
    methods: tableMethods,
    isMerge:false
});

const tableAction: TableActionType = {
    setProps,
    reload,
    fetchData
};
//编辑说明文件按钮只有admin可以看见
const EditExplain = computed(() => {
    return userStore.perms == 'Admin';
});

createTableContext(instance);

fetchData();

defineExpose(instance);
</script>

<style lang="less" scoped>
.Space {
    margin-left: 7px;
}



:deep(.ant-table-wrapper) {
    padding: 0 6px 6px 6px;
    .ant-table {       
        .ant-table-title {
            display: flex;
        }

        .ant-table-bordered {
            border: 1px solid black;
        }

        .ant-table-small .ant-table-thead > tr > th {
            padding: 1px !important;
        }

        .ant-image:hover {
            cursor: zoom-in;
        }

        .ant-btn {
            margin-right: 10px;
        }
    }
}

.actions > * {
    margin-right: 10px;
}
</style>
