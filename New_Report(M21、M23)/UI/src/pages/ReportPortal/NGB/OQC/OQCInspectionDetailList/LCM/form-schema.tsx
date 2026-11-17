import type { FormSchema } from '@/components/core/schema-form/';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil';
// import { formatToDateHour } from '@/utils/dateUtil';

  import {LcmBySizeByModuleProdParams,CompleteFromDateParams,CompleteToDateParams,InputWorkorderParams,FloorParams,LcdOwnerParams,LcdGradeParams,
    OperationParams,ShiftParams,LcmBySizeProdParams,CustomerParams,SourceFabParams,LcmOwnerParams,WoTypeBySapParams} from '@/pages/ReportPortal/Static/NGBCommonParams'
  
  export let Operation = [
    {value: "ALL", label: "ALL" },
    {value: "1700", label: "1700 D-Ken" }, 
    {value: "1750", label: "1750-間接檢驗" },
    {value: "1760", label: "1760-抽檢" },
    {value: "1800", label: "1800-PACKING" },
    {value: "1830", label: "1830-OQC" },
    {value: "1901", label: "1901-OQC" },
    {value: "1903", label: "1903-OBA" },
    {value: "1906", label: "1906-Sorting再檢" } ,
    {value: "1910", label: "1910-WARE HOUSE" } ,
    {value: "1930", label: "1930-OQC" } ,
    {value: "6000", label: "6000-OQC" }   
    ]
    export let Judgement = [
      {value: "ALL", label: "ALL" },
      {value: "OK", label: "OK" }, 
      {value: "DownGrade", label: "Down Grade" },
      {value: "UpGrade", label: "Up Grade" },
      {value: "NG", label: "NG" }   
      ]


export const schemas = (FactoryType:string,optionsdata:any)=>{
  return [
  {    
    component: 'CheckboxGroup',   
    field: 'UseDateFlag',
    componentProps: {
      options: [
        {
          label: 'By日期查詢',
          value: 'TRUE'
        },        
      ],
    },
    defaultValue: 'TRUE'
  },
  ...CompleteFromDateParams,
  ...CompleteToDateParams,
  ...ShiftParams,
  ...FloorParams,  
  ...new LcmBySizeByModuleProdParams(optionsdata,FactoryType).getSize(),
  ...new LcmOwnerParams(optionsdata,FactoryType)._obj,
  {
    label: 'Operation',
    component: 'Select',
    field: 'Operation',
    componentProps: () => {
      return {
        mode: 'multiple',
        options: Operation,      
      };
    },
    //defaultValue:'ALL'
  },
  {
    label: 'Judgement',
    component: 'Select',
    field: 'Judgement',
    componentProps: () => {
      return {
        mode: 'multiple',
        options: Judgement,      
      };
    },
    //defaultValue:'ALL'
  }, 
  {
    label: 'S/N',
    component: 'InputTextArea',
    field: 'SN',
  },
  {
    label: '工單號',
    component: 'InputTextArea',
    field: 'Wo_nbr',
  },
  {
    label: '檢查項目',
    component: 'RadioGroup',
    field: 'QueryType',
    componentProps: {
      options: [
        {
          label: 'OQC檢驗',
          value: 'OQC檢驗'
        },
        {
          label: 'VQE檢驗',
          value: "VQE檢驗"
        },
        {
          label: '間接檢驗',
          value: "間接檢驗"
        },
      ],
    },
    defaultValue: 'OQC檢驗'
  }
];
}
