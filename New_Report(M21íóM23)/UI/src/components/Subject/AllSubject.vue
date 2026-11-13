
<script type="text/jsx" >
import { defineComponent, reactive, ref } from '@vue/runtime-core';
import { Radio, message } from 'ant-design-vue';
import { AnswerRecord } from '@/store/module/record';
import { StandardAnswer } from '@/store/module/judge';
import Answer from '@/components/Answer/index.vue';

const UseRecord = AnswerRecord();
const UseStandard = StandardAnswer();

export const BaseRadio = (ValueList) => {
    return ValueList.map((item, index) => (
        <div>
            <Radio value={item.value} disabled={UseRecord.isComplete}>
                {item.value}.{item.text}
            </Radio>
        </div>
    ));
};
const SubModel = reactive({
    'test1-1': '',
    'test1-2': '',
    'test1-3': '',
    'test1-4': '',
    'test1-5': '',
    'test1-6': '',
    'test1-7': '',
    'test1-8': '',
    'test1-9': '',
    'test1-10': ''
});
const CheckBoxSub = (SubjectName, Subject) => {
    return SubjectName.map((item, index) => (
        //   console.log(item,index,SubjectName[index])
        <>
            <div>
                {index + 1}.{item.text}
            </div>
            <Radio.Group v-model:value={SubModel[item.model]} onChange={() => AddAnswer(SubModel[item.model], index)}>
                {BaseRadio(Subject[index])}
            </Radio.Group>
        </>
    ));
};
const AddAnswer = (value, index) => {
    const params = { useranswer: value, id: index + 1 };
    UseRecord.AddAnswer(params);
};

const props = {
    params: { type: Object, default: {} }
};
export default defineComponent({
    props,
    components: { Radio },
    setup(props, context) {
        const Submit = () => {
            let Fraction = 0;
            for (let i in SubModel) {
                if (SubModel[i] == '') {
                    return message.error('还有题目未作答,请全部作答后提交');
                } else if (SubModel[i] == props.params.Answer[i]) {
                    Fraction += 10;
                }
            }
            UseStandard.AddAnswer(props.params.Answer); //将标准答案与user答案对比进行判错
            UseRecord.ChangeCompleteStatus(true); //改变可选中状态
            UseRecord.ChangeRecord(Fraction);
            return message.success(`你的分数是:${Fraction}`);
        };

        //数据由此进入
        const render = () => {
            return (
                <a-row>
                   {/*<a-col span={8} style="margin:auto">
                        <Answer></Answer>
                    </a-col>*/}
                    <a-col span={24}>
                        <h2>{props.params.DesCription}</h2>
                        <div>{CheckBoxSub(props.params.SubjectName, props.params.Subject)}</div>
                        {/*<div>{test(SubjectName)}</div>*/}
                        <div style={{ textAlign: 'center' }}>
                            <a-button onClick={Submit} type="primary">
                                提交
                            </a-button>
                        </div>
                    </a-col>
                </a-row>
            );
        };
        return render;
    }
});
</script>
