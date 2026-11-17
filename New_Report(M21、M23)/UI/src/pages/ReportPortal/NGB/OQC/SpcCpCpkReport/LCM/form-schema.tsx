import type { FormSchema } from '@/components/core/schema-form/';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil';
// import { formatToDateHour } from '@/utils/dateUtil';

  import {FromDateParams,ToDateParams,
    EqptIdParams,ProdParams,GroupParams} from '@/pages/ReportPortal/Static/NGBCommonParams'
  
 export const schemas = (FactoryType:string,optionsdata:any)=>{
  return [ 
  {
    label: 'Query DateTime',
    component: 'RadioGroup',
    field: 'DateTimeType',
    componentProps: {
      options: [
        {
          label: 'By Month',
          value: 'Month'
        },
        {
          label: 'By Week',
          value: "Week"
        },
        {
          label: 'By 2Week',
          value: "2Week"
        },
        {
          label: 'By Day',
          value: "Day"
        },
      ],
    },
    defaultValue: 'Month'
  },
  ...FromDateParams,
  ...ToDateParams,
  ...new EqptIdParams(optionsdata,FactoryType)._obj,
  ...new ProdParams(optionsdata,FactoryType)._obj,
  ...new GroupParams(optionsdata,FactoryType)._obj,
];
}
