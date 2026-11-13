import type { FormSchema } from '@/components/core/schema-form/';
// import { message } from 'ant-design-vue';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil'


export let optionsdata: any = reactive({
  SizeData: [{ label: 'ALL', value: 'ALL' }],
  ProductData: [],
  BigProductData: [],
  ProductData1: [],
  BigProductData1: [],
  OwnerName:[],
  MeasTool:[],
  GroupID:[],
  Process:[],
})

export let GrpOptions = [
  { value: "OS_ACF", label: "OS_ACF" },
  { value: "S_TCP", label: "S_TCP" },
  { value: "G_TCP", label: "G_TCP" },
  { value: "PS_ACF", label: "PS_ACF" },
  { value: "X_PCB", label: "X_PCB" },
  { value: "X_PCB(R)", label: "X_PCB(R)" },
  { value: "Y_PCB", label: "Y_PCB" },
  { value: "CB", label: "CB" },
  { value: "BL", label: "BL" },
  { value: "CNV", label: "CNV" },
  { value: "INV", label: "INV" },
  { value: "BEZEL", label: "Bezel" },
  { value: "L-BEZEL", label: "L-Bezel" },
  { value: "R-BEZEL", label: "R-Bezel" },
  { value: "U-BEZEL", label: "U-Bezel" },
  { value: "D-BEZEL", label: "D-Bezel" },
  { value: "D_FRAME", label: "D_FRAME" },
  { value: "PCBA-X(L)L", label: "PCBA-X(L)L" },
  { value: "PCBA-X(L)R", label: "PCBA-X(L)R" },
  { value: "PCBA-X(R)L", label: "PCBA-X(R)L" },
  { value: "PCBA-X(R)R", label: "PCBA-X(R)R" },
  { value: "XC_PCB", label: "XC_PCB" },
  { value: "X_PCB(L)", label: "X_PCB(L)" },
]
let WotypeOptions = ['NA1', 'NA2', 'NA3'];
let text = ['報表被管控，如需查看請聯繫LCM'];
export const schemas2: FormSchema[] = [
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
export const schemas1: FormSchema[] = [
  { 
    field: 'NA1',
    component: 'RadioGroup',
    componentProps: {
      options: [
        {
          label: "NA1",
          value: "1",
        },
        {
          label: "NA2",
          value: "2",
          disabled:true
        },
        {
          label: "NA3",
          value: "3",
          disabled:true
        },
      ]
    },
    defaultValue:'1'
  },
  { 
    field: 'MeasureToolID',
    component: 'RadioGroup',
    componentProps: {
      options: [
        {
          label: "Measure ToolID ",
          value: "1",
        }
      ]
    },
    defaultValue:'1'
  },
  {
    label: 'MeasTool',
    component: 'Select',
    field: 'MeasTool',
    componentProps: () => {
      return {
        options: optionsdata.MeasTool,
      };
    },
    defaultValue:["CMO01"]
  },
  {
    label: 'Pre-Proc',
    component: 'Select',
    field: 'Pre-Proc',
    componentProps: () => {
      return {
        options: optionsdata.MeasTool,
      };
    },
    defaultValue:["AOPS01"]
  },
  {
    label: 'MES_ID',
    component: 'Select',
    field: 'MES_ID',
    componentProps: () => {
      return {
        options: optionsdata.Process,
      };
    },
    defaultValue:["ALL"]
  },
  {
    label: 'Group ID',
    component: 'Select',
    field: 'GroupID',
    componentProps: () => {
      return {
        options: optionsdata.GroupID,
      };
    },
    defaultValue:["ALL"]

  }
];
