import type { FormSchema } from '@/components/core/schema-form/';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil';
import {LcmBySizeByModuleProdParams,FromDateParams,ToDateParams,InputWorkorderParams,FloorParams,WoTypeBySap2Params,LcmEquipmentParams,
  OperationParams,ShiftParams,LcdOwnerParams,CustomerParams,LcmOwnerParams,SourceFabParams,ReworkedParams,IncludeBLParams,
  LcdGradeParams,FastReworkParams,PanelGradeParams,LCMDefectCodeParams,IntervalParams,WorkOrderLikeParams,InputOperatorParams} from '@/pages/ReportPortal/Static/NGBCommonParams'

  // export let SpecialCodeOptions = [
  //   {value: "ALL", label: "ALL" },
  //   {value: "PCZCQ", label: "C312(PCZCQ)" },
  //   {value: "PCZE4", label: "C311(PCZE4)" },
  //   {value: "D016", label: "D016" },
  //   {value: "PCZ63", label: "S209(PCZ63)" },
  //   {value: "PCZCH", label: "X300(PCZCH)" },
  //   {value: "PCZ64", label: "PCZ64" },
  //   {value: "PCZCV", label: "PCZCV" },
  //   {value: "PCZ54", label: "PCZ54" },
  // ];


  export const schemas = (FactoryType:string,optionsdata:any)=>{
    return [ 
  ...FromDateParams,
  ...ToDateParams,
  ...ShiftParams,
  ...IntervalParams,
  ...FloorParams, 
  { 
    label: 'common.commonpages.DisplayModel',
    component: 'RadioGroup',
    field: 'ShowMode',
    componentProps: {
      options: [
        {
          label: '站點',
          value: 'work_ctr'
        },
        {
          label: '小機種',
          value: "small_prod"
        },
        {
          label: '人員別',
          value: "operators"
        },
        
      ],
    },
    defaultValue: 'work_ctr'

  },
  ...WorkOrderLikeParams,
  ...InputOperatorParams,
  ...new LcmBySizeByModuleProdParams(optionsdata,FactoryType).getSize(), 
  ...PanelGradeParams,
  ...new LcmOwnerParams(optionsdata,FactoryType)._obj,
  ...WoTypeBySap2Params,   
    ...new LcmEquipmentParams(optionsdata,FactoryType)._obj,
    ...new LCMDefectCodeParams(optionsdata,FactoryType)._obj,
    ...new LcdGradeParams(optionsdata,FactoryType)._obj,
  
 ];
}



