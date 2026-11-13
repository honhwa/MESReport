import type { FormSchema } from '@/components/core/schema-form/';
import dayjs from 'dayjs';
import { message } from 'ant-design-vue';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil'
export let optionsdata: any = reactive({
  //SizeData: [{ label: 'ALL', value: 'ALL' }],
  MontTimeDatahLabel:[],
})
export const schemas2: FormSchema[] = [
  {
    label: '请输入密码',
    field: 'password',
    component: 'InputPassword',
    required: true
  },
  {
    label: '報表被管控，如需查看請聯繫REPORT窗口',
    field: 'test123',
  }
];

export const schemas: FormSchema[] = [
  
  {
    // label: '請選擇月份',
    label: 'common.commonpages.DateMonth',
    component: 'Select',
    field: 'data_month',
    componentProps: () => {
      return {
        options: optionsdata.TimeData,
      };
    },
    defaultValue:String(dayjs().year()) + dayjs().format("MM")
  },
  {
    //label: '請選擇數據類型',
    label: 'common.commonpages.DataType',
    component: 'RadioGroup',
    field: 'data_typ',
    componentProps: {
      options: [
        {
          label: 'IN',
          value: 'IN'
        },
        {
          label: 'OUT',
          value: 'OUT'

        }
      ],
    },
    defaultValue: 'IN'
  },

  {
    //label: '請輸入計劃版本',
    label: 'common.commonpages.PlanVersion',
    component: 'InputTextArea',
    field: 'PlanVersion',

  },
  
]
