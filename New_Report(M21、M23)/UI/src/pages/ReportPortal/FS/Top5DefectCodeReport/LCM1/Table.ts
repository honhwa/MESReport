import { AddCommonPageParamsWithOut } from '@/store/module/commonpage';
import router from '@/router';

export const Top5OperationsLabel = ['PCBI', 'BOND CKEN', 'A KEN', 'De Mura', 'A GING', 'C KEN', 'D KEN', 'Total']
export const Top5OperationsProps = ['1300', '1350', '1450', '1465', '1500', '1600', '1700', 'Total']
export const Top5OperationsDetailLabel = ['prod_nbr', 'entity_no', 'trans_date', 'trans_type', 'equip_nbr', 'A-PALLET', 'B-PALLET', 'C-PALLET', 'operator', 'output_fg', 'work_ctr', 'to_wc', 'defect_code', 'pre_dfctcode', 'rework 次數', 'dfct 次數', 'Pattern_Name', '合計片數'];

export const Top5OperationsDetailProps = ["PROD_NBR",
    "ENTITY_NO",
    "TRANS_DATE",
    "TRANS_TYPE",
    "EQUIP_NBR",
    "A-PALLET",
    "B-PALLET",
    "C-PALLET",
    "OPERATOR",
    "OUTPUT_FG",
    "WORK_CTR",
    "TO_WC",
    "DFCT_CODE",
    "PRE_DFCTCODE",
    "STIMES_TTL",
    "ITEM_NBR",
    "PATTERN_NAME",
    "PASS_QTY"]

export const Top5OperationsSecondLabel = ['COG', 'OLB', 'PCB', 'PCBI', 'ASSEMBLY', 'A GING', 'C-', 'D-', 'OQC']
export const Top5OperationsSecondProps = ['1050', '1100', '1200', '1300', '1400', '1500', '1600', '1700', '1900']

const Top5Columns = GetColumns(Top5OperationsLabel, Top5OperationsProps)
export const Top5DetailColumns = GetTableColumn(Top5OperationsDetailLabel, Top5OperationsDetailProps)
export const Top5SecondColumns = GetSecondTableColumn(Top5OperationsSecondLabel, Top5OperationsSecondProps)


export const Top5DefectCodeColumns: any[] = [

    {
        title: 'PROD',//label
        align: 'center',
        dataIndex: 'PROD1',//prop屬性,
        colSpan:'1',
        rowSpan:'1',
        fixed:'left',
    },
    ...Top5Columns

];

function GetColumns(Label: string[], Prop: string[]) {
    return Prop.map((item: string, index: number) => {
        return {
            title: Label[index],
            align: 'center' as any,
            dataIndex: item,
            colSpan: 2,
            rowSpan:1,
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
                                        console.log(record, column['__originColumn__']);

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
                                        router.push({ name: 'Top5DefectCode301Detail' });
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
                        UseParamsAdd.AddSecondTableParams({ ...record, ...column['__originColumn__'] })
                    }, style: { cursor: 'pointer' }
                }
            }
        }
    })
}

function GetSecondTableColumn(Label: string[], Prop: string[]) {
    let SecondChildDataProps: any[] = ["EQUIP_NBR",
    "TRANS_DATE1",
    "TRANS_DATE2",
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

    Prop.map((item: string, index: number) => {
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

