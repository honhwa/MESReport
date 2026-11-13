import type { FormSchema } from '@/components/core/schema-form/';
import { formatToDate } from '@/utils/dateUtil';
import { NGBSelectType} from '@/pages/ReportPortal/Static/NGBCommon-Form-schema';
import useRemoteSelect from '@/pages/ReportPortal/utils/NGBuseRemoteSelect';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { cloneDeep } from 'lodash-es';
import { formatToDateHour } from '@/utils/dateUtil';
import { message } from 'ant-design-vue';
import dayjs from 'dayjs';


const useCommonParams = AddCommonPageParams();
const { getCommonFormParamsNoAll,mounted,getCommonFormParams,mountedLCMDefectCode,mountedEqptId,
  mountedLCMowner,mountedModuleData,mountedSourceFab,InfiniteScroll,mountedCustomer,mountedSpcProd,
  mountedWoTypeBySap,mountedLCDowner, mountedLCDGrade,mountedHoldCode,mountedOper,
  mountedDataGroup,mountedLCMBigEquipment ,mountedLcdSize,mountedLCMGrade, mountedSpcModuleName,mountedSpcMeasTool,mountedSpcProcTool,
  mountedSpcLine, mountedSpcBeforeHours,mountedLCMOEEBigEquipment,mountedOperationForRWK,mountedOperationForINS,
  mountedSpcProdID,mountedSpcOper,mountedSpcProduct,mountedSpcProcess,mountedSpcPerLine,mountedSpcProdNum,
  mountedSpcPreLine } = useRemoteSelect()
let ifSelectall3=false// size bigsize是否全選標記
let ifSelectall4=false// 機台是否全選標記
let ifSelectall2=false// LCD size PRODUCT是否全選標記




/////***************各廠公用  時間，班別，樓層，工單輸入******************/////

//From開始時間
export const FromDateParams: FormSchema[] = [
  {
    field: 'FromDate',
    component: 'DatePicker',
    label: 'common.commonpages.Date',
    // helpMessage: ['TreeSelect组件', '使用接口提供的数据生成选项'],
    componentProps: {
      valueFormat: 'YYYY-MM-DD'
    },
    defaultValue: formatToDate(),
    required: true
  },
];

  //to結束時間
export const ToDateParams: FormSchema[] = [    
  {
    field: 'ToDate',
    component: 'DatePicker',
    label: 'common.commonpages.To',
    componentProps: {
      valueFormat: 'YYYY-MM-DD'
    },
    defaultValue: formatToDate(),
    required: true
  },
];

//From開始時間帶 小時+30分鐘的，如：2023-01-10 07:30
export const CompleteFromDateParams: FormSchema[] = [
  { label: 'common.commonpages.Date',
  component: 'DatePicker',
  field: 'CompleteFromDate',

  componentProps: {
    format: 'YYYY-MM-DD HH:mm',
    showTime:true,
    minuteStep:30,
    valueFormat:'YYYY-MM-DD HH:mm'
  },
  defaultValue:formatToDateHour(),
  required: true
 },
];


//to結束時間帶 小時+30分鐘的，如：2023-01-10 07:30
export const CompleteToDateParams: FormSchema[] = [
  {
    label: 'common.commonpages.To',
    component: 'DatePicker',
    field: 'CompleteToDate',
  
    componentProps: {
      format: 'YYYY-MM-DD HH:mm',
      showTime:true,
      minuteStep:30,
      valueFormat:'YYYY-MM-DD HH:mm'
    },
    defaultValue: formatToDateHour(),
    required: true
  },
];

//By month，如：202301
export class DateMonthParams{
  optionsdata:NGBSelectType;
  //FactoryType: string;
  _obj: FormSchema[];
  constructor(
    optionsdata:NGBSelectType,
    //FactoryType: string,
  ){
    this.optionsdata = optionsdata;
    //this.FactoryType = FactoryType;
    //optionsdata['FactoryType'] = this.FactoryType;
    this.init(optionsdata);
    this._obj =[ 
      {
        field: 'Month',
        component: 'Select',
        label: 'common.commonpages.DateMonth',
        componentProps: () => {
          return {
            options: optionsdata.TimeData,
          };
        },
        // defaultValue:String(dayjs().year()) + "06"
        defaultValue:String(dayjs().year()) + dayjs().format("MM"),
        required: true,
      },
    ]; 
    
  }
  // api請求數據
  async init(optionsdata: NGBSelectType) {
      function getMonths(start: string, end: string) {
        let startyear = Number(start.split('-')[0]);
        let startMonth = Number(start.split('-')[1]);
        let endyear = Number(end.split('-')[0]);
        let endMonth = Number(end.split('-')[1]);
        let result: string[] = [];
        let year = startyear;
        let month = startMonth;
        while (year < endyear || (year === endyear && month <= endMonth)) {
            if (month >= 10) {
                result.push(`${year}${month}`)
            } else {
                result.push(`${year}0${month}`)
            }
            month++;
            if (month > 12) {
                year++;
                month = 1;
            }
        }
        return result;
    }
    let start = String(dayjs().year()-1) + `-${dayjs().add(6,'months').format("MM")}`;
    let end = String(dayjs().year() ) + `-${dayjs().add(6,'months').format("MM")}`;
    optionsdata.TimeData = getMonths(start, end).map(item => {
        return { label: item, value: item }
    })  
  }

  // 獲取Marterial
  get obj() {
    return this._obj;
   }
   // 更改label
   setLabel(index: number, LabelValue: string) {
       this._obj[index]['label'] = LabelValue;
       return this;
    }
} 

//班別
export const ShiftParams: FormSchema[] = [
  {
    label: 'common.commonpages.Shift',
    component: 'RadioGroup',
    field: 'Shift',

    componentProps: {
      options: [
        {
          label: 'ALL',
          value: 'ALL'
        },
        {
          label: '日(A)',
          value: "A"
        },
        {
          label: '夜(B)',
          value: 'B'
        }
      ],
    },
    defaultValue: 'ALL'

  },
];

//樓層
export const FloorParams: FormSchema[] = [  
  {
    label: 'common.commonpages.Floor',
    component: 'RadioGroup',
    field: 'Floor',

    componentProps: {
      options: [
        {
          label: 'ALL',
          value: 'ALL'
        },
        {
          label: '1F',
          value: '1'
        },
        {
          label: '2F',
          value: '2'
        },
        {
          label: '3F',
          value: '3'
        }
      ]
    },
    defaultValue: 'ALL'
  },
];

//Interval
export const IntervalParams: FormSchema[] = [  
  {
    label: 'common.commonpages.Interval',
    component: 'RadioGroup',
    field: 'Interval',

    componentProps: {
      options: [
        {
          label: 'Day',
          value: 'Day'
        },
        {
          label: 'Week',
          value: 'Week'
        },
        {
          label: 'Month',
          value: 'Month'
        },
        
      ]
    },
    defaultValue: 'Day'
  },
];

//InputWorkorder 多文本框
export const InputWorkorderParams: FormSchema[] = [
  {
    label: 'common.commonpages.WorkOrder',
    component: 'InputTextArea',
    field: 'Wo_nbr',
  },
];

//InputWorkorder 單文本框
export const WorkOrderLikeParams: FormSchema[] = [
{ 
  label: 'common.commonpages.WorkOrderLike',
  component: 'Input',
  field: 'Wo_nbr',
},
]

//Inputprod 多文本框
export const InputProdNbrLikeParams: FormSchema[] = [
  { 
    label: 'Prod_nbr Like',
    component: 'Input',
    field: 'Prod_nbr',
  },
  ]

  //inputOperatorParams 單文本框
export const InputOperatorParams: FormSchema[] = [
  {
    label: 'common.commonpages.Operator',    
    field: 'Operator',  
    component: 'Input'    
    
  },]
  

////***************LCM 部分組件******************/////

//AbnormalCode
export const AbnormalCodeParams: FormSchema[] = [
  {
    label: 'common.commonpages.DisplayAbnormalCode',
    labelWidth: 180,
    component: 'RadioGroup',
    field: 'AbnormalCode',
    componentProps: {
      options: [
        {
          label: 'No',
          value: 'N'
        },
        {
          label: 'Yes',
          value: "Y"
        }
      ],
    },
    defaultValue: 'N'
  },
  ]


  //StartHours
export const StartHoursParams: FormSchema[] = [
  {
    label: 'Start Hours',    
    field: 'StartHour',  
    component: 'Input',    
    componentProps: () => {
      return {
        placeholder: '大於多少',
        onChange: (e: any) => {
          //console.log(e);
        },
      };
    },
    componentSlots: () => {
      return {
        prefix: () => 'over',
        suffix: () => '小時',
      };
    },
  },
  ]
  
  //ToHour
  export const ToHourParams: FormSchema[] = [
    {    
      field: 'ToHour',  
      component: 'Input', 
      componentProps: () => {
        return {
          placeholder: '小於多少',
          onChange: (e: any) => {
            console.log(e);
          },
        };
      },
      componentSlots: () => {
        return {
          prefix: () => 'over',
          suffix: () => '小時',
        };
      },
     
    }, 
    ]

