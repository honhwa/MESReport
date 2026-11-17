import type { FormSchema } from '@/components/core/schema-form/';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil';
import dayjs from 'dayjs';
import {
    ConmonGrade,
    Floor,
    FSType,
    Grade,
    LCMGrade,
    LCMProductSize,
    Owner,
    Shift,
    Size,
    Size2
} from '@/pages/ReportPortal/Static/common';
import { GetCommon } from '@/api/pages/common';
async function getOper(funcparams: String, FactoryType: String) {
    let ReturnData: any = [];
    let {
        Date: { Table: Data }
    } = await GetCommon({
        G_FAC: 'A',
        name: 'Date',
        apiName: funcparams,
        FactoryType: FactoryType
    });
    for (let i in Data) {
        if (FactoryType == 'BEOL2') {
            ReturnData.push({ label: Data[i]['PROC_ID'], value: Data[i]['PROC_ID'] });
        } else {
            //@ts-ignore
            ReturnData.push({
                label: `${Data[i]['WORK_CTR']}-${Data[i]['WORK_NAME']}`,
                value: Data[i]['WORK_CTR']
            });
        }
    }
    ReturnData.unshift({ label: 'ALL', value: 'ALL' });
    return ReturnData;
}
const mountedOper = async (optionsdata, FactoryType, optionsdataprop: string = 'OperationLabel') => {
    optionsdata[optionsdataprop] = await getOper('OQCQualityReport_Operation', FactoryType);
};
class Operation {
    _obj: FormSchema[];
    optionsdata: FSType;
    FactoryType: string;
    constructor(optionsdata: FSType, FactoryType: string, field: string = 'Operation') {
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
                        options: optionsdata.OperationLabel,
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
    async init(optionsdata: FSType, FactoryType: string) {
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

export const schemas = (FactoryType1: string) => {
    let FactoryType = "LCM1";
    let optiondata = reactive({});
    let optiondata2 = reactive({});
    return [
        {
            label: 'Date/Time',
            field: 'FromDate',
            component: 'DatePicker',
            componentProps: {
                valueFormat: 'YYYY-MM-DD'
            },
            defaultValue: formatToDate(),
            required: true
        },
        {
            label: '开始时间',
            field: 'FromTime',
            component: 'TimePicker',
            componentProps: {
                minuteStep: 30,
                format: 'HH:mm'
            },
            defaultValue: dayjs('08:00', 'HH:mm'),
            required: true
        },
        {
            label: 'To',
            field: 'ToDate',
            component: 'DatePicker',
            componentProps: {
                valueFormat: 'YYYY-MM-DD'
            },
            defaultValue: formatToDate(),
            required: true
        },
        {
            label: '结束时间',
            field: 'ToTime',
            component: 'TimePicker',
            componentProps: {
                minuteStep: 30,
                format: 'HH:mm'
            },
            defaultValue: dayjs('08:00', 'HH:mm'),
            required: true
        },
        ...new Shift(1)._obj,
        ...new Floor(1)._obj,
        {
            label: 'common.commonpages.LCMProductArea',
            component: 'RadioGroup',
            field: 'LCMProductArea',
            componentProps: {
                options: [
                    {
                        label: 'ALL',
                        value: 'ALL'
                    },
                    {
                        label: 'Moudle',
                        value: 'Moudle',
                        key: "Select Distinct part_nbr SOption From HMASHIS_IPMS@phars Where Fac_ID='A' and prod_type<>'SKD'"
                    },
                    {
                        label: 'SKD',
                        value: 'SKD',
                        key: "Select Distinct part_nbr SOption From HMASHIS_IPMS@phars Where Fac_ID='A' and prod_type='SKD'"
                    },
                    {
                        label: 'LED',
                        value: 'LED'
                    },
                    {
                        label: 'Down Grade ',
                        value: 'Down Grade '
                    },
                    {
                        label: '3D',
                        value: '3D'
                    }
                ]
            },
            defaultValue: 'ALL'
        },
        ...new LCMProductSize(optiondata, FactoryType,"prod_size").obj,
        {
            label: 'Interval ',
            component: 'RadioGroup',
            field: 'intercond',
            componentProps: {
                options: [
                    {
                        label: 'Day',
                        value: 'day'
                    },
                    {
                        label: 'Week',
                        value: 'week'
                    },
                    {
                        label: 'Month',
                        value: 'month'
                    }
                ]
            },
            defaultValue: 'day'
        },
        {
            label: 'Operation',
            component: 'RadioGroup',
            field: 'operation',
            componentProps: {
                options: [
                    {
                        label: '6500(OQC最終判退)',
                        value: "and work_ctr=6500 "
                    },
                    {
                        label: '1760(OQC初判不良)',
                        value: "and work_ctr=1760 "
                    },
                    {
                        label: '1850(特殊撕膜檢不良，非廠方責任)',
                        value: "and work_ctr=1850 "
                    },
                    
                    
                ]
            },
            defaultValue: 'and work_ctr=6500 '
        },
        {
            label: '機種類型',
            component: 'RadioGroup',
            field: 'por',
            componentProps: {
                options: [
                    {
                        label: 'ALL',
                        value: ''
                    },
                    {
                        label: 'TV MOD-UD(4K2K)',
                        value: "and substr(q.prod_nbr,1,1)='G' and substr(q.prod_nbr,6,1)='D'  "
                    },
                    {
                        label: 'TV MOD-FHD',
                        value: "and substr(q.prod_nbr,1,1)='G' and substr(q.prod_nbr,6,1)<>'D'  "
                    },
                    {
                        label: 'TV Open cell-UD(4K2K)',
                        value: "and substr(q.prod_nbr,1,1)='2' and substr(q.prod_nbr,6,1)='D'  "
                    },
                    {
                        label: 'TV Open cell FHD',
                        value: "and substr(q.prod_nbr,1,1)='2' and substr(q.prod_nbr,6,1)<>'D'  "
                    }
                ]
            },
            defaultValue: ''
        },
        {
            label: 'Reworked',
            component: 'Select',
            field: 'rework',
            componentProps: () => {
                return {
                    options: [
                        {
                            label: 'ALL',
                            value: ''
                        },
                        {
                            label: '1次',
                            value: '1'
                        },
                        {
                            label: '1次以上（含）',
                            value: 'over_1'
                        },
                        {
                            label: '2次',
                            value: '2'
                        },

                        {
                            label: '2次以上（含）',
                            value: 'over_2'
                        },
                        {
                            label: '3次',
                            value: '3'
                        },
                        {
                            label: 'No',
                            value: 'N'
                        }
                    ]
                };
            },
            defaultValue: 'ALL'
        },
        ...new Size(optiondata, FactoryType, {
            size: 'LCMprodsize',
            bigproduct: 'BigProduct',
            optionproduct: 'EntyGroup',
            selectmenu: '#optionmenu',
            product: 'vprop'
        }).obj,
        ...new Owner(optiondata, FactoryType).setLabel(0, 'Owner:').obj,

        {
            label: 'Judgement',
            component: 'Select',
            field: 'judgement',
            componentProps: {
                mode: 'multiple',
                options: [
                    {
                        label: 'ALL',
                        value: ''
                    },
                    {
                        label: 'OK',
                        value: "(rank<>'2')"
                    },
                    {
                        label: 'Down Grade',
                        value: "(q.grade_new > q.grade_old and rank1='NG')"
                    },
                    {
                        label: 'Up Grade',
                        value: "(q.grade_new < q.grade_old and rank1='NG')"
                    },
                    {
                        label: 'NG',
                        value: "(q.grade_new = q.grade_old and rank1='NG/RW')"
                    }
                ]
            },
            // defaultValue: ["1=1"],
            defaultValue: ['']
        },
        {
            label: 'Defect Group',
            component: 'Select',
            field: 'DefectGroup',
            componentProps: () => {
                return {
                    mode: 'multiple',
                    options: [
                        {
                            label: 'ALL',
                            value: 'ALL'
                        },
                        {
                            label: '電氣',
                            value: 'C'
                        },
                        {
                            label: '品位',
                            value: 'F'
                        },
                        {
                            label: '外觀',
                            value: 'D'
                        }
                    ]
                };
            },
            defaultValue: ['ALL']
        },
        ...new ConmonGrade(optiondata, FactoryType,"lcmgrade_grp").obj,
        {
            label: 'OQC Grade:',
            component: 'Select',
            field: 'oqcgrade_grp',
            componentProps: () => {
                return {
                    mode: 'multiple',
                    options: [
                        {
                            label: 'ALL',
                            value: 'ALL'
                        },
                        {
                            label: 'A',
                            value: 'A'
                        },
                        {
                            label: 'B',
                            value: 'B'
                        },
                        {
                            label: 'C',
                            value: 'C'
                        },
                        {
                            label: 'D',
                            value: 'D'
                        },
                        {
                            label: 'E',
                            value: 'E'
                        },
                        {
                            label: 'W',
                            value: 'W'
                        }
                    ]
                };
            },
            defaultValue: ['ALL']
        },
        ...new Operation(optiondata, FactoryType, 'workGroup').obj,
        ...new Grade(optiondata2, FactoryType,"in_lcd_Grade").obj
    ];
};
