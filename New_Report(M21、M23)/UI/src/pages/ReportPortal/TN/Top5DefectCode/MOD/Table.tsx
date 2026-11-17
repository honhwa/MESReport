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
                            UseParamsAdd.AddFirstTableParams('Top5DefectCode' + UseParamsAdd.Fab, {
                                ...record,
                                ...column['__originColumn__']
                            });
                            router.push({ name: 'Top5DefectCodeDetail_' + UseParamsAdd.Fab });
                        };
                    } else {
                        rs.colSpan = 2;
                        rs.rowSpan = 1;
                        if (record['DBCLICK']) {
                            rs.onDblclick = () => {
                                UseParamsAdd.AddFirstTableParams('Top5DefectCode' + UseParamsAdd.Fab, {
                                    ...record,
                                    ...column['__originColumn__']
                                });
                                router.push({ name: 'Top5DefectCodeDetail_' + UseParamsAdd.Fab });
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
                            UseParamsAdd.AddFirstTableParams('Top5DefectCode' + UseParamsAdd.Fab, {
                                ...record,
                                ...column['__originColumn__']
                            });
                            router.push({ name: 'Top5DefectCodeDetail_' + UseParamsAdd.Fab });
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
    'OPER',
    'TRANS_DATE',
    'OPERATOR',
    'EQUIP_NBR',
    'WO_NBR',
    'LCM_OWNER',
    'ROWNUM'
];
const detailColumnsLabel = [
    'Panel ID',
    'Operation',
    'Trans Date',
    'Operater',
    'EQP ID',
    'WO Nbr',
    'LCM Owner'
];
export const detailColumns = detailColumnsProp.map((prop, idx) => {
    return {
        title: detailColumnsLabel[idx],
        align: 'center',
        dataIndex: prop,
        className: 'pro_auto-center'
    };
});

export const getColumns = (Prop: string[], tableTitle: any) => {
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
                                UseParamsAdd.AddFirstTableParams('Top5DefectCode' + UseParamsAdd.Fab, {
                                    ...record,
                                    ...column['__originColumn__']
                                });
                                router.push({ name: 'Top5DefectCodeDetail_' + UseParamsAdd.Fab });
                            };
                        } else {
                            rs.colSpan = 2;
                            rs.rowSpan = 1;
                            if (record['DBCLICK']) {
                                rs.onDblclick = () => {
                                    UseParamsAdd.AddFirstTableParams('Top5DefectCode' + UseParamsAdd.Fab, {
                                        ...record,
                                        ...column['__originColumn__']
                                    });
                                    router.push({ name: 'Top5DefectCodeDetail_' + UseParamsAdd.Fab });
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
                                UseParamsAdd.AddFirstTableParams('Top5DefectCode' + UseParamsAdd.Fab, {
                                    ...record,
                                    ...column['__originColumn__']
                                });
                                router.push({ name: 'Top5DefectCodeDetail_' + UseParamsAdd.Fab });
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
