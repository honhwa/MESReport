import type { FormSchema } from '@/components/core/schema-form/';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil';

export let optionsdata: any = reactive({
    Shift: [
        {
            label: 'ALL',
            value: 'ALL'
        },
        {
            label: '日(A)',
            value: 'A'
        },
        {
            label: '夜(B)',
            value: 'B'
        }
    ],
    Rework: [
        { label: 'ALL', value: 'ALL' },
        { label: '是', value: 'Y' },
        { label: '否', value: 'N' },
    ],
    TransType: [
        { label: '快修NG視為OK', value: 'OK' },
        { label: '快修NG視為NG', value: 'NG' },
        { label: '所有快修', value: 'ALL' }
    ],
    TN_SizeData: [{ label: 'ALL', value: 'ALL' }],
    TN_ProductData: [],
    TN_BigProductData: [],
    TN_LCMOwner: [],
    TN_Opername: [],
    TN_OperationLabel: [],
    TN_BigEquipment:[],
    TN_Equipment: [],
    TN_ProductData1: [],
    TN_BigProductData1: [],
    TN_Equipment1: []
});

export const schemas: FormSchema[] = [
    {
        field: 'sDate',
        component: 'DatePicker',
        label: 'common.commonpages.Date',
        componentProps: {
            valueFormat: 'YYYY-MM-DD'
        },
        defaultValue: formatToDate(),
        required: true
    },
    {
        field: 'eDate',
        component: 'DatePicker',
        label: 'common.commonpages.Date',
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
        componentProps: () => {
            return {
                options: optionsdata.Shift
            };
        },
        defaultValue: 'ALL'
    },
    {
        label: '快修種類',
        component: 'RadioGroup',
        field: 'TransType',
        componentProps: () => {
            return {
                options: optionsdata.TransType
            };
        },
        defaultValue: 'OK'
    },
    {
        label: 'Rework',
        component: 'RadioGroup',
        field: 'Rework',
        componentProps: () => {
            return {
                options: optionsdata.Rework
            };
        },
        defaultValue: 'ALL'
    },
    {
        label: 'common.commonpages.size',
        component: 'Select',
        field: 'Size',
        componentProps: () => {
            return {
                options: optionsdata.TN_SizeData,
                placeholder: 'size',
                onChange: async (e: any) => {
                    if (e !== 'ALL') {
                        optionsdata.TN_BigProductData1 = optionsdata.TN_BigProductData.filter(
                            (item) => item?.value.indexOf(e) != -1
                        );
                    } else {
                        optionsdata.TN_BigProductData1 = optionsdata.TN_BigProductData;
                    }
                }
            };
        }
    },
    {
        label: 'common.commonpages.bigproduct',
        component: 'Select',
        field: 'Product',
        componentProps: () => {
            return {
                options: optionsdata.TN_BigProductData1,
                placeholder: 'big product',
                onChange: async (e: any) => {
                    if (e !== 'ALL') {
                        optionsdata.TN_ProductData1 = optionsdata.TN_ProductData.filter(
                            (item) => item?.value.indexOf(e) != -1
                        );
                    } else {
                        optionsdata.TN_ProductData1 = optionsdata.TN_ProductData;
                    }
                }
            };
        }
    },
    {
        label: 'common.commonpages.Product',
        field: 'Product',
        component: 'Select',
        componentProps: () => {
            return {
                mode: 'multiple',
                options: optionsdata.TN_ProductData1,
                placeholder: 'product'
            };
        }
    },
    {
        label: 'common.commonpages.LCMOwner',
        component: 'Select',
        field: 'LCMOwner',
        componentProps: () => {
            return {
                mode: 'multiple',
                options: optionsdata.TN_LCMOwner,
                placeholder: 'lcm_owner'
            };
        }
    },
    {
        label: 'common.commonpages.Operation',
        component: 'Select',
        field: 'Operation',
        componentProps: () => {
            return {
                mode: 'multiple',
                options: optionsdata.TN_OperationLabel,
                placeholder: 'Operation'
            };
        }
    },
    {
        label: 'Big Equipment',
        component: 'Select',
        field: 'BigEquipment',
        componentProps: () => {
            return {
                options: optionsdata.TN_BigEquipment,
                placeholder: 'BigEquipment',
                onChange: async (e: any) => {
                    if (e !== 'ALL') {
                        optionsdata.TN_Equipment1 = optionsdata.TN_Equipment.filter(
                            (item) => item?.value.indexOf(e) != -1
                        );
                    } else {
                        optionsdata.TN_Equipment1 = optionsdata.TN_Equipment;
                    }
                }
            };
        }
    },
    {
        label: 'Equipment',
        field: 'Equipment',
        component: 'Select',
        componentProps: () => {
            return {
                mode: 'multiple',
                options: optionsdata.TN_Equipment1,
                placeholder: 'Equipment'
            };
        }
    }
];
