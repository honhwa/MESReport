import type { FormSchema } from '@/components/core/schema-form/';
import {
  StartDateParams, EndDateParams, ShiftParams, LcmProductParams,
  LCMOwnerParams, LCDGradeParams, ReworkedParams, PanelSourceParams
} from '@/pages/ReportPortal/Static/CommonParams'
import { optionsdata } from '@/pages/ReportPortal/FS/PanelWipStayChart/LCM1/form-schema';


export const schemas: FormSchema[] = [
  StartDateParams,
  EndDateParams,
  {
    label: 'common.commonpages.Floor',
    component: 'RadioGroup',
    field: 'Floor',
    componentProps: {
      options: [{ label: 'ALL', value: '' }, { label: '1F', value: " and floor='1'" },
      { label: '2F', value: " and floor='2'" }, { label: '3F', value: " and floor='3'" },]
    },
    defaultValue: ''
  },
 
  {label: 'common.commonpages.Reworked',
  component: 'RadioGroup',
  field: 'Reworked',
  componentProps: {
    options: [
      {
        label: 'ALL',
        value: '1==1'
      },
      {
        label: 'Yes',
        value: "and  stimes_ttl>1"
      },
      {
        label: 'No',
        value: "and stimes_ttl<=1"
      },
    
    ],
  },
  defaultValue: ''
},

  {
    field: 'LCMDivider',
    component: 'Divider',
    label: 'common.commonpages.LCMProduct'
  },
  ...LcmProductParams,
  LCMOwnerParams,
  {
    label: 'common.commonpages.ErrorCode',
    component: 'Select',
    field: 'errcode',
    componentProps: () => {
      return {
        mode: 'multiple',
        options: optionsdata.errorcode
      }
    },
    defaultValue: ['ALL']
  },
];

