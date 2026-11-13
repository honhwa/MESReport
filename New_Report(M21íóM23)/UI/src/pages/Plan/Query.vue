<template>
    <DynamicTable header-title="排程管理" show-index :columns="columns" :data-all="PageSetting.List"
        :data-source="PageSetting.List">
    </DynamicTable>
    <a-modal v-model:visible="visible" title="排程修改" :footer="null" class="test" :width="1000">
        <add-scheduling ref="editscheduling" :SchedulingParams="recordprops" @closemodal="closemodal"></add-scheduling>
    </a-modal>
</template>

<script lang="tsx">
export default {
    name: 'Query'
};
</script>
<script setup lang="tsx">
import {
    PlusOutlined,
    ExclamationCircleOutlined
} from '@ant-design/icons-vue';
import { FormInstance, Modal, message, Tag, Switch } from 'ant-design-vue';

import { useTable } from '@/components/core/dynamic-table';
import { reactive, ref, defineExpose } from 'vue';
import AddScheduling from './Edit.vue';
import { GetCommon } from '@/api/pages/common';
import { onMounted } from 'vue';
import { log } from 'console';

const editscheduling = ref<FormInstance>();
let visible = ref(false);
const [DynamicTable, dynamicTableInstance] = useTable();

let recordprops = ref({});


let PageSetting = reactive({
    moreData: [] as any,
    List: [




    ] as any,
    Columns: [] as any,
    TopTitle: '',
});

let baseColumns = [
    {
        title: '報表名稱',
        dataIndex: 'MT_COLUMN',
        fixed: 'left',
        className: 'v-center',
    },
    {
        title: '任務名稱',
        dataIndex: 'ROUTE_NAME',
    },
    {
        title: 'Mapp會議室編號',
        dataIndex: 'MAPP_ID',
    },
    {
        title: '開始時間',
        dataIndex: 'START_TIME',
    },
    {
        title: '停止時間',
        dataIndex: 'ENDTIME',
    },
    {
        title: '下一次运行时间',
        dataIndex: 'NEXT_TIME',
    },
    {
        title: 'Cron表達式',
        dataIndex: 'CRON',
    },
    {
        title: '狀態',
        dataIndex: 'ISSTART',
        customRender: ({ text, record, index, column }) => {
            return (<Tag color={text ? 'green' : 'error'}>{text ? '運行' : '停止'}</Tag>)
        }
    },
    {
        title: '是否Mapp通知',
        dataIndex: 'ISNOTIFY',
        customRender: ({ text, record, index, column }) => {
            return (<Switch checked={text} checked-children="开" un-checked-children="关" onClick={async (checked) => {
                let { api01: { code, message: Message } } = await GetCommon({
                    apiName: 'Scheduling_Action',
                    routeId: record["ROUTE_ID"],
                    routeName: record["ROUTE_NAME"],
                    type: 'isnotify',
                    isnotify: String(checked).substring(0, 1).toUpperCase() + String(checked).substring(1)
                })
                if (code == 200) {
                    message.success(Message);
                    await gettableData();
                    dynamicTableInstance.reload();
                } else {
                    message.error(Message)
                }
            }}></Switch>)
        }
    },
    {
        title: '備注',
        dataIndex: 'BACKUP',
        ellipsis: true,
    },
    {
        title: '創建人',
        dataIndex: 'OWNER',
    },

]


// 查詢表格數據
async function gettableData() {
    let { api01: { Table: Data } } = await GetCommon({
        apiName: 'Scheduling_Show',
        routeId: ""
    });
    Data.forEach(item => {
        item["ISSTART"] = item["ISSTART"] == "True" ? Boolean(true) : Boolean(false);
        item["ISNOTIFY"] = item["ISNOTIFY"] == "True" ? Boolean(true) : Boolean(false);
        if (item['FIELDOPTIONS'] && typeof (item['FIELDOPTIONS']) == 'string') {
            item['FIELDOPTIONS'] = JSON.parse(item["FIELDOPTIONS"])
        }
        if (item["CRON"]) {
            item["CRON"] = item["CRON"].replace(/\+/g, '?');
        }
    })
    PageSetting.List = Data;
    console.log("输出", PageSetting.List);

}
onMounted(async () => {
    await gettableData();
})


/**
 * @description 表格删除行
 */
const delRowConfirm = async (record) => {
    Modal.confirm({
        title: '最后确认！！是否删除此排程?',
        icon: <ExclamationCircleOutlined />,
        centered: true,
        onOk: async () => {
            let { api01: { code, message: Message } } = await GetCommon({
                apiName: 'Scheduling_Action',
                routeId: record["ROUTE_ID"],
                routeName: record["ROUTE_NAME"],
                type: 'delete'
            })
            if (code == 200) {
                message.success(Message);
                await gettableData();
                dynamicTableInstance.reload();
            } else {
                message.error(Message)
            }

        }
    });
};
async function closemodal() {
    visible.value = false;
    await gettableData();
}
const columns = [
    ...baseColumns,
    {
        title: '操作',
        width: 150,
        dataIndex: '$action',
        align: 'center',
        fixed: 'right',
        actions: ({ record }) => [
            {
                label: '编辑',
                comtype: 'a-button',
                onClick: () => {
                    visible.value = true;
                    recordprops.value = { ...record };
                }
            },
            {
                label: '删除',
                comtype: 'a-button',
                popConfirm: {
                    title: '你确定要删除吗？',
                    onConfirm: () => delRowConfirm(record)
                }
            },
            {
                label: '运行',
                comtype: 'a-button',
                onClick: async () => {
                    let { api01: { code, message: Message } } = await GetCommon({
                        apiName: 'Scheduling_Action',
                        routeId: record["ROUTE_ID"],
                        routeName: record["ROUTE_NAME"],
                        type: 'start'
                    })
                    if (code == 200) {
                        message.success(Message);
                        await gettableData();
                        dynamicTableInstance.reload();
                    } else {
                        message.error(Message)
                    }

                }
            },
            {
                label: '停止',
                comtype: 'a-button',
                onClick: async () => {
                    let { api01: { code, message: Message } } = await GetCommon({
                        apiName: 'Scheduling_Action',
                        routeId: record["ROUTE_ID"],
                        routeName: record["ROUTE_NAME"],
                        type: 'end'
                    })
                    if (code == 200) {
                        message.success(Message);
                        await gettableData();
                        dynamicTableInstance.reload();
                    } else {
                        message.error(Message)
                    }
                }
            },

        ]
    }
];





defineExpose({
    gettableData
})
</script>

<style scoped lang="less">
:deep(.ant-modal-body) {
    width: 60vh !important;
}
</style>
