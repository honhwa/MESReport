import { RouteRecordRaw } from 'vue-router';
import { defineStore } from 'pinia';
// import { useWsStore } from './ws';
import { store } from '@/store';
import { login } from '@/api/login';
import { Storage } from '@/utils/Storage';
// import { logout } from '@/api/account';
import { resetRouter } from '@/router';
import { permmenu } from '@/api/system/menu';
import { generatorDynamicRouter } from '@/router/generator-router';
import { VerifyToken } from '@/api/account/index'
import { sortBy } from 'lodash-es';
import { GetCommon } from '@/api/pages/common';
import { formatToDate } from '@/utils/dateUtil';


interface UserState {
  token: string;
  name: string;
  avatar: string;
  // like [ 'sys:user:add', 'sys:user:update' ]
  perms: string;
  menus: RouteRecordRaw[];
  userInfo: Partial<API.AdminUserInfo>;
  routerPage: any[]
}

export const useUserStore = defineStore({
  id: 'user',
  state: (): UserState => ({
    token: Storage.get('ACCESS_TOKEN', null),
    name: 'amdin',
    avatar: 'CARUX',
    perms: '',
    menus: [],
    userInfo: {},
    routerPage: [],
  }),
  getters: {
    getToken(): string {
      return this.token;
    },
    getAvatar(): string {
      return this.avatar;
    },
    getName(): string {
      return this.name;
    },
    getPerms(): string {
      return this.perms;
    },
  },
  actions: {
    /** 清空token及用户信息 */
    resetToken() {
      this.avatar = this.token = this.name = '';
      this.perms = '';
      this.menus = [];
      this.userInfo = {};
      Storage.clear();
    },
    /** 登录成功保存token */
    setToken(token: string) {
      this.token = token ?? '';//看作三元表达式的简写，双问号语法
      // const ex = 2 * 60 * 60 * 1000;//不设置过期时间
      Storage.set('ACCESS_TOKEN', this.token, null);
    },
    async login(params: API.LoginParams) {
      try {
        const {data}  = await login(params);
        this.setToken(data);
        let VerifyParams = { Token: this.token, SysID: params.SysID }
        const userInfo = await VerifyToken(VerifyParams);
        Storage.set('UserMessage', {...userInfo.data,...{LoginDate:formatToDate()}}, null);
        // Storage.set('LoginDate',formatToDate(),null)
        return this.afterLogin();
      } catch (error:any) {
        if(error?.response.status!="401"){
          Storage.set('IsSSO', true, 60 * 1000);
        }
        return Promise.reject(error.response?.data || { message: error });
      }
    },
    /** 登录成功之后, 获取用户信息以及生成权限路由 */
    async afterLogin() {
      try {
        
        let UserParams = Storage.get('UserMessage');
        let GetMenuParams = { apiName: 'GetSysData', SYSID: process.env.VUE_APP_SYSID!, EMP_ID: UserParams['MemID'], EMP_NAME: UserParams['MemName'] }
        let GetRoleParams = { apiName: 'GetRole', EMP_ID: UserParams['MemID'], EMP_NAME: UserParams['MemName'], BACKUP: '', EMP_PERMS: '' };
        let [{api01}, Data2] = await Promise.all([permmenu(GetMenuParams), GetCommon(GetRoleParams)]);
        this.perms = Data2['api01']['Table'][0]['EMP_PERMS']??''
        let hideInPage = api01.filter((item: object, index: number) => {
          item['orderNum'] = parseInt(item['orderNum']) ?? index;
          return item['keepalive']=='True'&&item
        });
        let Data = sortBy(hideInPage, ['orderNum']);
        const generatorResult = await generatorDynamicRouter(Data);
        if (Data.length <= 0) return Promise.reject({ message: '对不起,获取节点失败,请联系管理员或者重新登录!' });
        this.menus = generatorResult.menus.filter((item: any) => !item.meta?.hideInMenu);
        let menus = this.menus;
      
        
        this.routerPage = generatorResult.menus;
        return { menus };
      } catch (error) {
        // window.localStorage.clear();
        // window.location.reload();

        return Promise.reject(error);
        // return this.logout();
      }
    },
    /** 登出 */
    async logout() {
      // await logout();
      // const wsStore = useWsStore();
      // wsStore.closeSocket();
      this.resetToken();
      resetRouter();
    },
    setAvatar(avatar:string){
      this.avatar = avatar;
    }
  },
});

// 在组件setup函数外使用
export function useUserStoreWithOut() {
  return useUserStore(store);
}
