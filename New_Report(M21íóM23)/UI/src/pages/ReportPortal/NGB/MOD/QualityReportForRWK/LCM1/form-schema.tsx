import type { FormSchema } from '@/components/core/schema-form/';
import { reactive, computed } from 'vue';
import { formatToDate } from '@/utils/dateUtil';
import { useI18n } from '@/hooks/useI18n';
import useRemoteSelect from '@/pages/ReportPortal/utils/NGBuseRemoteSelect';
import { AddCommonPageParams } from '@/store/module/commonpage';

import {
  LcmBySizeByModuleProdParams,
  FromDateParams,
  ToDateParams,
  InputWorkorderParams,
  FloorParams,
  WoTypeBySapParams,
  LcmEquipmentParams,
  OperationParams,
  ShiftParams,
  LcdOwnerParams,
  ReworkedParams,
  LcmOwnerParams,
  SourceFabParams,
  IncludeBLParams,
  LcdGradeParams,
  
  LCMDefectCodeParams,
  LCMDefectCodeNoParams,
  ReworkedOptionParams,
  ReworkedTimesParams,
  OperationForINSParams,
  OperationForRWKParams,
  InputOperatorParams,
} from '@/pages/ReportPortal/Static/NGBCommonParams';
const useCommonParams = AddCommonPageParams();
const { getCommonFormParams } = useRemoteSelect();

const { t } = useI18n();
export let SpecialCodeOptions = [{ value: 'PCZCV', label: 'PCZCV' }];

export const schemas = (FactoryType: string, optionsdata: any) => {
  return [
    {
      label: 'FAB Source',
      component: 'RadioGroup',
      field: 'FAC',
      componentProps: ({ formModel, formInstance }) => {
        return {
          options: [
            {
              label: 'NC2',
              value: 'LCM1',
            },
            {
              label: 'NB2',
              value: 'LCM2',
            },
          ],
          onChange: async (e: any) => {
            FactoryType = formModel.FAC;

            if (FactoryType == 'LCM1') {
              useCommonParams.G_FAC_COM = 'C';
            } else if (FactoryType == 'LCM2') {
              useCommonParams.Fab = 'LCM2';
              useCommonParams.G_FAC_COM = 'B';
            } else if (FactoryType == 'LCM3') {
              useCommonParams.Fab = 'LCM3';
              useCommonParams.G_FAC_COM = 'E';
            } else if (FactoryType == 'LCM4') {
              useCommonParams.G_FAC_COM = 'D';
            }
            formModel.Operation = undefined;
            const { updateSchema } = formInstance;
            updateSchema(
              ...new OperationForRWKParams(optionsdata, FactoryType)._obj,
              ...new LcmBySizeByModuleProdParams(optionsdata, FactoryType).getSize(),
              ...new SourceFabParams(optionsdata, FactoryType)._obj,
              ...new LcmOwnerParams(optionsdata, FactoryType)._obj,
              //...new ReworkedOptionParams(optionsdata, FactoryType)._obj,
              ...new OperationForINSParams(optionsdata, FactoryType)._obj,
              ...new LcmEquipmentParams(optionsdata, FactoryType)._obj,
              ...new LCMDefectCodeParams(optionsdata, FactoryType)._obj,             
              ...new LCMDefectCodeNoParams(optionsdata, FactoryType)._obj,
              ...new LcdGradeParams(optionsdata, FactoryType)._obj,


            );
          },
        };
      },
      defaultValue: 'LCM1',
    },

    ...FromDateParams,
    ...ToDateParams,
    ...ShiftParams,
    //...new OperationForRWKParams(optionsdata, FactoryType)._obj,
    // {
    //   label: 'common.commonpages.DisplayModel',
    //   component: 'RadioGroup',
    //   field: 'ShowMode',
    //   componentProps: {
    //     options: [
    //       {
    //         label: computed(() => t('common.commonpages.Operation')), //label: '站點',
    //         value: 'work_ctr',
    //       },
    //       {
    //         label: '機台別',
    //         value: 'equip_nbr',
    //       },
    //       {
    //         label: '大機種',
    //         value: 'big_prod',
    //       },
    //       {
    //         label: '人員別',
    //         value: 'operators',
    //       },
    //     ],
    //   },
    //   defaultValue: 'work_ctr',
    // },
    ...FloorParams,
    ...new LcmBySizeByModuleProdParams(optionsdata, FactoryType).getSize(),
    ...new SourceFabParams(optionsdata, FactoryType)._obj,
    {
      label: 'common.commonpages.DisplayModel',
      component: 'RadioGroup',
      field: 'ShowMode',
      componentProps: {
        options: [
          {
            label: computed(() => t('common.commonpages.Operation')), //label: '站點',
            value: 'work_ctr',
          },
          {
            label: '機台別',
            value: 'equip_nbr',
          },
          {
            label: '大機種',
            value: 'big_prod',
          },
          {
            label: '人員別',
            value: "operators"
          },
        ],
      },
      defaultValue: 'work_ctr',
    },

     ...IncludeBLParams,
    ...InputWorkorderParams,
    ...InputOperatorParams,
    ...new LcmOwnerParams(optionsdata, FactoryType)._obj,
    
      ReworkedOptionParams,
       ReworkedTimesParams,
    ...new OperationForINSParams(optionsdata, FactoryType)._obj,
    // 站點參數更換
    ...new OperationForRWKParams(optionsdata, FactoryType)._obj,
    ...new LcmEquipmentParams(optionsdata, FactoryType)._obj,
    ...new LCMDefectCodeParams(optionsdata, FactoryType)._obj,
   // 不含DEfect
    ...new LCMDefectCodeNoParams(optionsdata, FactoryType)._obj,
    ...new LcdGradeParams(optionsdata, FactoryType)._obj,

    {
      label: '過濾掉的Defect Code',
      field: 'SpecialCode',
      //component: 'Select',
      component: 'CheckboxGroup',
      componentProps: () => {
        return {
          //mode: "multiple",
          options: SpecialCodeOptions,
        };
      },
      defaultValue: ['ALL'],
    },
  ];
};
