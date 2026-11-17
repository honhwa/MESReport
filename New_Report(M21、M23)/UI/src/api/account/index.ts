import type { BaseResponse } from '@/utils/request';
import { request } from '@/utils/request';

export function updateAccountInfo(data: any) {
  return request<BaseResponse<any>>({
    url: '/api/Main/GetMenu',
    method: 'post',
    data,
  },
  {
    isGetDataDirectly: false,
    isSAM:true
  },);
}

export function updatePassword(data: any) {
  return request({
    url: 'account/password',
    method: 'post',
    data,
  });
}

export function VerifyToken(data: API.VerifyParams) {
  return request(
    {
      url: '/api/SSO/VerifyToken',
      method: 'post',
      data,
    },
    {
      isGetDataDirectly: false,
      isVerify:true
    },
  );
}




export function logout() {
  return request({
    url: 'account/logout',
    method: 'post',
  });
}
