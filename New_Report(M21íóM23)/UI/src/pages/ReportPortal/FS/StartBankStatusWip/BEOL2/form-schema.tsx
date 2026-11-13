import type { FormSchema } from '@/components/core/schema-form/';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil';
import { FSType } from '@/pages/ReportPortal/Static/common';
import { GetCommon } from '@/api/pages/common';
async function getOwner(params: String, funcparams: String, FactoryType: String) {
  let ReturnData: object[] = [];
  let Date = await GetCommon({
    G_FAC: 'A',
    name: 'Date',
    apiName: funcparams,
    FactoryType: FactoryType,
  });
  let Data;
  if (funcparams == 'LCMOwner') {
    Data = Date.Date.Data.Table;
  } else {
    Data = Date.Date.Table;
  }

  for (let i in Data) {
    //@ts-ignore
    ReturnData.push({ label: Data[i][params], value: Data[i][params] });
  }
  ReturnData.unshift({ label: 'ALL', value: 'ALL' });
  return ReturnData;
}
const mountedowner = async (optionsdata, FactoryType, optionsdataprop: string = 'OwnerName') => {
  let params = 'SOPTION';
  optionsdata[optionsdataprop] = await getOwner(params, 'StartBankStatusWipOwner', FactoryType);
};
export class Owner {
  _obj: FormSchema[];
  optionsdata: FSType;
  FactoryType: string;
  constructor(optionsdata: FSType, FactoryType: string, Field: string = 'Owner') {
    this.optionsdata = optionsdata;
    this.FactoryType = FactoryType;
    optionsdata['FactoryType'] = this.FactoryType;
    optionsdata['MarterialOption'] = [];
    this.init(optionsdata, this.FactoryType);
    this._obj = [
      {
        label: 'common.commonpages.LCMOwner',
        component: 'Select',
        field: Field,
        componentProps: (model) => {
          return {
            mode: 'multiple',
            options: optionsdata.OwnerName,
            dropdownMatchSelectWidth: 150,
            onSelect: (e: any, option) => {
              if (e != 'ALL') {
                model.formModel[Field] = model.formModel[Field].filter((item) => item != 'ALL');
              } else {
                model.formModel[Field] = ['ALL'];
              }
            },
          };
        },
        defaultValue: ['ALL'],
      },
    ];
  }
  // api請求數據
  async init(optionsdata: FSType, FactoryType: string) {
    await mountedowner(optionsdata, FactoryType);
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
export class SpecialSize {
  optionsdata: FSType;
  obj: FormSchema[];
  constructor(optionsdata: FSType) {
    this.optionsdata = optionsdata;
    optionsdata['tempoption'] = [];
    this.obj = this.setobj();
  }
  // 初始化obj
  setobj(optionsdata: FSType = this.optionsdata) {
    this.obj = [
      {
        label: 'LCD Product Size',
        component: 'Select',
        field: 'Size',
        componentProps: (model) => {
          return {
            options: optionsdata.SizeData,
            placeholder: 'size',
            onChange: async (e: any) => {
              if (e !== 'ALL') {
                optionsdata.ProductData1 = optionsdata.ProductData!.filter(
                  (item) => item?.value.charAt(2).concat(item?.value.charAt(3)).indexOf(e) != -1,
                  // (item) => item?.value.indexOf(e) != -1
                );
              } else {
                optionsdata.ProductData1 = optionsdata.ProductData;
              }
            },
            onSelect: async (e: any) => {
              // 默認選中第一個
              // model.formModel.OptionProduct = optionsdata.ProductData1!.map((item) => item.value)[0];
              model.formModel.OptionProduct = [];
              model.formModel['#optionmenu'] = '';
            },
          };
        },
        defaultValue: 'ALL',
      },
      // BLU2選中的是label，而不是value
      {
        label: 'common.commonpages.Product',
        field: 'OptionProduct',
        component: 'Select',
        componentProps: (model) => {
          return {
            mode: 'multiple',
            listHeight: 300,
            options: optionsdata.ProductData1,
            placeholder: 'product',
            maxTagCount: 8,
            onSelect: async (e: any, option) => {
              model.formModel.Product = Array.from(
                new Set([...model.formModel.Product, option.label]),
              );
              optionsdata['tempoption'] = model.formModel.Product.map((item) => {
                return {
                  label: item,
                  value: item,
                };
              });
              model.formModel['#optionmenu'] = '';
            },
            onDeselect: (value, option) => {
              model.formModel.Product = model.formModel.Product.filter((item) => item != value);
              optionsdata['tempoption'] = model.formModel.Product.map((item) => {
                return {
                  label: item,
                  value: item,
                };
              });
              model.formModel['#optionmenu'] = '';
            },
          };
        },
        defaultValue: [],
      },
      {
        label: '',
        field: '#optionmenu',
        component: 'RadioGroup',
        componentProps: (model) => {
          return {
            options: [
              {
                label: '全選',
                value: 'selectALL',
              },
              {
                label: '取消全選',
                value: 'cancelselect',
              },
            ],
            onChange: (e) => {
              // 全選，全部添加
              if (e.target.value == 'selectALL') {
                // 上一個選項的所有值
                let temp = optionsdata.ProductData1!.map((item) => item.label);
                model.formModel.OptionProduct = temp;
                model.formModel.Product = [...new Set([...model.formModel.Product, ...temp])];
                optionsdata['tempoption'] = model.formModel.Product.map((item) => {
                  return {
                    label: item,
                    value: item,
                  };
                });
              } else if (e.target.value == 'cancelselect') {
                // 全消全選
                model.formModel.Product = model.formModel.Product.filter(
                  (item) => !model.formModel.OptionProduct.includes(item),
                );
                model.formModel.OptionProduct = [];
              }
            },
          };
        },
        defaultValue: '',
      },
      {
        label: '',
        field: 'Product',
        component: 'Select',
        componentProps: (model) => {
          return {
            mode: 'multiple',
            placeholder: 'product',
            options: optionsdata['tempoption'],
            maxTagCount: 8,
            // 取消選中，前面的也應該取消選中
            onDeselect: (value, option) => {
              // model.formModel.OptionProduct取值是value,要求獲取label
              // value是取消選中的value
              // 從optionsdata.ProductData1!根據value取到label
              let indexrow = optionsdata.ProductData1!.find((element) => element.label == value);
              if (indexrow) {
                model.formModel.OptionProduct = model.formModel.OptionProduct.filter((item) => {
                  return item != indexrow.value;
                });
              }
              if (model.formModel.Product.length == 0) {
                optionsdata['tempoption'] = [];
              }
            },
          };
        },
        defaultValue: [],
      },
    ];
    return this.obj;
  }
  // 返回obj
  getSize() {
    return this.obj;
  }
  // 更改label
  setLabel(index: number, LabelValue: string) {
    this.obj[index]['label'] = LabelValue;
    return this;
  }
}

export let optionsdata: any = reactive({
  // size的后端数据
  SizeData: [{ label: 'ALL', value: 'ALL' }],
  //默认的所有的数据
  ProductData1: [],
  ProductData2: [],
  // 未处理过的produc数据
  ProductData: [],
  OwnerName: [],
  LcdGradeLabel: [
    {
      label: 'A',
      value: 'A',
    },
    {
      label: 'B',
      value: 'B',
    },
    {
      label: 'Y',
      value: 'Y',
    },
    {
      label: 'C',
      value: 'C',
    },
    {
      label: 'D',
      value: 'D',
    },
    {
      label: 'E',
      value: 'E',
    },
    {
      label: 'F',
      value: 'F',
    },
    {
      label: 'K',
      value: 'K',
    },
    {
      label: 'L',
      value: 'L',
    },
  ],
});

export const schemas = (FactoryType: string) => {
  let optiondata2 = reactive({});

  return [
    {
      label: 'common.commonpages.Date',
      field: 'vDate_e',
      component: 'DatePicker',
      componentProps: {
        valueFormat: 'YYYY-MM-DD',
      },
      defaultValue: formatToDate(),
      required: true,
    },
    {
      label: 'History:',
      component: 'RadioGroup',
      field: 'show_his',
      componentProps: () => {
        return {
          options: [
            {
              label: 'No',
              value: 'N',
            },
            {
              label: 'Yes',
              value: 'Y',
              disabled: true,
            },
          ],
        };
      },
      defaultValue: 'N',
    },
    {
      label: 'STK:',
      component: 'Select',
      field: 'STK',
      componentProps: () => {
        return {
          options: [
            {
              label: 'ALL',
              value: '',
            },
            {
              label: 'STK01',
              value: 'STKL01',
            },
            {
              label: 'STK02',
              value: 'STKL02',
            },
            {
              label: 'STK03',
              value: 'STKL03',
            },
            {
              label: 'STK04',
              value: 'STKL04',
            },
            {
              label: 'STK05',
              value: 'STKL05',
            },
          ],
        };
      },
      defaultValue: '',
    },
    ...new SpecialSize(optionsdata).getSize(),
    ...new Owner(optiondata2, FactoryType, 'owner_code').setLabel(0, 'LCD Owner').obj,
    {
      label: 'common.commonpages.LCDGrade',
      component: 'Select',
      field: 'grade_code',
      componentProps: (model) => {
        return {
          mode: 'multiple',
          options: [
            {
              label: 'A',
              value: 'A',
            },
            {
              label: 'B',
              value: 'B',
            },
            {
              label: 'C',
              value: 'C',
            },
            {
              label: 'D',
              value: 'D',
            },
            {
              label: 'E',
              value: 'E',
            },
            {
              label: 'F',
              value: 'F',
            },
            {
              label: 'K',
              value: 'K',
            },
            {
              label: 'L',
              value: 'L',
            },
            {
              label: 'Y',
              value: 'Y',
            },
          ],
          dropdownMatchSelectWidth: 150,
        };
      },
    },
  ];
};
