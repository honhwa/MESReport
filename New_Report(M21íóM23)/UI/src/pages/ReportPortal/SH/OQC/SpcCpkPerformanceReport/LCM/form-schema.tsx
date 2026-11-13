  import type { FormSchema } from '@/components/core/schema-form/'
  import { AddCommonPageParamsWithOut } from '@/store/module/commonpage';
  import { SHSelectType} from '@/pages/ReportPortal/Static/SHCommon-Form-schema';
  import useRemoteSelect from '@/pages/ReportPortal/utils/SHuseRemoteSelect';
  import {FromDateParams,ToDateParams,OperParams,ProductParams,ProcessParams,PerLineParams} from '@/pages/ReportPortal/Static/SHCommonParams'
  const useCommonParams = AddCommonPageParamsWithOut();

  
		
export const schemas = (FactoryType:string,optionsdata:any)=>{
  return [
  //Spc Floor
  {
   component: 'RadioGroup',
    field: 'SpcFloor',

    componentProps: {
      options: [
        {
          label: 'N'+useCommonParams.G_FAC_COM+'1',
          value: '1'
        },
        {
          label: 'N'+useCommonParams.G_FAC_COM+'2',
          value: '2'
        },
        {
          label: 'N'+useCommonParams.G_FAC_COM+'3',
          value: '3'
        },
        {
          label: 'N'+useCommonParams.G_FAC_COM,
          value: '4'
        }
        
      ],
    },
    defaultValue: '1'

  },
  //Query DateTime
  {
    component: 'RadioGroup',
    field: 'DateTimeType',
    componentProps: {
      options: [
        {
          label: 'By Month',
          value: 'Month'
        },
        {
          label: 'By Week',
          value: "Week"
        },
        {
          label: 'By 2Week',
          value: "2Week"
        },
        {
          label: 'By Day',
          value: "Day"
        },
      ],
    },
    defaultValue: 'Month'
  },
  ...FromDateParams,
  ...ToDateParams,
  {
    label: 'Cpk <=',
    component: 'InputTextArea',
    field: 'Cpk',
  },
   //Query by
   {
    label: 'Query by',
    component: 'RadioGroup',
     field: 'QueryType',
 
     componentProps: {
       options: [
         {
           label: '制程',
           value: 'Process'
         },
         {
           label: '項目',
           value: 'Item'
         },
         {
           label: '機種',
           value: 'Prod'
         },
         {
           label: '線別',
           value: 'Line'
         }
         
       ],
     },
     defaultValue: 'Process'
 
   },
   ...new OperParams(optionsdata,FactoryType)._obj,
   ...new ProductParams(optionsdata,FactoryType)._obj,
   ...new ProcessParams(optionsdata,FactoryType)._obj,
   ...new PerLineParams(optionsdata,FactoryType)._obj
  
];
}
