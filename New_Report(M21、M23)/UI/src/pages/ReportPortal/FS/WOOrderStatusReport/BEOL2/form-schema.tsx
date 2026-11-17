import type { FormSchema } from '@/components/core/schema-form/';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil'


// 定义从后端返回的下拉选择框的数据
export let optionsdata: any = reactive({
  LcmProdTypeData: [],
  ProductData:[],
  ProductData1:[],
  SizeData:[],
  BigProductData:[],
  BigProductData1:[],

})

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
            value: "   and status='3' "
          },
          {
            label: '全部未結案',
            value: " and status<>'3'  "
          },
          {
            label: '大于60天未結案',
            value: " and SF_A4R_GetDiffTime ('DD',substr(release_dt,1,10) ,'', to_char(current_Date,'YYYY-MM-DD'),'')>=4 and status<>'3' "
          },
        ],
      },
      defaultValue:" and status<>'3'  "
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
            label: 'HDB1EN',
            value: 'HDB1EN'
          },
          {
            label: 'HDBCRK',
            value: 'HDBCRK'
          },
          {
            label: 'HDB1MP',
            value: 'HDB1MP'
          },
          {
            label: 'HDB1RK',
            value: 'HDB1RK'
          },
          {
            label: 'HDBCEN',
            value: 'HDBCEN'
          },
          {
            label: 'HDBCMP',
            value: 'HDBCMP'
          },
          {
            label: '28015',
            value: '28015'
          },
          {
            label: '28773',
            value: '28773'
          },
          {
            label: '28774',
            value: '28774'
          },
          {
            label: '28204',
            value: '28204'
          },
          {
            label: '28793',
            value: '28793'
          },
          {
            label: '28032',
            value: '28032'
          },
          {
            label: '28207',
            value: '28207'
          },
          {
            label: '28208',
            value: '28208'
          },
          {
            label: '28036',
            value: '28036'
          },
        ]
      },
      defaultValue: ['ALL'],
    },
    {
      label: 'DEST_SHOP',
      component: 'CheckboxGroup',
      field:"DEST_SHOP",
      componentProps: {
        options: [
          {
            label: 'ALL',
            value: 'ALL'
          },
          {
            label: 'NHPA',
            value: 'NHPA'
          },
          {
            label: 'NHBD',
            value: 'NHBD'
          },
        ]
      },
      defaultValue: ['ALL'],
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
            value: "Y"
          },
        ],
      },
      defaultValue: 'N',
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
      },
      defaultValue: formatToDate(),
      required: true,
      dynamicDisabled: ({ formModel }) => formModel.isWoDueDay == 'N'
  
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
            value: "Y"
          },
        ],
      },
      defaultValue: 'N',
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
      component: "InputTextArea",
    },
    {
      label: 'Work order',
      field: 'WorkOrder',
      component: "InputTextArea",
    },
    {
      label: 'WO STATUS',
      field: 'wo_status',
      component: 'Select',
      componentProps: () => {
        return {
          options: [
            {
              label: "ALL",
              value: "ALL"
            },
            {
              label: "release",
              value: ""
            },
            {
              label: "complete",
              value: "3"
            }
          ]
        };
      },
      defaultValue: 'ALL',
    },
    {
      label: '投入率',
      field: 'mvin_rate',
      component: 'Select',
      componentProps: () => {
        return {
          options: [
            {
              label: "ALL",
              value: "ALL"
            },
            {
              label: "小於1",
              value: "小於1"
            },
            {
              label: "等於1",
              value: "等於1"
            },
          ]
        };
      },
      defaultValue: 'ALL',
    },
    {
      label: '完成率',
      field: 'mvou_rate',
      component: 'Select',
      componentProps: () => {
        return {
          options: [
            {
              label: "ALL",
              value: "ALL"
            },
            {
              label: "小於1",
              value: "小於1"
            },
            {
              label: "等於1",
              value: "等於1"
            },
          ]
        };
      },
      defaultValue: 'ALL',
    },
  ];
  
}