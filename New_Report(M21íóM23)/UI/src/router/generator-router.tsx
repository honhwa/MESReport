import { Result } from 'ant-design-vue';
import { notFound, errorRoute } from './staticModules/error';
import { REDIRECT_ROUTE } from './staticModules/besidesLayout';
import outsideLayout from './outsideLayout';
// import type { PermissionType } from '@/core/permission/modules/types';
import type { RouteRecordRaw } from 'vue-router';
import RouterView from '@/components/LayoutJSX/routerView/index.vue';
// import RouterPage from '@/pages/views/index.vue'
import { isUrl } from '@/utils/is';
import { uniqueSlash } from '@/utils/urlUtils';
// import { constantRouterComponents } from '@/router/asyncModules';
import common from '@/router/staticModules';
// import externaLink from '@/router/staticModules/externa-link'
import router, { routes } from '@/router';
import NotFound from '@/views/error/404.vue';
import { isObject } from '@/utils/is';


// 需要放在所有路由之后的路由
const endRoutes: RouteRecordRaw[] = [REDIRECT_ROUTE, errorRoute, notFound];

const importAll = (context) => {
  const map = {};
  for (const key of context.keys()) {
    const keyArr = key.replace('.', '');
    // keyArr.shift(); // 移除.
    map[keyArr] = context(key);
  }
  return map;
};
const PageMenus = importAll(require.context('@/pages', true, /\.vue|.tsx|.jsx$/));


export function filterAsyncRoute(
  routes: API.Menu[],
  parentRoute: API.Menu | null = null,
  lastNamePath: string[] = [],
): RouteRecordRaw[] {
  return routes
    .filter((item) => item.type !== 2 && item.parentId == parentRoute?.id)
    .map((item) => {

      const { router, viewPath, name, icon, orderNum, keepalive, id, REMARK, Explain, Owner, parentId } = item;
      let fullPath = '';
      const pathPrefix = lastNamePath.at(-1) || '';
      if (isUrl(router)) {
        fullPath = router;
      } else {
        fullPath = router.startsWith('/') ? router : `/${router}`;
        fullPath = router.startsWith(pathPrefix) ? fullPath : pathPrefix + fullPath;
        fullPath = [...new Set(uniqueSlash(fullPath).split('/'))].join('/');
      }
      let realRoutePath = router;
      if (parentRoute) {
        if (fullPath.startsWith(parentRoute?.router) && fullPath !== '') {
          realRoutePath = fullPath.split(parentRoute.router)[1];
        } else if (!isUrl(parentRoute.router) && !isUrl(router)) {
          realRoutePath = router;
        }
      }
      realRoutePath = realRoutePath.startsWith('/') ? realRoutePath.slice(1) : realRoutePath;
      let routername = REMARK ?? name
      const route: Partial<RouteRecordRaw> = {
        path: router,
        // name: `${viewPath ? toHump(viewPath) : fullPath}-${item.id}`,
        name: routername,
        meta: {
          orderNum,
          title: name,
          type: item.type,
          perms: item.perms,
          icon,
          hideInMenu: (String(item.isShow) == 'True') ? false : true,
          namePath: lastNamePath.concat(routername),
          keepAlive: keepalive,
          Explain,
          Owner,
          parentId,
          REMARK
        },
      };

      // 如果是目录
      if (item.type == 0) {
        const children = filterAsyncRoute(routes, item, lastNamePath.concat(routername));
        if (children?.length) {
          route.component = RouterView;
          route.children = children;
          route.redirect = { name: children[0].name };
        } else {
          route.component = (
            <Result
              status="500"
              title={routername}
              sub-title="目录类型菜单不是真实页面，请为当前目录添加页面级子菜单或更改当前菜单类型."
            />
          );
        }
        return route;
        // 如果是页面
      }
      else if (item.type == 1) {
        const children = filterAsyncRoute(routes, item, lastNamePath.concat(routername));
        if (children?.length) {
          route.component = RouterView;
          route.children = children;
          // route.redirect = { name: children[0].name };
        }
        // const compon = () => import(`${viewPath}`)
        // const Component = constantRouterComponents[viewPath] || NotFound;
        // component:()=>import(`@/views/JS/test1/index.vue`)
        // getDynamicRoute(router, route);
        /**
        *1.所有实际页面都只能写在src/pages目录下
        *2.所有组件不可写在pages目录下
        *3.未匹配成功组件会跳转至404
        */
        /**
         * 给组件的name属性赋值,用来进行keepalive
         */

        if (PageMenus[viewPath] &&PageMenus[viewPath].default) PageMenus[viewPath].default.name = id || 'NotFound'
        // console.log(menu);
        route.component = PageMenus[viewPath]?.default || NotFound
        return route;
      }


      return undefined;
    })
    .filter((item): item is RouteRecordRaw => !!item);
}


