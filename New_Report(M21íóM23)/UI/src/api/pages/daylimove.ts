import { request } from '@/utils/request';
// import type { BaseResponse } from '@/utils/request';

export function GetDailyMove(data:{}) {
    return request({
        url:"/SQLAgent/ApiWorkMulti",
        method: 'post',
        data
    });
}


export function GetBigProduct(data:any) {
    return request({
        url: '/api/Common/Big Product',
        method: 'get',
        data
    });
}

export function GetProduct(data:any) {
    return request({
        url: '/api/Common/Product',
        method: 'get',
        data
    });
}
export function GetSize(data:any) {
    return request({
        url: '/api/Common/Size',
        method: 'get',
        data
    });
}