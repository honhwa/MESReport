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
      defaultValue: '08:00',
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
      defaultValue: '08:00',
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
    ...new Size(optionsdata, 'LCM1').obj,
    {
      label: 'common.commonpages.Reworked',
      component: 'Select',
      field: 'Reworked',
      componentProps: {
        options: [
          {
            label: 'ALL',
            value: ''
          },
          {
            label: '1次',
            value: "1"
          },
          {
            label: '1次以上',
            value: "over_1"
          },
          {
            label: '2次',
            value: "2"
          },
          {
            label: '2次以上',
            value: "over_2"
          },
          {
            label: '3次',
            value: "3"
          },
          {
            label: 'No',
            value: "N"
          },
        ],
      },
      defaultValue: ''
    },
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
            label: '1710',
            value: ' and work_ctr=1710'
          },
          {
            label: '1800',
            value: ' and work_ctr=1800'
          },
          {
            label: '1850',
            value: ' and work_ctr=1850'
          },
          {
            label: '1910',
            value: ' and work_ctr=1910'
          }
        ],
      },
      defaultValue: ''
    }
  ];
}

