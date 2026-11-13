import router from '@/router';
import { ReportCommon } from '@/store/module/ReportCommon';
const UseParamsAdd = ReportCommon();

export const aheadColumns = [
    {
        title: '',
        align: 'center',
        dataIndex: 'KPI',
        className: 'v-center',
        width: 80,
        customCell: (record: Object, rowIndex: number, column: Object) => {
            let rs: any = {};
            rs.style = {
                'background-color': '#bbc2c9'
            };
            if (record['ISHERD']) {
                rs.colSpan = 2;
                rs.rowSpan = 1;
                rs.style = {
                    'background-color': '#39f',
                    'font-weight': 'bold',
                    color: '#fff'
                };
            }
            if (record['ISYIELD']) {
                rs.colSpan = 1;
                rs.rowSpan = 1;
                rs.style = {
                    'background-color': '#39f',
                    'font-weight': 'bold',
                    color: '#fff'
                };
            }
            rs['rowSpan'] = record['KPI'] ? record?.['KPI-span']?.['rowspan'] || 0 : '';
            return rs;
        }
    },
    {
        title: 'Date',
        align: 'center',
        dataIndex: 'TOP',
        className: 'v-center',
        width: 140,
        customCell: (record: Object, rowIndex: number, column: Object) => {
            let rs: any = {};
            rs.style = {
                'background-color': '#bbc2c9'
            };
            if (record['ISHERD']) {
                rs.colSpan = 0;
                rs.rowSpan = 0;
            }
            if (record['ISYIELD']) {
                rs.colSpan = 1;
                rs.rowSpan = 1;
                rs.style = {
                    'background-color': '#39f'
                };
            }
            return rs;
        }
    }
];

export const getColumns = (Prop: string[]) => {
    return Prop.map((prop) => {
        return {
            title: prop,
            align: 'center' as any,
            dataIndex: prop,
            colSpan: 2,
            className: 'v-center',
            children: [
                {
                    title: prop,
                    align: 'center',
                    dataIndex: prop + '-DFCT_CODE',
                    width: 140,
                    className: 'v-center',
                    customCell: (record: Object, rowIndex: number, column: Object) => {
                        let rs: any = {};
                        if (!record['ISHERD'] && !record['ISYIELD']) {
                            rs.colSpan = 1;
                            rs.rowSpan = 1;
                            rs.onDblclick = () => {
                                UseParamsAdd.AddFirstTableParams('Top5DefectCodeNew' + UseParamsAdd.Fab, {
                                    ...record,
                                    ...column['__originColumn__']
                                });
                                router.push({ name: 'Top5DefectCodeNewDetail_' + UseParamsAdd.Fab });
                            };
                            rs.style = rs.style || {};
                            if (record[prop + '-LIGHT']) {
                                switch (record[prop + '-LIGHT']) {
                                    case 'Color_R':
                                        rs.style['color'] = 'Red';
                                        break;
                                    case 'Color_O':
                                        rs.style['color'] = 'DarkOrange';
                                        break;
                                }
                            }
                        } else {
                            rs.colSpan = 0;
                            rs.rowSpan = 0;
                        }
                        return rs;
                    }
                },
                {
                    title: prop,
                    align: 'center',
                    dataIndex: `${prop}`,
                    width: 80,
                    className: 'v-center',
                    customCell: (record: Object, rowIndex: number, column: Object) => {
                        let rs: any = {};
                        if (!record['ISHERD']) {
                            if (!record['ISYIELD']) {
                                rs.colSpan = 1;
                                rs.rowSpan = 1;
                                rs.style = {
                                    cursor: 'pointer'
                                };
                                if (record[prop + '-LIGHT']) {
                                    switch (record[prop + '-LIGHT']) {
                                        case 'Color_R':
                                            rs.style['color'] = 'Red';
                                            break;
                                        case 'Color_O':
                                            rs.style['color'] = 'DarkOrange';
                                            break;
                                    }
                                }
                                rs.onDblclick = () => {
                                    UseParamsAdd.AddFirstTableParams('Top5DefectCodeNew' + UseParamsAdd.Fab, {
                                        ...record,
                                        ...column['__originColumn__']
                                    });
                                    router.push({ name: 'Top5DefectCodeNewDetail_' + UseParamsAdd.Fab });
                                };
                            } else {
                                rs.colSpan = 2;
                                rs.rowSpan = 1;
                                rs.style = rs.style || {};
                                if (record[prop + '-LIGHT']) {
                                    switch (record[prop + '-LIGHT']) {
                                        case 'Light_R':
                                            rs.style['background-color'] = '#f25577';
                                            break;
                                        case 'Light_Y':
                                            rs.style['background-color'] = '#ecf540';
                                            break;
                                        case 'Light_G':
                                            rs.style['background-color'] = '#5ad168';
                                            break;
                                    }
                                }
                            }
                        } else {
                            rs.colSpan = 2;
                            rs.rowSpan = 1;
                            rs.style = {
                                'background-color': '#39f',
                                'font-weight': 'bold',
                                color: '#fff'
                            };
                        }
                        return rs;
                    }
                }
            ]
        };
    });
};

