import type { FormSchema } from '@/components/core/schema-form/';
import { StartDateParams, EndDateParams, ShiftParams, LcmProductParams } from '@/pages/ReportPortal/Static/CommonParams'


export const schemas: FormSchema[] = [
  StartDateParams,
  EndDateParams,
  ShiftParams,
  {
    label: 'common.commonpages.ReportType',
    component: 'RadioGroup',
    field: 'ReportType',
    componentProps: {
      options: [
        {
          label: 'By Total',
          value: 'Total'
        },
        {
          label: 'By Detail',
          value: "Detail"
        },
      ],
    },
    defaultValue: 'Total'
  },
  {
    label: 'common.commonpages.Operation',
    component: 'Select',
    field: 'Operation',
    componentProps: {
      options: [
        {
          label: 'ALL',
          value: 'ALL'
        },
        {
          label: '1000-',
          value: "1000"
        },
      ],
    },
    defaultValue: 'Total'
  },
  {
    label: 'common.commonpages.Equipment',
    component: 'Select',
    field: 'Equipment',
    componentProps: {
      options: [
        {
          label: 'ALL',
          value: 'ALL'
        },
        {
          label: 'ALCD',
          value: "ALCD"
        },
      ],
    },
    defaultValue: 'Total'
  },
  ...LcmProductParams,
];

