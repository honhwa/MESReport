import type { TableColumn } from '@/components/core/dynamic-table';
export interface PageShowHidden{
    ShowMenu:boolean;
    TableSpan:number;
}

export type VexTableColumn={
    children?:TableColumn;
    field:string;
    title:string;
    key?:string;
    fixed?:string;
    className?:string;
    dataIndex?:string;
}