import { TableCustomCell } from '@/pages/ReportPortal/utils/common';

// const LA501Label = ['EQPT_ID','Proc_ID','UP_Rate','Run_Rate','Sequence','Move_Target','UPH_ILEF']

// const LA501Prop = ['EQUIP_ID','PROC_ID','UP_Rate','RUN_RATE','SEQ_CD','MOVE_CNT','UPH_ILEF']
//为什么不使用 TableCustomCell？结构不清晰
export const LA501BaseColumns = [
    {
        title: 'EQPT_ID',
        field: 'EQUIP_ID',
        // fixed:'left'
    }, {
        title: 'Proc_ID',
        field: 'PROC_ID'
    }, {
        title: 'UP_Rate',
        field: 'UP_Rate'
    }, {
        title: 'Run_Rate',
        field: 'RUN_RATE'
    }, {
        title: 'Sequence',
        field: 'SEQ_CD'
    }, {
        title: 'Move_Target',
        field: 'MOVE_CNT'
    }, {
        title: 'UPH_ILEF',
        field: 'UPH_ILEF'
    }
]