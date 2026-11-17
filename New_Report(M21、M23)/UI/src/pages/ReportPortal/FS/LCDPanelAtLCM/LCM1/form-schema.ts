import type { FormSchema } from '@/components/core/schema-form/';
import { formatToDate } from '@/utils/dateUtil';
import { optionsdata } from '@/pages/ReportPortal/FS/PanelWipStayChart/LCM1/form-schema';
import { OwnerName } from '@/pages/ReportPortal/Static/CommonParams';
import { Grade, Owner, Size, Size3 } from '@/pages/ReportPortal/Static/common';
import { reactive } from 'vue';

/* let LCDGradeOptions = [
    '1B',
    '1Y',
    'B',
    'C6',
    'D0',
    'G0',
    'L',
    'N',
    'D',
    'F',
    'M',
    'A',
    'C',
    'C1',
    'K',
    'E',
    '3N',
    'NA',
    'NC',
    'NM',
    'NN'
]; */
export const schemas =  (FactoryType:string) =>{
    let optionsdata2 = reactive({});
    let optionsdata3 = reactive({});
    return [
        {
            field: 'FromDate',
            component: 'DatePicker',
            label: 'Date',
            componentProps: {
                valueFormat: 'YYYY-MM-DD'
            },
            defaultValue: formatToDate()
        },
        {
            field: 'ToDate',
            component: 'DatePicker',
            label: 'To',
            componentProps: {
                valueFormat: 'YYYY-MM-DD'
            },
            defaultValue: formatToDate()
        },
        {
            field: '#LCD',
            component: 'Divider',
            label: 'LCD Product'
        },
        ...new Size3(optionsdata2,FactoryType,{size:"Size",optionproduct:"OptionProduct",selectmenu:"#optionmenu",product:"LCDProd"}).obj,
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
            }
        },
        {
            label: 'Product',
            field: 'LCDProd',
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
        ...new Grade(optionsdata2,FactoryType).obj,
        /* {
            field: 'LCDGRADE',
            label: 'LCD GRADE',
            component: 'Select',
            componentProps: {
                mode: 'multiple',
                options: LCDGradeOptions.map((item) => {
                    return { label: item, value: item };
                })
            },
            defaultValue: ['ALL']
        }, */
        {
            field: '#LCM',
            component: 'Divider',
            label: 'LCM Product'
        },
        ...new Size(optionsdata3,FactoryType,{size:"Size1",bigproduct:"BigProduct",optionproduct:"OptionProduct1",selectmenu:"#optionmenu1",product:"Product"}).obj,
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
        },
        {
            label: 'big product',
            component: 'Select',
            field: 'Product',
    
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
            // defaultValue: ''
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
        ...new Owner(optionsdata2,FactoryType,"LCMOwner").obj,
       /*  {
            label: 'LCM Owner',
            component: 'Select',
            field: 'LCMOwner',
            componentProps: {
                mode: 'multiple',
                options: OwnerName.map((item) => {
                    return { label: item, value: item };
                })
            },
            defaultValue: ['ALL']
        } */
    ];
    
}