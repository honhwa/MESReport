import type { TableColumnType } from 'ant-design-vue';
import type { VNode } from 'vue';
import type { FormSchema } from '@/components/core/schema-form';
import type { ActionItem } from './tableAction';
import { TableColumnItem } from '@/pages/permission/rolelist/components/roleuser/columns';

export type ColumnParams<T = any> = {
  record: T;
  text: string;
  index: number;
  column: TableColumnItem;
};

/**
 * 表格属性
 */
export interface TableColumn {
  title: string;
  // dataIndex: keyof T | '$action' | 'Total';
  field:string;
  key?:string;
  fixed?:string
  width?: number;
  className?:string;
  dataIndex?:string;
  /** 传递给 Form.Item 的配置,可以配置 rules */
}
