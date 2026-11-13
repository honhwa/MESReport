import { request } from '@/utils/request';

export function permmenu(data: API.GetMenuParams) {
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


export function createMenu(data: API.MenuAddParams) {
  return request(
    {
      url: '/SQLAgent/ApiWorkMulti',
      method: 'post',
      data,
    },
    {
      // isSetMock: true,
      successMsg: '创建成功',
    },
  );
}

export function updateMenu(data:API.MenuUpdateParams) {
  return request(
    {
      url: '/SQLAgent/ApiWorkMulti',
      method: 'post',
      data,
    },
    {
      // isSetMock: true,
      successMsg: '更新成功',
    },
  );
}

export function deleteMenu(data:API.MenuDeleteParams) {
  return request(
    {
      url: '/SQLAgent/ApiWorkMulti',
      method: 'post',
      data,
    },
    {
      // isSetMock: true,
      successMsg: '删除成功',
    },
  );
}
