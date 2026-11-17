import type { FormSchema } from '@/components/core/schema-form/';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil';
import { LCMProductType, Owner, Size, FSType, Ifield } from '@/pages/ReportPortal/Static/common';
import { GetCommon } from '@/api/pages/common';
// import { SpecialSize } from '../../OperationPerformance/BLU/form-schema';

/**
 * 取出小括号内的内容
 * @param text
 * @returns {string}
 */
function getindex(text: string) {
    let re = /\([^\)]+\)/g;
    text = text.match(re)![0];
    text = text.substring(1, text.length - 1);
    return text;
}
// 新版獲取product方法
async function getproduct(optiondata: Object, FactoryType: string) {
    let { api01 } = await GetCommon({
        G_FAC: 'A',
        apiName: 'PanelWipStayselect_prod',
        FactoryType: FactoryType
    });
    let {
        SizeData: { Table: SizeData },
        BigproductData: { Table: BigproductData },
        ProductData: { Table: ProductData }
    } = api01;
    console.log(SizeData);
    console.log(BigproductData);
    console.log(ProductData);
    optiondata['SizeData'] = SizeData.map((item) => {
        return { label: item.SIZE1, value: item.SIZE1 };
    });
    optiondata['SizeData'].unshift({ label: 'ALL', value: 'ALL' });
    optiondata['BigProductData1'] = optiondata['BigProductData'] = BigproductData.map((item) => {
        return {
            label: `${item.PART}(${item.SIZE1})`,
            value: `${item.PART}(${item.SIZE1})`
        };
    });
    // optiondata['BigProductData1'].unshift({ label: 'ALL', value: 'ALL' });
    optiondata['ProductData1'] = optiondata['ProductData'] = ProductData.map((item) => {
        return {
            // 如果SOPTION有-那就加size
            // 如果沒有，就不加size
            label:item.SOPTION.includes("-") ? `${item.SOPTION}${item.SIZE1.split('.')[0]}` : item.SOPTION,
            value: item.SOPTION.includes("-") ? `${item.SOPTION}${item.SIZE1.split('.')[0]}` : item.SOPTION,
        };
    });
}
export class SpecialSize2 {
    optionsdata: FSType;
    FactoryType: string;
    obj: FormSchema[];
    fieldobj: Ifield;
    constructor(
        optionsdata: FSType,
        FactoryType: string,
        fieldobj: Ifield = {
            size: 'Size',
            bigproduct: 'BigProduct',
            optionproduct: 'OptionProduct',
            selectmenu: '#optionmenu',
            product: 'Product'
        }
    ) {
        this.optionsdata = optionsdata;
        this.FactoryType = FactoryType;
        this.fieldobj = fieldobj;
        optionsdata['FactoryType'] = this.FactoryType;
        optionsdata['tempoption'] = [];
        this.init(optionsdata, this.FactoryType);
        console.log('size的特殊處理是optionsdata', optionsdata);

        this.obj = this.setobj();
    }
    // api請求數據
    async init(optionsdata: FSType, FactoryType: string) {
        await getproduct(optionsdata, FactoryType);
    }
    // 初始化obj
    setobj(optionsdata: FSType = this.optionsdata) {
        this.obj = [
            {
                label: 'common.commonpages.size',
                component: 'Select',
                field: this.fieldobj.size,
                componentProps: (model) => {
                    return {
                        options: optionsdata.SizeData,
                        placeholder: 'size',
                        onChange: async (e: any) => {
                            if (e !== 'ALL') {
                                optionsdata.BigProductData1 = optionsdata.BigProductData!.filter(
                                    (item) => getindex(item?.value).indexOf(e) != -1
                                );
                            } else {
                                optionsdata.BigProductData1 = optionsdata.BigProductData;
                            }
                        },
                        onSelect: async (e: any) => {
                            model.formModel[this.fieldobj.bigproduct] = optionsdata.BigProductData1!.map(
                                (item) => item.value
                            )[0];
                        }
                    };
                },
                defaultValue: 'ALL'
            },
            {
                label: 'common.commonpages.bigproduct',
                component: 'Select',
                field: this.fieldobj.bigproduct,
                componentProps: (model) => {
                    return {
                        options: optionsdata.BigProductData1,
                        placeholder: 'big product',
                        maxTagCount: 8,
                        onSelect: async (e: any) => {
                            if (e !== 'ALL') {
                                optionsdata.ProductData1 = optionsdata.ProductData!.filter(
                                    (item) => item?.value.startsWith(e.split('(')[0])
                                );
                            } else {
                                optionsdata.ProductData1 = optionsdata.ProductData;
                            }
                            model.formModel[this.fieldobj.optionproduct] = [];
                            model.formModel[this.fieldobj.selectmenu] = '';
                        }
                    };
                },
                defaultValue: 'ALL'
            },
            {
                label: 'common.commonpages.Product',
                field: this.fieldobj.optionproduct,
                component: 'Select',
                componentProps: (model) => {
                    return {
                        mode: 'multiple',
                        listHeight: 300,
                        options: optionsdata.ProductData1,
                        placeholder: 'product',
                        maxTagCount: 8,
                        dropdownMatchSelectWidth: 150,
                        onSelect: async (e, option) => {
                            // Blu2廠別特殊，取的值是label的值
                            model.formModel[this.fieldobj.product] = Array.from(
                                new Set([...model.formModel[this.fieldobj.product], option.label])
                            );
                            optionsdata['tempoption'] = model.formModel[this.fieldobj.product].map((item) => {
                                return {
                                    label: item,
                                    value: item
                                };
                            });
                            model.formModel[this.fieldobj.selectmenu] = '';
                        },
                        onDeselect: (value, option) => {
                            model.formModel[this.fieldobj.product] = model.formModel[this.fieldobj.product].filter(
                                (item) => item != value
                            );
                            optionsdata['tempoption'] = model.formModel[this.fieldobj.product].map((item) => {
                                return {
                                    label: item,
                                    value: item
                                };
                            });
                            model.formModel[this.fieldobj.selectmenu] = '';
                        }
                    };
                },
                defaultValue: []
            },
            {
                label: '',
                field: this.fieldobj.selectmenu,
                component: 'RadioGroup',
                componentProps: (model) => {
                    return {
                        options: [
                            {
                                label: '全選',
                                value: 'selectALL'
                            },
                            {
                                label: '取消全選',
                                value: 'cancelselect'
                            }
                        ],
                        onChange: (e) => {
                            // 全選，全部添加
                            if (e.target.value == 'selectALL') {
                                // 上一個選項的所有值
                                let temp = optionsdata.ProductData1!.map((item) => item.label);
                                model.formModel[this.fieldobj.optionproduct] = temp;
                                model.formModel[this.fieldobj.product] = [
                                    ...new Set([...model.formModel[this.fieldobj.product], ...temp])
                                ];
                                optionsdata['tempoption'] = model.formModel[this.fieldobj.product].map((item) => {
                                    return {
                                        label: item,
                                        value: item
                                    };
                                });
                            } else if (e.target.value == 'cancelselect') {
                                // 全消全選
                                model.formModel[this.fieldobj.product] = model.formModel[this.fieldobj.product].filter(
                                    (item) => !model.formModel[this.fieldobj.optionproduct].includes(item)
                                );
                                model.formModel[this.fieldobj.optionproduct] = [];
                            }
                        }
                    };
                },
                defaultValue: ''
            },
            {
                label: '',
                field: this.fieldobj.product,
                component: 'Select',
                componentProps: (model) => {
                    return {
                        mode: 'multiple',
                        placeholder: 'product',
                        options: optionsdata['tempoption'],
                        maxTagCount: 8,
                        dropdownMatchSelectWidth: 150,
                        // 取消選中，前面的也應該取消選中
                        onDeselect: (value, option) => {
                            model.formModel[this.fieldobj.optionproduct] = model.formModel[
                                this.fieldobj.optionproduct
                            ].filter((item) => item != value);
                        }
                    };
                },
                defaultValue: []
            }
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
        OwnerName: []
    });

