import { ShiftParams2, FloorParams2, LCMOwnerParams } from '@/pages/ReportPortal/Static/CommonParams'
import { formatToDateHour } from '@/utils/dateUtil';
import { reactive } from 'vue';
import { Size } from '@/pages/ReportPortal/Static/common';


let Operation = ['ALL', '1700-D KEN', '1710-D2 KEN', '1800-PACKING', '1800-PACK CHANGE', '1910-WAREHOUSE']
let Operation_SET = ['ALL', '8550-OQC InLine檢', '8650-OBA1 OQC檢', '8656-OBA2 客驗']
let JudgementLabel = ['ALL', 'OK', 'Down Grade', 'Up Grade', 'NG']
let JudgementValue = ['1=1', "(rank1='OK')", "(q.grade_new > q.grade_old and rank1='NG')", "(q.grade_new < q.grade_old and rank1='NG')", "(q.grade_new = q.grade_old and rank1='NG/RW')"]


export const schemas = (Fab) => {
  let OptionsData: any[] = []
  let optionsdata = reactive({})
  if (Fab == "SET1") OptionsData = Operation_SET.map(item => { return { label: item, value: item.split('-')[0] } })
  else OptionsData = Operation.map(item => { return { label: item, value: item.split('-')[0] } })
  return [
    {
      label: 'common.commonpages.Date',
      component: 'DatePicker',
      field: 'CompleteFromDate',

      componentProps: {
        format: 'YYYY-MM-DD HH:mm',
        showTime: true,
        minuteStep: 30,
        valueFormat: 'YYYY-MM-DD HH:mm'
      },
      defaultValue: formatToDateHour()
    },
    {
      label: 'common.commonpages.To',
      component: 'DatePicker',
      field: 'CompleteToDate',

      componentProps: {
        format: 'YYYY-MM-DD HH:mm',
        showTime: true,
        minuteStep: 30,
        valueFormat: 'YYYY-MM-DD HH:mm'
      },
      defaultValue: formatToDateHour(),
    },
    ShiftParams2,
    FloorParams2,
    {
      field: 'LCMDivider',
      component: 'Divider',
      label: 'common.commonpages.LCMProduct'
    },
    ...new Size(optionsdata, Fab).obj,
    LCMOwnerParams,
    {
      label: 'common.commonpages.Operation',
      component: 'Select',
      field: 'Operation',
      componentProps: {
        options: OptionsData
      },
      defaultValue: 'ALL'

    },
    {
      label: 'common.commonpages.Judgement',
      component: 'Select',
      field: 'Judgement',
      componentProps: {
        mode: "multiple",
        options: JudgementLabel.map((item, index) => { return { label: item, value: JudgementValue[index] } })
      },
      defaultValue: ['1=1']

    },
    {
      label: 'common.commonpages.SN',//S/N
      component: 'InputTextArea',
      field: 'SN',
      defaultValue: ''

    }
  ]
};

