import {  RouteRecordRaw } from 'vue-router';
import RouterView from '@/components/LayoutJSX/routerView/index.vue';

const moduleName = 'account';

const routes: Array<RouteRecordRaw> = [
  {
    path: '/account',
    component: RouterView,
    redirect: '/account/settings',
    meta: {
      title: '个人中心',
      hideInMenu: true,//在菜单隐藏,实际进行导航
    },
    children: [
      {
        path: 'settings',
        name: `${moduleName}-settings`,
        component: () => import('@/pages/personalpage/index.vue'),
        meta: { title: '个人中心', hideInMenu: true },
      },
      // {
      //   path: 'about',
      //   name: `${moduleName}-about`,
      //   component: () => import('@/views/account/about.vue'),
      //   meta: { title: '关于', hideInMenu: true },
      // },
    ],
  },
 
];

export default routes;
