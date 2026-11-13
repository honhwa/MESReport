import type { FormSchema } from '@/components/core/schema-form/';
import {
    ShiftParams,
    LcmProductParams,
    LcdProductParams,
    LCMOwnerParams,
    StartDateParams,
    EndDateParams,
    PanelSourceParams,
    LCDGrade1Params,
    LCMProductSizeParams,
    LCMProductTypeParams
} from '@/pages/ReportPortal/Static/CommonParams';
import { reactive } from 'vue';
import { Size, Date, LCMProductType, Owner, Grade, Size2, Size3, Operation, LCMProductSize } from '@/pages/ReportPortal/Static/common';

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
    LCDProductData1: []
});
let PanelSourceOptions = [
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
export let optionsdata2: any = reactive({
    LcdGradeLabel: [],
    OperationOptionLCM1: [
        {
            label: '1600 CKEN',
            value: '1600'
        },
        {
            label: '1700 DKEN',
            value: '1700'
        },
        {
            label: '1800 PAKING',
            value: '1800'
        },
        {
            label: '1910 SHIPPING',
            value: '1910'
        },
        {
            label: '1350 BOND CKEN',
            value: '1350'
        }
    ],
    OperationOptionLCM2: [
        {
            label: '1600 CKEN',
            value: '1600'
        },
        {
            label: '1700 DKEN',
            value: '1700'
        },
        {
            label: '1800 PAKING',
            value: '1800'
        },
        {
            label: '1910 SHIPPING',
            value: '1910'
        },
        {
            label: '1300 BOND CKEN',
            value: '1300'
        },
        {
            label: '1350 BOND CKEN',
            value: '1350'
        }
    ],
    OperationOption: []
});

let optiondata3 = reactive({});
let optiondata4 = reactive({});

export const schemas: FormSchema[] = [
    ...new Date({ starttime: 'FromDate', endtime: 'ToDate' }).obj,
    /*  StartDateParams,
  EndDateParams, */
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
        label: 'common.commonpages.Floor',
        component: 'RadioGroup',
        field: 'Floor',
        componentProps: {
            options: [
                { label: 'ALL', value: '' },
                { label: '1F', value: " and floor='1'" },
                { label: '2F', value: " and floor='2'" },
                { label: '3F', value: " and floor='3'" }
            ]
        },
        defaultValue: ''
    },
    {
        label: 'common.commonpages.LCMProductArea',
        component: 'CheckboxGroup',
        field: 'LCMProductArea',
        componentProps: {
            options: [
                {
                    label: 'ALL',
                    value: 'ALL'
                },
                {
                    label: 'Moudle',
                    value: 'Moudle'
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
                    label: 'Down Grade ',
                    value: 'Down Grade '
                }
            ]
        },
        defaultValue: ['ALL']
    },
    {
        label: 'common.commonpages.SourceFab',
        component: 'RadioGroup',
        field: 'SourceFab',
        componentProps: {
            options: [
                {
                    label: 'ALL',
                    value: ''
                },
                {
                    label: 'CMO',
                    value: "and source_fab<>'IDT'"
                },
                {
                    label: 'IDT',
                    value: "and source_fab='IDT'"
                }
            ]
        },
        defaultValue: ''
    },
    // PanelSourceParams,
    {
        label: 'common.commonpages.PanelSource',
        component: 'Select',
        field: 'PanelSource',
        componentProps: {
            options: PanelSourceOptions.map((item) => {
                return { label: item, value: item };
            })
        },
        defaultValue: 'ALL'
    },
    {
        label: 'common.commonpages.ShowMode',
        component: 'RadioGroup',
        field: 'ShowMode',
        componentProps: {
            options: [
                {
                    label: '人員別',
                    value: 'Operator'
                },
                {
                    label: '機種別良率',
                    value: 'Operator1'
                },
                {
                    label: '機台別',
                    value: 'equip_nbr'
                },
                {
                    label: '機種別',
                    value: 'Prod_nbr'
                }
            ]
        },
        defaultValue: 'Operator'
    },
    {
        field: 'dgrate',
        component: 'Input',
        label: 'common.commonpages.Ratio',
        componentSlots: () => {
            return {
                prefix: () => 'Downgrage >',
                suffix: () => '%'
            };
        },
        defaultValue: '-100'
    },
    {
        field: 'rwrate',
        component: 'Input',
        label: '',
        componentSlots: () => {
            return {
                prefix: () => '送修 >',
                suffix: () => '%'
            };
        },
        defaultValue: '-100'
    },
    {
        field: 'WorkOrderLike',
        component: 'Input',
        label: 'common.commonpages.WorkOrderLike'
    },
    ...new LCMProductType(optiondata3, 'LCM2', 'LCMProductType').obj,
    /* {
    label: 'common.commonpages.LCMProductType',
    component: 'Select',
    field: 'LCMProductType',
    componentProps: () => {
      return {
        options: optionsdata2.LcmProdTypeData,
      };
    },
    defaultValue:"ALL"
  }, */
    {
        field: 'LCMDivider',
        component: 'Divider',
        label: 'common.commonpages.LCMProduct'
    },
    // ...LcmProductParams,
    ...new Size(optiondata3, 'LCM2', {
        size: 'Size',
        bigproduct: 'bigproduct',
        optionproduct: 'OptionProduct',
        selectmenu: '#optionmenu',
        product: 'Product'
    }).getSize(),
    ...new Owner(optiondata3, 'LCM2', 'LCMOwner').obj,
    // LCMOwnerParams,
    ...new Grade(optiondata3, 'LCM2', 'LCDGrade').obj,
    /* {
        label: 'LCD Grade',
        component: 'Select',
        field: 'LCDGrade',
        componentProps: () => {
            return {
                mode: 'multiple',
                options: optionsdata2.LcdGradeLabel
            };
        }
    }, */
    {
        field: '#LCDDivider',
        component: 'Divider',
        label: 'LCD Product'
    },
    ...new Size3(optiondata4, 'LCM2', {
        size: 'LCDSize',
        optionproduct: 'LCDoptionproduct',
        selectmenu: '#LCDoptionmenu',
        product: 'LCDProduct'
    }).obj,
    // ...LcdProductParams,
    {
        label: 'common.commonpages.Operation',
        component: 'Select',
        field: 'Operation',
        componentProps: () => {
            return {
                options: optionsdata2.OperationOption
            };
        },
        defaultValue: '1600'
    },
    ...new LCMProductSize(optiondata3,"LCM2","LCMProductSize").obj,
    // LCMProductSizeParams
];