//bySize,ByModule 查詢料號
export class LcmBySizeByModuleProdParams{
  optionsdata:NGBSelectType;
  FactoryType: string;
  obj: FormSchema[];
  constructor(
    optionsdata:NGBSelectType,
    FactoryType: string,
  ){
    this.optionsdata = optionsdata;
    this.FactoryType = FactoryType;
    optionsdata['FactoryType'] = this.FactoryType;
    this.init(optionsdata, this.FactoryType);
    this.obj = this.setobj();
  }
  // api請求數據
  async init(optionsdata: NGBSelectType, FactoryType: string) {
    await mounted(optionsdata, FactoryType);
    await mountedModuleData(optionsdata, FactoryType);
  }
  // 初始化obj
  setobj(optionsdata: NGBSelectType = this.optionsdata) {
    this.obj = [
      {
        field: '#LCMDivider',
        component: 'Divider',
        label: 'LCM Product'
      },
      {
        label: '料號查詢方式',
        component: 'RadioGroup',
        field: 'choose_prodtype',
        componentProps: (model) => {
          return {
            options: [
              {
                label: 'By Size',
                value: 'bysize'
              },
              {
                label: 'By Module',
                value: 'bymodule'
              }
            ],  
            onChange: (e) => {
              model.formModel.Prod_nbr=[];
              model.formModel.Product=[];  
              model.formModel.Option_Prod_nbr=[];
              model.formModel['#optionmenu']='';  
            },
          }  
    
        },
        defaultValue: 'bysize'
    
      },
    
      {
        label: 'common.commonpages.size',
        component: 'Select',
        field: 'Size',
        componentProps: ({ formModel, field }) => {
          return {
            mode: 'multiple',
            options: optionsdata.SizeData,
            placeholder: 'size',
            onChange: async (e: any) => {
              formModel.Product=[]; 
              optionsdata.BigProductData1 = optionsdata.BigProductData= await getCommonFormParamsNoAll('PART', {
                name: "Date",
                apiName: 'BigProduct',
                FactoryType:  optionsdata['FactoryType'],
                G_FAC: useCommonParams.G_FAC_COM,
                Size: e
              }) 
            },
          };
        },
        vIf: ({ formModel }) => formModel.choose_prodtype == 'bysize'
      },
      {
        label: 'common.commonpages.bigproduct',
        component: 'Select',
        field: 'Product',
        componentProps: (model) => {
          return {
            mode: 'multiple',
            options: optionsdata.BigProductData1,
            placeholder: 'big product',
            onChange: async (e: any) => {
              model.formModel.Option_Prod_nbr=[];
              model.formModel['#optionmenu']='';      
            
              optionsdata.ProductData1 = optionsdata.ProductData= await getCommonFormParamsNoAll('SOPTION', {
                  name: "Date",
                  apiName: 'Product',
                  FactoryType: optionsdata['FactoryType'],
                  G_FAC: useCommonParams.G_FAC_COM,
                  BigProduct:model.formModel.Product
                })        
            }
          };
        },
        vIf: ({ formModel }) => formModel.choose_prodtype == 'bysize'
    
      },  
      {
        label: 'Module',
        component: 'Select',
        field: 'Module',
        componentProps: ({ formModel, field }) => {
          return {
            mode: 'multiple',
            options: optionsdata.ModuleData,
            placeholder: 'Module',
            onChange: async (e: any) => {
              // formModel.Prod_nbr=[]; 
              // formModel.Option_Prod_nbr=[]; 
              formModel['#optionmenu']='';         
                optionsdata.ModuleProductData1 = optionsdata.ModuleProductData = await getCommonFormParamsNoAll('SOPTION', {
                  name: "Date",
                  apiName: "ModuleProduct",
                  FactoryType:  optionsdata['FactoryType'],
                  G_FAC: useCommonParams.G_FAC_COM,
                  Module: e
                })
            },
          };
        },
        vIf: ({ formModel }) => formModel.choose_prodtype == 'bymodule'
      },
      {
        label: 'Product',
        field: 'Option_Prod_nbr',
        component: 'Select',
        componentProps: (model) => { 
            if ( model.formModel.choose_prodtype == 'bymodule') { 
                return {
                  mode: 'multiple',
                  options: optionsdata.ModuleProductData1,
                  maxTagCount:10,
                  placeholder: 'product',
                  onSelect: async (e: any, option) => {
                    model.formModel['Prod_nbr'] = Array.from(
                        new Set([...model.formModel['Prod_nbr'], option.label])
                    );
                    optionsdata.Tempoption = model.formModel['Prod_nbr'].map((item) => {
                        return {
                            label: item,
                            value: item
                        };
                    });
                    model.formModel['#optionmenu'] = '';
                },
                onDeselect: (value, option) => {
                    model.formModel['Option_Prod_nbr'] = model.formModel['Option_Prod_nbr'].filter(
                        (item) => item != value
                    );
                    optionsdata.Tempoption = model.formModel['Option_Prod_nbr'].map((item) => {
                        return {
                            label: item,
                            value: item
                        };
                    });
                    model.formModel['#optionmenu'] = '';
                }
            };
    
          }else{
                return {
                  mode: 'multiple',
                  options: optionsdata.ProductData1,
                  maxTagCount:10,
                  placeholder: 'product',
                  onSelect: async (e: any, option) => {
                    model.formModel['Prod_nbr'] = Array.from(
                        new Set([...model.formModel['Prod_nbr'], option.label])
                    );
                    optionsdata.Tempoption = model.formModel['Prod_nbr'].map((item) => {
                        return {
                            label: item,
                            value: item
                        };
                    });
                    model.formModel['#optionmenu'] = '';
                },
                onDeselect: (value, option) => {
                    model.formModel['Option_Prod_nbr'] = model.formModel['Option_Prod_nbr'].filter(
                        (item) => item != value
                    );
                    optionsdata.Tempoption = model.formModel['Option_Prod_nbr'].map((item) => {
                        return {
                            label: item,
                            value: item
                        };
                    });
                    model.formModel['#optionmenu'] = '';
                }
            };
          }
          
        }
      },
      {
        label: '',
        // field: '#optionmenu',
        field: '#optionmenu',
        component: 'RadioGroup',
        componentProps: (model) => {
            return {
                options: [
                    {
                        label: '全選',
                        value: 'selectALL'
                    },
                    {
                        label: '取消全選',
                        value: 'cancelselect'
                    }
                ],
               
                onChange: (e) => {
    
                    if ( model.formModel.choose_prodtype == 'bymodule') { 
                        // 全選，全部添加
                        if (e.target.value == 'selectALL') {
                            // 上一個選項的所有值
                            let temp;
                            temp = optionsdata.ModuleProductData1!.map((item) => item.value);
                            
                            model.formModel['Option_Prod_nbr'] = temp;
                            model.formModel['Prod_nbr'] = [
                                ...new Set([...model.formModel['Prod_nbr'], ...temp])
                            ];
                            optionsdata.Tempoption = model.formModel['Prod_nbr'].map((item) => {                     
                                return {
                                    label: item,
                                    value: item
                                };
                            });
                        } else if (e.target.value == 'cancelselect') {
                            // 全消全選
                            model.formModel['Prod_nbr'] = model.formModel['Prod_nbr'].filter(
                                (item) => !model.formModel['Option_Prod_nbr'].includes(item)
                            );
                            model.formModel['Option_Prod_nbr'] = [];
                        }
    
    
    
                    }else{
                        // 全選，全部添加
                        if (e.target.value == 'selectALL') {
                          // 上一個選項的所有值
                          let temp;
                          temp = optionsdata.ProductData1!.map((item) => item.value);
                          
                          model.formModel['Option_Prod_nbr'] = temp;
                          model.formModel['Prod_nbr'] = [
                              ...new Set([...model.formModel['Prod_nbr'], ...temp])
                          ];
                          optionsdata.Tempoption = model.formModel['Prod_nbr'].map((item) => {                     
                              return {
                                  label: item,
                                  value: item
                              };
                          });
                      } else if (e.target.value == 'cancelselect') {
                          // 全消全選
                          model.formModel['Prod_nbr'] = model.formModel['Prod_nbr'].filter(
                              (item) => !model.formModel['Option_Prod_nbr'].includes(item)
                          );
                          model.formModel['Option_Prod_nbr'] = [];
                      }
    
                    }
    
                    if(model.formModel.Prod_nbr.length>1000){
                      let prodnum=model.formModel.Prod_nbr.length
                       message.error('目前料號數"'+prodnum+' “超過最大限制筆數:1000” '); 
                     }
      
      
                  }
              };
          },
          defaultValue: ''
        },
        {
          label: '(限制1000筆)',
          // field: 'Product',
          field: 'Prod_nbr',
          component: 'Select',
          componentProps: (model) => {
             // console.log("optionsdata.Tempoption",optionsdata['tempoption'])
              return {
                  mode: 'multiple',
                  placeholder: 'product',
                  options: optionsdata.Tempoption,
                  maxTagCount: 8,
                  dropdownMatchSelectWidth: 150,             
    
                  onDeselect: (value, option) => {          
                  model.formModel['Option_Prod_nbr'] = model.formModel['Option_Prod_nbr'].filter((item) => item != value);
                      
                }
              };
            
          },
          defaultValue: []
        },
     ]
     return this.obj;
  }
   // 返回obj
   getSize() {
    return this.obj;
}
// 更改label
setLabel(index: number, LabelValue: string) {
    this.obj[index]['label'] = LabelValue;
    return this;
 }
}


  //bySize 查詢料號
  export class LcmBySizeProdParams{
    optionsdata:NGBSelectType;
    FactoryType: string;
    obj: FormSchema[];
    constructor(
      optionsdata:NGBSelectType,
      FactoryType: string,

    ){
      this.optionsdata = optionsdata;
      this.FactoryType = FactoryType;
      optionsdata['FactoryType'] = this.FactoryType;
      this.init(optionsdata, this.FactoryType);
      this.obj = this.setobj();
    }
    // api請求數據
    async init(optionsdata: NGBSelectType, FactoryType: string) {
      await mounted(optionsdata, FactoryType);
      //console.log("optionsdata.SizeData",optionsdata.SizeData)
    }
    // 初始化obj
    setobj(optionsdata: NGBSelectType = this.optionsdata) {
      this.obj = [
        {
          field: '#LCMDivider',
          component: 'Divider',
          label: 'LCM Product'
        },
        {
          label: 'common.commonpages.size',
          component: 'Select',
          field: 'Size',
          componentProps: ({ formModel, field }) => {
            return {
              mode: 'multiple',
              options: optionsdata.SizeData,
              placeholder: 'size',
              onChange: async (e: any) => {
                 formModel.Product=[]; 
                 formModel.Option_Prod_nbr=[];
                  optionsdata.BigProductData1 = optionsdata.BigProductData= await getCommonFormParamsNoAll('PART', {
                    name: "Date",
                    apiName: 'BigProduct',
                    FactoryType: optionsdata['FactoryType'],
                    G_FAC: useCommonParams.G_FAC_COM,
                    Size: e
                  })  
              },
            };
          },
          //defaultValue:'ALL'
        },
        {
          label: 'common.commonpages.bigproduct',
          component: 'Select',
          field: 'Product',
          componentProps: (model ) => {
            return {
              mode: 'multiple',
              options: optionsdata.BigProductData1,
              placeholder: 'big product',
              onChange: async (e: any) => {
                model.formModel.Option_Prod_nbr=[];
                model.formModel['#optionmenu']='';
                optionsdata.ProductData1 = optionsdata.ProductData= await getCommonFormParamsNoAll('SOPTION', {
                  name: "Date",
                  apiName: 'Product',
                  FactoryType: optionsdata['FactoryType'],
                  G_FAC: useCommonParams.G_FAC_COM,
                  BigProduct:model.formModel.Product.filter(item => item.value != "ALL")
                })  
              }
            };
          },
          //defaultValue:'ALL'
        },
        {
          label: 'Product',
          field: 'Option_Prod_nbr',
          component: 'Select',
          componentProps: (model) => {
            return {
              mode: 'multiple',
              options: optionsdata.ProductData1,
              maxTagCount:10,
              placeholder: 'product',
              onSelect: async (e: any, option) => {
                model.formModel['Prod_nbr'] = Array.from(
                    new Set([...model.formModel['Prod_nbr'], option.label])
                );
                optionsdata.Tempoption = model.formModel['Prod_nbr'].map((item) => {
                    return {
                        label: item,
                        value: item
                    };
                });
                model.formModel['#optionmenu'] = '';
            },
             onDeselect: (value, option) => {
                model.formModel['Option_Prod_nbr'] = model.formModel['Option_Prod_nbr'].filter(
                    (item) => item != value
                );
                optionsdata.Tempoption = model.formModel['Option_Prod_nbr'].map((item) => {
                    return {
                        label: item,
                        value: item
                    };
                });
                model.formModel['#optionmenu'] = '';
            }
              
            };
            
          }
        },
        {
          label: '',
          // field: '#optionmenu',
          field: '#optionmenu',
          component: 'RadioGroup',
          componentProps: (model) => {
              return {
                  options: [
                      {
                          label: '全選',
                          value: 'selectALL'
                      },
                      {
                          label: '取消全選',
                          value: 'cancelselect'
                      }
                  ],
                 
                  onChange: (e) => {
                      // 全選，全部添加
                      if (e.target.value == 'selectALL') {
                          // 上一個選項的所有值
                          let temp;
                          temp = optionsdata.ProductData1!.map((item) => item.value);
                          
                          model.formModel['Option_Prod_nbr'] = temp;
                          model.formModel['Prod_nbr'] = [
                              ...new Set([...model.formModel['Prod_nbr'], ...temp])
                          ];
                          optionsdata.Tempoption = model.formModel['Prod_nbr'].map((item) => {                     
                              return {
                                  label: item,
                                  value: item
                              };
                          });
                      } else if (e.target.value == 'cancelselect') {
                          // 全消全選
                          model.formModel['Prod_nbr'] = model.formModel['Prod_nbr'].filter(
                              (item) => !model.formModel['Option_Prod_nbr'].includes(item)
                          );
                          model.formModel['Option_Prod_nbr'] = [];
                      }
                    }
                };
            },
            defaultValue: ''
          },
        {
        label: '(限制1000筆)',
        // field: 'Product',
        field: 'Prod_nbr',
        component: 'Select',
        componentProps: (model) => {
            //console.log("optionsdata.Tempoption",optionsdata['tempoption'])
            return {
                mode: 'multiple',
                placeholder: 'product',
                options: optionsdata.Tempoption,
                maxTagCount: 8,
                dropdownMatchSelectWidth: 150,
                onDeselect: (value, option) => {          
                model.formModel['Option_Prod_nbr'] = model.formModel['Option_Prod_nbr'].filter((item) => item != value);
                    
                }
            };
        },
        defaultValue: []
      }
     ];
      return this.obj;
    }
     // 返回obj
     getSize() {
      return this.obj;
  }
  // 更改label
  setLabel(index: number, LabelValue: string) {
      this.obj[index]['label'] = LabelValue;
      return this;
  }
}
  

