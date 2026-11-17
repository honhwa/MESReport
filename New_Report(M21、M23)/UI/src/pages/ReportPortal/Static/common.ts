import { reactive } from 'vue';
import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect';
import { AddCommonPageParamsWithOut } from '@/store/module/commonpage';
import { GetCommon } from '@/api/pages/common';
import type { FormSchema } from '@/components/core/schema-form/';
import { formatToDate } from '@/utils/dateUtil';
import { callbackify } from 'util';
const {
    mountedLcmProdType,
    mountedowner,
    mountedExpectDefectCode,
    mountedLCDGrade,
    mountedLCMGrade,
    mountedcommonLCMGrade,
    mountedOper,
    mountedEquip,
    mountedLCMProductSize,
    mountedDefectCode,
    mountedLCDProduct,
    mountedHoldCode,
    mountedLCMowner,
    mountedRMAFabType,
    mountedRMA3Date,
    mountedRMAMatType,
    mountedRMAProductType,
    mountedRMAModelType,
    mountedRMAPartType,
    mountedRMAPart3Type,
    mounteEDCLineid,
    mounteEDCUnit,
    mountedMaterial,
} = useRemoteSelect();
export function getFactoryType(optionsdata, FactoryType: string) {
    optionsdata['FactoryType'] = FactoryType;
}
// operaction的BLU1厰和BLue2厰是特殊情況
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
    for (let i in Data) {
        //@ts-ignore
        ReturnData.push({ label: Data[i][params1], value: Data[i][params2] });
    }
    ReturnData.unshift({ label: 'ALL', value: 'ALL' });
    return ReturnData;
}
export async function getCategoryData(params1: String, params2: string, funcparams: String, FactoryType: String) {
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
    for (let i in Data) {
        //@ts-ignore
        ReturnData.push({ label: Data[i][params1], value: Data[i][params2] });
    }
    ReturnData.unshift({ label: 'ALL', value: 'ALL' });
    return ReturnData;
}
// 其餘廠別使用
const mountedFSSize = async (optionsdata, FactoryType) => {
    optionsdata.SizeData = await getSizeData('SIZE1', 'SIZE1', 'Size', FactoryType);
    optionsdata.BigProductData1 = optionsdata.BigProductData = await getSizeData(
        'PART',
        'PART',
        'BigProduct',
        FactoryType
    );
    let temp = await getSizeData('SOPTION', 'SOPTION', 'Product', FactoryType);
    // 刪除bigprod的ALL選項
    temp.shift();
    optionsdata.ProductData1 = optionsdata.ProductData = temp;
    return optionsdata;
};
// BLU2使用
const mountedFSSize2 = async (optionsdata, FactoryType) => {
    optionsdata.SizeData = await getSizeData('SIZE1', 'SIZE1', 'Size', FactoryType);
    let temp = await getSizeData('PART_NBR', 'SOPTION', 'Product', FactoryType);
    // 刪除bigprod的ALL選項
    temp.shift();
    optionsdata.ProductData1 = optionsdata.ProductData = temp;
    return optionsdata;
};
const mountedFSCategory = async (optionsdata, FactoryType) => {
    optionsdata.Category = await getCategoryData('SIZE1', 'SIZE1', 'Category_OQCDefectGroupSetup', FactoryType);
    let temp = await getCategoryData('PART', 'PART', 'OQCDfctGroupSet_Defectcode', FactoryType);
    // 刪除bigprod的ALL選項
    temp.shift();
    optionsdata.defectcode1 = optionsdata.defectcode = temp;

    return optionsdata;
};

