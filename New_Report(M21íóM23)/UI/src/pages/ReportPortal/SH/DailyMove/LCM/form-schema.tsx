import type { FormSchema } from '@/components/core/schema-form/';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil';
import {LcmBySizeByModuleProdParams,FromDateParams,ToDateParams,InputWorkorderParams,FloorParams,
  OperationParams,ShiftParams,LcmBySizeProdParams,CustomerParams,LcmOwnerParams,} from '@/pages/ReportPortal/Static/SHCommonParams'
 


 export const schemas = (FactoryType:string,optionsdata:any)=>{
 return[  
   ...FromDateParams,
  ...ToDateParams,
  ...ShiftParams,
  ...FloorParams,  
  {
    label: 'RW Operation',
    component: 'RadioGroup',
    field: 'RWOperation',

    componentProps: {
      options: [
        {
          label: 'YES',
          value: 'YES'
        },
        {
          label: 'NO',
          value: 'NO'
        }
      ]
    },
    defaultValue: 'NO'

  }, 
  ...InputWorkorderParams,
   ...new LcmBySizeProdParams(optionsdata,FactoryType).getSize(),
    ...new OperationParams(optionsdata,FactoryType)._obj,
   ...new LcmOwnerParams(optionsdata,FactoryType)._obj,
  ...new CustomerParams(optionsdata,FactoryType)._obj,
  ];
 }


// export const schemas = (FactoryType:string,optionsdata:any)=>{
//   return [  
//   ...FromDateParams,
//   ...ToDateParams,
//   ...ShiftParams,
//   ...FloorParams,   
//   {
//     label: 'Reworked',
//     component: 'RadioGroup',
//     field: 'IsReworked',
//     componentProps: {
//       options: [
//         {
//           label: 'ALL',
//           value: 'ALL'
//         },
//         {
//           label: 'YES',
//           value: "YES"
//         },
//         {
//           label: 'NO',
//           value: "NO"
//         },
//       ],
//     },
//     defaultValue: 'ALL'
//   }, 
//   {
//     label: 'common.commonpages.ShowMode',
//     component: 'RadioGroup',
//     field: 'ShowMode',
//     componentProps: {
//       options: [
//         {
//           label: '人員別',
//           value: 'Operator'
//         },
//         {
//           label: '機種別',
//           value: "Prod_nbr"
//         },        
//         {
//           label: 'Defect別',
//           value: "Defect_Code"
//         },
//       ],
//     },
//     defaultValue: 'Prod_nbr'
//   },
 
//   ...LcmBySizeByModuleProdParams,
//   ...LcmOwnerParams,
  

// ];
// }