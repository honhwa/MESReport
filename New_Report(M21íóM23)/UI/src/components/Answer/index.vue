<script type="text/jsx">
import { computed, defineComponent } from '@vue/runtime-core';
import { AnswerRecord } from '@/store/module/record';
import { StandardAnswer } from '@/store/module/judge';

const props = {
    AnswerParams: { type: Object, default: {} }
};

const FatherClass = {
    display: 'flex',
    flexDirection: 'row',
    flexWrap: 'wrap'
};

export default defineComponent({
    props,
    setup(props, context) {
        const UseRecord = AnswerRecord();
        const UseStandard = StandardAnswer();
        // const useranswer = UseStandard.Standard;
        const isRight = (item, StandardAnswer) => {
            let color = {};
            if (!StandardAnswer) {
                color = 'red';
            } else if (StandardAnswer[`test1-${item.id}`] != item.useranswer) {
                color = 'lightblue';
            } else {
                color = 'blue';
            }
            return color;
        };
        const BaseCircle = (List) => {
            return List.map((item, index) => (
                <div class="ChildClass">
                    <div class="Circle" class={isRight(item, UseStandard.Standard)}>
                        {/*只有class才支持函数式绑定,style只支持js表达式
            类似于computed属性,和onClick不同
            */}
                        <div style={{ textAlign: 'center' }}>{item.id}</div>
                    </div>
                    <div class="sortnum">{item.useranswer}</div>
                </div>
            ));
        };
        const Record = (RecordParams) => {
            return (
                <div class="title Record">
                    <div>你最终的成绩是</div>
                    <div >{RecordParams}</div>
                </div>
            );
        };
{/**class="sidebar" */}
        const render = () => {
            return (
                <div class="sidebar">
                    <div class="title">答题卡</div>
                    <div style={FatherClass}>{BaseCircle(UseRecord.Answer)}</div>
                    {Record(UseRecord.Record)}
                </div>
            );
        };
        return render;
    }
});
</script>

<style scoped src="./index.css">

</style>