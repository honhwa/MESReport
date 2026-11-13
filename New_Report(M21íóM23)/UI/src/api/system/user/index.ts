import { request } from '@/utils/request';
// import type { BaseResponse } from '@/utils/request';

export function getUserInfo(data:any) {
  return request({
    url: '/SQLAgent/ApiWorkMulti',
    method: 'post',
    data
  });
}

// export function getUserList(data?: API.PageParams) {
//   return request<API.UserListResult>({
//     url: Api.list,
//     method: 'get',
//     data,
//   });
// }

// export function getUserListByPage(query: API.PageParams) {
//   return request({
//     url: Api.page,
//     method: 'get',
//     params: query,
//   });
// }

export function createUser(data: API.CreateUserParams) {
  return request(
    {
      url: '/SQLAgent/ApiWorkMulti',
      method: 'post',
      data,
    },
    {
      successMsg: '创建用户成功',
    },
  );
}

export function updateUser(data: API.UpdateUserParams) {
  return request(
    {
      url: '/SQLAgent/ApiWorkMulti',
      method: 'post',
      data,
    },
    {
      successMsg: '更新用户成功',
    },
  );
}

export function deleteUser(data: API.DeleteUserParams) {
  return request(
    {
      url: '/SQLAgent/ApiWorkMulti',
      method: 'post',
      data,
    },
    {
      successMsg: '删除用户成功',
    },
  );
}
