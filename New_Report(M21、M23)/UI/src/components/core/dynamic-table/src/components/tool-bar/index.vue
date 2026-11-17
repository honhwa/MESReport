<template>
    <div style="display: flex; justify-content: space-between; padding: 3px">
        <slot name="headerTitle">
            <div class="owner"  v-if="OwnerFoldParams" @click="showHidden(PageParams)">
                <span class="menu-fold" >
                    <component :is="!PageParams.ShowMenu ? MenuUnfoldOutlined : MenuFoldOutlined" />
                </span>
                <span v-if="OwnerParams">Owner:{{ owner }}</span>
            </div>
            <div class="title">
                {{ title }}
                <BasicHelp v-if="titleTooltip" class="ml-6px pt-3px" :text="titleTooltip" />
            </div>
        </slot>
        <!-- windicss layer base  -->
        <div>
            <slot name="toolbar"></slot>
            <slot></slot>
            <Divider v-if="$slots.toolbar && showTableSetting" type="vertical" />
            <TableSetting v-if="showTableSetting" />
        </div>
    </div>
</template>

<script lang="ts" setup>
import { MenuFoldOutlined, MenuUnfoldOutlined } from '@ant-design/icons-vue';
import { Divider } from 'ant-design-vue';
import TableSetting from '../table-settings/index.vue';
import BasicHelp from '@/components/basic/basic-help/index.vue';
import { PageShowHidden } from '../../types/setting';
import { showHidden } from '@/pages/ReportPortal/utils/common';

defineProps({
    title: {
        type: String,
        default: ''
    },
    titleTooltip: {
        type: [Array, String] as PropType<string[] | string>,
        default: ''
    },
    showTableSetting: {
        type: Boolean,
        default: true
    },
    owner: {
        type: String as PropType<string>,
        default: ''
    },
    PageParams: {
        type: Object as PropType<PageShowHidden>,
        default: () => ({})
    },    
    OwnerParams: {
       type: Boolean as PropType<boolean>,
       default: true,
    },
    OwnerFoldParams: {
       type: Boolean as PropType<boolean>,
       default: true,
    }


});
</script>

<style lang="less" scoped>
.title {
    display: flex;
    align-items: center;
    justify-content: flex-start;
    font-weight: 500;
    font-size: 16px;
}
.owner {
    font-weight: 500;
    font-size: 16px;
    cursor: pointer;
}
.menu-fold {
    font-size: 14px;
}
</style>
