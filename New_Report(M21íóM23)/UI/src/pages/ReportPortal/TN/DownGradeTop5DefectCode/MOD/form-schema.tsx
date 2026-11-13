import type { FormSchema } from '@/components/core/schema-form/';
// import { AddCommonPageParamsWithOut } from '@/store/module/commonpage';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil'

import {FromDateParams,ToDateParams,FloorParams} from '@/pages/ReportPortal/Static/TNCommonParams'
import {Size, Owner, DownGradeL2, ErrorCode, PanelSource} from '@/pages/ReportPortal/Static/TNCommonParams'

export const schemas = (FactoryType:string)=>{ 
  let optionsdata = reactive({});
  return [
    ...FromDateParams,
    ...ToDateParams,
    ...FloorParams, 
    ...new PanelSource(optionsdata,FactoryType,'PanelSource').obj,
    ...new Size(optionsdata, FactoryType).obj,
    ...new Owner(optionsdata,FactoryType,'owner_code').obj,
    ...new ErrorCode(optionsdata,FactoryType,'ErrorCode').obj,
    ...new DownGradeL2(optionsdata,FactoryType,'workGroup').obj,
  ];
};