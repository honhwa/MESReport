import type { FormSchema } from '@/components/core/schema-form/'
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil'
import { AddCommonPageParams } from '@/store/module/commonpage';
// import useRemoteSelect from '@/pages/ReportPortal/utils/SHuseRemoteSelect';
import {LcmBySizeByModuleProdParams,FromDateParams,ToDateParams,FloorParams,ReworkedParams,
  OperationParams,LcmOwnerParams,WorkOrderLikeParams} from '@/pages/ReportPortal/Static/SHCommonParams'

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
        label: 'NC2',
        value: "LCM1"
      },        
      {
        label: 'NB2',
        value: "LCM2"
      },    
          ],    
          onChange: async (e: any) => {
            
            
             FactoryType=formModel.FAC;
            
          if (FactoryType=="LCM1" )
           {
            
            useCommonParams.G_FAC_COM='C'          
          }
          else if (FactoryType=="LCM2" )
          {
          useCommonParams.Fab="LCM2"
           useCommonParams.G_FAC_COM='B'           
         }
         else if (FactoryType=="LCM3" )
         {
          useCommonParams.Fab="LCM3"
          useCommonParams.G_FAC_COM='E'           
        }
        else if (FactoryType=="LCM4" )
        {
         useCommonParams.G_FAC_COM='D'          
       }
      /// console.log("pre optiondata",useCommonParams.G_FAC_COM);
      // console.log("pre optiondata",FactoryType);
      //  formModel.formInstance.updateSchema(); 
      formModel.Operation = undefined; //  reset city value
      // formModel.choose_prodtype = undefined;
      // formModel.Size = undefined;
      // formModel.Product = undefined;
      // formModel.Module = undefined;
      // formModel.Option_Prod_nbr = undefined;
      // formModel.optionmenu = undefined;
      // formModel.Prod_nbr = undefined;
      const { updateSchema } = formInstance;
         updateSchema(
         ...new OperationParams(optionsdata,FactoryType)._obj,
         ...new LcmBySizeByModuleProdParams(optionsdata,FactoryType).getSize(),
         //...new LcmBySizeByModuleProdParams(optionsdata,FactoryType).obj,
        
         );
                  
      // console.log("sssssss",formModel)
       // await mounted(optionsdata, FactoryType);
      // await mountedModuleData(optionsdata, useCommonParams.Fab);

          
            //new LcmBySizeByModuleProdParams(optionsdata,FactoryType),
            //new LcmBySizeByModuleProdParams(optionsdata,FactoryType).getSize,
            //new LcmBySizeByModuleProdParams(optionsdata,FactoryType).obj,
           
      
     
         },       
        }
  
      },
      defaultValue: 'LCM1',
    
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
  //...new LcmBySizeByModuleProdParams(optionsdata,FactoryType).getSize(),
  ...new LcmOwnerParams(optionsdata,FactoryType)._obj,
  ...new OperationParams(optionsdata,FactoryType)._obj,
  

];
}
   

