export const LCM_LINEColumns = [
        {
            title: '',
            align: 'center',
            dataIndex: ``,
            className: 'v-center'
        },
        {
            title: "",
            align: 'center',
            dataIndex: "",
            className: 'v-center'
        },
        
    ];


export const LinebottomColumns = [
    {
        title: 'BL',
        align: 'center',
        children: [
            {
                title: 'Line',
                align: 'center',
                dataIndex: `BL_LINE`
            },
            {
                title: 'Prod Nbr',
                align: 'center',
                dataIndex: `PROD_NBR`
            },
            {
                title: 'MPS Plan',
                align: 'center',
                dataIndex: `BL_MPS_PLAN`
            },
            {
                title: 'Act',
                align: 'center',
                dataIndex: `BL_PASS_ACT`
            },
            {
                title: 'Diff',
                align: 'center',
                // BL_Diff = BL_PASS_ACT - BL_MPS_PLAN
                dataIndex: `BL_Diff`
            },
            {
                title: 'FPY(%)',
                align: 'center',
                dataIndex: `BL_FPY_PLAN`
            },
            {
                title: 'Act(%)',
                align: 'center',
                dataIndex: `BL_FPY_ACT`
            },
            {
                title: 'Bank',
                align: 'center',
                dataIndex: `BL_WIP`
            }
        ]
    }
];