    return [
        {
            field: 'category',
            component: 'RadioGroup',
            label: '查詢類型',
            componentProps: {
                options: [
                    {
                        label: 'WIP停留時間',
                        value: '1'
                    },
                    {
                        label: 'BANK停留時間',
                        value: '2'
                    }
                ]
            },
            defaultValue: '1'
        },
        {
            field: 'trans_type',
            component: 'CheckboxGroup',
            label: 'Trans Type ',
            componentProps: {
                options: [
                    {
                        label: 'ALL',
                        value: 'ALL'
                    },
                    {
                        label: 'BL出貨',
                        value: 'SHIP'
                    },
                    {
                        label: 'RMA出貨',
                        value: 'RMAS'
                    },
                    {
                        label: 'ASSY耗用',
                        value: 'KITT'
                    },
                    {
                        label: '待修品',
                        value: 'BLNG'
                    },
                    {
                        label: '成品庫存',
                        value: 'WAIT'
                    }
                ]
            },
            defaultValue: ['ALL']
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
            label: 'Reworked',
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
        /*  ...new SpecialSize2(optionsdata, FactoryType, {
            size: 'Size',
            bigproduct: 'prod_type',
            optionproduct: 'OptionProduct',
            selectmenu: '#optionmenu',
            product: 'EntyGroup'
        }).getSize(), */
        /*  ...new Size(optionsdata, FactoryType, {
            size: 'Size',
            bigproduct: 'prod_type',
            optionproduct: 'OptionProduct',
            selectmenu: '#optionmenu',
            product: 'EntyGroup'
        }).obj, */
        ...new SpecialSize2(optionsdata, FactoryType, {
            size: 'Size',
            bigproduct: 'bigproduct',
            optionproduct: 'OptionProduct',
            selectmenu: '#optionmenu',
            product: 'EntyGroup'
        }).obj,

        {
            label: 'PROD ID',
            component: 'InputTextArea',
            field: 'pord'
        },
        ...new Owner(optionsdata, FactoryType, 'owner_code').obj

        /* {
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
    ];
};
