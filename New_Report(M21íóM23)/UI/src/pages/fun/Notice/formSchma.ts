// import MultipleCascader from './components/multiple-cascader/index.vue';
import type { FormSchema } from '@/components/core/schema-form/';

export const explainSchemas: FormSchema[] = [
    {
        field: 'NOTICETYPE',
        component: 'Select',
        label: '公告类型',
        componentProps: {
            options: [{ label: '新增', value: '新增' },
            { label: '系统功能', value: '系统功能' },
            { label: '異常修复', value: '異常修复' },]
        }
    },
    {
        field: 'NOTICENUM',
        component: 'Input',
        label: '排序號',
        vShow: false,
    },
    {
        field: 'NOTICETITLE',
        component: 'Input',
        label: '标题',
    }, {
        field: 'NOTICEPEOPLE',
        component: 'Input',
        label: '发布人',
        vShow: false,
    }, {
        field: 'NOTICETIME',
        component: 'Input',
        label: '发布时间',
        vShow: false,
    },
    {
        field: 'NOTICEDATA',
        component: 'Tinymce',
        label: '富文本编辑框',
        defaultValue: 'defaultValue',
        componentProps: ({ formModel, field }) => {
            return {
                showDownLoadList:false,
                showUpLoadList:true,
                onChange: (value: string) => {
                    formModel[field] = value;
                }
            };
        }
    }
];