// import type { TableColumn } from '@/components/core/dynamic-table';
import router from '@/router';
import { AddCommonPageParams } from '@/store/module/commonpage';
import {TableCustomCell} from '@/pages/ReportPortal/utils/SHcommon'
import { ReportCommon } from '@/store/module/ReportCommon';

// import { Avatar, Space, Tag } from 'ant-design-vue';





export const DetailLabel = [
  'operator',
  'equip_nbr',
  'prod_nbr',
  'pnl_id',
  'trans_date',

]
const DetailProps = [
  'OPERATORS',
  'EQUIP_NBR',
  'PROD_NBR',
  'PNL_ID',
  'TRANS_DATE',

]
const DetailWidth = [
  30,30,30,30,50
]



export let MfgOperatorNewDetailColumns = TableCustomCell(DetailLabel, DetailProps,[],DetailWidth)

