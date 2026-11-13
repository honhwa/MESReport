import { isNavigationFailure, RouteLocationNormalized } from 'vue-router';
import NProgress from 'nprogress'; // progress bar
import { WhiteNameList, LOGIN_NAME, REDIRECT_NAME } from './constant';
import type { Router } from 'vue-router';
import { useUserStore } from '@/store/module/user';
import { useKeepAliveStore } from '@/store/module/keepAlive';
import { Storage } from '@/utils/Storage';
import { to as _to } from '@/utils/awaitTo';
import { AddCommonPageParamsWithOut } from '@/store/module/commonpage';
// import { menus1 } from '@/router/staticRoutes';
import { ReportCommonWithOut } from '@/store/module/ReportCommon';

NProgress.configure({ showSpinner: false }); // NProgress Configuration

const defaultRoutePath = '/';
// const UseRecord = AnswerRecordWithOut();
const UseCommonParams = AddCommonPageParamsWithOut();
const ReportCommonParams = ReportCommonWithOut();

export function createRouterGuards(router: Router, whiteNameList: WhiteNameList) {
    router.beforeEach(async (to, from, next) => {
        const userStore = useUserStore();
        NProgress.start(); // start progress bar
        const token = Storage.get('ACCESS_TOKEN', null);
        if (token) {
            if (to.name === LOGIN_NAME) {
                next({ path: defaultRoutePath });
                NProgress.done();
                //console.log(3);
            } else {
                const hasRoute = router.hasRoute(to.name!);
                //! 操作符,在我们明确知道to.name的类型不是undefind时，让ts也明确知道类型不为undefined

                if (userStore.menus.length === 0) {
                    //if (menus1.length === 0) {
                    // 从后台获取菜单
                    const [err] = await _to(userStore.afterLogin());
                    if (err) {
                        userStore.resetToken();
                        return next({ name: LOGIN_NAME });
                    }
                    if (!hasRoute) {
                        // 请求带有 redirect 重定向时，登录自动重定向到该地址
                        const redirect = decodeURIComponent((from.query.redirect || '') as string);
                        if (to.path === redirect) {
                            //console.log(5);
                            next({ ...to, replace: true });
                        } else {
                            // console.log(6);
                            // 跳转到目的路由
                            next({ ...to, replace: true });
                        }
                    }

                    if (whiteNameList.some((n) => n === to.name) || hasRoute) {
                        // 在免登录名单，直接进入
                        // console.log(7);
                        next();
                    }
                } else {
                    // console.log(4,to,router);
                    next();
                }
            }
        } else {
            // not login
            if (whiteNameList.some((n) => n === to.name)) {
                // 在免登录名单，直接进入
                //console.log(1);
                next();
            } else {
                //console.log(2);

                next({ name: LOGIN_NAME, query: { redirect: to.fullPath }, replace: true });
                NProgress.done(); // if current page is login will not trigger afterEach hook, so manually handle it
            }
        }
    });

    /** 获取路由对应的组件名称 */
    const getComponentName = (route: RouteLocationNormalized) => {
        return route.matched.find((item) => item.name === route.name)?.components?.default.name;
    };

    router.afterEach((to, from, failure) => {
        let Fab = to?.fullPath.split('/')[2] ?? 'LCM1';
        let SubFab = to?.fullPath.split('/')[3];
        let G_FAC_COM: string = '';
        let G_CLASS:string = '';
        if (process.env.VUE_APP_SYSID == 'NGBUI') {
            if (Fab.indexOf('LCM') >= 0) {
                if (Fab == 'LCM3') {
                    G_FAC_COM = 'E';
                } else if (Fab == 'LCM1') {
                    G_FAC_COM = 'C';
                } else if (Fab == 'LCM4') {
                    G_FAC_COM = 'D';
                } else if (Fab == 'LCM2') {
                    G_FAC_COM = 'B';
                }
            } else if (Fab.indexOf('BL') >= 0) {
                if (Fab == 'BL3') {
                    G_FAC_COM = 'E';
                } else if (Fab == 'BL1') {
                    G_FAC_COM = 'C';
                } else if (Fab == 'BL4') {
                    G_FAC_COM = 'D';
                } else if (Fab == 'BL2') {
                    G_FAC_COM = 'B';
                }
            } else if (Fab.indexOf('BEOL') >= 0) {
                if (Fab == 'BEOL1') {
                    G_FAC_COM = 'C';
                } else if (Fab == 'BEOL2') {
                    G_FAC_COM = 'D';
                }
            } else {
                G_FAC_COM = Fab;
            }
            // G_FAC_COM = Fab == 'LCM3' ? 'E' : 'C';
        } else if (process.env.VUE_APP_SYSID == 'FSUI') {
            G_FAC_COM = 'A';
            if (Fab == 'LCM2') {
                G_CLASS="M"
            }
        }else if (process.env.VUE_APP_SYSID == 'SHUI') {
            if (Fab.indexOf('LCM') >= 0) {
                if (Fab == 'LCM3') {
                    G_FAC_COM = 'E';
                } else if (Fab == 'LCM1') {
                    G_FAC_COM = 'C';
                } else if (Fab == 'LCM4') {
                    G_FAC_COM = 'D';
                } else if (Fab == 'LCM2') {
                    G_FAC_COM = 'B';
                }
            } else if (Fab.indexOf('BL') >= 0) {
                if (Fab == 'BL3') {
                    G_FAC_COM = 'E';
                } else if (Fab == 'BL1') {
                    G_FAC_COM = 'C';
                } else if (Fab == 'BL4') {
                    G_FAC_COM = 'D';
                } else if (Fab == 'BL2') {
                    G_FAC_COM = 'B';
                }
            } else if (Fab.indexOf('BEOL') >= 0) {
                if (Fab == 'BEOL1') {
                    G_FAC_COM = 'C';
                } else if (Fab == 'BEOL2') {
                    G_FAC_COM = 'D';
                }
            } else {
                G_FAC_COM = Fab;
            }
        }
        UseCommonParams.AddFabType(Fab, G_FAC_COM,G_CLASS);
        ReportCommonParams.AddFabType(Fab, G_FAC_COM, SubFab,G_CLASS);
        // console.log(UseCommonParams.Fab, to?.fullPath.split('/'));

        const keepAliveStore = useKeepAliveStore();
        const token = Storage.get('ACCESS_TOKEN', null);
        document.title = to.meta.title ?? 'CUP';
        // UseRecord.ChangeCompleteStatus(false);//在离开当前页面后清除成绩,暂时屏蔽不让清除
        // UseRecord.ChangeRecord(0);
        // UseRecord.ClearAnswer()
        if (isNavigationFailure(failure)) {
            console.error('failed navigation', failure);
        }
        // 在这里设置需要缓存的组件名称
        const componentName = getComponentName(to);
        // 判断当前页面是否开启缓存，如果开启，则将当前页面的 componentName 信息存入 keep-alive 全局状态
        if (to.meta?.keepAlive == 'True') {
            // 需要缓存的组件
            if (componentName) {
                keepAliveStore.add(componentName);
            } else {
                console.warn(`${to.fullPath}页面组件的keepAlive为true但未设置组件名，会导致缓存失效，请检查`);
            }
        } else {
            // 不需要缓存的组件
            if (componentName) {
                keepAliveStore.remove(componentName);
            }
        }
        // 如果进入的是 Redirect 页面，则也将离开页面的缓存清空
        if (to.name === REDIRECT_NAME) {
            const componentName = getComponentName(from);
            componentName && keepAliveStore.remove(componentName);
        }
        // 如果用户已登出，则清空所有缓存的组件
        if (!token) {
            keepAliveStore.clear();
        }

        NProgress.done(); // finish progress bar
    });

    router.onError((error) => {
        console.log(error, '路由错误');
    });
}