//LcmOwner
export class LcmOwnerParams{
  optionsdata:NGBSelectType;
  FactoryType: string;
  _obj: FormSchema[];
  constructor(
    optionsdata:NGBSelectType,
    FactoryType: string,
  ){
    this.optionsdata = optionsdata;
    this.FactoryType = FactoryType;
    optionsdata['FactoryType'] = this.FactoryType;
    this.init(optionsdata, this.FactoryType);
    this._obj =[ 
      {
        label: 'common.commonpages.LCMOwner',
        field: 'LcmOwner',
        component: 'Select',
        componentProps: () => {
          return {
            mode: 'multiple',
            maxTagCount:5,
            options: optionsdata.OwnerName,
            placeholder: 'LcmOwner',
          };
        },
      },
    ]; 
    
  }
  // api請求數據
  async init(optionsdata: NGBSelectType, FactoryType: string) {
    await mountedLCMowner(optionsdata, FactoryType); 

  }

  // 獲取Marterial
  get obj() {
    return this._obj;
   }
   // 更改label
   setLabel(index: number, LabelValue: string) {
       this._obj[index]['label'] = LabelValue;
       return this;
    }

}


//LcdOwner
export class LcdOwnerParams{
  optionsdata:NGBSelectType;
  FactoryType: string;
  _obj: FormSchema[];
  constructor(
    optionsdata:NGBSelectType,
    FactoryType: string,
  ){
    this.optionsdata = optionsdata;
    this.FactoryType = FactoryType;
    optionsdata['FactoryType'] = this.FactoryType;
    this.init(optionsdata, this.FactoryType);
    this._obj =[ 
      {
        label: 'common.commonpages.LCDOwner',
        component: 'Select',
        field: 'LcdOwner',
        componentProps: () => {
          return {
            mode: 'multiple',
            maxTagCount:5,
            options: optionsdata.LcdOwnerLabel,
            placeholder: 'LcdOwner',
          }
        },
        //defaultValue:[''],
      },
    ]; 
    
  }
  // api請求數據
  async init(optionsdata: NGBSelectType, FactoryType: string) {
    await mountedLCDowner(optionsdata, FactoryType);  
  }

  // 獲取Marterial
  get obj() {
    return this._obj;
   }
   // 更改label
   setLabel(index: number, LabelValue: string) {
       this._obj[index]['label'] = LabelValue;
       return this;
    }

}



//LcdGrade
export class LcdGradeParams{
  optionsdata:NGBSelectType;
  FactoryType: string;
  _obj: FormSchema[];
  constructor(
    optionsdata:NGBSelectType,
    FactoryType: string,
  ){
    this.optionsdata = optionsdata;
    this.FactoryType = FactoryType;
    optionsdata['FactoryType'] = this.FactoryType;
    this.init(optionsdata, this.FactoryType);
    this._obj =[ 
      {
        label: 'common.commonpages.LCDGrade',
        component: 'Select',
        field: 'LcdGrade',
        componentProps: () => {
          return {
            mode: 'multiple',
            maxTagCount:5,
            options: optionsdata.LcdGradeLabel,
            placeholder: 'LcdGrade',
          }
        },
       // defaultValue:[''],
      },
    ]; 
    
  }
  // api請求數據
  async init(optionsdata: NGBSelectType, FactoryType: string) {
    await mountedLCDGrade(optionsdata, FactoryType);  
  }

  // 獲取Marterial
  get obj() {
    return this._obj;
   }
   // 更改label
   setLabel(index: number, LabelValue: string) {
       this._obj[index]['label'] = LabelValue;
       return this;
    }
} 


//LcmGrade
export class LcmGradeParams{
  optionsdata:NGBSelectType;
  FactoryType: string;
  _obj: FormSchema[];
  constructor(
    optionsdata:NGBSelectType,
    FactoryType: string,
  ){
    this.optionsdata = optionsdata;
    this.FactoryType = FactoryType;
    optionsdata['FactoryType'] = this.FactoryType;
    this.init(optionsdata, this.FactoryType);
    this._obj =[ 
      {
        label: 'common.commonpages.LCMGrade',
        component: 'Select',
        field: 'LcmGrade',
        componentProps: () => {
          return {
            mode: 'multiple',
            maxTagCount:5,
            options: optionsdata.LcmGradeLabel,
            placeholder: 'LcmGrade',
          }
        },
       // defaultValue:[''],
      },
    ]; 
    
  }
  // api請求數據
  async init(optionsdata: NGBSelectType, FactoryType: string) {
    await mountedLCMGrade(optionsdata, FactoryType); 
  }