export interface FSType {
    SizeData?: any[];
    BigProductData1?: any[];
    BigProductData?: any[];
    ProductData1?: any[];
    ProductData?: any[];
    BL_sizeData?: any[];
    PART_NBRData?: any[];
    PART_NBRALLData?: any[];
    PART_NBRdefaultData?: any[];
    OwnerName?: any[];
    LCMProductSizeData?: any[];
    LcmProdTypeData?: any[];
    RMA3DateData?: any[];
    RMAFabTypeData?: any[];
    RMAProductTypeData?: any[];
    RMAMatTypeData?: any[];
    RMAModelTypeData?: any[];
    RMAPartTypeData?: any[];
    RMAPart3TypeData?: any[];
    EDCLineidData?: any[];
    EDCUnitData?: any[];
    OperationLabel?: any[];
    LcdGradeLabel?: any[];
    EquipmentData?: any[];
    BigEquipmentData?: any[];
    BigEquipmentData1?: any[];
    DefectCodeData?: any[];
    LCDProductData?: any[];
    BigLCDProductData?: any[];
    BigLCDProductData1?: any[];
    ExpectDefectCodeData?: any[];
    HoldCodeData?: any[];
    LCMGradeData?: any[];
    Category?: any[];
    defectcode?: any[];
    defectcode1?: any[];
    Marterial?: any[];
}
export type Ifield = {
    size: string;
    bigproduct: string;
    optionproduct: string;
    selectmenu: string;
    product: string;
};
export type ICategory = {
    category: string;
    optiondefectcode: string;
    selectmenu: string;
    defectcode: string;
};
export type IEquipment = {
    equip: string;
    Optionequip: string;
    equipselectmenu: string;
    equipment: string;
};
// 獲取廠別,size,product,bigproduct
export class Size {
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
                                // LCM的情况特殊，GV500DK1S截取的是BigProductData1.substring(2,5) == "500"这一种，其余厂别，indexof就好了
                                if(this.FactoryType == "LCM1"){
                                    optionsdata.BigProductData1 = optionsdata.BigProductData!.filter(item=>{
                                        return item.value.substring(2,5) == e;
                                    })
                                }
                                else{
                                    optionsdata.BigProductData1 = optionsdata.BigProductData!.filter(
                                        (item) => item?.value.indexOf(e) != -1
                                    );
                                }
                            } else {
                                optionsdata.BigProductData1 = optionsdata.BigProductData;
                            }
                        },
                        onSelect: async (e: any) => {
                            // model.formModel.BigProduct = optionsdata.BigProductData1!.map((item) => item.value)[0];
                            model.formModel[this.fieldobj.bigproduct] = optionsdata.BigProductData1!.map(
                                (item) => item.value
                            )[0];
                        }
                    };
                },
                defaultValue: 'ALL',
                LabelStyle: { color: 'blue' }
            },
            {
                label: 'common.commonpages.bigproduct',
                component: 'Select',
                // field: 'BigProduct',
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
                defaultValue: 'ALL',
                LabelStyle: { color: 'blue' }
            },
            {
                label: 'common.commonpages.Product',
                // field: 'OptionProduct',
                field: this.fieldobj.optionproduct,
                component: 'Select',
                componentProps: (model) => {
                    return {
                        mode: 'multiple',
                        // defaultOpen:true,
                        listHeight: 300,
                        options: optionsdata.ProductData1,
                        placeholder: 'product',
                        maxTagCount: 8,
                        dropdownMatchSelectWidth: 150,
                        onSelect: async (e, option) => {
                            // Blu2廠別特殊，取的值是label的值
                            if (this.FactoryType == 'BLU2') {
                                model.formModel[this.fieldobj.product] = Array.from(
                                    new Set([...model.formModel[this.fieldobj.product], option.label])
                                );
                            } else {
                                model.formModel[this.fieldobj.product] = Array.from(
                                    new Set([...model.formModel[this.fieldobj.product], e])
                                );
                            }
                            optionsdata['tempoption'] = model.formModel[this.fieldobj.product].map((item) => {
                                return {
                                    label: item,
                                    value: item
                                };
                            });
                            model.formModel[this.fieldobj.selectmenu] = '';
                            model.formModel[this.fieldobj.product] = model.formModel[this.fieldobj.product].filter((item) => item != 'ALL');
                        },
                        // 取消選中，后面的也應該取消選中
                        onDeselect: (value, option) => {
                            if (this.FactoryType == 'BLU2') {
                                model.formModel[this.fieldobj.product] = model.formModel[this.fieldobj.product].filter(
                                    (item) => item != option.label
                                );
                            } else {
                                model.formModel[this.fieldobj.product] = model.formModel[this.fieldobj.product].filter(
                                    (item) => item != value
                                );
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
                defaultValue: ["ALL"],
                LabelStyle: { color: 'blue' }
            },
            {
                label: '',
                // field: '#optionmenu',
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
                                if (this.FactoryType == 'BLU2') {
                                    temp = optionsdata.ProductData1!.map((item) => item.label);
                                } else {
                                    temp = optionsdata.ProductData1!.map((item) => item.value);
                                }
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
                                model.formModel[this.fieldobj.product] = model.formModel[this.fieldobj.product].filter((item) => item != 'ALL');
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
                // field: 'Product',
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
                            if (this.FactoryType == 'BLU2') {
                                model.formModel[this.fieldobj.optionproduct] = model.formModel[
                                    this.fieldobj.optionproduct
                                ].filter(
                                    (item) =>
                                        item != optionsdata.ProductData1?.find((item) => item.label == value).value
                                );
                            } else {
                                model.formModel[this.fieldobj.optionproduct] = model.formModel[
                                    this.fieldobj.optionproduct
                                ].filter((item) => item != value);
                            }
                        }
                    };
                },
                defaultValue: ["ALL"],

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
export class BLUSize {
    optionsdata: FSType;
    FactoryType: string;
    obj: FormSchema[];
    fieldobj: Ifield;
    constructor(
        optionsdata: FSType,
        FactoryType: string,
        fieldobj: Ifield = {
            size: 'BLUSize',
            bigproduct: 'BLUBigProduct',
            optionproduct: 'BLUOptionProduct',
            selectmenu: '#BLUoptionmenu',
            product: 'BLUProduct'
        }
    ) {
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
        let {api01}  = await GetCommon({
            G_FAC: 'A',
            apiName: 'PanelBluReportWorkCtr',
            FactoryType: FactoryType
        });
        optionsdata.SizeData = api01['SizeData']['Table'].map(item=>{
            return {label:item['SIZE1'],value:item['SIZE1']}
        })
        optionsdata.BigProductData =optionsdata.BigProductData1 = api01['BigProductData']['Table'].map(item=>{
            return {label:item['PART'],value:item['PART']}
        })
       
        optionsdata.ProductData1=optionsdata.ProductData = api01['ProductData']['Table'].map(item=>{
            return {label:item['SOPTION'],value:item['SOPTION']}
        })

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
                            // model.formModel.BigProduct = optionsdata.BigProductData1!.map((item) => item.value)[0];
                            model.formModel[this.fieldobj.bigproduct] = optionsdata.BigProductData1!.map(
                                (item) => item.value
                            )[0];
                        }
                    };
                },
                defaultValue: 'ALL',
                LabelStyle: { color: 'blue' }
            },
            {
                label: 'common.commonpages.bigproduct',
                component: 'Select',
                // field: 'BigProduct',
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
                defaultValue: 'ALL',
                LabelStyle: { color: 'blue' }
            },
            {
                label: 'common.commonpages.Product',
                // field: 'OptionProduct',
                field: this.fieldobj.optionproduct,
                component: 'Select',
                componentProps: (model) => {
                    return {
                        mode: 'multiple',
                        // defaultOpen:true,
                        listHeight: 300,
                        options: optionsdata.ProductData1,
                        placeholder: 'product',
                        maxTagCount: 8,
                        dropdownMatchSelectWidth: 150,
                        onSelect: async (e, option) => {
                            model.formModel[this.fieldobj.product] = Array.from(
                                new Set([...model.formModel[this.fieldobj.product], e])
                            );
                            optionsdata['tempoption'] = model.formModel[this.fieldobj.product].map((item) => {
                                return {
                                    label: item,
                                    value: item
                                };
                            });
                            model.formModel[this.fieldobj.selectmenu] = '';
                            model.formModel[this.fieldobj.product] = model.formModel[this.fieldobj.product].filter((item) => item != 'ALL');
                        },
                        // 取消選中，后面的也應該取消選中
                        onDeselect: (value, option) => {
                            if (this.FactoryType == 'BLU2') {
                                model.formModel[this.fieldobj.product] = model.formModel[this.fieldobj.product].filter(
                                    (item) => item != option.label
                                );
                            } else {
                                model.formModel[this.fieldobj.product] = model.formModel[this.fieldobj.product].filter(
                                    (item) => item != value
                                );
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
                defaultValue: ["4503M000QV10V"],
                LabelStyle: { color: 'blue' }
            },
            {
                label: '',
                // field: '#optionmenu',
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

                                temp = optionsdata.ProductData1!.map((item) => item.value);
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
                                model.formModel[this.fieldobj.product] = model.formModel[this.fieldobj.product].filter((item) => item != 'ALL');
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
                defaultValue: '4503M000QV10V'
            },
            {
                label: '',
                // field: 'Product',
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
                            if (this.FactoryType == 'BLU2') {
                                model.formModel[this.fieldobj.optionproduct] = model.formModel[
                                    this.fieldobj.optionproduct
                                ].filter(
                                    (item) =>
                                        item != optionsdata.ProductData1?.find((item) => item.label == value).value
                                );
                            } else {
                                model.formModel[this.fieldobj.optionproduct] = model.formModel[
                                    this.fieldobj.optionproduct
                                ].filter((item) => item != value);
                            }
                        }
                    };
                },
                defaultValue: ["4503M000QV10V"],

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
// 獲取廠別,size,product
export class Size2 {
    optionsdata: FSType;
    FactoryType: string;
    obj: FormSchema[];
    fieldobj: Omit<Ifield, 'bigproduct'>;
    /**
     * 
     * @param optionsdata 
     * @param FactoryType 
     * @param fieldobj 組件綁定的參數為發給後端的參數{
            size: 'Size',
            optionproduct: 'OptionProduct',
            selectmenu: '#optionmenu',
            product: 'Product'
        }
     */
    constructor(
        optionsdata: FSType,
        FactoryType: string,
        fieldobj: Omit<Ifield, 'bigproduct'> = {
            size: 'Size',
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
        this.obj = this.setobj();
    }
    // api請求數據
    async init(optionsdata: FSType, FactoryType: string) {
        await mountedFSSize2(optionsdata, FactoryType);
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
                                if (this.FactoryType == 'BLU1') {
                                    optionsdata.ProductData1 = optionsdata.ProductData!.filter(
                                        (item) => item?.value.slice(0, 4).indexOf(e) != -1
                                    );
                                } else {
                                    optionsdata.ProductData1 = optionsdata.ProductData!.filter(
                                        (item) => item?.value.indexOf(e) != -1
                                    );
                                }
                            } else {
                                optionsdata.ProductData1 = optionsdata.ProductData;
                            }
                        },
                        onSelect: async (e: any) => {
                            // 默認選中第一個
                            // model.formModel.OptionProduct = optionsdata.ProductData1!.map((item) => item.value)[0];
                            model.formModel[this.fieldobj.optionproduct] = [];
                            model.formModel[this.fieldobj.selectmenu] = '';
                        }
                    };
                },
                defaultValue: 'ALL',
                LabelStyle: { color: 'blue' }
            },
            // BLU2選中的是label，而不是value
            {
                label: 'common.commonpages.Product',
                // field: 'OptionProduct',
                field: this.fieldobj.optionproduct,
                component: 'Select',
                componentProps: (model) => {
                    return {
                        listHeight: 300,
                        options: optionsdata.ProductData1,
                        placeholder: 'product',
                        maxTagCount: 8,
                        dropdownMatchSelectWidth: 150,
                        onSelect: async (e: any, option) => {
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
                            model.formModel[this.fieldobj.product] = model.formModel[this.fieldobj.product].filter((item) => item != 'ALL');
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
                defaultValue: [],
                LabelStyle: { color: 'blue' }
            },
            {
                label: '',
                // field: '#optionmenu',
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
                                model.formModel[this.fieldobj.product] = model.formModel[this.fieldobj.product].filter((item) => item != 'ALL');
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
                            // model.formModel.OptionProduct取值是value,要求獲取label
                            // value是取消選中的value
                            // 從optionsdata.ProductData1!根據value取到label
                            let indexrow = optionsdata.ProductData1!.find((element) => element.label == value);
                            if (indexrow) {
                                model.formModel[this.fieldobj.optionproduct] = model.formModel[
                                    this.fieldobj.optionproduct
                                ].filter((item) => {
                                    return item != indexrow.value;
                                });
                            }
                            if (model.formModel[this.fieldobj.product].length == 0) {
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

// 特殊的size，在start bank start wip和(MA205) LCD廠PANEL於lcm廠投產分佈狀況 中使用
export class Size3 {
    optionsdata: FSType;
    obj: FormSchema[];
    fieldobj: Omit<Ifield, 'bigproduct'>;
    /**
     *
     * @param optionsdata
     * @param FactoryType
     * @param fieldobj  size: 'Size',optionproduct: 'OptionProduct',selectmenu: '#optionmenu',product: 'Product'
     */
    constructor(
        optionsdata: FSType,
        FactoryType: string,
        fieldobj: Omit<Ifield, 'bigproduct'> = {
            size: 'Size',
            optionproduct: 'OptionProduct',
            selectmenu: '#optionmenu',
            product: 'Product'
        }
    ) {
        this.fieldobj = fieldobj;
        this.init(optionsdata, FactoryType);
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
                // field: 'Size',
                field: this.fieldobj.size,
                componentProps: (model) => {
                    return {
                        options: optionsdata.SizeData,
                        placeholder: 'size',
                        onChange: async (e: any) => {
                            if (e !== 'ALL') {
                                optionsdata.ProductData1 = optionsdata.ProductData!.filter(
                                    (item) => item?.value.charAt(2).concat(item?.value.charAt(3)).indexOf(e) != -1
                                    // (item) => item?.value.indexOf(e) != -1
                                );
                            } else {
                                optionsdata.ProductData1 = optionsdata.ProductData;
                            }
                        },
                        onSelect: async (e: any) => {
                            // 默認選中第一個
                            // model.formModel.OptionProduct = optionsdata.ProductData1!.map((item) => item.value)[0];
                            model.formModel[this.fieldobj.optionproduct] = [];
                            model.formModel[this.fieldobj.selectmenu] = '';
                        }
                    };
                },
                defaultValue: 'ALL',
                LabelStyle: { color: 'blue' }
            },
            // BLU2選中的是label，而不是value
            {
                label: 'common.commonpages.Product',
                // field: 'OptionProduct',
                field: this.fieldobj.optionproduct,
                component: 'Select',
                componentProps: (model) => {
                    return {
                        mode: 'multiple',
                        listHeight: 300,
                        options: optionsdata.ProductData1,
                        placeholder: 'product',
                        maxTagCount: 8,
                        onSelect: async (e: any, option) => {
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
                            model.formModel[this.fieldobj.product] = model.formModel[this.fieldobj.product].filter((item) => item != 'ALL');

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
                defaultValue: [],
                LabelStyle: { color: 'blue' }
            },
            {
                label: '',
                // field: '#optionmenu',
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
                                model.formModel[this.fieldobj.product] = model.formModel[this.fieldobj.product].filter((item) => item != 'ALL');

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
                // field: 'Product',
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
                            // model.formModel.OptionProduct取值是value,要求獲取label
                            // value是取消選中的value
                            // 從optionsdata.ProductData1!根據value取到label
                            let indexrow = optionsdata.ProductData1!.find((element) => element.label == value);
                            if (indexrow) {
                                model.formModel[this.fieldobj.optionproduct] = model.formModel[
                                    this.fieldobj.optionproduct
                                ].filter((item) => {
                                    return item != indexrow.value;
                                });
                            }
                            if (model.formModel[this.fieldobj.product].length == 0) {
                                optionsdata['tempoption'] = [];
                            }
                        }
                    };
                },
                defaultValue: [],
                LabelStyle: { color: 'blue' }
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
    async init(optionsdata: any, FactoryType: string) {
        const {
            Date: {
                Data: { Table: Data }
            }
        } = await GetCommon({
            name: 'Date',
            apiName: 'StartBankStatusWip_Size',
            G_FAC: 'A',
            FactoryType: FactoryType
        });
        let SizeData: object[] = [];
        Data.forEach((item) => {
            SizeData.push({
                label: item['SIZE1'],
                value: item['SIZE1']
            });
        });
        SizeData.unshift({
            label: 'ALL',
            value: 'ALL'
        });
        optionsdata.SizeData = SizeData;
        let {
            Date: { Table: ProductData }
        } = await GetCommon({
            name: 'Date',
            apiName: 'StartBankStatusWip_Product',
            G_FAC: 'A',
            Size: '',
            FactoryType: FactoryType
        });
        let ALLproduct: object[] = [];
        ProductData.forEach((item) => {
            ALLproduct.push({
                label: item['PART'],
                value: item['PART']
            });
        });
        optionsdata.ProductData1 = optionsdata.ProductData = ALLproduct;
    }
}
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
            label: item.SOPTION.includes("-") ? `${item.SOPTION}${item.SIZE1.split('.')[0]}` : item.SOPTION,
            value: item.SOPTION.includes("-") ? `${item.SOPTION}${item.SIZE1.split('.')[0]}` : item.SOPTION,
        };
    });
}
// operaction performance的blu1和panel wip stay chat的blu1中使用
// size  26  bigproduct 46-D0(26)這總形式
export class SpecialSize {
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
// 獲取Date
export class Date {
    _obj: FormSchema[];
    field: {
        starttime: string
        endtime: string
    };
    constructor(field: {
        starttime: string
        endtime: string
    } = {
            starttime: "FromDate",
            endtime: "ToDate"
        }) {
        this.field = field;
        this._obj = [
            {
                label: 'common.commonpages.Date',
                field: this.field.starttime,
                component: 'DatePicker',
                componentProps: {
                    valueFormat: 'YYYY-MM-DD'
                },
                defaultValue: formatToDate(),
                required: true
            },
            {
                label: 'common.commonpages.To',
                field: this.field.endtime,
                component: 'DatePicker',
                componentProps: {
                    valueFormat: 'YYYY-MM-DD'
                },
                defaultValue: formatToDate(),
                required: true
            }
        ];
    }
    // 更改label
    setLabel(index: number, LabelValue: string) {
        this._obj[index]['label'] = LabelValue;
        return this;
    }
    // 獲取Date
    get obj() {
        return this._obj;
    }
}

// 獲取Marterial
async function getMarterial(optionsdata: object, FactoryType: string) {
    // 獲取BL_size:
    let BL_SizeData: object[] = [];
    let {
        api01: {
            Data: { Table: Data = null }
        }
    } = await GetCommon({
        G_FAC: 'A',
        apiName: 'BL_Size',
        FactoryType: FactoryType
    });
    Data.forEach((item) => {
        BL_SizeData.push({
            label: item['BL_SIZE'],
            value: item['BL_SIZE']
        });
    });
    BL_SizeData.unshift({
        label: 'ALL',
        value: 'ALL'
    });
    optionsdata['BL_sizeData'] = BL_SizeData;
    // 獲取PART_NBR:
    let PART_NBRshowData: object[] = [];
    let {
        api01: {
            Data: { Table: Data1 = null }
        }
    } = await GetCommon({
        G_FAC: 'A',
        apiName: 'BL_PART_NBR',
        FactoryType: FactoryType
    });
    Data1.forEach((item) => {
        PART_NBRshowData.push({
            label: item['PART_NBR'],
            value: item['PART_NBR']
        });
    });
    optionsdata['PART_NBRData'] = PART_NBRshowData;
    optionsdata['PART_NBRdefaultData'] = PART_NBRshowData;
    optionsdata['PART_NBRALLData'] = Data1;
}
export class Marterial {
    _obj: FormSchema[];
    optionsdata: FSType;
    FactoryType: string;
    constructor(optionsdata: FSType, FactoryType?: string) {
        let UseParamsAdd = AddCommonPageParamsWithOut();
        let routerFactoryType = UseParamsAdd.Fab;
        this.optionsdata = optionsdata;
        this.FactoryType = FactoryType ?? routerFactoryType;
        optionsdata['FactoryType'] = this.FactoryType;
        optionsdata['MarterialOption'] = [];
        this.init(optionsdata, this.FactoryType);
        this._obj = [
            {
                label: 'common.commonpages.Marterial',
                component: 'Divider',
                field: '#Marterial'
            },
            {
                label: 'common.commonpages.BL_Size',
                component: 'Select',
                field: 'BL_Size',
                componentProps: (model) => {
                    return {
                        options: this.optionsdata.BL_sizeData,
                        onChange: async (e: any) => {
                            if (e != 'ALL') {
                                let temp: object[] = [];
                                this.optionsdata.PART_NBRALLData!.forEach((item) => {
                                    if (item['BL_SIZE'] === e) {
                                        temp.push({
                                            label: item['PART_NBR'],
                                            value: item['PART_NBR']
                                        });
                                    }
                                });
                                this.optionsdata.PART_NBRData = temp;
                            } else {
                                this.optionsdata.PART_NBRData = this.optionsdata.PART_NBRdefaultData;
                            }
                        },
                        onSelect: () => {
                            model.formModel.MarterialPARTNBR = [];
                            model.formModel['#optionMarterial'] = '';
                        }
                    };
                },
                defaultValue: 'ALL'
            },
            {
                label: 'PART_NBR:',
                component: 'Select',
                field: 'MarterialPARTNBR',
                componentProps: (model) => {
                    return {
                        mode: 'multiple',
                        options: this.optionsdata.PART_NBRData,
                        maxTagCount: 8,
                        onSelect: (e: any, option) => {
                            // console.log("輸出e和option",e,option);

                            model.formModel.MarterialProduct = [...model.formModel.MarterialProduct, e];
                            this.optionsdata['MarterialOption'] = [...this.optionsdata['MarterialOption'], option];
                        }
                    };
                },
                defaultValue: []
            },
            {
                label: '',
                field: '#optionMarterial',
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
                                let temp = this.optionsdata.PART_NBRData!.map((item) => item.value);
                                model.formModel.MarterialPARTNBR = temp;
                                model.formModel.MarterialProduct = [
                                    ...new Set([...model.formModel.MarterialProduct, ...temp])
                                ];
                                this.optionsdata['MarterialOption'] = model.formModel.MarterialProduct.map((item) => {
                                    return {
                                        label: item,
                                        value: item
                                    };
                                });
                            } else if (e.target.value == 'cancelselect') {
                                // 全消全選
                                model.formModel.MarterialProduct = model.formModel.MarterialProduct.filter(
                                    (item) => !model.formModel.MarterialPARTNBR.includes(item)
                                );
                                model.formModel.MarterialPARTNBR = [];
                            }
                        }
                    };
                },
                defaultValue: ''
            },
            {
                label: '',
                field: 'MarterialProduct',
                component: 'Select',
                componentProps: (model) => {
                    return {
                        mode: 'multiple',
                        placeholder: 'product',
                        options: this.optionsdata['MarterialOption'],
                        maxTagCount: 8,
                        // 取消選中，前面的也應該取消選中
                        onDeselect: (value, option) => {
                            model.formModel.MarterialPARTNBR = model.formModel.MarterialPARTNBR.filter(
                                (item) => item != value
                            );
                            if (model.formModel.MarterialProduct.length == 0) {
                                this.optionsdata['MarterialOption'] = [];
                            }
                        }
                    };
                },
                defaultValue: []
            }
        ];
    }
    // api請求數據
    async init(optionsdata: FSType, FactoryType: string) {
        await getMarterial(optionsdata, FactoryType);
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
// 獲取owner,其實是LCM Owner，apiname使用的是owner
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
                        }
                    };
                },
                defaultValue: ['ALL']
            }
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
// 獲取LCD owner，apiname使用的是LCMowner,後端寫錯了
export class LCDOwner {
    _obj: FormSchema[];
    optionsdata: FSType;
    FactoryType: string;
    constructor(optionsdata: FSType, FactoryType: string, Field: string = 'LCDOwner') {
        this.optionsdata = optionsdata;
        this.FactoryType = FactoryType;
        optionsdata['FactoryType'] = this.FactoryType;
        optionsdata['MarterialOption'] = [];
        this.init(optionsdata, this.FactoryType);
        this._obj = [
            {
                label: 'common.commonpages.LCDOwner',
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
                        }
                    };
                },
                defaultValue: ['ALL']
            }
        ];
    }
    // api請求數據
    async init(optionsdata: FSType, FactoryType: string) {
        await mountedLCMowner(optionsdata, FactoryType);
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
// 獲取shift
export class Shift {
    _obj: FormSchema[];
    /**
     * 
     * @param type 
     * @param fieldname 
     */
    constructor(type: number, fieldname: string = 'Shift') {
        this._obj = this.Typehandle(type, fieldname);
    }
    Typehandle(type: number, fieldname: string) {
        let obj: FormSchema[] = [];
        switch (type) {
            case 1:
                obj = [
                    {
                        label: 'common.commonpages.Shift',
                        component: 'RadioGroup',
                        field: fieldname,
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
                            ]
                        },
                        defaultValue: ''
                    }
                ];
                break;
            case 2:
                obj = [
                    {
                        label: 'common.commonpages.Shift',
                        component: 'RadioGroup',
                        field: fieldname,
                        componentProps: {
                            options: [
                                {
                                    label: 'ALL',
                                    value: ''
                                },
                                {
                                    label: '日(A)',
                                    value: 'A'
                                },
                                {
                                    label: '夜(B)',
                                    value: 'B'
                                }
                            ]
                        },
                        defaultValue: ''
                    }
                ];
                break;
            default:
                break;
        }
        return obj;
    }
    // 獲取shift
    get obj() {
        return this._obj;
    }
}
// 獲取Floor

