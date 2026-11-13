import type { TableColumn } from '@/components/core/dynamic-table';
// import { Avatar, Space, Tag } from 'ant-design-vue';

export type TableListItem = API.DayliMoveThItem;
export type TableColumnItem = TableColumn<TableListItem>;

export const baseColumns: TableColumnItem[] = [
  {
    title: 'Panel ID',//label
    width:90,
    align: 'center',
    dataIndex: 'PNL_ID',//prop屬性
    fixed: 'left',
    className:"v-center",
    customCell: (record) => {
      let text = record['PNL_ID']
      let obj = {
        text,
        style: {}
      }
      obj['rowSpan'] = record?.['PNL_ID-span']?.['rowspan'] || 0
      return obj
    }
  },
  {
    title: '時間',
    width:150,
    align: 'center',
    dataIndex: 'TRANS_DATE',
    customCell: (record) => {
      let text = record['TRANS_DATE']
      let obj = {
        text,
        style: {}
      }
      obj['rowSpan'] = record?.['TRANS_DATE-span']?.['rowspan'] || 0
      return obj
    }
  },  
  {
    title: 'Prod ID',
    width:100,
    align: 'center',
    dataIndex: 'PROD_NBR',
    customCell: (record) => {
      let text = record['PROD_NBR']
      let obj = {
        text,
        style: {}
      }
      obj['rowSpan'] = record?.['PROD_NBR-span']?.['rowspan'] || 0
      return obj
    }
  },
  {
    title: '設備編號',
    align: 'center',
    dataIndex: 'EQUIP_NBR',
    width:65,
  },  
  {
    title: 'BL料號',
    width:90,
    align: 'center',
    dataIndex: 'MAT_ID',
  },
  {
    title: 'BL批號',
    width:150,
    align: 'center',
    dataIndex: 'LOT_NUMBER',
  },
  {
    title: '人員工號',
    width:60,
    align: 'center',
    dataIndex: 'OPERATORS',
  },
  {
    title: '物料類型',
    width:50,
    align: 'center',
    dataIndex: 'MAT_TYPE',
  }, 
  {
    title: 'Light-Bar類型',
    width:70,
    align: 'center',
    dataIndex: 'BL_MAT_TYPE',
  }, 
  {
    title: 'Light-Bar料號',
    width:90,
    align: 'center',
    dataIndex: 'BL_MAT_ID',
  },  
  {
    title: 'LB人員工號',
    width:60,
    align: 'center',
    dataIndex: 'BL_OPERATOR',
  },
];

// export let SpanColumns =[ 
//   {
//     title: '設備編號',
//     align: 'center',
//     children: [
//           {
//             title: '設備編號',
//             align: 'center',
//             dataIndex: 'EQUIP_NBR',
//             width:70,
//           }            
//         ]
//   },
//   {
//     title: 'LCM',
//     align: 'center',
//     children: [
//           {
//             title: 'BL料號',
//             align: 'center',
//             dataIndex: 'MAT_ID',
//             width:70,
//           }, 
//           {
//             title: 'BL批號',
//             align: 'center',
//             dataIndex: 'LOT_NUMBER',
//             width:120,
//           },           
//         ]
//   },
     
// ];





