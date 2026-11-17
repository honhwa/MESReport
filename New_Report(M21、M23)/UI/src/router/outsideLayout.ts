import { LOGIN_NAME,LOGOUT_NAME } from '@/router/constant';


/**
 * layout布局之外的路由
 */
export const LoginRoute = [
  {
    path: '/login',
    name: LOGIN_NAME,
    component: () => import(/* webpackChunkName: "login" */ '@/pages/login/index.vue'),
    meta: {
      title: '登录',
    },
  },
  {
    path: '/logout',
    name: LOGOUT_NAME,
    component: () => import(/* webpackChunkName: "logout" */ '@/pages/login/logout.vue'),
    meta: {
      title: '退出登录',
    },
  },
  {
    path: '/search',
    name: 'search',
    component: () => import(/* webpackChunkName: "HomePage" */ '@/pages/search/index.vue'),
    meta: {
      title: 'search',
    },
  },
  {
    path: '/error/404',
    name: 'error',
    component: () => import(/* webpackChunkName: "404" */ '@/views/error/404.vue'),
    meta: {
      title: '错误页',
      icon: 'EditOutlined',
    },

  },

];

export default LoginRoute;
