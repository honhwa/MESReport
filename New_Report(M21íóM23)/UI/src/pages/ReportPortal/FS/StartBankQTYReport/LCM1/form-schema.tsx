import type { FormSchema } from '@/components/core/schema-form/';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil'

export let optionsdata: any = reactive({
  ProductData: [],
  ProductData1: [],
  SizeData: [],
  BigProductData: [],
  BigProductData1: [],
  LCDProductData: [],
  BigLCDProductData: [],
  BigLCDProductData1: [],
})

export const schemas:FormSchema[] =  [
  {
    field: 'ToDate',
    component: 'DatePicker',
    label: 'Date',
    componentProps: {
      valueFormat: 'YYYY-MM-DD'
    },
    defaultValue: formatToDate(),
    required: true
  },

  {
    label: 'size',
    component: 'Select',
    field: 'Size',
    componentProps: () => {
      return {
        options: optionsdata.SizeData,
        placeholder: 'size',
        onChange: async (e: any) => {
          if (e !== 'ALL') {
            optionsdata.BigProductData1 = optionsdata.BigProductData.filter(item => item?.value.indexOf(e) != -1);

          } else {
            optionsdata.BigProductData1 = optionsdata.BigProductData
          }

        },
      };
    },
    defaultValue: 'ALL'
  },
  {
    label: 'big product',
    component: 'Select',
    field: 'bigProduct',
    componentProps: () => {
      return {
        options: optionsdata.BigProductData1,
        placeholder: 'big product',
        onChange: async (e: any) => {
          if (e !== 'ALL') {
            optionsdata.ProductData1 = optionsdata.ProductData.filter(item => item?.value.indexOf(e) != -1)
          } else {
            optionsdata.ProductData1 = optionsdata.ProductData
          }
        }
      };
    },
    defaultValue: 'ALL'
  },
  {
    label: 'Product',
    field: 'Product',
    component: 'Select',
    componentProps: () => {
      return {
        mode: 'multiple',
        options: optionsdata.ProductData1,
        placeholder: 'product',
      };
    },
    // defaultValue: ['ALL']

  },
 
  {
    label: 'LCD Product Size',
    component: 'Select',
    field: 'LCDProductSize',
    componentProps: () => {
      return {
        options: optionsdata.LCDProductData,
        onChange: async (e: any) => {
          let BigLCDProductData: object[] = [];
          if (e !== 'ALL') {
            optionsdata.BigLCDProductData1.forEach(item => {
              if (item["SIZE1"] == e) {
                BigLCDProductData.push({
                  label: item["PART"],
                  value: item["PART"],
                })
              }
            });
            optionsdata.BigLCDProductData = BigLCDProductData
          } else {
            optionsdata.BigLCDProductData = optionsdata.tempBigLCDProductData
          }
        }
      };
    },
    defaultValue: ["ALL"],
  },
  {
    label: 'LCD Product',
    component: 'Select',
    field: 'LCDProduct',
    componentProps: () => {
      return {
        mode: 'multiple',
        options: optionsdata.BigLCDProductData,
      };
    },
    defaultValue: ["ALL"],
  },
 
]

