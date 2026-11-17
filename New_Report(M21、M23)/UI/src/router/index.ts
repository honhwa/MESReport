import { App } from 'vue';
import 'nprogress/css/nprogress.css';

import { createRouter, createWebHashHistory } from 'vue-router';

import { createRouterGuards } from './router-guards';

import outsideLayout from './outsideLayout';
// import { notFound, errorRoute } from './staticModules/error';
import { whiteNameList } from './constant';
import type { RouteRecordRaw } from 'vue-router';


// const endRoutes: RouteRecordRaw[] = [errorRoute, notFound];
export const routes: Array<RouteRecordRaw> = [
  
  // {
  //   path: `/Home`,
  //   name: `Home`,
  //   component: ()=>import(/* webpackChunkName: "Home" */  '@/pages/HomePage/Home'),
  //   meta: {
  //     title: '首页',
  //     icon: 'icon-externa-link',
  //   },
  // },
  {
    path: '/',
    name: 'DashBoardPage',
    redirect: '/DashBoard',
    component: () => import(/* webpackChunkName: "DashBoardPage" */ '@/components/LayoutJSX/index.vue'),
    meta: {
      title: 'DashBoardPage'
    },
    children: [
    ]
  }, 
  ...outsideLayout,
];

export const router = createRouter({
  // process.env.BASE_URL
  history: createWebHashHistory(),
  routes,
});

// reset router
export function resetRouter() {
  router.getRoutes().forEach((route) => {
    const { name } = route;
    if (name && !whiteNameList.some((n) => n === name)) {
      router.hasRoute(name) && router.removeRoute(name);
    }
  });
}

export async function setupRouter(app: App) {
  // 创建路由守卫

  createRouterGuards(router, whiteNameList);

  app.use(router);

  // 路由准备就绪后挂载APP实例
  await router.isReady();
}

export default router