  // 獲取Marterial
  get obj() {
    return this._obj;
   }
   // 更改label
   setLabel(index: number, LabelValue: string) {
       this._obj[index]['label'] = LabelValue;
       return this;
    }
} 


//HoldCode
export class HoldCodeParams{
  optionsdata:NGBSelectType;
  FactoryType: string;
  _obj: FormSchema[];
  constructor(
    optionsdata:NGBSelectType,
    FactoryType: string,
  ){
    this.optionsdata = optionsdata;
    this.FactoryType = FactoryType;
    optionsdata['FactoryType'] = this.FactoryType;
    this.init(optionsdata, this.FactoryType);
    this._obj =[ 
      {
        label: 'Hold Code',
        component: 'Select',
        field: 'HoldCode',
        componentProps: () => {
          return {
            mode: 'multiple',
            maxTagCount:5,
            options: optionsdata.HoldCodeLabel,
            placeholder: 'HoldCode',
          }
        }
      },
    ]; 
    
  }
  // api請求數據
  async init(optionsdata: NGBSelectType, FactoryType: string) {
    await mountedHoldCode(optionsdata, FactoryType);  
  }

  // 獲取Marterial
  get obj() {
    return this._obj;
   }
   // 更改label
   setLabel(index: number, LabelValue: string) {
       this._obj[index]['label'] = LabelValue;
       return this;
    }
}


//Operation
export class OperationParams{
  optionsdata:NGBSelectType;
  FactoryType: string;
  _obj: FormSchema[];
  constructor(
    optionsdata:NGBSelectType,
    FactoryType: string,
  ){
    this.optionsdata = optionsdata;
    this.FactoryType = FactoryType;
    optionsdata['FactoryType'] = this.FactoryType;
    this.init(optionsdata, this.FactoryType);
    this._obj =[ 
      {
        label: 'common.commonpages.Operation',
        component: 'Select',
        field: 'Operation',
        componentProps: () => {
          return {
            mode: 'multiple',
            options: optionsdata.OperationLabel,
            maxTagCount:10,
            //options: optionsdata.OperationLabel.map((item:Object) => { return { label: item['label'], value: item['value'].split('-')[0],} }),
            placeholder: 'Operation',
          };
        },
      },
    ]; 
    
  }
  // api請求數據
  async init(optionsdata: NGBSelectType, FactoryType: string) {
    await mountedOper(optionsdata,  FactoryType );  
  }

  // 獲取Marterial
  get obj() {
    return this._obj;
   }
   // 更改label
   setLabel(index: number, LabelValue: string) {
       this._obj[index]['label'] = LabelValue;
       return this;
    }
}


//Customer
export class CustomerParams{
  optionsdata:NGBSelectType;
  FactoryType: string;
  _obj: FormSchema[];
  constructor(
    optionsdata:NGBSelectType,
    FactoryType: string,
  ){
    this.optionsdata = optionsdata;
    this.FactoryType = FactoryType;
    optionsdata['FactoryType'] = this.FactoryType;
    this.init(optionsdata, this.FactoryType);
    this._obj =[ 
      {
        label: 'Customer',
        component: 'Select',
        field: 'Customer',
        componentProps: () => {
          return {
            mode: 'multiple',
            maxTagCount:5,
            options: optionsdata.CustomData,
            placeholder: 'Customer',
          };
        },
        //defaultValue:['']
       
      },
    ]; 
    
  }
  // api請求數據
  async init(optionsdata: NGBSelectType, FactoryType: string) {
    await mountedCustomer(optionsdata, FactoryType);  
  }

  // 獲取Marterial
  get obj() {
    return this._obj;
   }
   // 更改label
   setLabel(index: number, LabelValue: string) {
       this._obj[index]['label'] = LabelValue;
       return this;
    }
} 




//Reworked次數
export const ReworkedParams:FormSchema={
  label: 'common.commonpages.Reworked',
  component: 'Select',
  field: 'Reworked',
  componentProps: {
    options: [
      {
        label: 'ALL',
        value: ''
      },
      {
        label: '1次',
        value: "1"
      },
      {
        label: '1次以上',
        value: "over_1"
      },
      {
        label: '2次',
        value: "2"
      },
      {
        label: '2次以上',
        value: "over_2"
      },
      {
        label: '3次',
        value: "3"
      },
      {
        label: 'No',
        value: "N"
      },
    ],
  },
  defaultValue: 'ALL'
}


//Reworked ALL/YES/NO
export const ReworkedParams2:FormSchema={
  label: 'common.commonpages.Reworked',
  component: 'Select',
  field: 'Reworked',
  componentProps: {
    options: [
      {
        label: 'ALL',
        value: 'ALL'
      },
      
      {
        label: 'YES',
        value: "YES"
      },
      {
        label: 'NO',
        value: "NO"
      },
    ],
  },
  defaultValue: 'ALL'
}

//是否含快修
export const FastReworkParams:FormSchema={
  label: '是否含快修',
  component: 'Select',
  field: 'FastRework',
  componentProps: {
    options: [
      {
        label: '快修NG視為OK',
        value: '1'
      },
      {
        label: '快修NG視為NG',
        value: "2"
      },
      {
        label: '不含快修',
        value: "3"
      },
      {
        label: '只含快修',
        value: "4"
      },
      {
        label: '包含2800',
        value: "7"
      },
      {
        label: '內部快修',
        value: "8"
      },
      
    ],
  },
  defaultValue: '1'
}

//SourceFab
export class SourceFabParams{
  optionsdata:NGBSelectType;
  FactoryType: string;
  _obj: FormSchema[];
  constructor(
    optionsdata:NGBSelectType,
    FactoryType: string,
  ){
    this.optionsdata = optionsdata;
    this.FactoryType = FactoryType;
    optionsdata['FactoryType'] = this.FactoryType;
    this.init(optionsdata, this.FactoryType);
    this._obj =[ 
      {
        label: 'common.commonpages.SourceFab',
        component: 'Select',
        field: 'SourceFab',
        componentProps: () => {
          return {
            options: optionsdata.SourceFabLabel,
            placeholder: 'Source Fab',
          }
        },
        defaultValue: 'ALL'
      }
    ]; 
    
  }
  // api請求數據
  async init(optionsdata: NGBSelectType, FactoryType: string) {
    await mountedSourceFab(optionsdata, FactoryType);  
  }

  // 獲取Marterial
  get obj() {
    return this._obj;
   }
   // 更改label
   setLabel(index: number, LabelValue: string) {
       this._obj[index]['label'] = LabelValue;
       return this;
    }
}



//WoTypeBySap
export class WoTypeBySapParams{
  optionsdata:NGBSelectType;
  FactoryType: string;
  _obj: FormSchema[];
  constructor(
    optionsdata:NGBSelectType,
    FactoryType: string,
  ){
    this.optionsdata = optionsdata;
    this.FactoryType = FactoryType;
    optionsdata['FactoryType'] = this.FactoryType;
    this.init(optionsdata, this.FactoryType);
    this._obj =[ 
      {
        label: '工單性質選項分類',
        component: 'Select',
        field: 'WoType',
        componentProps: () => {
          return {
            mode: 'multiple',
            maxTagCount:5,
            options: optionsdata.WoTypeBySapLabel, 
            placeholder: 'WoType',
          }
        },
        //defaultValue:[''],
      },
    ]; 
    
  }
  // api請求數據
  async init(optionsdata: NGBSelectType, FactoryType: string) {
    await mountedWoTypeBySap(optionsdata, FactoryType);  
  }

  // 獲取Marterial
  get obj() {
    return this._obj;
   }
   // 更改label
   setLabel(index: number, LabelValue: string) {
       this._obj[index]['label'] = LabelValue;
       return this;
    }
}



//Quality Report For Down Grade  ===>WoTypeBySap2
export const WoTypeBySap2Params: FormSchema[] = [
  {
    label: '工單性質選項分類',
    component: 'Select',
    field: 'WoType',
    componentProps: {
      mode: 'multiple',
      options: [
        {
          label: 'ALL',
          value: 'ALL'
        },
        {
          label: 'MP',
          value: "MP"
        },
        {
          label: 'MH',
          value: "MH"
        },
        {
          label: 'MK',
          value: "MK"
        },
        {
          label: 'DG',
          value: "DG"
        },
        {
          label: 'DH',
          value: "DH"
        },
        {
          label: 'ME',
          value: "ME"
        },
        {
          label: 'RY',
          value: "RK"
        },
        {
          label: 'RG',
          value: "RG"
        },
        {
          label: 'RI',
          value: "RI"
        },
        {
          label: 'IN',
          value: "IN"
        },
        {
          label: 'OA',
          value: "OA"
        },
        {
          label: 'RO',
          value: "RO"
        },
        
      ],
    },
    defaultValue: ['ALL']
  }
]


