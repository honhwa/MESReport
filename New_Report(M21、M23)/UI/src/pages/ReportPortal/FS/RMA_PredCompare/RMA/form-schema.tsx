import type { FormSchema } from '@/components/core/schema-form/';
import dayjs from 'dayjs';
import { message } from 'ant-design-vue';
import { RMA3DateType, RMAPartType,RMAFabType, RMAPart3Type, Size } from '@/pages/ReportPortal/Static/common';
import { reactive } from 'vue';
import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect';
const { mountedRMAPartType } = useRemoteSelect();

import { formatToDate } from '@/utils/dateUtil'
export let optionsdata: any = reactive({
  //SizeData: [{ label: 'ALL', value: 'ALL' }],
  MontTimeDatahLabel:[],
  RMAFabType:[],
  RMA3DateType:[],
  RMAPart3Type:[],
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
  
    ...new RMAFabType(optionsdata, "LCM1", 'FAB').obj,
    ...new RMAPart3Type(optionsdata, "LCM1", 'PART').obj,
    ...new RMA3DateType(optionsdata, "LCM1", 'ACCT_DATE').obj,
    // {
    //   // label: '請選擇月份',
    //   label: 'common.commonpages.DateMonth',
    //   component: 'Select',
    //   field: 'ACCT_DATE',
    //   componentProps: () => {
    //     return {
    //       options: optionsdata.TimeData,
    //     };
    //   },
      
    //   defaultValue: '202212'
    //   // defaultValue:String(dayjs().year()) + dayjs().format("MM")
    // },
    {
      label: '產品狀態',
      component: 'Select',
      field: 'JD_TY',
      componentProps: () => {
          return {
              options: [
                  {
                    label: 'ALL',
                    value: 'ALL'
                  },
                  {
                      label: 'MP',
                      value: 'MP'
                  },
                  {
                      label: 'NA',
                      value: 'NA'
                  },
                  {
                      label: 'EOL',
                      value: 'EOL'
                  }
              ]
          };
      },
      defaultValue: 'ALL'
  }
         
    // {
    //   //label: '請輸入計劃版本',
    //   label: 'common.commonpages.PlanVersion',
    //   component: 'InputTextArea',
    //   field: 'plan_ver',
  
    // }
    
  ]
}
