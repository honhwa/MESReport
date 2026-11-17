import type { FormSchema } from '@/components/core/schema-form/';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil';
import { Size, FSType, Ifield, Owner ,SpecialSize} from '@/pages/ReportPortal/Static/common';
import { GetCommon } from '@/api/pages/common';
export async function getSizeData(params1: String, params2: string, funcparams: String, FactoryType: String) {
    let ReturnData: any[] = [];
    let {
        Date: {
            Data: { Table: Data }
        }
    } = await GetCommon({
        G_FAC: 'A',
        name: 'Date',
        apiName: funcparams,
        FactoryType: FactoryType
    });
    if (FactoryType == 'BLU1' && funcparams == 'BigProduct') {
        for (let i in Data) {
            //@ts-ignore
            ReturnData.push({
                //@ts-ignore
                label: `${Data[i][params1]}(${String(Data[i][params2])})`,
                //@ts-ignore
                value: `${String(Data[i][params2])}${Data[i][params1]}`
            });
        }
    } else {
        for (let i in Data) {
            //@ts-ignore
            ReturnData.push({ label: Data[i][params1], value: Data[i][params2] });
        }
    }
    ReturnData.unshift({ label: 'ALL', value: 'ALL' });
    return ReturnData;
}

// BLU1的size是特殊情況
const mountedFSSize = async (optionsdata, FactoryType) => {
    let temp;
    optionsdata.SizeData = await getSizeData('SIZE1', 'SIZE1', 'Size', FactoryType);
    if (FactoryType == 'BLU1') {
        optionsdata.BigProductData1 = optionsdata.BigProductData = await getSizeData(
            'PART',
            'SIZE1',
            'BigProduct',
            FactoryType
        );
        temp = await getSizeData('PART_NBR', 'SOPTION', 'Product', FactoryType);
    } else {
        optionsdata.BigProductData1 = optionsdata.BigProductData = await getSizeData(
            'PART',
            'PART',
            'BigProduct',
            FactoryType
        );
        temp = await getSizeData('SOPTION', 'SOPTION', 'Product', FactoryType);
    }

    // 刪除bigprod的ALL選項
    temp.shift();
    optionsdata.ProductData1 = optionsdata.ProductData = temp;
    return optionsdata;
};
export class SpecialSize2 {
    optionsdata: FSType;
    FactoryType: string;
    obj: FormSchema[];
    fieldobj: Ifield;
    constructor(optionsdata: FSType, FactoryType: string,fieldobj: Ifield = {
        size: 'Size',
        bigproduct: 'BigProduct',
        optionproduct: 'OptionProduct',
        selectmenu: '#optionmenu',
        product: 'Product'
    }) {
        this.optionsdata = optionsdata;
        this.FactoryType = FactoryType;
        this.fieldobj = fieldobj;
        optionsdata['FactoryType'] = this.FactoryType;
        optionsdata['tempoption'] = [];
        this.init(optionsdata, this.FactoryType);
        this.obj = this.setobj();
    }
    // api請求數據
    async init(optionsdata: FSType, FactoryType: string) {
        await mountedFSSize(optionsdata, FactoryType);
    }
    // 初始化obj
    setobj(optionsdata: FSType = this.optionsdata) {
        this.obj = [
            {
                label: 'common.commonpages.size',
                component: 'Select',
                // field: 'Size',
                field: this.fieldobj.size,
                componentProps: (model) => {
                    return {
                        options: optionsdata.SizeData,
                        placeholder: 'size',
                        onChange: async (e: any) => {
                            if (e !== 'ALL') {
                                optionsdata.BigProductData1 = optionsdata.BigProductData!.filter(
                                    (item) => item?.value.indexOf(e) != -1
                                );
                            } else {
                                optionsdata.BigProductData1 = optionsdata.BigProductData;
                            }
                        },
                        onSelect: async (e: any) => {
                            model.formModel[this.fieldobj.bigproduct] = optionsdata.BigProductData1!.map((item) => item.value)[0];
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
                                    (item) => item?.value.indexOf(e) != -1
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
                        onSelect: async (e, option) => {
                            // Blu2廠別特殊，取的值是label的值
                            if (this.FactoryType == 'BLU1') {
                                model.formModel[this.fieldobj.product] = Array.from(
                                    new Set([...model.formModel[this.fieldobj.product], option.label])
                                );
                            } else {
                                model.formModel[this.fieldobj.product] = Array.from(new Set([...model.formModel[this.fieldobj.product], e]));
                            }
                            optionsdata['tempoption'] = model.formModel[this.fieldobj.product].map((item) => {
                                return {
                                    label: item,
                                    value: item
                                };
                            });
                            model.formModel[this.fieldobj.selectmenu] = '';
                        },
                        // 取消選中，后面的也應該取消選中
                        onDeselect: (value, option) => {
                            if (this.FactoryType == 'BLU1') {
                                model.formModel[this.fieldobj.product] = model.formModel[this.fieldobj.product].filter(
                                    (item) => item != option.label
                                );
                            } else {
                                model.formModel[this.fieldobj.product] = model.formModel[this.fieldobj.product].filter((item) => item != value);
                            }
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
                                let temp;
                                if (this.FactoryType == 'BLU1') {
                                    temp = optionsdata.ProductData1!.map((item) => item.label);
                                } else {
                                    temp = optionsdata.ProductData1!.map((item) => item.value);
                                }
                                model.formModel[this.fieldobj.optionproduct] = temp;
                                model.formModel[this.fieldobj.product] = [...new Set([...model.formModel[this.fieldobj.product], ...temp])];
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
                        // 取消選中，前面的也應該取消選中
                        onDeselect: (value, option) => {
                            if (this.FactoryType == 'BLU1') {
                                model.formModel[this.fieldobj.optionproduct] = model.formModel[this.fieldobj.optionproduct].filter(
                                    (item) =>
                                        item != optionsdata.ProductData1?.find((item) => item.label == value).value
                                );
                            } else {
                                model.formModel[this.fieldobj.optionproduct] = model.formModel[this.fieldobj.optionproduct].filter(
                                    (item) => item != value
                                );
                            }
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
function handlesize(FactoryType: string) {
    if (['BLU1'].includes(FactoryType)) {
        return new SpecialSize(optionsdata, FactoryType).getSize();
    } else {
        // BLU2
        return new Size(optionsdata, FactoryType).getSize();
    }
}

export let optionsdata: any = reactive({
    SizeData: [{ label: 'ALL', value: 'ALL' }],
    ProductData: [],
    BigProductData: [],
    ProductData1: [],
    BigProductData1: [],
    OwnerName: []
});
export const schemas = (FactoryType: string) => {
    let optiondata2 = reactive({});
    return [
        {
            label: 'common.commonpages.Date',
            field: 'DateFrom',
            component: 'DatePicker',
            // helpMessage: ['TreeSelect组件', '使用接口提供的数据生成选项'],
            componentProps: {
                valueFormat: 'YYYY-MM-DD'
            },
            defaultValue: formatToDate(),
            required: true
        },
        {
            label: 'common.commonpages.To',
            field: 'DateTo',
            component: 'DatePicker',
            componentProps: {
                valueFormat: 'YYYY-MM-DD'
            },
            defaultValue: formatToDate(),
            required: true
        },
        {
            label: 'common.commonpages.Shift',
            component: 'RadioGroup',
            field: 'Shift',
            componentProps: {
                options: [
                    {
                        label: 'ALL',
                        value: 'ALL'
                    },
                    {
                        label: '日(A)',
                        value: '日'
                    },
                    {
                        label: '夜(B)',
                        value: '夜'
                    }
                ]
            },
            defaultValue: 'ALL'
        },
        {
            label: 'common.commonpages.Floor',
            component: 'RadioGroup',
            field: 'Floor',
            componentProps: {
                options: [
                    {
                        label: 'ALL',
                        value: 'ALL'
                    },
                    {
                        label: '2F',
                        value: '2'
                    },
                    {
                        label: '3F',
                        value: '3'
                    }
                ]
            },
            defaultValue: 'ALL'
        },
        ...handlesize(FactoryType),
        ...new Owner(optiondata2,FactoryType,"LcmOwner").setLabel(0,"Owner").obj,
       /*  {
            label: 'common.commonpages.LCMOwner',
            component: 'Select',
            field: 'LcmOwner',
            componentProps: () => {
                return {
                    mode: 'multiple',
                    options: optionsdata.OwnerName
                };
            }
        } */
    ];
};