export class Floor {
    _obj: FormSchema[];
    /**
     * 
     * @param type 樓層類型，1-2
     * @param fieldname  發給後端的名字
     */
    constructor(type: number, fieldname: string = 'Floor') {
        this._obj = this.Typehandle(type, fieldname);
    }
    Typehandle(type: number, fieldname: string) {
        let obj: FormSchema[] = [];
        switch (type) {
            case 1:
                obj = [
                    {
                        label: 'common.commonpages.Floor',
                        component: 'RadioGroup',
                        field: fieldname,
                        componentProps: {
                            options: [
                                {
                                    label: 'ALL',
                                    value: ''
                                },
                                {
                                    label: '1F',
                                    value: " and floor='1'"
                                },
                                {
                                    label: '2F',
                                    value: " and floor='2'"
                                },
                                {
                                    label: '3F',
                                    value: " and floor='3'"
                                }
                            ]
                        },
                        defaultValue: ''
                    }
                ];
                break;
            case 2:
                break;
            default:
                break;
        }
        return obj;
    }
    // 獲取shift
    get obj() {
        return this._obj;
    }
}

// 獲取LCM product size
export class LCMProductSize {
    _obj: FormSchema[];
    optionsdata: FSType;
    FactoryType: string;
    constructor(optionsdata: FSType, FactoryType: string, fieldname: string = 'LCMProductSize') {
        this.optionsdata = optionsdata;
        this.FactoryType = FactoryType;
        optionsdata['FactoryType'] = this.FactoryType;
        this.init(optionsdata, this.FactoryType);
        this._obj = [
            {
                label: 'common.commonpages.LCMProductSize',
                component: 'Select',
                field: fieldname,
                componentProps: (model) => {
                    return {
                        mode: 'multiple',
                        options: optionsdata.LCMProductSizeData,
                        onSelect: (e: any, option) => {
                            if (e != 'ALL') {
                                model.formModel[fieldname] = model.formModel[fieldname].filter((item) => item != 'ALL');
                            } else {
                                model.formModel[fieldname] = ['ALL'];
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
        await mountedLCMProductSize(optionsdata, FactoryType);
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

// 獲取LCM product type
export class LCMProductType {
    _obj: FormSchema[];
    optionsdata: FSType;
    FactoryType: string;
    constructor(optionsdata: FSType, FactoryType: string, fieldname: string = 'LCMProductType') {
        this.optionsdata = optionsdata;
        this.FactoryType = FactoryType;
        optionsdata['FactoryType'] = this.FactoryType;
        this.init(optionsdata, this.FactoryType);
        this._obj = [
            {
                label: 'common.commonpages.LCMProductType',
                component: 'Select',
                // field: 'LCMProductType',
                field: fieldname,
                componentProps: () => {
                    return {
                        options: optionsdata.LcmProdTypeData
                    };
                },
                defaultValue: 'ALL'
            }
        ];
    }
    // api請求數據
    async init(optionsdata: FSType, FactoryType: string) {
        await mountedLcmProdType(optionsdata, FactoryType);
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
//獲取RMA Mat type
export class RMAMatType {
    _obj: FormSchema[];
    optionsdata: FSType;
    FactoryType: string;
    constructor(optionsdata: FSType, FactoryType: string, Field: string = 'Mat') {
        this.optionsdata = optionsdata;
        this.FactoryType = FactoryType;
        optionsdata['FactoryType'] = this.FactoryType;
        optionsdata['MarterialOption'] = [];
        this.init(optionsdata, this.FactoryType);
        this._obj = [
            {
                label: '物料料號',
                component: 'Select',
                field: Field,
                componentProps: (model) => {
                    return {
                        mode: 'multiple',
                        options: optionsdata.RMAMatTypeData,
                        dropdownMatchSelectWidth: 150,
                        onSelect: (e: any, option) => {
                            if (e != 'ALL') {
                                model.formModel[Field] = model.formModel[Field].filter((item) => item != 'ALL');
                            } else {
                                model.formModel[Field] = ['ALL'];
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
        await mountedRMAMatType(optionsdata, FactoryType);
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

//獲取RMA Product type
export class RMAProductType {
    _obj: FormSchema[];
    optionsdata: FSType;
    FactoryType: string;
    constructor(optionsdata: FSType, FactoryType: string, Field: string = 'Product') {
        this.optionsdata = optionsdata;
        this.FactoryType = FactoryType;
        optionsdata['FactoryType'] = this.FactoryType;
        optionsdata['MarterialOption'] = [];
        this.init(optionsdata, this.FactoryType);
        this._obj = [
            {
                label: 'PRODUCT',
                component: 'Select',
                field: Field,
                componentProps: (model) => {
                    return {
                        mode: 'multiple',
                        options: optionsdata.RMAProductTypeData,
                        dropdownMatchSelectWidth: 150,
                        onSelect: (e: any, option) => {
                            if (e != 'ALL') {
                                model.formModel[Field] = model.formModel[Field].filter((item) => item != 'ALL');
                            } else {
                                model.formModel[Field] = ['ALL'];
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
        await mountedRMAProductType(optionsdata, FactoryType);
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
//獲取RMA Model type
export class RMAModelType {
    _obj: FormSchema[];
    optionsdata: FSType;
    FactoryType: string;
    constructor(optionsdata: FSType, FactoryType: string, Field: string = 'Model') {
        this.optionsdata = optionsdata;
        this.FactoryType = FactoryType;
        optionsdata['FactoryType'] = this.FactoryType;
        optionsdata['MarterialOption'] = [];
        this.init(optionsdata, this.FactoryType);
        this._obj = [
            {
                label: 'MODEL',
                component: 'Select',
                field: Field,
                componentProps: (model) => {
                    return {
                        mode: 'multiple',
                        options: optionsdata.RMAModelTypeData,
                        dropdownMatchSelectWidth: 150,
                        onSelect: (e: any, option) => {
                            if (e != 'ALL') {
                                model.formModel[Field] = model.formModel[Field].filter((item) => item != 'ALL');
                            } else {
                                model.formModel[Field] = ['ALL'];
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
        await mountedRMAModelType(optionsdata, FactoryType);
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

// 獲取RMA3 Date
export class RMA3DateType {
    _obj: FormSchema[];
    optionsdata: FSType;
    FactoryType: string;
    constructor(optionsdata: FSType, FactoryType: string, fieldname: string = 'ACCT_DATE') {
        this.optionsdata = optionsdata;
        this.FactoryType = FactoryType;
        optionsdata['FactoryType'] = this.FactoryType;
        this.init(optionsdata, this.FactoryType);
        this._obj = [
            {
                label: '年月',
                component: 'Select',
                field: fieldname,
                componentProps: () => {
                    return {
                        options: optionsdata.RMA3DateData
                    };
                }
                // ,
                // defaultValue: 'ALL'
            }
        ];
    }
    // api請求數據
    async init(optionsdata: FSType, FactoryType: string) {
        await mountedRMA3Date(optionsdata, FactoryType);
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
// 獲取RMA Fab type
export class RMAFabType {
    _obj: FormSchema[];
    optionsdata: FSType;
    FactoryType: string;
    constructor(optionsdata: FSType, FactoryType: string, fieldname: string = 'FAB') {
        this.optionsdata = optionsdata;
        this.FactoryType = FactoryType;
        optionsdata['FactoryType'] = this.FactoryType;
        this.init(optionsdata, this.FactoryType);
        this._obj = [
            {
                label: '廠別',
                component: 'Select',
                field: fieldname,
                componentProps: () => {
                    return {
                        options: optionsdata.RMAFabTypeData
                    };
                },
                defaultValue: 'ALL'
            }
        ];
    }
    // api請求數據
    async init(optionsdata: FSType, FactoryType: string) {
        await mountedRMAFabType(optionsdata, FactoryType);
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
// 獲取RMA Part type
export class RMAPartType {
    _obj: FormSchema[];
    optionsdata: FSType;
    FactoryType: string;
    constructor(optionsdata: FSType, FactoryType: string, Field: string = 'PART') {
        this.optionsdata = optionsdata;
        this.FactoryType = FactoryType;
        optionsdata['FactoryType'] = this.FactoryType;
        optionsdata['MarterialOption'] = [];
        this.init(optionsdata, this.FactoryType);
        this._obj = [
            {
                label: 'PART分類',
                component: 'Select',
                field: Field,
                componentProps: (model) => {
                    return {
                        mode: 'multiple',
                        options: optionsdata.RMAPartTypeData,
                        dropdownMatchSelectWidth: 150,
                        onSelect: (e: any, option) => {
                            if (e != 'ALL') {
                                model.formModel[Field] = model.formModel[Field].filter((item) => item != 'ALL');
                            } else {
                                model.formModel[Field] = ['ALL'];
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
        await mountedRMAPartType(optionsdata, FactoryType);
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
// 獲取RMA Part3 type
export class RMAPart3Type {
    _obj: FormSchema[];
    optionsdata: FSType;
    FactoryType: string;
    constructor(optionsdata: FSType, FactoryType: string, Field: string = 'PART') {
        this.optionsdata = optionsdata;
        this.FactoryType = FactoryType;
        optionsdata['FactoryType'] = this.FactoryType;
        optionsdata['MarterialOption'] = [];
        this.init(optionsdata, this.FactoryType);
        this._obj = [
            {
                label: '比對PART分類',
                component: 'Select',
                field: Field,
                componentProps: (model) => {
                    return {
                        mode: 'multiple',
                        options: optionsdata.RMAPart3TypeData,
                        dropdownMatchSelectWidth: 150,
                        onSelect: (e: any, option) => {
                            if (e != 'ALL') {
                                model.formModel[Field] = model.formModel[Field].filter((item) => item != 'ALL');
                            } else {
                                model.formModel[Field] = ['ALL'];
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
        await mountedRMAPart3Type(optionsdata, FactoryType);
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
// 獲取EDCLine query
export class EDCLineid {
    _obj: FormSchema[];
    optionsdata: FSType;
    FactoryType: string;
    constructor(optionsdata: FSType, FactoryType: string, fieldname: string = 'EDCLineid') {
        this.optionsdata = optionsdata;
        this.FactoryType = FactoryType;
        optionsdata['FactoryType'] = this.FactoryType;
        this.init(optionsdata, this.FactoryType);
        this._obj = [
            {
                label: 'common.commonpages.EDCLineid',
                component: 'Select',
                // field: 'EDCLineid',
                field: fieldname,
                componentProps: () => {
                    return {
                        options: optionsdata.EDCLineidData
                    };
                },
                defaultValue: 'ALL'
            }
        ];
    }
    // api請求數據
    async init(optionsdata: FSType, FactoryType: string) {
        await mounteEDCLineid(optionsdata, FactoryType);
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

// 獲取EDCUnit query
export class EDCUnit {
    _obj: FormSchema[];
    optionsdata: FSType;
    FactoryType: string;
    constructor(optionsdata: FSType, FactoryType: string, fieldname: string = 'EDCUnit') {
        this.optionsdata = optionsdata;
        this.FactoryType = FactoryType;
        optionsdata['FactoryType'] = this.FactoryType;
        this.init(optionsdata, this.FactoryType);
        this._obj = [
            {
                label: 'common.commonpages.EDCUnit',
                component: 'Select',
                // field: 'EDCUnit',
                field: fieldname,
                componentProps: () => {
                    return {
                        options: optionsdata.EDCUnitData
                    };
                },
                defaultValue: 'ALL'
            }
        ];
    }
    // api請求數據
    async init(optionsdata: FSType, FactoryType: string) {
        await mounteEDCUnit(optionsdata, FactoryType);
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

// 獲取operaction
export class Operation {
    _obj: FormSchema[];
    optionsdata: FSType;
    FactoryType: string;
    defaultvalue: string[];
    constructor(optionsdata: FSType, FactoryType: string, field: string = 'Operation',callBack:Function = ()=>{}) {
        this.optionsdata = optionsdata;
        this.FactoryType = FactoryType;
        optionsdata['FactoryType'] = this.FactoryType;
        this.defaultvalue = ['ALL'];
        this.init(optionsdata, this.FactoryType,callBack);
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
    async init(optionsdata: FSType, FactoryType: string,callBack:Function) {
        await mountedOper(optionsdata, FactoryType);
        // 修改裏面的内容
        callBack(optionsdata,this.defaultvalue)
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
     // 更改默認值
     setDefault(index: number, defaultValue: string[]) {
        this._obj[index]['defaultValue'] = defaultValue;
        return this;
    }
}
// 獲取grade
// 使用接口mountedLCDGrade
export class Grade {
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
                        options: optionsdata.LcdGradeLabel
                    };
                }
            }
        ];
    }
    // api請求數據
    async init(optionsdata: FSType, FactoryType: string) {
        await mountedLCDGrade(optionsdata, FactoryType);
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
// 使用接口mountedLCMGrade
/**
 * fieldname: string = 'LCMGrade'
 */
export class LCMGrade {
    _obj: FormSchema[];
    optionsdata: FSType;
    FactoryType: string;
    constructor(optionsdata: FSType, FactoryType: string, fieldname: string = 'LCMGrade',optionfiled:string = "LcdGradeLabel") {
        this.optionsdata = optionsdata;
        this.FactoryType = FactoryType;
        optionsdata['FactoryType'] = this.FactoryType;
        this.init(optionsdata, this.FactoryType);
        this._obj = [
            {
                label: 'common.commonpages.LCMGrade',
                component: 'Select',
                field: fieldname,
                componentProps: () => {
                    return {
                        mode: 'multiple',
                        options: optionsdata[optionfiled]
                    };
                }
            }
        ];
    }
    // api請求數據
    async init(optionsdata: FSType, FactoryType: string) {
        await mountedLCMGrade(optionsdata, FactoryType);
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
// 使用接口mountedcommonLCMGrade
export class ConmonGrade {
    _obj: FormSchema[];
    optionsdata: FSType;
    FactoryType: string;
    constructor(optionsdata: FSType, FactoryType: string, fieldname: string = 'LCMGrade') {
        this.optionsdata = optionsdata;
        this.FactoryType = FactoryType;
        optionsdata['FactoryType'] = this.FactoryType;
        this.init(optionsdata, this.FactoryType);
        this._obj = [
            {
                label: 'common.commonpages.LCMGrade',
                component: 'Select',
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

// 獲取Equipment
export class Equipment {
    optionsdata: FSType;
    FactoryType: string;
    obj: FormSchema[];
    Type: number;
    fieldobj: IEquipment;
    /**
     *
     * @param optionsdata 下拉選擇框綁定的值
     * @param FactoryType 廠別
     * @param Type 0-3
     * @param fieldobj 字段動態名
     */
    constructor(
        optionsdata: FSType,
        FactoryType: string,
        Type: number,
        fieldobj: IEquipment = {
            equip: 'equip',
            Optionequip: 'Optionequip',
            equipselectmenu: '#optionequipmenu',
            equipment: 'Equipment'
        }
    ) {
        this.optionsdata = optionsdata;
        this.FactoryType = FactoryType;
        this.Type = Type;
        this.fieldobj = fieldobj;
        optionsdata['FactoryType'] = this.FactoryType;
        optionsdata['tempEquipment'] = [];
        this.init(optionsdata, this.FactoryType, Type);
        this.obj = this.setobj();
    }
    // api請求數據
    async init(optionsdata: FSType, FactoryType: string, Type: number) {
        await mountedEquip(optionsdata, FactoryType, Type);
    }
    // 初始化obj
    setobj(optionsdata: FSType = this.optionsdata) {
        this.obj = [
            {
                label: 'common.commonpages.Equipment',
                component: 'Select',
                // field: 'equip',
                field: this.fieldobj.equip,
                componentProps: (model) => {
                    return {
                        options: optionsdata.EquipmentData,
                        placeholder: 'size',
                        onChange: async (e: any) => {
                            if (e !== 'ALL') {
                                optionsdata.BigEquipmentData1 = optionsdata.BigEquipmentData!.filter(
                                    (item) => item?.value.indexOf(e) != -1
                                );
                            } else {
                                optionsdata.BigEquipmentData1 = optionsdata.BigEquipmentData;
                            }
                        },
                        onSelect: async (e: any) => {
                            // 默認選中第一個
                            // model.formModel.OptionProduct = optionsdata.ProductData1!.map((item) => item.value)[0];
                            model.formModel[this.fieldobj.Optionequip] = [];
                            model.formModel[this.fieldobj.equipselectmenu] = '';
                        }
                    };
                },
                defaultValue: 'ALL'
            },
            {
                label: '',
                // field: 'Optionequip',
                field: this.fieldobj.Optionequip,
                component: 'Select',
                componentProps: (model) => {
                    return {
                        mode: 'multiple',
                        listHeight: 300,
                        options: optionsdata.BigEquipmentData1,
                        placeholder: '',
                        maxTagCount: 8,
                        onSelect: async (e: any, option) => {
                            model.formModel[this.fieldobj.equipment] = [
                                ...model.formModel[this.fieldobj.equipment],
                                option.label
                            ];
                            optionsdata['tempEquipment'] = model.formModel[this.fieldobj.equipment].map((item) => {
                                return {
                                    label: item,
                                    value: item
                                };
                            });
                            model.formModel[this.fieldobj.equipselectmenu] = '';
                        }
                    };
                },
                defaultValue: []
            },
            {
                label: '',
                // field: '#optionequipmenu',
                field: this.fieldobj.equipselectmenu,
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
                                let temp = optionsdata.BigEquipmentData1!.map((item) => item.label);
                                model.formModel[this.fieldobj.Optionequip] = temp;
                                model.formModel[this.fieldobj.equipment] = [
                                    ...new Set([...model.formModel[this.fieldobj.equipment], ...temp])
                                ];
                                optionsdata['tempEquipment'] = model.formModel[this.fieldobj.equipment].map((item) => {
                                    return {
                                        label: item,
                                        value: item
                                    };
                                });
                            } else if (e.target.value == 'cancelselect') {
                                // 全消全選
                                model.formModel[this.fieldobj.equipment] = model.formModel[
                                    this.fieldobj.equipment
                                ].filter((item) => !model.formModel[this.fieldobj.Optionequip].includes(item));
                                model.formModel[this.fieldobj.Optionequip] = [];
                            }
                        }
                    };
                },
                defaultValue: ''
            },
            {
                label: '',
                // field: 'Equipment',
                field: this.fieldobj.equipment,
                component: 'Select',
                componentProps: (model) => {
                    return {
                        mode: 'multiple',
                        placeholder: 'Equipment',
                        options: optionsdata['tempEquipment'],
                        maxTagCount: 8,
                        // 取消選中，前面的也應該取消選中
                        onDeselect: (value, option) => {
                            let indexrow = optionsdata.BigEquipmentData1!.find((element) => element.label == value);
                            if (indexrow) {
                                model.formModel[this.fieldobj.Optionequip] = model.formModel[
                                    this.fieldobj.Optionequip
                                ].filter((item) => {
                                    return item != indexrow.value;
                                });
                            }
                            if (model.formModel[this.fieldobj.equipment].length == 0) {
                                optionsdata['tempEquipment'] = [];
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
    getEquipment() {
        return this.obj;
    }
    // 更改label
    setLabel(index: number, LabelValue: string) {
        this.obj[index]['label'] = LabelValue;
        return this;
    }
}

// 獲取defect code
export class DefectCode {
    _obj: FormSchema[];
    optionsdata: FSType;
    FactoryType: string;
    defaultvalue: string[];
    constructor(optionsdata: FSType, FactoryType: string, fieldname: string = 'DefectCode') {
        this.optionsdata = optionsdata;
        this.FactoryType = FactoryType;
        this.defaultvalue = ['1=1'];
        optionsdata['FactoryType'] = this.FactoryType;

        this.init(optionsdata, this.FactoryType);
        this._obj = [
            {
                label: 'common.commonpages.DefectCode',
                component: 'Select',
                field: fieldname,
                componentProps: (model) => {
                    return {
                        mode: 'multiple',
                        options: optionsdata.DefectCodeData,
                        maxTagCount: 8,
                        dropdownMatchSelectWidth: 150,
                        onSelect: (e: any, option) => {
                            if (e != 'ALL') {
                                model.formModel[fieldname] = model.formModel[fieldname].filter((item) => item != '1=1');
                            } else {
                                model.formModel[fieldname] = ['1=1'];
                            }
                        }
                    };
                },
                defaultValue: this.defaultvalue
            }
        ];
    }
    // api請求數據
    async init(optionsdata: FSType, FactoryType: string) {
        await mountedDefectCode(optionsdata, FactoryType);
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
    // 更改默認值
    setDefault(index: number, defaultValue: string[]) {
        this._obj[index]['defaultValue'] = defaultValue;
        return this;
    }
}
// 獲取LCD product size
export class LCDProductSize {
    optionsdata: FSType;
    FactoryType: string;
    obj: FormSchema[];
    constructor(optionsdata: FSType, FactoryType: string, Type: number) {
        let UseParamsAdd = AddCommonPageParamsWithOut();
        let routerFactoryType = UseParamsAdd.Fab;
        this.optionsdata = optionsdata;
        this.FactoryType = FactoryType ?? routerFactoryType;
        optionsdata['FactoryType'] = this.FactoryType;
        optionsdata['templcdproductoption'] = [];
        this.init(optionsdata, this.FactoryType, Type);
        this.obj = this.setobj();
    }
    // api請求數據
    async init(optionsdata: FSType, FactoryType: string, Type: number) {
        await mountedLCDProduct(optionsdata, FactoryType, Type);
    }
    // 初始化obj
    setobj(optionsdata: FSType = this.optionsdata) {
        this.obj = [
            {
                label: 'common.commonpages.size',
                component: 'Select',
                field: 'LCDProductSize',
                componentProps: (model) => {
                    return {
                        options: optionsdata.LCDProductData,
                        placeholder: 'size',
                        onChange: async (e: any) => {
                            if (e !== 'ALL') {
                                optionsdata.BigLCDProductData = optionsdata.BigLCDProductData1!.filter(
                                    (item) => item?.SIZE1.indexOf(e) != -1
                                ).map(item => {return {label:item.PART,value:item.PART}});
                            } else {
                                optionsdata.BigLCDProductData = optionsdata.BigLCDProductData1;
                            }
                        },
                        onSelect: async (e: any) => {
                            // 默認選中第一個
                            model.formModel.OptionLCDProduct = [];
                            model.formModel['#optionlcdproductmenu'] = '';
                        }
                    };
                },
                defaultValue: 'ALL'
            },
            {
                label: 'common.commonpages.Product',
                field: 'OptionLCDProduct',
                component: 'Select',
                componentProps: (model) => {
                    return {
                        mode: 'multiple',
                        listHeight: 300,
                        options: optionsdata.BigLCDProductData,
                        placeholder: 'product',
                        maxTagCount: 8,
                        onSelect: async (e: any, option) => {
                            model.formModel.LCDProduct = [...model.formModel.LCDProduct, option.label];
                            optionsdata['templcdproductoption'] = model.formModel.LCDProduct.map((item) => {
                                return {
                                    label: item,
                                    value: item
                                };
                            });
                            model.formModel['#optionlcdproductmenu'] = '';
                        }
                    };
                },
                defaultValue: []
            },
            {
                label: '',
                field: '#optionlcdproductmenu',
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
                                let temp = optionsdata.BigLCDProductData!.map((item) => item.label);
                                model.formModel.OptionLCDProduct = temp;
                                model.formModel.LCDProduct = [...new Set([...model.formModel.LCDProduct, ...temp])];
                                optionsdata['templcdproductoption'] = model.formModel.LCDProduct.map((item) => {
                                    return {
                                        label: item,
                                        value: item
                                    };
                                });
                            } else if (e.target.value == 'cancelselect') {
                                // 全消全選
                                model.formModel.LCDProduct = model.formModel.LCDProduct.filter(
                                    (item) => !model.formModel.OptionLCDProduct.includes(item)
                                );
                                model.formModel.OptionLCDProduct = [];
                            }
                        }
                    };
                },
                defaultValue: ''
            },
            {
                label: '',
                field: 'LCDProduct',
                component: 'Select',
                componentProps: (model) => {
                    return {
                        mode: 'multiple',
                        placeholder: 'product',
                        options: optionsdata['templcdproductoption'],
                        maxTagCount: 8,
                        // 取消選中，前面的也應該取消選中
                        onDeselect: (value, option) => {
                            // model.formModel.OptionProduct取值是value,要求獲取label
                            // value是取消選中的value
                            // 從optionsdata.ProductData1!根據value取到label
                            let indexrow = optionsdata.BigLCDProductData!.find((element) => element.label == value);
                            if (indexrow) {
                                model.formModel.OptionLCDProduct = model.formModel.OptionLCDProduct.filter((item) => {
                                    return item != indexrow.value;
                                });
                            }
                            if (model.formModel.LCDProduct.length == 0) {
                                optionsdata['templcdproductoption'] = [];
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

// 獲取Expect Defect Code
export class ExpectDefectCode{
    _obj: FormSchema[];
    optionsdata: FSType;
    FactoryType: string;
    defaultvalue: string[];
    constructor(optionsdata: FSType, FactoryType: string, fieldname: string = 'ExpectDefectCode') {
        this.optionsdata = optionsdata;
        this.FactoryType = FactoryType;
        this.defaultvalue = ['1==1'];
        optionsdata['FactoryType'] = this.FactoryType;
        this.init(optionsdata, this.FactoryType);
        this._obj = [
            {
                label: 'common.commonpages.ExpectDefectCode',
                component: 'Select',
                field: fieldname,
                componentProps: (model) => {
                    return {
                        mode: 'multiple',
                        options: optionsdata.ExpectDefectCodeData,
                        onSelect: (e: any, option) => {
                            if (e != 'ALL') {
                                model.formModel[fieldname] = model.formModel[fieldname].filter(
                                    (item) => item != '1==1'
                                );
                            } else {
                                model.formModel[fieldname] = ['1==1'];
                            }
                        }
                    };
                },
                defaultValue: this.defaultvalue
            }
        ];
    }
    // api請求數據
    async init(optionsdata: FSType, FactoryType: string) {
        await mountedExpectDefectCode(optionsdata, FactoryType);
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
    // 更改默認值
    setDefault(index: number, defaultValue: string[]) {
        this._obj[index]['defaultValue'] = defaultValue;
        return this;
    }
}
// 獲取hold code
export class HoldCode {
    _obj: FormSchema[];
    optionsdata: FSType;
    FactoryType: string;
    constructor(optionsdata: FSType, FactoryType: string, field: string = 'HoldCode') {
        this.optionsdata = optionsdata;
        this.FactoryType = FactoryType;
        optionsdata['FactoryType'] = this.FactoryType;
        this.init(optionsdata, this.FactoryType);
        this._obj = [
            {
                label: 'Hold Code',
                component: 'Select',
                field: field,
                componentProps: (model) => {
                    return {
                        mode: 'multiple',
                        options: optionsdata.HoldCodeData,
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
        await mountedHoldCode(optionsdata, FactoryType);
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
// 獲取時間格式為202206的時間
export class YearMonth {}

export class Category {
    optionsdata: FSType;
    FactoryType: string;
    obj: FormSchema[];
    fieldobj: Omit<Ifield, 'bigproduct'>;
    constructor(
        optionsdata: FSType,
        FactoryType: string,
        fieldobj: Omit<Ifield, 'bigproduct'> = {
            size: 'Category',
            optionproduct: 'OptionProduct',
            selectmenu: '#optionmenu',
            product: 'Defectcode'
        }
    ) {
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
        await mountedFSCategory(optionsdata, FactoryType);
    }
    // 初始化obj
    setobj(optionsdata: FSType = this.optionsdata) {
        this.obj = [
            {
                label: '品别:',
                component: 'Select',
                // field: 'Size',
                field: this.fieldobj.size,
                componentProps: (model) => {
                    return {
                        options: optionsdata.Category,
                        placeholder: 'size',
                        onChange: async (e: any) => {
                            if (e !== 'ALL') {

                                optionsdata.defectcode1 = optionsdata.defectcode!.filter(
                                    (item) => item?.value.substr(0, 1).indexOf(e) != -1
                                );
                            } else {
                                optionsdata.defectcode1 = optionsdata.defectcode;
                            }
                        },
                        /*  onSelect: async (e: any) => {
                            // 默認選中第一個
                            // model.formModel.OptionProduct = optionsdata.ProductData1!.map((item) => item.value)[0];
                            model.formModel.OptionProduct = [];
                            model.formModel['#optionmenu'] = '';
                        } */
                        onSelect: async (e: any) => {
                            // 默認選中第一個
                            // model.formModel.OptionProduct = optionsdata.ProductData1!.map((item) => item.value)[0];
                            model.formModel[this.fieldobj.optionproduct] = [];
                            model.formModel[this.fieldobj.selectmenu] = '';
                        }
                    };
                },
                defaultValue: 'ALL',
                LabelStyle: { color: 'blue' }
            },
            // BLU2選中的是label，而不是value
            {
                label: 'defectcode:',
                // field: 'OptionProduct',
                field: this.fieldobj.optionproduct,
                component: 'Select',
                componentProps: (model) => {
                    return {
                        mode: 'multiple',
                        listHeight: 300,
                        options: optionsdata.defectcode1,
                        placeholder: 'Defectcode',
                        maxTagCount: 8,
                        dropdownMatchSelectWidth: 150,
                        onSelect: async (e: any, option) => {
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
                defaultValue: [],
                LabelStyle: { color: 'blue' }
            },
            {
                label: '',
                // field: '#optionmenu',
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
                            /* if (e.target.value == 'selectALL') {
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
                            } */
                            if (e.target.value == 'selectALL') {
                                // 上一個選項的所有值
                                let temp = optionsdata.defectcode1!.map((item) => item.label);
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
                // field: 'Product',
                field: this.fieldobj.product,
                component: 'Select',
                componentProps: (model) => {
                    return {
                        mode: 'multiple',
                        placeholder: 'Defectcode',
                        options: optionsdata['tempoption'],
                        maxTagCount: 8,
                        dropdownMatchSelectWidth: 150,
                        // 取消選中，前面的也應該取消選中
                        /* onDeselect: (value, option) => {
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
                        } */
                        onDeselect: (value, option) => {
                            // model.formModel.OptionProduct取值是value,要求獲取label
                            // value是取消選中的value
                            // 從optionsdata.ProductData1!根據value取到label
                            let indexrow = optionsdata.defectcode1!.find((element) => element.label == value);
                            if (indexrow) {
                                model.formModel[this.fieldobj.optionproduct] = model.formModel[
                                    this.fieldobj.optionproduct
                                ].filter((item) => {
                                    return item != indexrow.value;
                                });
                            }
                            if (model.formModel[this.fieldobj.product].length == 0) {
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


export class Material1 {
  _obj: FormSchema[];
  optionsdata: FSType;
  FactoryType: string;
  constructor(optionsdata: FSType, FactoryType: string, field: string = 'MaterialType') {
      this.optionsdata = optionsdata;
      this.FactoryType = FactoryType;
      optionsdata['FactoryType'] = this.FactoryType;
      this.init(optionsdata, this.FactoryType);
      this._obj = [
          {
              label: 'Material Type',
              component: 'Select',
              field: field,
              componentProps: (model) => {
                  return {
                      mode: 'multiple',
                      options: optionsdata.Marterial,
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
      await mountedMaterial(optionsdata, FactoryType);
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