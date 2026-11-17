//import {TableCustomCell} from '@/pages/ReportPortal/utils/TNcommon'
import router from '@/router';
import { message } from 'ant-design-vue';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { ReportCommonWithOut } from '@/store/module/ReportCommon';

const useCommonParams = AddCommonPageParams();
const useReportCommon = ReportCommonWithOut();
export let WipPanSumLabel = [
    "Product",
    "Revision",
    "Panel ID",
    "Owner",
    "Operation",
    "Operation Desc",
    "Cassette ID",
    "SLOT NO",
    "LCD Product",
    "LCD Grade",
    "Exp/RMA/Resorting No",
    "Status",
    "Hold",
    "Rework",
    "Stay Hrs",
    "Date",
    "Hold Category",
    "Hold Comments",
    "Priority",
    "Start Time",
    "Start Hours",
    "Plan Start DT",
    "Due DT",
    "Floor",
    "PC CUST",
    "PRE Operation(abn.)",
    "PRE Operation(RW)"
    ]
let WipPanSumProp = [
"PROD_NBR",
"VER",
"PNL_ID",
"LCM_OWNER",
"WORK_CTR",
"WORK_NAME",
"CST_ID",
"SLOT_NO",
"PANEL_PN",
"LCD_GRADE",
"WO_NBR",
"STATUS",
"HOLD_FLAG",
"STIMES_TTL",
"STAY_HOURS",
"TRANS_DATE",
"HOLD_CODE",
"PRIORITY",
"HOLD_COMMENT",
"START_TIME",
"START_HOURS",
"PLAN_ST_DT",
"DUE_DT",
"FLOOR",
"RESERVED_10",
"PRE_OPER",
"P_RWK_OPER",
]

let WipPanSumWidth=[100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100]
//let WipPanSumWidth=[]

export let WipPanSumDetailCol = TableCustomCell(WipPanSumLabel,WipPanSumProp,[],WipPanSumWidth)

function TableCustomCell(Label: string[], Prop: string[], ClickColumn: string[] = [],Width:number[]=[],isMergeColumns: boolean = false, ToOtherpage?: string) {
    return Prop.map((item: string, index: number) => {
        return {
            title: ClickColumn.includes(item) ? `${Label[index]}*` : Label[index],//可点击的栏位显示*号
            align: 'center' as any,
            dataIndex: item,
            col:index,
            width: Width[index],
            // className: 'v-center',
            //@ts-ignore
            customCell: (record: Object, rowindex: number, column: Object) => {
                if (ClickColumn.includes(item)) {
                    return {
                        onDblclick: () => {
                            //const UseParamsAdd = AddCommonPageParamsWithOut()
                            // UseParamsAdd.AddSecondTableParams({ ...record, ...column })
                            //UseParamsAdd.AddFirstTableParams({ ...record, ...column['__originColumn__'] })
                            useReportCommon.AddFirstTableParams(`WipPanelSummaryDetail_${useCommonParams.Fab}`,{ ...record, ...column['__originColumn__'] })
                            router.push({ name: `WipPanelSummaryDetail_${useCommonParams.Fab}`});
                            if (ToOtherpage) router.push({ name: ToOtherpage });
                        }, style: { cursor: 'pointer',width:40 }
                    }
                }
                else if (isMergeColumns) {
                    return {
                        rowSpan: record?.[`${item}-span`]?.['rowspan'] || 0,
                        colSpan:record?.[`${item}-span`]?.['colspan'] || 0,
                         style: { width:40 }
                    }
                }
                else {
                    return {
                         style: { width:40 },
                        onDblclick: () => {
                            message.warning('此栏位没有二阶数据')
                        }
                    }
                }

            }
        }
    })

}