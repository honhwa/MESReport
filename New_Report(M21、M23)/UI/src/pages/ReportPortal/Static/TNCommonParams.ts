import type { FormSchema } from '@/components/core/schema-form/';
import { formatToDate } from '@/utils/dateUtil';
import { optionsdata } from '@/pages/ReportPortal/Static/TNCommon-Form-schema';
import useRemoteSelect from '@/pages/ReportPortal/utils/TNuseRemoteSelect';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { cloneDeep } from 'lodash-es';
import dayjs from 'dayjs';

const useCommonParams = AddCommonPageParams();
const {
  mountedEQP, mountedTNSize, mountedLCMOwner, mountedOper, mountedLCDOwner, mountedLCDGrade, mountedDownGrade, 
  mountedDownGradeL2, mountedHoldCode, mountedErrorCode, mountedPanelSource, mountedDefectCode, mountedDateMonth
} = useRemoteSelect();
let ifSelectall=false// 是否全選標記
let ifProdChoose="bysize"//料號查詢是是否切換bysize by module（默認false）


//From開始時間
export const FromDateParams: FormSchema[] = [
  {
    field: 'vDate_s',
    component: 'DatePicker',
    label: 'common.commonpages.Date',
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
    field: 'vDate_e1',
    component: 'DatePicker',
    label: 'common.commonpages.To',


    componentProps: {
      valueFormat: 'YYYY-MM-DD'
    },
    defaultValue: formatToDate(),
    required: true
  },
];

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
    label: 'common.commonpages.TN_Floor',
    component: 'RadioGroup',
    field: 'floor',

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
        },
        {
          label: '4F',
          value: '4'
        },
        {
          label: '5F',
          value: '5'
        }
      ]
    },
    defaultValue: 'ALL'
  },
];

//來源廠別
export const SourceFabParams: FormSchema[] = [
  {
    label: 'common.commonpages.SourceFab',
    component: 'RadioGroup',
    field: 'SourceFab',
    componentProps: {
      options: [
        {
          label: 'ALL',
          value: ''
        },
        {
          label: 'CMO',
          value: "and source_fab<>'IDT'"
        },
        {
          label: 'IDT',
          value: "and source_fab='IDT'"
        },
      ],
    },
    defaultValue: ''
  },
];

//InputWorkorder
export const InputWorkorderParams: FormSchema[] = [
  {
    label: 'common.commonpages.WorkOrder',
    component: 'InputTextArea',
    field: 'wo_num',

  },
];

