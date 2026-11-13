import type { FormSchema } from '@/components/core/schema-form/';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil';
import {LcmBySizeByModuleProdParams,FromDateParams,ToDateParams,InputWorkorderParams,FloorParams,WoTypeBySap2Params,LcmEquipmentParams,
  OperationParams,ShiftParams,LcdOwnerParams,CustomerParams,LcmOwnerParams,SourceFabParams,ReworkedParams,IncludeBLParams,
  LcdGradeParams,FastReworkParams,PanelGradeParams,LCMDefectCodeParams,IntervalParams,WorkOrderLikeParams,LCMOEEBigEquipmentParams} from '@/pages/ReportPortal/Static/SHCommonParams'

  export const schemas = (FactoryType:string,optionsdata:any)=>{
    return [ 
  ...FromDateParams,
  ...ToDateParams,
  ...ShiftParams,
  ...FloorParams, 
  {
    label: 'PASS量來源選擇',
    component: 'RadioGroup',
    field: 'Type',

    componentProps: {
      options: [
        {
          label: '機台MES過賬量',
          value: 'MES'
        },        
      ],
    },
    defaultValue: 'MES'

  },     
    ...new LCMOEEBigEquipmentParams(optionsdata,FactoryType)._obj,  
 ];
}



