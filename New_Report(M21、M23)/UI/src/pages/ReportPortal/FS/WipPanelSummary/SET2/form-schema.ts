import type { FormSchema } from '@/components/core/schema-form/';
import { HoldCode, LCMProductType, Operation, Size } from '@/pages/ReportPortal/Static/common';
import { reactive } from 'vue';
import {GrpOptions} from '../LCM1/form-schema'
export let optionsdata: any = reactive({
    ProductData: [],
    ProductData1: [],
    SizeData: [],
    BigProductData: [],
    BigProductData1: [],
    OperationLabel: [],
    LcmProdTypeData: [],
    HoldCodeData: []
});



export const schemas = (FactoryType:string)=>{
  return  [
        {
            field: 'StartHours',
            component: 'Input',
            label: 'Start Hours'
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
                        value: '1'
                    },
                    {
                        label: '2F',
                        value: '2'
                    },
                    {
                        label: '3F',
                        value: '3'
                    }
                ]
            },
            defaultValue: ''
        },
        {
            label: 'Work Order Like',
            component: 'Input',
            field: 'Workorderlike'
        },
        ...new LCMProductType(optionsdata,FactoryType).obj,
        ...new Size(optionsdata,FactoryType).getSize(),
        ...new Operation(optionsdata,FactoryType).obj,
        ...new HoldCode(optionsdata,FactoryType).obj,
        {
            label: '顯示欄位',
            field: 'Colname',
            component: 'Select',
            componentProps: {
                mode: 'multiple',
                options: GrpOptions
            }
        },
        {
          label: 'LCM product',
          component: 'InputTextArea',
          field: 'inputprod',
        },
    ];
    
}