import type { FormSchema } from '@/components/core/schema-form/';
import { HoldCode, LCMProductType, Operation, Size } from '@/pages/ReportPortal/Static/common';
import { reactive } from 'vue';
export let GrpOptions = [
  { value: 'ALL', label: '全选' },
  { value: 'prod_nbr', label: 'Lcm Product', dataIndex: "NVL(RTRIM(PROD_NBR),'')" },
  { value: 'lcm_owner', label: 'Lcm Owner', dataIndex: 'LCM_OWNER' },
  { value: 'work_ctr', label: 'Operation', dataIndex: 'WORK_CTR' },
  { value: 'hold_flag', label: 'Hold', dataIndex: 'HOLD_FLAG' },
  { value: 'hold_code', label: 'Hold_code', dataIndex: 'HOLD_CODE' },
  { value: 'b.c_Desc', label: 'Hold Desc', dataIndex: 'C_DESC' },
  { value: 'panel_size', label: 'Size', dataIndex: 'PANEL_SIZE' },
  { value: 'wo_nbr', label: 'Work Order', dataIndex: "NVL(RTRIM(WO_NBR),'')" },
  { value: 'LCD_PROD_NBR ', label: 'Lcd Product', dataIndex: 'LCD_PROD_NBR' },
  { value: 'lcd_grade', label: 'Lcd Grade', dataIndex: 'LCD_GRADE' },
  { value: 'cur_hold_comment', label: 'Hold comment', dataIndex: 'CUR_HOLD_COMMENT' },
  { value: 'due_dt', label: 'Due Day', dataIndex: 'DUE_DT' },
  { value: 'LAST_EQP_NBR', label: 'line',dataIndex: 'LAST_EQP_NBR'},
  { value: 'fg_type', label: 'wo_type' ,dataIndex:"FG_TYPE"},
];
export let optionsdata: any = reactive({
  ProductData: [],
  ProductData1: [],
  SizeData: [],
  BigProductData: [],
  BigProductData1: [],
  OperationLabel: [],
  LcmProdTypeData: [],
  HoldCodeData: [],
});

export const schemas = (FactoryType: string) => {
  return [
    {
      field: 'StartHours',
      component: 'Input',
      label: 'Start Hours',
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
            label: '1F',
            value: '1',
          },
          {
            label: '2F',
            value: '2',
          },
          {
            label: '3F',
            value: '3',
          },
        ],
      },
      defaultValue: '',
    },
    {
      label: 'Line Type',
      component: 'RadioGroup',
      field: 'line_type',
      componentProps: {
        options: [
          {
            label: 'ALL',
            value: '1',
          },
          {
            label: '主線',
            value: '2',
          },
          {
            label: 'RW',
            value: '3',
          },
        ],
      },
      defaultValue: '1',
    },
    {
      label: '工單類型',
      component: 'CheckboxGroup',
      field: 'fg_type',
      componentProps: {
        options: [
          {
            label: 'ALL',
            value: 'ALL',
          },
          {
            label: 'P',
            value: 'P',
          },
          {
            label: 'E',
            value: 'E',
          },
          {
            label: 'R',
            value: 'R',
          },
          {
            label: 'M',
            value: 'M',
          },
        ],
      },
      defaultValue: ['ALL'],
    },
    {
      label: 'Work Order Like',
      component: 'Input',
      field: 'Workorderlike',
    },
    ...new LCMProductType(optionsdata, FactoryType).obj,
    ...new Size(optionsdata, FactoryType).getSize(),
    // LCM2使用LCM1的operaction
    ...new Operation(optionsdata, 'LCM1').obj,
    ...new HoldCode(optionsdata, FactoryType).obj,
    {
      label: '顯示欄位',
      field: 'Colname',
      component: 'Select',
      componentProps: (model) => {
        return {
          mode: 'multiple',
          options: GrpOptions,
          onSelect: (e: any, option) => {
            if (e == 'ALL') {
              model.formModel['Colname'] = GrpOptions.map((item) => item.value).filter(
                (item) => item != 'ALL',
              );
            }
          },
        };
      },
    },
    {
      label: 'LCM product',
      component: 'InputTextArea',
      field: 'inputprod',
    },
  ];
};
