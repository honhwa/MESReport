import type { FormSchema } from '@/components/core/schema-form/';
// import { GetProduct, GetBigProduct } from '@/api/pages/daylimove';
// import { message } from 'ant-design-vue';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil';
import {Size, Owner, Operation} from '@/pages/ReportPortal/Static/TNCommonParams'


export const schemas = (FactoryType:string)=>{ 
  let optionsdata = reactive({});
  return [
    {
      field: 'vDate_s',
      component: 'DatePicker',
      label: 'common.commonpages.Date',
      componentProps: {
        valueFormat: 'YYYY-MM-DD'
      },
      defaultValue: formatToDate(),
      required: true
    },
    {
      field: 'vDate_e1',
      component: 'DatePicker',
      label: 'common.commonpages.To',


      componentProps: {
        valueFormat: 'YYYY-MM-DD'
      },
      defaultValue: formatToDate(),
      required: true
    },
    {
      label: 'common.commonpages.Shift',
      component: 'RadioGroup',
      field: 'Shift',

      componentProps: {
        options: [
          {
            label: 'ALL',
            value: 'ALL'
          },
          {
            label: '日(A)',
            value: "A"
          },
          {
            label: '夜(B)',
            value: 'B'
          }
        ],
      },
      defaultValue: 'ALL'

    },
    {
      label: 'common.commonpages.TN_Floor',
      component: 'RadioGroup',
      field: 'floor',

      componentProps: {
        options: [
          {
            label: 'ALL',
            value: 'ALL'
          },
          {
            label: '1F',
            value: '1'
          },
          {
            label: '2F',
            value: '2'
          },
          {
            label: '3F',
            value: '3'
          }
        ]
      },
      defaultValue: 'ALL'
    },
    {
      label: 'common.commonpages.WorkOrderLike',
      component: 'Input',
      field: 'Wo_nbr',


    },
    {
      label: 'common.commonpages.WorkOrder',
      component: 'InputTextArea',
      field: 'wo_num',

    },
    ...new Size(optionsdata, FactoryType).obj,
    ...new Owner(optionsdata,FactoryType,'owner_code').obj,
    ...new Operation(optionsdata, FactoryType,'workGroup').obj,
  ];
};
