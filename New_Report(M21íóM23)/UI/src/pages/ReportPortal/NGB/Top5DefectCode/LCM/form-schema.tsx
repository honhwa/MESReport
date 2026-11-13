import type { FormSchema } from '@/components/core/schema-form/';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil';
import {LcmBySizeByModuleProdParams,FromDateParams,ToDateParams,InputWorkorderParams,FloorParams,WoTypeBySapParams,LcmEquipmentParams,
  OperationParams,ShiftParams,LcdOwnerParams,CustomerParams,LcmOwnerParams,SourceFabParams,ReworkedParams,IncludeBLParams,
  LcdGradeParams,FastReworkParams,LCMDefectCodeParams,InputProdNbrLikeParams,ReworkedParams2} from '@/pages/ReportPortal/Static/NGBCommonParams'

  export let SpecialOperOptions = [
    {value: "1", label: "不含1630&1410&1430&1590站點" },
    {value: "2", label: "含1630站點" },
    {value: "3", label: "含1410站點" },
    {value: "4", label: "含1430站點" },
    {value: "5", label: "含1590站點" },
  ];


  export const schemas = (FactoryType:string,optionsdata:any)=>{
    return [
  ...FromDateParams,
  ...ToDateParams,
  ...ShiftParams,
  ...FloorParams, 
  ...new SourceFabParams(optionsdata,FactoryType)._obj, 
  ReworkedParams2,
  FastReworkParams,
  { 
    label: 'common.commonpages.DisplayModel',
    component: 'RadioGroup',
    field: 'ProdType',
    componentProps: {
      options: [
        {
          label: '大機種',
          value: '1'
        },
        {
          label: '小機種',
          value: "2"
        },
        {
          label: '太極機種',
          value: "3"
        },
        
      ],
    },
    defaultValue: '1'

  },
  ...new LcmBySizeByModuleProdParams(optionsdata,FactoryType).getSize(),
  ...new LcmOwnerParams(optionsdata,FactoryType)._obj,
  ...new WoTypeBySapParams(optionsdata,FactoryType)._obj,
  ...new LCMDefectCodeParams(optionsdata,FactoryType)._obj,
  //...InputProdNbrLikeParams,   
  ...new LcdGradeParams(optionsdata,FactoryType)._obj,
  {
    label: '是否含1630&1410站點&1430站點',
    field: 'SpecialOper',
    component: 'Select',
    componentProps: () => {
      return {
        options: SpecialOperOptions,        
     }
   },
   defaultValue:["1"],
  },
];
}




