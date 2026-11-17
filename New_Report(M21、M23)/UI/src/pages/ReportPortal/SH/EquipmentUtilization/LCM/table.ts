import router from '@/router';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { ReportCommon } from '@/store/module/ReportCommon';
import { reactive } from 'vue';


const useCommonParams = AddCommonPageParams();
let useReportCommon = ReportCommon();

export const baseColumns: any[] = [
    {
        title: 'Status',
        align: 'center',
        dataIndex: 'CODE',
    }, 
    {
        title: 'Code_desc',
        align: 'center',
        dataIndex: 'C_DESC',
    }
];

const customCellRender = (record: Object, rowIndex: number, column: Object) => {
  return {
      ondblclick: () => { 
         useReportCommon.AddFirstTableParams(`EquipmentUtilization-${useCommonParams.Fab}`,{ ...record, ...column['__originColumn__'] }) 
          router.push({ name: `EquipmentUtilizationDetail-${useCommonParams.Fab}`});
      },
      style: {
          cursor: 'pointer'
      }
  };
};

export const detail1: any[] = [
    {
        title: 'son code',
        align: 'center',
        dataIndex: `SON_CODE`,
    },
    {
        title: 'son_code_desc',
        align: 'center',
        dataIndex: 'SON_CODE_DESC',
    },
    {
        title: 'usetime',
        align: 'center',
        dataIndex: 'USETIME',
        customCell: customCellRender,
    },
    
];


export const detail2: any[] = [
    {
        title: 'equip_nbr',
        align: 'center',
        dataIndex: `EQUIP_NBR`,
        width:70,
    },
    {
        title: 'equip_grp',
        align: 'center',
        dataIndex: 'EQUIP_GRP',
        width:70,
    },
    {
        title: 'son code',
        align: 'center',
        dataIndex: 'SON_CODE',
        width:70,
    },
    {
        title: 'eq status',
        align: 'center',
        dataIndex: 'EQ_STATUS',
        width:60,
    },
    {
        title: 'entity status',
        align: 'center',
        dataIndex: 'ENTITY_STATUS',
        width:60,
    },
    {
        title: 'operator',
        align: 'center',
        dataIndex: 'OPERATORS',
        width:60,
    },
    {
        title: 'comments',
        align: 'center',
        dataIndex: 'COMMENTS',
        width:80,
    },
    {
        title: 'stay time',
        align: 'center',
        dataIndex: 'STAY_TIME',
        width:80,
    },
    {
        title: 'start time',
        align: 'center',
        dataIndex: 'START_TIME',
        width:110,
    },
    {
        title: 'close time',
        align: 'center',
        dataIndex: 'CLOSE_TIME',
        width:110,
    },
];
