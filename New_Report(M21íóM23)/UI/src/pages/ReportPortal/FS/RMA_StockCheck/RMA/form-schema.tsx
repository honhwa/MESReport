import type { FormSchema } from '@/components/core/schema-form/';
import dayjs from 'dayjs';
import { message } from 'ant-design-vue';
import { FSType, RMAPartType,RMAFabType, Owner, Size } from '@/pages/ReportPortal/Static/common';
import { reactive } from 'vue';
import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect';
const { mountedRMAPartType } = useRemoteSelect();

import { formatToDate } from '@/utils/dateUtil'
export let optionsdata: any = reactive({
  //SizeData: [{ label: 'ALL', value: 'ALL' }],
  MontTimeDatahLabel:[],
  RMAFabType:[],
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
  
    // {
    //   // label: '請選擇月份',
    //   label: 'common.commonpages.DateMonth',
    //   component: 'Select',
    //   field: 'data_month',
    //   componentProps: () => {
    //     return {
    //       options: optionsdata.TimeData,
    //     };
    //   },
    //   defaultValue:String(dayjs().year()) + dayjs().format("MM")
    // },
    {
      //label: '請選擇數據類型',
      label: 'common.commonpages.DataType',
      component: 'RadioGroup',
      field: 'data_typ',
      componentProps: {
        options: [
          {
            label: '庫存核檢模塊一',
            value: '1'
          },
          {
            label: '庫存核檢模塊二',
            value: '2'
  
          }
        ],
      },
      defaultValue: '1'
    },
  
    // ...new LCMProductType(optionsdata, "LCM1", 'ProdType').obj,
    ...new RMAFabType(optionsdata, "LCM1", 'FAB').obj,
    ...new RMAPartType(optionsdata, "LCM1", 'PART').obj
         
    // {
    //   //label: '請輸入計劃版本',
    //   label: 'common.commonpages.PlanVersion',
    //   component: 'InputTextArea',
    //   field: 'plan_ver',
  
    // }
    
  ]
  
}
