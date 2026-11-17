<template>
    <div>
        <template v-for="item in columns">
            <vxe-colgroup v-if="item.children" :title="item.title" :key="item.dataIndex">
                <vxe-column
                    v-for="item1 in item.children"
                    :field="item1.dataIndex"
                    :title="item1.title"
                    :key="item1.dataIndex"
                    width="120"
                ></vxe-column>
            </vxe-colgroup>
            <vxe-column v-else :field="item.dataIndex" :title="item.title" width="120" :key="item.title"></vxe-column>
        </template>
    </div>
</template>

<script  lang="ts" setup>
import { VxeColumn, VxeColgroup } from 'vxe-table/types/index';
</script>
<script lang="ts">
export default {
    name: 'VxeTableGroup',
    props: {
        columns: Object,
        field: String,
        title: String
    }
};
</script>
