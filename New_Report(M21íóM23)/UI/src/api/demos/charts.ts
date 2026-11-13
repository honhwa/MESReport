// import type { BaseResponse } from '@/utils/request';
import { request } from '@/utils/request';

/**
 * @description 获取柱状图数据
 */
export function GetChartDemoData(query: {type:string}) {
  return request(
    {
      url: '/getReExamData',
      method: 'get',
      params: query,
    }
  );
}
export function GetChartDemoData1(query: {type:string}) {
  return request(
    {
      url: '/getReExamData',
      method: 'get',
      params: query,
    }
  );
}

