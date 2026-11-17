// import router from '@/router';
import { TableCustomCell } from '@/pages/ReportPortal/utils/common';
import router from '@/router';
import { AddCommonPageParamsWithOut } from '@/store/module/commonpage';

//幾種別,人員別
export let DownGradeLabel = ['累計檢驗數量', 'A', 'B', 'C', 'C2', 'C3', 'C4', 'C5', 'C6', 'D', 'E', 'L', 'W',
    'R', 'K', 'G', 'SS', 'Down Grade數量', 'Down Grade比率', '送修數量', '送修比率', '檢驗良品率', 'Down Grade',
    'A GRADE 比率', 'A~C5 GRADE 比率'];
export let DownGradeLabelLCM2 = ['累計檢驗數量', 'A', 'B', 'C', 'C2', 'C3', 'C4', 'C5', 'C6', 'D', 'E', 'L', 'W',
'R', 'K', 'G',  'Down Grade數量', 'Down Grade比率', '送修數量', '送修比率', '檢驗良品率', 'Down Grade',
'A GRADE 比率'];
//機臺良率別
let DownGradeProdLabel = ['Product', 'LCD 等級', '累計檢驗數量', 'A', 'B', 'C', 'C2', 'C3', 'C4', 'C5', 'C6', 'D', 'E', 'L', 'W', 'R', 'K', 'G']

//可點擊列
let ClickProps = DownGradeLabel.filter(item => item != '累計檢驗數量')
//需添加百分號的列
let AddPercentColumns: string[] = ['Down Grade比率','A GRADE 比率', 'A~C5 GRADE 比率', '送修比率', '檢驗良品率',];
let AddPercentColumnsLCM2: string[] = ['Down Grade比率','A GRADE 比率',  '送修比率', '檢驗良品率',];

let DownSecondTableLabel = ['Item', 'Defect', '中文說明', 'grade', '數量']
let DownSecondTableProps = ['ITEM', 'DEFECT_CODE', '中文說明', 'LCM_GRADE', 'QTY']
let DownThridTableLabel = ["Panel Id",
    "Defect",
    "Product",
    "LCM Owner",
    "LCM Grade",
    "Work Order",
    "Operator",
    "Trans_type",
    "Output_fg",
    "X_AXIS",
    "Y_AXIS",
    "Equipment",
    "TRANS_DATE",
    "Pattern_Name"]
    let DownThridTableLabelLCM2 = ["Panel Id",
    "Defect",
    "Product",
    "LCM Owner",
    "LCM Grade",
    "Work Order",
    "Operator",
    "Trans_type",
    "Output_fg",
    "X_AXIS",
    "Y_AXIS",
    "Equipment",
    "TRANS_DATE"]



export const DownSecondColumns = TableCustomCellWidth(DownSecondTableLabel, DownSecondTableProps, DownSecondTableProps, [])
//Data0送修
//Data1 DownGrade

export const DownGradeColumns = TableCustomCellWidth(DownGradeLabel, DownGradeLabel, ClickProps, AddPercentColumns, 'DownGradeDetailLCM1MFG');
export const DownGradeColumnsLCM2 = TableCustomCellWidth(DownGradeLabelLCM2, DownGradeLabelLCM2, ClickProps, AddPercentColumnsLCM2, 'DownGradeDetailLCM2MFG');
export const DownGradeProdColumns = GetProdColumns(DownGradeProdLabel, DownGradeProdLabel)

export const DownThridColumns = TableCustomCell(DownThridTableLabel,DownThridTableLabel,[])
export const DownThridColumnsLCM2 = TableCustomCell(DownThridTableLabelLCM2,DownThridTableLabelLCM2,[])
function TableCustomCellWidth(Label: string[], Prop: string[], ClickColumn: string[] = [], AddPercent: string[], ToOtherpage?: string) {
    return Prop.map((item: string, index: number) => {
        return {
            title:  Label[index],
            align: 'center' as any,
            dataIndex: item,
            customRender: ({ text }) => {
                if (AddPercent.includes(item)) {
                    return (<span>
                        {text}%
                    </span>)
                }
            },
            //@ts-ignore
            customCell: (record: Object, rowindex: number, column: Object) => {
                if (ClickColumn.includes(item)) {
                    return {
                        ondblclick: () => {
                            const UseParamsAdd = AddCommonPageParamsWithOut()
                            if (AddPercent.length > 0) UseParamsAdd.AddFirstTableParams({ ...record, ...column['__originColumn__'] })
                            else UseParamsAdd.AddSecondTableParams({ ...record, ...column['__originColumn__'] })
                            if (ToOtherpage) router.push({ name: ToOtherpage })

                        }, style: { cursor: 'pointer' }
                    }
                }


            }
        }
    })

}

function GetProdColumns(Label: string[], Prop: string[]) {
    return Prop.map((item: string, index: number) => {
        return {
            title: Label[index],
            align: 'center' as any,
            dataIndex: item,
            //@ts-ignore
            customCell: (record: Object, rowindex: number, column: Object) => {
                if (item == 'Product') {
                    return {
                        rowSpan: record?.[`${item}-span`]?.['rowspan'],
                        colSpan: record?.[`${item}-span`]?.['colspan']
                    }
                }

            }
        }
    })

}