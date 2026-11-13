import type { FormSchema } from '@/components/core/schema-form/';
// import { GetProduct, GetBigProduct } from '@/api/pages/daylimove';
// import { message } from 'ant-design-vue';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil'
import useRemoteSelect from '@/pages/ReportPortal/utils/NGBuseRemoteSelect';
import { AddCommonPageParams } from '@/store/module/commonpage';
import {LcmBySizeByModuleProdParams,FromDateParams,ToDateParams,InputWorkorderParams,FloorParams,
  OperationParams,ShiftParams,LcmBySizeProdParams,CustomerParams,LcmOwnerParams,LcdOwnerParams,IncludeBLParams} from '@/pages/ReportPortal/Static/NGBCommonParams'


const useCommonParams = AddCommonPageParams();
const { getCommonFormParams } = useRemoteSelect()



export const schemas = (FactoryType:string,optionsdata:any)=>{
  return [ 
  ...FromDateParams,
  ...ToDateParams,
  ...ShiftParams,
  ...IncludeBLParams,
  ...FloorParams,
  ...InputWorkorderParams,
  ...new LcmBySizeByModuleProdParams(optionsdata,FactoryType).getSize(),
  ...new LcmOwnerParams(optionsdata,FactoryType)._obj,
  ...new LcdOwnerParams(optionsdata,FactoryType)._obj,
];
}
