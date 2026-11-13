import type { FormSchema } from '@/components/core/schema-form/';
// import { message } from 'ant-design-vue';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil'
import dayjs from 'dayjs';
import { Equipment } from '@/pages/ReportPortal/Static/common';


export let optionsdata: any = reactive({
 
  EquipmentData:[],
BigEquipmentData:[],
BigEquipmentData1:[],
})

let optiondata2 = reactive({});
export const schemas = ()=>{return[
  {
      label: 'common.commonpages.Date_show',
      field: 'FromDate',
      component: 'DatePicker',
      componentProps: {
          valueFormat: 'YYYY-MM-DD'
      },
      defaultValue: formatToDate(),
      required: true
  },
  ...new Equipment(optiondata2,"LCM2",4).obj,
  // {
  //     label: 'Equipment',
  //     component: 'Select',
  //     field: 'equip',
  //     componentProps: () => {
  //       return {
  //         // mode: 'multiple',
  //         options: optionsdata.EquipmentData,
  //         // onChange: async (e: any) => {
  //         //   if (e !== 'ALL') {
  //         //     optionsdata.BigEquipmentData1 = optionsdata.BigEquipmentData.filter(item => item?.value.indexOf(e) != -1)
  //         //   } else {
  //         //     optionsdata.BigEquipmentData1 = optionsdata.BigEquipmentData
  //         //   }
  //         // }
  //       };
  //     },
  //     defaultValue:["ALL"],
  //   },
 
]};
let FactoryType = 'LCM2';
export function pushFactoryType(sFactoryType: string) {
  FactoryType = sFactoryType;
  
}


