import type { FormSchema } from '@/components/core/schema-form/';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil';

import {LcmBySizeByModuleProdParams,FromDateParams,ToDateParams,InputWorkorderParams,FloorParams,LcdOwnerParams,LcdGradeParams,
  OperationParams,ShiftParams,LcmBySizeProdParams,LcmEquipmentParams} from '@/pages/ReportPortal/Static/NGBCommonParams'




export const schemas = (FactoryType:string,optionsdata:any)=>{
  return [  
  ...FromDateParams,
  ...ToDateParams,
  ...ShiftParams,
  ...FloorParams,
  {
    label: 'common.commonpages.Interval',
    component: 'RadioGroup',
    field: 'Interval',
    componentProps: {
      options: [
        {
          label: 'Shift',
          value: 'shift'
        },
        {
          label: 'Day',
          value: 'day'
        },
        {
          label: 'Week',
          value: 'week'
        },
        {
          label: 'Month',
          value: 'month'
        },
        {
          label: 'Equipment',
          value: 'equipment'
        },
        {
          label: 'Equipment+Shift',
          value: 'equipShift'
        },
      ]
    },
    defaultValue: 'shift'

  },
  ...new LcmEquipmentParams(optionsdata,FactoryType)._obj,   
 
];
}


