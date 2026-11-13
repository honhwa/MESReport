import axios from 'axios';
import { message as $message } from 'ant-design-vue';
import type { AxiosRequestConfig } from 'axios';
import { Storage } from '@/utils/Storage';
import { cloneDeep } from 'lodash-es';

// import { useUserStore } from '@/store/module/user';
// import {ExclamationCircleOutlined} from '@ant-design/icons'
/**为什么要进行請求封装
 * 对请求做统一处理，易于携带参数,如token,empno
 * 对api传回的错误通过响应拦截进行统一处理
 * 独立数据来源,易于切换
 * 重复调用某个接口时直接引入方法即可,更加方便
 * 代码更规范整洁,易于管理
 */

export interface RequestOptions {
  /** 当前接口权限, 不需要鉴权的接口请忽略， 格式：sys:user:add */
  permCode?: string;
  /** 是否直接获取data，而忽略message等 */
  isGetDataDirectly?: boolean;
  /** 请求成功是提示信息 */
  successMsg?: string;
  /** 请求失败是提示信息 */
  errorMsg?: string;
  /** 是否属于sam网址*/
  isSAM?: boolean;
  /** 是否属于Verify网址 */
  isVerify?: boolean;
  /** 是否时文件上传 */
  isUpload?: boolean;
}

const UNKNOWN_ERROR = '未知错误，请重试';
const PARAMS_ERROR = '请确认节点ID与排序号是否重复'
// 是否生产环境
// const IS_PROD = ['production', 'prod'].includes(process.env.NODE_ENV);
/** 真实请求的路径前缀 */
const baseApiUrl = process.env.VUE_APP_BASE_API;
/** SAM的路径前缀 */
const baseSAMUrl = process.env.VUE_APP_SAM
/** VerifyToken的前缀 */
const baseVerifyUrl = process.env.VUE_APP_VERIFYTOKEN

const service = axios.create({
  // baseURL: baseApiUrl,
  /** 等待时间 */
  // timeout: 6000,
});

service.interceptors.request.use(
  (config) => {
    const UserParams = Storage.get('USERMESSAGE');
    // let token = btoa(encodeURIComponent(UserParams?.MemName??'hello'));
    let token = btoa(process.env.VUE_APP_SYSID+UserParams?.MemID+UserParams?.LoginDate ?? 'null')
    console.log(config);
    
    if (token && config.headers) {
      // 请求头token信息，请根据实际情况进行修改
      if(process.env.VUE_APP_SITE=='SH'){
          if(!config.url?.includes("VerifyToken")){
            config.headers['reporttoken'] = token;
          }
       }else{
         config.headers['reporttoken'] = token;
       }
    }
    if (config.headers&&process.env.VUE_APP_SITE=='FS') config.headers['ticket'] = 'Yes'
    return config;
  },
  (error) => {
    Promise.reject(error);
  },
);

service.interceptors.response.use(
  (response) => {
    const res = response;
    // if the custom code is not 200, it is judged as an error.
    if (res.status !== 200) {
      $message.error(res.status || UNKNOWN_ERROR);
      if (res.status === 11001 || res.status === 11002 || res.status === 9001) {
        window.localStorage.clear();
        window.location.reload();
        // to re-login
        // Modal.confirm({
        //   title: '警告',
        //   content: res.message || '账号异常，您可以取消停留在该页上，或重新登录',
        //   okText: '重新登录',
        //   cancelText: '取消',
        //   onOk: () => {
        //     localStorage.clear();
        //     window.location.reload();
        //   }
        // });
      }

      // throw other
      const error = new Error(res.statusText || UNKNOWN_ERROR) as Error & { code: any };
      error.code = res.status;
      return Promise.reject(error);
    } else {
      return res;
    }
  },
  (error) => {
    // debugger
    // 处理 422 或者 500 的错误异常提示
    // const errMsg = error?.response?.data?.message ?? PARAMS_ERROR;
    const errMsg = error?.message ?? PARAMS_ERROR;
    $message.error('未查詢到相關數據!');
    error.message = errMsg;
    return Promise.reject(error);
  },
);

export type Response<T = any> = {
  code: number;
  message: string;
  data: T;
};

export type BaseResponse<T = any> = Promise<Response<T>>;

/**
 *
 * @param method - request methods
 * @param url - request url
 * @param data - request data or params
 */
export const request = async <T = any>(
  config: AxiosRequestConfig,
  options: RequestOptions = {},
): Promise<T> => {
  try {
    const { successMsg, errorMsg, isVerify, isSAM, isUpload, isGetDataDirectly = true } = options;
    // 如果当前是需要鉴权的接口 并且没有权限的话 则终止请求发起
    // if (permCode && !useUserStore().perms.includes(permCode)) {
    //   return $message.error('你没有访问该接口的权限，请联系管理员！');
    // }
    let url = ''
    if (isVerify) url = baseVerifyUrl!
    else if (isSAM) url = baseSAMUrl!
    // else if (isSetMock) url = 'http://127.0.0.1:5000/'
    else url = baseApiUrl!

    if (url == baseApiUrl!) {
      config.headers = {
        'Content-Type': isUpload ? 'multipart/form-data;charset=UTF-8' : 'application/json;charset=utf-8', //配置请求头，注意Post请求与get请求的请求头可能会不一致,
        Accept: 'application/json, text/plain, */*',
      };
      if (['/SQLAgent/ApiWorkMulti'].includes(config.url!)) {


        let tempconfigdata = cloneDeep(config.data);
        let keyArr: any[] = [];
        let apiJob: any;
        let apiName: any;
        if (Object.prototype.toString.call(tempconfigdata) === '[object Object]') {
          for (let key in tempconfigdata) {
            if (Array.isArray(tempconfigdata[key])) {
              tempconfigdata[key] = tempconfigdata[key].join("','");
            }
            keyArr.push(key);
          }
          apiJob = [tempconfigdata];
        } else if (Array.isArray(tempconfigdata)) {
          tempconfigdata.forEach(item => {
            for (let key in item) {
              if (Array.isArray(item[key])) {
                item[key] = item[key].join("','");
              }
              keyArr.push(key);
            }
          })
          keyArr = Array.from(new Set(keyArr));
          apiJob = [...tempconfigdata];
        } else {
          apiName = tempconfigdata
        }
        let temp = JSON.stringify(apiJob);
        // console.log(temp);
        // console.log(keyArr);

        // for (let i = 0; i < keyArr.length; i++) {
        //   let tempitem1 = `"${keyArr[i]}"`;
        //   let tempitem2 = keyArr[i];
        //   // let reg = new RegExp(tempitem1, "g");避免键值一样，而g是全局替换
        //   let reg = new RegExp(tempitem1);

        //   if (temp.search(reg)) {
        //     let tempreaplace = temp.replace(reg, tempitem2);
        //     temp = tempreaplace;
        //   }
        // }
        if (temp) tempconfigdata = "apiJob=" + temp;
        else tempconfigdata = "apiName=" + apiName;
        config.data = tempconfigdata;
        if (config.method == 'get') {
          config.params = tempconfigdata;
        }


      }


    }

    const fullUrl = `${url + config.url}`;
    config.url = fullUrl.replace(/(?<!:)\/{2,}/g, '/');
    // if (IS_PROD) {
    //   // 保持api请求的协议与当前访问的站点协议一致
    //   config.url.replace(/^https?:/g, location.protocol);
    // }
    const res = await service.request(config);
    successMsg && $message.success(successMsg);
    errorMsg && $message.error(errorMsg);


    return isGetDataDirectly ? res.data : res;
  } catch (error: any) {
    return Promise.reject(error);
  }
};
