// import type { TableColumn } from '@/components/core/dynamic-table';
import router from '@/router';
import { AddCommonPageParams } from '@/store/module/commonpage';
import {TableCustomCell} from '@/pages/ReportPortal/utils/SHcommon'

// import { Avatar, Space, Tag } from 'ant-design-vue';
import { AddCommonPageParamsWithOut } from '@/store/module/commonpage';
const useCommonParams = AddCommonPageParams();


export const DetailLabel = [
  'operator',
  'equip_nbr',
  'prod_nbr',
  'pnl_id',
  'trans_date',
  'trans_type',
  'qty'
]
const DetailProps = [
  'OPERATORS',
  'EQUIP_NBR',
  'PROD_NBR',
  'PNL_ID',
  'TRANS_DATE',
  'TRANS_TYPE',
  'QTY',
]
const DetailWidth = [
  30,30,30,30,50,20,15
]



export let MfgOperatorDetailColumns = TableCustomCell(DetailLabel, DetailProps,[],DetailWidth)

