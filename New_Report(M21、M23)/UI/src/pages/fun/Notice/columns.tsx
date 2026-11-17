import type { TableColumn } from '@/components/core/dynamic-table';
import {formatToDateTime} from '@/utils/dateUtil'
import { ref } from 'vue';
import { useModal } from '@/hooks/useModal/';

export type TableListItem = API.MenuListResultItem;
export type TableColumnItem = TableColumn<TableListItem>;
export let RoleList = ref<any[]>([]);
const [fnModal] = useModal();


export const baseColumns: TableColumnItem[] = [
    {
        title: '公告类型',
        dataIndex: 'NOTICETYPE'
    },
    
    {
        title: '发布人',
        dataIndex: 'NOTICEPEOPLE'
    },
    {
        title: '发布时间',
        dataIndex: 'NOTICETIME',
        bodyCell:({record})=><>{formatToDateTime(record['NOTICETIME'])}</>
    },
    {
        title:'内容',
        dataIndex:'NOTICEDATA',
        hideInTable:true
    }
];
export const handleOpenHookModal = (record) => {
    fnModal.setProps({
        title: record['NOTICETITLE'],
        width: 1200
    });
    fnModal.show({
        content: record['NOTICEDATA']
    });
};
