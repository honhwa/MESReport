import type { FormSchema } from '@/components/core/schema-form/';
import { StartDateParams, EndDateParams, ShiftParams, LcmProductParams,OperationParams } from '@/pages/ReportPortal/Static/CommonParams'
import { optionsdata } from '@/pages/ReportPortal/FS/PanelWipStayChart/LCM1/form-schema';
import { Equipment, Size ,Operation} from '@/pages/ReportPortal/Static/common';
import { reactive } from 'vue';


export const schemas = (FactoryType:string)=>{
  let optiondata = reactive({});
  return [
    StartDateParams,
    EndDateParams,
    ShiftParams,
    {
      label: 'common.commonpages.ReportType',
      component: 'RadioGroup',
      field: 'ReportType',
      componentProps: {
        options: [
          {
            label: 'By Total',
            value: ''
          },
          {
            label: 'By Detail',
            value: "MoveOutTimeDataSetDetail"
          },
        ],
      },
      defaultValue: ''
    },
    OperationParams,
    /* {
      label: 'common.commonpages.Equipment',
      component: 'Select',
      field: 'BigEquipment',
      componentProps: ()=>{
        return {
          options:optionsdata.EquipmentBig,
          onChange: async (e: any) => {
            if (e !== 'ALL') {
              optionsdata.Equipment1 = optionsdata.Equipment.filter(item => item?.value.indexOf(e) != -1)
            } else {
              optionsdata.Equipment1 = optionsdata.Equipment
            }
          }
        }
      },
      defaultValue: 'ALL'
    },
    {
      // label: '23ds',
      component: 'Select',
      field: 'Equipment',
      componentProps: ()=>{
        return {
          options:optionsdata.Equipment1,
          
        }
      },
      defaultValue: 'ALL'
    }, */
    ...new Equipment(optiondata, FactoryType, 2, {
      equip: 'equip',
      Optionequip: 'Optionequip',
      equipselectmenu: '#optionequipmenu',
      equipment: 'Equipment',
    }).obj,
    // ...LcmProductParams,
    ...new Size(optiondata,FactoryType,{
      size: 'Size',
      bigproduct: 'bigproduct',
      optionproduct: 'OptionProduct',
      selectmenu: '#optionmenu',
      product: 'Product'
    }).obj,
  ];
}

