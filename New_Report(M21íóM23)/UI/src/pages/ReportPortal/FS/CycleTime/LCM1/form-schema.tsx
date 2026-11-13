import type { FormSchema } from '@/components/core/schema-form/';
import { reactive } from 'vue';
import { formatToDate, PreDate } from '@/utils/dateUtil';
import { Size,Owner, Operation, Equipment } from '@/pages/ReportPortal/Static/common';

/* export let optionsdata: any = reactive({
    SizeData: [{ label: 'ALL', value: 'ALL' }],
    ProductData: [],
    BigProductData: [],
    ProductData1: [],
    BigProductData1: [],
    OwnerName: [],
    OperationLabel: [],
    EquipmentData: [],
    BigEquipmentData: [],
    BigEquipmentData1: []
});
 */
let optionsdata = reactive({});
export const schemas= (FactoryType:string)=>{
    return [
        {
            label: 'common.commonpages.Date',
            field: 'FromDate',
            component: 'DatePicker',
            componentProps: {
                valueFormat: 'YYYY-MM-DD'
            },
            defaultValue: PreDate(),
            required: true
        },
        {
            label: 'common.commonpages.To',
            field: 'ToDate',
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
                        value: ''
                    },
                    {
                        label: '日(A)',
                        value: "and shift_id='A'"
                    },
                    {
                        label: '夜(B)',
                        value: "and shift_id='B'"
                    }
                ]
            },
            defaultValue: ''
        },
        {
            label: 'common.commonpages.Interval',
            component: 'RadioGroup',
            field: 'Interval',
            componentProps: {
                options: [
                    {
                        label: 'Day',
                        value: 'day'
                    },
                    {
                        label: 'Week',
                        value: 'week'
                    },
                    {
                        label: 'Month',
                        value: 'month'
                    }
                ]
            },
            defaultValue: 'day'
        },
        {
            label: 'common.commonpages.Floor',
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
        // product type 下拉单选框
        {
            label: 'LCM Product Type',
            component: 'Select',
            field: 'ProdType',
            componentProps: {
                options: [
                    {
                        label: 'All',
                        value: 'ALL'
                    },
                    {
                        label: 'Normal',
                        value: 'Normal'
                    },
                    {
                        label: 'FCN',
                        value: 'FCN'
                    },
                    {
                        label: 'FSOC',
                        value: 'FSOC'
                    },
                    {
                        label: 'MI',
                        value: 'MI'
                    },
                    {
                        label: 'SKD',
                        value: 'SKD'
                    },
                    {
                        label: 'TWOC',
                        value: 'TWOC'
                    }
                ]
            },
            defaultValue: ['ALL']
        },
        ...new Size(optionsdata, 'LCM1', {
            size: 'Size',
            bigproduct: 'BigProduct',
            optionproduct: 'OptionProduct',
            selectmenu: '#optionmenu',
            product: 'Product'
        }).obj,
       /*  {
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
            defaultValue: ['ALL']
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
            },
            defaultValue: ['ALL']
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
        ...new Owner(optionsdata, 'LCM1', 'LCM Owner').obj,
        /* {
        label: 'LCM Owner',
        component: 'Select',
        field: 'LcmOwner',
        componentProps: () => {
          return {
            mode: 'multiple',
            options: optionsdata.OwnerName,
          };
        },
        defaultValue:["ALL"],
      }, */
        ...new Operation(optionsdata, 'LCM1', 'Operation').obj,
        /* {
        label: 'Operation',
        component: 'Select',
        field: 'Operation',
        componentProps: () => {
          return {
            mode: 'multiple',
            options: optionsdata.OperationLabel,
          };
        }
      }, */
        ...new Equipment(optionsdata, 'LCM1', 0, {
            equip: 'equip',
            Optionequip: 'Optionequip',
            equipselectmenu: '#optionequipmenu',
            equipment: 'Equipment'
        }).obj
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
            }
            // defaultValue:["ALL"],
        } */
    ];
    
}