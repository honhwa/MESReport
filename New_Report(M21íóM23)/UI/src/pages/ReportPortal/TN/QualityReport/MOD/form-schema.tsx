import type { FormSchema } from '@/components/core/schema-form/';
import { reactive } from 'vue';
import {
  FromDateParams,ToDateParams, ShiftParams, FloorParams,ReworkedParams, ReworkTypeParams} from '@/pages/ReportPortal/Static/TNCommonParams'

  import {Equipment,Size, Owner, Operation, LCD_Grade, PanelSource, DefectCode} from '@/pages/ReportPortal/Static/TNCommonParams'

  export const schemas = (FactoryType:string)=>{ 
    let optionsdata = reactive({});
  return [
    ...FromDateParams,
    ...ToDateParams,
    ...ShiftParams,
    ...FloorParams,
    ...new PanelSource(optionsdata,FactoryType,'PanelSource').obj,
    ...ReworkTypeParams,
    ReworkedParams,
    {
      label: 'common.commonpages.DisplayModel',
      component: 'RadioGroup',
      field: 'DisplayMode',
      componentProps: {
        options: [
          {
            label: '站點',
            value: 'work_ctr'
          },
          {
            label: '機台別',
            value: 'equip_nbr'
          },
          {
            label: '大機種',
            value: 'big_prod'
          }
        ]
      },
      defaultValue: 'work_ctr'
    },
    { 
      label: 'common.commonpages.WorkOrderLike',
      component: 'Input',
      field: 'Wo_nbr',
    },
    {
      label: 'common.commonpages.OperationLike',
      field: 'Operator',
      component: 'Input',
    },
    {
      label: 'Product',
      component: 'InputTextArea',
      field: 'Input_Prod_nbr',

    },  
    ...new Size(optionsdata, FactoryType).obj,
    ...new Owner(optionsdata,FactoryType,'owner_code').obj,
    ...new Operation(optionsdata, FactoryType,'workGroup').obj,
    ...new LCD_Grade(optionsdata,FactoryType,'LcdGrade').obj,
    //...TNByEQPGroupParams,
    ...new Equipment(optionsdata,FactoryType,"Quality Report",{
        equip: 'equip',
        Optionequip: 'optionEquipment',
        equipselectmenu: '#optionequipmenu',
        equipment: 'EQPID'
    }).obj,
    ...new DefectCode(optionsdata,FactoryType,'DefectCode').obj

  ];
};
