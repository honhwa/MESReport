import type { FormSchema } from '@/components/core/schema-form/';
import {
  StartDateParams, EndDateParams, ShiftParams, LcmProductParams,
  LCMOwnerParams, LCDGrade1Params, ReworkedParams, PanelSourceParams
} from '@/pages/ReportPortal/Static/CommonParams'
import { optionsdata } from '@/pages/ReportPortal/FS/PanelWipStayChart/LCM1/form-schema';
import { Size } from '@/pages/ReportPortal/Static/common';


export const schemas: FormSchema[] = [
  StartDateParams,
  EndDateParams,
  ShiftParams,
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
  {
    label: 'common.commonpages.DisplayModel',
    component: 'RadioGroup',
    field: 'ShowType',
    componentProps: {
      options: [{ label: '大機種', value: 'A' }, { label: '小機種', value: 'B' },
      ]
    },
    defaultValue: 'A'
  },
  PanelSourceParams,
  ReworkedParams,
  {
    label: 'common.commonpages.LCMProductArea',
    component: 'RadioGroup',
    field: 'LCMProductArea',
    componentProps: {
      options: [
        {
          label: 'ALL',
          value: 'ALL'
        },
        {
          label: 'Moudle',
          value: "Moudle"
        },
        {
          label: 'SKD',
          value: "SKD"
        },
        {
          label: 'LED',
          value: "LED"
        },
        {
          label: 'Down Grade ',
          value: "Down Grade "
        },
      ],
    },
    defaultValue: 'ALL'
  },
  {
    field: 'LCMDivider',
    component: 'Divider',
    label: 'common.commonpages.LCMProduct'
  },
  ...new Size(optionsdata, 'LCM1').getSize(),
  LCMOwnerParams,
  LCDGrade1Params,
  {
    label: 'common.commonpages.DefectCode',
    component: 'Select',
    field: 'DefectCode',
    componentProps: ({ formModel,field }) => {
      return {
        mode: 'multiple',
        options: optionsdata.DefectCode,
        onSelect: (e: any) => {
          if (e != 'ALL') {
              formModel[field] = formModel[field].filter((item) => item != 'ALL');
          } else {
              formModel[field] = ['ALL'];
          }
      }
      }
    },
    defaultValue: ['ALL']
  },
];

