//import { TableCustomCell } from '@/pages/ReportPortal/utils/common';
import { AddCommonPageParamsWithOut } from '@/store/module/commonpage';
import { ReportCommonWithOut } from '@/store/module/ReportCommon';
import router from '@/router';
import { message } from 'ant-design-vue';

const UseParamsAdd = AddCommonPageParamsWithOut()
const useReportParams = ReportCommonWithOut();

export let ToBeScrapLabel = ['Product ID', 'Rev', 'Scrap Code', 'Total', 'Shipping QTY', 'Total Yield%']
export let ToBeScrapProps = ['PROD_NBR', 'VER', 'scrap_code', 'Total', 'QTY', 'Yield']
export let ToBeScrapDetailLabel = [
    "Panel Id",
    "Lcd Product",
    "Lcm Product",
    "送修站點",
    "報廢責任站點",
    "Defect",
    "責任歸屬",
    "報廢日期",
    "報廢代碼",
    "報廢原因",
    "報廢狀態",
    "報廢人員",
    "報廢單號",
    "MRwk Comment",
    "MRwk 時間",
    "合計片數",
]
export let ToBeScrapDetailProps = ['ENTITY_NO', 'LCD_PROD_NBR', 'PROD_NBR', 'MAIN_WC', 'RESP_OPER',
    'DFCT_CODE', 'RESP_OWNER', 'ACCT_DATE', 'SCRAP_CODE',  'C_DESC', 'SFG_STATUS', 'OPERATOR', 'SCRAP_NBR', 'MRWK_COMMENT', 'MRWK_DATE', 'QTY']

// export const ToBeScrapColumns = TableCustomCell(ToBeScrapLabel, ToBeScrapProps, ['Total'], false, 'ToBecrapReportCM1MFGDetail')
export const ToBeScrapDetailColumns = TableCustomCell(ToBeScrapDetailLabel, ToBeScrapDetailProps, [], false, '')

import { Avatar, Space, Tag } from 'ant-design-vue';

function TableCustomCell(Label: string[], Prop: string[], ClickColumn: string[] = [], isMergeColumns: boolean = false, ToOtherpage?: string) {
    return Prop.map((item: string, index: number) => {
        return {
            title: ClickColumn.includes(item) ? `${Label[index]}*` : Label[index],//可点击的栏位显示*号
            align: 'center' as any,
            dataIndex: item,
            //width:150,
            // className: 'v-center',
            //@ts-ignore
            customCell: (record: Object, rowindex: number, column: Object) => {
                if (ClickColumn.includes(item)) {
                    return {
                        onDblclick: () => {
                            //const UseParamsAdd = AddCommonPageParamsWithOut()
                            // UseParamsAdd.AddSecondTableParams({ ...record, ...column })
                            //UseParamsAdd.AddFirstTableParams({ ...record, ...column['__originColumn__'] })
                            useReportParams.AddFirstTableParams(`ToBeScrapReportDetail_${UseParamsAdd.Fab}`,{ ...record, ...column['__originColumn__'] })
                            router.push({ name: ToOtherpage+`_${UseParamsAdd.Fab}` });
                            debugger
                        }, style: { cursor: 'pointer'}
                    }
                }
                else if (isMergeColumns) {

                    return {
                        rowSpan: record?.[`${item}-span`]?.['rowspan'] || 0,
                        colSpan:record?.[`${item}-span`]?.['colspan'] || 0,
                         style: {  }
                    }
                }
                else {
                    return {
                         style: {  },
                        onDblclick: () => {
                            message.warning('此栏位没有二阶数据')
                        }
                    }
                }

            }
        }
    })

}

export const ToBeScrapColumns:any[] = [

    {
        title: 'Product ID',//label
        align: 'center',
        dataIndex: 'PROD_NBR',//prop屬性
        fixed: 'left',
        className: 'v-center'
    },
    {
        title: 'Rev',
        align: 'center',
        dataIndex: 'VER',
        fixed: 'left',
        className: 'v-center'
    },
    {
        title: 'Scrap Code',
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
                    
                    // UseParamsAdd.AddSecondTableParams({ ...record, ...column })
                    //UseParamsAdd.AddFirstTableParams({ ...record, ...column['__originColumn__'] })
                    useReportParams.AddFirstTableParams(`ToBeScrapReportDetail_${UseParamsAdd.Fab}`,{ ...record, ...column['__originColumn__'] })
                    router.push({ name: 'ToBeScrapDetail'+`_${UseParamsAdd.Fab}` });
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
        title: 'Total Yield (%)',
        align: 'center',
        dataIndex: 'Yield',
        className: 'Yellow'
    }
];