//LCM Equipment 查詢
export class LcmEquipmentParams{
  optionsdata:NGBSelectType;
  FactoryType: string;
  _obj: FormSchema[];
  constructor(
    optionsdata:NGBSelectType,
    FactoryType: string,
  ){
    this.optionsdata = optionsdata;
    this.FactoryType = FactoryType;
    optionsdata['FactoryType'] = this.FactoryType;
    this.init(optionsdata, this.FactoryType);
    this._obj =[ 
      {
        label: 'Big Equipment',
        component: 'Select',
        field: 'BIGEQP',
        componentProps: ({ formModel, field }) => {
          return {   
            mode: 'multiple',  
            options: optionsdata.LCMBigEquipmentLabel,
            placeholder: 'BigEquipment',
            onChange: async (e: any) => {
              formModel.Equipment=[]; 
              if (e !== 'ALL') {
                //optionsdata.ModuleProductData1 = optionsdata.ModuleProductData.filter(item => item?.value.indexOf(e) != -1)
                optionsdata.LCMEquipment1 = optionsdata.LCMEquipment = await getCommonFormParams('EQUIP_NBR', {
                  name: "Date",
                  apiName: "Equipment",
                  FactoryType: optionsdata['FactoryType'],
                  G_FAC: useCommonParams.G_FAC_COM,
                  BigEquipment: formModel.BIGEQP,
                })
    
              } else {
                optionsdata.LCMEquipment1 = optionsdata.LCMEquipment
              }
    
            },
          };
        },
      },
      {
        label: 'Equipment',
        field: 'Equipment',
        component: 'Select',
        componentProps: ({ formModel, field }) => {
              return {
                mode: 'multiple',
                options: optionsdata.LCMEquipment1,
                maxTagCount:10,
                placeholder: 'Equipment',
                onChange: async (e: any) => {
                  formModel.Equipment=[];
                  if (e.indexOf('ALL')!= -1&& ifSelectall4!=true) {
                    let SelectData: string[] = []  
                     optionsdata.LCMEquipment1!.forEach(item => {
                      SelectData.push(item.value)
                    });
                    formModel[field] = SelectData
                    ifSelectall4=true
                  }else if (e.indexOf('ALL')!= -1&& ifSelectall4===true) {
                    e.map((val,i)=>{
                       if(val==="ALL"){
                        e.splice(i,1);
                       }
                    })
                    
                    formModel[field]=e          
                    ifSelectall4=false
                  }
                  else {
                  
                    if(e.indexOf('ALL')=== -1 && ifSelectall4===true)
                    {
                      formModel[field]=[]
                      ifSelectall4=false
                      
                    }else{
                      formModel[field]=e
                    }
                    
                  }
          
                }
              }
    
         
        },
      },
    ]; 
    
  }
  // api請求數據
  async init(optionsdata: NGBSelectType, FactoryType: string) {
    await mountedLCMBigEquipment(optionsdata, FactoryType);  
  }

  // 獲取Marterial
  get obj() {
    return this._obj;
   }
   // 更改label
   setLabel(index: number, LabelValue: string) {
       this._obj[index]['label'] = LabelValue;
       return this;
    }
}





//Include B/L
export const IncludeBLParams: FormSchema[] = [
  {
    label: 'Include B/L',
    component: 'RadioGroup',
    field: 'IncludeBL',

    componentProps: {
      options: [
        {
          label: 'YES',
          value: 'YES'
        },
        {
          label: 'NO',
          value: "NO"
        }
        
      ],
    },
    defaultValue: 'YES'

  },
];


//LCMDefectCode
export class LCMDefectCodeParams{
  optionsdata:NGBSelectType;
  FactoryType: string;
  _obj: FormSchema[];
  constructor(
    optionsdata:NGBSelectType,
    FactoryType: string,
  ){
    this.optionsdata = optionsdata;
    this.FactoryType = FactoryType;
    optionsdata['FactoryType'] = this.FactoryType;
    this.init(optionsdata, this.FactoryType);
    this._obj =[ 
      {
        label: 'common.commonpages.DefectCode',
        component: 'Select',
        field: 'DefectCode',
        componentProps: () => {
          return {
            mode: 'multiple',
            maxTagCount:10,
            options: optionsdata.LCMDefectCodeLabel,
            placeholder: 'DefectCode',
          };
        },
        //defaultValue:['ALL'],
      },
    ]; 
    
  }
  // api請求數據
  async init(optionsdata: NGBSelectType, FactoryType: string) {
    await mountedLCMDefectCode(optionsdata, FactoryType);  
  }

  // 獲取Marterial
  get obj() {
    return this._obj;
   }
   // 更改label
   setLabel(index: number, LabelValue: string) {
       this._obj[index]['label'] = LabelValue;
       return this;
    }
}



//Panel Grade
export const PanelGradeParams:FormSchema[]=[
  {label: 'Panel Grade',
  component: 'Select',
  field: 'Reworked',
  componentProps: {
    mode: 'multiple',
    options: [
      {
        label: 'ALL',
        value: 'ALL'
      },
      {
        label: 'A',
        value: "A"
      },
      {
        label: 'B',
        value: "B"
      },
      {
        label: 'C2',
        value: "C2"
      },
      {
        label: 'C3',
        value: "C3"
      },
      {
        label: 'C4',
        value: "C4"
      },
      {
        label: 'C5',
        value: "C5"
      },
      {
        label: 'C6',
        value: "C6"
      },
      {
        label: 'C',
        value: "C"
      },
      {
        label: 'D',
        value: "D"
      },
      {
        label: 'E',
        value: "E"
      },
      {
        label: 'W',
        value: "W"
      },
      {
        label: 'K',
        value: "K"
      },
      {
        label: 'G',
        value: "G"
      },
      {
        label: 'L',
        value: "L"
      },
    ],
  },
  defaultValue: ['ALL']
 }
]


//byLCD Size PRODUCT查詢料號
export class LCDBySizeProdParams{
  optionsdata:NGBSelectType;
   FactoryType: string;
   obj: FormSchema[];
   constructor(
     optionsdata:NGBSelectType,
     FactoryType: string,

   ){
     this.optionsdata = optionsdata;
     this.FactoryType = FactoryType;
     optionsdata['FactoryType'] = this.FactoryType;
     this.init(optionsdata, this.FactoryType);
     this.obj = this.setobj();
   }
   // api請求數據
   async init(optionsdata: NGBSelectType, FactoryType: string) {
     await mountedLcdSize(optionsdata, FactoryType);
   }
   // 初始化obj
   setobj(optionsdata: NGBSelectType = this.optionsdata) {
     this.obj = [
      {
        field: '#LCDDivider',
        component: 'Divider',
        label: 'LCD Product'
      },
       {
         label: 'common.commonpages.size',
         component: 'Select',
         field: 'LCDSize',
         componentProps: ({ formModel, field }) => {
           return {
             mode: 'multiple',
             options: optionsdata.LcdSizeLabel,
             placeholder: 'size',
             onChange: async (e: any) => {
                formModel.Prod_nbr=[];
                 optionsdata.ProductData1 = optionsdata.ProductData= await getCommonFormParams('PART', {
                   name: "Date",
                   apiName: 'LcdProduct',
                   FactoryType: optionsdata['FactoryType'],
                   G_FAC: useCommonParams.G_FAC_COM,
                   Size: e
                 })  
             },
           };
         },
         //defaultValue:'ALL'
       },
       {
         label: 'Product (限制1000筆)',
         component: 'Select',
         field: 'LcdProduct',
         componentProps: ({ formModel, field }) => {
           return {
             mode: 'multiple',
             options: optionsdata.ProductData1,
             placeholder: 'LCD product',
             maxTagCount:10,
             onChange: async (e: any) => {
               formModel.Prod_nbr=[];
               if (e.indexOf('ALL')!= -1&& ifSelectall2!=true) {
                 let SelectData: string[] = []  
                  optionsdata.ProductData1!.forEach(item => {
                   SelectData.push(item.value)
                 });
                 formModel[field] = SelectData
                 ifSelectall2=true
               }else if (e.indexOf('ALL')!= -1&& ifSelectall2===true) {
                 e.map((val,i)=>{
                    if(val==="ALL"){
                     e.splice(i,1);
                    }
                 })
                 
                 formModel[field]=e          
                 ifSelectall2=false
               }
               else {
               
                 if(e.indexOf('ALL')=== -1 && ifSelectall2===true)
                 {
                   formModel[field]=[]
                   ifSelectall2=false
                   
                 }else{
                   formModel[field]=e
                 }
                 
               }
       
       
               if(formModel[field].length>1000){
                 let prodnum=formModel[field].length
                  message.error('目前料號數"'+prodnum+' “超過最大限制筆數:1000” '); 
                }
        
             }
           };
         },
         //defaultValue:'ALL'
       },
    ];
     return this.obj;
   }
    // 返回obj
    getSize() {
     return this.obj;
 }
 // 更改label
 setLabel(index: number, LabelValue: string) {
     this.obj[index]['label'] = LabelValue;
     return this;
 }
}




