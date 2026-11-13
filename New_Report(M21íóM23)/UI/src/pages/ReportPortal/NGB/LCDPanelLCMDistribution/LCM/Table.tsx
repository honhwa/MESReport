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
         useReportCommon.AddFirstTableParams(`LCDPanelLCMDistribution-${useCommonParams.Fab}`,{ ...record, ...column['__originColumn__'] }) 
          router.push({ name: `LCDPanelLCMDistributionDetail-${useCommonParams.Fab}`});
      },
      style: {
          cursor: 'pointer'
      }
  };
};

const colorCellRender = (record: Object, rowIndex: number, column: Object) => {  
   if(record[column["dataIndex"]]>0){
    return{style:{'color': 'rgba(255, 0,0)'}}
   }
};


export const FirstSumLabel =[ 
  {
    title: 'LCD等級',
    align: 'center',
    dataIndex: 'LCD_GRADE',
    width:70,
  },
  {
    title: '理論 plan',
    align: 'center',
    children: [
          {
            title: 'Total',
            align: 'center',
            dataIndex: 'TPLAN_TOTAL',
            width:70,
          },
          {
            title: 'BEOL1',
            align: 'center',
            dataIndex: 'TPLAN_BEOL1',
            width:70,
          },
          {
            title: 'BEOL2',
            align: 'center',
            dataIndex: 'TPLAN_BEOL2',
            width:70,
          },
          {
            title: 'TN',
            align: 'center',
            dataIndex: 'TPLAN_TN',
            width:70,
          },            
        ]
  },
  {
    title: 'Act',
    align: 'center',
    children: [
      {
        title: 'Total',
        align: 'center',
        dataIndex: 'ACT_TOTAL',
        width:70,
      },
      {
        title: 'BEOL1',
        align: 'center',
        dataIndex: 'ACT_BEOL1',
        width:70,
      },
      {
        title: 'BEOL2',
        align: 'center',
        dataIndex: 'ACT_BEOL2',
        width:70,
      },
      {
        title: 'TN',
        align: 'center',
        dataIndex: 'ACT_TN',
        width:70,
      },           
    ]
  },
  {
    title: 'Diff',
    align: 'center',
    children: [
      {
        title: 'Total',
        align: 'center',
        dataIndex: 'DIFF_TOTAL',
        width:70,
        //customCell: colorCellRender,
      },
      {
        title: 'BEOL1',
        align: 'center',
        dataIndex: 'DIFF_BEOL1',
        width:70,
        //customCell: colorCellRender,
      },
      {
        title: 'BEOL2',
        align: 'center',
        dataIndex: 'DIFF_BEOL2',
        width:70,
        //customCell: colorCellRender,
      },
      {
        title: 'TN',
        align: 'center',
        dataIndex: 'DIFF_TN',
        width:70,
        //customCell: colorCellRender,
      },           
    ]
  },
  {
    title: '實際LOSS片數',
    align: 'center',
    dataIndex: 'LOSS_NBR',
    width:70,
    customCell: colorCellRender,
  },
     
];

export const FirstSum2Label =[ 
  {
    title: 'Prod ID*',
    align: 'center',
    dataIndex: 'PROD_NBR',
    width:120,
    customCell: customCellRender,
  },
  {
    title: 'Prod Type*',
    align: 'center',
    dataIndex: 'PROD_TYPE',
    width:50,
    customCell: customCellRender,
  },
  {
    title: 'LCD Grade*',
    align: 'center',
    dataIndex: 'LCD_GRADE',
    width:100,
    customCell: customCellRender,
  },
  {
    title: '混貨比例',
    align: 'center',
    dataIndex: 'MIXRATE',
    width:60,
  },
  {
    title: '理論 plan',
    align: 'center',
    children: [
          {
            title: 'Total',
            align: 'center',
            dataIndex: 'TPLAN_TOTAL',
            width:60,
          },
          {
            title: 'BEOL1',
            align: 'center',
            dataIndex: 'TPLAN_BEOL1',
            width:60,
          },
          {
            title: 'BEOL2',
            align: 'center',
            dataIndex: 'TPLAN_BEOL2',
            width:60,
          },
          {
            title: 'TN',
            align: 'center',
            dataIndex: 'TPLAN_TN',
            width:60,
          },            
        ]
  },
  {
    title: 'Act',
    align: 'center',
    children: [
      {
        title: 'Total*',
        align: 'center',
        dataIndex: 'ACT_TOTAL',
        width:60,
        customCell: customCellRender,
      },
      {
        title: 'BEOL1*',
        align: 'center',
        dataIndex: 'ACT_BEOL1',
        width:60,
        customCell: customCellRender,
      },
      {
        title: 'BEOL2*',
        align: 'center',
        dataIndex: 'ACT_BEOL2',
        width:60,
        customCell: customCellRender,
      },
      {
        title: 'TN*',
        align: 'center',
        dataIndex: 'ACT_TN',
        width:70,
        customCell: customCellRender,
      },           
    ]
  },
  {
    title: 'Diff',
    align: 'center',
    children: [
      {
        title: 'Total',
        align: 'center',
        dataIndex: 'DIFF_TOTAL',
        width:60,
        //customCell: colorCellRender,
      },
      {
        title: 'BEOL1',
        align: 'center',
        dataIndex: 'DIFF_BEOL1',
        width:60,
        //customCell: colorCellRender,
      },
      {
        title: 'BEOL2',
        align: 'center',
        dataIndex: 'DIFF_BEOL2',
        width:60,
        //customCell: colorCellRender,
      },
      {
        title: 'TN',
        align: 'center',
        dataIndex: 'DIFF_TN',
        width:60,
        //customCell: colorCellRender,
      },           
    ]
  },
  {
    title: '實際LOSS片數',
    align: 'center',
    dataIndex: 'LOSS_NBR',
    width:60,
    customCell: colorCellRender,
  },
  {
    title: '庫存',
    align: 'center',
    children: [
      {
        title: '當天庫存',
        align: 'center',
        dataIndex: 'TODAY_INVEN',
        width:60,
      },
      {
        title: 'LCM Bank',
        align: 'center',
        dataIndex: 'LCM_BANK_QTY',
        width:60,
      },
      {
        title: '半成品庫存',
        align: 'center',
        dataIndex: 'SEMI_BANK_QTY',
        width:70,
      },
      {
        title: 'LCD Bank',
        align: 'center',
        dataIndex: 'LCD_BANK_QTY',
        width:60,
      },           
    ]
  },
  
];







