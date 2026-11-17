<script type="text/jsx">
import { computed, defineComponent, reactive, ref } from "@vue/runtime-core";
import { BaseRadio } from "./AllSubject.vue";
import { Radio, message } from "ant-design-vue";
import { PoweroffOutlined, SearchOutlined } from '@ant-design/icons-vue';

let current = ref(0);
const props = {
  params: { type: Object, default: {} },
};

export default defineComponent({
  props,
  setup(props, context) {
    let Buttonlist = [
      { type: "primary", title: "上一个", handle: () => LastOne() },
      { type: "dashed", title: "下一个", handle: () => NextOne() },
      {
        type: "primary",
        title: "提交",
        showicon: true,
        icon: PoweroffOutlined,
        handle: () => Submit(),
      },
    ];
    // let modeltext =props.params.SubjectName[current.value]["model"]
    //我这么写不能为什么modeltext会不变？？因为modeltext并不具有响应性啊
    const modeltext = computed(() => {
      return props.params.SubjectName[current.value]["model"];
    });
    let texttitle = computed(() => {
      return props.params.SubjectName[current.value]["text"];
    });
    const NextOne = () => {
      if (current.value < props.params.Subject.length - 1) {
        current.value++;
      } else {
        current.value = 0;
      }
    };
    const LastOne = () => {
      if (current.value > 0) {
        current.value--;
      } else {
        current.value = props.params.Subject.length - 1;
      }
    };
    const Submit = () => {
      let Fraction = 0;
      for (let i in SubModel) {
        if (SubModel[i] == "") {
          return message.error("还有题目未作答,请全部作答后提交");
        } else if (SubModel[i] == props.params.Answer[i]) {
          Fraction += 10;
        }
      }
      return message.success(`你的分数是:${Fraction}`);
    };
    const button = (Buttonlist) => {
      return Buttonlist.map((item, index) => (
        <>
          <a-button
            onClick={() => item.handle()}
            type="primary"
            style={{ marginLeft: "15px" }}
          >
            {item.title}
          </a-button>
        </>
      ));
    };
    let SubModel = reactive({
      "test1-1": "",
      "test1-2": "",
      "test1-3": "",
      "test1-4": "",
      "test1-5": "",
      "test1-6": "",
      "test1-7": "",
      "test1-8": "",
      "test1-9": "",
      "test1-10": "",
    });
    const render = () => {
      return (
        <div>
          <div>
            {current.value + 1}.{texttitle.value}
          </div>
          <div style={{ minHeight: "120px" }}>
            <Radio.Group v-model:value={SubModel[modeltext.value]}>
              {BaseRadio(props.params.Subject[current.value])}
            </Radio.Group>
          </div>
          <div style={{ textAlign: "center" }}>{button(Buttonlist)}</div>
        </div>
      );
    };
    return render;
  },
});
</script>