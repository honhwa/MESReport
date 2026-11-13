import type { FormSchema } from '@/components/core/schema-form/';
import { ShiftParams, FloorParams1, LCMOwnerParams, } from '@/pages/ReportPortal/Static/CommonParams'
import { formatToDateHour } from '@/utils/dateUtil';
import { Size2,Size } from '@/pages/ReportPortal/Static/common';
import { reactive } from 'vue';

export const schemas=(Fab:string):FormSchema[]=>{
  let optionsdata =reactive({}) 
  
  return [
    {
      label: 'common.commonpages.Date',
      component: 'DatePicker',
      field: 'CompleteFromDate',
  
      componentProps: {
        format: 'YYYY-MM-DD HH:mm',
        showTime: true,
        minuteStep: 30,
        valueFormat: 'YYYY-MM-DD HH:mm'
      },
      defaultValue: formatToDateHour()
    },
    {
      label: 'common.commonpages.To',
      component: 'DatePicker',
      field: 'CompleteToDate',
  
      componentProps: {
        format: 'YYYY-MM-DD HH:mm',
        showTime: true,
        minuteStep: 30,
        valueFormat: 'YYYY-MM-DD HH:mm'
      },
      defaultValue: formatToDateHour(),
    },
    ShiftParams,
    FloorParams1,
    {
      field: 'operato',
      component: 'Input',
      label: 'Operator'
    },
    {
      field: 'LCMDivider',
      component: 'Divider',
      label: 'BLProduct'
    },
    ...new Size(optionsdata, Fab).obj,
    LCMOwnerParams,
  
  ];
}

