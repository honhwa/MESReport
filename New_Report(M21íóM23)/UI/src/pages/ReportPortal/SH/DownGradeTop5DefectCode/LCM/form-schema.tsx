import type { FormSchema } from '@/components/core/schema-form/';
import { reactive,computed } from 'vue';
import { formatToDate } from '@/utils/dateUtil';
import { useI18n } from '@/hooks/useI18n';

import {LcmBySizeByModuleProdParams,FromDateParams,ToDateParams,InputWorkorderParams,FloorParams,WoTypeBySapParams,LcmEquipmentParams,
  OperationParams,ShiftParams,LcdOwnerParams,CustomerParams,LcmOwnerParams,SourceFabParams,ReworkedParams,IncludeBLParams,
  LcdGradeParams,ReworkedParams2,FastReworkParams,LCMDefectCodeParams,InputOperatorParams,LcmGradeParams} from '@/pages/ReportPortal/Static/SHCommonParams'

  const { t } = useI18n();
  


  export const schemas = (FactoryType:string,optionsdata:any)=>{
    return [
  ...FromDateParams,
  ...ToDateParams,
  ...ShiftParams,
  ...FloorParams, 
  ...new SourceFabParams(optionsdata,FactoryType)._obj, 
  { 
    label: 'common.commonpages.DisplayModel',
    component: 'RadioGroup',
    field: 'ProdType',
    componentProps: {
      options: [
        {
          label: '大機種',
          value: "1"
        },
        {
          label: '小機種',
          value: "2"
        },
      ],
    },
    defaultValue: '1'

  },
  ReworkedParams2,
  ...new LcmBySizeByModuleProdParams(optionsdata,FactoryType).getSize(),   
  ...new LcmOwnerParams(optionsdata,FactoryType)._obj,
  ...new WoTypeBySapParams(optionsdata,FactoryType)._obj,
  ...new LcmGradeParams(optionsdata,FactoryType)._obj,
  ...new LcdGradeParams(optionsdata,FactoryType)._obj,
  ...new LCMDefectCodeParams(optionsdata,FactoryType)._obj,
  
    {
      label: '',
      component: 'CheckboxGroup',
      field: 'SpecialOper',
      componentProps: {
       // mode: 'multiple',
        options: [
          {
            label: '包含1590',
            value: '1590'
          },
          {
            label: '包含1400',
            value: "1400"
          },
          
          
        ],
      },
      
    },
  
];
}




