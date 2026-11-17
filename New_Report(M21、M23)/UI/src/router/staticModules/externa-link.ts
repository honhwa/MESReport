import {  RouteRecordRaw } from 'vue-router';


const routes: Array<RouteRecordRaw> = [
  // {
  //   path: `${window.location.origin}/#/Home`,
  //   name: `${window.location.origin}/#/Home`,
  //   component: RouterView,
  //   meta: {
  //     title: '首页',
  //     icon: 'icon-externa-link',
  //   },
  // },
  {
    path: `/Home`,//仅仅为了添加菜单,不是实际路由,目的是跳出子路由导航
    name: `Home`,
    component: ()=>import(/* webpackChunkName: "Home" */  '@/pages/HomePage/Home'),
    meta: {
      title: '首页',
      icon: 'icon-externa-link',
    },
  },
  
];

export default routes;
