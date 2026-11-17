import { reactive } from 'vue';
import {
  FromDateParams,
  ToDateParams,
  ShiftParams,
  FloorParams,
  PanelSource,
  InputWorkorderParams,
  Owner,
  Size,
  LCD_Grade,
} from '@/pages/ReportPortal/Static/TNCommonParams';

export const schemas = (FactoryType: string) => {
  let optionsdata = reactive({});
  return [
    ...FromDateParams,
    ...ToDateParams,
    ...ShiftParams,
    {
      label: 'common.commonpages.Interval',
      component: 'RadioGroup',
      field: 'Interval',
      componentProps: {
        options: [
          {
            label: 'Day',
            value: 'day',
          },
          {
            label: 'Week',
            value: 'week',
          },
          {
            label: 'Month',
            value: 'month',
          },
        ],
      },
      defaultValue: 'day',
    },
    ...FloorParams,
    ...new PanelSource(optionsdata, FactoryType, 'PanelSource').obj,
    ...InputWorkorderParams,
    ...new Size(optionsdata, FactoryType).obj,
    ...new Owner(optionsdata, FactoryType, 'owner_code').obj,
    ...new LCD_Grade(optionsdata, FactoryType, 'LcdGrade').obj,

    {
      label: '報廢單號碼',
      component: 'Input',
      field: 'scrap_nbr',
    },
  ];
};
