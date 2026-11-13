import type { FormSchema } from '@/components/core/schema-form/'
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil'
import { AddCommonPageParams } from '@/store/module/commonpage';
// import useRemoteSelect from '@/pages/ReportPortal/utils/NGBuseRemoteSelect';
import {LcmBySizeByModuleProdParams,FromDateParams,ToDateParams,FloorParams,ReworkedParams,
  OperationParams,LcmOwnerParams,WorkOrderLikeParams} from '@/pages/ReportPortal/Static/NGBCommonParams'

  const useCommonParams = AddCommonPageParams();



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
      label: 'FAB Source',
      component: 'RadioGroup',
      field: 'FAC',
      componentProps: ({formModel,formInstance}) => {
        return {          
            options: [
      {
        label: 'ND2',
        value: "LCM3"
      },        
      {
        label: 'ND3',
        value: "LCM4"
      },    
          ],    
          onChange: async (e: any) => {
            FactoryType=formModel.FAC;
         
        if (FactoryType=="LCM3" )
          {
          useCommonParams.G_FAC_COM='E'           
          }
         else if (FactoryType=="LCM4" )
         {
         useCommonParams.G_FAC_COM='D'          
       }
      const { updateSchema } = formInstance;
         updateSchema(
         ...new OperationParams(optionsdata,FactoryType)._obj,
         ...new LcmOwnerParams(optionsdata,FactoryType)._obj,
         ...new LcmBySizeByModuleProdParams(optionsdata,FactoryType).getSize(),
        );
                  
    
         },       
        }
  
      },
      defaultValue: 'LCM3',
    
    }, 

 
    
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
        },
        {
          label: 'RW累計停留時間',
          value: 'RW_start_time'
        },
        
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
   



