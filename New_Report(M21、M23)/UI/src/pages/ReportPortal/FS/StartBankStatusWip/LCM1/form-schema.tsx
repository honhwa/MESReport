import type { FormSchema } from '@/components/core/schema-form/';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil';
import { FSType,  Grade,  LCDOwner} from '@/pages/ReportPortal/Static/common';

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
                label: 'common.commonpages.size',
                component: 'Select',
                field: 'Size',
                componentProps: (model) => {
                    return {
                        options: optionsdata.SizeData,
                        placeholder: 'size',
                        onChange: async (e: any) => {
                            if (e !== 'ALL') {
                                optionsdata.ProductData1 = optionsdata.ProductData!.filter(
                                    // (item) => item?.value.indexOf(e) != -1
                                    (item) => item?.value.charAt(2).concat(item?.value.charAt(3)).indexOf(e) != -1

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
                        }
                    };
                },
                defaultValue: 'ALL'
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
                            model.formModel.Product = Array.from(new Set([...model.formModel.Product, option.label]));
                            optionsdata['tempoption'] = model.formModel.Product.map((item) => {
                                return {
                                    label: item,
                                    value: item
                                };
                            });
                            model.formModel['#optionmenu'] = '';
                        },
                        onDeselect: (value, option) => {
                            model.formModel.Product = model.formModel.Product.filter((item) => item != value);
                            optionsdata['tempoption'] = model.formModel.Product.map((item) => {
                                return {
                                    label: item,
                                    value: item
                                };
                            });
                            model.formModel['#optionmenu'] = '';
                        }
                    };
                },
                defaultValue: []
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
                                model.formModel.OptionProduct = temp;
                                model.formModel.Product = [...new Set([...model.formModel.Product, ...temp])];
                                optionsdata['tempoption'] = model.formModel.Product.map((item) => {
                                    return {
                                        label: item,
                                        value: item
                                    };
                                });
                            } else if (e.target.value == 'cancelselect') {
                                // 全消全選
                                model.formModel.Product = model.formModel.Product.filter(
                                    (item) => !model.formModel.OptionProduct.includes(item)
                                );
                                model.formModel.OptionProduct = [];
                            }
                        }
                    };
                },
                defaultValue: ''
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

export let optionsdata: any = reactive({
    // size的后端数据
    SizeData: [{ label: 'ALL', value: 'ALL' }],
    //默认的所有的数据
    ProductData1: [],
    ProductData2: [],
    // 未处理过的produc数据
    ProductData: [],
    LcdGradeLabel: [],
    OwnerName: []
});

export const schemas = (FactoryType:string)=>{
    let optiondata = reactive({});
    return [
        {
            label: 'common.commonpages.Date',
            field: 'vDate_e',
            component: 'DatePicker',
            componentProps: {
                valueFormat: 'YYYY-MM-DD'
            },
            defaultValue: formatToDate(),
            required: true
        },
        {
            label: 'common.commonpages.DisplaySite2',
            labelWidth: 180,
            component: 'RadioGroup',
            field: 'show_lcd',
            componentProps: {
                options: [
                    {
                        label: 'No',
                        value: 'N'
                    },
                    {
                        label: 'Yes',
                        value: 'Y'
                    }
                ]
            },
            defaultValue: 'N'
        },
        {
            label: 'common.commonpages.DisplayAbnormalCode',
            labelWidth: 180,
            component: 'RadioGroup',
            field: 'show_abn',
            componentProps: {
                options: [
                    {
                        label: 'No',
                        value: 'N'
                    },
                    {
                        label: 'Yes',
                        value: 'Y'
                    }
                ]
            },
            defaultValue: 'N'
        },
        ...new SpecialSize(optionsdata).getSize(),
        ...new LCDOwner(optiondata,FactoryType,"owner_code").obj,
        ...new Grade(optiondata,FactoryType,"grade_code").obj,
    ];
}
