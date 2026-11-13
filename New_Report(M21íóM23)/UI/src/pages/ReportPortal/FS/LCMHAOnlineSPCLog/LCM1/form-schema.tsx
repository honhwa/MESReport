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
let WotypeOptions = ['1', '2', '3','4', '5', '6','7', '8', '9','10', '11', '12','13', '14', '15','16', '17', '18','19', '20', '21','22', '23'];
export const schemas1: FormSchema[] = [
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
        {
          label: "NAPX",
          value: "4",
        },
      ]
    },
    defaultValue:'1'
  },
  { 
    field: 'MeasureToolID',
    component: 'RadioGroup',
    componentProps: {
      options: [
        {
          label: "Measure ToolID ",
          value: "1",
        }
      ]
    },
    defaultValue:'1'
  },
  {
    label: 'MeasTool',
    component: 'Select',
    field: 'MeasTool',
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
    label: 'ProcTool',
    component: 'Select',
    field: 'ProcTool',
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
    label: 'Query Type',
    component: 'Select',
    field: 'QueryType',
    componentProps: {
      options: [
        {
          label: "Line",
          value: "Line",
        },
        {
          label: "Module Name",
          value: "Module Name",
        },
        {
          label: "Control Item",
          value: "Control Item",
        },
        {
          label: "Chart ID",
          value: "Chart ID",
        },
      ]
    },
    defaultValue:["Line"]
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
  {
    label: 'Before',
    component: 'Select',
    field: 'Before',
    componentProps: {
      options: WotypeOptions.map(item => {
        return { label: item, value: item }
      })
    },
    defaultValue:["1"]
  },
];