///***************BL 部分組件******************/////
//byBLSize 查詢料號
export class BLBySizeProdParams{
   optionsdata:NGBSelectType;
    FactoryType: string;
    obj: FormSchema[];
    constructor(
      optionsdata:NGBSelectType,
      FactoryType: string,

    ){
      this.optionsdata = optionsdata;
      this.FactoryType = FactoryType;
      optionsdata['FactoryType'] = this.FactoryType;
      this.init(optionsdata, this.FactoryType);
      this.obj = this.setobj();
    }
    // api請求數據
    async init(optionsdata: NGBSelectType, FactoryType: string) {
      await mounted(optionsdata, FactoryType);
    }
    // 初始化obj
    setobj(optionsdata: NGBSelectType = this.optionsdata) {
      this.obj = [
        {
          label: 'common.commonpages.size',
          component: 'Select',
          field: 'Size',
          componentProps: ({ formModel, field }) => {
            return {
              mode: 'multiple',
              options: optionsdata.SizeData,
              placeholder: 'size',
              onChange: async (e: any) => {
                 formModel.Product=[]; 
                 formModel.Prod_nbr=[];
                  optionsdata.BigProductData1 = optionsdata.BigProductData= await getCommonFormParams('PART', {
                    name: "Date",
                    apiName: 'BigProduct',
                    FactoryType: optionsdata['FactoryType'],
                    G_FAC: useCommonParams.G_FAC_COM,
                    Size: e
                  })  
              },
            };
          },
          //defaultValue:'ALL'
        },
        {
          label: 'Product (限制1000筆)',
          component: 'Select',
          field: 'Prod_nbr',
          componentProps: ({ formModel, field }) => {
            return {
              mode: 'multiple',
              options: optionsdata.BigProductData1,
              placeholder: 'big product',
              maxTagCount:10,
              onChange: async (e: any) => {
                formModel.Prod_nbr=[];
                if (e.indexOf('ALL')!= -1&& ifSelectall3!=true) {
                  let SelectData: string[] = []  
                   optionsdata.BigProductData1!.forEach(item => {
                    SelectData.push(item.value)
                  });
                  formModel[field] = SelectData
                  ifSelectall3=true
                }else if (e.indexOf('ALL')!= -1&& ifSelectall3===true) {
                  e.map((val,i)=>{
                     if(val==="ALL"){
                      e.splice(i,1);
                     }
                  })
                  
                  formModel[field]=e          
                  ifSelectall3=false
                }
                else {
                
                  if(e.indexOf('ALL')=== -1 && ifSelectall3===true)
                  {
                    formModel[field]=[]
                    ifSelectall3=false
                    
                  }else{
                    formModel[field]=e
                  }
                  
                }
        
        
                if(formModel[field].length>1000){
                  let prodnum=formModel[field].length
                   message.error('目前料號數"'+prodnum+' “超過最大限制筆數:1000” '); 
                 }
         
              }
            };
          },
          //defaultValue:'ALL'
        },
     ];
      return this.obj;
    }
     // 返回obj
     getSize() {
      return this.obj;
  }
  // 更改label
  setLabel(index: number, LabelValue: string) {
      this.obj[index]['label'] = LabelValue;
      return this;
  }
}


//LCMOEEBigEquipment
export class LCMOEEBigEquipmentParams{
  optionsdata:NGBSelectType;
  FactoryType: string;
  _obj: FormSchema[];
  constructor(
    optionsdata:NGBSelectType,
    FactoryType: string,
  ){
    this.optionsdata = optionsdata;
    this.FactoryType = FactoryType;
    optionsdata['FactoryType'] = this.FactoryType;
    this.init(optionsdata, this.FactoryType);
    this._obj =[ 
      {
        label: 'Equip Group',
        component: 'Select',
        field: 'EquipGroup',
        componentProps: ({ formModel, field }) => {
          return {   
            mode: 'multiple',  
            options: optionsdata.LCMBigEquipmentLabel,
            placeholder: 'EquipGroup',
            onChange: async (e: any) => {
              formModel.Equipment=[]; 
              if (e !== 'ALL') {
                //optionsdata.ModuleProductData1 = optionsdata.ModuleProductData.filter(item => item?.value.indexOf(e) != -1)
                optionsdata.LCMEquipment1 = optionsdata.LCMEquipment = await getCommonFormParams('EQUIP_NBR', {
                  name: "Date",
                  apiName: "OEEEquipment",
                  FactoryType: optionsdata['FactoryType'],
                  G_FAC: useCommonParams.G_FAC_COM,
                  BigEquipment: formModel.EquipGroup,
                })
    
              } else {
                optionsdata.LCMEquipment1 = optionsdata.LCMEquipment
              }
    
            },
          };
        },
      },
      {
        label: 'Equipment',
        field: 'Equipment',
        component: 'Select',
        componentProps: ({ formModel, field }) => {
              return {
                mode: 'multiple',
                options: optionsdata.LCMEquipment1,
                maxTagCount:10,
                placeholder: 'Equipment',
                onChange: async (e: any) => {
                  formModel.Equipment=[];
                  if (e.indexOf('ALL')!= -1&& ifSelectall4!=true) {
                    let SelectData: string[] = []  
                     optionsdata.LCMEquipment1!.forEach(item => {
                      SelectData.push(item.value)
                    });
                    formModel[field] = SelectData
                    ifSelectall4=true
                  }else if (e.indexOf('ALL')!= -1&& ifSelectall4===true) {
                    e.map((val,i)=>{
                       if(val==="ALL"){
                        e.splice(i,1);
                       }
                    })
                    
                    formModel[field]=e          
                    ifSelectall4=false
                  }
                  else {
                  
                    if(e.indexOf('ALL')=== -1 && ifSelectall4===true)
                    {
                      formModel[field]=[]
                      ifSelectall4=false
                      
                    }else{
                      formModel[field]=e
                    }
                    
                  }
          
                }
              }
    
         
        },
      },
    ]; 
    
  }
  // api請求數據
  async init(optionsdata: NGBSelectType, FactoryType: string) {
    await mountedLCMOEEBigEquipment(optionsdata, FactoryType);  
  }

  // 獲取Marterial
  get obj() {
    return this._obj;
   }
   // 更改label
   setLabel(index: number, LabelValue: string) {
       this._obj[index]['label'] = LabelValue;
       return this;
    }
}


