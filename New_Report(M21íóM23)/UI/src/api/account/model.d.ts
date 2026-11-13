declare namespace API {
  type Menu = {
    createTime: Date;
    updateTime: Date;
    id: string;
    parentId: string;
    name: string;
    router: string;
    perms: string;
    /** 当前菜单类型 0: 目录 | 1: 菜单 | 2: 权限 | 3:路由页面 */
    type: 0 | 1 | 2 | 3;
    icon: string;
    orderNum: number;
    viewPath: string;
    keepalive: string;
    isShow: string;
    REMARK:string;
    Explain:string;
    Owner?:string;
  };
  type SamMenu = {
    funDesc: string;
    funDesc_E: string;
    funName: string;
    funName_E: string;
    iconClass: string;
    isVlogByPortal: boolean;
    nodeID: string;
    parentID: string;
    sortIndex: number;
    target: string;
    url: string;
    version: string;
  }

  type PermMenu = {
    menus: Menu[];
    perms: string[];
  };

  type AdminUserInfo = {
    createTime: Date;
    updateTime: Date;
    id: number;
    departmentId: number;
    name: string;
    username: string;
    password: string;
    psalt: string;
    nickName: string;
    headImg: string;
    loginIp: string;
    email: string;
    phone: string;
    remark: string;
    status: number;
    roles: number[];
    departmentName: string;
  };
}