/**
 * 动态生成菜单
 * @param token
 * @returns {Promise<Router>}
 */
export const generatorDynamicRouter = (asyncMenus: API.Menu[]) => {
  try {
    //debugger
    const routepre = TransListData(asyncMenus);
    const routeList = filterAsyncRoute(routepre);

    const layout = routes.find((item) => item.name == 'DashBoardPage')!;
    // 给公共路由添加namePath
    generatorNamePath(common);
    // console.log(common);
    // const menus = [...externaLink, ...routeList, ...endRoutes, ...common];

    const menus = [...routeList, ...endRoutes, ...common];
    layout.children = menus;
    const addAllRoute = router.addRoute(layout);
    // 获取所有没有包含children的路由，上面addRoute的时候，vue-router已经帮我们拍平了所有路由
    const filterRoutes = router
      .getRoutes()
      .filter(
        (item) => {
          let notroute: any[] = []
          if (item.children.length > 0) {
            //当所有子菜单都不在菜单中显示时把根菜单添加进路由
            notroute = item.children.filter(itemno => !itemno?.meta?.hideInMenu)
          }
          if ((!notroute.length || Object.is(item.meta?.hideChildrenInMenu, true)) &&
            !outsideLayout.some((n) => n.name === item.name)) {
            if (item.children.length > 0) item.children = []
            return item
          }
        }
      );

    // 生成所有路由
    addAllRoute();
    //@ts-ignore
    layout.children = [...filterRoutes].filter(item => item.name != 'Home');
    // 重新添加拍平后的路由
    router.addRoute(layout);
    //console.log('所有路由', router.getRoutes(),layout,menus1);
    return Promise.resolve({
      menus,
      routes: layout.children,
    });
  } catch (error) {
    console.error('生成路由时出错', error);
    return Promise.reject(`生成路由时出错: ${error}`);
  }
};

export function TransListData(asyncMenus: any) {
  asyncMenus.filter(item => {
    if (item['parentId'] == process.env.VUE_APP_SYSID) {
      item['parentId'] = undefined
    }
    return item
  }
  )

  return asyncMenus

}

/**
 * 主要方便于控制a-menu的open-keys，即控制左侧菜单应当展开哪些菜单
 * @param {RouteRecordRaw[]} routes 需要添加namePath的路由
 * @param {string[]} namePath
 */
export const generatorNamePath = (
  routes: RouteRecordRaw[],
  namePath?: string[],
  parent?: RouteRecordRaw,
) => {
  routes.forEach((item) => {
    if (item.meta && typeof item.name === 'string') {
      item.meta.namePath = Array.isArray(namePath) ? namePath.concat(item.name) : [item.name];
      item.meta.fullPath = parent?.meta?.fullPath
        ? [parent.meta.fullPath, item.path].join('/')
        : item.path;
      item.meta.fullPath = uniqueSlash(item.meta.fullPath);

      if (item.children?.length) {
        generatorNamePath(item.children, item.meta.namePath, item);
      }
    }
  });
};
