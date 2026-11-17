import router from '@/router';
import { ReportCommon } from '@/store/module/ReportCommon';
const UseParamsAdd = ReportCommon();

export const aheadColumns = [
    {
        title: 'RANK',
        align: 'center',
        dataIndex: 'RANK',
        className: 'pro_auto-center',
        rowSpan: 3,
        customCell: (record) => {
            let text = record['RANK'];
            let obj = {
                text,
                style: {}
            };
            obj['rowSpan'] = text && text != '-' ? record?.['RANK-span']?.['rowspan'] || 0 : '';
            return obj;
        }
    }
];
export const rearColumns = [
    {
        title: 'Total',
        align: 'center' as any,
        dataIndex: 'TOTAL',
        colSpan: 2,
        children: [
            {
                title: 'Total',
                align: 'center',
                dataIndex: 'TOTAL',
                colSpan: 0,
                rowSpan: 0,
                customCell: (record: Object, rowIndex: number, column: Object) => {
                    let rs: any = {};
                    if (!record['ISHEAD']) {
                        rs.colSpan = 1;
                        rs.rowSpan = 1;
                        rs.style = {
                            cursor: 'pointer'
                        };
                        rs.onDblclick = () => {
                            UseParamsAdd.AddFirstTableParams('P3YieldReport' + UseParamsAdd.Fab, {
                                ...record,
                                ...column['__originColumn__']
                            });
                            router.push({ name: 'P3YieldReportDetail_' + UseParamsAdd.Fab });
                        };
                    } else {
                        rs.colSpan = 2;
                        rs.rowSpan = 1;
                        if (record['DBCLICK']) {
                            rs.onDblclick = () => {
                                UseParamsAdd.AddFirstTableParams('P3YieldReport' + UseParamsAdd.Fab, {
                                    ...record,
                                    ...column['__originColumn__']
                                });
                                router.push({ name: 'P3YieldReportDetail_' + UseParamsAdd.Fab });
                            };
                            rs.style = {
                                cursor: 'pointer'
                            };
                        }
                    }
                    return rs;
                }
            },
            {
                title: 'Total',
                align: 'center',
                dataIndex: 'TOTAL-DFCT_CODE',
                colSpan: 0,
                rowSpan: 0,
                customCell: (record: Object, rowIndex: number, column: Object) => {
                    let rs: any = {};
                    if (!record['ISHEAD']) {
                        rs.colSpan = 1;
                        rs.rowSpan = 1;
                        rs.style = {
                            cursor: 'pointer'
                        };
                        rs.onDblclick = () => {
                            UseParamsAdd.AddFirstTableParams('P3YieldReport' + UseParamsAdd.Fab, {
                                ...record,
                                ...column['__originColumn__']
                            });
                            router.push({ name: 'P3YieldReportDetail_' + UseParamsAdd.Fab });
                        };
                        if (record[column['dataIndex']]) {
                            rs.style['background-color'] = '#99cae3';
                        }
                    } else {
                        rs.colSpan = 0;
                        rs.rowSpan = 0;
                    }
                    return rs;
                }
            }
        ]
    }
];

const detailColumnsProp = [
    'PNL_ID',
    'PROD_NBR',
    'OPER',
    'TRANS_DATE',
    'OPERATOR',
    'EQPID',
    'WO_NBR',
    'LCM_OWNER',
    'ERRC_NBR',
    'ERRC_DESCR'
];
const detailColumnsLabel = [
    'Panel ID',
    'Prod Nbr',
    'Operation',
    'Trans Date',
    'Operater',
    'EQPID',
    'WO_NBR',
    'LCM_OWNER',
    'Defect Code',
    'Defect Desc'
];
export const detailColumns = detailColumnsProp.map((prop, idx) => {
    return {
        title: detailColumnsLabel[idx],
        align: 'center',
        dataIndex: prop,
        className: 'pro_auto-center'
    };
});

const tableTitle = {
    '1400': 'Assy(B/L自組)',
    '1402': 'P3_自動線',
    '1405': 'Hot bar',
    '1406': 'Hot bar Offline',
    '1415': 'Assy(B/L外購)',
    '1420': 'AAFC',
    '1500': 'AGING',
    '1510': 'TPI',
    '1600': 'C KEN',
    '1630': 'C KEN2',
    '1700': 'D KEN'
};

export const getColumns = (Prop: string[]) => {
    return Prop.map((prop) => {
        return {
            title: tableTitle[prop],
            align: 'center' as any,
            dataIndex: prop,
            colSpan: 2,
            children: [
                {
                    title: tableTitle[prop],
                    align: 'center',
                    dataIndex: `${prop}`,
                    colSpan: 0,
                    rowSpan: 0,
                    customCell: (record: Object, rowIndex: number, column: Object) => {
                        let rs: any = {};
                        if (!record['ISHEAD']) {
                            rs.colSpan = 1;
                            rs.rowSpan = 1;
                            rs.style = {
                                cursor: 'pointer'
                            };
                            rs.onDblclick = () => {
                                UseParamsAdd.AddFirstTableParams('P3YieldReport' + UseParamsAdd.Fab, {
                                    ...record,
                                    ...column['__originColumn__']
                                });
                                router.push({ name: 'P3YieldReportDetail_' + UseParamsAdd.Fab });
                            };
                        } else {
                            rs.colSpan = 2;
                            rs.rowSpan = 1;
                            if (record['DBCLICK']) {
                                rs.onDblclick = () => {
                                    UseParamsAdd.AddFirstTableParams('P3YieldReport' + UseParamsAdd.Fab, {
                                        ...record,
                                        ...column['__originColumn__']
                                    });
                                    router.push({ name: 'P3YieldReportDetail_' + UseParamsAdd.Fab });
                                };
                                rs.style = {
                                    cursor: 'pointer'
                                };
                            }
                        }
                        return rs;
                    }
                },
                {
                    title: tableTitle[prop],
                    align: 'center',
                    dataIndex: prop + '-DFCT_CODE',
                    colSpan: 0,
                    rowSpan: 0,
                    customCell: (record: Object, rowIndex: number, column: Object) => {
                        let rs: any = {};
                        if (!record['ISHEAD']) {
                            rs.colSpan = 1;
                            rs.rowSpan = 1;
                            rs.style = {
                                cursor: 'pointer'
                            };
                            rs.onDblclick = () => {
                                UseParamsAdd.AddFirstTableParams('P3YieldReport' + UseParamsAdd.Fab, {
                                    ...record,
                                    ...column['__originColumn__']
                                });
                                router.push({ name: 'P3YieldReportDetail_' + UseParamsAdd.Fab });
                            };
                            if (record[column['dataIndex']]) {
                                rs.style['background-color'] = '#99cae3';
                            }
                        } else {
                            rs.colSpan = 0;
                            rs.rowSpan = 0;
                        }
                        return rs;
                    }
                }
            ]
        };
    });
};
