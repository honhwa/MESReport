import { request } from '@/utils/request';
import { defHttp } from '@/utils/http/axios';

/**
 * @description 登录
 * @param {LoginParams} data
 * @returns
 */
// export function login(data:any) {
//   return request(
//     {
//       url: '/getSetLoginData/',
//       method: 'post',
//       data,
//     },
//     {
//       isSetMock:true
//     },
//   );
// }
// export function login(data:any) {
//   return request(

//     {
//       url: '/api/SSO/Login',
//       method: 'post',
//       data,
//     },
//     {
//       isGetDataDirectly: false,
//       isVerify:true
//     },

//   );
// }
/**
 * @description 获取验证码
 */
export function getImageCaptcha(params?: API.CaptchaParams) {
  return request<API.CaptchaResult>({
    url: 'captcha/img',
    method: 'get',
    params,
  });
}
/**
 * 
 * @param params 
 * @returns token
 */
export function login(
  params: any,
) {
  return defHttp.request<any>(
    {
      url: '/api/SSO/Login',
      params,
      method:'POST',
      
    }, {
    apiUrl: process.env.VUE_APP_VERIFYTOKEN,
    isReturnNativeResponse:true
  }
  )
}
