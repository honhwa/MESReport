import type { FormSchema } from '@/components/core/schema-form/';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil';
import {LcmBySizeByModuleProdParams,LcmBySizeProdParams,FromDateParams,ToDateParams,InputWorkorderParams,FloorParams,WoTypeBySapParams,LcmEquipmentParams,
  OperationParams,ShiftParams,LcdOwnerParams,CustomerParams,LcmOwnerParams,SourceFabParams,ReworkedParams,IncludeBLParams,
  LcdGradeParams,FastReworkParams,LCMDefectCodeParams,IntervalParams,WorkOrderLikeParams,InputOperatorParams,LcmGradeParams,} from '@/pages/ReportPortal/Static/SHCommonParams'

  // export let SpecialCodeOptions = [
  //   {value: "ALL", label: "ALL" },
  //   {value: "PC000", label: "PC000" },
  //   {value: "PCZ00", label: "PCZ00" },
  //   {value: "PCZ72", label: "PCZ72" },
  //   {value: "PCZE4", label: "PCZE4" },
  // ];


  export const schemas = (FactoryType:string,optionsdata:any)=>{
    return [
  ...FromDateParams,
  ...ToDateParams,
  ...ShiftParams,
  ...FloorParams, 
  ...new LcmBySizeByModuleProdParams(optionsdata,FactoryType).getSize(),
  ...new LcmOwnerParams(optionsdata,FactoryType)._obj,
  ...new OperationParams(optionsdata,FactoryType)._obj,
];
}




