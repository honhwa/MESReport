import type { FormSchema } from '@/components/core/schema-form/';
// import { message } from 'ant-design-vue';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil'
import dayjs from 'dayjs';


// 定义从后端返回的下拉选择框的数据
export let optionsdata: any = reactive({
  ProductData: [],
  ProductData1: [],
  SizeData: [],
  BigProductData: [],
  BigProductData1: [],
  OwnerName: [],
  LcdGradeLabel: [],
})

export const schemas: FormSchema[] = [
  {
    label: 'Date/Time',
    field: 'FromDate',
    component: 'DatePicker',
    componentProps: {
      valueFormat: 'YYYY-MM-DD',
    },
    defaultValue: formatToDate(),
    required: true,
  },
  {
    label: '开始时间',
    field: 'FromTime',
    component: 'TimePicker',
    componentProps: {
      minuteStep: 30,
      format: "HH:mm",
    },
    defaultValue: dayjs('07:30', 'HH:mm'),
    required: true,
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
  },
  {
    label: '结束时间',
    field: 'ToTime',
    component: 'TimePicker',
    componentProps: {
      minuteStep: 30,
      format: "HH:mm",
    },
    defaultValue: dayjs('07:30', 'HH:mm'),
    required: true,
  },
  {
    label: 'Shift',
    component: 'RadioGroup',
    field: 'Shift',
    componentProps: {
      options: [
        {
          label: 'ALL',
          value: ''
        },
        {
          label: '日(A)',
          value: "and shift_id='A'"
        },
        {
          label: '夜(B)',
          value: "and shift_id='B'"
        }
      ],
    },
    defaultValue: ''
  },
  {
    label: 'Floor',
    component: 'RadioGroup',
    field: 'Floor',
    componentProps: {
      options: [
        {
          label: 'ALL',
          value: ''
        },
        {
          label: '1F',
          value: " and floor='1'"
        },
        {
          label: '2F',
          value: " and floor='2'"
        },
        {
          label: '3F',
          value: " and floor='3'"
        }
      ]
    },
    defaultValue: '',
  },
  {
    label: 'Operation',
    component: 'RadioGroup',
    field: 'Operation',
    componentProps: {
      options: [
        {
          label: 'ALL',
          value: ''
        },
        {
          label: 'Sampling',
          value: "and q.work_ctr in (1700,1800,1900,1905) "
        },
        {
          label: 'OBA',
          value: "and q.work_ctr=1910"
        },
        {
          label:"2450",
          value:"and q.work_ctr=2450"
        },
      ]
    },
    defaultValue: '',
  },
 
  {
    label: 'size',
    component: 'Select',
    field: 'Size',
    componentProps: () => {
      return {
        options: optionsdata.SizeData,
        placeholder: 'size',
        onChange: async (e: any) => {
          if (e !== 'ALL') {
            optionsdata.BigProductData1 = optionsdata.BigProductData.filter(item => item?.value.indexOf(e) != -1);

          } else {
            optionsdata.BigProductData1 = optionsdata.BigProductData
          }

        },
      };
    },
    defaultValue: 'ALL'
  },
  {
    label: 'big product',
    component: 'Select',
    field: 'bigProduct',
    componentProps: () => {
      return {
        options: optionsdata.BigProductData1,
        placeholder: 'big product',
        onChange: async (e: any) => {
          if (e !== 'ALL') {
            optionsdata.ProductData1 = optionsdata.ProductData.filter(item => item?.value.indexOf(e) != -1)
          } else {
            optionsdata.ProductData1 = optionsdata.ProductData
          }
        }
      };
    },
    defaultValue: 'ALL'
  },
  {
    label: 'Product',
    field: 'Product',
    component: 'Select',
    componentProps: () => {
      return {
        mode: 'multiple',
        options: optionsdata.ProductData1,
        placeholder: 'product',
      };
    },
    defaultValue: ['ALL']

  },
 
  {
    label: 'LCM Owner',
    component: 'Select',
    field: 'LCMOwner',
    componentProps: () => {
      return {
        mode: 'multiple',
        options: optionsdata.OwnerName,
      };
    },
    defaultValue: ["ALL"],
  },

  {
    label: 'Judgement',
    component: 'Select',
    field: 'Judgement',
    componentProps: {
      mode: "multiple",
      options: [
        {
          label: "ALL",
          value: ""
        },
        {
          label: "OK",
          value: "(rank<>'2')"
        },
        {
          label: "Down Grade",
          value: "(q.grade_new > q.grade_old and rank <> '1')"
        },
        {
          label: "Up Grade",
          value: "(q.grade_new < q.grade_old and rank <> '1')"
        },
        {
          label: "NG",
          value: "(q.grade_new = q.grade_old and rank='2')"
        },
      ]
    },
    // defaultValue: ["1=1"],
    defaultValue: [''],
  },
  {
    label: 'Defect Group',
    component: 'Select',
    field: 'DefectGroup',
    componentProps: () => {
      return {
        mode: 'multiple',
        options: [
          {
            label:"ALL",
            value:"ALL"
          },
          {
            label:"電氣",
            value:"C"
          },
          {
            label:"品位",
            value:"F"
          },
          {
            label:"外觀",
            value:"F"
          },
        ]
      };
    },
    defaultValue:["ALL"]

  },
  {
    label: 'LCD Grade',
    component: 'Select',
    field: 'LCDGrade',
    componentProps: () => {
      return {
        mode: 'multiple',
        options: optionsdata.LcdGradeLabel,
      };
    }

  },


];
