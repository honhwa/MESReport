import type { FormSchema } from '@/components/core/schema-form/';
// import { GetProduct, GetBigProduct } from '@/api/pages/daylimove';
// import { message } from 'ant-design-vue';
import { reactive } from 'vue';


// export let optionsdata: any = reactive({
//   SizeData: [{ label: 'ALL', value: 'ALL' }],
//   ModuleData: [],
//   ProductData: [],
//   BigProductData: [],
//   ProductData1: [],
//   BigProductData1: [],
//   OwnerName: [],
//   LcdOwnerLabel: [{ label: 'ALL', value: '' }],
//   LcdGradeLabel: [{ label: 'ALL', value: '' }],
//   ModuleProductData: [],
//   ModuleProductData1: [],
//   OperationLabel: [],
//   CustomDate: [{ label: 'ALL', value: '' }],
//   HoldCodeLabel: [{ label: 'ALL', value: '' }],
//   SourceFabLabel: [{ label: 'ALL', value: '' }],
//   WoTypeBySapLabel: [{ label: 'ALL', value: '' }],
//   LCMBigEquipmentLabel: [],
//   LCMEquipment: [],
//   LCMEquipment1: [],
//   Tempoption: [],
// })


export interface SHSelectType {
  SizeData?: any[];
  ModuleData?: any[];
  ProductData?: any[];
  ProductData1?: any[];
  BigProductData1?: any[];
  BigProductData?: any[];
  OwnerName?: any[];
  LcdOwnerLabel?: any[];
  ModuleProductData?: any[];
  ModuleProductData1?: any[];
  OperationLabel?: any[];
  LcdGradeLabel?: any[];
  LcmGradeLabel?: any[];
  LCMDefectCodeLabel?: any[];
  DefectCodeLabel?: any[];
  SpcProdLabel?: any[];
  SpcEqptIdLabel?: any[];
  SpcGroupLabel?: any[];
  LcdSizeLabel?: any[];
  TimeData?: any[];

  LCMEDCEquipmentLabel?: any[]; 
  LCMEDCUnitTemp?: any[]; 
  LCMEDCUnit?: any[];
  LCMEDCItem?: any[];
  
  BL_sizeData?: any[];
  PART_NBRData?: any[];
  PART_NBRALLData?: any[];
  PART_NBRdefaultData?: any[];
  
  CustomData?: any[];
  HoldCodeLabel?: any[];
  SourceFabLabel?: any[];
  WoTypeBySapLabel?: any[];
  EquipmentLabel?: any[];
  LCMBigEquipmentLabel?: any[];
  LCMEquipment?: any[];
  LCMEquipment1?: any[];
  Tempoption?: any[];

  ///mod
  OperationForRWK?: any[];
  OperationForINS?: any[];



  SpcModuleNameLabel?: any[];
  SpcMeasToolLabel?: any[];
  SpcProcToolLabel?: any[];
  SpcLineLabel?: any[];
  SpcBeforeHoursLabel?: any[];
  SpcProdNumLabel?: any[];
  SpcPreLineLabel?: any[];
  SpcProdIDLabel?: any[];
  SpcOperLabel?: any[];
  SpcProductLabel?: any[];
  SpcProcessLabel?: any[];
  SpcPerLineLabel?: any[];
}




