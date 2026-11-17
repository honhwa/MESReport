import type { FormSchema } from '@/components/core/schema-form/';
// import { message } from 'ant-design-vue';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil'
import { Equipment } from '@/pages/ReportPortal/Static/common';


export let optionsdata: any = reactive({
  EquipmentData:[],
  BigEquipmentData:[],
  BigEquipmentData1:[],
})
export const schemas = (FactoryType:string)=>{
  return [
    {
      label: 'common.commonpages.Date',
      field: 'FromDate',
      component: 'DatePicker',
      componentProps: {
        valueFormat: 'YYYY-MM-DD'
      },
      defaultValue: formatToDate(),
      required: true
    },
    {
      label: 'common.commonpages.To',
      field: 'ToDate',
      component: 'DatePicker',
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
            value: ''
          },
          {
            label: '日(A)',
            value: "A"
          },
          {
            label: '夜(B)',
            value: "B"
          }
        ],
      },
      defaultValue: ''
    },
    {
      label: 'common.commonpages.Interval',
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
    {
      label: 'common.commonpages.Floor',
      component: 'RadioGroup',
      field: 'Floor',
      componentProps: {
        options: [
          {
            label: 'ALL',
            value: ''
          },
          {
            label: '1F',
            value: " and floor='1'"
          },
          {
            label: '2F',
            value: " and floor='2'"
          },
          {
            label: '3F',
            value: " and floor='3'"
          }
        ]
      },
      defaultValue: '',
    },
    
   /*  {
      label: 'Equipment',
      component: 'Select',
      field: 'equip',
      componentProps: () => {
        return {
          // mode: 'multiple',
          options: optionsdata.EquipmentData,
          onChange: async (e: any) => {
            if (e !== 'ALL') {
              optionsdata.BigEquipmentData1 = optionsdata.BigEquipmentData.filter(item => item?.value.indexOf(e) != -1)
            } else {
              optionsdata.BigEquipmentData1 = optionsdata.BigEquipmentData
            }
          }
        };
      },
      defaultValue:["ALL"],
    },
    {
      label: 'Big Equipment',
      component: 'Select',
      field: 'Equipment',
      componentProps: () => {
        return {
          mode: 'multiple',
          options: optionsdata.BigEquipmentData1,
        };
      },
      // defaultValue:["ALL"],
    }, */
    ...new Equipment(optionsdata,FactoryType,0,{
      equip: 'equip',
      Optionequip: 'Optionequip',
      equipselectmenu: '#optionequipmenu',
      equipment: 'Equipment'
    }).obj
  ]
};
