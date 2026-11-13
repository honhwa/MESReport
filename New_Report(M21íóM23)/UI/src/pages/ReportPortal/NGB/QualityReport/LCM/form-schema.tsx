import type { FormSchema } from '@/components/core/schema-form/';
import { reactive,computed } from 'vue';
import { formatToDate } from '@/utils/dateUtil';
import { useI18n } from '@/hooks/useI18n';

import {LcmBySizeByModuleProdParams,FromDateParams,ToDateParams,InputWorkorderParams,FloorParams,WoTypeBySapParams,LcmEquipmentParams,
  OperationParams,ShiftParams,LcdOwnerParams,CustomerParams,LcmOwnerParams,SourceFabParams,ReworkedParams,IncludeBLParams,
  LcdGradeParams,FastReworkParams,LCMDefectCodeParams,InputOperatorParams} from '@/pages/ReportPortal/Static/NGBCommonParams'

  const { t } = useI18n();
  export let SpecialCodeOptions = [
    {value: "ALL", label: "ALL" },
    {value: "PCZCQ", label: "C312(PCZCQ)" },
    {value: "PCZE4", label: "C311(PCZE4)" },
    {value: "D016", label: "D016" },
    {value: "PCZ63", label: "S209(PCZ63)" },
    {value: "PCZCH", label: "X300(PCZCH)" },
    {value: "PCZ64", label: "PCZ64" },
    {value: "PCZCV", label: "PCZCV" },
    {value: "PCZ54", label: "PCZ54" },
  ];


  export const schemas = (FactoryType:string,optionsdata:any)=>{
    return [
  ...FromDateParams,
  ...ToDateParams,
  ...ShiftParams,
  ...FloorParams, 
  ...new LcmBySizeByModuleProdParams(optionsdata,FactoryType).getSize(), 
  ...new SourceFabParams(optionsdata,FactoryType)._obj, 
  ReworkedParams,
  FastReworkParams,
  { 
    label: 'common.commonpages.DisplayModel',
    component: 'RadioGroup',
    field: 'ShowMode',
    componentProps: {
      options: [
        {
          label:computed(()=>t('common.commonpages.Operation')),//label: '站點',
          value: 'work_ctr'
        },
        {
          label: '機台別',
          value: "equip_nbr"
        },
        {
          label: '大機種',
          value: "big_prod"
        },
        //{
         // label: '1300to1600',
       //   value: "1300to1600"
       // },
      ],
    },
    defaultValue: 'work_ctr'

  },

  ...IncludeBLParams,
  ...InputWorkorderParams,
  ...InputOperatorParams,
  ...new LcmOwnerParams(optionsdata,FactoryType)._obj,
  ...new WoTypeBySapParams(optionsdata,FactoryType)._obj,
   ...new LcdOwnerParams(optionsdata,FactoryType)._obj,
    ...new OperationParams(optionsdata,FactoryType)._obj,
    ...new LcmEquipmentParams(optionsdata,FactoryType)._obj,
    ...new LCMDefectCodeParams(optionsdata,FactoryType)._obj,
  {
    label: '過濾掉的Defect Code',
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




