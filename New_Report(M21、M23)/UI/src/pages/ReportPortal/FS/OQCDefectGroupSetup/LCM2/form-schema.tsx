import type { FormSchema } from '@/components/core/schema-form/';
// import { message } from 'ant-design-vue';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil'
import { Category} from '@/pages/ReportPortal/Static/common';

export let optionsdata: any = reactive({
  SizeData: [{ label: 'ALL', value: 'ALL' }],
  ProductData: [],
  BigProductData: [],
  ProductData1: [],
  BigProductData1: [],
  OwnerName:[],
  defectcode:[],
  Category:[],
})

let WotypeOptions = ['By日期查詢'];
export const schemas1 = (FactoryType: string) => {
  return [
    ...new Category(optionsdata, 'LCM2').obj,
    //  type 下拉单选框
    // {
    //   label: '品別',
    //   component: 'Select',
    //   field: 'SIZE1',
    //   componentProps: () => {
    //     return {
    //       options: optionsdata.Category,
    //     };
    //   },
    //   defaultValue:["ALL"]
    // },
    // {
    //   label: 'defectcode',
    //   component: 'Select',
    //   field: 'PART',
    //   componentProps: () => {
    //     return {
    //       mode: 'multiple',
    //       options: optionsdata.defectcode,
    //     };
    //   },
    //   defaultValue:["ALL"]
    // }
  ];
};
