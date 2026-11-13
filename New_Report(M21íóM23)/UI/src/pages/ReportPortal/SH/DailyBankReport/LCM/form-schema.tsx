import type { FormSchema } from '@/components/core/schema-form/';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil';
import {LcmBySizeByModuleProdParams,FromDateParams,ToDateParams,InputWorkorderParams,FloorParams,
  OperationParams,ShiftParams,LcmBySizeProdParams, LcmOwnerParams,} from '@/pages/ReportPortal/Static/SHCommonParams'


export const schemas = (FactoryType:string,optionsdata:any)=>{ 
 return [
  ...FromDateParams,
  ...ToDateParams,
  ...ShiftParams,
  ...FloorParams,
  ...new LcmBySizeProdParams(optionsdata,FactoryType).getSize(), 
  ...new LcmOwnerParams(optionsdata,FactoryType)._obj,
];
}
