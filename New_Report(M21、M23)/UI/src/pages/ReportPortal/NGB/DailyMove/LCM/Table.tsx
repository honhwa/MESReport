import type { TableColumn } from '@/components/core/dynamic-table';
// import { Avatar, Space, Tag } from 'ant-design-vue';

export type TableListItem = API.DayliMoveThItem;
export type TableColumnItem = TableColumn<TableListItem>;
export const baseColumns: TableColumnItem[] = [

  {
    title: 'Operation',//label
    width:90,
    align: 'center',
    dataIndex: 'WORK_CTR',//prop屬性
    className: "v-center",
    customCell: (record) => {
      let text = record['WORK_CTR']
      let obj = {
        text,
        style: {}
      }
      obj['rowSpan'] = record?.['WORK_CTR-span']?.['rowspan'] || 0
      return obj
    }
  },
  {
    title: 'Size',
    width:50,
    align: 'center',
    dataIndex: 'SIZE1',
    className: "v-center",
    // customRender: ({ text, record }) => {
    //   const obj = {
    //     children: text,
    //     props: {} as any
    //   }
    //   // record 是当前行的数据
    //   obj.props.rowSpan = record?.['size1-span']?.['rowspan'] || 0
    //   obj.props.colSpan = 1
    //   return obj
    // }
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
    title: 'Owner',
    width:70,
    align: 'center',
    dataIndex: 'LCM_OWNER',
    className: "v-center",
    // customRender: ({ text, record }) => {
    //   const obj = {
    //     children: text,
    //     props: {} as any
    //   }
    //   // record 是当前行的数据
    //   obj.props.rowSpan = record?.['lcm_owner-span']?.['rowspan'] || 0
    //   return obj
    // }
    customCell: (record) => {
      let text = record['LCM_OWNER']
      let obj = {
        text,
        style: {}
      }
      obj['rowSpan'] = record?.['LCM_OWNER-span']?.['rowspan'] || 0
      return obj
    }
  },
  {
    title: 'Product',
    width:150,
    align: 'center',
    dataIndex: 'PROD_NBR',
    className: "v-center",
    
  }, 
  {
    title: 'Work Oder',
    align: 'center',
    dataIndex: 'WO_NBR',
    className: "v-center",
  },
  
  {
    title: 'Customer',
    align: 'center',
    dataIndex: 'CUST_GRP',
    className: 'v-center',
    // customRender: ({ text }) => {
    //   const obj = {
    //     children: text,
    //     props: {} as any,
    //     style: { 'background-color': '#80ff80' },
    //   }
    //   // record 是当前行的数据
    //   // obj.props.rowSpan = record?.['lcm_owner-span']?.['rowspan']||0
    //   if (text == '小计' || text == '合计') obj.props.colSpan = 3
    //   return obj
    // },
    customCell: (record) => {
      let text = record['CUST_GRP']
      let obj = {
        text,
        style: {}
      }
      if (text == '小计' || text == '合计') obj['colSpan'] = 3
      return obj
    },
  },
    
  {
      title: '需求單號',
      align: 'center',
      dataIndex: 'SO_NBR',
      className: 'v-center',
      
  }


  
  // {
  //   title: '总计',
  //   align: 'center',
  //   dataIndex: 'Total',
  //   fixed:'right',
  //   customRender: ({ record }) => {
  //     return <span>{record['QTY']}</span>;
  //   },
  // },
];
