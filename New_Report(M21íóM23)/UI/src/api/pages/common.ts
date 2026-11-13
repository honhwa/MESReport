import { request } from '@/utils/request';
import { UploadFileParams } from '../../../types/axios';
import { defHttp } from '@/utils/http/axios';



export function GetCommon(data: {}) {
    return request({
        url: '/SQLAgent/ApiWorkMulti',
        method: 'post',
        data
    });
}
export function UploadCommon(data: {}) {
    return request(
        {
          // 走ApiWorkMulti參數被處理了，所以用ApiWork
            url: '/SQLAgent/ApiWork',
            method: 'post',
            data
        },
        {
           // isUpload: true
        }
    );
}
// export function UploadFile(data:FormData) {
//     return request({
//         url:"/api/Interface/UploadFiles",
//         method: 'post',
//         data
//     },{
//       isUplaod:true
//     });
// }
export function UploadFile(params: UploadFileParams, onUploadProgress: (progressEvent: ProgressEvent) => void) {
    return defHttp.uploadFile<any>(
        {
            url: 'api/Interface/UploadFiles',
            onUploadProgress
        },
        params
    );
}
export function UploadExcel(params: UploadFileParams, onUploadProgress: (progressEvent: ProgressEvent) => void) {
    return defHttp.uploadFile<any>(
        {
            url: params.url,
            onUploadProgress
        },
        params
    );
}

export function GetFileList(data: any) {
    return request({
        url: '/api/Interface/GetItems',
        method: 'post',
        data
    });
}
export function DownLoadFile(data: any) {
    return request({
        url: '/api/Interface/DownloadFile',
        method: 'get',
        data
    });
}

export function RemoveFile(data: any) {
    return request(
        {
            url: '/api/Interface/DeleteFile',
            method: 'post',
            data
        },
        {
           // isUpload: true
        }
    );
}
