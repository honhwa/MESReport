import router from '@/router';
import { AddCommonPageParams } from '@/store/module/commonpage';
const UseParamsAdd = AddCommonPageParams();

export const baseColumns = [
    {
        title: 'Product',
        align: 'center',
        dataIndex: 'PROD_NBR',
        className: 'pro_auto-center'
    },
    {
        title: 'LCM Grade',
        align: 'center',
        dataIndex: 'LCM_GRADE',
        className: 'pro_auto-center'
    },
    {
        title: 'Non Hold',
        align: 'center',
        dataIndex: 'Non Hold',
        children: [],
        className: 'pro_auto-center'
    },
    {
        title: 'Hold',
        align: 'center',
        dataIndex: 'Hold',
        children: [],
        className: 'pro_auto-center'
    },
    {
        title: '合計',
        align: 'center',
        dataIndex: '合計',
        className: 'pro_auto-center YellowGreen'
    }
];

export const detailColumns = [
  {
      title: 'Product',
      align: 'center',
      dataIndex: 'PROD_NBR',
      className: 'pro_auto-center'
  },
  {
      title: 'Revision',
      align: 'center',
      dataIndex: 'VER',
      className: 'pro_auto-center'
  },
  {
      title: 'Panel ID',
      align: 'center',
      dataIndex: 'PNL_ID',
      className: 'pro_auto-center'
  },
  {
      title: 'PC Cust',
      align: 'center',
      dataIndex: 'PC',
      className: 'pro_auto-center'
  }
];