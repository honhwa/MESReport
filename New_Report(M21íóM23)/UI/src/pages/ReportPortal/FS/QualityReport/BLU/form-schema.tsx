import type { FormSchema } from '@/components/core/schema-form/';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil';
import { DefectCode, ExpectDefectCode, FSType, Ifield, Owner, Size3 } from '@/pages/ReportPortal/Static/common';
import { GetCommon } from '@/api/pages/common';
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
    optionsdata['SizeData'] = BL_SizeData;
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
    // 初始化時候的不選擇size時候的product
    optionsdata['ProductData1'] = PART_NBRshowData;
    // 重新選擇Size為ALL的時候的product
    optionsdata['ProductData2'] = PART_NBRshowData;
    // Size不選擇All的時候篩選
    optionsdata['ProductData'] = Data1.map((item) => {
        return {
            label: item['BL_SIZE'],
            value: item['PART_NBR']
        };
    });
}
export class SpecialSize {
    optionsdata: FSType;
    FactoryType: string;
    obj: FormSchema[];
    fieldobj: Omit<Ifield, 'bigproduct'>;
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
        await getMarterial(optionsdata, FactoryType);
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
                            if (e != 'ALL') {
                                console.log('輸出可供選擇的optionsdata.ProductData', optionsdata.ProductData);

                                optionsdata.ProductData1 = optionsdata.ProductData?.filter(
                                    (item) => item?.label.indexOf(e) != -1
                                ).map((item) => {
                                    return {
                                        label: item['value'],
                                        value: item['value']
                                    };
                                });
                            } else {
                                optionsdata.ProductData1 = optionsdata['ProductData2'];
                            }
                        },
                        onSelect: async (e: any) => {
                            // 默認選中第一個
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

                        onDeselect: (value, option) => {
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

export const schemas = (FactoryType: string) => {
    let optionsdata: any = reactive({
        LcmProdTypeData: [],
        ProductData: [],
        ProductData1: [],
        SizeData: [],
        BigProductData: [],
        BigProductData1: [],
        OwnerName: [],
        OperationLabel: [],
        EquipmentData: [],
        BigEquipmentData: [],
        BigEquipmentData1: [],
        LcdGradeLabel: [],
        LCMProductSizeData: [],
        DefectCodeData: [],
        LCDProductData: [],
        BigLCDProductData: [],
        BigLCDProductData1: [],
        tempBigLCDProductData: [],
        ExpectDefectCodeData: [],
        BL_sizeData: [],
        PART_NBRData: [],
        PART_NBRdefaultData: [],
        PART_NBRALLData: []
    });
    
    return [
        {
            field: 'FromDate',
            component: 'DatePicker',
            label: 'Date',
            componentProps: {
                valueFormat: 'YYYY-MM-DD'
            },
            defaultValue: formatToDate(),
            required: true
        },
        {
            field: 'ToDate',
            component: 'DatePicker',
            label: 'To',
            componentProps: {
                valueFormat: 'YYYY-MM-DD'
            },
            defaultValue: formatToDate(),
            required: true
        },
        {
            label: '查詢范圍',
            component: 'RadioGroup',
            field: 'model',
            componentProps: {
                options: [
                    {
                        label: 'ALL',
                        value: 'ALL'
                    },
                    {
                        label: '自組',
                        value: 'self'
                    },
                    {
                        label: '外購',
                        value: 'buy'
                    }
                ]
            },
            defaultValue: 'ALL'
        },
        {
            label: '顯示方式',
            component: 'RadioGroup',
            field: 'DisplayMode',
            componentProps: {
                options: [
                    {
                        label: '站點',
                        value: 'work_ctr'
                    },
                    {
                        label: '主線線別',
                        value: 'equip_nbr'
                    },
                    {
                        label: '主線機種',
                        value: 'big_prod'
                    },
                    {
                        label: 'BL機種',
                        value: 'BL_big_prod'
                    }
                ]
            },
            defaultValue: 'work_ctr'
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
                ]
            },
            defaultValue: ''
        },

        {
            label: 'Floor',
            component: 'RadioGroup',
            field: 'Floor',
            componentProps: {
                options: [
                    {
                        label: 'ALL',
                        value: ''
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
        },
        {
            label: 'Reworked',
            component: 'RadioGroup',
            field: 'rework',
            componentProps: {
                options: [
                    {
                        label: 'ALL',
                        value: ''
                    },
                    {
                        label: 'No',
                        value: " and  BL_SHIP_TYPE ='NORMAL PRODUCT' "
                    },
                    {
                        label: 'BL Rework',
                        value: " and  BL_SHIP_TYPE ='QUICKLY REWORK' "
                    },
                    {
                        label: 'BL Resorting',
                        value: " and  BL_SHIP_TYPE ='NORMAL REWORK' "
                    }
                ]
            },
            defaultValue: ''
        },

        {
            label: 'Marterial',
            component: 'Divider',
            field: '#Marterial'
        },
        ...new SpecialSize(optionsdata, FactoryType, {
            size: 'BL_size',
            optionproduct: 'OptionProduct',
            selectmenu: '#optionmenu',
            product: 'Product'
        }).obj,
        /*  {
            label: 'BL_size',
            component: 'Select',
            field: 'BL_size',
            componentProps: (model) => {
                return {
                    options: optionsdata.BL_sizeData,
                    onChange: async (e: any) => {
                        if (e != 'ALL') {
                            let temp: object[] = [];
                            optionsdata.PART_NBRALLData.forEach((item) => {
                                if (item['BL_SIZE'] === e) {
                                    temp.push({
                                        label: item['PART_NBR'],
                                        value: item['PART_NBR']
                                    });
                                }
                            });
                            optionsdata.PART_NBRData = temp;
                        } else {
                            optionsdata.PART_NBRData = optionsdata.PART_NBRdefaultData;
                        }
                    },
                    onSelect: async (e: any) => {
                        model.formModel.Product = Array.from(
                            new Set(optionsdata.PART_NBRData.map((item) => item.value))
                        );
                    }
                };
            },
            defaultValue: 'ALL'
        },
        {
            label: 'PART_NBR:',
            component: 'Select',
            field: 'Product',
            componentProps: () => {
                return {
                    mode: 'multiple',
                    options: optionsdata.PART_NBRData,
                    maxTagCount: 8
                };
            },
            defaultValue: ['ALL']
        }, */
        ...new Owner(optionsdata, FactoryType, 'LCMOwner').setLabel(0, 'Owner').obj,
        /* {
      label: 'Owner',
      component: 'Select',
      field: 'LCMOwner',
      componentProps: () => {
        return {
          mode: 'multiple',
          options: optionsdata.OwnerName,
        }
      },
      defaultValue: ["ALL"],
    }, */
        ...new DefectCode(optionsdata, FactoryType, 'DefectCode').setDefault(0, [
          "dfct_code = 'PCU16'",
          "dfct_code = 'PCU12'",
          "dfct_code = 'PCU11'",
          "dfct_code = 'PCU50'",
          "dfct_code = 'PCE13'",
          "dfct_code = 'PCE04'",
          "dfct_code = 'PCUC2'",
          "dfct_code = 'PCEC8'",
          "dfct_code = 'PCZ80'",
          "dfct_code = 'PCZ81'",
          "dfct_code = 'PCEC1'",
          "dfct_code = 'PCUCA'",
          "dfct_code = 'PCUC4'",
          "dfct_code = 'PCUC5'",
          "dfct_code = 'PCE07'",
          "dfct_code = 'PCZ79'",
          "dfct_code = 'PCE09'",
          "dfct_code = 'PCK02'",
          "dfct_code = 'PCEC2'",
          "dfct_code = 'PCEC3'",
          "dfct_code = 'PCEC4'",
          "dfct_code = 'PCEC5'",
          "dfct_code = 'PCEC6'",
          "dfct_code = 'PCEC7'",
          "dfct_code = 'PCH10'",
          "dfct_code = 'PCU10'",
          "dfct_code = 'PCU14'",
          "dfct_code = 'PCU18'",
          "dfct_code = 'PCU45'",
          "dfct_code = 'PCUC1'",
          "dfct_code = 'PCUC3'",
          "dfct_code = 'PCUC8'",
          "dfct_code = 'PCUC9'",
          "dfct_code = 'PCUCD'",
          "dfct_code = 'PCUCG'",
          "dfct_code = 'PCUCH'",
          "dfct_code = 'PCUH4'",
          "dfct_code = 'PCV43'",
          "dfct_code = 'PCV44'",
          "dfct_code = 'PCV45'",
          "dfct_code = 'PCV46'",
          "dfct_code = 'PCV48'",
          "dfct_code = 'PCV58'",
          "dfct_code = 'PCV59'",
          "dfct_code = 'PCV65'",
          "dfct_code = 'PCZ78'",
          "dfct_code = 'PCZC6'",
          "dfct_code = 'PCZCD'",
          "dfct_code = 'PCE32'",
        ]).obj,
        /* {
            label: 'Defect Code',
            component: 'Select',
            field: 'DefectCode',
            componentProps: () => {
                return {
                    mode: 'multiple',
                    options: optionsdata.DefectCodeData
                };
            },
            defaultValue: ['1=1']
        } */
    ];
};
