import type { FormSchema } from '@/components/core/schema-form/';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil'
import { Owner, Size } from '@/pages/ReportPortal/Static/common';

export let optionsdata: any = reactive({
  SizeData: [{ label: 'ALL', value: 'ALL' }],
  ProductData: [],
  BigProductData: [],
  ProductData1: [],
  BigProductData1: [],
  OwnerName:[],

})

export const schemas= (FactoryType:string)=>{
  return [
    {
      label: 'Date',
      field: 'vDate_e1',
      component: 'DatePicker',
      componentProps: {
        valueFormat: 'YYYY-MM-DD'
      },
      defaultValue: formatToDate(),
      required: true
    },
    //  type 下拉单选框
    {
      label: 'type',
      component: 'Select',
      field: 'type1',
      componentProps: {
        options: [
          {
            label: "all",
            value: "all"
          },
          {
            label: "hold",
            value: "hold"
          },
          {
            label: "non-hold",
            value: "non-hold"
          },
        ]
      },
      defaultValue:"all"
  
    },
    ...new Size(optionsdata,FactoryType,{size:'OperGroup',bigproduct:'EntyGroup',optionproduct:'optionproduct',selectmenu:"selectmenu",product:'prod_nbr'}).getSize(),
    /* {
      label: 'size',
      component: 'Select',
      field: 'OperGroup',
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
      defaultValue:["ALL"]
    },
    {
      label: 'big product',
      component: 'Select',
      field: 'EntyGroup',
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
      defaultValue:["ALL"]
  
    },
    {
      label: 'Product',
      field: 'prod_nbr',
      component: 'Select',
      componentProps: () => {
        return {
          mode: 'multiple',
          options: optionsdata.ProductData1,
          placeholder: 'product',
        };
      },
      defaultValue:["ALL"],
    }, */
    /* {
      label: 'LCM Owner',
      component: 'Select',
      field: 'owner_code',
      componentProps: () => {
        return {
          mode: 'multiple',
          options: optionsdata.OwnerName,
        };
      },
      defaultValue:["ALL"],
    }, */
    ...new Owner(optionsdata,FactoryType,"owner_code").obj,
    {
      label: 'LCM product',
      component: 'InputTextArea',
      field: 'inputprod',
    },
  ];
  
} 
