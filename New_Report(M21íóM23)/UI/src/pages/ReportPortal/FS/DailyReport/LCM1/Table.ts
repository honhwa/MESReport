export const SumColumns = (Date: string) => {
    return [
        {
            title: '',
            align: 'center',
            dataIndex: `prod`,
            className: 'v-center'
        },
        {
            title: Date,
            align: 'center',
            dataIndex: Date,
            className: 'v-center'
        },
        {
            title: 'Daily Input',
            align: 'center',
            children: [
                {
                    title: 'ALL',
                    align: 'center',
                    dataIndex: `INPUT`
                },
                {
                    title: 'Shift',
                    align: 'center',
                    dataIndex: `INPUT_SHIFT`
                }
            ]
        },
        {
            title: 'MTD Input',
            align: 'center',
            children: [
                {
                    title: 'MTD Input',
                    align: 'center',
                    dataIndex: `MONTH_INPUT`
                }
            ]
        },
        {
            title: 'Daily Output',
            align: 'center',
            children: [
                {
                    title: 'ALL',
                    align: 'center',
                    dataIndex: `OUTPUT`
                },
                {
                    title: 'Shift',
                    align: 'center',
                    dataIndex: `OUTPUT_SHIFT`
                }
            ]
        },
        {
            title: 'MTD Output',
            align: 'center',
            children: [
                {
                    title: 'MTD Output',
                    align: 'center',
                    dataIndex: `MONTH_OUTPUT`
                }
            ]
        },
        {
            title: 'FPY(%)',
            align: 'center',
            children: [
                {
                    title: 'FPY(%)',
                    align: 'center',
                    dataIndex: `RATE`
                }
            ]
        },
        {
            title: 'WIP/Bank',
            align: 'center',
            children: [
                {
                    title: 'WIP/Bank',
                    align: 'center',
                    dataIndex: `WIP`
                }
            ]
        }
    ];
};
export const LineTopcolumns = [
    {
        title: 'ITEM',
        align: 'center',
        dataIndex: `FLOOR`,
        className: 'v-center'
    },
    {
        title: 'Bonding',
        align: 'center',
        children: [
            {
                title: 'Bonding',
                align: 'center',
                dataIndex: `BOND_LINE`
            },
            {
                title: 'SIZE',
                align: 'center',
                dataIndex: `PROD_SIZE`
            },
            {
                title: 'PROD_NBR',
                align: 'center',
                dataIndex: `PROD_NBR`
            }
        ]
    },
    {
        title: 'Bonding',
        align: 'center',
        children: [
            {
                title: 'MPS Plan',
                align: 'center',
                dataIndex: `BOND_MPS_PLAN`
            },
            {
                title: 'Act',
                align: 'center',
                dataIndex: `BOND_PASS_ACT`
            },
            {
                title: 'Diff',
                align: 'center', // 值爲BOND_PASS_ACT - BOND_MPS_PLAN
                dataIndex: `BOND_Diff`
            },
            { title: 'FPY(%)', align: 'center', dataIndex: 'BOND_FPY_PLAN' },
            { title: 'Act(%)', align: 'center', dataIndex: 'BOND_FPY_ACT' },
            { title: 'Scrap', align: 'center', dataIndex: 'BOND_SCRAP_QTY' },
            { title: 'Scrap%', align: 'center', dataIndex: 'BOND_SCRAP_YIELD' },
            { title: 'WIP', align: 'center', dataIndex: 'BOND_WIP' }
        ]
    },
    {
        title: 'Assy',
        align: 'center',
        children: [
            { title: 'Assy', align: 'center', dataIndex: 'ASSY_LINE' },
            { title: 'MPS Plan', align: 'center', dataIndex: 'ASSY_MPS_PLAN' },
            { title: 'Act', align: 'center', dataIndex: 'ASSY_PASS_ACT' },
            // ASSY_Diff = ASSY_PASS_ACT - ASSY_MPS_PLAN
            { title: 'Diff', align: 'center', dataIndex: 'ASSY_Diff' },
            { title: 'FPY(%)', align: 'center', dataIndex: 'ASSY_FPY_PLAN' },
            { title: 'Act(%)', align: 'center', dataIndex: 'ASSY_FPY_ACT' },
            { title: 'Scrap', align: 'center', dataIndex: 'ASSY_SCRAP_QTY' },
            { title: 'Scrap%', align: 'center', dataIndex: 'ASSY_SCRAP_YIELD' },
            { title: 'WIP', align: 'center', dataIndex: 'ASSY_WIP' }
        ]
    },
    {
        title: 'CDP',
        align: 'center',
        children: [
            { title: 'CDP', align: 'center', dataIndex: 'CDP_LINE' },
            { title: 'MPS Plan', align: 'center', dataIndex: 'CDP_MPS_PLAN' },
            { title: 'Act', align: 'center', dataIndex: 'CDP_PASS_ACT' },
            // CDP_Diff = CDP_PASS_ACT-CDP_MPS_PLAN
            { title: 'Diff', align: 'center', dataIndex: 'CDP_Diff' },
            { title: 'FPY(%)', align: 'center', dataIndex: 'CDP_FPY_PLAN' },
            { title: 'Act(%)', align: 'center', dataIndex: 'CDP_FPY_ACT' },
            { title: 'Scrap', align: 'center', dataIndex: 'CDP_SCRAP_QTY' },
            { title: 'Scrap%', align: 'center', dataIndex: 'CDP_SCRAP_YIELD' },
            { title: 'WIP', align: 'center', dataIndex: 'CDP_WIP' }
        ]
    }
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
