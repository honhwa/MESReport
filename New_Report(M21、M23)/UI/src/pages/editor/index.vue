<template>
    <div>
        <Alert message="富文本组件示例" type="info" show-icon>
            <template #description> </template>
        </Alert>
        <Card title="demo" style="margin-top: 20px">
        <!-- <Tinymce v-model="value" width="100%" @change="handleChange" /> -->
        <SchemaForm @submit="confirm"> </SchemaForm>
        <!-- <div v-html="TestEditor"></div> -->
        <div>{{TestEditor}}</div>
        </Card>
    </div>
</template>
<script lang="ts" setup>
import { h, ref } from 'vue';
import { Alert, Card } from 'ant-design-vue';
// import { Tinymce } from '@/components/Tinymce/index';
import type { FormSchema } from '@/components/core/schema-form/';
import { useForm } from '@/components/core/schema-form';

const schemas: FormSchema[] = [
    {
        field: 'title',
        component: 'Input',
        label: 'title',
        defaultValue: 'defaultValue',
        rules: [{ required: true }]
    },
    {
        field: 'tinymce',
        component: 'Tinymce',
        label: 'tinymce',
        defaultValue: 'defaultValue',
        componentProps: ({ formModel, field }) => {
            return {
                onChange: (value: string) => {
                    formModel[field] = value;
                }
            };
        }
    }
];
const [SchemaForm, dynamicFormRef] = useForm({
    // labelWidth: 120,
    schemas,
    actionColOptions: {
        span: 24
    },
    layout: 'vertical'
});
let TestEditor = ref("")
// const value = ref('hello world!');
function confirm(values) {
    TestEditor.value=values.tinymce
    console.log(values);
}
</script>
