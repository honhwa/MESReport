import { PoweroffOutlined, SearchOutlined } from '@ant-design/icons-vue';

export const Buttonlist = [
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