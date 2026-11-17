<template>
    <Col v-if="showActionButtonGroup" v-bind="actionColOpt">
        <div style="width: 100%" :style="{ textAlign: actionColOpt.style.textAlign }">
            <Form.Item>
                <slot name="resetBefore"></slot>
                <Button
                    v-if="showResetButton"
                    type="default"
                    class="mr-2"
                    v-bind="getResetBtnOptions"
                    @click="resetFields"
                >
                    {{ getResetBtnOptions.text }}
                </Button>
                <slot name="submitBefore"></slot>

                <Button
                    v-if="showSubmitButton"
                    type="primary"
                    class="mr-2"
                    id="selectvalue"
                    v-bind="getSubmitBtnOptions"
                    @click="submit"
                >
                    {{ getSubmitBtnOptions.text }}
                </Button>
                <slot name="uploadBefore"></slot>
                <Button
                    v-if="showUploadButton"
                    type="success"
                    class="mr-2"
                    v-bind="getUploadBtnOptions"
                    @click="upload"
                >
                    <!-- @click="$router.push('/FSUI/WaterMark')" -->

                    {{ getUploadBtnOptions.text }}
                </Button>
                <slot name="customBefore"></slot>
                <Button
                    v-if="showcustomButton"
                    type="success"
                    class="mr-2"
                    v-bind="getcustomBtnOptions"
                    @click="custom"
                >
                    <!-- @click="$router.push('/FSUI/WaterMark')" -->

                    {{ getcustomBtnOptions.text }}
                </Button>
                <slot name="advanceBefore"></slot>
                <Button v-if="showAdvancedButton && !hideAdvanceBtn" type="link" size="small" @click="toggleAdvanced">
                    {{ isAdvanced ? '收起' : '展开' }}
                    <BasicArrow class="ml-1" :expand="!isAdvanced" />
                </Button>
                <slot name="advanceAfter"></slot>
            </Form.Item>
        </div>
    </Col>
</template>
<script lang="ts" setup>
import { computed, PropType } from 'vue';
import { Form, Col } from 'ant-design-vue';
import { useFormContext } from '../hooks/useFormContext';
import type { ColEx } from '../types/component';
import { Button, ButtonProps } from '@/components/basic/button';
import { BasicArrow } from '@/components/basic/basic-arrow';

type ButtonOptions = Partial<ButtonProps> & { text: string };

// defineOptions({
//   name: 'FormAction',
//   inheritAttrs: false,
// });

const emit = defineEmits(['toggle-advanced']);

const props = defineProps({
    showActionButtonGroup: {
        type: Boolean,
        default: true
    },
    showTextPosition: {
        type: String,
        default: 'center'
    },
    showResetButton: {
        type: Boolean,
        default: false
    },
    showUploadButton: {
        type: Boolean,
        default: false
    },
    showcustomButton: {
        type: Boolean,
        default: false
    },
    showSubmitButton: {
        type: Boolean,
        default: true
    },
    showAdvancedButton: {
        type: Boolean,
        default: true
    },
    resetButtonOptions: {
        type: Object as PropType<ButtonOptions>,
        default: () => ({})
    },
    submitButtonOptions: {
        type: Object as PropType<ButtonOptions>,
        default: () => ({})
    },
    uploadButtonOptions: {
        type: Object as PropType<ButtonOptions>,
        default: () => ({})
    },
    customButtonOptions: {
        type: Object as PropType<ButtonOptions>,
        default: () => ({})
    },
    actionColOptions: {
        type: Object as PropType<Partial<ColEx>>,
        default: () => ({})
    },
    actionSpan: {
        type: Number,
        default: 6
    },
    isAdvanced: Boolean,
    hideAdvanceBtn: Boolean
});

const { resetFields, submit ,upload, custom} = useFormContext();


const actionColOpt = computed(() => {
    const { showAdvancedButton, actionSpan: span, actionColOptions, showTextPosition } = props;
    const actionSpan = 24 - span;
    const advancedSpanObj = showAdvancedButton ? { span: actionSpan < 6 ? 24 : actionSpan } : {};
    const actionColOpt: Partial<ColEx> = {
        style: { textAlign: showTextPosition },
        span: showAdvancedButton ? 6 : 4,
        ...advancedSpanObj,
        ...actionColOptions
    };
    return actionColOpt;
});

const getResetBtnOptions = computed((): ButtonOptions => {
    return Object.assign(
        {
            text: '重置'
        },
        props.resetButtonOptions
    );
});

const getSubmitBtnOptions = computed(() => {
    return Object.assign(
        {
            text: '查询'
        },
        props.submitButtonOptions
    );
});

const getUploadBtnOptions = computed((): ButtonOptions => {
    return Object.assign(
        {
            text: '上传'
        },
        props.uploadButtonOptions
    );
});

const getcustomBtnOptions = computed((): ButtonOptions => {
    return Object.assign(
        {
            text: 'UPDATA ALL'
        },
        props.customButtonOptions
    );
});

function toggleAdvanced() {
    emit('toggle-advanced', props.isAdvanced);
}
</script>
