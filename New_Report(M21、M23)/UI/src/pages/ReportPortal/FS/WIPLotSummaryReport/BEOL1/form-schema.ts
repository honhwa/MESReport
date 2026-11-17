import type { FormSchema } from '@/components/core/schema-form/';
import { reactive } from 'vue';
import { Size, Date } from '@/pages/ReportPortal/Static/common';
import { formatToDate } from '@/utils/dateUtil';
export let optionsdata = reactive({
  groupby: [
    { label: 'Operation', value: 'PROC_ID' },
    { label: 'PFCD', value: 'PFCD' },
    { label: 'Group ID', value: 'GROUP_ID' },
    { label: 'Owner', value: 'OWNER_ID' },
    { label: 'Status', value: 'SHEET_STAT' },
    { label: 'Grade', value: 'GRADE' },
    { label: 'Engineer', value: 'ENGINEER' },
    { label: 'Abnormal Flag2', value: 'ABNORMAL_FLG' },
    { label: 'Location', value: 'POSITION' },
    { label: 'Entity', value: 'ENTITY' },
    { label: 'Cassette', value: 'CASET_ID' },
    { label: 'Comment', value: 'SHT_OPE_MSG' },
    { label: 'Work_Order_ID', value: 'WORK_ORDER' },
    { label: 'PFCD_2CUT', value: 'PFCD_2CUT' },
    { label: 'PFCD_ASMB', value: 'PFCD_ASMB' },
    { label: 'Work_Order_Comment', value: 'WORDER_DSC' },
    { label: 'Destination', value: 'DESTINATION' },
  ],
});
export const schemas = (FactoryType: string) => {
  return [
    {
      label: 'History',
      field: 'isHistory',
      component: 'Switch',
      componentProps: () => {
        return {
          checkedChildren: '開',
          unCheckedChildren: '關',
        };
      },
      defaultValue: false,
    },
    {
      label: 'common.commonpages.Date',
      field: 'udate',
      component: 'DatePicker',
      componentProps: {
        valueFormat: 'YYYY-MM-DD',
      },
      defaultValue: formatToDate(),
      required: true,
    },
    {
      label: 'common.commonpages.Shift',
      component: 'RadioGroup',
      field: 'Shifts',
      componentProps: {
        options: [
          {
            label: 'Day',
            value: " and shift = '1' ",
          },
          {
            label: 'Night',
            value: " and shift = '2' ",
          },
        ],
      },
      defaultValue: " and shift = '2' ",
      dynamicDisabled: ({ formModel }) => !formModel.isHistory,
    },
    {
      label: 'Product Type',
      component: 'Select',
      field: 'ProdTypes',
      componentProps: (model) => {
        return {
          options: [
            { label: 'All', value: 'All' },
            { label: 'Prod+E→P+Eng', value: 'PE' },
            { label: 'Prod+E→P', value: 'PX' },
            { label: 'Product', value: 'Prod' },
            { label: 'E→P', value: 'EXP' },
            { label: 'Engineer', value: 'Eng' },
            { label: 'QLot', value: 'Qlot' },
            { label: 'Dummy', value: 'Dumy' },
          ],
          placeholder: 'ProdTypes',
        };
      },
      defaultValue: 'PX',
    },
    {
      label: 'Detail Type',
      component: 'Select',
      field: 'DetailType',
      componentProps: (model) => {
        return {
          options: [
            { label: 'By PPbox ID', value: 'CST' },
            { label: 'By Panel', value: 'PNL' },
            { label: 'By Pallet ID', value: 'PLT' },
            { label: 'By Polarizer', value: 'POL' },
          ],
        };
      },
      defaultValue: 'PLT',
    },
    {
      label: 'Grade',
      component: 'Select',
      field: 'Grade',
      componentProps: (model) => {
        return {
          mode: 'multiple',
          options: optionsdata['Grade'],
          placeholder: 'Grade',
        };
      },
      defaultValue: ["All"],
    },
    {
      label: 'Operation',
      component: 'Select',
      field: 'Operation',
      componentProps: (model) => {
        return {
          mode: 'multiple',
          options: optionsdata['Operation'],
          placeholder: 'Operation',
        };
      },
      defaultValue: [],
    },
    {
      label: 'Group By',
      component: 'Select',
      field: 'optiongroup',
      componentProps: (model) => {
        return {
          mode: 'multiple',
          options: optionsdata.groupby,
          onSelect: async (e, option) => {
            model.formModel['all_group'] = Array.from(
              new Set([...model.formModel['all_group'], e]),
            );
            optionsdata['tempoption'] = model.formModel['all_group'].map((item) => {
              let findobj = optionsdata.groupby.find((fitem) => {
                return fitem.value == item;
              });
              if (findobj) {
                return {
                  label: findobj.label,
                  value: findobj.value,
                };
              }
            });
            model.formModel['#optionmenu'] = '';
          },
          onDeselect: (value, option) => {
            model.formModel['all_group'] = model.formModel['all_group'].filter(
              (item) => item != value,
            );
            optionsdata['tempoption'] = model.formModel['all_group'].map((item) => {
              let findobj = optionsdata.groupby.find((fitem) => {
                return fitem.value == item;
              });
              if (findobj) {
                return {
                  label: findobj.label,
                  value: findobj.value,
                };
              }
            });
            model.formModel['#optionmenu'] = '';
          },
        };
      },
      defaultValue: [],
    },
    {
      label: '',
      field: '#optionmenu',
      component: 'RadioGroup',
      componentProps: (model) => {
        return {
          options: [
            {
              label: '全選',
              value: 'selectALL',
            },
            {
              label: '取消全選',
              value: 'cancelselect',
            },
          ],
          onChange: (e) => {
            // 全選，全部添加
            if (e.target.value == 'selectALL') {
              model.formModel['all_group'] = optionsdata.groupby.map((item) => {
                return item.value;
              });
              optionsdata['tempoption'] = optionsdata.groupby;
            } else if (e.target.value == 'cancelselect') {
              // 全消全選
              model.formModel['all_group'] = [];
              optionsdata['tempoption'] = [];
            }
          },
        };
      },
      defaultValue: '',
    },
    {
      label: '',
      field: 'all_group',
      component: 'Select',
      componentProps: (model) => {
        return {
          mode: 'multiple',
          options: optionsdata['tempoption'],
          maxTagCount: 8,
          dropdownMatchSelectWidth: 150,
          // 取消選中，前面的也應該取消選中
          onDeselect: (value, option) => {
            model.formModel['optiongroup'] = model.formModel['optiongroup'].filter((item) => {
              return item != value;
            });
            optionsdata['tempoption'] = optionsdata['tempoption'].filter((item) => {
              return item.value != value;
            });
            model.formModel['#optionmenu'] = '';
          },
        };
      },
      defaultValue: [],
    },
    {
      label: 'Constrain',
      component: 'RadioGroup',
      field: 'Constrain',
      componentProps: (model) => {
        return {
          options: [
            {
              label: '空',
              value: 'null',
            },
            {
              label: 'PPbox qty',
              value: '9',
            },
            {
              label: 'Pallet qty',
              value: '10',
            },
          ],
          onChange: (e: Event) => {
            let value = (e.target as HTMLInputElement).value;
            if(value == '9'){
              model.formModel['Const3'] = '';
            }else if(value == '10'){
              model.formModel['Const2'] = '';
            }else{
              model.formModel['Const2'] = '';
              model.formModel['Const3'] = '';
            }
          },
        };
      },
      defaultValue: '',
    },
    {
      label: '',
      field: 'Const2',
      component: 'Input',
      componentProps: () => {
        return {};
      },
      componentSlots: () => {
        return {
          prefix: () => 'PPbox qty <',
          suffix: () => 'pcs',
        };
      },
      defaultValue: '',
      dynamicDisabled: ({ formModel }) => !(formModel.Constrain == '9'),
    },
    {
      label: '',
      field: 'Const3',
      component: 'Input',
      componentProps: () => {
        return {};
      },
      componentSlots: () => {
        return {
          prefix: () => 'Pallet qty <',
          suffix: () => 'pcs',
        };
      },
      defaultValue: '',
      dynamicDisabled: ({ formModel }) => !(formModel.Constrain == '10'),
    },

    {
      field: '#PFCD',
      component: 'Divider',
      label: 'PFCD',
    },
    {
      label: 'Prod',
      component: 'Select',
      field: 'Prod',
      componentProps: (model) => {
        return {
          options: optionsdata['Prod'],
          onSelect: async (e, option) => {
            optionsdata['optionPFCD'] = optionsdata['PFCD']
              .filter((item) => {
                return item.substring(2, 6) === e;
              })
              .map((item) => {
                return {
                  label: item,
                  value: e + item,
                };
              });
            model.formModel['#PFCDmenu'] = '';
          },
        };
      },
      defaultValue: '',
      LabelStyle: { color: 'blue' },
    },
    {
      label: 'PFCD',
      component: 'Select',
      field: 'PFCD',
      componentProps: (model) => {
        return {
          mode: 'multiple',
          options: optionsdata['optionPFCD'],
          maxTagCount: 8,
          listHeight: 300,
          dropdownMatchSelectWidth: 150,
          onSelect: async (e, option) => {
            model.formModel['Group_Conditions'] = Array.from(
              new Set([...model.formModel['Group_Conditions'], e]),
            );
            optionsdata['Group_ConditionsOption'] = model.formModel['Group_Conditions'].map(
              (item) => {
                return {
                  label: item,
                  value: item,
                };
              },
            );
            model.formModel['#PFCDmenu'] = '';
          },
          onDeselect: (value, option) => {
            model.formModel['Group_Conditions'] = model.formModel['Group_Conditions'].filter(
              (item) => item != value,
            );
            optionsdata['Group_ConditionsOption'] = model.formModel['Group_Conditions'].map(
              (item) => {
                return {
                  label: item,
                  value: item,
                };
              },
            );
            model.formModel['#PFCDmenu'] = '';
          },
        };
      },
      defaultValue: [],
      LabelStyle: { color: 'blue' },
    },

    {
      label: '',
      field: '#PFCDmenu',
      component: 'RadioGroup',
      componentProps: (model) => {
        return {
          options: [
            {
              label: '全選',
              value: 'selectALL',
            },
            {
              label: '取消全選',
              value: 'cancelselect',
            },
          ],
          onChange: (e) => {
            // 全選，全部添加
            if (e.target.value == 'selectALL') {
              model.formModel['PFCD'] = model.formModel['Group_Conditions'] = optionsdata[
                'optionPFCD'
              ].map((item) => {
                return item.value;
              });
              optionsdata['Group_ConditionsOption'] = model.formModel['Group_Conditions'].map(
                (item) => {
                  return {
                    label: item,
                    value: item,
                  };
                },
              );
            } else if (e.target.value == 'cancelselect') {
              // 全消全選
              model.formModel['PFCD'] = model.formModel['Group_Conditions'] = [];
              optionsdata['Group_ConditionsOption'] = [];
            }
          },
        };
      },
      defaultValue: '',
      LabelStyle: { color: 'blue' },
    },
    {
      label: 'Group Conditions',
      field: 'Group_Conditions',
      component: 'Select',
      componentProps: (model) => {
        return {
          mode: 'multiple',
          options: optionsdata['Group_ConditionsOption'],
          maxTagCount: 8,
          listHeight: 300,
          dropdownMatchSelectWidth: 150,
          // 取消選中，前面的也應該取消選中
          onDeselect: (value, option) => {
            model.formModel['PFCD'] = model.formModel['PFCD'].filter((item) => {
              return item != value;
            });
            optionsdata['Group_ConditionsOption'] = optionsdata['Group_ConditionsOption'].filter(
              (item) => {
                return item.value != value;
              },
            );
            model.formModel['#PFCDmenu'] = '';
          },
        };
      },
      defaultValue: [],
      LabelStyle: { color: 'blue' },
    },
  ];
};