///***************SPC 部分組件******************/////
  //SPC EQPTID
  export class EqptIdParams{
    optionsdata:NGBSelectType;
    FactoryType: string;
    _obj: FormSchema[];
    constructor(
      optionsdata:NGBSelectType,
      FactoryType: string,
    ){
      this.optionsdata = optionsdata;
      this.FactoryType = FactoryType;
      optionsdata['FactoryType'] = this.FactoryType;
      this.init(optionsdata, this.FactoryType);
      this._obj =[ 
        {
          label: 'EQPTID',
          field: 'EQPTID',
          component: 'Select',
          componentProps: () => {
                return {
                  options: optionsdata.SpcEqptIdLabel,
                  placeholder: 'EQPTID'
                }
          },
          defaultValue: 'ALL'
        },
      ]; 
      
    }
    // api請求數據
    async init(optionsdata: NGBSelectType, FactoryType: string) {
      await mountedEqptId(optionsdata, FactoryType);  
    }
  
    // 獲取Marterial
    get obj() {
      return this._obj;
     }
     // 更改label
     setLabel(index: number, LabelValue: string) {
         this._obj[index]['label'] = LabelValue;
         return this;
      }
  
  }
  
  
  //SPC PROD
  export class ProdParams{
    optionsdata:NGBSelectType;
    FactoryType: string;
    _obj: FormSchema[];
    constructor(
      optionsdata:NGBSelectType,
      FactoryType: string,
    ){
      this.optionsdata = optionsdata;
      this.FactoryType = FactoryType;
      optionsdata['FactoryType'] = this.FactoryType;
      this.init(optionsdata, this.FactoryType);
      this._obj =[ 
        {
          label: 'PROD',
          field: 'Prod',
          component: 'Select',
          componentProps: () => {
                return {
                  options: optionsdata.SpcProdLabel,
                  placeholder: 'Prod'
                }
          },
          defaultValue: 'ALL'
        },
      ]; 
      
    }
    // api請求數據
    async init(optionsdata: NGBSelectType, FactoryType: string) {
      await mountedSpcProd(optionsdata, FactoryType);  
    }
  
    // 獲取Marterial
    get obj() {
      return this._obj;
      }
      // 更改label
      setLabel(index: number, LabelValue: string) {
          this._obj[index]['label'] = LabelValue;
          return this;
      }
  
  
  }
  
  //SPC DATA GROUP
  export class GroupParams{
    optionsdata:NGBSelectType;
    FactoryType: string;
    _obj: FormSchema[];
    constructor(
      optionsdata:NGBSelectType,
      FactoryType: string,
    ){
      this.optionsdata = optionsdata;
      this.FactoryType = FactoryType;
      optionsdata['FactoryType'] = this.FactoryType;
      this.init(optionsdata, this.FactoryType);
      this._obj =[ 
        {
          label: 'DATA GROUP',
          field: 'DATAGROUP',
          component: 'Select',
          componentProps: () => {
                return {
                  options: optionsdata.SpcGroupLabel,
                  placeholder: 'DATAGROUP'
                }
          },
          defaultValue: 'ALL'
        },
      ]; 
      
    }
    // api請求數據
    async init(optionsdata: NGBSelectType, FactoryType: string) {
      await mountedDataGroup(optionsdata, FactoryType);  
    }
  
    // 獲取Marterial
    get obj() {
      return this._obj;
      }
      // 更改label
      setLabel(index: number, LabelValue: string) {
          this._obj[index]['label'] = LabelValue;
          return this;
      }
  }
      
  //SPC Module Name
  export class ModuleNameParams{
    optionsdata:NGBSelectType;
    FactoryType: string;
    _obj: FormSchema[];
    constructor(
      optionsdata:NGBSelectType,
      FactoryType: string,
    ){
      this.optionsdata = optionsdata;
      this.FactoryType = FactoryType;
      optionsdata['FactoryType'] = this.FactoryType;
      this.init(optionsdata, this.FactoryType);
      this._obj =[ 
        {
          label: 'Module Name',
          field: 'ModuleName',
          component: 'Select',
          componentProps: () => {
                return {
                  options: optionsdata.SpcModuleNameLabel,
                  placeholder: 'ModuleName'
                }
          },
          defaultValue: 'ALL'
        },
      ]; 
      
    }
    // api請求數據
    async init(optionsdata: NGBSelectType, FactoryType: string) {
      await mountedSpcModuleName(optionsdata, useCommonParams.Fab);  
    }
  
    // 獲取Marterial
    get obj() {
      return this._obj;
      }
      // 更改label
      setLabel(index: number, LabelValue: string) {
          this._obj[index]['label'] = LabelValue;
          return this;
      }
  
  
  }
  
  //SPC Meas Tool
  export class MeasToolParams{
    optionsdata:NGBSelectType;
    FactoryType: string;
    _obj: FormSchema[];
    constructor(
      optionsdata:NGBSelectType,
      FactoryType: string,
    ){
      this.optionsdata = optionsdata;
      this.FactoryType = FactoryType;
      optionsdata['FactoryType'] = this.FactoryType;
      this.init(optionsdata, this.FactoryType);
      this._obj =[ 
        {
          label: 'Meas Tool',
          field: 'MeasTool',
          component: 'Select',
          componentProps: () => {
                return {
                  options: optionsdata.SpcMeasToolLabel,
                  placeholder: 'MeasTool'
                }
          },
          defaultValue: 'ALL'
        },
      ]; 
      
    }
    // api請求數據
    async init(optionsdata: NGBSelectType, FactoryType: string) {
      await mountedSpcMeasTool(optionsdata, useCommonParams.Fab);  
    }
  
    // 獲取Marterial
    get obj() {
      return this._obj;
      }
      // 更改label
      setLabel(index: number, LabelValue: string) {
          this._obj[index]['label'] = LabelValue;
          return this;
      }
  
  
  }
  
  //SPC ProcTool
  export class ProcToolParams{
    optionsdata:NGBSelectType;
    FactoryType: string;
    _obj: FormSchema[];
    constructor(
      optionsdata:NGBSelectType,
      FactoryType: string,
    ){
      this.optionsdata = optionsdata;
      this.FactoryType = FactoryType;
      optionsdata['FactoryType'] = this.FactoryType;
      this.init(optionsdata, this.FactoryType);
      this._obj =[ 
        {
          label: 'Proc Tool',
          field: 'ProcTool',
          component: 'Select',
          componentProps: () => {
                return {
                  options: optionsdata.SpcProcToolLabel,
                  placeholder: 'ProcTool'
                }
          },
          defaultValue: 'ALL'
        },
      ]; 
      
    }
    // api請求數據
    async init(optionsdata: NGBSelectType, FactoryType: string) {
      await mountedSpcProcTool(optionsdata, useCommonParams.Fab);  
    }
  
    // 獲取Marterial
    get obj() {
      return this._obj;
      }
      // 更改label
      setLabel(index: number, LabelValue: string) {
          this._obj[index]['label'] = LabelValue;
          return this;
      }
  
  
  }
  
  //SPC Line
  export class LineParams{
    optionsdata:NGBSelectType;
    FactoryType: string;
    _obj: FormSchema[];
    constructor(
      optionsdata:NGBSelectType,
      FactoryType: string,
    ){
      this.optionsdata = optionsdata;
      this.FactoryType = FactoryType;
      optionsdata['FactoryType'] = this.FactoryType;
      this.init(optionsdata, this.FactoryType);
      this._obj =[ 
        {
          label: 'Line',
          field: 'Line',
          component: 'Select',
          componentProps: () => {
                return {
                  options: optionsdata.SpcLineLabel,
                  placeholder: 'Line'
                }
          },
          defaultValue: 'ALL'
        },
      ]; 
      
    }
    // api請求數據
    async init(optionsdata: NGBSelectType, FactoryType: string) {
      await mountedSpcLine(optionsdata, useCommonParams.Fab);  
    }
  
    // 獲取Marterial
    get obj() {
      return this._obj;
      }
      // 更改label
      setLabel(index: number, LabelValue: string) {
          this._obj[index]['label'] = LabelValue;
          return this;
      }
  
  
  }
  
  //SPC Before Hours
  export class BeforeHoursParams{
    optionsdata:NGBSelectType;
    FactoryType: string;
    _obj: FormSchema[];
    constructor(
      optionsdata:NGBSelectType,
      FactoryType: string,
    ){
      this.optionsdata = optionsdata;
      this.FactoryType = FactoryType;
      optionsdata['FactoryType'] = this.FactoryType;
      this.init(optionsdata, this.FactoryType);
      this._obj =[ 
        {
          label: 'Before (Hours)',
          field: 'BeforeHours',
          component: 'Select',
          componentProps: () => {
                return {
                  options: optionsdata.SpcBeforeHoursLabel,
                  placeholder: 'BeforeHours'
                }
          },
          defaultValue: '1'
        },
      ]; 
      
    }
    // api請求數據
    async init(optionsdata: NGBSelectType, FactoryType: string) {
      await mountedSpcBeforeHours(optionsdata, useCommonParams.Fab);  
    }
  
    // 獲取Marterial
    get obj() {
      return this._obj;
      }
      // 更改label
      setLabel(index: number, LabelValue: string) {
          this._obj[index]['label'] = LabelValue;
          return this;
      }
  }
  //Spc PreLine
  export class PreLineParams{
    optionsdata:NGBSelectType;
    FactoryType: string;
    _obj: FormSchema[];
    constructor(
      optionsdata:NGBSelectType,
      FactoryType: string,
    ){
      this.optionsdata = optionsdata;
      this.FactoryType = FactoryType;
      optionsdata['FactoryType'] = this.FactoryType;
      this.init(optionsdata, this.FactoryType);
      this._obj =[ 
        {
          label: 'Pre_Line',
          field: 'PreLine',
          component: 'Select',
          componentProps: () => {
                return {
                  options: optionsdata.SpcPreLineLabel,
                  placeholder: 'PreLine'
                }
          },
          defaultValue: 'CMO01'
        },
      ]; 
      
    }
    // api請求數據
    async init(optionsdata: NGBSelectType, FactoryType: string) {
      await mountedSpcPreLine(optionsdata, useCommonParams.Fab);  
    }
  
    // 獲取Marterial
    get obj() {
      return this._obj;
      }
      // 更改label
      setLabel(index: number, LabelValue: string) {
          this._obj[index]['label'] = LabelValue;
          return this;
      }
  }
  //SPC ProdID
  export class ProdIDParams{
    optionsdata:NGBSelectType;
    FactoryType: string;
    _obj: FormSchema[];
    constructor(
      optionsdata:NGBSelectType,
      FactoryType: string,
    ){
      this.optionsdata = optionsdata;
      this.FactoryType = FactoryType;
      optionsdata['FactoryType'] = this.FactoryType;
      this.init(optionsdata, this.FactoryType);
      this._obj =[ 
        {
          label: 'Prod',
          field: 'ProdID',
          component: 'Select',
          componentProps: () => {
                return {
                  options: optionsdata.SpcProdIDLabel,
                  placeholder: 'ProdID'
                }
          },
          defaultValue: 'ALL'
        },
      ]; 
      
    }
    // api請求數據
    async init(optionsdata: NGBSelectType, FactoryType: string) {
      await mountedSpcProdID(optionsdata, useCommonParams.Fab); 
    }
  
    // 獲取Marterial
    get obj() {
      return this._obj;
      }
      // 更改label
      setLabel(index: number, LabelValue: string) {
          this._obj[index]['label'] = LabelValue;
          return this;
      }
  }
  //SPC PROD NUM
  export class ProdNumParams{
    optionsdata:NGBSelectType;
    FactoryType: string;
    _obj: FormSchema[];
    constructor(
      optionsdata:NGBSelectType,
      FactoryType: string,
    ){
      this.optionsdata = optionsdata;
      this.FactoryType = FactoryType;
      optionsdata['FactoryType'] = this.FactoryType;
      this.init(optionsdata, this.FactoryType);
      this._obj =[ 
        {
          label: 'SPC PROD',
          field: 'ProdNum',
          component: 'Select',
          componentProps: () => {
                return {
                  options: optionsdata.SpcProdNumLabel,
                  placeholder: 'ProdNum'
                }
          },
          defaultValue: 'ALL'
        },
      ]; 
      
    }
    // api請求數據
    async init(optionsdata: NGBSelectType, FactoryType: string) {
      await mountedSpcProdNum(optionsdata, useCommonParams.Fab);   
    }
  
    // 獲取Marterial
    get obj() {
      return this._obj;
      }
      // 更改label
      setLabel(index: number, LabelValue: string) {
          this._obj[index]['label'] = LabelValue;
          return this;
      }
  }
  //SPC Cpk Performance Report
  //Control Item
  export class OperParams{
    optionsdata:NGBSelectType;
    FactoryType: string;
    _obj: FormSchema[];
    constructor(
      optionsdata:NGBSelectType,
      FactoryType: string,
    ){
      this.optionsdata = optionsdata;
      this.FactoryType = FactoryType;
      optionsdata['FactoryType'] = this.FactoryType;
      this.init(optionsdata, this.FactoryType);
      this._obj =[ 
        {
          label: 'Control Item',
          field: 'Oper',
          component: 'Select',
          componentProps: () => {
                return {
                  options: optionsdata.SpcOperLabel,
                  placeholder: 'Oper'
                }
          }
        },
      ]; 
      
    }
    // api請求數據
    async init(optionsdata: NGBSelectType, FactoryType: string) {
      await mountedSpcOper(optionsdata, useCommonParams.Fab);   
    }
  // 獲取Marterial
  get obj() {
    return this._obj;
    }
    // 更改label
    setLabel(index: number, LabelValue: string) {
        this._obj[index]['label'] = LabelValue;
        return this;
    }
  }
    // Product
  export class ProductParams{
    optionsdata:NGBSelectType;
    FactoryType: string;
    _obj: FormSchema[];
    constructor(
      optionsdata:NGBSelectType,
      FactoryType: string,
    ){
      this.optionsdata = optionsdata;
      this.FactoryType = FactoryType;
      optionsdata['FactoryType'] = this.FactoryType;
      this.init(optionsdata, this.FactoryType);
      this._obj =[ 
        {
          label: 'Product',
          field: 'Product',
          component: 'Select',
          componentProps: () => {
                return {
                  options: optionsdata.SpcProductLabel,
                  placeholder: 'Product'
                }
          }
        },
      ]; 
      
    }
    // api請求數據
    async init(optionsdata: NGBSelectType, FactoryType: string) {
      await mountedSpcProduct(optionsdata, useCommonParams.Fab);   
    }
  // 獲取Marterial
  get obj() {
    return this._obj;
    }
    // 更改label
    setLabel(index: number, LabelValue: string) {
        this._obj[index]['label'] = LabelValue;
        return this;
    }
  }
  
    //Process
  export class ProcessParams{
    optionsdata:NGBSelectType;
    FactoryType: string;
    _obj: FormSchema[];
    constructor(
      optionsdata:NGBSelectType,
      FactoryType: string,
    ){
      this.optionsdata = optionsdata;
      this.FactoryType = FactoryType;
      optionsdata['FactoryType'] = this.FactoryType;
      this.init(optionsdata, this.FactoryType);
      this._obj =[ 
        {
          label: 'Process',
          field: 'Process',
          component: 'Select',
          componentProps: () => {
                return {
                  options: optionsdata.SpcProcessLabel,
                  placeholder: 'Process'
                }
          }
        },
      ]; 
      
    }
    // api請求數據
    async init(optionsdata: NGBSelectType, FactoryType: string) {
      await mountedSpcProcess(optionsdata, useCommonParams.Fab);   
    }
  // 獲取Marterial
  get obj() {
    return this._obj;
    }
    // 更改label
    setLabel(index: number, LabelValue: string) {
        this._obj[index]['label'] = LabelValue;
        return this;
    }
  }
  
    //Line
  export class PerLineParams{
    optionsdata:NGBSelectType;
    FactoryType: string;
    _obj: FormSchema[];
    constructor(
      optionsdata:NGBSelectType,
      FactoryType: string,
    ){
      this.optionsdata = optionsdata;
      this.FactoryType = FactoryType;
      optionsdata['FactoryType'] = this.FactoryType;
      this.init(optionsdata, this.FactoryType);
      this._obj =[ 
        {
          label: 'Line',
          field: 'Line',
          component: 'Select',
          componentProps: () => {
                return {
                  options: optionsdata.SpcPerLineLabel,
                  placeholder: 'Line'
                }
          }
        },
      ]; 
      
    }
    // api請求數據
    async init(optionsdata: NGBSelectType, FactoryType: string) {
      await mountedSpcPerLine(optionsdata, useCommonParams.Fab);   
    }
    // 獲取Marterial
    get obj() {
      return this._obj;
      }
      // 更改label
      setLabel(index: number, LabelValue: string) {
          this._obj[index]['label'] = LabelValue;
          return this;
      }
    }

