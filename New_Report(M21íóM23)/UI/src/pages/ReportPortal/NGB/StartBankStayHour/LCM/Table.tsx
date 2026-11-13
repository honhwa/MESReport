// import type { TableColumn } from '@/components/core/dynamic-table';
import router from '@/router';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { ReportCommon } from '@/store/module/ReportCommon';
import { reactive } from 'vue';

// import { Avatar, Space, Tag } from 'ant-design-vue';
//import { AddCommonPageParamsWithOut } from '@/store/module/commonpage';
const useCommonParams = AddCommonPageParams();
let useReportCommon = ReportCommon();

const customCellRender = (record: Object, rowIndex: number, column: Object) => {
  return {
      ondblclick: () => { 
         useReportCommon.AddFirstTableParams(`StartBankStayHour-${useCommonParams.Fab}`,{ ...record, ...column['__originColumn__'] }) 
          router.push({ name: `StartBankStayHourDetail-${useCommonParams.Fab}`});
      },
      style: {
          cursor: 'pointer'
      }
  };
};



export const FirstLabel =[ 
  {
    title: 'size',
    align: 'center',
    dataIndex: 'SIZE1',
    width:35,
    customCell: (record) => {
        let text = record['SIZE1']
        let obj = {
          text,
          style: {}
        }
        obj['rowSpan'] = record?.['SIZE1-span']?.['rowspan'] || 0
        return obj
      }
  },  
  {
    title: 'oper',
    align: 'center',
    dataIndex: 'OPER',
    width:35,
    customCell: customCellRender,
  },
  {
    title: 'panel_pn',
    align: 'center',
    dataIndex: 'PANEL_PN',
    width:80,
    customCell: customCellRender,    
  },
  {
    title: 'owner',
    align: 'center',
    dataIndex: 'OWNER',
    width:40,
    customCell: customCellRender,
  },
  {
    title: 'lcd_grade',
    align: 'center',
    dataIndex: 'LCD_GRADE',
    width:50,
    customCell: customCellRender,
  },
  {
    title: 'cassette in date',
    align: 'center',
    dataIndex: 'TRANS_DATE',
    width:95,
    customCell: customCellRender,
  },
  {
    title: 'Cst_qty',
    align: 'center',
    dataIndex: 'CST_QTY',
    width:50,
    customCell: customCellRender,
  },
  {
    title: 'Cst_id',
    align: 'center',
    dataIndex: 'CST_ID',
    width:60,
    customCell: customCellRender,
  },
  {
    title: 'stay hours',
    align: 'center',
    dataIndex: 'STAY_HOUR',
    width:60,
  }, 
  {
    title: 'lcm_exp_no',
    align: 'center',
    dataIndex: 'LCM_EXP_NO',
    width:70,
  },
  {
    title: 'lcm_exp_name',
    align: 'center',
    dataIndex: 'LCM_EXP_NAME',
    width:70,
  },
  {
    title: 'Q_TIME_CODE',
    align: 'center',
    dataIndex: 'Q_TIME_CODE',
    width:70,
  },   
];

export const SecondLabel =[ 
  {
    title: 'fac_id',
    align: 'center',
    dataIndex: 'FAC_ID',
    width:50,
    customCell: (record,column: Object) => {
      let text = record['FAC_ID']
      let obj = {
        text,
        style: {},
        
      }
      obj['rowSpan'] = record?.['FAC_ID-span']?.['rowspan'] || 0
      return obj
    }
  },
  {
    title: 'floor',
    align: 'center',
    dataIndex: 'FLOOR',
    width:50,
    customCell: (record,column: Object) => {
      let text = record['FLOOR']
      let obj = {
        text,
        style: {},
        
      }
      obj['rowSpan'] = record?.['FLOOR-span']?.['rowspan'] || 0
      return obj
    }
  },
  {
    title: 'cst_id',
    align: 'center',
    dataIndex: 'CST_ID',
    width:70,
    customCell: (record,column: Object) => {
      let text = record['CST_ID']
      let obj = {
        text,
        style: {},
        
      }
      obj['rowSpan'] = record?.['CST_ID-span']?.['rowspan'] || 0
      return obj
    }
  },
  {
    title: 'pnl_id',
    align: 'center',
    dataIndex: 'PNL_ID',
    width:70,
  },  
  {
    title: 'lcd_prod_nbr',
    align: 'center',
    dataIndex: 'LCD_PROD_NBR',
    width:70,
  },  
  {
    title: 'lcd_grade',
    align: 'center',
    dataIndex: 'LCD_GRADE',
    width:50,
  },
  {
    title: 'oper',
    align: 'center',
    dataIndex: 'OPER',
    width:50,
  },
  {
    title: 'last_trans_date',
    align: 'center',
    dataIndex: 'LAST_TRANS_DT',
    width:90,
  },
  {
    title: 'stay_hours',
    align: 'center',
    dataIndex: 'STAY_HOURS',
    width:90,
  },
];







