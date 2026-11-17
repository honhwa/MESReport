import type { FormSchema } from '@/components/core/schema-form/';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil'
import {LcmBySizeByModuleProdParams,FromDateParams,ToDateParams,InputWorkorderParams,FloorParams,
  OperationParams,ShiftParams,LcmBySizeProdParams,CustomerParams,LcdOwnerParams,LcdGradeParams, AbnormalCodeParams} from '@/pages/ReportPortal/Static/NGBCommonParams'




  export const schemas = (FactoryType:string,optionsdata:any)=>{
    return [
  ...ToDateParams,
  ...AbnormalCodeParams, 
  {
    label: '顯示BANK NG (comments是CR開頭的):',
    labelWidth: 180,
    component: 'RadioGroup',
    field: 'BankNG',
    componentProps: {
      options: [
        {
          label: 'ALL',
          value: 'ALL'
        },
        {
          label: 'No',
          value: 'NG'
        },
        {
          label: 'OK',
          value: "OK"
        }
      ],
    },
    defaultValue: 'ALL'
  },   
  ...FloorParams, 
  {
    label: 'common.commonpages.LCDProduct',
    component: 'InputTextArea',
    field: 'Lcd_Prod_nbr',

  },  
  ...new LcdOwnerParams(optionsdata,FactoryType)._obj,
  ...new LcdGradeParams(optionsdata,FactoryType)._obj,
 ];
}
