import { TableCustomCell } from '@/pages/ReportPortal/utils/common';



export let MoveOutTimeLabel = ['Group', 'Date', 'Equipment', 'Product', 'Tact Time']
export let MoveOutTimeProps = ['MAINEQP', 'TRANS_DATE1', 'EQUIP_NBR', 'PROD_NBR', 'TactTime']
export let MoveOutTimeDetailLabel = [
    "Date",
    "Time",
    "Equipment",
    "Product",
    "Tact Time",
]
export let MoveOutTimeDetailProps = [
    "TRANS_DATE1",
    "Time",
    "EQUIP_NBR",
    "PROD_NBR",
    "TactTime",
]


export const MoveOutTimeColumns = TableCustomCell(MoveOutTimeLabel, MoveOutTimeProps, ['TactTime'],false, 'MoveOutTimeLCM2IEDetail')
export const MoveOutTimeDetailColumns = TableCustomCell(MoveOutTimeDetailLabel, MoveOutTimeDetailProps, [],false, '')