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

export const schemas= (FactoryType:string)=>{
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
            value: " and status in ('2','3') "
          },
          {
            label: '全部未結案',
            value: " and status not in ('2','3') "
          },
          {
            label: '大于60天未結案',
            value: " and SF_A4R_GetDiffTime ('DD',substr(release_dt,1,10) ,'', to_char(current_Date,'YYYY-MM-DD'),'')>=60 and status not in ('2','3') "
          },
        ],
      },
      defaultValue:" and status not in ('2','3') ",
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
            label: 'MP',
            value: 'MP'
          },
          {
            label: 'UP',
            value: 'UP'
          },
          {
            label: 'UR',
            value: 'UR'
          },
          {
            label: 'EN',
            value: 'EN'
          },
          {
            label: 'MW',
            value: 'MW'
          },
          {
            label: 'UH',
            value: 'UH'
          },
          {
            label: 'ZH',
            value: 'ZH'
          },
          {
            label: 'ZR',
            value: 'ZR'
          },
          {
            label: '28761',
            value: '28761'
          },
          {
            label: '28011',
            value: '28011'
          },
          {
            label: '28763',
            value: '28763'
          },
          {
            label: '28018',
            value: '28018'
          },
          {
            label: '28201',
            value: '28201'
          },
          {
            label: '28791',
            value: '28791'
          },
          {
            label: '28019',
            value: '28019'
          },
          {
            label: '28037',
            value: '28037'
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
