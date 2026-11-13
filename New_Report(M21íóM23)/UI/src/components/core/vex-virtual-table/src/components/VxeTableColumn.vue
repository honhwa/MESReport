<template>
    <div>
        <vxe-table-column
            v-for="column in columns"
            :field="column.dataIndex"
            :title="column.title"
            :children="column.children"
        ></vxe-table-column>
    </div>
</template>

<script>
export default {
    name: 'VxeTableColumn',
    props: {
        columns: Array
    }
};
</script>
