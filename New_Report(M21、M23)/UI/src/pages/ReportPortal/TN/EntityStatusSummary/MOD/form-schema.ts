import type { FormSchema } from '@/components/core/schema-form/';
import {FromDateParams,ToDateParams,ShiftParams,FloorParams} from '@/pages/ReportPortal/Static/TNCommonParams'
import {Equipment} from '@/pages/ReportPortal/Static/TNCommonParams'
import { reactive } from 'vue';
export let optionsdata: any = reactive({})

export const schemas = (FactoryType:string)=>{
  return [
    ...FromDateParams,
    ...ToDateParams,
    ...ShiftParams,
    ...new Equipment(optionsdata,FactoryType,"Equipment Utilization",{
      equip: 'equip',
      Optionequip: 'optionEquipment',
      equipselectmenu: '#optionequipmenu',
      equipment: 'EQPID'
  }).obj,
  ];
}

