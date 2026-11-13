import { request } from '@/utils/request';

export function GetRoleList(data: any) {
    return request(
        {
            url: '/SQLAgent/ApiWorkMulti',
            method: 'post',
            data,
        },
        {
            // isSetMock: true
        },
    );
}


export function UpdateRoleFun(data: any) {
    return request(
        {
            url: '/SQLAgent/ApiWorkMulti',
            method: 'post',
            data,
        },
        {
            successMsg: '更新成功!',
        },
    );

}