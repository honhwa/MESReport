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

export const schemas = (FactoryType: object) => {
  return [
    {
      // label使用vue-i18n中英文的定义的变量
      label: 'ChartID',
      // filed为v-model绑定的值
      field: 'ChartID',
      // component的值是ant-design-vue组件的名字
      component: 'Input',
      // 组件属性，里面使用的是ant-design-vue原生的属性
      // defaultValue为自定义的类型
      defaultValue: FactoryType['ONCHID'],
      dynamicDisabled: true,
      colProps: {
        span: 8
      }
    },
    {
      label: 'Chart Type',
      field: 'ChartType',
      component: 'Select',
      componentProps: {
        options: [
          {
            label: "AGRG",
            value: "AGRG"
          },
          {
            label: "XXRM",
            value: "XXRM"
          },
          {
            label: "C_CHAR",
            value: "C_CHAR"
          }
        ]
      },
      defaultValue: 'AGRG',
      colProps: {
        span: 8
      }
    },
    {
      // label使用vue-i18n中英文的定义的变量
      label: 'Owner',
      // filed为v-model绑定的值
      field: 'Owner',
      // component的值是ant-design-vue组件的名字
      component: 'Input',
      // 组件属性，里面使用的是ant-design-vue原生的属性
      // defaultValue为自定义的类型
      defaultValue: FactoryType['COWNER'],
      dynamicDisabled: true,
      colProps: {
        span: 8
      }
    },
    {
      // label使用vue-i18n中英文的定义的变量
      label: 'Decription',
      // filed为v-model绑定的值
      field: 'Decription',
      // component的值是ant-design-vue组件的名字
      component: 'Input',
      // 组件属性，里面使用的是ant-design-vue原生的属性
      // defaultValue为自定义的类型
      defaultValue: FactoryType['ONCHID'],
      colProps: {
        span: 8
      }
    },
    {
      // label使用vue-i18n中英文的定义的变量
      label: 'UpDate All',
      // filed为v-model绑定的值
      field: 'UpDateAll',
      // component的值是ant-design-vue组件的名字
      component: FactoryType['ONCHID'],
      // 组件属性，里面使用的是ant-design-vue原生的属性
      // defaultValue为自定义的类型
      colProps: {
        span: 8
      }
    },
    {
      // label使用vue-i18n中英文的定义的变量
      label: ' ',
      // filed为v-model绑定的值
      field: 'UpDateAll123',
      // component的值是ant-design-vue组件的名字
      component: '',
      // 组件属性，里面使用的是ant-design-vue原生的属性
      // defaultValue为自定义的类型
      colProps: {
        span: 8
      }
    },
    // product type 下拉单选框
    {
      label: 'Data Type',
      component: 'Select',
      field: 'DataType',
      componentProps: {
        options: [
          {
            label: "Measure",
            value: "Measure"
          },
          {
            label: "Process",
            value: "Process"
          }
        ]
      },
      defaultValue: 'Measure',
      colProps: {
        span: 6
      }
    },
    {
      // label使用vue-i18n中英文的定义的变量
      label: 'Measure SEQ',
      // filed为v-model绑定的值
      field: 'MeasureSEQ',
      // component的值是ant-design-vue组件的名字
      component: 'Input',
      // 组件属性，里面使用的是ant-design-vue原生的属性
      // defaultValue为自定义的类型
      defaultValue: FactoryType['MES_ID'],
      dynamicDisabled: true,
      colProps: {
        span: 6
      }
    },
    {
      // label使用vue-i18n中英文的定义的变量
      label: 'Model Tool',
      // filed为v-model绑定的值
      field: 'ModelTool',
      // component的值是ant-design-vue组件的名字
      component: 'Input',
      // 组件属性，里面使用的是ant-design-vue原生的属性
      // defaultValue为自定义的类型
      defaultValue: FactoryType['CTOOLID'],
      colProps: {
        span: 6
      }
    },
    {
      // label使用vue-i18n中英文的定义的变量
      label: 'Pre_ToolID',
      // filed为v-model绑定的值
      field: 'Pre_ToolID',
      // component的值是ant-design-vue组件的名字
      component: 'Input',
      // 组件属性，里面使用的是ant-design-vue原生的属性
      // defaultValue为自定义的类型
      defaultValue: FactoryType['PEQPT_ID'],
      dynamicDisabled: true,
      colProps: {
        span: 6
      }
    },
    {
      label: 'Data Unit',
      component: 'Select',
      field: 'DataUnit',
      componentProps: {
        options: [
          {
            label: "S",
            value: "S"
          },
          {
            label: "C",
            value: "C"
          }
        ]
      },
      defaultValue: 'S',
      colProps: {
        span: 6
      }
    },
    {
      // label使用vue-i18n中英文的定义的变量
      label: 'MeasureGroup',
      // filed为v-model绑定的值
      field: 'MeasureGroup',
      // component的值是ant-design-vue组件的名字
      component: 'Input',
      // 组件属性，里面使用的是ant-design-vue原生的属性
      // defaultValue为自定义的类型
      defaultValue: FactoryType['DATA_GROUP'],
      dynamicDisabled: true,
      colProps: {
        span: 6
      }
    },
    {
      // label使用vue-i18n中英文的定义的变量
      label: 'P2P ToolID',
      // filed为v-model绑定的值
      field: 'P2PToolID',
      // component的值是ant-design-vue组件的名字
      component: 'Input',
      // 组件属性，里面使用的是ant-design-vue原生的属性
      // defaultValue为自定义的类型
      defaultValue: FactoryType['P2PTOOL'],
      colProps: {
        span: 6
      }
    },
    {
      // label使用vue-i18n中英文的定义的变量
      label: 'ToolID',
      // filed为v-model绑定的值
      field: 'ToolID',
      // component的值是ant-design-vue组件的名字
      component: 'Input',
      // 组件属性，里面使用的是ant-design-vue原生的属性
      // defaultValue为自定义的类型
      defaultValue: FactoryType['P2PTOOL'],
      colProps: {
        span: 6
      }
    },
    {
      // label使用vue-i18n中英文的定义的变量
      label: 'SpecSEQ',
      // filed为v-model绑定的值
      field: 'SpecSEQ',
      // component的值是ant-design-vue组件的名字
      component: 'Input',
      // 组件属性，里面使用的是ant-design-vue原生的属性
      // defaultValue为自定义的类型
      defaultValue: FactoryType['SPECSEQ'],
      dynamicDisabled: true,
      colProps: {
        span: 6
      }
    },
    {
      // label使用vue-i18n中英文的定义的变量
      label: 'Specver',
      // filed为v-model绑定的值
      field: 'Specver',
      // component的值是ant-design-vue组件的名字
      component: 'Input',
      // 组件属性，里面使用的是ant-design-vue原生的属性
      // defaultValue为自定义的类型
      defaultValue: FactoryType['SPECVER'],
      dynamicDisabled: true,
      colProps: {
        span: 6
      }
    },
    {
      // label使用vue-i18n中英文的定义的变量
      label: 'CLSEQ',
      // filed为v-model绑定的值
      field: 'CLSEQ',
      // component的值是ant-design-vue组件的名字
      component: 'Input',
      // 组件属性，里面使用的是ant-design-vue原生的属性
      // defaultValue为自定义的类型
      defaultValue: FactoryType['CLSEQ'],
      dynamicDisabled: true,
      colProps: {
        span: 6
      }
    },
    {
      // label使用vue-i18n中英文的定义的变量
      label: 'Clver',
      // filed为v-model绑定的值
      field: 'Clver',
      // component的值是ant-design-vue组件的名字
      component: 'Input',
      // 组件属性，里面使用的是ant-design-vue原生的属性
      // defaultValue为自定义的类型
      defaultValue: FactoryType['CLVER'],
      dynamicDisabled: true,
      colProps: {
        span: 6
      }
    },
    {
      // label使用vue-i18n中英文的定义的变量
      label: 'U Spec',
      // filed为v-model绑定的值
      field: 'USpec',
      // component的值是ant-design-vue组件的名字
      component: 'Input',
      // 组件属性，里面使用的是ant-design-vue原生的属性
      // defaultValue为自定义的类型
      defaultValue: FactoryType['USPEC'],
      required: true,
      colProps: {
        span: 4
      }
    },
    {
      // label使用vue-i18n中英文的定义的变量
      label: 'UCL1',
      // filed为v-model绑定的值
      field: 'UCL1',
      // component的值是ant-design-vue组件的名字
      component: 'Input',
      // 组件属性，里面使用的是ant-design-vue原生的属性
      // defaultValue为自定义的类型
      defaultValue: FactoryType['UCL1'],
      required: true,
      colProps: {
        span: 4
      }
    },
    {
      // label使用vue-i18n中英文的定义的变量
      label: 'UCL2',
      // filed为v-model绑定的值
      field: 'UCL2',
      // component的值是ant-design-vue组件的名字
      component: 'Input',
      // 组件属性，里面使用的是ant-design-vue原生的属性
      // defaultValue为自定义的类型
      defaultValue: FactoryType['UCL2'],
      required: true,
      colProps: {
        span: 4
      }
    },
    {
      // label使用vue-i18n中英文的定义的变量
      label: 'UCL3',
      // filed为v-model绑定的值
      field: 'UCL3',
      // component的值是ant-design-vue组件的名字
      component: 'Input',
      // 组件属性，里面使用的是ant-design-vue原生的属性
      // defaultValue为自定义的类型
      defaultValue: FactoryType['UCL3'],
      colProps: {
        span: 4
      }
    },
    {
      // label使用vue-i18n中英文的定义的变量
      label: '',
      // filed为v-model绑定的值
      field: 'Clver123456',
      // component的值是ant-design-vue组件的名字
      component: '',
      // 组件属性，里面使用的是ant-design-vue原生的属性
      // defaultValue为自定义的类型
      colProps: {
        span: 8
      }
    },
    {
      // label使用vue-i18n中英文的定义的变量
      label: 'T arget',
      // filed为v-model绑定的值
      field: 'Target',
      // component的值是ant-design-vue组件的名字
      component: 'Input',
      // 组件属性，里面使用的是ant-design-vue原生的属性
      // defaultValue为自定义的类型
      defaultValue: FactoryType['TARGET'],
      required: true,
      colProps: {
        span: 4
      }
    },
    {
      // label使用vue-i18n中英文的定义的变量
      label: 'CL1',
      // filed为v-model绑定的值
      field: 'CL1',
      // component的值是ant-design-vue组件的名字
      component: 'Input',
      // 组件属性，里面使用的是ant-design-vue原生的属性
      // defaultValue为自定义的类型
      defaultValue: FactoryType['CL1'],
      required: true,
      colProps: {
        span: 4
      }
    },
    {
      // label使用vue-i18n中英文的定义的变量
      label: 'CL2',
      // filed为v-model绑定的值
      field: 'CL2',
      // component的值是ant-design-vue组件的名字
      component: 'Input',
      // 组件属性，里面使用的是ant-design-vue原生的属性
      // defaultValue为自定义的类型
      defaultValue: FactoryType['CL2'],
      colProps: {
        span: 4
      }
    },
    {
      // label使用vue-i18n中英文的定义的变量
      label: 'CL3',
      // filed为v-model绑定的值
      field: 'CL3',
      // component的值是ant-design-vue组件的名字
      component: 'Input',
      // 组件属性，里面使用的是ant-design-vue原生的属性
      // defaultValue为自定义的类型
      defaultValue: FactoryType['CL3'],
      colProps: {
        span: 4
      }
    },
    {
      // label使用vue-i18n中英文的定义的变量
      label: '',
      // filed为v-model绑定的值
      field: 'Clver123456',
      // component的值是ant-design-vue组件的名字
      component: '',
      // 组件属性，里面使用的是ant-design-vue原生的属性
      // defaultValue为自定义的类型
      colProps: {
        span: 8
      }
    },
    {
      // label使用vue-i18n中英文的定义的变量
      label: 'L Spec',
      // filed为v-model绑定的值
      field: 'LSpec',
      // component的值是ant-design-vue组件的名字
      component: 'Input',
      // 组件属性，里面使用的是ant-design-vue原生的属性
      // defaultValue为自定义的类型
      defaultValue: FactoryType['LSPEC'],
      required: true,
      colProps: {
        span: 4
      }
    },
    {
      // label使用vue-i18n中英文的定义的变量
      label: 'LCL1',
      // filed为v-model绑定的值
      field: 'LCL1',
      // component的值是ant-design-vue组件的名字
      component: 'Input',
      // 组件属性，里面使用的是ant-design-vue原生的属性
      // defaultValue为自定义的类型
      defaultValue: FactoryType['LCL1'],
      required: true,
      colProps: {
        span: 4
      }
    },
    {
      // label使用vue-i18n中英文的定义的变量
      label: 'LCL2',
      // filed为v-model绑定的值
      field: 'LCL2',
      // component的值是ant-design-vue组件的名字
      component: 'Input',
      // 组件属性，里面使用的是ant-design-vue原生的属性
      // defaultValue为自定义的类型
      defaultValue: FactoryType['LCL2'],
      colProps: {
        span: 4
      }
    },
    {
      // label使用vue-i18n中英文的定义的变量
      label: 'LCL3',
      // filed为v-model绑定的值
      field: 'LCL3',
      // component的值是ant-design-vue组件的名字
      component: 'Input',
      // 组件属性，里面使用的是ant-design-vue原生的属性
      // defaultValue为自定义的类型
      defaultValue: FactoryType['LCL3'],
      colProps: {
        span: 4
      }
    },
    {
      // label使用vue-i18n中英文的定义的变量
      label: '',
      // filed为v-model绑定的值
      field: 'Clver123456',
      // component的值是ant-design-vue组件的名字
      component: '',
      // 组件属性，里面使用的是ant-design-vue原生的属性
      // defaultValue为自定义的类型
      colProps: {
        span: 8
      }
    },
    // Query type 单选按钮
    {
      label: 'WER',
      component: 'CheckboxGroup',
      field: 'WER',
      componentProps: {
        options: [
          {
            label: '1 out of 1 points above 3 stddev',
            value: '1 out of 1 points above 3 stddev',
          },
          {
            label: '2 out of 3 points above 2 stddev',
            value: "2 out of 3 points above 2 stddev",
          },
          {
            label: '4 out of 5 points above 1 stddev',
            value: '4 out of 5 points above 1 stddev',
          },
          {
            label: '8 out of 8 points above average',
            value: '8 out of 8 points above average',
          },
          {
            label: '1 out of 1 points below-3 stddev',
            value: '1 out of 1 points below 3 stddev'
          },
          {
            label: '2 out of 3 points below 2 stddev',
            value: "2 out of 3 points below 2 stddev"
          },
          {
            label: '4 out of 5 points below 1 stddev',
            value: '4 out of 5 points below 1 stddev'
          },
          {
            label: '8 out of 8 points below average',
            value: '8 out of 8 points below average'
          },
          {
            label: '15 out of 15 points -1~1 stddev',
            value: '15 out of 15 points -1~1 stddev'
          },
          {
            label: '8 out of 8 points -1~1 stddev',
            value: '8 out of 8 points -1~1 stddev'
          },
          {
            label: '9 out ofp 9 points above average',
            value: '9 out ofp 9 points above average'
          },
          {
            label: '7 out of7 points steady increase',
            value: '7 out of7 points steady increase'
          },
          {
            label: '9 out of 9 points below average',
            value: '9 out of 9 points below average'
          },
          {
            label: '7 out of 7 points steady decrease',
            value: '7 out of 7 points steady decrease'
          },
          {
            label: '14 out of 14 points up and down',
            value: '14 out of 14 points up and down'
          },
          {
            label: 'Out of Manger Line',
            value: 'Out of Manger Line'
          }
        ],
      },
      defaultValue: ['1 out of 1 points above 3 stddev', '1 out of 1 points below 3 stddev','7 out of 7 points steady decrease','7 out of7 points steady increase'],
      colProps: {
        span: 16
      }
    },
    // worder type 下拉多选框   api
    {
      label: '',
      field: 'Selwo',
      component: '',
      colProps: {
        span: 8
      }
    },
    // Report Format 单选按钮
    {
      label: 'ChartGrade',
      component: 'Select',
      field: 'ChartGrade',
      componentProps: {
        options: [
          {
            label: 'A(Majior)',
            value: 'A'
          },
          {
            label: 'I(Minor)',
            value: "I"
          },
          {
            label: 'O(NonCritical)',
            value: "O"
          }
        ],
      },
      defaultValue: 'A',
      colProps: {
        span: 6
      }
    },
    {
      label: '',
      field: 'Selwo123',
      component: '',
      colProps: {
        span: 18
      }
    },
    {
      label: 'Manger',
      field: 'Selwo123',
      component: ' ',
      colProps: {
        span: 8
      }
    },
    {
      label: 'OfflnSPC Ctrl',
      field: 'Selwo123',
      component: ' ',
      colProps: {
        span: 8
      }
    },
    {
      label: 'Other',
      field: 'Selwo123',
      component: ' ',
      colProps: {
        span: 8
      }
    },
    {
      label: 'Lower',
      field: 'Lower',
      component: 'Input',
      defaultValue: '0',
      colProps: {
        span: 8
      }
    },
    {
      label: 'Sample Cnt',
      field: 'SampleCnt',
      component: 'Input',
      defaultValue: FactoryType['SAMPLECNT'],
      colProps: {
        span: 8
      }
    },
    {
      label: 'RunMode',
      field: 'RunMode',
      component: 'Select',
      componentProps: {
        options: [
          {
            label: '(X)Don\'t care',
            value: 'X'
          },
          {
            label: '(N)Nomal',
            value: "N"
          },
          {
            label: '(A)Abnormal',
            value: "A"
          },
          {
            label: '(P)Product',
            value: "P"
          },
          {
            label: '(R)RTC',
            value: "R"
          },
          {
            label: '(D)Dummy-Monitor Run',
            value: "D"
          }
        ],
      },
      defaultValue: 'X',
      colProps: {
        span: 8
      }
    },
    {
      label: 'Target',
      field: 'Target',
      component: 'Input',
      defaultValue: '0',
      colProps: {
        span: 8
      }
    },
    {
      label: 'Avg Type',
      field: 'AvgType',
      component: 'Select',
      componentProps: {
        options: [
          {
            label: '1.sum(Xi)/n',
            value: '1'
          },
          {
            label: '2.X double bar',
            value: "2"
          },
          {
            label: '3.Avg(MaxXi+MinXi)/2)',
            value: "3"
          }
        ],
      },
      defaultValue: '1',
      colProps: {
        span: 8
      }
    },
    {
      label: 'TargetTool',
      field: 'TargetTool',
      component: 'Select',
      componentProps: {
        options: [
          {
            label: '(P)Product',
            value: 'P'
          },
          {
            label: '(M)Measure',
            value: "M"
          }
        ],
      },
      defaultValue: 'P',
      colProps: {
        span: 8
      }
    },
    {
      label: 'Upper',
      field: 'Upper',
      component: 'Input',
      defaultValue: '0',
      colProps: {
        span: 8
      }
    },
    {
      label: 'Singma Type',
      field: 'SingmaType',
      component: 'Select',
      componentProps: {
        options: [
          {
            label: '1.Sqrt(sum((Xi-Xbar)^2)/(n-1))',
            value: '1'
          },
          {
            label: '2.sqrt(sum((Xiba-X double bar)^2)/(n-1))',
            value: "2"
          },
          {
            label: '3.R bar/d2',
            value: "3"
          },
          {
            label: '4.S bar/C2',
            value: "4"
          },
          {
            label: '5.Rm bar/d2',
            value: "5"
          }
        ],
      },
      defaultValue: '1',
      colProps: {
        span: 8
      }
    },
    {
      label: 'SPEC Act',
      field: 'SPECAct',
      component: 'Select',
      componentProps: {
        options: [
          {
            label: '(N) Don\'t Care',
            value: 'N'
          },
          {
            label: '(Z)Set SPEC value to Zero',
            value: "P"
          }
        ],
      },
      defaultValue: 'N',
      colProps: {
        span: 8
      }
    },
    {
      label: '',
      field: 'Selwo123',
      component: '',
      colProps: {
        span: 8
      }
    },
    {
      label: 'Chart State',
      field: 'ChartState',
      component: 'Select',
      componentProps: {
        options: [
          {
            label: 'Testing',
            value: 'T'
          },
          {
            label: 'Product',
            value: "P"
          }
        ],
      },
      defaultValue: 'P',
      colProps: {
        span: 8
      }
    },
    {
      label: 'CL Act',
      field: 'CLAct',
      component: 'Select',
      componentProps: {
        options: [
          {
            label: '(N)Don\'t Care',
            value: 'N'
          },
          {
            label: '(Z)Set SPEC value to Zero',
            value: "Z"
          }
        ],
      },
      defaultValue: 'N',
      colProps: {
        span: 8
      }
    },
  ];
};
