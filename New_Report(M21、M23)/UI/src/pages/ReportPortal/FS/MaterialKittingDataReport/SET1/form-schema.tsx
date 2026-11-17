import type { FormSchema } from '@/components/core/schema-form/';
// import { message } from 'ant-design-vue';
import { Equipment, FSType, getSizeData } from '@/pages/ReportPortal/Static/common'
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil'
import { Size } from '@/pages/ReportPortal/Static/common';


// export let optionsdata: any = reactive({
//   SizeData: [{ label: 'ALL', value: 'ALL' }],
//   ProductData: [],
//   BigProductData: [],
//   ProductData1: [],
//   BigProductData1: [],
//   OwnerName: [],

// })

export let GrpOptions = [
  { value: "Bkt_L", label: "Bkt_L" },
  { value: "Bkt_R", label: "Bkt_R" },
  { value: "Isolator", label: "Isolator" },
  { value: "Controller", label: "Controller" },
  { value: "MB", label: "MB" },
  { value: "WB", label: "WB" },
  { value: "PB", label: "PB" },
  { value: "WF_B", label: "WF_B" },
  { value: "PWR_Cord", label: "PWR_Cord" },
]
let WotypeOptions = ['By日期查詢'];
let text = ['報表被管控，如需查看請聯繫LCM'];
export const schemas2 = () => {
  return [
    {
      label: '请输入密码',
      field: 'password',
      component: 'InputPassword',
      required: true
    },
    {
      label: '報表被管控，如需查看請聯繫LCM MES窗口！電話82874',
      field: 'test123',
    }
  ];
};
export const schemas1 = () => {
  let optionsdata: any = reactive({
    SizeData: [{ label: 'ALL', value: 'ALL' }],
    ProductData: [],
    BigProductData: [],
    ProductData1: [],
    BigProductData1: [],
    // OwnerName: [],
  })
  return [
    {
      field: 'isWoStartDay',
      component: 'CheckboxGroup',
      componentProps: {
        options: WotypeOptions.map(item => {
          return { label: item, value: item }
        })
      },
    },
    {
      label: 'Date',
      field: 'FromDate',
      component: 'DatePicker',
      componentProps: {
        valueFormat: 'YYYY-MM-DD'
      },
      defaultValue: formatToDate(),
      required: true,
      dynamicDisabled: ({ formModel }) => formModel.isWoStartDay != 'By日期查詢'
    },
    {
      label: 'To',
      field: 'ToDate',
      component: 'DatePicker',
      componentProps: {
        valueFormat: 'YYYY-MM-DD'
      },
      defaultValue: formatToDate(),
      required: true,
      dynamicDisabled: ({ formModel }) => formModel.isWoStartDay != 'By日期查詢'
    },
    //  type 下拉单选框
    {
      label: 'Shift',
      component: 'RadioGroup',
      field: 'Shift',
      componentProps: {
        options: [
          {
            label: "ALL",
            value: ""
          },
          {
            label: "日(A)",
            value: " and shift_id='A'"
          },
          {
            label: "夜(B)",
            value: " and shift_id='B'"
          },
        ]
      },
      defaultValue: ""
    },
    {
      label: 'Floor',
      component: 'RadioGroup',
      field: 'Floor',
      componentProps: {
        options: [
          {
            label: "ALL",
            value: ""
          },
          {
            label: "1F",
            value: " and floor='1'"
          },
          {
            label: "2F",
            value: " and floor='2'"
          },
          {
            label: "3F",
            value: " and floor='3'"
          },
        ]
      },
      defaultValue: ""
    },
    {
      field: 'LCMDivider',
      component: 'Divider',
      label: 'common.commonpages.LCMProduct'
    },
    ...new Size(optionsdata, 'SET1').getSize(),
    ...new Equipment(optionsdata, 'SET1', 0).obj,
    {
      label: 'Material Type',
      component: 'Select',
      field: 'MaterialType',
      componentProps: {
        mode: "multiple",
        options: GrpOptions,
      },
    },
    {
      label: 'Panel ID',
      component: 'InputTextArea',
      field: 'PanelID',
      defaultValue: "",
      required: ({ formModel }) => formModel.isWoStartDay != 'By日期查詢'
    },
  ];
};
