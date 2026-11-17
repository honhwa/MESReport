<template>
    <div>
        <h2 v-if="SchedulingParams.type == 'add'">新增{{ SchedulingParams?.reportname }}排程</h2>
        <a-form ref="formRef" name="dynamic_form_nest_item" :model="dynamicValidateForm" @finish="onFinish"
            layout="vertical" :label-col="{ span: 18 }" :wrapper-col="{ span: 22 }">
            <a-form-item name="TaskName" label="任務名稱" :rules="[{ required: true, message: '請輸入任務名稱' }]">
                <a-input v-model:value="dynamicValidateForm.TaskName" style="width: 280px" allowClear autocomplete="off" />
            </a-form-item>
            <a-form-item name="Mapp_ID" label="Mapp會議室編號" :rules="[{ required: true, message: '請輸入Mapp會議室編號' }]">
                <a-input-number v-model:value="dynamicValidateForm.Mapp_ID" style="width: 180px" />
            </a-form-item>
            <a-form-item name="StartTime" label="開始時間">
                <a-date-picker :show-time="{ format: 'HH:mm' }" placeholder="Select Time"
                    v-model:value="dynamicValidateForm.StartTime" format="YYYY-MM-DD HH:mm"
                    valueFormat="YYYY-MM-DD hh:mm" />
                <p>爲空表示不限制開始時間</p>
            </a-form-item>
            <a-form-item name="EndTime" label="停止時間">
                <a-date-picker :show-time="{ format: 'HH:mm' }" placeholder="Select Time"
                    v-model:value="dynamicValidateForm.EndTime" format="YYYY-MM-DD HH:mm" valueFormat="YYYY-MM-DD hh:mm" />
                <p>爲空表示不限制停止時間</p>
                <a-form-item name="Cron" label="Cron表達式(輸入格式不帶秒)">
                    <ul>
                        <li>
                            <a href="https://cron.qqe2.com/" target="_blank">cron在綫網站1</a>
                        </li>
                        <li>
                            <a href="https://www.pppet.net/" target="_blank">cron在綫網站2
                            </a>
                        </li>
                    </ul>
                    <a-input v-model:value="dynamicValidateForm.cronstr" style="width: 280px" allowClear
                        autocomplete="off" />
                </a-form-item>
            </a-form-item>
            <a-space size="middle">
                <a-form-item name="不确定列" :label="Options.ObservedColumnYlabel">
                    <a-select v-model:value="dynamicValidateForm.basecondition.RelationalOperators0"
                        :options="Options.fieldoptions" style="width: 180px"></a-select>
                </a-form-item>
                <a-form-item name="栏位" label="栏位">
                    <a-select v-model:value="dynamicValidateForm.basecondition.Field0"
                        :options="Options.RelationalOperators" style="width: 180px">
                    </a-select>
                </a-form-item>
                <a-form-item name="conditionvalue1" label="值(between输入范例：1 and 2)">
                    <a-input v-model:value="dynamicValidateForm.basecondition.Conditionvalue0" style="width: 260px">
                        <template #addonBefore>
                            <a-select v-model:value="Options.addonBeforeValue" style="width: 100px">
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
                <a-switch v-model:checked="dynamicValidateForm.ExecuteImmediately" />
            </a-form-item>
            <a-form-item name="Notify" label="是否Mapp通知">
                <a-switch v-model:checked="dynamicValidateForm.Notify" />
            </a-form-item>
            <a-form-item name="Recipient" label="收件人(爲空時不發送郵件,輸入格式:逗號分割，可輸入工號)">
                <a-textarea v-model:value="dynamicValidateForm.Recipient" placeholder="請輸入收件人郵箱" auto-size allowClear />
            </a-form-item>
            <a-form-item name="CC" label="抄送(cc)">
                <a-textarea v-model:value="dynamicValidateForm.EMAIL_COPY" placeholder="請輸入收件人郵箱" auto-size allowClear />
            </a-form-item>
            <a-form-item name="Notes" label="備注">
                <a-textarea v-model:value="dynamicValidateForm.Notes" :rows="4" placeholder="任務描述" show-count
                    :maxlength="300" />
            </a-form-item>
            <a-form-item>
                <a-button type="primary" html-type="submit">提交</a-button>
            </a-form-item>
        </a-form>
    </div>
</template>

