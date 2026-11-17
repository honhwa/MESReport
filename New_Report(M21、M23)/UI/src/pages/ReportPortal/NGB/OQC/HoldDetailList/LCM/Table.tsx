import type { TableColumn } from '@/components/core/dynamic-table';
// import { Avatar, Space, Tag } from 'ant-design-vue';

export type TableListItem = API.DayliMoveThItem;
export type TableColumnItem = TableColumn<TableListItem>;
export const baseColumns: TableColumnItem[] = [

    {title: '6000出賬時間',align: 'center',dataIndex: 'TRANS_DATE',className: 'v-center'},
    {title: 'Carton ID',align: 'center',dataIndex: 'CART_ID',className: 'v-center'},
    {title: 'CART_ID_CUST',align: 'center',dataIndex: 'CART_ID_CUST',className: 'v-center'},
    {title: '确發Hold PNL_ID',align: 'center',dataIndex: 'PNL_ID',className: 'v-center'},
    {title: '機種',align: 'center',dataIndex: 'PROD_NBR',className: 'v-center'},
    {title: '判定結果',align: 'center',dataIndex: 'RANK',className: 'v-center'},
    {title: 'Hold Defect',align: 'center',dataIndex: 'DFCT_CODE',className: 'v-center'},
    {title: 'hold狀態',align: 'center',dataIndex: 'CUR_HOLD_CODE',className: 'v-center'},
    {title: 'ES_WMS_STATU',align: 'center',dataIndex: 'MES_WMS_STATUS',className: 'v-center'},
    {title: 'WMS_PUT_FLAG',align: 'center',dataIndex: 'WMS_PUT_FLAG',className: 'v-center'},
    {title: '圈定時',align: 'center',dataIndex: 'START_DATE',className: 'v-center'},
    {title: '觸發站',align: 'center',dataIndex: 'STATION',className: 'v-center'}
];


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
            
                    return {
                         style: { width:40 }
                    }
            }
        }
    })

}