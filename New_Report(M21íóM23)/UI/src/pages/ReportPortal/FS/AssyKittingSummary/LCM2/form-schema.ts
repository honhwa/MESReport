import type { FormSchema } from '@/components/core/schema-form/';
import { reactive } from 'vue';
import { Size, Date ,Shift,Equipment} from '@/pages/ReportPortal/Static/common';

export const schemas = (FactoryType: string) => {
  let optionsdata = reactive({});
  return [
    ...new Date().obj,
    {
      label: '',
      component: 'Alert',
      field: '#alert',
      componentProps: {
        message:"期間請勿超過10天" ,
        type:"error",
        icon:true,
        banner:true,
        // description:"期間請勿超過10天"
      },
    },
    {
        label: 'Equipment Floor',
        component: 'RadioGroup',
        field: "Eqfloor",
        componentProps: {
            options: [
                {
                    label: 'ALL',
                    value: ''
                },
                {
                    label: '1F',
                    value: " and TRIM(equip_nbr) between 'ASSY01' and 'ASSY08' "
                },
                {
                    label: '2F',
                    value: " and TRIM(equip_nbr) between 'ASSY09' and 'ASSY20' "
                },
                {
                    label: '3F',
                    value:" and TRIM(equip_nbr) between 'ASSY21' and 'ASSY32' "
                }
            ]
        },
        defaultValue: ''
    },
    ...new Shift(1).obj,
    ...new Size(optionsdata,FactoryType).obj,
    ...new Equipment(optionsdata,FactoryType,1).obj
  ];
};
