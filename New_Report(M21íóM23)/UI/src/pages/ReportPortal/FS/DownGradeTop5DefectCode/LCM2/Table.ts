import { AddCommonPageParamsWithOut } from '@/store/module/commonpage';
import router from '@/router';

const AllColumnsLabel = ["BONC", "C Ken", "C2 Ken", "Total"];
const AllColumnsProps = ["1350", "1600", "1610", "Total"];
let _1300ColumnsLabel = ["1300 Ken 狀況"];
let _1350ColumnsProps = ["1350"];
let _1600ColumnsLabel = ["1600 Ken 狀況"];
let _1600ColumnsProps = ["1600"];
let _1350ColumnsLabel = ["1350 Ken 狀況"];
let _1300ColumnsProps = ["1300"];
export const Top5OperationsDetailLabel = ['prod_nbr', 'entity_no', 'trans_date', 'trans_type', 'equip_nbr', 'operator', 'grade', 'output_fg', 'work_ctr', 'to_wc', 'defect_code', 'dfct_grade', 'rework 次數', 'dfct 次數', 'Pattern_Name', '合計片數'];



export const Top5OperationsDetailProps = ["PROD_NBR",
    "ENTITY_NO",
    "TRANS_DATE",
    "TRANS_TYPE",
    "EQUIP_NBR",
    "OPERATOR",
    "LCM_GRADE",
    "OUTPUT_FG",
    "WORK_CTR",
    "TO_WC",
    "DFCT_CODE",
    "DFCT_GRADE",
    "STIMES_TTL",
    "ITEM_NBR",
    "PATTERN_NAME",
    "PASS_QTY"]

export const Top5OperationsSecondLabel = ['COG', 'OLB', 'PCB', 'PCBI', 'ASSEMBLY', 'A GING', 'C-', 'D-', 'OQC']
export const Top5OperationsSecondProps = ['1050', '1100', '1200', '1300', '1400', '1500', '1600', '1700', '1900']
export const Top5DetailColumns = GetTableColumn(Top5OperationsDetailLabel, Top5OperationsDetailProps)
export const Top5SecondColumns = GetSecondTableColumn(Top5OperationsSecondLabel, Top5OperationsSecondProps)

export function ChangeColumns(operaction: string) {
    let Top5OperationsLabel: string[] = [];
    let Top5OperationsProps: string[] = [];
    let Top5DefectCodeColumns: object[] = [];
    switch (operaction) {
        case "1300":
            Top5OperationsLabel = _1300ColumnsLabel;
            Top5OperationsProps = _1300ColumnsProps;
            let _1300Columns = GetColumns(_1300ColumnsLabel, _1300ColumnsProps);
            Top5DefectCodeColumns = [

                {
                    title: 'PROD',//label
                    align: 'center',
                    dataIndex: 'PROD1',//prop屬性,
                },
                ..._1300Columns

            ];
            break;
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
        case "1350":
            Top5OperationsLabel = _1350ColumnsLabel;
            Top5OperationsProps = _1350ColumnsProps;
            let _1350Columns = GetColumns(_1350ColumnsLabel, _1350ColumnsProps);
            Top5DefectCodeColumns = [

                {
                    title: 'PROD',//label
                    align: 'center',
                    dataIndex: 'PROD1',//prop屬性,
                },
                ..._1350Columns

            ];
            break;
        default:
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
                                        const UseParamsAdd = AddCommonPageParamsWithOut()
                                        UseParamsAdd.AddFirstTableParams({ ...record, ...column['__originColumn__'] })
                                        console.log("輸出record：", record);

                                        console.log("輸出__originColumn__：", column['__originColumn__']);

                                        router.push({ name: 'Top5DefectCode301Detail' });
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
                                        const UseParamsAdd = AddCommonPageParamsWithOut()
                                        UseParamsAdd.AddFirstTableParams({ ...record, ...column['__originColumn__'] })
                                        /* console.log("輸出record：",record);
                                        console.log("輸出__originColumn__：", column['__originColumn__']); */
                                        router.push({ name: 'DownGradeTop5DefectCodeDetail' });
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
                        const UseParamsAdd = AddCommonPageParamsWithOut()
                        UseParamsAdd.AddSecondTableParams({ ...record, ...column['__originColumn__'], rowindex })
                    }, style: { cursor: 'pointer' }
                }
            }
        }
    })
}

function GetSecondTableColumn(Label: string[], Prop: string[]) {
    /* let AGINGLabel = [
        "TA",
        "trans_type",
        "comment",
    ]
    let AGINGProps = [
        "TRANS_DATE2",
        "OPERATOR",
        "TRANS_TYPE",
    ] */
    let SecondChildDataProps: any[] = [
        "EQUIP_NBR",
        "TRANS_DATE",
        "TRANS_DATE",
        "OPERATOR",
        "TRANS_TYPE",
        "COMMENT1"]
    let SecondChildDataLabel: any[] = [
        "Eqp",
        "TNnsDT",
        "TA",
        "trans_type",
        "comment",
        "defect_code"]
    let Columns: any[] = [{
        title: 'Panel ID',
        align: 'center',
        dataIndex: 'ENTITY_NO',
    }, {
        title: 'prod_nbr',
        align: 'center',
        dataIndex: 'PROD_NBR',
    }]
    /* let tempProps:any[] = [];
    let tempLabels:any[] = []; */
    Prop.map((item: string, index: number) => {
        /*  if (item == "1050") {
             tempProps = AGINGLabel;
             tempLabels = AGINGProps;
         }else{
             tempProps = SecondChildDataProps;
             tempLabels = SecondChildDataLabel;
         } */
        let ChildCoulmns = SecondChildDataProps.map((item1: string, index1: number) => {
            return {
                title: SecondChildDataLabel[index1],
                align: 'center' as any,
                dataIndex: `${item}-${item1}`,
            }
        })

        Columns.push({
            title: Label[index],
            align: 'center' as any,
            dataIndex: item,
            children: ChildCoulmns

        })
    })
    return Columns


}

