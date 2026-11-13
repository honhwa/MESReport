import type { FormSchema } from '@/components/core/schema-form/';
import { HoldCode, LCMProductType, Operation, Size } from '@/pages/ReportPortal/Static/common';
import { reactive } from 'vue';
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

export let GrpOptions = [
  { value: 'ALL', label: '全选' },
    { value: 'prod_nbr', label: 'Lcm Product',dataIndex:"NVL(RTRIM(PROD_NBR),'')" },

    { value: 'lcm_owner', label: 'Lcm Owner',dataIndex:"LCM_OWNER" },

    { value: 'work_ctr', label: 'Operation',dataIndex:"WORK_CTR" },

    { value: 'hold_flag', label: 'Hold',dataIndex:"HOLD_FLAG" },

    { value: 'hold_code', label: 'Hold_code',dataIndex:"HOLD_CODE" },

    { value: 'b.c_Desc', label: 'Hold Desc',dataIndex:"C_DESC" },

    { value: 'panel_size', label: 'Size',dataIndex:"PANEL_SIZE" },

    { value: 'wo_nbr', label: 'Work Order',dataIndex:"NVL(RTRIM(WO_NBR),'')" },

    { value: 'LCD_PROD_NBR ', label: 'Lcd Product',dataIndex:"LCD_PROD_NBR" },

    { value: 'lcd_grade', label: 'Lcd Grade',dataIndex:"LCD_GRADE" },

    { value: 'cur_hold_comment', label: 'Hold comment',dataIndex:"CUR_HOLD_COMMENT" },

    { value: 'due_dt', label: 'Due Day',dataIndex:"DUE_DT" }
];

export const schemas = (FactoryType:string)=>{
    return [
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
            componentProps: (model) => {
              return {
                mode: 'multiple',
                options: GrpOptions,
                onSelect: (e: any, option) => {
                  if (e == 'ALL') {
                      model.formModel["Colname"] = GrpOptions.map((item) => item.value).filter((item) => item != 'ALL');
                  } 
              }
              };
            },
          },
        {
          label: 'LCM product',
          component: 'InputTextArea',
          field: 'inputprod',
        },
    ];
    
}