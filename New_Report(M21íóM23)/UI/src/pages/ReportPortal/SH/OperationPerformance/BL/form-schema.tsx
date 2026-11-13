import type { FormSchema } from '@/components/core/schema-form/';
// import { GetProduct, GetBigProduct } from '@/api/pages/daylimove';
// import { message } from 'ant-design-vue';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil'
import useRemoteSelect from '@/pages/ReportPortal/utils/SHuseRemoteSelect';
import { AddCommonPageParams } from '@/store/module/commonpage';
import {LcmBySizeByModuleProdParams,FromDateParams,ToDateParams,InputWorkorderParams,FloorParams,
  OperationParams,ShiftParams,BLBySizeProdParams,CustomerParams,LcmOwnerParams,LcdOwnerParams,IncludeBLParams} from '@/pages/ReportPortal/Static/SHCommonParams'


const useCommonParams = AddCommonPageParams();
const { getCommonFormParams } = useRemoteSelect()



export const schemas = (FactoryType:string,optionsdata:any)=>{
  return [ 
  ...FromDateParams,
  ...ToDateParams,
  ...ShiftParams,
  ...new LcmOwnerParams(optionsdata,FactoryType)._obj,
  ...new BLBySizeProdParams(optionsdata,FactoryType).getSize(),
];
}
