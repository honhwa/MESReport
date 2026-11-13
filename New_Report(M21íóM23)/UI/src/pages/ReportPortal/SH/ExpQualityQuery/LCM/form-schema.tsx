import type { FormSchema } from '@/components/core/schema-form/';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil';
import {LcmBySizeByModuleProdParams,LcmBySizeProdParams,FromDateParams,ToDateParams,InputWorkorderParams,FloorParams,WoTypeBySapParams,LcmEquipmentParams,
  OperationParams,ShiftParams,LcdOwnerParams,CustomerParams,LcmOwnerParams,SourceFabParams,ReworkedParams,IncludeBLParams,
  LcdGradeParams,FastReworkParams,LCMDefectCodeParams,IntervalParams,WorkOrderLikeParams,InputOperatorParams,LcmGradeParams,} from '@/pages/ReportPortal/Static/SHCommonParams'

  export let SpecialCodeOptions = [
    {value: "ALL", label: "ALL" },
    {value: "PC000", label: "PC000" },
    {value: "PCZ00", label: "PCZ00" },
    {value: "PCZ72", label: "PCZ72" },
    {value: "PCZE4", label: "PCZE4" },
  ];


  export const schemas = (FactoryType:string,optionsdata:any)=>{
    return [
  ...FromDateParams,
  ...ToDateParams,
  ...ShiftParams,
  ...FloorParams, 
  {
    label: 'common.commonpages.QueryType',
    component: 'RadioGroup',
    field: 'QueryType',

    componentProps: {
      options: [       
        {
          label: 'FPY',
          value: "FPY"
        },
        {
          label: 'GradeYield',
          value: 'GradeYield'
        }
      ],
    },
    defaultValue: 'FPY'

  },
  {
    label: '是否含快修',
    component: 'Select',
    field: 'FastRework',
    componentProps: {
      options: [
        
        {
          label: '快修NG視為OK',
          value: "OK"
        },
        {
          label: '快修NG視為NG',
          value: "NG"
        },
      ],
    },
    defaultValue: 'OK'
  },
  ...new SourceFabParams(optionsdata,FactoryType)._obj, 
  ...WorkOrderLikeParams,
  ...new LcmBySizeProdParams(optionsdata,FactoryType).getSize(),
  ...new LcmOwnerParams(optionsdata,FactoryType)._obj,
  ...new LcmGradeParams(optionsdata,FactoryType)._obj,
  ...new OperationParams(optionsdata,FactoryType)._obj,
  ...new LcmEquipmentParams(optionsdata,FactoryType)._obj,
  ...new LCMDefectCodeParams(optionsdata,FactoryType)._obj,
  ...new LcdGradeParams(optionsdata,FactoryType)._obj,
  {
    label: 'Defect Code',
    field: 'SpecialCode',
    //component: 'Select',
    component: 'CheckboxGroup',
    componentProps: () => {
      return {
        //mode: "multiple",
        options: SpecialCodeOptions,        
     }
   },
   defaultValue: ['ALL']
  },
];
}




