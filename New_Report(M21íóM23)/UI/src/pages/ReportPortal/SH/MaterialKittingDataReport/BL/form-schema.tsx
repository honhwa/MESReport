import type { FormSchema } from '@/components/core/schema-form/';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil';
import {LcmBySizeByModuleProdParams,FromDateParams,ToDateParams,InputWorkorderParams,FloorParams,
  OperationParams,ShiftParams,LcmBySizeProdParams,CustomerParams,LcmEquipmentParams,BLBySizeProdParams,} from '@/pages/ReportPortal/Static/SHCommonParams'


  export let MaterialTypeOptions = [
    {value: "B-CNV", label: "B-CNV" },
    { value: "B-Diff-B", label: "B-Diff-B" },
    { value: "B-Diff-P", label: "B-Diff-P" },
    { value: "B-Diff-T", label: "B-Diff-T" },
    { value: "B-Diff-F", label: "B-Diff-F" },
    { value: "B-Front ", label: "B-Front" },
    { value: "B-HousingA", label: "B-HousingA" },
    { value: "INV", label: "B-INV" },
    { value: "B-Lamp", label: "B-Lamp" },
    { value: "B-LENS", label: "B-LENS" },
    { value: "B-LENS-B", label: "B-LENS-B" },
    { value: "B-LENS-T", label: "B-LENS-T" },
    { value: "B-LENS-DB", label: "B-LENS-DB" },
    { value: "B-LENS-DT", label: "B-LENS-DT" },
    { value: "B-LENS-LB", label: "B-LENS-LB" },
    { value: "B-LENS-LT", label: "B-LENS-LT" },
    { value: "B-LGP", label: "B-LGP" },
    { value: "B-Rear ", label: "B-Rear" },
    { value: "B-Ref ", label: "B-Ref" },
    { value: "LBd", label: "LBd" }, 
    { value: "Light-BarA ", label: "Light-BarA" },
    { value: "Light-BarB", label: "Light-BarB" },
    { value: "Light-BarC", label: "Light-BarC" },
    { value: "Light-BarD", label: "Light-BarD" },
    { value: "Light-BarE", label: "Light-BarE" },
    ]


export const schemas = (FactoryType:string,optionsdata:any)=>{
  return [ 
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
  ...new BLBySizeProdParams(optionsdata,FactoryType).getSize(),
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
    label: 'BL/ID',
    component: 'InputTextArea',
    field: 'PanelId',
    componentProps: () => {
      return{
        placeholder:"最大限制筆數:1000",
      }
    }
  },

  {
    label: '料號',
    component: 'InputTextArea',
    field: 'PCBA',
    componentProps: () => {
      return{
        placeholder:"最大限制筆數:1000",
      }
    }
  },
  {
    label: '批號',
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
