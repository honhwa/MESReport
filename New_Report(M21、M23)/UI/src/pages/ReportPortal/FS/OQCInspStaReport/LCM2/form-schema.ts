import type { FormSchema } from '@/components/core/schema-form/';
import { LcmProductParams } from '@/pages/ReportPortal/Static/CommonParams'
import { formatToDateHour } from '@/utils/dateUtil';
import { formatToDate } from '@/utils/dateUtil';
import { reactive } from 'vue';
import dayjs from 'dayjs';
import { Size } from '@/pages/ReportPortal/Static/common';



export const schemas = (Fab): FormSchema[] => {
  let optionsdata = reactive({})
  return [
    {
      label: 'Date/Time',
      field: 'FromDate',
      component: 'DatePicker',
      componentProps: {
        valueFormat: 'YYYY-MM-DD'
      },
      defaultValue: formatToDate(),
      required: true
    },
    {
      label: '开始时间',
      field: 'FromTime',
      component: 'TimePicker',
      componentProps: {
        minuteStep: 30,
        format: 'HH:mm',
        valueFormat: 'HH:mm'
      },
      defaultValue: '07:30',
      required: true
    },
    {
      label: 'To',
      field: 'ToDate',
      component: 'DatePicker',
      componentProps: {
        valueFormat: 'YYYY-MM-DD'
      },
      defaultValue: formatToDate(),
      required: true
    },
    {
      label: '结束时间',
      field: 'ToTime',
      component: 'TimePicker',
      componentProps: {
        minuteStep: 30,
        format: 'HH:mm',
        valueFormat: 'HH:mm'
      },
      defaultValue: '07:30',
      required: true
    },
    {
      label: 'common.commonpages.Floor',
      component: 'RadioGroup',
      field: 'Floor',
      componentProps: {
        options: [{ label: 'ALL', value: '' }, { label: '1F', value: " and floor='1'" },
        { label: '2F', value: " and floor='2'" }, { label: '3F', value: " and floor='3'" },]
      },
      defaultValue: ''
    },
    ...new Size(optionsdata, 'LCM2').obj,
    {
      label: 'Operation',
      component: 'RadioGroup',
      field: 'Operation',
      componentProps: {
        options: [
          {
            label: 'ALL',
            value: ''
          },
          {
            label: '1700',
            value: " and work_ctr=1700"
          },
          {
            label: '1800',
            value: ' and work_ctr=1800'
          },
          {
            label: '1900',
            value: ' and work_ctr=1900'
          },
          {
            label: '1905',
            value: ' and work_ctr=1905'
          },
          {
            label: '1910',
            value: ' and work_ctr=1910'
          },
          {
            label: '2450',
            value: ' and work_ctr=2450'
          }
        ],
      },
      defaultValue: ''
    }
  ];
}

