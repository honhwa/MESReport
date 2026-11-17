import type { FormSchema } from '@/components/core/schema-form/'
import dayjs from 'dayjs';
import { reactive } from 'vue';
export let optionsdata: any = reactive({
  TimeData: [],
  SizeData: [],
  ProductData: [],
  ProductData1: [],
  EquipmentSizeData: [],
  EquipmentGroupData: [],
  EquipmentGroupData1: [],
})
console.log(optionsdata.TimeData);

export const schemas: FormSchema[] = [
  {
    field: 'Months',
    component: 'Select',
    label: '月份',
    componentProps: () => {
      return {
        options: optionsdata.TimeData,
      };
    },
    // defaultValue:String(dayjs().year()) + "06"
    defaultValue:String(dayjs().year()) + dayjs().format("MM")
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
            console.log("执行了", e);
            optionsdata.ProductData = optionsdata.ProductData.filter(item => item?.value.includes(e));
          } else {
            optionsdata.ProductData = optionsdata.ProductData1
          }
        },
      };
    }
  },
  {
    label: 'Product',
    field: 'Product',
    component: 'Select',
    componentProps: () => {
      return {
        mode: 'multiple',
        options: optionsdata.ProductData,
        placeholder: 'product',
      };
    },
    defaultValue: ['ALL']
  },
  {
    label: 'Equipment:',
    component: 'Select',
    field: 'Equipment',
    componentProps: () => {
      return {
        options: optionsdata.EquipmentSizeData,
        placeholder: 'size',
        onChange: async (e: any) => {
          if (e !== 'ALL') {
            console.log("执行了", e);
            optionsdata.EquipmentGroupData = optionsdata.EquipmentGroupData.filter(item => item?.value.includes(e));
          } else {
            optionsdata.EquipmentGroupData = optionsdata.EquipmentGroupData1
          }
        },
      };
    }
  },
  {
    label: 'Equipment Group',
    field: 'EquipmentGroup',
    component: 'Select',
    componentProps: () => {
      return {
        mode: 'multiple',
        options: optionsdata.EquipmentGroupData,
        placeholder: 'product',
      };
    },
  },
];

