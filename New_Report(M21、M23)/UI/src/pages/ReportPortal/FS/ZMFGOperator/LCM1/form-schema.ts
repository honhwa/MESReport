import type { FormSchema } from '@/components/core/schema-form/';
import {
  ShiftParams2, LCMOwnerParams,FloorParams2,StartDateParams,EndDateParams,OperationParams,
} from '@/pages/ReportPortal/Static/CommonParams'
import {  Size,Equipment } from '@/pages/ReportPortal/Static/common';
import {reactive} from 'vue'


export let optionsdata: any = reactive({
  });
export const schemas: FormSchema[] = [
  StartDateParams,
  EndDateParams,
  ShiftParams2,
  FloorParams2,
  {
    label: 'common.commonpages.LCMProductArea',
    component: 'RadioGroup',
    field: 'LCMProductArea',
    componentProps: {
      options: [
        {
          label: 'ALL',
          value: ''
        },
        {
          label: 'Moudle',
          value: "Moudle"
        },
        {
          label: 'SKD',
          value: "SKD"
        },
      ],
    },
    defaultValue: ''
  }, 
  {
    label: 'common.commonpages.QueryType',
    component: 'RadioGroup',
    field: 'QType',
    componentProps: {
      options: [{value:'operators',label:'人員別'},{value:'equip_nbr',label:'機台別'}]
    },
    defaultValue: 'operators'
  
  },
  {
    label:'Operator',
    component:'Input',
    field:'Operator',

  },
  // {
  //   label:'TestButton',
  //   component:'AButton',
  //   field:'TestButton',
  //   componentSlots: () => {
  //     return {
  //       name: () => 'TestButton',
  //       // suffix: () => 'Event Code',
  //     };
  //   },
  // },
  {
    field: 'LCMDivider',
    component: 'Divider',
    label: 'LCM Product'
  },
  ...new Size(optionsdata, 'LCM1').getSize(),
  LCMOwnerParams,
  ...new Equipment(optionsdata, 'LCM1', 0).obj,
];



