import { request } from '@/utils/request';

export function GetBigProduct(data:any) {
    return request({
        url: '/api/Common/Big Product',
        method: 'get',
        data
    });
}