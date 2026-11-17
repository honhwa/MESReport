
import { request } from '@/utils/request';

export function GetCommon(data:{}) {
    return request({
        url:"/SQLAgent/ApiWorkMulti",
        method: 'post',
        data
    });
}