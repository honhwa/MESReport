import type { FormSchema } from '@/components/core/schema-form/';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil'
import {LcmBySizeByModuleProdParams,FromDateParams,ToDateParams,InputWorkorderParams,FloorParams,
  OperationParams,ShiftParams,LcmBySizeProdParams,CustomerParams,LcmOwnerParams,LcdOwnerParams,LcdGradeParams,
  AbnormalCodeParams,LCDBySizeProdParams} from '@/pages/ReportPortal/Static/NGBCommonParams'

  
  export const schemas = (FactoryType:string,optionsdata:any)=>{
    return [
      {
        label: 'common.commonpages.DisplayModel',
        component: 'RadioGroup',
        field: 'QueryType',
    
        componentProps: ({formModel,model,formInstance}) => {
          return {
            options: [
              {
                label: '混貨監控',
                value: 'MIX'
              },
              {
                label: '實際下線',
                value: "ACT"
              },
             
            ],
            onChange: async (e: any) => {
                 if(formModel.QueryType=="ACT"){
                  // formInstance.removeSchemaByFiled(`Floor`);
                  // formInstance.removeSchemaByFiled(`Shift`);
                  formInstance.appendSchemaByField(...FloorParams,'ToDate')
                  formInstance.appendSchemaByField(...ShiftParams,'Floor')
                  formInstance.appendSchemaByField(...AbnormalCodeParams,'Shift')
                  formInstance.appendSchemaByField([...new LCDBySizeProdParams(optionsdata,FactoryType).getSize()],'AbnormalCode')
                  formInstance.appendSchemaByField([...new LcmOwnerParams(optionsdata,FactoryType)._obj],'Prod_nbr')
                 }
                 if(formModel.QueryType=="MIX"){
                  formInstance.removeSchemaByFiled(`Floor`);
                  formInstance.removeSchemaByFiled(`Shift`);
                  formInstance.removeSchemaByFiled(`AbnormalCode`);
                  formInstance.removeSchemaByFiled(['#LCDDivider','LCDSize','LcdProduct']);
                  formInstance.removeSchemaByFiled('LcmOwner');
                 }
            },    
          }
        },
        defaultValue: 'MIX'
      },
      ...FromDateParams,
      ...ToDateParams,     
      // ...FloorParams,
      // ...ShiftParams,
      // ...AbnormalCodeParams,      
      //...new LCDBySizeProdParams(optionsdata,FactoryType).getSize(),
      ...new LcdGradeParams(optionsdata,FactoryType)._obj,
      ...new LcmBySizeProdParams(optionsdata,FactoryType).getSize(),
     //...new LcmOwnerParams(optionsdata,FactoryType)._obj,
  
 ];
}
