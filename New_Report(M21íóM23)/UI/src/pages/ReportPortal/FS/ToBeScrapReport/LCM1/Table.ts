import { TableCustomCell } from '@/pages/ReportPortal/utils/common';
import { AddCommonPageParamsWithOut } from '@/store/module/commonpage';
import router from '@/router';


export let ToBeScrapLabel = ['product_id', 'Rev', 'scrap_code', 'Total', 'Shipping QTY', 'Total Yield%']
export let ToBeScrapProps = ['PROD_NBR', 'VER', 'scrap_code', 'Total', 'QTY', 'Yield']
export let ToBeScrapDetailLabel = [
    "Panel Id",
    "Src Product",
    "Lcd Product",
    "Lcm Product",
    "目前站點",
    "責任站點",
    "責任歸屬",
    "Final責任歸屬",
    "LCD GRADE",
    "Defect",
    "Defect說明",
    "報廢日期",
    "報廢代碼",
    "報廢原因",
    "Reason",
    "Reason說明",
    "責任人",
    "Comments",
    "合計片數",
]
export let ToBeScrapDetailProps = ['ENTITY_NO', 'RESERVED_16', 'LCD_PROD_NBR', 'PROD_NBR', 'WORK_CTR', 'MAIN_WC', 'RESP_OWNER',
    'FINAL_RESP_OWNER', 'LCD_GRADE', 'DFCT_CODE', 'ERRC_DESCR', 'ACCT_DATE', 'SCRAP_CODE', 'C_DESC', 'DFCT_REASON', 'ERRM_DESCR', 'DUTY', 'COMMENTS', 'TOBESCRAP_QTY']


// export const ToBeScrapColumns = TableCustomCell(ToBeScrapLabel, ToBeScrapProps, ['Total'], false, 'ToBecrapReportCM1MFGDetail')
export const ToBeScrapDetailColumns = TableCustomCell(ToBeScrapDetailLabel, ToBeScrapDetailProps, [], false, '',['ENTITY_NO'])

// import { Avatar, Space, Tag } from 'ant-design-vue';


export const ToBeScrapColumns:any[] = [

    {
        title: 'product_id',//label
        align: 'center',
        dataIndex: 'PROD_NBR',//prop屬性
    },
    {
        title: 'Rev',
        align: 'center',
        dataIndex: 'VER',
    },
    {
        title: 'scrap_code',
        align: 'center',
        dataIndex: 'scrap_code',
        // children:TableColumnItem[]
    }, {
        title: 'Total',
        align: 'center',
        dataIndex: 'Total',
        //@ts-ignore
        customCell: (record: Object, rowindex: number, column: Object) => {
            return {
                onDblclick: () => {
                    const UseParamsAdd = AddCommonPageParamsWithOut()
                    // UseParamsAdd.AddSecondTableParams({ ...record, ...column })
                    UseParamsAdd.AddFirstTableParams({ ...record, ...column['__originColumn__'] })
                    router.push({ name: 'ToBecrapReportCM1MFGDetail' });
                }, style: { cursor: 'pointer' }
            }
        }
    },
    {
        title: 'Shipping QTY',
        align: 'center',
        dataIndex: 'QTY',
        className: 'Yellow'
    }, {
        title: 'Total Yield%',
        align: 'center',
        dataIndex: 'Yield',
        className: 'Yellow'
    }
];