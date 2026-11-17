import type { FormSchema } from '@/components/core/schema-form/';
import { reactive,computed } from 'vue';
import { formatToDate } from '@/utils/dateUtil';
import { useI18n } from '@/hooks/useI18n';

import {LcmBySizeByModuleProdParams,FromDateParams,ToDateParams,InputWorkorderParams,FloorParams,WoTypeBySapParams,LcmEquipmentParams,EquipmentParams,
  OperationParams,ShiftParams,CustomerParams,SourceFabParams,ReworkedParams,IncludeBLParams,
  LcdGradeParams,FastReworkParams,LCMDefectCodeParams,InputOperatorParams} from '@/pages/ReportPortal/Static/SHCommonParams'

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

  export let WoTypeOptions = [
    { value: "ALL", label: "ALL" },
    { value: "N", label: "N" },
    { value: "W", label: "W" },
    { value: "E", label: "E" },
    { value: "X", label: "X" },
    { value: "S", label: "S" },
    { value: "D", label: "D" },
    { value: "P", label: "P" },
    { value: "Q", label: "Q" },
    { value: "R", label: "R" },
    { value: "C", label: "C" },
    { value: "M", label: "M" },
    { value: "B", label: "B" },
    { value: "Y", label: "Y" },
    ]

  let ifSelectColall=false// 是否全選標記
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
        //  label: '1300to1600',
        //  value: "1300to1600"
        //},
      ],
    },
    defaultValue: 'work_ctr'

  },

  ...IncludeBLParams,
  ...InputWorkorderParams,
  ...InputOperatorParams,

  //...new WoTypeBySapParams(optionsdata,FactoryType)._obj,
  {
    label: '工單類型',
    field: 'WoType',
    component: 'Select',
    componentProps: ({ formModel, field }) => {
      return {
        mode: "multiple",
        options: WoTypeOptions,
        maxTagCount:10,
        onChange: async (e: any) => {  
                         
          if (e.indexOf('ALL')!= -1&& ifSelectColall!=true) {
           
            let SelectData: string[] = []    
            WoTypeOptions.forEach(item => {
              SelectData.push(item.value)
            });
            formModel[field] = SelectData
            ifSelectColall=true
          }else if (e.indexOf('ALL')!= -1&& ifSelectColall===true) {
            
            e.map((val,i)=>{
              if(val==="ALL"){
               e.splice(i,1);
              }
           })
           formModel[field]=e           
           ifSelectColall=false
          
          }else {
            //console.log(66) 
            if(e.indexOf('ALL')=== -1 && ifSelectColall===true)
            {
              formModel[field]=[]
              ifSelectColall=false
              
              
            }else{
              //console.log("999")
              formModel[field]=e
            }
            
          }
        }
     }
   }
  },
    ...new OperationParams(optionsdata,FactoryType)._obj,
    //...new LcmEquipmentParams(optionsdata,FactoryType)._obj,
    ...new EquipmentParams(optionsdata,FactoryType)._obj,
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