const detailColumnsProp = ['PROD_NBR', 'PASS_PNL_CNT', 'PNL_CNT', 'DEFT_RATE'];
const detailColumnsLabel = ['Product', '投入片數', '不良片數', '不良率'];
export const detailColumns = detailColumnsProp.map((prop, idx) => {
    return {
        title: detailColumnsLabel[idx],
        align: 'center',
        dataIndex: prop,
        className: 'pro_auto-center',
        customCell: (record: Object, rowIndex: number, column: Object) => {
            let rs: any = {};
            rs.style = {
                cursor: 'pointer'
            };
            rs.onDblclick = () => {
                UseParamsAdd.AddSecondTableParams('Top5DefectCodeNew' + UseParamsAdd.Fab, {
                    ...record,
                    ...column['__originColumn__']
                });
            };
            return rs;
        }
    };
});

const SecondColumnsProp = ['PROD_NBR', 'PNL_ID', 'WORK_CTR', 'TRANS_DATE', 'EQUIP_NBR', 'OPERATOR', 'WO_NBR'];
const SecondColumnsLabel = ['Product', 'Panel ID', 'Oper.', 'Trans Date', 'Entity', 'Operator', 'Work Order'];
export const SecondColumns = SecondColumnsProp.map((prop, idx) => {
    return {
        title: SecondColumnsLabel[idx],
        align: 'center',
        dataIndex: prop,
        className: 'pro_auto-center'
    };
});

const LightProp = ['DESC', 'FPTYIELD', 'PCBI', 'LAM', 'ACDP'];
const LightLabel = ['Target說明', 'FPT Yield', 'PCBI', 'LAM', 'ACDP'];
//const LightWidth = [30, 125, 125, 40];
export const LightColumns = LightProp.map((prop, idx) => {
    return {
        title: LightLabel[idx],
        align: 'center',
        dataIndex: prop,
        className: 'v-center',
        customCell: (record: Object, rowIndex: number, column: Object) => {
            let rs: any = {};
            rs.style = rs.style || {};
            if (rowIndex == 0) {
                rs.style['font-weight'] = 'bold';
                rs.style['color'] = '#fff';
                rs.style['background-color'] = '#5d7b9d';
            }
            if (column['dataIndex'] == 'DESC') {
                switch (record['DESC']) {
                    case '綠燈':
                        rs.style['background-color'] = '#5ad168';
                        break;
                    case '黃燈':
                        rs.style['background-color'] = '#ecf540';
                        break;
                    case '紅燈':
                        rs.style['background-color'] = '#f25577';
                        break;
                }
            }
            return rs;
        }
    };
});

const Top5Prop = ['TOP', 'PROD_NBR', 'ERRC_DESCR', 'QTY'];
const TOP5Label = ['TOP', 'Product', 'Defect', 'Qty'];
const TOP5Width = [30, 125, 125, 40];
export const TOP5Columns = Top5Prop.map((prop, idx) => {
    return {
        title: TOP5Label[idx],
        align: 'center',
        dataIndex: prop,
        width: TOP5Width[idx],
        className: 'v-center',
        customCell: (record: Object, rowIndex: number, column: Object) => {
            let rs: any = {};
            rs.style = rs.style || {};
            if (rowIndex == 0) {
                rs.style['font-weight'] = 'bold';
                rs.style['color'] = '#fff';
                rs.style['background-color'] = '#5d7b9d';
            }
            return rs;
        }
    };
});
