import type { TableColumn } from '@/components/core/dynamic-table';
import { cloneDeep } from 'lodash-es';
export type TableListItem = API.DayliMoveThItem;
export type TableColumnItem = TableColumn<TableListItem>;
export const LCM1Columns = [
    {
        title: 'Operation', //label
        align: 'center',
        dataIndex: 'WORK_CTR',
        fixed: 'left',
        className:'v-center',
       
    },
    {
        title: 'Work Order',
        align: 'center',
        dataIndex: 'WO_NBR',
        fixed: 'left',
        className:'v-center'
    },
    {
        title: 'Size',
        align: 'center',
        dataIndex: 'SIZE1',
        fixed: 'left',
        className:'v-center'
    },
    {
        title: 'Owner',
        align: 'center',
        dataIndex: 'LCM_OWNER',
        fixed: 'left',
        className:'v-center'
    },
    {
        title: 'Product',
        align: 'center',
        dataIndex: 'PROD_NBR',
        fixed: 'left',
        className:'v-center'

    },
    {
        title: 'Customer',
        align: 'center',
        dataIndex: 'CUSTOMER',
        fixed: 'left',
        className:'v-center'

    }
];
export const LCM2Columns = cloneDeep(LCM1Columns);
export const BLUColumns = [
    {
        title: 'Operation', //label
        align: 'center',
        dataIndex: 'WORK_CTR',
        fixed: 'left',
        className:'v-center'

    },
    {
        title: 'Work Order',
        align: 'center',
        dataIndex: 'WO_NBR',
        fixed: 'left',
        className:'v-center'

    },
    {
        title: 'Size',
        align: 'center',
        dataIndex: 'SIZE1',
        fixed: 'left',
        className:'v-center'

    },
    {
        title: 'Owner',
        align: 'center',
        dataIndex: 'LCM_OWNER',
        fixed: 'left',
        className:'v-center'

    },
    {
        title: 'Product',
        align: 'center',
        dataIndex: 'PROD_NBR',
        fixed: 'left',
        className:'v-center'

    }
];
export const SET1PCColumns = cloneDeep(LCM1Columns);
export const SET1TestColumns = [
    {
        title: 'Operation', //label
        align: 'center',
        dataIndex: 'WORK_CTR',
        fixed: 'left',
        className:'v-center'

    },
    {
        title: 'Work Order',
        align: 'center',
        dataIndex: 'WO_NBR',
        fixed: 'left',
        className:'v-center'

    },
    {
        title: 'Size',
        align: 'center',
        dataIndex: 'SIZE1',
        fixed: 'left',
        className:'v-center'

    },
    {
        title: 'Owner',
        align: 'center',
        dataIndex: 'LCM_OWNER',
        fixed: 'left',
        className:'v-center'

    },
    {
        title: 'Product',
        align: 'center',
        dataIndex: 'PROD_NBR',
        fixed: 'left',
        className:'v-center'

    },
    {
        title: 'Equip_Nbr',
        align: 'center',
        dataIndex: 'EQUIP_NBR',
        fixed: 'left',
        className:'v-center'

    },
    {
        title: 'Customer',
        align: 'center',
        dataIndex: 'CUSTOMER',
        fixed: 'left',
        className:'v-center'

    }
];
export const SET1TestNewColumns = [
    {
        title: 'Operation',
        align: 'center',
        dataIndex: 'WORK_CTR',
        fixed: 'left',
        className:'v-center'

    },
    {
        title: 'Owner',
        align: 'center',
        dataIndex: 'LCM_OWNER',
        fixed: 'left',
        className:'v-center'

    },
    {
        title: 'Product',
        align: 'center',
        dataIndex: 'PROD_NBR',
        fixed: 'left',
        className:'v-center'

    },
    {
        title: 'Equip_Nbr',
        align: 'center',
        dataIndex: 'EQUIP_NBR',
        fixed: 'left',
        className:'v-center'

    }
];
export const SET2PCColumns = cloneDeep(LCM1Columns);
export const SET2TestColumns = cloneDeep(SET1TestColumns);
export const SET2TestNewColumns = cloneDeep(SET1TestNewColumns);
export const baseColumns: TableColumnItem[] = [];
