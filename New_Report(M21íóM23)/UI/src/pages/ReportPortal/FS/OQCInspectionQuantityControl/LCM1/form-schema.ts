import type { FormSchema } from '@/components/core/schema-form/';
import { LcmProductParams } from '@/pages/ReportPortal/Static/CommonParams'
import { formatToDateHour } from '@/utils/dateUtil';
import { reactive } from 'vue';
import { Size } from '@/pages/ReportPortal/Static/common';



export const schemas = (Fab): FormSchema[] => {
  let optionsdata = reactive({})
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
    {
      label: 'common.commonpages.Shift',
      component: 'RadioGroup',
      field: 'Shift',

      componentProps: {
        options: [
          {
            label: 'ALL',
            value: ''
          },
          {
            label: '日(A)',
            value: "and shift_id='A'"
          },
          {
            label: '夜(B)',
            value: "and shift_id='B'"
          }
        ],
      },
      defaultValue: ''
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
    {
      field: 'LCMDivider',
      component: 'Divider',
      label: 'common.commonpages.LCMProduct'
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
      defaultValue: 'N'
    },
    {
      label: 'common.commonpages.ModelType',
      component: 'RadioGroup',
      field: 'ModelType',
      componentProps: {

        options: [
          {
            label: 'ALL',
            value: ''
          },
          {
            label: 'TV MOD-UD(4K2K)',
            value: "and substr(a.prod_nbr,1,1)='G' and substr(a.prod_nbr,6,1)='D' "
          },
          {
            label: 'TV MOD-FHD ',
            value: "and substr(a.prod_nbr,1,1)='G' and substr(a.prod_nbr,6,1)<>'D' "
          },
          {
            label: 'TV Open cell-UD(4K2K)',
            value: "and substr(a.prod_nbr,1,1)='2' and substr(a.prod_nbr,6,1)='D' "
          }, {
            label: 'TV Open cell FHD ',
            value: "and substr(a.prod_nbr,1,1)='2' and substr(a.prod_nbr,6,1)<>'D' "
          }
        ],
      },
      defaultValue: ''

    },
    {
      label: 'common.commonpages.InspectionSite',
      component: 'RadioGroup',
      field: 'InspectionSite',
      componentProps: ({ formModel, field }) => {
        return {
          options: [
            {
              label: '6500(OQC最終判退)',
              value: "Aand work_ctr='1760'"
            },
            {
              label: '1760(OQC初判不良)',
              value: "Band work_ctr='1760'"
            },
            {
              label: '1850(特殊撕膜檢不良，非廠方責任)',
              value: "Band work_ctr='1850'"
            }
          ],
          // onChange: () => {
          //   if (['A','B'].includes(formModel[field])) {
          //     formModel['InspectionSiteData'] = "and work_ctr ='1760'"
          //   }  else {
          //     // formModel[field] = 'B'
          //     formModel['InspectionSiteData'] = "and work_ctr ='1850'"
          //   }
          // }
        }
      },
      defaultValue: "Aand work_ctr='1760'"

    },
    {
      label: '',
      component: 'div',
      field: 'InspectionSiteData',
      vShow: false
    },
    {
      label: 'common.commonpages.OBAInspectionType',
      component: 'RadioGroup',
      field: 'OBAInspectionType',
      componentProps: {
        options: [
          {
            label: 'ALL',
            value: ''
          },
          {
            label: 'JQC檢',
            value: "JQC"
          },
          {
            label: 'VOE檢',
            value: 'VOE'
          },
          {
            label: 'OBA檢',
            value: 'OBA'
          }
        ],
      },
      defaultValue: ''

    }
  ];
}

