import type { FormSchema } from '@/components/core/schema-form/';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil'
import {FromDateParams,ToDateParams,InputWorkorderParams,FloorParams} from '@/pages/ReportPortal/Static/TNCommonParams'

import {LCD_Owner, LCD_Grade} from '@/pages/ReportPortal/Static/TNCommonParams'



export const schemas = (FactoryType:string)=>{
  let optionsdata = reactive({});
  return [
    ...ToDateParams,
    {
      label: 'common.commonpages.DisplayAbnormalCode',
      labelWidth: 180,
      component: 'RadioGroup',
      field: 'show_abn',
      componentProps: {
        options: [
          {
            label: 'No',
            value: 'N'
          },
          {
            label: 'Yes',
            value: "Y"
          }
        ],
      },
      defaultValue: 'N'
    }, 
    {
      label: 'common.commonpages.DisplayCSTType',
      labelWidth: 180,
      component: 'RadioGroup',
      field: 'show_cst',
      componentProps: {
        options: [
          {
            label: 'No',
            value: 'N'
          },
          {
            label: 'Yes',
            value: "Y"
          }
        ],
      },
      defaultValue: 'N'
    },   
    {
      label: 'common.commonpages.LCDProduct',
      component: 'InputTextArea',
      field: 'Lcd_Prod_nbr',

    },  
    ...new LCD_Owner(optionsdata,FactoryType,'LcdOwner').obj,
    ...new LCD_Grade(optionsdata,FactoryType,'LcdGrade').obj,
    
  ];
};
