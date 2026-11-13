<template>
    <a-form ref="formRef" name="dynamic_form_nest_item" :model="SchedulingParams" @finish="onFinish" layout="vertical"
        :label-col="{ span: 18 }" :wrapper-col="{ span: 22 }">
        <a-form-item name="TaskName" label="任務名稱" >
            <a-input v-model:value="SchedulingParams.ROUTE_NAME" style="width: 280px" allowClear autocomplete="off" />
        </a-form-item>
        <a-form-item name="Mapp_ID" label="Mapp會議室編號" >
            <a-input-number v-model:value="SchedulingParams.MAPP_ID" style="width: 180px" />
        </a-form-item>
        <a-form-item name="StartTime" label="開始時間">
            <a-date-picker :show-time="{ format: 'HH:mm' }" placeholder="Select Time"
                v-model:value="SchedulingParams.START_TIME" format="YYYY-MM-DD HH:mm" valueFormat="YYYY-MM-DD hh:mm" />
            <p>爲空表示不限制開始時間</p>
        </a-form-item>
        <a-form-item name="EndTime" label="停止時間">
            <a-date-picker :show-time="{ format: 'HH:mm' }" placeholder="Select Time"
                v-model:value="SchedulingParams.ENDTIME" format="YYYY-MM-DD HH:mm" valueFormat="YYYY-MM-DD hh:mm" />
            <p>爲空表示不限制停止時間</p>
            <a-form-item name="Cron" label="Cron表達式">
                <ul>
                        <li>
                            <a href="https://cron.qqe2.com/" target="_blank">cron在綫網站1</a>
                        </li>
                        <li>
                            <a href="https://www.pppet.net/" target="_blank">cron在綫網站2
                            </a>
                        </li>
                    </ul>
                <a-input v-model:value="SchedulingParams.CRON" style="width: 280px" allowClear autocomplete="off" />
            </a-form-item>
        </a-form-item>
        <a-space size="middle">
            <a-form-item name="不确定列" :label="SchedulingParams.FIELDOPTIONS.Options.ObservedColumnYlabel">
                <a-select v-model:value="SchedulingParams.CONDITION"
                    :options="SchedulingParams.FIELDOPTIONS.Options.fieldoptions" style="width: 180px"></a-select>
            </a-form-item>
            <a-form-item name="栏位" label="栏位">
                <a-select v-model:value="SchedulingParams.FIELDOPTIONS.Field0" :options="SchedulingParams.FIELDOPTIONS.Options.RelationalOperators
                    " style="width: 180px">
                </a-select>
            </a-form-item>
            <a-form-item name="conditionvalue1" label="值(between输入范例：1 and 2)">
                <a-input v-model:value="SchedulingParams.ADDONBEFOREVALUE" style="width: 260px">
                    <template #addonBefore>
                        <a-select v-model:value="SchedulingParams.FIELDOPTIONS.Options.addonBeforeValue"
                            style="width: 100px">
                            <a-select-option value=">">></a-select-option>
                            <a-select-option value=">=">>=</a-select-option>
                            <a-select-option value="=">=</a-select-option>
                            <a-select-option value="<">&lt;</a-select-option>
                            <a-select-option value="<=">&lt;=</a-select-option>
                            <a-select-option value="between">between</a-select-option>
                            <a-select-option value="not between">not between</a-select-option>
                        </a-select>
                    </template>
                </a-input>
            </a-form-item>
        </a-space>
        <a-form-item name="ExecuteImmediately" label="是否運行">
            <a-switch v-model:checked="SchedulingParams.ISSTART" />
        </a-form-item>
        <a-form-item name="Notify" label="是否Mapp通知">
            <a-switch v-model:checked="SchedulingParams.ISNOTIFY" />
        </a-form-item>
        <a-form-item name="Recipient" label="收件人(爲空時不發送郵件,輸入格式:逗號分割，可輸入工號)">
                <a-textarea v-model:value="SchedulingParams.EMAIL" placeholder="請輸入收件人郵箱" auto-size allowClear/>
            </a-form-item>
            <a-form-item name="Recipient" label="抄送(cc)">
                <a-textarea v-model:value="SchedulingParams.EMAIL_COPY" placeholder="請輸入收件人郵箱" auto-size allowClear />
            </a-form-item>
        <a-form-item name="Notes" label="備注">
            <a-textarea v-model:value="SchedulingParams.BACKUP" :rows="4"  placeholder="任務描述" show-count :maxlength="300" />
        </a-form-item>
        <a-form-item>
            <a-button type="primary" html-type="submit">提交</a-button>
            <a-button  @click="handleCancel">取消</a-button>
        </a-form-item>
    </a-form>
