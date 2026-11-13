import type { FormSchema } from '@/components/core/schema-form/';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil';
import {LcmBySizeByModuleProdParams,FromDateParams,ToDateParams,FloorParams,
  OperationParams,ShiftParams,BLBySizeProdParams,CustomerParams} from '@/pages/ReportPortal/Static/SHCommonParams'


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
        {value:'operators',label:'人員別'},
        {value:'equip_nbr',label:'機台別'},
      ]
    },
    defaultValue: 'operators'
  },
  ...new BLBySizeProdParams(optionsdata,FactoryType).getSize(),
  ...new OperationParams(optionsdata,FactoryType)._obj,
];
}
