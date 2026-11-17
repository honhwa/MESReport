import type { FormSchema } from '@/components/core/schema-form/';
import { Grade, LCMProductSize, LCMProductType, Owner, Size, Size2, Size3 } from '@/pages/ReportPortal/Static/common';
import {
    StartDateParams,
    EndDateParams,
    ShiftParams,
    LcmProductParams,
    LCMOwnerParams,
    LCMProductSize1Params,
    LCDGradeParams,
    LcdProductParams,
    LCMProductTypeParams,
    PanelSourceParams
} from '@/pages/ReportPortal/Static/CommonParams';
import { reactive } from 'vue';

let optiondata3 = reactive({});
let optiondata4 = reactive({});

export const schemas: FormSchema[] = [
    StartDateParams,
    EndDateParams,
    ShiftParams,
    PanelSourceParams,
    {
        label: 'common.commonpages.LCMProductArea',
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
                    value: 'Moudle'
                },
                {
                  label: 'SKD',
                  value: 'SKD'
                },
                {
                    label: 'Down Grade ',
                    value: 'Down Grade '
                }
            ]
        },
        defaultValue: 'ALL'
    },
    {
        label: 'common.commonpages.LCMProductSize',
        component: 'RadioGroup',
        field: 'LCMProductSize',
        componentProps: {
            options: [
                {
                    label: 'ALL',
                    value: 'ALL'
                },
                {
                    label: '37”以下',
                    value: '37”以下'
                },
                {
                    label: '37”及以上',
                    value: '37”及以上'
                }
            ]
        },
        defaultValue: 'ALL'
    },
    // LCMProductSize1Params,
    ...new LCMProductSize(optiondata3, 'LCM2', 'LCMProductSize1').obj,
    ...new LCMProductType(optiondata3,"LCM2","LCMProductType").obj,
    // LCMProductTypeParams,
    
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
    }).obj,

    // LCMOwnerParams,
    ...new Owner(optiondata3,"LCM2","LCMOwner").obj,
    {
        field: 'LCDDivider',
        component: 'Divider',
        label: 'common.commonpages.LCDProduct'
    },
    // ...LcdProductParams,
    ...new Size3(optiondata4,"LCM2",{
      size: 'LCDSize',
      optionproduct: 'lcdOptionProduct',
      selectmenu: '#lcdoptionmenu',
      product: 'LCDProduct'
    }).obj,
    // LCDGradeParams,
    ...new Grade(optiondata3,"LCM2").obj
];
