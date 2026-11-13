import type { FormSchema } from '@/components/core/schema-form/';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil';
import {FromDateParams,ToDateParams,ReworkTypeOKParams,FloorParams,ShiftParams,
  PanelSource,ReworkedParams,Size,Owner,LCD_Grade,Operation} from '@/pages/ReportPortal/Static/TNCommonParams'


  export const schemas = (FactoryType:string)=>{
  let optionsdata = reactive({});
  return [
  ...FromDateParams,
  ...ToDateParams,
  ...FloorParams,
  ...ShiftParams,
  { 
    label: 'common.commonpages.DisplayModel',
    component: 'RadioGroup',
    field: 'ProdType',
    componentProps: {
      options: [
        {
          label: '大機種',
          value: '1'
        },
        {
          label: '小機種',
          value: "2"
        },
        
      ],
    },
    defaultValue: '1'

  },
  ...ReworkTypeOKParams,
  {
    label: 'common.commonpages.WorkOrder',
    component: 'InputTextArea',
    field: 'wo_num',

  },
  ...new PanelSource(optionsdata,FactoryType,'PanelSource').obj,
  ReworkedParams,
  ...new Size(optionsdata, FactoryType).obj,
  ...new Owner(optionsdata,FactoryType,'owner_code').obj,
  ...new LCD_Grade(optionsdata,FactoryType,'LcdGrade').obj,
  ...new Operation(optionsdata, FactoryType,'workGroup').obj,
];
}




