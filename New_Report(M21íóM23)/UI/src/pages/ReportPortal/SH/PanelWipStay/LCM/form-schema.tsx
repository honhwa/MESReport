import type { FormSchema } from '@/components/core/schema-form/'
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil'
import { AddCommonPageParams } from '@/store/module/commonpage';
// import useRemoteSelect from '@/pages/ReportPortal/utils/SHuseRemoteSelect';
import {LcmBySizeByModuleProdParams,FromDateParams,ToDateParams,InputWorkorderParams,FloorParams,ReworkedParams,
  OperationParams,ShiftParams,LcmBySizeProdParams,CustomerParams,LcdOwnerParams,LcdGradeParams,LcmOwnerParams,WorkOrderLikeParams} from '@/pages/ReportPortal/Static/SHCommonParams'

  const useCommonParams = AddCommonPageParams();

   export let Rw_wonbrOptions12 = [
    {value: "ALL", label: "ALL" },
    {value: "NO", label: "不包含以下所有工單" },
    {value: "62104", label: "不含62104的工單" },   
    ]

    export let Rw_wonbrOptions3 = [
      {value: "ALL", label: "ALL" },
      {value: "NO", label: "不包含以下所有工單" },
      {value: "6002", label: "不含6002的工單" },  
    ]

    export let Rw_wonbrOptions4 = [
      {value: "ALL", label: "ALL" },
      {value: "NO", label: "不包含以下所有工單" },
      {value: "6201", label: "不含6201的工單" },
      {value: "6206", label: "不含6206的工單" }, 
      {value: "6207", label: "不含6207的工單" },
      {value: "6208", label: "不含6208的工單" },   
    ]

    export let WotypeOptions = [
      {value: "ALL", label: "ALL" },
      {value: "1", label: "P" },
      {value: "2", label: "E" },
      {value: "3", label: "R" },
      {value: "4", label: "M" },
      {value: "5", label: "S" },
    ];



export const schemas = (FactoryType:string,optionsdata:any)=>{
  return [ 
  {
    label: 'common.commonpages.QueryType',
    component: 'RadioGroup',
    field: 'Queryby',

    componentProps: {
      options: [
        {
          label: '各站停留時間',
          value: 'last_trans_dt'
        },
        {
          label: '累計停留時間',
          value: 'start_time'
        }
      ]
    },
    defaultValue: 'last_trans_dt'

  }, 
  ...FloorParams, 
  { 
    label: '是否HOLD品',
    component: 'RadioGroup',
    field: 'Hold',
    componentProps: {
      options: [
        {
          label: 'ALL',
          value: 'ALL'
        },        
        {
          label: '是',
          value: "YES"
        },
        {
          label: '否',
          value: 'NO'
        },
        
      ],
    },
    defaultValue: 'ALL'

  },  
  { 
    label: '顯示Product ID',
    component: 'RadioGroup',
    field: 'Check_type',
    componentProps: {
      options: [
        {
          label: '否',
          value: 'NO'
        },
        {
          label: '是',
          value: "YES"
        },
        
      ],
    },
    defaultValue: 'NO'

  },
  {
    label: '是否含RW工單',
    component: 'Select',
    field: 'Rw_wonbr',
    componentProps: () => {
      if(useCommonParams.Fab=="LCM1"||useCommonParams.Fab=="LCM2"){
        return {
          options: Rw_wonbrOptions12,      
        };
      }else if(useCommonParams.Fab=="LCM3"){
        return {
          options: Rw_wonbrOptions3,      
        };
      }else if(useCommonParams.Fab=="LCM4"){
        return {
          options: Rw_wonbrOptions4,      
        };
      }
      
    },
    defaultValue:'ALL'
  },
  ReworkedParams,
  ...WorkOrderLikeParams,
  {
    label: '工單類型',
    component: 'CheckboxGroup',   
    field: 'Wotype',
    componentProps: () => {
      return {
        options: WotypeOptions,
      };
    },
    defaultValue: ['ALL']
  },
  { 
    label: '選擇工單Due Day',
    component: 'RadioGroup',
    field: 'Iswodueday',
    componentProps: {
      options: [
        {
          label: '否',
          value: 'NO'
        },
        {
          label: '是',
          value: "YES"
        },
        
      ],
    },
    defaultValue: 'NO'

  },
  ...FromDateParams,
  ...ToDateParams,
  ...new LcmBySizeByModuleProdParams(optionsdata,FactoryType).getSize(),
  ...new LcmOwnerParams(optionsdata,FactoryType)._obj,
  ...new OperationParams(optionsdata,FactoryType)._obj,
];
}
        


