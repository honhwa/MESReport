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
    field: 'NA1',
    component: 'RadioGroup',
    componentProps: {
      options: [
        {
          label: "NA1",
          value: "1",
        },
        {
          label: "NA2",
          value: "2",
          disabled:true
        },
        {
          label: "NA3",
          value: "3",
          disabled:true
        },
      ]
    },
    defaultValue:'1'
  },
  // {
  //   label: 'Query DateTime',
  //   field: '123'
  // },
  {
    label: 'Month',
    field: 'MonthDate',
    component: 'DatePicker',
    componentProps: {
      valueFormat: 'YYYY-MM',
      picker:"month"
    },
   defaultValue: formatToDate('YYYY-MM'),
    required: true,
  },
  //  type 下拉单选框
 /*  {
    label: 'Module',
    field: 'Module',
    component: 'Select',
    componentProps: () => {
      return {
        options: optionsdata.Module,
        placeholder: 'Module',
      };
    },
    defaultValue:["ALL"],
  }, */
];
