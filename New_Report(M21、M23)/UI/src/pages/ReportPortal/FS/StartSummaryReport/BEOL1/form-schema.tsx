import type { FormSchema } from '@/components/core/schema-form/';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil'


// 定义从后端返回的下拉选择框的数据
export let optionsdata: any = reactive({
  worderTypeData: [],
})



// 處理下拉選擇框api的數據
/* func：請求接口的方法名，params：請求接口的data的屬性名，funcparams：接口所需的參數 */
export async function getSizeData(func: Function, params: String, funcparams: {}) {
  let ReturnData: any = []
  const { Date: { Table: Data } } = await func(funcparams);

  for (let i in Data) {
    //@ts-ignore
    ReturnData.push({ label: Data[i][params], value: Data[i][params] });
  }
  ReturnData.unshift({ label: 'ALL', value: 'ALL' })
  return ReturnData

}

export const schemas: FormSchema[] = [
  {
    // label使用vue-i18n中英文的定义的变量
    label: 'From',
    // filed为v-model绑定的值
    field: 'date_f',
    // component的值是ant-design-vue组件的名字
    component: 'DatePicker',
    // 组件属性，里面使用的是ant-design-vue原生的属性
    componentProps: {
      valueFormat: 'YYYY-MM-DD'
    },
    // defaultValue为自定义的类型
    defaultValue: formatToDate(),
    required: true,
    colProps: {
      span: 8
    }
  },
  {
    label: 'To',
    field: 'date_t',
    component: 'DatePicker',
    componentProps: {
      valueFormat: 'YYYY-MM-DD'
    },
    defaultValue: formatToDate(),
    required: true,
    colProps: {
      span: 8
    }
  },
  // product type 下拉单选框
  {
    label: 'Product Type',
    component: 'Select',
    field: 'ProdTypes',
    componentProps: {
      options: [
        {
          label: "All",
          value: "ALL"
        },
        {
          label: "Prod+E→P+Eng",
          value: "PE"
        },
        {
          label: "Prod+E→P",
          value: "PX"
        },
        {
          label: "Product",
          value: "Prod"
        },
        {
          label: "E→P",
          value: "EXP"
        },
        {
          label: "Engineer",
          value: "Eng"
        },
        {
          label: "Qlot",
          value: "Qlot"
        },
        {
          label: "Dummy",
          value: "Dumy"
        },
      ]
    },
    defaultValue: 'PX',
    colProps: {
      span: 8
    }

  },
  // Query type 单选按钮
  {
    label: 'Query Type',
    component: 'RadioGroup',
    field: 'QueryType',
    componentProps: {
      options: [
        {
          label: 'By Daily',
          value: 'DAY'
        },
        {
          label: 'By EQ',
          value: "EQ"
        },
        {
          label: 'By Source fab',
          value: 'FA'
        }
      ],
    },
    defaultValue: 'DAY',
    colProps: {
      span: 8
    }
  },
  // worder type 下拉多选框   api
  {
    label: 'Worder Type',
    field: 'Selwo',
    component: 'Select',
    componentProps: () => {
      return {
        mode: 'multiple',
        options: optionsdata.worderTypeData,
        placeholder: 'Worder Type',
      };
    },
    defaultValue: ["ALL"],
    colProps: {
      span: 8
    }
  },


  // Report Format 单选按钮
  {
    label: 'Report Format',
    component: 'RadioGroup',
    field: 'ReportFormat',
    componentProps: {
      options: [
        {
          label: 'Summary By Effective Move-in',
          value: 'start'
        },
        {
          label: 'Summary By Move-in/Cancel Move-in',
          value: "startcancel"
        },
      ],
    },
    defaultValue: 'start',
    colProps: {
      span: 8
    }
  },

];
