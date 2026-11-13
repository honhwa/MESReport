declare namespace API {
  /** 新增用户 */
  type CreateUserParams = API.APIBaseParams & API.UserListResultItem
  /** 更新用户 */
  type UpdateUserParams = API.APIBaseParams & {
    EMP_ID: string;
    EMP_NAME: string;
    EMP_PERMS?: string;

  };

  type DeleteUserParams = API.APIBaseParams & {
    EMP_ID: string ;
    EMP_NAME?: string;

  }

  type DayliMoveThItem = {
    work_ctr: string;
    wo_nbr: string;
    size1: string;
    lcm_owner: string;
    prod_nbr: string;
    customer: string;
  };
  type OperationPerformanceThItem = {
    WORK_CTR: string;
    IN_QTY: string;
    PASS_QTY: string;
    WIP_QTY: string;
    HOLD_WIP_QTY: string;
    NHOLD_WIP_QTY: string;
    TD: string;
    TR: string;
    RWK_OUTQTY: string;
    TOTALRWK_QTY: string;
    FirstYield: string;
    TotalYield: string;
    RWK_IN_QTY: string;
    RWK_PASS_QTY: string;
    RWK_WIP_QTY: string;
    RWK_HOLDWIP_QTY: string;
    RWK_NHOLDWIP_QTY: string;
    TDR: string;
    TRR: string;
    RWK_TORWK_QTY: string;
    MTO: string;
    TOM: string;
    RepairRate: string;
    RWK_TOMAIN_QTY: string;
    TOBESCRAP_QTY: string;
    PASS2_QTY: string;
    PASS2_QTYRate: string;
    S1_WIP_QTY: string;
    S2_WIP_QTY: string;
    S3_WIP_QTY: string;
  };
/** 用户列表项 */
type UserListResultItem = {
  EMP_ID: string;
  EMP_NAME: string;
  PASSWORD: string;
  COUNT: string;
  EMP_AD: string;
  EMP_PERMS: string;
  BACKUP: string;
  SHOW_TOKEN?:boolean;
};
type RoleListItem = {
  ROLE: string;
  ROLELEVEL: string;
  BACKUP: string;
  PERSON: string;
};
  /** 用户列表 */
  type UserListResult = UserListResultItem[];

  /** 用户详情 */
  type UserInfoResult = {
    Table:UpdateUserParams[];
    UserInfo: {
      EMP_ID: string;
      EMP_NAME: string;
      EMP_PERMS: string;
    };
    menus: {
      createTime: string;
      updateTime: string;
      id: number;
      UserId: number;
      menuId: number;
    }[];
    depts: {
      createTime: string;
      updateTime: string;
      id: number;
      UserId: number;
      departmentId: number;
    }[];
  };

  type CreateMasterFlow = {
    remarks: string;
    remote: string
  }
}
