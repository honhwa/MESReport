import type { FormSchema } from '@/components/core/schema-form/';
// import { message } from 'ant-design-vue';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil'


export let optionsdata: any = reactive({
  SizeData: [{ label: 'ALL', value: 'ALL' }],
  ProductData: [],
  BigProductData: [],
  ProductData1: [],
  BigProductData1: [],
  OwnerName:[],

})

export const schemas: FormSchema[] = [
  {
    label: 'Query DateTime',
    component: 'RadioGroup',
    field: 'Duration',
    componentProps: {
      options: [
        {
          label: "By Month",
          value: "N",
        },
        {
          label: "By Week",
          value: " and shift_id='A'"
        },
      ]
    },
    defaultValue:"N"
  },
  {
    label: 'Date',
    field: 'FromDate',
    component: 'DatePicker',
    componentProps: {
      valueFormat: 'YYYY-MM-DD'
    },
    defaultValue: formatToDate(),
    required: true,
  },
  {
    label: 'To',
    field: 'ToDate',
    component: 'DatePicker',
    componentProps: {
      valueFormat: 'YYYY-MM-DD'
    },
    defaultValue: formatToDate(),
    required: true,
  },
  //  type 下拉单选框
  {
    label: 'Module',
    field: 'Module',
    component: 'Select',
    componentProps: () => {
      return {
        options: optionsdata.Product,
        placeholder: 'product',
      };
    },
    defaultValue:"ALL",
  },
];