</template>

<script setup lang="ts">
import { reactive, ref, watch } from 'vue';
import { FormInstance, message } from 'ant-design-vue';
import { GetCommon } from '@/api/pages/common';



const formRef = ref<FormInstance>();

const props = defineProps({
    SchedulingParams: {
        type: Object,
        require: true,
        default: () => {
            return {}
        }
    },
})


// 提交
const onFinish = async (values) => {
    console.log('Received values of form:', values);
    console.log('SchedulingParams:', props.SchedulingParams);
    let baseparams = {
        apiName: 'Scheduling_Update',
        ROUTE_NAME: props.SchedulingParams.ROUTE_NAME,
        ROUTE_ID: props.SchedulingParams.ROUTE_ID,
        MAPP_ID: props.SchedulingParams.MAPP_ID,
        START_TIME: props.SchedulingParams.START_TIME ?? '',
        ENDTIME: props.SchedulingParams.ENDTIME ?? '',
        CRON: props.SchedulingParams.CRON,
        CONDITION: props.SchedulingParams.CONDITION,
        COLUMN1: props.SchedulingParams.FIELDOPTIONS.Field0,
        VALUE: props.SchedulingParams.FIELDOPTIONS.Options.addonBeforeValue + " " + props.SchedulingParams.ADDONBEFOREVALUE,
        ISSTART:String(props.SchedulingParams.ISSTART).substring(0,1).toUpperCase() + String(props.SchedulingParams.ISSTART).substring(1),
        ISNOTIFY: String(props.SchedulingParams.ISNOTIFY).substring(0,1).toUpperCase() + String(props.SchedulingParams.ISNOTIFY).substring(1),
        basecondition:{Field0:props.SchedulingParams.FIELDOPTIONS.Field0,},
        ADDONBEFOREVALUE:props.SchedulingParams.ADDONBEFOREVALUE,
        EMAIL:props.SchedulingParams.EMAIL ?? "",
        EMAIL_COPY:props.SchedulingParams.EMAIL_COPY ?? "",
        BACKUP:props.SchedulingParams.BACKUP ?? "",
    } as any;
    baseparams.basecondition.Options = {
        // 欄位選項
        fieldoptions:props.SchedulingParams.FIELDOPTIONS.Options.fieldoptions,
        // 關係運算符
        RelationalOperators: props.SchedulingParams.FIELDOPTIONS.Options.RelationalOperators,
        // 设定列
        ObservedColumnYlabel: props.SchedulingParams.FIELDOPTIONS.Options.ObservedColumnYlabel,
        addonBeforeValue: props.SchedulingParams.FIELDOPTIONS.Options.addonBeforeValue,
        RelationalOperators0:props.SchedulingParams.CONDITION,
    }
    baseparams.basecondition = JSON.stringify(baseparams.basecondition);
    function checkworknumber(worknumber: string,type:string) {
        let flag = true;
        let regEmail = /^\w{3,}(\.\w+)*@[A-z0-9]+(\.[A-z]{2,5}){1,2}$/;
        // 如果是數字
        if (!isNaN(parseFloat(Number(worknumber))) && isFinite(Number(worknumber))) {
            if (worknumber.length != 8) {
                message.error(`${type}${worknumber}工號格式不正確!`);
                flag = false;
            }
        }
        // 如果是郵箱
        else if (!regEmail.test(worknumber)) {
            message.error(`${type}${worknumber}郵箱格式不正確!`);
            flag = false;
        }
        return flag
    }
    let Recipientemail = baseparams.EMAIL.trim().split(',');
    if(Recipientemail.length  && Recipientemail[0]!=''){
        for (let i = 0; i < Recipientemail.length; i++) {
        if (!checkworknumber(Recipientemail[i],"收件人")) {
            return;
        }
    }
    }
    let Ccemail = baseparams.EMAIL_COPY.trim().split(',');
    if(Ccemail.length && Ccemail[0]!=''){
        for (let i = 0; i < Ccemail.length; i++) {
        if (!checkworknumber(Ccemail[i],"抄送")) {
            return;
        }
    }
    }
    let { api01: { code, message: Message } } = await GetCommon(baseparams);
    if (code == 200) {
        message.success(Message);
        handleCancel();
    } else {
        message.error(Message);
    }
    emits('closemodal', false);
}

const emits = defineEmits(['closemodal']);
function handleCancel() {
    formRef.value?.resetFields();
    emits('closemodal', false);
}
</script>

<style lang="less" scoped></style>