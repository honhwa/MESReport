import type { FormSchema } from '@/components/core/schema-form/';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil';
// import { formatToDateHour } from '@/utils/dateUtil';

  import {GroupParams} from '@/pages/ReportPortal/Static/SHCommonParams'

export const schemas = (FactoryType:string,optionsdata:any)=>{
  return [  
  { label: 'common.commonpages.Date',
    component: 'DatePicker',
    field: 'MonthDate',

    componentProps: {
      format: 'YYYY-MM',
      showTime:true,
      minuteStep:30,
      valueFormat:'YYYY-MM'
    },
    defaultValue:formatToDate(),
    required: true
  },
  ...new GroupParams(optionsdata,FactoryType)._obj,
];
}
