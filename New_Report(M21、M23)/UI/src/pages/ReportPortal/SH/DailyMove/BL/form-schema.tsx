import type { FormSchema } from '@/components/core/schema-form/';
// import { GetProduct, GetBigProduct } from '@/api/pages/daylimove';
// import { message } from 'ant-design-vue';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil';


export let optionsdata: any = reactive({
  SizeData: [{ label: 'ALL', value: 'ALL' }],
  ProductData: [],
  ProductData1: [],
  OwnerName: [{ label: 'ALL', value: '' }],
  OperationLabel: [],
  
})



export const schemas: FormSchema[] = [
  {
    field: 'FromDate',
    component: 'DatePicker',
    label: 'Date',
    componentProps: {
      valueFormat: 'YYYY-MM-DD'
    },
    defaultValue: formatToDate(),
    required: true
  },
  {
    field: 'ToDate',
    component: 'DatePicker',
    label: 'To',
    componentProps: {
      valueFormat: 'YYYY-MM-DD'
    },
    defaultValue: formatToDate(),
    required: true
  },
  {
    label: 'Shift',
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
    label: 'Floor',
    component: 'RadioGroup',
    field: 'Floor',
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
    label: 'Work Order',
    component: 'InputTextArea',
    field: 'Wo_nbr',

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
            optionsdata.ProductData1 = optionsdata.ProductData.filter(item => item?.value.indexOf(e) != -1);
            // 如果不为All,big product需要选中
            // schemas.
            /* schemas.forEach((item)=>{
              if(item.field =='Product'){
                console.log(item);
                console.log(optionsdata.BigProductData1);
                console.log(optionsdata.BigProductData1[0].value);
                
                
                item.defaultValue =  optionsdata.BigProductData1[0].value
              }
            }) */
          } else {
            optionsdata.ProductData1 = optionsdata.ProductData
          }

        },
      };
    },
    defaultValue:'ALL'
  },
  {
    label: 'Product',
    field: 'Prod_nbr',
    component: 'Select',
    componentProps: () => {
      return {
        mode: 'multiple',
        options: optionsdata.ProductData1,
        placeholder: 'product',
      };
    }
  },
  {
    label: 'BL Owner',
    component: 'Select',
    field: 'LcmOwner',
    componentProps: () => {
      return {
        mode: 'multiple',
        options: optionsdata.OwnerName,
        placeholder: 'LcmOwner',
      };
    },
    //defaultValue:['']
  
  },
  {
    label: 'Operation',
    component: 'Select',
    field: 'Operation',
    componentProps: () => {
      return {
        mode: 'multiple',
        options: optionsdata.OperationLabel,
        //options: optionsdata.OperationLabel.map((item:Object) => { return { label: item['label'], value: item['value'].split('-')[0],} }),
        placeholder: 'Operation',
      };
    },
   // defaultValue:['']
  },
  
];
