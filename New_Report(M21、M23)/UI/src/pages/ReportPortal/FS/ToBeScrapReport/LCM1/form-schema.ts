import type { FormSchema } from '@/components/core/schema-form/';
import {
  StartDateParams, EndDateParams, ShiftParams, LcmProductParams,
  LCMOwnerParams, LCMProductSize1Params, LCDGradeParams, LcdProductParams,
  LCMProductTypeParams, PanelSourceParams
} from '@/pages/ReportPortal/Static/CommonParams'
import { Size } from '@/pages/ReportPortal/Static/common';
import {reactive} from 'vue'



export let optionsdata: any = reactive({
});
export const schemas: FormSchema[] = [
  StartDateParams,
  EndDateParams,
  ShiftParams,
  PanelSourceParams,
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
        }, {
          label: 'Down Grade ',
          value: "Down Grade "
        },
      ],
    },
    defaultValue: 'ALL'
  },
  {
    label: 'common.commonpages.LCMProductSize',
    component: 'RadioGroup',
    field: 'LCMProductSize',
    componentProps: {
      options: [
        {
          label: 'ALL',
          value: 'ALL'
        },
        {
          label: '37”以下',
          value: "37”以下"
        }, {
          label: '37”及以上',
          value: "37”及以上"
        },
      ],
    },
    defaultValue: 'ALL'
  },
  LCMProductSize1Params,
  LCMProductTypeParams,
  {
    field: 'LCMDivider',
    component: 'Divider',
    label: 'common.commonpages.LCMProduct'
  },
  ...new Size(optionsdata, 'LCM1').getSize(),

  LCMOwnerParams,
  {
    field: 'LCDDivider',
    component: 'Divider',
    label: 'common.commonpages.LCDProduct'
  },
  ...LcdProductParams,
  LCDGradeParams,

];

