import type { FormSchema } from '@/components/core/schema-form/';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil';
import {LcmBySizeByModuleProdParams,FromDateParams,ToDateParams,InputWorkorderParams,FloorParams,
  OperationParams,ShiftParams,LcmBySizeProdParams,CustomerParams,LcmEquipmentParams} from '@/pages/ReportPortal/Static/NGBCommonParams'


  export let MaterialTypeOptions = [
    {value: "SACF", label: "SACF" },
    { value: "SIC", label: "SIC" },
    { value: "S_TCP", label: "S_TCP" },
    { value: "OS_ACF", label: "OS_ACF" },
    { value: "PS_ACF", label: "PS_ACF" },
    { value: "X_PCB", label: "X_PCB" },
    { value: "PCBA-X(R)", label: "PCBA-X(R)" },
    { value: "PCBA-Y", label: "PCBA-Y" },
    { value: "PCBA-C", label: "PCBA-C" },
    { value: "FOG", label: "FOG" },
    { value: "C_BOARD", label: "CB" },
    { value: "BL", label: "BL" },
    ]


  export const schemas = (FactoryType:string,optionsdata:any)=>{
  return[
  {    
    component: 'CheckboxGroup',   
    field: 'CK_UseDate',
    componentProps: {
      options: [
        {
          label: 'By日期查詢',
          value: 'checked'
        },        
      ],
    },
  },
  ...FromDateParams,
  ...ToDateParams,
  ...ShiftParams,
  ...FloorParams,  
  ...new LcmBySizeProdParams(optionsdata,FactoryType).getSize(),
  ...new LcmEquipmentParams(optionsdata,FactoryType)._obj,
  {
    label: 'Material Type',
    field: 'MatType',
    component: 'Select',
    componentProps: {
      mode: "multiple",
      options: MaterialTypeOptions
    },
  }, 
  {
    label: 'Panel ID',
    component: 'InputTextArea',
    field: 'PanelId',
    componentProps: () => {
      return{
        placeholder:"最大限制筆數:1000",
      }
    }
  },
  {
    label: 'BL ID(Lot Number)',
    component: 'InputTextArea',
    field: 'BLId',
    componentProps: () => {
      return{
        placeholder:"最大限制筆數:1000",
      }
    }
    
  },
  {
    label: 'PCBA(Mat ID)',
    component: 'InputTextArea',
    field: 'PCBA',
    componentProps: () => {
      return{
        placeholder:"最大限制筆數:1000",
      }
    }
  },
  {
    label: 'CB(Lot Number)',
    component: 'InputTextArea',
    field: 'CB',
    componentProps: () => {
      return{
        placeholder:"最大限制筆數:1000",
      }
    }
  },
];
}
