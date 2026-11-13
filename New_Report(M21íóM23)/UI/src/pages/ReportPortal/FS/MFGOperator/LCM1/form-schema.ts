import type { FormSchema } from '@/components/core/schema-form/';
import {
  ShiftParams2, LcmProductParams, LCMOwnerParams, StartDateParams, EndDateParams, OperationParams,
} from '@/pages/ReportPortal/Static/CommonParams'
import {Grade,Size} from '@/pages/ReportPortal/Static/common';
import { reactive } from 'vue'

export let optionsdata: any = reactive({
  SizeData: [{ label: 'ALL', value: 'ALL' }],
  ProductData: [],
  BigProductData: [],
  ProductData1: [],
  BigProductData1: [],
  OperationLabel: [],
});
export const schemas: FormSchema[] = [
  {
    label: '查询类型',
    component: 'RadioGroup',
    field: 'QueryType',
    componentProps: {
      options: [
        {
          label: 'BY 機種 ',
          value: "Equipment"
        },
        {
          label: 'BY 小时 ',
          value: "Hour"
        }
      ]
    },
    defaultValue: 'Equipment',
  },
  StartDateParams,
  EndDateParams,
  ShiftParams2,
  {
    label: 'common.commonpages.Floor',
    component: 'RadioGroup',
    field: 'Floor',
    componentProps: {
      options: [
        {
          label: 'ALL',
          value: ''
        },
        {
          label: '1F',
          value: " and floor='1'"
        },
        {
          label: '2F',
          value: " and floor='2'"
        },
        {
          label: '3F',
          value: " and floor='3'"
        }
      ]
    },
    defaultValue: '',
  },

  {
    label: 'common.commonpages.QueryType',
    component: 'RadioGroup',
    field: 'QType',
    componentProps: {
      options: [{ value: 'operator', label: '人員別' }, { value: 'equip_nbr', label: '機台別' }]
    },
    defaultValue: 'operator'

  },
  {
    field: 'LCMDivider',
    component: 'Divider',
    label: 'LCMProduct'
  },

  ...new Size(optionsdata, 'LCM1').getSize(),
  LCMOwnerParams,
  {
    label: 'common.commonpages.Operation',
    component: 'Select',
    field: 'Operation',
    componentProps: () => {
      return {
        options: optionsdata.OperationLabel
      }
    },
    defaultValue: 'ALL'
  },
  ...new Grade(optionsdata, 'LCM1').setLabel(0, 'common.commonpages.LCMGrade')._obj,
];

