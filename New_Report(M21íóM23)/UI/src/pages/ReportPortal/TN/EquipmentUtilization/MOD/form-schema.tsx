import type { FormSchema } from '@/components/core/schema-form/';
// import { message } from 'ant-design-vue';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil'

import {FromDateParams,ToDateParams,ShiftParams,FloorParams} from '@/pages/ReportPortal/Static/TNCommonParams'

import {Equipment} from '@/pages/ReportPortal/Static/TNCommonParams'

export const schemas = (FactoryType:string)=>{
  let optionsdata = reactive({});
    return [
    ...FromDateParams,
    ...ToDateParams,
    ...ShiftParams,
    {
      label: 'common.commonpages.TN_Interval',
      component: 'RadioGroup',
      field: 'Interval',
      componentProps: {
        options: [
          {
            label: 'Shift',
            value: 'shift'
          },
          {
            label: 'Day',
            value: 'day'
          },
          {
            label: 'Week',
            value: 'week'
          },
          {
            label: 'Month',
            value: 'month'
          },
          {
            label: 'Equipment',
            value: 'equipment'
          },
          {
            label: 'Equipment+Shift',
            value: 'equipshift'
          },
        ]
      },
      defaultValue: 'shift'

    },
    ...FloorParams,
    
    {
      label: 'common.commonpages.EQStatus',
      component: 'RadioGroup',
      field: 'EQSTATUS',
      componentProps: {
        options: [
          {
            label: 'ALL',
            value: 'ALL'
          },
          {
            label: 'RUN',
            value: 'RUN'
          },
          {
            label: 'DOWN',
            value: 'DOWN'
          },
          {
            label: 'IDLE',
            value: 'IDLE'
          }
        ]
      },
      defaultValue: 'ALL'
    },

    ...new Equipment(optionsdata,FactoryType,"Equipment Utilization",{
        equip: 'equip',
        Optionequip: 'optionEquipment',
        equipselectmenu: '#optionequipmenu',
        equipment: 'EQPID'
    }).obj,
  ];
};
