import type { FormSchema } from '@/components/core/schema-form/';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil';
import { AddCommonPageParams } from '@/store/module/commonpage';
import useRemoteSelect from '@/pages/ReportPortal/utils/SHuseRemoteSelect';
import {LcmBySizeByModuleProdParams,FromDateParams,ToDateParams,FloorParams,
  OperationParams,ShiftParams,LcmBySizeProdParams,CustomerParams,LcmOwnerParams} from '@/pages/ReportPortal/Static/SHCommonParams'
  const useCommonParams = AddCommonPageParams();
 
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
              
               if (FactoryType=="LCM1" )
               {
                useCommonParams.Fab="LCM1"
                useCommonParams.G_FAC_COM="C"          
              }
              else if (FactoryType=="LCM2" )
              {
              useCommonParams.Fab="LCM2"
               useCommonParams.G_FAC_COM="B"           
             }
             else if (FactoryType=="LCM3" )
             {
              useCommonParams.Fab="LCM3"
              useCommonParams.G_FAC_COM="E"           
            }
            else if (FactoryType=="LCM4" )
            {
             useCommonParams.G_FAC_COM="D"          
           }
        
        formModel.Operation = undefined; //  reset city value
        const { updateSchema } = formInstance;
           updateSchema(
            ...new LcmBySizeByModuleProdParams(optionsdata,FactoryType).getSize(),
            ...new LcmOwnerParams(optionsdata,FactoryType)._obj,
            ...new OperationParams(optionsdata,FactoryType)._obj,
          
           );
        
       
           },       
          }
    
        },
        defaultValue: 'LCM3',
      
      },  
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
