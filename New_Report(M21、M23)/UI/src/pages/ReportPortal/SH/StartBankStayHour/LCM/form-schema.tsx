import type { FormSchema } from '@/components/core/schema-form/';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil'
import {LcmBySizeByModuleProdParams,FromDateParams,ToDateParams,InputWorkorderParams,FloorParams,
  OperationParams,ShiftParams,LcmBySizeProdParams,StartHoursParams,LcdOwnerParams,LcdGradeParams, AbnormalCodeParams} from '@/pages/ReportPortal/Static/SHCommonParams'




  export const schemas = (FactoryType:string,optionsdata:any)=>{
    return [
  ...StartHoursParams,
  ...FloorParams,
  ...AbnormalCodeParams, 
    ...new LcdOwnerParams(optionsdata,FactoryType)._obj,
 ];
}