<script setup lang="ts">
import { reactive, ref, watch } from 'vue';
import { FormInstance, message } from 'ant-design-vue';
import { ReportCommonWithOut } from '@/store/module/ReportCommon';
import { GetCommon } from '@/api/pages/common';
import { Storage } from '@/utils/Storage';
import { cloneDeep } from 'lodash-es';
const UserParams = Storage.get('UserMessage');
const ReportCommonParams = ReportCommonWithOut();

let SchedulingParams = reactive<any>(ReportCommonParams.Scheduling);



const formRef = ref<FormInstance>();


let Options = reactive({
    // 欄位選項
    fieldoptions: SchedulingParams["ObservedColumnYValueOptions"] ?? [],
    // 關係運算符
    RelationalOperators: SchedulingParams["ObservedColumnXOptions"] ?? [],
    // 设定列
    ObservedColumnYlabel: SchedulingParams['ObservedColumnY'] ? SchedulingParams['ObservedColumnY'].label + '设定列' : '设定列',
    addonBeforeValue: "=",
})
const dynamicValidateForm = reactive({
    // 任務名稱
    TaskName: "",
    StartTime: "",
    EndTime: "",
    Mapp_ID: undefined,
    cronstr: '',
    basecondition: {
        RelationalOperators0: SchedulingParams?.column?.dataIndex,
        Field0: SchedulingParams?.column?.dataIndex,
        Conditionvalue0: SchedulingParams?.record?.[SchedulingParams?.column?.dataIndex],
    } as any,
    Notes: "",
    // 是否運行
    ExecuteImmediately: true,
    // 是否mapp通知
    Notify: true,
    // 是否創建成功啓動
    ISIMMEDIATE: true,
    // 收件人
    Recipient: "",
    // 抄送(CC)
    EMAIL_COPY: ""

});



// 提交
const onFinish = async (values) => {
    console.log('Received values of form:', values);
    console.log('dynamicValidateForm:', dynamicValidateForm);
    let dynamicValidateFormclone = cloneDeep(dynamicValidateForm);
    // 拼接运算符号和数字
    let originCondition = dynamicValidateFormclone.basecondition.Conditionvalue0;
    dynamicValidateFormclone.basecondition.Conditionvalue0 = Options.addonBeforeValue + " " + dynamicValidateFormclone.basecondition.Conditionvalue0;
    dynamicValidateFormclone.basecondition.Options = Options;
    let baseparams = {
        apiName: 'Scheduling_Get',
        creator: UserParams.MemName,
        ...dynamicValidateFormclone,
        apiparams: SchedulingParams.apiparams,
        routeId: SchedulingParams.routeId,
        MT_COLUMN: SchedulingParams.MT_COLUMN,
        // 观察列的dataindex
        CONTROLROW: SchedulingParams['ObservedColumnY'].value,
        addonBeforeValue: originCondition,
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
    let Recipientemail = baseparams.Recipient.trim().split(',');
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
    try {
        let { api01 } = await GetCommon(baseparams);
        if (api01 == "Insert Success!") {
            // 表單重置
            formRef.value?.resetFields();
            return message.success("添加成功");
        } else {
            return message.error("添加失敗");
        }
    } catch (error) {
        console.log(error);
        message.error("添加失敗:" + (error as Error).message);
    }
}


watch(
    () => ReportCommonParams.Scheduling,
    (newvalue) => {
        SchedulingParams = newvalue;
        console.log("子組件更新", SchedulingParams);
        if (SchedulingParams["Allcolumns"]) {
            Options.fieldoptions = SchedulingParams["ObservedColumnYValueOptions"];
            Options.RelationalOperators = SchedulingParams["ObservedColumnXOptions"] ?? [];
            Options.ObservedColumnYlabel = SchedulingParams['ObservedColumnY'] ? SchedulingParams['ObservedColumnY'].label + '设定列' : '设定列';
            Options.addonBeforeValue = '=';
            dynamicValidateForm.basecondition.RelationalOperators0 = SchedulingParams?.record?.[SchedulingParams?.ObservedColumnY.value];
            dynamicValidateForm.basecondition.Conditionvalue0 = SchedulingParams?.record?.[SchedulingParams?.column?.dataIndex];
            dynamicValidateForm.basecondition.Field0 = SchedulingParams?.column?.dataIndex
        }
        // 表單重置
        formRef.value?.resetFields();
    },
    {
        deep: true,
        immediate: true
    }
)
</script>

<style lang="less" scoped></style>