//Reworked次數
export const ReworkedParams:FormSchema={
  label: 'Reworked',
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

//快修種類
export const ReworkTypeParams: FormSchema[] = [
{
    label: '快修種類',
    component: 'RadioGroup',
    field: 'RWType',
    componentProps: {
      options: [
        {
          label: '快修NG視為OK',
          value: 'OK'
        },
        {
          label: '快修NG視為NG',
          value: "NG"
        },
        {
          label: '所有快修',
          value: "ALL"
        }
      ]
    },
    defaultValue: 'NG'
  }
 ];

 export const ReworkTypeOKParams: FormSchema[] = [
    {
        label: '快修種類',
        component: 'RadioGroup',
        field: 'RWType',
        componentProps: {
          options: [
            {
              label: '快修NG視為OK',
              value: 'OK'
            },
            {
              label: '快修NG視為NG',
              value: "NG"
            },
            {
              label: '所有快修',
              value: "ALL"
            }
          ]
        },
        defaultValue: 'NG'
      }
     ];

//MOD1 Plant
export const PlantParams: FormSchema[] = [  
  {
    label: 'Plant',
    component: 'RadioGroup',
    field: 'plant',

    componentProps: {
      options: [
        {
          label: 'ALL',
          value: 'ALL'
        },
        {
          label: 'MOD1',
          value: 'M011'
        },
        {
          label: 'M016',
          value: 'M016'
        }
      ]
    },
    defaultValue: 'ALL'
  },
];

//MOD1 TVSET
export const TVSETParams: FormSchema[] = [  
  {
    label: 'TVSET',
    component: 'RadioGroup',
    field: 'tvset',

    componentProps: {
      options: [
        {
          label: 'ALL',
          value: 'ALL'
        },
        {
          label: '是',
          value: 'Y'
        },
        {
          label: '否',
          value: 'N'
        }
      ]
    },
    defaultValue: 'ALL'
  },
];


export interface TNType {
  TN_SizeData?: any [];
  TN_ProductData?: any [];
  TN_BigProductData?: any [];
  TN_Opername?: any [];
  TN_LCMOwner?: any[];
  TN_ProductData1?: any [];
  TN_BigProductData1?: any [];
  TN_Opername1?: any [];
  TN_OperationLabel?: [],
  TN_LcdGrade?: any [];
  TN_LcdOwner?: any [];
  TN_HoldCode?: any [];
  TN_PanelSource?: any [];
  TN_DefectCode?: any [];
  TN_OperationByDownGradeLabel?: any [];
  TN_OperationByDownGradeL2Label?: any [];
  TN_ErrorCode?: any [];
  TN_EQPGroup?: any [];
  TN_EQPID?: any [];
  TN_EQPID1?: any[];
  TN_DateMonth?: any[];
}

export type IEquipment = {
  equip: string;
  Optionequip: string;
  equipselectmenu: string;
  equipment: string;
};

// 獲取Equipment
export class Equipment {
  optionsdata: TNType;
  FactoryType: string;
  obj: FormSchema[];
  Type: string;
  fieldobj: IEquipment;
  /**
   *
   * @param optionsdata 下拉選擇框綁定的值
   * @param FactoryType 廠別
   * @param Type 0-3
   * @param fieldobj 字段動態名
   */
  constructor(
      optionsdata: TNType,
      FactoryType: string,
      Type: string,
      fieldobj: IEquipment = {
          equip: 'equip',
          Optionequip: 'Optionequip',
          equipselectmenu: '#optionequipmenu',
          equipment: 'EQPID'
      }
  ) {
      this.optionsdata = optionsdata;
      this.FactoryType = FactoryType;
      this.Type = Type;
      this.fieldobj = fieldobj;
      optionsdata['FactoryType'] = this.FactoryType;
      optionsdata['tempEquipment'] = [];
      this.init(optionsdata, this.FactoryType, Type);
      this.obj = this.setobj();
  }
  // api請求數據
  async init(optionsdata: TNType, FactoryType: string, Type: string) {
      await mountedEQP(optionsdata, FactoryType, Type);
  }
  // 初始化obj
  setobj(optionsdata: TNType = this.optionsdata) {
      this.obj = [
          {
            label: 'Equipment Group',
            component: 'Select',
            field: 'EQPGroup',
              componentProps: (model) => {
                  return {
                    options: optionsdata.TN_EQPGroup,
                    placeholder: 'EQP Group',
                      onChange: async (e: any) => {
                          if (e !== 'ALL') {
                              optionsdata.TN_EQPID1 = optionsdata.TN_EQPID!.filter(
                                  (item) => item?.value.indexOf(e) != -1
                              );
                          } else {
                              optionsdata.TN_EQPID1 = optionsdata.TN_EQPID;
                          }
                      },
                      onSelect: async (e: any) => {
                          // 默認選中第一個
                          // model.formModel.OptionProduct = optionsdata.ProductData1!.map((item) => item.value)[0];
                          model.formModel[this.fieldobj.Optionequip] = [];
                          model.formModel[this.fieldobj.equipselectmenu] = '';
                      }
                  };
              },
              defaultValue: 'ALL'
          },
          {
              label: '',
              // field: 'Optionequip',
              field: this.fieldobj.Optionequip,
              component: 'Select',
              componentProps: (model) => {
                  return {
                      mode: 'multiple',
                      listHeight: 300,
                      options: optionsdata.TN_EQPID1,
                      placeholder: 'EQP ID',
                      maxTagCount: 8,
                      onSelect: async (e: any, option) => {
                          model.formModel[this.fieldobj.equipment] = [
                              ...model.formModel[this.fieldobj.equipment],
                              option.label
                          ];
                          optionsdata['tempEquipment'] = model.formModel[this.fieldobj.equipment].map((item) => {
                              return {
                                  label: item,
                                  value: item
                              };
                          });
                          model.formModel[this.fieldobj.equipselectmenu] = '';
                      }
                  };
              },
              defaultValue: []
          },
          {
              label: '',
              // field: '#optionequipmenu',
              field: this.fieldobj.equipselectmenu,
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
                              let temp = optionsdata.TN_EQPID1!.map((item) => item.label);
                              model.formModel[this.fieldobj.Optionequip] = temp;
                              model.formModel[this.fieldobj.equipment] = [
                                  ...new Set([...model.formModel[this.fieldobj.equipment], ...temp])
                              ];
                              optionsdata['tempEquipment'] = model.formModel[this.fieldobj.equipment].map((item) => {
                                  return {
                                      label: item,
                                      value: item
                                  };
                              });
                          } else if (e.target.value == 'cancelselect') {
                              // 全消全選
                              model.formModel[this.fieldobj.equipment] = model.formModel[
                                  this.fieldobj.equipment
                              ].filter((item) => !model.formModel[this.fieldobj.Optionequip].includes(item));
                              model.formModel[this.fieldobj.Optionequip] = [];
                          }
                      }
                  };
              },
              defaultValue: ''
          },
          {
              label: '',
              // field: 'Equipment',
              field: this.fieldobj.equipment,
              component: 'Select',
              componentProps: (model) => {
                  return {
                      mode: 'multiple',
                      placeholder: 'Equipment',
                      options: optionsdata['tempEquipment'],
                      maxTagCount: 8,
                      // 取消選中，前面的也應該取消選中
                      onDeselect: (value, option) => {
                          let indexrow = optionsdata.TN_EQPID1!.find((element) => element.label == value);
                          if (indexrow) {
                              model.formModel[this.fieldobj.Optionequip] = model.formModel[
                                  this.fieldobj.Optionequip
                              ].filter((item) => {
                                  return item != indexrow.value;
                              });
                          }
                          if (model.formModel[this.fieldobj.equipment].length == 0) {
                              optionsdata['tempEquipment'] = [];
                          }
                      }
                  };
              },
              defaultValue: []
          }
      ];
      return this.obj;
  }
  // 返回obj
  getEquipment() {
      return this.obj;
  }
  // 更改label
  setLabel(index: number, LabelValue: string) {
      this.obj[index]['label'] = LabelValue;
      return this;
  }
}

