// import router from '@/router';
// import { TableCustomCell } from '@/pages/ReportPortal/utils/common';
// import router from '@/router';
// import { AddCommonPageParamsWithOut } from '@/store/module/commonpage';



export let MFGOperatorColumns = {
    'operators':[{
        title: 'Operators',
        align: 'center',
        dataIndex: 'QITEM',
        fixed:'left',
        className:'v-center',
    },
    {
        title: 'Name',
        align: 'center',
        dataIndex: 'USER_NAME'
    },
    {
        title: 'Sum',
        align: 'center',
        dataIndex: 'Sum'
    },],
    'equip_nbr':[
        {
            title: 'Equipment',
            align: 'center',
            dataIndex: 'QITEM',
            fixed:'left',
            className:'v-center',
        },
        {
            title: 'Sum',
            align: 'center',
            dataIndex: 'Sum'
        },
    ]
}


