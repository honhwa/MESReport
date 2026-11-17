import type { TableColumn } from '@/components/core/dynamic-table';
// import { Avatar, Space, Tag } from 'ant-design-vue';

export type TableListItem = API.DayliMoveThItem;
export type TableColumnItem = TableColumn<TableListItem>;
export const baseColumns: TableColumnItem[] = [

  {
    title: 'Operation',//label
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
    title: 'Work Oder',
    align: 'center',
    dataIndex: 'WO_NBR',
    className: "v-center",
  },
  {
    title: 'Size',
    align: 'center',
    dataIndex: 'SIZE1',
    className: "v-center",

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
    align: 'center',
    dataIndex: 'LCM_OWNER',
    className: "v-center",

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
    align: 'center',
    dataIndex: 'PROD_NBR',
    className: "v-center",
  }
];
