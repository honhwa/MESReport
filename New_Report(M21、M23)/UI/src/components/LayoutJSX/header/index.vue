<template>
    <Layout.Header :style="headerStyle" class="layout-header">
        <Space :size="20">
            <slot>
                <Space :size="20">
                    <span class="menu-fold" @click="() => emit('update:collapsed', !collapsed)">
                        <component :is="collapsed ? MenuUnfoldOutlined : MenuFoldOutlined" />
                    </span>
                    <!-- <Breadcrumb separator="/"> -->
                    <Breadcrumb :separator=separator >
                        <Breadcrumb.Item v-for="(routeItem, rotueIndex) in menus" :key="routeItem?.name"  :class="{ 'active': menus.length-1 === rotueIndex&&Border,'menustyle':Border }">
                            <!-- class="menustyle" :class="{ 'active': menus.length-1 === rotueIndex }"> -->
                            <!-- <span >{{ routeItem?.meta?.title }}</span> -->
                            <title-i18n :title="routeItem?.meta?.title"/>
                            <template v-if="routeItem?.children?.length" #overlay>
                                <Menu :selected-keys="getSelectKeys(rotueIndex, routeItem)">
                                    <template v-for="childItem in routeItem?.children" :key="childItem.name">
                                        <Menu.Item
                                            v-if="!childItem.meta?.hideInMenu && !childItem.meta?.hideInBreadcrumb"
                                            :key="childItem.name" @click="clickMenuItem(childItem)">
                                            <!-- <span>{{ childItem.meta?.title }}</span> -->
                                            <title-i18n :title="childItem.meta?.title"/>
                                        </Menu.Item>
                                    </template>
                                </Menu>
                            </template>
                        </Breadcrumb.Item>
                    </Breadcrumb>
                </Space>
            </slot>
        </Space>
        <Space :size="20">
            <Search />
            <FullScreen />
            <Tooltip title="锁定屏幕" placement="bottom">
                <LockOutlined @click="lockscreenStore.setLock(true)" />
            </Tooltip>
            <LocalePicker />
            <Dropdown placement="bottomRight">
                <Avatar src="/avatar.png" :alt="username.MemName" class="bootem">
                    {{ username.MemName }}
                </Avatar>

                <template #overlay>
                    <Menu>
                        <!-- <Menu.Item @click="toGitLab"> 关于 </Menu.Item> -->
                        <Menu.Item @click="$router.push({ name: 'account-settings' })"> 个人中心 </Menu.Item>
                        <Menu.Divider />
                        <Menu.Item>
                            <div @click.prevent="doLogout"><poweroff-outlined /> 退出登录</div>
                        </Menu.Item>
                    </Menu>
                </template>
            </Dropdown>
            <ProjectSetting />
        </Space>
    </Layout.Header>
</template>

<script lang="tsx" setup>
import { computed, nextTick, CSSProperties } from 'vue';
import { useRouter, useRoute, RouteRecordRaw } from 'vue-router';
import { useLockscreenStore } from '@/store/module/LockScreen';
import { WEB_HOST } from '@/pages/HomePage/constant';
import {
    QuestionCircleOutlined,
    MenuFoldOutlined,
    MenuUnfoldOutlined,
    PoweroffOutlined,
    LockOutlined
} from '@ant-design/icons-vue';
import { Layout, message, Modal, Dropdown, Menu, Space, Breadcrumb, Avatar, Tooltip, MenuTheme } from 'ant-design-vue';
import { ProjectSetting, Search, FullScreen } from './components/';
import { LocalePicker } from '@/components/basic/locale-picker';
import { useThemeStore } from '@/store/module/projectConfig';
import { useUserStore } from '@/store/module/user';
import { useKeepAliveStore } from '@/store/module/keepAlive';
import {tabsTheme} from './components/setting/constant'
import { TitleI18n } from '@/components/basic/title-i18n';
// import {menus1} from '@/router/staticRoutes'
import { Storage } from '@/utils/Storage';

defineProps({
    collapsed: {
        type: Boolean
    },
    theme: {
        type: String
    }
});
const lockscreenStore = useLockscreenStore();
const emit = defineEmits(['update:collapsed']);
const themeStore = useThemeStore();
const userStore = useUserStore();
const keepAliveStore = useKeepAliveStore();

