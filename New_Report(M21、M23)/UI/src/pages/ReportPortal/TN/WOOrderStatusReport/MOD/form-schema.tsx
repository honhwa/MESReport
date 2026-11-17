import type { FormSchema } from '@/components/core/schema-form/';
// import { message } from 'ant-design-vue';
import { reactive, ref } from 'vue';
import { formatToDate } from '@/utils/dateUtil'


export const schemas = (FactoryType:string)=>{ 
  let optionsdata = reactive({});
  return [
    {
      label: 'common.commonpages.QueryType',
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
            value: " and status='3' "
          },
          {
            label: '全部未結案',
            value: " and status<>'3' "
          },
          {
            label: '大於60天未結案',
            value: " and TIMESTAMPDIFF (16,CHAR(current timestamp-release_dt))>=60 and status<>'3' "
          },
        ],
      },
      defaultValue: " and status<>'3' ",
    },
    {
      label: 'common.commonpages.WorkType',
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
          },
          {
            label: 'S',
            value: 'S'
          },
        ]
      },
      defaultValue: ['ALL'],
    },
    {
      label: 'common.commonpages.WorkDueDayFlag',
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
        onChange: async (e: any) => {
        if(e.target.value == "Y"){
          schemas[3]["componentProps"]["disabled"] = false;
        }else{
          schemas[3]["componentProps"]["disabled"] = true;
        }
          

        }
      },
      defaultValue: 'N',
    },
    {
      label: 'common.commonpages.SelectWorkDueDay',
      field: 'FromDate_DueDay',
      component: 'DatePicker',
      componentProps: {
        valueFormat: 'YYYY-MM-DD',
        // disabled: true,
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
        valueFormat: 'YYYY-MM-DD',
        // disabled: true,
      },
      defaultValue: formatToDate(),
      dynamicDisabled: ({ formModel }) => formModel.isWoDueDay == 'N',
      required: true,
    },
    {
      label: 'common.commonpages.WorkStartDayFlag',
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
      label: 'common.commonpages.SelectWorkStartDay',
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
      label: 'common.commonpages.WorkOrder',
      field: 'WorkOrder',
      component: "InputTextArea",
    },
    {
      label: 'common.commonpages.WorkStatus',
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
              value: "release"
            },
            {
              label: "complete",
              value: "complete"
            },
            {
              label: "close",
              value: "close"
            },
          ]
        };
      },
      defaultValue: 'ALL',
    },
    {
      label: 'common.commonpages.InputRatio',
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
      label: 'common.commonpages.FinishRatio',
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
};