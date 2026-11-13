<template>
    <!-- 目录 -->
    <template v-if="isShowSubMenu">
        <Menu.SubMenu :key="props.menuInfo?.name" v-bind="$attrs">
            <template #title>
                <span>
                    <icon-font :type="props.menuInfo.meta?.icon" />
                    <span>{{ props.menuInfo?.meta?.title }}</span>
                </span>
            </template>
            <template v-for="item in menuChildren" :key="item.name || item.fullPath">
                <!-- 递归生成菜单 -->
                <MyMenuItem :menu-info="item" />
            </template>
        </Menu.SubMenu>
    </template>
    <!-- 菜单 -->
    <template v-else>
        <Menu.Item :key="props.menuInfo?.name">
            <icon-font :type="props.menuInfo?.meta?.icon" />
            <span>{{ props.menuInfo?.meta?.title }}</span>
        </Menu.Item>
    </template>
</template>
<script lang="ts">
export default {
    name: 'MyMenuItem'
};
</script>

<script setup lang="ts">
import { PropType, computed } from 'vue';
import { Menu } from 'ant-design-vue';
import type { RouteRecordRaw } from 'vue-router';
import { IconFont } from '@/components/basic/icon-font/index';

// defineOptions({
//   name: 'MyMenuItem',
// });

const props = defineProps({
    menuInfo: {
        type: Object as PropType<RouteRecordRaw>
    }
});

const menuChildren = computed(() => {
    return [...(props.menuInfo?.children || [])]
        .filter((n) => !n.meta?.hideInMenu)
        .sort((a, b) => (a?.meta?.orderNum || 0) - (b?.meta?.orderNum || 0));
});

const isShowSubMenu = computed(() => {
    let notroute: any[] = []
    const menuInfo = props.menuInfo;
    //@ts-ignore
    if (menuInfo.children && menuInfo.children.length > 0) {
        //当所有子菜单都不在菜单中显示时把根菜单添加进路由
        //@ts-ignore
        notroute = menuInfo.children.filter(itemno => !itemno?.meta?.hideInMenu)
        if (notroute.length == 0) return false

    }
    return menuInfo?.meta?.type === 0 || (!Object.is(menuInfo?.meta?.hideChildrenInMenu, true) && menuInfo?.children?.length);

});
</script>

<style scoped>

</style>
