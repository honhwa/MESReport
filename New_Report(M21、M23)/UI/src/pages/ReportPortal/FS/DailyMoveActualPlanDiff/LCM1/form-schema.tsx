import type { FormSchema } from '@/components/core/schema-form/';
// import { message } from 'ant-design-vue';
import { reactive, ref } from 'vue';
import { formatToDate } from '@/utils/dateUtil'
import { Operation, Owner, Size, Size2 } from '@/pages/ReportPortal/Static/common';
import { log } from 'console';
import { GetCommon } from '@/api/pages/common';
import { resetRouter } from '@/router';

export let optionsdata: any = reactive({
  SizeData: [{ label: 'ALL', value: 'ALL' }],
  ProductData: [],
  BigProductData: [],
  ProductData1: [],
  BigProductData1: [],
  OwnerName: [],
  PlanVer: ""
})
async function getPlanVer(optionsdata) {
  let returndata: object[] = [];
  let Data = await GetCommon({
    name: 'Data',
    apiName: 'DailyPlanVer',
  });
  Data = Data.Data.Table
  Data.forEach((item) => {
    returndata.push(item['PLAN_VER'])
  });
  optionsdata['PlanVer'] = returndata;
  console.log(returndata);

  return returndata;

}

class PlanVer {
  _obj: FormSchema[];
  optionsdata;
  FactoryType: string;
  defaultValue: string;
  constructor(optionsdata, FactoryType: string, field: string = 'HoldCode') {
    this.optionsdata = optionsdata;
    this.FactoryType = FactoryType;
    optionsdata['FactoryType'] = this.FactoryType;
    this.defaultValue = '';
    this.init(optionsdata, FactoryType);
    console.log('输出tes', optionsdata.PlanVer.value);
    this.defaultValue = optionsdata['PlanVer'];
    this._obj = [];
  }
  // api請求數據
  async init(optionsdata, FactoryType: string) {
    await getPlanVer(optionsdata);
    return optionsdata;
  }
  // 獲取Marterial
  get obj() {
    this._obj = [
      {
        label: 'common.commonpages.WorkOrderLike',
        component: 'Input',
        field: 'Wo_nbr',
        defaultValue: this.defaultValue
      }
    ];
    return this._obj;
  }
  // 更改label
  setLabel(index: number, LabelValue: string) {
    this._obj[index]['label'] = LabelValue;
    return this;
  }
  // 更改默認值
  setDefault(index: number, defaultValue: string) {
    this._obj[index]['defaultValue'] = defaultValue;
    return this;
  }
}
export const schemas = () => {
  let defaultvalue: string;
  defaultvalue = optionsdata.PlanVer;
  // new getmount(FactoryType,optionsdata);
  return [
    {
      field: 'FromDate',
      component: 'DatePicker',
      label: 'common.commonpages.Date',
      componentProps: {
        valueFormat: 'YYYY-MM-DD'
      },
      defaultValue: formatToDate(),
      required: true
    },
    {
      field: 'ToDate',
      component: 'DatePicker',
      label: 'common.commonpages.To',

      componentProps: {
        valueFormat: 'YYYY-MM-DD'
      },
      defaultValue: formatToDate(),
      required: true
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
            value: " and shift_id='A'"
          },
          {
            label: '夜(B)',
            value: " and shift_id='B'"
          }
        ],
      },
      defaultValue: ''
    },
    {
      label: 'In/Out(act-plan)',
      component: 'RadioGroup',
      field: 'In/Out(act-plan)',

      componentProps: {
        options: [
          {
            label: 'In',
            value: 'IN'
          },
          {
            label: 'Out',
            value: 'OUT'
          }
        ]
      },
      defaultValue: 'IN'
    },
    {
      label: 'Type',
      component: 'RadioGroup',
      field: 'Type',

      componentProps: {
        options: [
          {
            label: '量產',
            value: 'DGDY,PROD,RES0,QTAP,PROY,DGDB,CRN0,PQOY'
          },
          {
            label: '重工',
            value: 'RMA0,RWK0,RWKY'
          },
          {
            label: '實驗',
            value: 'TVP0'
          }
        ]
      },
      defaultValue: 'DGDY,PROD,RES0,QTAP,PROY,DGDB,CRN0,PQOY'
    },
    {
      label: 'Plan Ver',
      component: 'Input',
      field: 'PlanVer',
      defaultValue: "",
      dynamicDisabled: true,
    },
    // ...new PlanVer(optionsdata, 'LCM1').obj,
    {
      field: 'LCMDivider',
      component: 'Divider',
      label: 'common.commonpages.LCMProduct'
    },
    ...new Size(optionsdata, 'LCM1').obj,
    ...new Owner(optionsdata, 'LCM1', 'LCMOwner').obj,
  ];
};

