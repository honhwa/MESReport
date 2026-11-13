import type { FormSchema } from '@/components/core/schema-form/';
import {
  ShiftParams2, LcmProductParams, LCMOwnerParams, StartDateParams, EndDateParams, FloorParams2,
} from '@/pages/ReportPortal/Static/CommonParams'
import { optionsdata } from '@/pages/ReportPortal/FS/PanelWipStayChart/LCM1/form-schema';
import {Equipment, Grade} from '@/pages/ReportPortal/Static/common';

export const schemas=(Fab):FormSchema[] =>{
  return  [

    StartDateParams,
    EndDateParams,
    ShiftParams2,
    FloorParams2,
    {
      field: 'EventCode',
      component: 'Input',
      label: 'EventCode',
      componentSlots: () => {
        return {
          prefix: () => '顯示Top',
          // suffix: () => 'Event Code',
        };
      },
      defaultValue:10
    },{
      field: 'ShowType',
      component: 'RadioGroup',
      componentProps: {
        options: [
          {
            label: 'Times ',
            value: 'count'
          },
          {
            label: 'Sub_Time ',
            value: "subtime"
          },
        ],
      },
      defaultValue: 'count'
    },
    ...new Equipment(optionsdata, Fab, 0).obj,
  ];
}

