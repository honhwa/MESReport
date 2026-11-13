import type { FormSchema } from '@/components/core/schema-form/';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil';
import {LcmBySizeByModuleProdParams,FromDateParams,ToDateParams,FloorParams,
  OperationParams,ShiftParams,LcmBySizeProdParams,CustomerParams,LcmOwnerParams} from '@/pages/ReportPortal/Static/NGBCommonParams'


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
        {value:'operators_small',label:'人員別(小機種)'},
        {value:'equip_nbr_small',label:'機台別(小機種)'}
      ]
    },
    defaultValue: 'operators'
  },
  ...new LcmBySizeByModuleProdParams(optionsdata,FactoryType).getSize(),
  ...new LcmOwnerParams(optionsdata,FactoryType)._obj,
  ...new OperationParams(optionsdata,FactoryType)._obj,
];
}
