import { RouteRecordRaw } from 'vue-router';
import RouterView from '@/components/LayoutJSX/routerView/index.vue';
// import OperationPerformance from '@/pages/OperationPerformance/Detail.vue'
const moduleName = 'permission';

const routes: Array<RouteRecordRaw> = [
  {
    path: `/${moduleName}`,
    component: RouterView,
    redirect: '/permission/menulist',
    meta: {
      title: '权限管理',
      // hideInMenu: false,//在菜单隐藏,实际进行导航
      icon: 'yonghu1'
    },
    children: [
      {
        path: 'menulist',
        name: `${moduleName}-menulist`,
        component: () => import(/* webpackChunkName: "menulist" */  '@/pages/permission/menu/index.vue'),
        meta: {
          title: '菜单列表',
          // hideInMenu: false, 
          icon: 'icon_shiyongwendang',
        },

      },
    ],
  },
  // {
  //   path: '/OperationPerformance/Detail',
  //   component: OperationPerformance,
  //   name:'OperationPerformanceDetail',
  //   meta: {
  //     title: 'OperationPerformanceDetail',
  //      hideInMenu: true,//在菜单隐藏,实际进行导航
  //     icon: 'yonghu1'
  //   },
  //   children: []
  // }
];

export default routes;
