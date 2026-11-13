import type { FormSchema } from '@/components/core/schema-form/';
// import { message } from 'ant-design-vue';
import { reactive, ref } from 'vue';
import { formatToDate } from '@/utils/dateUtil';
import { LCMProductType, Size } from '@/pages/ReportPortal/Static/common';

// 定义从后端返回的下拉选择框的数据
export let optionsdata: any = reactive({
    LcmProdTypeData: [],
    ProductData: [],
    ProductData1: [],
    SizeData: [],
    BigProductData: [],
    BigProductData1: []
});
let optionsdata2 = reactive({});
export const schemas = (FactoryType:string)=>{
    return [
        {
            label: '查询范围',
            component: 'RadioGroup',
            field: 'QueryRange',
            componentProps: {
                options: [
                    {
                        label: '全部',
                        value: ''
                    },
                    {
                        label: '全部已結案',
                        value: " and status  in ('2','3') "
                    },
                    {
                        label: '全部未結案',
                        value: " and status NOT IN ('2', '3') "
                    },
                    {
                        label: '大于60天未結案',
                        value: " and SF_A4R_GetDiffTime ('DD',substr(release_dt,1,10) ,'', to_char(current_Date,'YYYY-MM-DD'),'')>=60 and status<>'3' "
                    }
                ]
            },
            defaultValue: " and status NOT IN ('2', '3') "
        },
        {
            label: '工單類型',
            component: 'CheckboxGroup',
            field: 'WorkOrderType',
            componentProps: {
                options: [
                    {
                        label: 'ALL',
                        value: 'ALL'
                    },
                    {
                        label: 'P',
                        value: 'P'
                    },
                    {
                        label: 'E',
                        value: 'E'
                    },
                    {
                        label: 'R',
                        value: 'R'
                    },
                    {
                        label: 'M',
                        value: 'M'
                    }
                ]
            },
            defaultValue: ['ALL']
        },
        {
            label: '選擇工單Due Day',
            component: 'RadioGroup',
            field: 'isWoDueDay',
            componentProps: {
                options: [
                    {
                        label: '否',
                        value: 'N'
                    },
                    {
                        label: '是',
                        value: 'Y'
                    }
                ]
            },
            defaultValue: 'N'
        },
        {
            label: '工單Due Day',
            field: 'FromDate_DueDay',
            component: 'DatePicker',
            componentProps: {
                valueFormat: 'YYYY-MM-DD'
            },
            defaultValue: formatToDate(),
            required: true,
            dynamicDisabled: ({ formModel }) => formModel.isWoDueDay == 'N'
        },
        {
            label: 'To',
            field: 'ToDate_DueDay',
            component: 'DatePicker',
            componentProps: {
                valueFormat: 'YYYY-MM-DD'
                // disabled: true,
            },
            defaultValue: formatToDate(),
            dynamicDisabled: ({ formModel }) => formModel.isWoDueDay == 'N',
            required: true
        },
        {
            label: '選擇工單Start Day',
            component: 'RadioGroup',
            field: 'isWoStartDay',
            componentProps: {
                options: [
                    {
                        label: '否',
                        value: 'N'
                    },
                    {
                        label: '是',
                        value: 'Y'
                    }
                ]
            },
            defaultValue: 'N'
        },
        {
            label: '工單Start Day',
            field: 'FromDate_StartDay',
            component: 'DatePicker',
            componentProps: {
                valueFormat: 'YYYY-MM-DD'
            },
            defaultValue: formatToDate(),
            required: true,
            dynamicDisabled: ({ formModel }) => formModel.isWoStartDay == 'N'
        },
        {
            label: 'To',
            field: 'ToDate_StartDay',
            component: 'DatePicker',
            componentProps: {
                valueFormat: 'YYYY-MM-DD'
            },
            defaultValue: formatToDate(),
            required: true,
            dynamicDisabled: ({ formModel }) => formModel.isWoStartDay == 'N'
        },
        {
            label: 'Work order like',
            field: 'workOrderLike',
            component: 'InputTextArea'
        },
        {
            label: 'Work order',
            field: 'WorkOrder',
            component: 'InputTextArea'
        },
        ...new LCMProductType(optionsdata2, FactoryType, 'LCMProductType').obj,
        /* {
        label: 'LCM Product Type',
        component: 'Select',
        field: 'LCMProductType',
        componentProps: () => {
          return {
            options: optionsdata.LcmProdTypeData,
          };
        },
        defaultValue: 'ALL',
      }, */
        ...new Size(optionsdata2, FactoryType, {
            size: 'Size',
            bigproduct: 'bigProduct',
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
            },
            defaultValue: ['ALL']
        }, */
        {
            label: 'WO STATUS',
            field: 'wo_status',
            component: 'Select',
            componentProps: () => {
                return {
                    options: [
                        {
                            label: 'ALL',
                            value: 'ALL'
                        },
                        {
                            label: 'release',
                            value: 'release'
                        },
                        {
                            label: 'complete',
                            value: 'complete'
                        },
                        {
                            label: 'close',
                            value: 'close'
                        }
                    ]
                };
            },
            defaultValue: 'ALL'
        },
        {
            label: '投入率',
            field: 'mvin_rate',
            component: 'Select',
            componentProps: () => {
                return {
                    options: [
                        {
                            label: 'ALL',
                            value: 'ALL'
                        },
                        {
                            label: '小於1',
                            value: '小於1'
                        },
                        {
                            label: '等於1',
                            value: '等於1'
                        }
                    ]
                };
            },
            defaultValue: 'ALL'
        },
        {
            label: '完成率',
            field: 'mvou_rate',
            component: 'Select',
            componentProps: () => {
                return {
                    options: [
                        {
                            label: 'ALL',
                            value: 'ALL'
                        },
                        {
                            label: '小於1',
                            value: '小於1'
                        },
                        {
                            label: '等於1',
                            value: '等於1'
                        }
                    ]
                };
            },
            defaultValue: 'ALL'
        }
    ];
}
