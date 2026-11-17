import type { FormSchema } from '@/components/core/schema-form/';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil';
import { Owner, Size ,FSType, Grade, Equipment, DefectCode, Size2, Size3, ExpectDefectCode, LCMProductType,LCMProductSize} from '@/pages/ReportPortal/Static/common';
import { GetCommon } from '@/api/pages/common';
async function getOperaction(optionsdata,FactoryType, oper_type) {
    let returndata: object[] = [];
    let {
        Date: { Table: Data }
    } = await GetCommon({
        G_FAC: 'A',
        name: 'Date',
        apiName: 'Q_Operaction',
        oper_type: oper_type,
        FactoryType:FactoryType
    });
    Data.forEach((item) => {
        returndata.push({
            label: `${item['WORK_CTR']}-${item['WORK_NAME']}`,
            value: item['WORK_CTR']
        });
    });
    returndata.unshift({ label: 'ALL', value: 'ALL' });
    optionsdata['OperationLabel'] = returndata;
}
// 獲取operaction
 class Operation {
    _obj: FormSchema[];
    optionsdata: FSType;
    FactoryType: string;
    constructor(optionsdata: FSType, FactoryType: string, oper_type:string,field: string = 'Operation') {
        this.optionsdata = optionsdata;
        this.FactoryType = FactoryType;
        optionsdata['FactoryType'] = this.FactoryType;
        this.init(optionsdata, this.FactoryType,oper_type);
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
                            }
                        }
                    };
                },
                defaultValue: ['ALL']
            }
        ];
    }
    // api請求數據
    async init(optionsdata: FSType, FactoryType: string,oper_type:string) {
        await getOperaction(optionsdata, FactoryType,1);
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
        ExpectDefectCodeData: []
    });
    let optiondata2 = reactive({});
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
        },
        {
            label: 'Module',
            component: 'RadioGroup',
            field: 'Module',
            componentProps: {
                options: [
                    {
                        label: 'ALL',
                        value: ''
                    },
                    {
                        label: 'CMO',
                        value: " and source_fab<>'IDT'"
                    },
                    {
                        label: 'IDT',
                        value: " and source_fab='IDT'"
                    },
                    {
                        label: 'Module',
                        value: `" and prod_nbr in (select prod_nbr from HM"&G_FAC&"SHIS_SETUP_DOWNGRADE where module_1='MODULE')`
                    },
                    {
                        label: 'Olympic',
                        value: `" and prod_nbr in (select prod_nbr from HM"&G_FAC&"SHIS_SETUP_DOWNGRADE where module_1='OLYMPIC')"`
                    },
                    {
                        label: 'No aging',
                        value: ' and aging_count=0'
                    },
                    {
                        label: '1次 aging',
                        value: ' and aging_count=1'
                    },
                    {
                        label: '2次以上aging',
                        value: ' and aging_count>=2'
                    }
                ]
            },
            defaultValue: ''
        },
        {
            label: 'LCM Product Area',
            component: 'RadioGroup',
            field: 'prodarea',
            componentProps: {
                options: [
                    {
                        label: 'ALL',
                        value: 'ALL'
                    },
                    {
                        label: 'Moudle',
                        value: "Moudle"
                    },
                    {
                        label: 'SKD',
                        value: "SKD"
                    },
                    {
                        label: 'LED',
                        value: 'LED'
                    },
                    // 後面三個無效
                    {
                        label: 'Down Grade',
                        value: "Down Grade"
                    },
                    {
                        label: '3D',
                        value: '3D'
                    }
                ],
                onChange: async (e: any) => {
                    console.log('輸出e', e);
                }
            },
            defaultValue: 'ALL'
        },
        ...new LCMProductSize(optionsdata,FactoryType,"LCMProductSize").obj,
        /* {
            label: 'LCM Product Size',
            component: 'Select',
            field: 'LCMProductSize',
            componentProps: () => {
                return {
                    mode: 'multiple',
                    options: optionsdata.LCMProductSizeData
                };
            },
            defaultValue: ['ALL']
        }, */
        {
            label: 'Panel Source',
            component: 'Select',
            field: 'PanelSource',
            componentProps: () => {
                let tempdata = [
                    'ALL',
                    'LCD1',
                    'LCD2',
                    'LCD3',
                    'LCD4',
                    'LCD5',
                    'LCD6',
                    'LCD7',
                    'LCD8',
                    'LCDM',
                    'HDA1',
                    'NHAP',
                    'NHAL',
                    'NLC3',
                    'NHBD'
                ];
                let optiondata: object[] = [];
                tempdata.forEach((item) => {
                    optiondata.push({
                        label: item,
                        value: item
                    });
                });
                return {
                    options: optiondata
                };
            },
            defaultValue: 'ALL'
        },
        {
            label: 'Reworked',
            component: 'Select',
            field: 'Reworked',
            componentProps: () => {
                return {
                    options: [
                        {
                            label: 'ALL',
                            value: 'ALL'
                        },
                        {
                            label: '1次',
                            value: '1'
                        },
                        {
                            label: '1次以上',
                            value: 'over_1'
                        },
                        {
                            label: '2次',
                            value: '2'
                        },

                        {
                            label: '2次以上',
                            value: 'over_2'
                        },
                        {
                            label: '3次',
                            value: '3'
                        },
                        {
                            label: 'No',
                            value: 'N'
                        }
                    ]
                };
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
                        label: '機台別',
                        value: 'equip_nbr'
                    },
                    {
                        label: '大機種',
                        value: 'big_prod'
                    },
                    {
                        label: '線別',
                        value: 'equip_line'
                    }
                ],
                style: { color: 'red' }
            },
            defaultValue: 'work_ctr'
        },
        {
            label: 'Work order like',
            field: "WorkOrderLike",
            component: 'InputTextArea'
        },
        {
            label: 'Operator',
            field: 'Operator',
            component: 'InputTextArea'
        },
        ...new LCMProductType(optiondata2,FactoryType,"LCMProductType").obj,

        /* {
            label: 'LCM Product Type',
            component: 'Select',
            field: 'LCMProductType',
            componentProps: () => {
                return {
                    options: optionsdata.LcmProdTypeData
                };
            },
            defaultValue: 'ALL'
        }, */
        {
            field: 'LCMDivider',
            component: 'Divider',
            label: 'common.commonpages.LCMProduct'
        },
        ...new Size(optionsdata, FactoryType, {
            size: 'Size',
            bigproduct: 'bigProduct',
            optionproduct: 'OptionProduct',
            selectmenu: '#optionmenu',
            product: 'Product'
        }).obj,
        /* {
            label: 'size',
            component: 'Select',
            field: 'Size',
            componentProps: () => {
                return {
                    options: optionsdata.SizeData,
                    placeholder: 'size',
                    onChange: async (e: any) => {
                        if (e !== 'ALL') {
                            optionsdata.BigProductData1 = optionsdata.BigProductData.filter(
                                (item) => item?.value.indexOf(e) != -1
                            );
                        } else {
                            optionsdata.BigProductData1 = optionsdata.BigProductData;
                        }
                    }
                };
            },
            defaultValue: 'ALL'
        },
        {
            label: 'big product',
            component: 'Select',
            field: 'bigProduct',
            componentProps: () => {
                return {
                    options: optionsdata.BigProductData1,
                    placeholder: 'big product',
                    onChange: async (e: any) => {
                        if (e !== 'ALL') {
                            optionsdata.ProductData1 = optionsdata.ProductData.filter(
                                (item) => item?.value.indexOf(e) != -1
                            );
                        } else {
                            optionsdata.ProductData1 = optionsdata.ProductData;
                        }
                    }
                };
            },
            defaultValue: 'ALL'
        },
        {
            label: 'Product',
            field: 'Product',
            component: 'Select',
            componentProps: () => {
                return {
                    mode: 'multiple',
                    options: optionsdata.ProductData1,
                    placeholder: 'product'
                };
            }
            // defaultValue: ['ALL']
        }, */
        ...new Owner(optionsdata, FactoryType, 'LCMOwner').obj,
        /* {
            label: 'LCM Owner',
            component: 'Select',
            field: 'LCMOwner',
            componentProps: () => {
                return {
                    mode: 'multiple',
                    options: optionsdata.OwnerName
                };
            },
            defaultValue: ['ALL']
        }, */
        ...new Operation(optionsdata,FactoryType,"Operation").obj,
        /* {
            label: 'Operation',
            component: 'Select',
            field: 'Operation',
            componentProps: () => {
                return {
                    mode: 'multiple',
                    options: optionsdata.OperationLabel
                };
            },
            defaultValue: ['ALL']
        }, */
        ...new Grade(optionsdata,FactoryType,"LCDGrade").obj,
        /* {
            label: 'LCD Grade',
            component: 'Select',
            field: 'LCDGrade',
            componentProps: () => {
                return {
                    mode: 'multiple',
                    options: optionsdata.LcdGradeLabel
                };
            }
        }, */
        ...new Equipment(optionsdata,FactoryType,1,{
            equip: 'equip',
            Optionequip: 'optionEquipment',
            equipselectmenu: '#optionequipmenu',
            equipment: 'Equipment'
        }).obj,
       /*  {
            label: 'Equipment',
            component: 'Select',
            field: 'equip',
            componentProps: () => {
                return {
                    // mode: 'multiple',
                    options: optionsdata.EquipmentData,
                    onChange: async (e: any) => {
                        if (e !== 'ALL') {
                            optionsdata.BigEquipmentData1 = optionsdata.BigEquipmentData.filter(
                                (item) => item?.value.indexOf(e) != -1
                            );
                        } else {
                            optionsdata.BigEquipmentData1 = optionsdata.BigEquipmentData;
                        }
                    }
                };
            },
            defaultValue: ['ALL']
        },
        {
            label: 'Big Equipment',
            component: 'Select',
            field: 'Equipment',
            componentProps: () => {
                return {
                    mode: 'multiple',
                    options: optionsdata.BigEquipmentData1
                };
            },
            defaultValue: ['ALL']
        }, */
        ...new DefectCode(optionsdata,FactoryType,"DefectCode").obj,
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
        }, */
        {
            field: 'LCDDivider',
            component: 'Divider',
            label: 'common.commonpages.LCDProduct'
        },
        ...new Size3(optiondata2,FactoryType,{
            size: 'LCDProductSize',
            optionproduct: 'LCDOptionProduct',
            selectmenu: '#LCDoptionmenu',
            product: 'LCDProduct'
        }).obj,
       /*  {
            label: 'LCD Product Size',
            component: 'Select',
            field: 'LCDProductSize',
            componentProps: () => {
                return {
                    options: optionsdata.LCDProductData,
                    onChange: async (e: any) => {
                        let BigLCDProductData: object[] = [];
                        if (e !== 'ALL') {
                            optionsdata.BigLCDProductData1.forEach((item) => {
                                if (item['SIZE1'] == e) {
                                    BigLCDProductData.push({
                                        label: item['PART'],
                                        value: item['PART']
                                    });
                                }
                            });
                            optionsdata.BigLCDProductData = BigLCDProductData;
                        } else {
                            optionsdata.BigLCDProductData = optionsdata.tempBigLCDProductData;
                        }
                    }
                };
            },
            defaultValue: ['ALL']
        },
        {
            label: 'LCD Product',
            component: 'Select',
            field: 'LCDProduct',
            componentProps: () => {
                return {
                    mode: 'multiple',
                    options: optionsdata.BigLCDProductData
                };
            },
            defaultValue: ['ALL']
        }, */
        ...new ExpectDefectCode(optiondata2,FactoryType,"ExpectDefectCode").setDefault(0,['1==1', 'C311', 'D016', 'PCZCJ', 'PCZE4']).obj,
        /* {
            label: 'Expect Defect Code',
            component: 'Select',
            field: 'ExpectDefectCode',
            componentProps: () => {
                return {
                    mode: 'multiple',
                    options: optionsdata.ExpectDefectCodeData
                };
            },
            defaultValue: ['1==1', 'C311', 'D016', 'PCZCJ', 'PCZE4']
        } */
    ];
};