export type Ifield = {
  size: string;
  bigproduct: string;
  optionproduct: string;
  selectmenu: string;
  product: string;
};

// 獲取廠別,size,product,bigproduct
export class Size {
  optionsdata: TNType;
  FactoryType: string;
  obj: FormSchema[];
  fieldobj: Ifield;
  constructor(
      optionsdata: TNType,
      FactoryType: string,
      fieldobj: Ifield = {
          size: 'Size',
          bigproduct: 'Product',
          optionproduct: 'OptionProduct',
          selectmenu: '#optionmenu',
          product: 'prod_nbr'
      }
  ) {
      this.optionsdata = optionsdata;
      this.FactoryType = FactoryType;
      this.fieldobj = fieldobj;
      optionsdata['FactoryType'] = this.FactoryType;
      optionsdata['tempoption'] = [];
      this.init(optionsdata, this.FactoryType);
      this.obj = this.setobj();
  }
  // api請求數據
  async init(optionsdata: TNType, FactoryType: string) {
      await mountedTNSize(optionsdata, FactoryType);
  }
  // 初始化obj
  setobj(optionsdata: TNType = this.optionsdata) {
      this.obj = [
          {
              label: 'common.commonpages.size',
              component: 'Select',
              // field: 'Size',
              field: this.fieldobj.size,
              componentProps: (model) => {
                  return {
                      options: optionsdata.TN_SizeData,
                      placeholder: 'size',
                      onChange: async (e: any) => {
                          if (e !== 'ALL') {
                              optionsdata.TN_BigProductData1 = optionsdata.TN_BigProductData!.filter(
                                  (item) => item?.value.indexOf(e) != -1
                              );
                          } else {
                              optionsdata.TN_BigProductData1 = optionsdata.TN_BigProductData;
                          }
                      },
                      onSelect: async (e: any) => {
                          // model.formModel.BigProduct = optionsdata.BigProductData1!.map((item) => item.value)[0];
                          model.formModel[this.fieldobj.bigproduct] = optionsdata.TN_BigProductData1!.map(
                              (item) => item.value
                          )[0];
                      }
                  };
              },
              defaultValue: 'ALL',
              LabelStyle: { color: 'blue' }
          },
          {
              label: 'common.commonpages.bigproduct',
              component: 'Select',
              // field: 'BigProduct',
              field: this.fieldobj.bigproduct,
              componentProps: (model) => {
                  return {
                      options: optionsdata.TN_BigProductData1,
                      placeholder: 'big product',
                      maxTagCount: 8,
                
                      onSelect: async (e: any) => {
                          if (e !== 'ALL') {
                              optionsdata.TN_ProductData1 = optionsdata.TN_ProductData!.filter(
                                  (item) => item?.value.indexOf(e) != -1
                              );
                          } else {
                              optionsdata.TN_ProductData1 = optionsdata.TN_ProductData;
                          }
                          model.formModel[this.fieldobj.optionproduct] = [];
                          model.formModel[this.fieldobj.selectmenu] = '';
                      }
                  };
              },
              defaultValue: 'ALL',
              LabelStyle: { color: 'blue' }
          },
          {
              label: 'common.commonpages.Product',
              // field: 'OptionProduct',
              field: this.fieldobj.optionproduct,
              component: 'Select',
              componentProps: (model) => {
                  return {
                      mode: 'multiple',
                      // defaultOpen:true,
                      listHeight: 300,
                      options: optionsdata.TN_ProductData1,
                      placeholder: 'product',
                      maxTagCount: 8,
                      dropdownMatchSelectWidth: 150,
                    
                      onSelect: async (e, option) => {
                          
                          model.formModel[this.fieldobj.product] = Array.from(
                              new Set([...model.formModel[this.fieldobj.product], e])
                          );

                          optionsdata['tempoption'] = model.formModel[this.fieldobj.product].map((item) => {
                              return {
                                  label: item,
                                  value: item
                              };
                          });
                          model.formModel[this.fieldobj.selectmenu] = '';
                      },
                      // 取消選中，后面的也應該取消選中
                      onDeselect: (value, option) => {
                          
                          model.formModel[this.fieldobj.product] = model.formModel[this.fieldobj.product].filter(
                              (item) => item != value
                          );
                         
                          optionsdata['tempoption'] = model.formModel[this.fieldobj.product].map((item) => {
                              return {
                                  label: item,
                                  value: item
                              };
                          });
                          model.formModel[this.fieldobj.selectmenu] = '';
                      }
                  };
              },
              defaultValue: [],
              LabelStyle: { color: 'blue' }
          },
          {
              label: '',
              // field: '#optionmenu',
              field: this.fieldobj.selectmenu,
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
                              
                              temp = optionsdata.TN_ProductData1!.map((item) => item.value);
                              
                              model.formModel[this.fieldobj.optionproduct] = temp;
                              model.formModel[this.fieldobj.product] = [
                                  ...new Set([...model.formModel[this.fieldobj.product], ...temp])
                              ];
                              optionsdata['tempoption'] = model.formModel[this.fieldobj.product].map((item) => {
                                  return {
                                      label: item,
                                      value: item
                                  };
                              });
                          } else if (e.target.value == 'cancelselect') {
                              // 全消全選
                              model.formModel[this.fieldobj.product] = model.formModel[this.fieldobj.product].filter(
                                  (item) => !model.formModel[this.fieldobj.optionproduct].includes(item)
                              );
                              model.formModel[this.fieldobj.optionproduct] = [];
                          }
                      }
                  };
              },
              defaultValue: ''
          },
          {
              label: '',
              // field: 'Product',
              field: this.fieldobj.product,
              component: 'Select',
              componentProps: (model) => {
                  return {
                      mode: 'multiple',
                      placeholder: 'product',
                      options: optionsdata['tempoption'],
                      maxTagCount: 8,
                      dropdownMatchSelectWidth: 150,
                     
                      onDeselect: (value, option) => {
                        
                            model.formModel[this.fieldobj.optionproduct] = model.formModel[
                                this.fieldobj.optionproduct
                            ].filter((item) => item != value);
                      
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

// 獲取LCM Owner
export class Owner {
  _obj: FormSchema[];
  optionsdata: TNType;
  FactoryType: string;
  constructor(optionsdata: TNType, FactoryType: string, Field: string = 'owner_code') {
      this.optionsdata = optionsdata;
      this.FactoryType = FactoryType;
      optionsdata['FactoryType'] = this.FactoryType;
      optionsdata['MarterialOption'] = [];
      this.init(optionsdata, this.FactoryType);
      this._obj = [
          {
              label: 'common.commonpages.LCMOwner',
              component: 'Select',
              field: Field,
              componentProps: (model) => {
                  return {
                      mode: 'multiple',
                      options: optionsdata.TN_LCMOwner,
                      dropdownMatchSelectWidth: 150,
                      onSelect: (e: any, option) => {
                          if (e != 'ALL') {
                              model.formModel[Field] = model.formModel[Field].filter((item) => item != 'ALL');
                          } else {
                              model.formModel[Field] = ['ALL'];
                          }
                      }
                  };
              }
          }
      ];
  }
  // api請求數據
  async init(optionsdata: TNType, FactoryType: string) {
      await mountedLCMOwner(optionsdata, FactoryType);
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

//獲取 Operation
export class Operation {
  _obj: FormSchema[];
  optionsdata: TNType;
  FactoryType: string;
  constructor(optionsdata: TNType, FactoryType: string, field: string = 'workGroup') {
      this.optionsdata = optionsdata;
      this.FactoryType = FactoryType;
      optionsdata['FactoryType'] = this.FactoryType;
      this.init(optionsdata, this.FactoryType);
      this._obj = [
          {
              label: 'common.commonpages.Operation',
              component: 'Select',
              field: field,
              componentProps: (model) => {
                  return {
                      mode: 'multiple',
                      options: optionsdata.TN_OperationLabel,
                      dropdownMatchSelectWidth: 150,
                      onSelect: (e: any, option) => {
                          if (e != 'ALL') {
                              model.formModel[field] = model.formModel[field].filter((item) => item != 'ALL');
                          } else {
                              model.formModel[field] = ['ALL'];
                          }
                      }
                  };
              }
          }
      ];
  }
  // api請求數據
  async init(optionsdata: TNType, FactoryType: string) {
      await mountedOper(optionsdata, FactoryType);
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

// 獲取LCD Owner
export class LCD_Owner {
  _obj: FormSchema[];
  optionsdata: TNType;
  FactoryType: string;
  constructor(optionsdata: TNType, FactoryType: string, Field: string = 'LcdOwner') {
      this.optionsdata = optionsdata;
      this.FactoryType = FactoryType;
      optionsdata['FactoryType'] = this.FactoryType;
      optionsdata['MarterialOption'] = [];
      this.init(optionsdata, this.FactoryType);
      this._obj = [
          {
              label: 'common.commonpages.LCDOwner',
              component: 'Select',
              field: Field,
              componentProps: (model) => {
                  return {
                      mode: 'multiple',
                      options: optionsdata.TN_LcdOwner,
                      dropdownMatchSelectWidth: 150,
                      onSelect: (e: any, option) => {
                          if (e != 'ALL') {
                              model.formModel[Field] = model.formModel[Field].filter((item) => item != 'ALL');
                          } else {
                              model.formModel[Field] = ['ALL'];
                          }
                      }
                  };
              },
              defaultValue: ['ALL']
          }
      ];
  }
  // api請求數據
  async init(optionsdata: TNType, FactoryType: string) {
      await mountedLCDOwner(optionsdata, FactoryType);
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

//獲取 LcdGrade
export class LCD_Grade {
  _obj: FormSchema[];
  optionsdata: TNType;
  FactoryType: string;
  constructor(optionsdata: TNType, FactoryType: string, Field: string = 'LcdGrade') {
      this.optionsdata = optionsdata;
      this.FactoryType = FactoryType;
      optionsdata['FactoryType'] = this.FactoryType;
      optionsdata['MarterialOption'] = [];
      this.init(optionsdata, this.FactoryType);
      this._obj = [
          {
              label: 'common.commonpages.LCDGrade',
              component: 'Select',
              field: Field,
              componentProps: (model) => {
                  return {
                      mode: 'multiple',
                      options: optionsdata.TN_LcdGrade,
                      dropdownMatchSelectWidth: 150,
                      onSelect: (e: any, option) => {
                          if (e != 'ALL') {
                              model.formModel[Field] = model.formModel[Field].filter((item) => item != 'ALL');
                          } else {
                              model.formModel[Field] = ['ALL'];
                          }
                      }
                  };
              }
          }
      ];
  }
  // api請求數據
  async init(optionsdata: TNType, FactoryType: string) {
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

//獲取 Down Grade
export class DownGrade {
  _obj: FormSchema[];
  optionsdata: TNType;
  FactoryType: string;
  constructor(optionsdata: TNType, FactoryType: string, field: string = 'workGroup') {
      this.optionsdata = optionsdata;
      this.FactoryType = FactoryType;
      optionsdata['FactoryType'] = this.FactoryType;
      this.init(optionsdata, this.FactoryType);
      this._obj = [
          {
              label: 'common.commonpages.DownGrade',
              component: 'Select',
              field: field,
              componentProps: (model) => {
                  return {
                      mode: undefined,
                      options: optionsdata.TN_OperationByDownGradeLabel,
                      dropdownMatchSelectWidth: 150,
                      onSelect: (e: any, option) => {
                          if (e != 'ALL') {
                              model.formModel[field] = model.formModel[field].filter((item) => item != 'ALL');
                          } else {
                              model.formModel[field] = ['ALL'];
                          }
                      }
                  };
              },
              defaultValue: ['1600']
          }
      ];
  }
  // api請求數據
  async init(optionsdata: TNType, FactoryType: string) {
      await mountedDownGrade(optionsdata, FactoryType);
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

//獲取 Down Grade
export class DownGradeL2 {
  _obj: FormSchema[];
  optionsdata: TNType;
  FactoryType: string;
  constructor(optionsdata: TNType, FactoryType: string, field: string = 'workGroup') {
      this.optionsdata = optionsdata;
      this.FactoryType = FactoryType;
      optionsdata['FactoryType'] = this.FactoryType;
      this.init(optionsdata, this.FactoryType);
      this._obj = [
          {
              label: 'common.commonpages.DownGrade',
              component: 'Select',
              field: field,
              componentProps: (model) => {
                  return {
                      mode: undefined,
                      options: optionsdata.TN_OperationByDownGradeL2Label,
                      dropdownMatchSelectWidth: 150,
                      
                  };
              },
              defaultValue: '1600'
          }
      ];
  }
  // api請求數據
  async init(optionsdata: TNType, FactoryType: string) {
      await mountedDownGradeL2(optionsdata, FactoryType);
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

//獲取 Hold Code
export class HoldCode {
  _obj: FormSchema[];
  optionsdata: TNType;
  FactoryType: string;
  constructor(optionsdata: TNType, FactoryType: string, field: string = 'HoldCode') {
      this.optionsdata = optionsdata;
      this.FactoryType = FactoryType;
      optionsdata['FactoryType'] = this.FactoryType;
      this.init(optionsdata, this.FactoryType);
      this._obj = [
          {
              label: 'common.commonpages.HoldCode',
              component: 'Select',
              field: field,
              componentProps: (model) => {
                  return {
                      mode: 'multiple',
                      options: optionsdata.TN_HoldCode,
                      dropdownMatchSelectWidth: 150,
                      onSelect: (e: any, option) => {
                          if (e != 'ALL') {
                              model.formModel[field] = model.formModel[field].filter((item) => item != 'ALL');
                          } else {
                              model.formModel[field] = ['ALL'];
                          }
                      }
                  };
              },
              defaultValue: ['ALL']
          }
      ];
  }
  // api請求數據
  async init(optionsdata: TNType, FactoryType: string) {
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

//獲取 Error Code
export class ErrorCode {
  _obj: FormSchema[];
  optionsdata: TNType;
  FactoryType: string;
  constructor(optionsdata: TNType, FactoryType: string, field: string = 'ErrorCode') {
      this.optionsdata = optionsdata;
      this.FactoryType = FactoryType;
      optionsdata['FactoryType'] = this.FactoryType;
      this.init(optionsdata, this.FactoryType);
      this._obj = [
          {
              label: 'common.commonpages.ErrorCode',
              component: 'Select',
              field: field,
              componentProps: (model) => {
                  return {
                      mode: 'multiple',
                      options: optionsdata.TN_ErrorCode,
                      dropdownMatchSelectWidth: 150,
                      onSelect: (e: any, option) => {
                          if (e != 'ALL') {
                              model.formModel[field] = model.formModel[field].filter((item) => item != 'ALL');
                          } else {
                              model.formModel[field] = ['ALL'];
                          }
                      }
                  };
              },
              defaultValue: ['ALL']
          }
      ];
  }
  // api請求數據
  async init(optionsdata: TNType, FactoryType: string) {
      await mountedErrorCode(optionsdata, FactoryType);
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

//獲取 PanelSource
export class PanelSource {
  _obj: FormSchema[];
  optionsdata: TNType;
  FactoryType: string;
  constructor(optionsdata: TNType, FactoryType: string, Field: string = 'PanelSource') {
      this.optionsdata = optionsdata;
      this.FactoryType = FactoryType;
      optionsdata['FactoryType'] = this.FactoryType;
      optionsdata['MarterialOption'] = [];
      this.init(optionsdata, this.FactoryType);
      this._obj = [
          {
              label: 'common.commonpages.PanelSource',
              component: 'Select',
              field: Field,
              componentProps: (model) => {
                  return {
                      mode: 'multiple',
                      options: optionsdata.TN_PanelSource,
                      dropdownMatchSelectWidth: 150,
                      onSelect: (e: any, option) => {
                          if (e != 'ALL') {
                              model.formModel[Field] = model.formModel[Field].filter((item) => item != 'ALL');
                          } else {
                              model.formModel[Field] = ['ALL'];
                          }
                      }
                  };
              },
              defaultValue: ['ALL']
          }
      ];
  }
  // api請求數據
  async init(optionsdata: TNType, FactoryType: string) {
      await mountedPanelSource(optionsdata, FactoryType);
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

//獲取 Defect Code
export class DefectCode {
  _obj: FormSchema[];
  optionsdata: TNType;
  FactoryType: string;
  constructor(optionsdata: TNType, FactoryType: string, Field: string = 'DefectCode') {
      this.optionsdata = optionsdata;
      this.FactoryType = FactoryType;
      optionsdata['FactoryType'] = this.FactoryType;
      optionsdata['MarterialOption'] = [];
      this.init(optionsdata, this.FactoryType);
      this._obj = [
          {
              label: 'common.commonpages.DefectCode',
              component: 'Select',
              field: Field,
              componentProps: (model) => {
                  return {
                      mode: 'multiple',
                      options: optionsdata.TN_DefectCode,
                      dropdownMatchSelectWidth: 150,
                      onSelect: (e: any, option) => {
                          if (e != 'ALL') {
                              model.formModel[Field] = model.formModel[Field].filter((item) => item != 'ALL');
                          } else {
                              model.formModel[Field] = ['ALL'];
                          }
                      }
                  };
              }
          }
      ];
  }
  // api請求數據
  async init(optionsdata: TNType, FactoryType: string) {
      await mountedDefectCode(optionsdata, FactoryType);
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

//獲取 DateMonth
export class DateMonth {
    _obj: FormSchema[];
    optionsdata: TNType;
    FactoryType: string;
    DateRange: string;
    constructor(optionsdata: TNType, FactoryType: string, DateRange: string) {
        debugger
        this.optionsdata = optionsdata;
        this.FactoryType = FactoryType;
        this.DateRange = DateRange;
        optionsdata['FactoryType'] = this.FactoryType;
        this.init(optionsdata, this.FactoryType, this.DateRange);
        this._obj = [
            {
                label: 'common.commonpages.DateMonth',
                component: 'Select',
                field: 'DateMonth',
                componentProps: (model) => {
                    return {
                        options: optionsdata.TN_DateMonth,
                        dropdownMatchSelectWidth: 150,
                        onSelect: (e: any, option) => {
                            if (e != 'ALL') {
                                model.formModel['DateMonth'] = model.formModel['DateMonth'].filter((item) => item != 'ALL');
                            } else {
                                model.formModel['DateMonth'] = ['ALL'];
                            }
                        }
                    };
                },
                defaultValue:String(dayjs().year()) + dayjs().format("MM")
            }
        ];
    }
    // api請求數據
    async init(optionsdata: TNType, FactoryType: string, DateRange: string) {
        await mountedDateMonth(optionsdata, FactoryType, DateRange);
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

