import type { FormSchema } from '@/components/core/schema-form/';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil';
// import { formatToDateHour } from '@/utils/dateUtil';

  import {FromDateParams,ToDateParams, MeasToolParams, ProcToolParams, ModuleNameParams,LineParams,BeforeHoursParams} from '@/pages/ReportPortal/Static/SHCommonParams'

  export let QueryType = [
    {value: "Module Name", label: "Module Name" },
    {value: "Control Item", label: "Control Item" }, 
    {value: "Chart ID", label: "Chart ID" },
    {value: "Line", label: "Line" }
    ]
  export let ControlItem = [
    {value: "BA", label: "BA" },
    {value: "BS", label: "BS" }, 
    {value: "PS", label: "PS" },
    {value: "PN", label: "PN" }
    ]

    export let SearchType = [
      {value: "QueryLog", label: "QueryLog" },
      {value: "QueryHisLog", label: "QueryHisLog" }, 
      {value: "QueryRemarkLog", label: "QueryRemarkLog" }
      ]
         
export const schemas = (FactoryType:string,optionsdata:any)=>{
  return [  
  ...new MeasToolParams(optionsdata,FactoryType)._obj,
  ...new ProcToolParams(optionsdata,FactoryType)._obj, 
    {
      label: 'Query Type',
      component: 'Select',
      field: 'QueryType',
      componentProps: () => {
        return {
          options: QueryType,      
        };
      },
      defaultValue:'ALL'
    },

  ...new ModuleNameParams(optionsdata,FactoryType)._obj,

    {
      label: 'Control Item',
      component: 'Select',
      field: 'ControlItem',
      componentProps: () => {
        return {
          options: ControlItem,      
        };
      },
      defaultValue:'ALL'
    },
  ...new LineParams(optionsdata,FactoryType)._obj,
  ...FromDateParams,
  ...ToDateParams,
  ...new BeforeHoursParams(optionsdata,FactoryType)._obj,
  {
    label: '查詢項目',
      component: 'RadioGroup',
      field: 'SearchType',
      componentProps: () => {
        return {
          options: SearchType,      
        };
      },
      defaultValue:'QueryLog'
  }
];
}
