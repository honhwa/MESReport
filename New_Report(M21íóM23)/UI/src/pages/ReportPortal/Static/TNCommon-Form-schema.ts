import type { FormSchema } from '@/components/core/schema-form/';
// import { GetProduct, GetBigProduct } from '@/api/pages/daylimove';
// import { message } from 'ant-design-vue';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil'
import useRemoteSelect from '@/pages/ReportPortal/utils/TNuseRemoteSelect';
import { AddCommonPageParams } from '@/store/module/commonpage';

const useCommonParams = AddCommonPageParams();
const { getCommonFormParams } = useRemoteSelect()
let ifSelectall=false// 是否全選標記
let ifProdChoose="bysize"//料號查詢是是否切換bysize by module（默認false）

export let optionsdata: any = reactive({
  TN_SizeData: [{ label: 'ALL', value: 'ALL' }],
  TN_ProductData: [],
  TN_BigProductData: [],
  TN_Opername: [],
  TN_LCMOwner:[],
  TN_ProductData1: [],
  TN_BigProductData1: [],
  TN_Opername1: [],
  TN_OperationLabel: [],
  TN_LcdGrade: [],
  TN_LcdOwner: [],
  TN_HoldCode: [],
  TN_PanelSource: [],
  TN_DefectCode: [],
  TN_OperationByDownGradeLabel: [],
  TN_OperationByDownGradeL2Label: [],
  TN_ErrorCode: [],
  TN_EQPGroup: [],
  TN_EQPID: [],
  TN_EQPID1: [],
})





