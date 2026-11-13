import type { FormSchema } from '@/components/core/schema-form/';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil';
import {
    Date,
    Floor,
    Shift,
    LCMProductSize,
    LCMProductType,
    Size,
    Owner,
    Operation,
    Grade,
    Equipment,
    DefectCode,
    LCDProductSize,
    ExpectDefectCode
} from '@/pages/ReportPortal/Static/common';
export let optionsdata: any = reactive({
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

export const schemas = (FactoryType: string, SubFab: string) => {
    return [
        ...new Date()._obj,
        ...new Shift(1)._obj,
        ...new Floor(1)._obj,
        ...new LCMProductSize(optionsdata, FactoryType)._obj,
        {
            label: 'common.commonpages.PanelSource',
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
            label: 'common.commonpages.Reworked',
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
            label: 'common.commonpages.ShowMode',
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
                ]
            },
            defaultValue: 'work_ctr'
        },
        {
            label: 'common.commonpages.WorkOrderLike',
            field: 'WorkOrderLike',
            component: 'InputTextArea'
        },
        {
            label: 'common.commonpages.Operator',
            field: 'Operator',
            component: 'InputTextArea'
        },
        ...new LCMProductType(optionsdata, FactoryType)._obj,
        {
            field: 'LCMDivider',
            component: 'Divider',
            label: 'common.commonpages.LCMProduct'
        },
        ...new Size(optionsdata, FactoryType).obj,
        ...new Owner(optionsdata, FactoryType, 'LCMOwner').setLabel(0, 'Set Owner')._obj,
        ...new Operation(optionsdata, SubFab == 'INT' ? 'SET11' : FactoryType)._obj,
        ...new Grade(optionsdata, FactoryType).setLabel(0, 'common.commonpages.LCMGrade')._obj,
        ...new Equipment(optionsdata, FactoryType, 0).getEquipment(),
        ...new DefectCode(optionsdata, FactoryType)._obj,
        {
            field: 'LCDDivider',
            component: 'Divider',
            label: 'common.commonpages.LCDProduct'
        },
        ...new LCDProductSize(optionsdata, FactoryType, 2).obj,
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
        ...new ExpectDefectCode(optionsdata, FactoryType)._obj
    ];
};
