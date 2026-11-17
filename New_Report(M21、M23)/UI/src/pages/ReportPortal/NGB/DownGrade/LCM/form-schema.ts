import type { FormSchema } from '@/components/core/schema-form/';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil';

import {LcmBySizeByModuleProdParams,FromDateParams,ToDateParams,InputWorkorderParams,FloorParams,LcdOwnerParams,LcdGradeParams,
  OperationParams,ShiftParams,LcmBySizeProdParams,CustomerParams,SourceFabParams,LcmOwnerParams,WoTypeBySapParams,WorkOrderLikeParams} from '@/pages/ReportPortal/Static/NGBCommonParams'

  export let Operation = [
    {value: "1400", label: "1400" },
    {value: "1600", label: "1600 C-Ken" },
    {value: "1630", label: "1630 C2-Ken" },
    {value: "1640", label: "1640 C-Ken for TaiChi" },
    {value: "1700", label: "1700 D-Ken" }, 
    {value: "1900", label: "1900 OQC" },  
    ]



  export const schemas = (FactoryType:string,optionsdata:any)=>{
  return [  
  ...FromDateParams,
  ...ToDateParams,
  ...ShiftParams,
  ...FloorParams,
  ...new SourceFabParams(optionsdata,FactoryType)._obj,   
  {
    label: 'Reworked',
    component: 'RadioGroup',
    field: 'IsReworked',
    componentProps: {
      options: [
        {
          label: 'ALL',
          value: 'ALL'
        },
        {
          label: 'YES',
          value: "YES"
        },
        {
          label: 'NO',
          value: "NO"
        },
      ],
    },
    defaultValue: 'ALL'
  }, 
  {
    label: 'common.commonpages.ShowMode',
    component: 'RadioGroup',
    field: 'ShowMode',
    componentProps: {
      options: [
        {
          label: '人員別',
          value: 'Operator'
        },
        {
          label: '機種別',
          value: "Prod_nbr"
        },        
        {
          label: 'Defect別',
          value: "Defect_Code"
        },
      ],
    },
    defaultValue: 'Prod_nbr'
  },
  ...WorkOrderLikeParams,
  ...new LcmBySizeByModuleProdParams(optionsdata,FactoryType).getSize(),
  ...new LcmOwnerParams(optionsdata,FactoryType)._obj,
  ...new WoTypeBySapParams(optionsdata,FactoryType)._obj,
  ...new LcdOwnerParams(optionsdata,FactoryType)._obj,
  {
    label: 'common.commonpages.Operation',
    component: 'Select',
    field: 'Operation',
    componentProps: () => {
      return {
        options: Operation,      
      };
    },
    defaultValue:'1600'
  },
  ...new LcdGradeParams(optionsdata,FactoryType)._obj,
];
}
