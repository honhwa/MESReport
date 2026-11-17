import type { FormSchema } from '@/components/core/schema-form/';
import { Size, Operation } from '@/pages/ReportPortal/Static/common';
import { reactive } from 'vue';
export let optionsdata: any = reactive({
  ProductData: [],
  ProductData1: [],
  SizeData: [],
  BigProductData: [],
  BigProductData1: [],
  OperationLabel: [],
});

export let GrpOptions = [
  { value: 'ALL', label: '全選' },
  { value: 'prod_nbr', label: 'Lcm product' },
  { value: 'lcm_owner', label: 'owner' },
  { value: 'work_ctr', label: 'operation' },
  { value: 'hold_flag', label: 'hold' },
  { value: 'hold_code', label: 'hold_code' },
  { value: 'panel_size', label: 'size' },
  { value: 'wo_nbr', label: 'work order' },
  { value: 'COMMENT1', label: 'Hold commit' },
];
export const schemas = (FactoryType: string) => {
  let optiondata = reactive({});
  return [
    {
      label: 'Work Order Like',
      component: 'Input',
      field: 'Workorderlike',
    },
    {
      label: 'Floor',
      component: 'RadioGroup',
      field: 'Floor',
      componentProps: {
        options: [
          {
            label: 'ALL',
            value: '',
          },
          {
            label: '2F',
            value: " and floor='2'",
          },
          {
            label: '3F',
            value: " and floor='3'",
          },
        ],
      },
      defaultValue: '',
    },
    ...new Size(optiondata, FactoryType, {
      size: 'Size',
      bigproduct: 'bigProduct',
      optionproduct: 'OptionProduct',
      selectmenu: '#optionmenu',
      product: 'Product',
    }).obj,
    ...new Operation(optiondata, FactoryType, 'Operation').obj,
    {
      label: 'Hold Code',
      field: 'HoldCode',
      component: 'Select',
      componentProps: {
        mode: 'multiple',
        options: [{ label: 'ALL', value: 'ALL' }],
      },
      defaultValue: ['ALL'],
    },
    {
      label: '顯示欄位',
      field: 'Colname',
      component: 'Select',
      componentProps: (model) => {
        return {
          mode: 'multiple',
          options: GrpOptions,
          onSelect: (e: any) => {
            if (e == 'ALL') {
              model.formModel.Colname = GrpOptions.filter((item) => {
                return item.value != 'ALL';
              }).map((item) => {
                return item.value;
              });
            } else {
              model.formModel.Colname = model.formModel.Colname.filter((item) => {
                return item.value != 'ALL';
              });
            }
          },
        };
      },
    },
  ];
};
