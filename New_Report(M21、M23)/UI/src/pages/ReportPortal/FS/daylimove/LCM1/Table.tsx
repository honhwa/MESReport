import type { TableColumn } from '@/components/core/dynamic-table';
import { cloneDeep } from 'lodash-es';
export type TableListItem = API.DayliMoveThItem;
export type TableColumnItem = TableColumn<TableListItem>;
export const LCM1Columns = [
    {
        title: 'Operation', //label
        align: 'center',
        dataIndex: 'WORK_CTR',
        resizable: true,
        fixed: 'left',
    },
    {
        title: 'Work Order',
        align: 'center',
        dataIndex: 'WO_NBR',
        resizable: true
    },
    {
        title: 'Size',
        align: 'center',
        dataIndex: 'SIZE1',
        resizable: true
    },
    {
        title: 'Owner',
        align: 'center',
        dataIndex: 'LCM_OWNER',
        resizable: true
    },
    {
        title: 'Product',
        align: 'center',
        dataIndex: 'PROD_NBR',
    },
    {
        title: 'Customer',
        align: 'center',
        dataIndex: 'CUSTOMER',
        resizable: true
    }
]
export const LCM2Columns = cloneDeep(LCM1Columns);
export const BLUColumns = [
    {
        title: 'Operation', //label
        align: 'center',
        dataIndex: 'WORK_CTR',
        resizable: true,
        fixed: 'left',
    },
    {
        title: 'Work Order',
        align: 'center',
        dataIndex: 'WO_NBR',
        resizable: true
    },
    {
        title: 'Size',
        align: 'center',
        dataIndex: 'SIZE1',
        resizable: true
    },
    {
        title: 'Owner',
        align: 'center',
        dataIndex: 'LCM_OWNER',
        resizable: true
    },
    {
        title: 'Product',
        align: 'center',
        dataIndex: 'PROD_NBR',
    },
];
export const SET1PCColumns= cloneDeep(LCM1Columns);
export const SET1TestColumns = [
    {
        title: 'Operation', //label
        align: 'center',
        dataIndex: 'WORK_CTR',
        resizable: true,
        fixed: 'left',
    },
    {
        title: 'Work Order',
        align: 'center',
        dataIndex: 'WO_NBR',
        resizable: true
    },
    {
        title: 'Size',
        align: 'center',
        dataIndex: 'SIZE1',
        resizable: true
    },
    {
        title: 'Owner',
        align: 'center',
        dataIndex: 'LCM_OWNER',
        resizable: true
    },
    {
        title: 'Product',
        align: 'center',
        dataIndex: 'PROD_NBR',
    },
    {
        title: 'Equip_Nbr',
        align: 'center',
        dataIndex: 'EQUIP_NBR',
    },
    {
        title: 'Customer',
        align: 'center',
        dataIndex: 'CUSTOMER',
        resizable: true
    }
];
export const SET1TestNewColumns = [
    {
        title: 'Operation', 
        align: 'center',
        dataIndex: 'WORK_CTR',
        resizable: true,
        fixed: 'left',
    },
    {
        title: 'Owner',
        align: 'center',
        dataIndex: 'LCM_OWNER',
        resizable: true
    },
    {
        title: 'Product',
        align: 'center',
        dataIndex: 'PROD_NBR',
    },
    {
        title: 'Equip_Nbr',
        align: 'center',
        dataIndex: 'EQUIP_NBR',
    },
];
export const SET2PCColumns= cloneDeep(LCM1Columns);
export const SET2TestColumns = cloneDeep(SET1TestColumns);
export const SET2TestNewColumns = cloneDeep(SET1TestNewColumns);
export const baseColumns: TableColumnItem[] = [
    
];