///***************MOD 部分組件******************/////
// 大中小 Rwk
// export class ReworkedOptionParams{
//   optionsdata:NGBSelectType;
//   FactoryType: string;
//   _obj: FormSchema[];
//   constructor(
//     optionsdata:NGBSelectType,
//     FactoryType: string,
//   ){
//     this.optionsdata = optionsdata;
//     this.FactoryType = FactoryType;
//     optionsdata['FactoryType'] = this.FactoryType;
//     this.init(optionsdata, this.FactoryType);
//     this._obj =[ 
//       {
//         label: '大小rwk',
//         component: 'Select',
//         field: 'ReworkedOption',
//         componentProps: () => {
//           return {
//              options: optionsdata.ReworkedOption,
//             placeholder: '請選擇',
//           }
//         },
        
//       }
//     ]; 
    
//   }
//   // api請求數據
//   async init(optionsdata: NGBSelectType, FactoryType: string) {
//     await mountedReworkedOption(optionsdata, FactoryType);  
//   }

//   // 獲取Marterial
//   get obj() {
//     return this._obj;
//    }
//    // 更改label
//    setLabel(index: number, LabelValue: string) {
//        this._obj[index]['label'] = LabelValue;
//        return this;
//     }
// }
export const ReworkedOptionParams:FormSchema={
  label: '大中小 Rwk',
  component: 'Select',
  field: 'ReworkedOption',
  componentProps: {
    options: [    
      {
        label: '小RWK',
        value: "1"
      },
      {
        label: '大RWK',
        value: "2"
      },
      {
        label: '大RWK中小RWK',
        value: "3"
      },        
     ],
  },
}
// rwk 次數
export const ReworkedTimesParams:FormSchema={
  label: 'Rework 次數',
  component: 'Select',
  field: 'ReworkedTimes',
  componentProps: {
    options: [    
      {
        label: '1次',
        value: "=1"
      },
      {
        label: '1次以上',
        value: ">=1"
      },
      {
        label: '2次',
        value: "=2"
      },
      {
        label: '2次以上',
        value: ">=2"
      },
      {
        label: '3次',
        value: "=3"
      },
      {
        label: '3次以上',
        value: ">=3"
      },
      {
        label: '4次',
        value: "=4"
      },
      {
        label: '4次以上',
        value: ">=4"
      },
      {
        label: '5次',
        value: "=5"
      },
      {
        label: '5次以上',
        value: ">=5"
      },
    ],
  },
}
//站點2900默認選擇
export class OperationForINSParams{
  optionsdata:NGBSelectType;
  FactoryType: string;
  _obj: FormSchema[];
  constructor(
    optionsdata:NGBSelectType,
    FactoryType: string,
  ){
    this.optionsdata = optionsdata;
    this.FactoryType = FactoryType;
    optionsdata['FactoryType'] = this.FactoryType;
    this.init(optionsdata, this.FactoryType);
    this._obj =[ 
      {
        label:'Operation For Inspection',
        component: 'Select',
        field: 'OperationForINS',
        componentProps: () => {
          return {
            options: optionsdata.OperationForINS,
            placeholder: 'OperationForINS',
          }
        },
        defaultValue:['2900'],
      }
    ]; 
    
  }
  // api請求數據
  async init(optionsdata: NGBSelectType, FactoryType: string) {
    await mountedOperationForINS(optionsdata, FactoryType);  
  }

  // 獲取Marterial
  get obj() {
    return this._obj;
   }
   // 更改label
   setLabel(index: number, LabelValue: string) {
       this._obj[index]['label'] = LabelValue;
       return this;
    }
}
//站點
export class OperationForRWKParams{
  optionsdata:NGBSelectType;
  FactoryType: string;
  _obj: FormSchema[];
  constructor(
    optionsdata:NGBSelectType,
    FactoryType: string,
  ){
    this.optionsdata = optionsdata;
    this.FactoryType = FactoryType;
    optionsdata['FactoryType'] = this.FactoryType;
    this.init(optionsdata, this.FactoryType);
    this._obj =[ 
      {
        label: '站點',
        component: 'Select',
        field: 'OperationForRWK',
        componentProps: () => {
          return {
            mode: 'multiple',
            options: optionsdata.OperationForRWK,
            maxTagCount:10,
            //options: optionsdata.OperationLabel.map((item:Object) => { return { label: item['label'], value: item['value'].split('-')[0],} }),
            placeholder: 'OperationForRWK',
          };
        },
     
      },
    ]; 
    
  }
  // api請求數據
  async init(optionsdata: NGBSelectType, FactoryType: string) {
    await mountedOperationForRWK(optionsdata,  FactoryType );  
  }

  
  get obj() {
    return this._obj;
   }
   // 更改label
   setLabel(index: number, LabelValue: string) {
       this._obj[index]['label'] = LabelValue;
       return this;
    }
}
//不含dfctcode
export class LCMDefectCodeNoParams{
  optionsdata:NGBSelectType;
  FactoryType: string;
  _obj: FormSchema[];
  constructor(
    optionsdata:NGBSelectType,
    FactoryType: string,
  ){
    this.optionsdata = optionsdata;
    this.FactoryType = FactoryType;
    optionsdata['FactoryType'] = this.FactoryType;
    this.init(optionsdata, this.FactoryType);
    this._obj =[ 
      {
        label: '不含 DefectCode',
        component: 'Select',
        field: 'DefectCodeNO',
        componentProps: () => {
          return {
            mode: 'multiple',
            maxTagCount:10,
            options: optionsdata.LCMDefectCodeLabel,
            placeholder: 'DefectCodeNO',
          };
        },
        defaultValue:['ALL'],
      },
    ]; 
    
  }
  // api請求數據
  async init(optionsdata: NGBSelectType, FactoryType: string) {
    await mountedLCMDefectCode(optionsdata, FactoryType);  
  }

  // 獲取Marterial
  get obj() {
    return this._obj;
   }
   // 更改label
   setLabel(index: number, LabelValue: string) {
       this._obj[index]['label'] = LabelValue;
       return this;
    }
}








  

  
  
  
  





  


  
