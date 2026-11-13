import { AddCommonPageParamsWithOut } from '@/store/module/commonpage';
import { ReportCommonWithOut } from '@/store/module/ReportCommon';
import router from '@/router';

const useCommonParams = AddCommonPageParamsWithOut();
const useReportParams = ReportCommonWithOut();
let _1600ColumnsLabel = ["1600 Ken 狀況"];
let _1600ColumnsProps = ["1600"];
let ColumnsLabel = [];
let ColumnsProps = [];
export const Top5OperationsDetailLabel = ['PROD_NBR', 'QTY'];

export const Top5OperationsDetailProps = ["PROD_NBR","D_QTY"]

export const Top5DetailColumns = GetTableColumn(Top5OperationsDetailLabel, Top5OperationsDetailProps)

export function ChangeColumns(operaction: string) {
    let Top5OperationsLabel: string[] = [];
    let Top5OperationsProps: string[] = [];
    let Top5DefectCodeColumns: object[] = [];
    debugger
    let sOperaction = operaction.split('-');
    switch (operaction) {
        case "1600":
            Top5OperationsLabel = _1600ColumnsLabel;
            Top5OperationsProps = _1600ColumnsProps;
            let _1600Columns = GetColumns(_1600ColumnsLabel, _1600ColumnsProps);
            Top5DefectCodeColumns = [

                {
                    title: 'PROD',//label
                    align: 'center',
                    dataIndex: 'PROD1',//prop屬性,
                },
                ..._1600Columns

            ];
            break;

        default:
            Top5OperationsLabel[0] = sOperaction[1];
            Top5OperationsProps[0] = sOperaction[0];
            let _Columns = GetColumns(Top5OperationsLabel, Top5OperationsProps);
            Top5DefectCodeColumns = [

                {
                    title: 'PROD',//label
                    align: 'center',
                    dataIndex: 'PROD1',//prop屬性,
                },
                ..._Columns

            ];
            break;
    }
    return { Top5OperationsLabel, Top5OperationsProps, Top5DefectCodeColumns }

}

function GetColumns(Label: string[], Prop: string[]) {
    return Prop.map((item: string, index: number) => {
        return {
            title: Label[index],
            align: 'center' as any,
            dataIndex: item,
            colSpan: 2,
            children:
                [
                    {
                        title: Label[index],
                        align: 'center',
                        dataIndex: `${item}`,
                        colSpan: 0,
                        rowSpan: 0,
                        customCell: (record: Object, rowIndex: number, column: Object) => {

                            if (!Reflect.has(record, 'OutYield') && record[item] != record[`${item}-DFCT_CODE`]) {
                                return {
                                    colSpan: 1,
                                    rowSpan: 1,
                                    style: {
                                        cursor: 'pointer',
                                    },
                                    onDblclick: () => {
                                        useReportParams.AddFirstTableParams(`DownGradeTop5DefectCodeDetail_${useCommonParams.Fab}`,{ ...record, ...column['__originColumn__'] })
                                 
                                        router.push({ name: `DownGradeTop5DefectCodeDetail_${useCommonParams.Fab}` });

                                        //console.log("輸出record：", record);

                                        //console.log("輸出__originColumn__：", column['__originColumn__']);
                                    }
                                }
                            }
                            // else if(!record[item]||record[item]==) {
                            //     return {
                            //         colSpan: 0,
                            //         rowSpan: 0
                            //     }
                            // }
                            else {
                                return {
                                    colSpan: 2,
                                    rowSpan: 1

                                }
                            }
                        },
                    },
                    {
                        title: Label[index],
                        align: 'center',
                        dataIndex: `${item}-DFCT_CODE`,
                        colSpan: 0,
                        rowSpan: 0,
                        customCell: (record: Object, rowIndex: number, column: Object) => {
                            if (Reflect.has(record, 'OutYield') || record[`${item}-DFCT_CODE`] == record[item]) {
                                return {
                                    colSpan: 0,
                                    rowSpan: 0
                                }
                            }
                            else {
                                return {
                                    colSpan: 1,
                                    rowSpan: 1,
                                    // classname:"LightBlue",
                                    onDblclick: () => {
                                        useReportParams.AddFirstTableParams(`DownGradeTop5DefectCodeDetail_${useCommonParams.Fab}`,{ ...record, ...column['__originColumn__'] })
                                 
                                        router.push({ name: `DownGradeTop5DefectCodeDetail_${useCommonParams.Fab}` });
                                    }, style: {
                                        cursor: 'pointer',
                                        // background: '#9088c0'
                                    }
                                }
                            }

                        },

                    }
                ]
        }
    })
}

function GetTableColumn(Label: string[], Prop: string[]) {
    return Prop.map((item: string, index: number) => {
        return {
            title: Label[index],
            align: 'center' as any,
            dataIndex: item,
            customCell: (record: Object, rowindex: number, column: Object) => {
                return {
                    onDblclick: () => {
                        useReportParams.AddSecondTableParams(`DownGradeTop5DefectCodeDetail_${useCommonParams.Fab}`,{ ...record, ...column['__originColumn__'] })
                                 
                    }, style: { cursor: 'pointer' }
                }
            }
        }
    })
}


