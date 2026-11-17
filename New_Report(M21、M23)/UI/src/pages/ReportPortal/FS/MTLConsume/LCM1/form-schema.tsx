import type { FormSchema } from '@/components/core/schema-form/';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil'
import { Equipment, Size } from '@/pages/ReportPortal/Static/common';


// 定义从后端返回的下拉选择框的数据
export let optionsdata: any = reactive({
  LcmProdTypeData: [],
  ProductData: [],
  ProductData1: [],
  SizeData: [],
  BigProductData: [],
  BigProductData1: [],

  EquipmentData: [],
  BigEquipmentData: [],
  BigEquipmentData1: [],

  MatTypeData: [],
  BigMatTypeData: [],
  BigMatTypeData1: [],
  BigMatTypeData2: [],


})
let optiondata2 = reactive({});
export const schemas = (FactoryType:string)=>{
  return [
    {
      label: 'Date',
      field: 'FromDate',
      component: 'DatePicker',
      componentProps: {
        valueFormat: 'YYYY-MM-DD'
      },
      defaultValue: formatToDate(),
      required: true,
    },
    {
      label: 'To',
      field: 'ToDate',
      component: 'DatePicker',
      componentProps: {
        valueFormat: 'YYYY-MM-DD'
      },
      defaultValue: formatToDate(),
      required: true,
    },
    {
      label: 'Shift',
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
      label: '查詢類型',
      component: 'RadioGroup',
      field: 'QueryType',
      componentProps: {
        options: [
          {
            label: 'ALL',
            value: '1'
          },
          {
            label: 'Lot number',
            value: "2"
          }
        ],
      },
      defaultValue: '1'
    },
    // 用戶要求刪除此欄位，sql并沒有使用
   /*  {
      label: 'Work order like',
      field: 'WorkOrderlike',
      component: "InputTextArea",
    }, */
    ...new Size(optiondata2,"LCM1",{
      size: 'Size',
      bigproduct: 'bigProduct',
      optionproduct: 'OptionProduct',
      selectmenu: '#optionmenu',
      product: 'Product'
    }).obj,
    /* {
      label: 'size',
      component: 'Select',
      field: 'Size',
      componentProps: () => {
        return {
          options: optionsdata.SizeData,
          placeholder: 'size',
          onChange: async (e: any) => {
  
            if (e !== 'ALL') {
              optionsdata.BigProductData1 = optionsdata.BigProductData.filter(item => item?.value.indexOf(e) != -1);
  
            } else {
              optionsdata.BigProductData1 = optionsdata.BigProductData
            }
  
          },
        };
      },
      defaultValue: 'ALL'
    },
    {
      label: 'big product',
      component: 'Select',
      field: 'bigProduct',
      componentProps: () => {
        return {
          options: optionsdata.BigProductData1,
          placeholder: 'big product',
          onChange: async (e: any) => {
            if (e !== 'ALL') {
              optionsdata.ProductData1 = optionsdata.ProductData.filter(item => item?.value.indexOf(e) != -1)
            } else {
              optionsdata.ProductData1 = optionsdata.ProductData
            }
          }
        };
      },
      defaultValue: 'ALL'
    },
    {
      label: 'Product',
      field: 'Product',
      component: 'Select',
      componentProps: () => {
        return {
          mode: 'multiple',
          options: optionsdata.ProductData1,
          placeholder: 'product',
        };
      },
      defaultValue: ['ALL']
  
    }, */
    ...new Equipment(optiondata2,"LCM1",0,{
      equip: 'equip',
      Optionequip: 'Optionequip',
      equipselectmenu: '#optionequipmenu',
      equipment: 'Equipment'
    }).obj,
   /*  {
      label: 'Equipment',
      component: 'Select',
      field: 'equip',
      componentProps: () => {
        return {
          // mode: 'multiple',
          options: optionsdata.EquipmentData,
          onChange: async (e: any) => {
            if (e !== 'ALL') {
              optionsdata.BigEquipmentData1 = optionsdata.BigEquipmentData.filter(item => item?.value.indexOf(e) != -1)
            } else {
              optionsdata.BigEquipmentData1 = optionsdata.BigEquipmentData
            }
          }
        };
      },
      defaultValue: ["ALL"],
    },
    {
      label: 'Big Equipment',
      component: 'Select',
      field: 'Equipment',
      componentProps: () => {
        return {
          mode: 'multiple',
          options: optionsdata.BigEquipmentData1,
        };
      },
      defaultValue: ["ALL"],
    }, */
  
    {
      label: 'MAT_TYPE:',
      component: 'Select',
      field: 'MAT_TYPE',
      componentProps: (model) => {
        return {
          options: optionsdata.MatTypeData,
          onChange: async (e: any) => {
            if (e !== 'ALL') {
              let temp: any[] = [];
              optionsdata.BigMatTypeData.forEach(item => {
                if (item["MAT_TYPE"] == e) {
                    temp.push({
                      label: item["PART_NBR"],
                      value: item["PART_NBR"],
                    })
                  
                }
              });
              optionsdata.BigMatTypeData1 = temp;
              model.formModel["PART_NBR"] = model.formModel["PART_NBR"].filter(
                (item) => item != 'ALL'
            );
            } else {
              optionsdata.BigMatTypeData1 = optionsdata.BigMatTypeData2
            }
  
          },
        };
      },
      defaultValue: ["ALL"]
    },
    {
      label: 'PART_NBR:',
      component: 'Select',
      field: 'PART_NBR',
      componentProps: () => {
        return {
          mode: 'multiple',
          options: optionsdata.BigMatTypeData1,
        };
      },
      defaultValue: ["ALL"]
    },
    {
      label: '替代用料',
      field: 'AlternativeMaterials',
      component: "InputTextArea",
    },
  ];
  
}