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
    field: 'tmpWEEKCHK',
    component: 'RadioGroup',
    componentProps: {
      options: [
        {
          label: "By Month",
          value: "N",
        },
        {
          label: "By Week",
          value: "Y",
        }
      ]
    },
    defaultValue:'N'
  },
  // {
  //   label: 'Query DateTime',
  //   field: '123'
  // },
  {
    label: 'From',
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
  {
    label: 'DATA_PAT',
    field: 'DATA_PAT',
    component: 'Select',
    componentProps: () => {
      return {
        options: [
          {
            label:"M",
            value:"M"
          },{
            label:"P",
            value:"P"
          }
        ],
        placeholder: 'DATA_PAT',
      };
    },
    defaultValue:"P",
  },
  {
    label: 'Product',
    field: 'Product',
    component: 'Select',
    componentProps: () => {
      return {
        options: optionsdata.Product,
        placeholder: 'product',
      };
    },
    defaultValue:["ALL"],
  },
  {
    label: '樣本組數',
    field: 'cntID',
    component: 'Select',
    componentProps: () => {
      return {
        options: [
          {
            label:"ALL",
            value:"ALL"
          },{
            label:"CNT>=25",
            value:" CNT_SAMPLE>=25 "
          }
        ],
        placeholder: '樣本組數',
      };
    },
    defaultValue:"ALL",
  },
  
];
