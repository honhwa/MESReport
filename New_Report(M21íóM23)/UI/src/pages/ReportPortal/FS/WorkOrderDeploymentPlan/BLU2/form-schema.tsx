import type { FormSchema } from '@/components/core/schema-form/';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil';
import dayjs from 'dayjs';
import {
  ConmonGrade,
  Floor,
  FSType,
  Grade,
  LCMGrade,
  LCMProductSize,
  Owner,
  Shift,
  Size,
  Size2
} from '@/pages/ReportPortal/Static/common';
import { GetCommon } from '@/api/pages/common';
async function getOper(funcparams: String, FactoryType: String) {
  let ReturnData: any = [];
  let {
    Date: { Table: Data }
  } = await GetCommon({
    G_FAC: 'A',
    name: 'Date',
    apiName: funcparams,
    FactoryType: FactoryType
  });
  for (let i in Data) {
    if (FactoryType == 'BEOL2') {
      ReturnData.push({ label: Data[i]['PROC_ID'], value: Data[i]['PROC_ID'] });
    } else {
      //@ts-ignore
      ReturnData.push({
        label: `${Data[i]['WORK_CTR']}-${Data[i]['WORK_NAME']}`,
        value: Data[i]['WORK_CTR']
      });
    }
  }
  ReturnData.unshift({ label: 'ALL', value: 'ALL' });
  return ReturnData;
}
const mountedOper = async (optionsdata, FactoryType, optionsdataprop: string = 'OperationLabel') => {
  optionsdata[optionsdataprop] = await getOper('OQCQualityReport_Operation', FactoryType);
};
class Operation {
  _obj: FormSchema[];
  optionsdata: FSType;
  FactoryType: string;
  constructor(optionsdata: FSType, FactoryType: string, field: string = 'Operation') {
    this.optionsdata = optionsdata;
    this.FactoryType = FactoryType;
    optionsdata['FactoryType'] = this.FactoryType;
    this.init(optionsdata, this.FactoryType);
    this._obj = [
      {
        label: 'common.commonpages.Operation',
        component: 'Select',
        field: field,
        componentProps: (model) => {
          return {
            mode: 'multiple',
            options: optionsdata.OperationLabel,
            dropdownMatchSelectWidth: 150,
            onSelect: (e: any, option) => {
              if (e != 'ALL') {
                model.formModel[field] = model.formModel[field].filter((item) => item != 'ALL');
              } else {
                model.formModel[field] = ['ALL'];
              }
            }
          };
        },
        defaultValue: ['ALL']
      }
    ];
  }
  // api請求數據
  async init(optionsdata: FSType, FactoryType: string) {
    await mountedOper(optionsdata, FactoryType);
  }
  // 獲取Marterial
  get obj() {
    return this._obj;
  }
  // 更改label
  setLabel(index: number, LabelValue: string) {
    this._obj[index]['label'] = LabelValue;
    return this;
  }
}

export const schemas = (FactoryType: string) => {
  let optiondata = reactive({});
  let optiondata2 = reactive({});
  return [
    {
      label: 'Date/Time',
      field: 'FromDate',
      component: 'DatePicker',
      componentProps: {
        valueFormat: 'YYYY-MM-DD'
      },
      defaultValue: formatToDate(),
      required: true
    },
    {
      label: '开始时间',
      field: 'FromTime',
      component: 'TimePicker',
      componentProps: {
        minuteStep: 30,
        format: 'HH:mm'
      },
      defaultValue: dayjs('08:00', 'HH:mm'),
      required: true
    },
    {
      label: 'To',
      field: 'ToDate',
      component: 'DatePicker',
      componentProps: {
        valueFormat: 'YYYY-MM-DD'
      },
      defaultValue: formatToDate(),
      required: true
    },
    {
      label: '结束时间',
      field: 'ToTime',
      component: 'TimePicker',
      componentProps: {
        minuteStep: 30,
        format: 'HH:mm'
      },
      defaultValue: dayjs('08:00', 'HH:mm'),
      required: true
    },
    {
      label: 'Work Order like',
      field: 'WorkOrderlike',
      component: 'Input',
    },
    {
      field: 'LCMDivider',
      component: 'Divider',
      label: 'common.commonpages.LCMProduct'
    },
    ...new Size(optiondata, 'BLU2', {
      size: 'LCMprodsize',
      bigproduct: 'BigProduct',
      optionproduct: 'EntyGroup',
      selectmenu: '#optionmenu',
      product: 'Product'
    }).obj,
  ];
};
