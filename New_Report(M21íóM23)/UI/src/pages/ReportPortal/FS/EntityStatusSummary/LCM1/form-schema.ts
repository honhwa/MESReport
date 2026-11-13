import type { FormSchema } from '@/components/core/schema-form/';
import { Date, Shift, Equipment } from '@/pages/ReportPortal/Static/common';
import { reactive } from 'vue';

export const schemas = (FactoryType: string) => {
  let optionsdata: any = reactive({});
  return [
    ...new Date()._obj,
    ...new Shift(1)._obj,
    {
      label: 'common.commonpages.ReportVer',
      component: 'RadioGroup',
      field: 'ReportVer',
      componentProps: {
        options: [
          {
            label: 'New Version',
            value: '1',
          },
          {
            label: 'Old Version',
            value: '2',
          },
        ],
      },
      defaultValue: '1',
    },
    {
      label: 'common.commonpages.ReportType',
      component: 'RadioGroup',
      field: 'ReportType1',
      componentProps: {
        options: [
          {
            label: 'By Total',
            value: '1',
          },
          {
            label: 'By Detail',
            value: '2',
          },
        ],
      },
      defaultValue: '1',
    },
    ...new Equipment(optionsdata, FactoryType, 0, {
      equip: 'Equipment',
      Optionequip: 'Optionequip',
      equipselectmenu: '#optionequipmenu',
      equipment: 'equip',
    }).obj,
  ];
};
