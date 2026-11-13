import type { FormSchema } from '@/components/core/schema-form/';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil';
import { FSType, LCMProductType, Owner, Size } from '@/pages/ReportPortal/Static/common';
import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect';
const { mountedcommonLCMGrade } = useRemoteSelect();
export let optionsdata: any = reactive({
    SizeData: [{ label: 'ALL', value: 'ALL' }],
    ProductData: [],
    BigProductData: [],
    ProductData1: [],
    BigProductData1: [],
    LCMOwner: [],
    OperationLabel: [],
    LCDSizeData: [{ label: 'ALL', value: 'ALL' }],
    LCDProductData: [],
    LCDProductData1: [],
    LCMProductSize: [],
    LCMProductType: [],
    DefectCode: [],
    LcmProdTypeData: [],
    LCMGradeData: [],
    OwnerName: [],
    errorcode: []
});
// 特殊情況
class Grade {
    _obj: FormSchema[];
    optionsdata: FSType;
    FactoryType: string;
    constructor(optionsdata: FSType, FactoryType: string, fieldname: string = 'LCDGrade') {
        this.optionsdata = optionsdata;
        this.FactoryType = FactoryType;
        optionsdata['FactoryType'] = this.FactoryType;
        this.init(optionsdata, this.FactoryType);
        this._obj = [
            {
                label: 'common.commonpages.LCDGrade',
                component: 'Select',
                // field: 'LCDGrade',
                field: fieldname,
                componentProps: () => {
                    return {
                        mode: 'multiple',
                        options: optionsdata.LCMGradeData
                    };
                }
            }
        ];
    }
    // api請求數據
    async init(optionsdata: FSType, FactoryType: string) {
        await mountedcommonLCMGrade(optionsdata, FactoryType);
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
let ReworkedOptions: any[] = [
    { label: 'All', value: '' },
    { label: '1次', value: '1' },
    { label: '1次以上', value: 'over_1' },
    { label: '2次', value: '2' },
    { label: '2次以上', value: 'over_2' },
    { label: '3次', value: '3' },
    { label: 'NO', value: 'N' }
];
let WotypeOptions = ['ALL', 'P', 'E', 'R', 'M'];
export const schemas = (FactoryType: string) => {
    let optionsdata: any = reactive({
        SizeData: [{ label: 'ALL', value: 'ALL' }],
        ProductData: [],
        BigProductData: [],
        ProductData1: [],
        BigProductData1: [],
        LCMOwner: [],
        OperationLabel: [],
        LCDSizeData: [{ label: 'ALL', value: 'ALL' }],
        LCDProductData: [],
        LCDProductData1: [],
        LCMProductSize: [],
        LCMProductType: [],
        DefectCode: [],
        LcmProdTypeData: [],
        LCMGradeData: [],
        OwnerName: [],
        errorcode: []
    });
    return [
        {
            field: 'category',
            component: 'RadioGroup',
            label: '查詢類型',
            componentProps: {
                options: [
                    {
                        label: '站點停留時間',
                        value: '1'
                    },
                    {
                        label: 'FAB停留時間',
                        value: '2'
                    },
                    {
                        label: 'SIZE',
                        value: '3'
                    }
                ]
            },
            defaultValue: '1'
        },
        {
            field: 'floor',
            component: 'RadioGroup',
            label: 'Floor',
            componentProps: {
                options: [
                    {
                        label: 'All',
                        value: ''
                    },
                    {
                        label: '1F',
                        value: "and floor='1'"
                    },
                    {
                        label: '2F',
                        value: "and floor='2'"
                    },
                    {
                        label: '3F',
                        value: "and floor='3'"
                    }
                ]
            },
            defaultValue: ''
        },
        {
            field: 'hold',
            component: 'RadioGroup',
            label: '是否HOLD品',
            componentProps: {
                options: [
                    {
                        label: 'ALL',
                        value: '1'
                    },
                    {
                        label: '是',
                        value: '2'
                    },
                    {
                        label: '否',
                        value: '3'
                    }
                ]
            },
            defaultValue: '1'
        },
        {
            field: 'qcck',
            component: 'RadioGroup',
            label: '是否OQC抽檢',
            componentProps: {
                options: [
                    {
                        label: 'ALL',
                        value: '1'
                    },
                    {
                        label: '是',
                        value: '2'
                    },
                    {
                        label: '否',
                        value: '3'
                    }
                ]
            },
            defaultValue: '1'
        },
        {
            label: 'rework',
            component: 'Select',
            field: 'rework',
            componentProps: {
                options: ReworkedOptions
            },
            defaultValue: ''
        },
        {
            label: 'Work Order Like',
            component: 'Input',
            field: 'workorder'
        },
        {
            field: 'wotype',
            component: 'CheckboxGroup',
            label: '工單類型',
            componentProps: {
                options: WotypeOptions.map((item) => {
                    return { label: item, value: item };
                })
            },
            defaultValue: ['ALL']
        },
        {
            field: 'isWoDueDay',
            component: 'RadioGroup',
            label: '選擇工單Due Day',
            componentProps: {
                options: [
                    {
                        label: '否',
                        value: 'N'
                    },
                    {
                        label: '是',
                        value: 'Y'
                    }
                ]
            },
            defaultValue: 'N'
        },
        {
            field: 'vDate_s_wodueday',
            component: 'DatePicker',
            label: '工單Due day',
            componentProps: {
                valueFormat: 'YYYY-MM-DD'
            },
            defaultValue: formatToDate(),
            dynamicDisabled: ({ formModel }) => formModel.isWoDueDay == 'N'
        },
        {
            field: 'vDate_e_wodueday',
            component: 'DatePicker',
            label: 'To',
            componentProps: {
                valueFormat: 'YYYY-MM-DD'
            },
            defaultValue: formatToDate(),
            dynamicDisabled: ({ formModel }) => formModel.isWoDueDay == 'N'
        },
        ...new LCMProductType(optionsdata, FactoryType, 'prod_type').obj,

        /* {
      label: 'LCM Product Type',
      component: 'Select',
      field: 'ProdType',
      componentProps: () => {
        return {
          options: optionsdata.LcmProdTypeData,
        };
      },
      defaultValue:"ALL"
  
    }, */
        /*  {
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
      }
    },
    {
      label: 'big product',
      component: 'Select',
      field: 'prod_type',
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
      field: 'EntyGroup',
      component: 'Select',
      componentProps: () => {
        return {
          mode: 'multiple',
          options: optionsdata.ProductData1,
          placeholder: 'product',
        };
      },
      defaultValue: ['ALL']
    }, */
        ...new Size(optionsdata, FactoryType,{
            size: 'Size',
            bigproduct: 'bigproduct',
            optionproduct: 'OptionProduct',
            selectmenu: '#optionmenu',
            product: 'EntyGroup'
        }).getSize(),
        {
            label: 'PROD ID',
            component: 'InputTextArea',
            field: 'pord'
        },
        ...new Owner(optionsdata, FactoryType, 'owner_code').obj,
        /*  {
      label: 'LCM Owner',
      component: 'Select',
      field: 'owner_code',
      componentProps: () => {
        return {
          mode: 'multiple',
          options: optionsdata.OwnerName,
        };
      },
      defaultValue: ["ALL"],
    }, */
        ...new Grade(optionsdata, FactoryType, 'lcm_grade').obj
        /* {
      label: 'LCM Grade',
      component: 'Select',
      field: 'lcm_grade',
      componentProps: () => {
        return {
          mode: 'multiple',
          options: optionsdata.LCMGradeData,
        }
      },
    }, */
    ];
};
