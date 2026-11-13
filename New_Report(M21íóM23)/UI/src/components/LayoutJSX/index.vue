<template>
    <Layout class="layout">
        <Layout.Sider v-if="themeStore.layout === 'sidemenu'" :collapsed="collapsed" :width="asiderWidth"
            :trigger="null" collapsible :theme="getTheme" class="layout-sider">
            <Logo :collapsed="collapsed" />
            <AsideMenu :collapsed="collapsed" :theme="getTheme" class="asidemenu" />
        </Layout.Sider>
        <Layout>
            <PageHeader v-model:collapsed="collapsed" :theme="getTheme">
                <!--子组件向父组件传值-->
                <template v-if="themeStore.layout === 'topmenu'" #default>
                    <Logo :collapsed="collapsed" />
                    <AsideMenu :collapsed="collapsed" :theme="getTheme" class="asidemenu" />
                </template>
            </PageHeader>
            <Layout.Content class="layout-content tabs-view-content">
                <Loading :loading="compState.loading" :absolute="compState.absolute" :theme="compState.theme"
                    :background="compState.background" :tip="compState.tip" />
                <tabs-view ref="domRef" />
                <!-- <router-view v-slot="{ Component }">
                    <template v-if="Component">
                        <transition
                                :name="Object.is(route.meta?.transitionName, false) ? '' : 'fade-transform'"
                                mode="out-in"
                                appear
                            >
                        <keep-alive>
                            <component :is="Component" />
                        </keep-alive>
                        </transition>
                    </template>
                </router-view> -->
            </Layout.Content>
            <!-- <PageFooter /> -->
        </Layout>
    </Layout>
</template>

<script lang="ts" setup>
import Answer from '@/components/Answer/index.vue';
import { ref, computed, provide, reactive } from 'vue';
import { Layout } from 'ant-design-vue';
import Logo from './logo/index.vue';
import AsideMenu from './menu/menu.vue';
import PageHeader from './header/index.vue';
import PageFooter from './footer';
import { TabsView } from './tabs';
// import Button from '@/views/test1/index.vue'
import content from '@/views/index.vue';
import { useThemeStore } from '@/store/module/projectConfig';
import { Loading } from '@/components/feat/Loading';

const domRef = ref<Nullable<HTMLElement>>(null);
provide('domRef', domRef);


const compState = reactive({
    absolute: true,
    loading: false,
    theme: 'dark',
    background: 'rgba(111,111,111,.7)',
    tip: '加载中...'
});
const themeStore = useThemeStore();
const collapsed = ref<boolean>(true);
// 自定义侧边栏菜单收缩和展开时的宽度
const asiderWidth = computed(() => (collapsed.value ? 80 : 220));
const getTheme = computed(() => (themeStore.navTheme == 'light' ? 'light' : 'dark'));
</script>

<style lang="less" scoped>
.layout {
    display: flex;
    height: 100vh;
    overflow: hidden;

    .ant-layout {
        overflow: hidden;
    }

    .layout-content {
        // background-color: aliceblue;
        // flex: none;
        // display: flex;
        // flex: row;
    }

    .tabs-view-content {
        height: calc(100vh - 110px);
        // padding: 20px 14px 0;
        overflow: auto;
    }

   
}
// 设置菜单样式
:deep(.ant-menu,.ant-menu-sub,.ant-menu-inline){
  font-size: 16px;
//   background-color: #044d50 !important;
}
/* // 设置子菜单展开样式
:deep(.ant-menu-submenu > .ant-menu ){
    // color: red;
//   background-color: rgb(16, 71, 83) !important;
}
:deep(.ant-menu-submenu-title){
    // color: red;
} */

</style>
