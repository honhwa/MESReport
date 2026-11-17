import type { FormSchema } from '@/components/core/schema-form/';
import dayjs from 'dayjs';
import { message } from 'ant-design-vue';
import { FSType, RMAPartType,RMAFabType, RMAMatType, RMAProductType,RMAModelType } from '@/pages/ReportPortal/Static/common';
import { reactive } from 'vue';
import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect';
const { mountedRMAPartType } = useRemoteSelect();

import { formatToDate } from '@/utils/dateUtil'
export let optionsdata: any = reactive({
  //SizeData: [{ label: 'ALL', value: 'ALL' }],
  MontTimeDatahLabel:[],
  RMAFabType:[],
  RMAMatType:[],
  RMAProductType:[],
  RMAModelType:[],
  RMAPartType:[],
  LCMProductType: []
})


export const schemas2: FormSchema[] = [
  {//報表管控
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

export const schemas = ()=>{
  return [
    {
      // label: '請選擇月份',
      label: 'common.commonpages.DateMonth',
      component: 'Select',
      field: 'ACCT_DATE',
      componentProps: () => {
        return {
          options: optionsdata.TimeData,
        };
      },
      defaultValue:String(dayjs().year()) + dayjs().format("MM")
    },
    ...new RMAFabType(optionsdata, "LCM1", 'FAB').obj,
    ...new RMAModelType(optionsdata, "LCM1", 'Model').obj,
    ...new RMAProductType(optionsdata, "LCM1", 'Product').obj,
    ...new RMAMatType(optionsdata, "LCM1", 'Mat').obj,
    // {
    //   //label: '請輸入計劃版本',
    //   label: 'common.commonpages.PlanVersion',
    //   component: 'InputTextArea',
    //   field: 'plan_ver',
  
    // }
    
  ]
}
