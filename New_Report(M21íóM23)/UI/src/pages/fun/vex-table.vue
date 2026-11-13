<template>
    <div>
        <a-button @click="change">change data</a-button>
        <VexTable :columns="columns" :tableData="mockList1" :height="700" exportFileName="测试" :data="mockList1"></VexTable>
    </div>
</template>

<script lang="ts" setup>
import  VexTable  from '@/components/core/vex-virtual-table/src/vex-virtual-table.vue';
import { reactive, ref } from 'vue';

const mockList1 = ref<any[]>([]);
let count=ref(10)
const change = () => {
    count.value+=10;
    
    let a: any[] = [];
    for (let index = 0; index < count.value; index++) {
        a.push({
            name: 'Test' + index,
            role: 'Developer',
            sex: '男'
        });
    }
    mockList1.value = a;
};
const columns = [
    { dataIndex: 'name', title: 'name' },
    { dataIndex: 'role', title: 'role' },
    { dataIndex: 'sex', title: 'sex' }
];

// for (let index = 0; index < 20; index++) {
//     mockList1.push({
//         name: 'Test' + index,
//         role: 'Developer',
//         sex: '男'
//     });
// }
</script>
