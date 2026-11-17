<template>
    <div class="app-container">
        <UpExcel :on-success="handleSuccess" :before-upload="beforeUpload" />
        <Table
            :data="tableData"
            :dataSource="tableData"
            :columns="tableHeader"
            :scroll="tableHeight"
            :pagination="true"
        >
        </Table>
    </div>
</template>

<script lang="ts">
import { message, Table } from 'ant-design-vue';
import { defineComponent, reactive, toRefs,ref } from 'vue';
//@ts-ignore
import UpExcel from './index.vue';
export default defineComponent({
    components: {
        UpExcel,
        Table
    },
    setup() {
        const tableHeight = ref({  y: document.body.clientHeight - 430 });
        const dataMap = reactive({
            tableData: [],
            tableHeader: Array<Object>(),
            beforeUpload: (file: File) => {
                const isLt1M = file.size / 1024 / 1024 < 1;
                if (isLt1M) {
                    return true;
                }
                message.warning('Please do not upload files larger than 1m in size.');
                return false;
            },

            handleSuccess: ({ results, header }: { results: any; header: string[] }) => {
                dataMap.tableData = results;
                console.log(results);
                
                dataMap.tableHeader = header.map((item) => {
                    return {
                        dataIndex: item,
                        title: item,
                        key: item
                    };
                });
            }
        });
        return { ...toRefs(dataMap),tableHeight };
    }
});
</script>