let username = Storage.get('UserMessage', 'test');
const router = useRouter();
const route = useRoute();

const headerStyle = computed<CSSProperties>(() => {
    const { navTheme, layout } = themeStore;
    const isDark = navTheme == 'dark' && layout == 'topmenu';
    return {
        backgroundColor: navTheme === 'realDark' || isDark ? '' : 'rgba(255, 255, 255, 0.85)',
        color: navTheme === 'realDark' ? 'rgba(255, 255, 255, 0.85)' : ''
    };
});
const Border = computed(()=> !themeStore.noBorder)
const headHeight = computed(()=>themeStore.headHeight)
const separator = computed(()=>themeStore.separator)

function toGitLab() {
    window.open(WEB_HOST);
}
const menus = computed(() => {
    if (route.meta?.namePath) {
        let children = userStore.menus;
        const paths = route.meta?.namePath?.map((item) => {
            const a = children.find((n) => n.name === item);
            children = a?.children || [];
            return a;
        });
        return [
            {
                name: '__index',
                meta: {
                    title: '首页'
                },
                children: userStore.menus
            },
            ...paths
        ];
    }
    return route.matched;
});
// const menus = computed(() => {
//     if (route.meta?.namePath) {
//         let children = userStore.menus;
//         const paths = route.meta?.namePath?.map((item) => {
//             const a = children.find((n) => n.path === item);
//             children = a?.children || [];
//             // console.log(item,a,userStore.menus);

//             return a;
//         });
//         return [
//             {
//                 name: 'Home',
//                 meta: {
//                     title: '首页'
//                 },
//                 children: userStore.menus
//             },
//             ...paths
//         ];
//     }
//     return route.matched;
// });
const getSelectKeys = (rotueIndex: number, routeItem: any) => {
    return [menus.value[rotueIndex + 1]?.name] as string[];
};
/**我用的就是AnyScript。。。 */
//@ts-ignore
const findLastChild = (route?: RouteRecordRaw) => {
    if (typeof route?.redirect === 'object') {
        const redirectValues = Object.values(route.redirect);
        if (route?.children?.length) {
            const target = route.children.find((n) =>
                redirectValues.some((m) => [n.name, n.path, n.meta?.fullPath].some((v) => v === m))
            );
            return findLastChild(target);
        }
        return redirectValues.find((n) => typeof n === 'string');
    } else if (typeof route?.redirect === 'string') {
        if (route?.children?.length) {
            const target = route.children.find((n) =>
                [n.name, n.path, n.meta?.fullPath].some((m) => m === route?.redirect)
            );
            return findLastChild(target);
        }
        return route?.redirect;
    }
    return route;
};

// 点击菜单
const clickMenuItem = (menuItem: RouteRecordRaw) => {
    const lastChild = findLastChild(menuItem);

    if (lastChild?.name === route.name) return;
    if (/http(s)?:/.test(lastChild?.name)) {
        window.open(lastChild?.name);
    } else if (lastChild?.name) {
        router.push({ name: lastChild.name });
    }
};

// 退出登录
const doLogout = () => {
    Modal.confirm({
        title: '您确定要退出登录吗？',
        icon: <QuestionCircleOutlined />,
        centered: true,
        onOk: async () => {
            await userStore.logout();
            keepAliveStore.clear(); // 移除标签页
            localStorage.clear();
            message.success('成功退出登录');
            await nextTick();
            router.replace({
                name: 'Login',
                query: {
                    redirect: route.fullPath
                }
            });
            // window.location.reload()//之前有想过直接放弃挣扎,结果是我的keepAliveStore没有清空!为什么keepAliveStore
            //没清空会影响我页面跳转？
        }
    });
};
</script>

<style lang="less" scoped>
@headHeight:v-bind(headHeight);
.bootem:hover {
    border: 1px solid lightblue;
}

.layout-header {
    position: sticky;
    top: 0;
    z-index: 10;
    display: flex;
    // height: 64px;
    height: @headHeight;
    padding: 0 20px;
    justify-content: space-between;
    align-items: center;

    * {
        cursor: pointer;
    }
}

.menustyle{
    border: 1px solid black;
    padding: 6px;
    background-color: #ffc0cb;
}


.active  {
    background-color: #ee82ee !important;
    // color: gold;
}
</style>
