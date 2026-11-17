declare namespace API {
  type MenuListResultItem = {
    createTime: string;
    updatedAt: string;
    id: string;
    parentId: string;
    name: string;
    router: string;
    perms: string;
    type: number;
    icon: string;
    orderNum: number;
    viewPath: string;
    keepalive: string;
    isShow: string;
    keyPath?: number[];
    REMARK?: string;
    Explain?:string;
    title?:string;
    Owner?:string;
  };

  /** 获取菜单列表参数 */
  type MenuListResult = MenuListResultItem[];

  /** 新增菜单参数 */
  type MenuAddParams = {
    type: number;
    parentId: number;
    name: string;
    orderNum: number;
    router: string;
    isShow: boolean;
    keepalive: boolean;
    icon: string;
    perms: string;
    viewPath: string;
    id: string;
    SYSID?: string;
    REMARK?: string;
    Owner?: string;
    Explain?: string;
    role?:string|string[];
  };

  /** 更新某项菜单参数 */
  type MenuUpdateParams = API.APIBaseParams & MenuAddParams & {
    menuId: string;
  };

  /** 获取菜单详情结果 */
  type MenuInfoResult = {
    menu: {
      createTime: string;
      updateTime: string;
      id: number;
      parentId: number;
      name: string;
      router: string;
      perms: string;
      type: number;
      icon: string;
      orderNum: number;
      viewPath: string;
      keepalive: boolean;
      isShow: boolean;
    };
    parentMenu: {
      createTime: string;
      updateTime: string;
      id: number;
      parentId: number;
      name: string;
      router: string;
      perms: string;
      type: number;
      icon: string;
      orderNum: number;
      viewPath: string;
      keepalive: boolean;
      isShow: boolean;
    };
  };

  /** 删除某个菜单或页面 */
  type MenuDeleteParams =API.APIBaseParams & {
    id: string;
    orderNum: number;
    SYSID: string; 
    perms: string;
  }
}
