import type { FormSchema } from '@/components/core/schema-form/';
// import { GetProduct, GetBigProduct } from '@/api/pages/daylimove';
// import { message } from 'ant-design-vue';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil';
import useRemoteSelect from '@/pages/ReportPortal/utils/NGBuseRemoteSelect';
import { AddCommonPageParams } from '@/store/module/commonpage';
import {
  LcmBySizeByModuleProdParams,
  FromDateParams,
  ToDateParams,
  InputWorkorderParams,
  FloorParams,
  OperationParams,
  ShiftParams,
  LcmBySizeProdParams,
  CustomerParams,
  LcmOwnerParams,
  LcdOwnerParams,
  IncludeBLParams,
} from '@/pages/ReportPortal/Static/NGBCommonParams';

const useCommonParams = AddCommonPageParams();
const { getCommonFormParams } = useRemoteSelect();

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
              label: 'ND2',
              value: 'LCM3',
            },
            {
              label: 'ND3',
              value: 'LCM4',
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

            const { updateSchema } = formInstance;
            updateSchema(
              ...new LcmBySizeByModuleProdParams(optionsdata, FactoryType).getSize(),
              ...new LcmOwnerParams(optionsdata, FactoryType)._obj,
              ...new LcdOwnerParams(optionsdata, FactoryType)._obj,
            );
          },
        };
      },
      defaultValue: 'LCM3',
    },
    ...FromDateParams,
    ...ToDateParams,
    ...ShiftParams,
    ...IncludeBLParams,
    ...FloorParams,
    ...InputWorkorderParams,
    ...new LcmBySizeByModuleProdParams(optionsdata, FactoryType).getSize(),
    ...new LcmOwnerParams(optionsdata, FactoryType)._obj,
    ...new LcdOwnerParams(optionsdata, FactoryType)._obj,
  ];
};
