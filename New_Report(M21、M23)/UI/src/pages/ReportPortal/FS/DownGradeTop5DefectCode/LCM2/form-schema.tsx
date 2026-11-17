import type { FormSchema } from '@/components/core/schema-form/';
// import { AddCommonPageParamsWithOut } from '@/store/module/commonpage';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil';
import { Owner, Size, FSType, Grade, Size2, Size3 } from '@/pages/ReportPortal/Static/common';

import { ErrorCode } from '@/pages/ReportPortal/FS/DownGradeTop5DefectCode/LCM1/form-schema';
// const UseParamsAdd = AddCommonPageParamsWithOut();
export let optionsdata: any = reactive({
    ProductData: [],
    ProductData1: [],
    SizeData: [],
    BigProductData: [],
    BigProductData1: [],
    OwnerName: [],
    LcdGradeLabel: [],
    LCDProductData: [],
    BigLCDProductData: [],
    BigLCDProductData1: [],
    tempBigLCDProductData: [],
    Error_codeData: []
});

export const schemas = (FactoryType: string) => {
    let optiondata = reactive({});
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
            field: 'Todate',
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
            label: 'LCM Product Area',
            component: 'RadioGroup',
            field: 'LCMProductArea',
            componentProps: {
                options: [
                    {
                        label: 'ALL',
                        value: 'ALL'
                    },
                    {
                        label: 'Moudle',
                        value: "Select Distinct part_nbr SOption,prod_type From HMASHIS_IPMS Where Fac_ID='A' and length(part_nbr) >10  Order by part_nbr"
                    },
                    {
                        label: 'SKD',
                        value: 'SKD'
                    },
                    {
                        label: 'LED',
                        value: 'LED'
                    },
                    {
                        label: 'Down Grade',
                        value: 'Down Grade'
                    }
                ]
            },
            // defaultValue: ["ALL"],
            defaultValue: 'ALL'
        },
        {
            label: '顯示方式',
            component: 'RadioGroup',
            field: 'ShowType',
            componentProps: {
                options: [
                    {
                        label: '大機種',
                        value: ' substr(d.prod_nbr,1,5) '
                    },
                    {
                        label: '小機種',
                        value: ' d.prod_nbr '
                    }
                ]
            },
            defaultValue: ' substr(d.prod_nbr,1,5) '
            // vIf:()=>UseParamsAdd.Fab=='LCM1'
        },
        {
            label: '檢測站點',
            component: 'RadioGroup',
            field: 'Operation',
            componentProps: {
                options: [
                    {
                        label: '1350',
                        value: '1350'
                    },
                    {
                        label: '1300',
                        value: '1300'
                    },
                    {
                        label: '1600',
                        value: '1600'
                    }
                ]
            },
            defaultValue: '1600'
        },
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
            label: 'LCM Product',
            component: 'Divider',
            field: '#LCM'
        },
        ...new Size(optiondata, FactoryType, {
            size: 'Size',
            bigproduct: 'BigProduct',
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
            }
            // defaultValue: 'ALL'
        },
        {
            label: 'big product',
            component: 'Select',
            field: 'BigProduct',
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
            }
            // defaultValue: 'ALL'
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
            },
            defaultValue: ['ALL']
        }, */
        ...new Owner(optiondata, FactoryType, 'LCMOwner').obj,
        /*  {
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
        ...new ErrorCode(optiondata, FactoryType, 'ErrorCode').obj,

        /* {
            label: 'Error Code',
            component: 'Select',
            field: 'ErrorCode',
            componentProps: () => {
                return {
                    mode: 'multiple',
                    options: optionsdata.Error_codeData
                };
            },
            defaultValue: ['ALL']
        }, */
        ...new Grade(optiondata, FactoryType, 'LCDGrade').obj,

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
        {
            label: 'LCD Product',
            component: 'Divider',
            field: '#LCD'
        },
        ...new Size3(optiondata2, FactoryType, {
            size: 'LCDProductSIZE',
            optionproduct: 'LCDOptionProduct',
            selectmenu: '#LCDoptionmenu',
            product: 'LCDProduct'
        }).obj
        /*  {
            label: 'Size',
            component: 'Select',
            field: 'LCDProductSIZE',
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
            defaultValue: 'ALL'
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
        } */
    ];
};
