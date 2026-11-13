
export const Subject = [
  [
    { value: 'A', text: '<script>' },
    { value: 'B', text: '<javascript>' },
    { value: 'C', text: '<js>' },
    { value: 'D', text: '<scripting>' },
  ],
  [
    { value: 'A', text: '("Hello World")' },
    { value: 'B', text: 'Hello World' },
    { value: 'C', text: 'response.write("Hello World")' },
    { value: 'D', text: 'document.write("Hello World")' },
  ],
  [
    { value: 'A', text: '<body> 部分' },
    { value: 'B', text: '<head> 部分' },
    { value: 'C', text: '<body> 部分和<head> 部分' },
  ],
  [
    { value: 'A', text: ' <script src="runoob.js">' },
    { value: 'B', text: ' <script href="runoob.js">' },
    { value: 'C', text: '<script name="runoob.js">' },
  ],
  [
    { value: 'A', text: '<body> 需要' },
    { value: 'B', text: '<head> 不需要' },
  ],
  [
    { value: 'A', text: 'alertBox="Hello World"' },
    { value: 'B', text: 'msgBox("Hello World")' },
    { value: 'C', text: 'alert("Hello World")' },
    { value: 'D', text: 'alertBox("Hello World")' },
  ],
  [
    { value: 'A', text: 'function:myFunction()' },
    { value: 'B', text: 'function myFunction()' },
    { value: 'C', text: 'function=myFunction()' },
  ],
  [
    { value: 'A', text: 'call function runoobFunction' },
    { value: 'B', text: 'call runoobFunction()' },
    { value: 'C', text: 'runoobFunction()' },
  ],
  [
    { value: 'A', text: 'if (i==5)' },
    { value: 'B', text: 'if i=5 then' },
    { value: 'C', text: 'if i=5' },
    { value: 'D', text: 'if i==5 then' },
  ],
  [
    { value: 'A', text: 'if !=5 then' },
    { value: 'B', text: 'if <>5' },
    { value: 'C', text: 'if (i <> 5)' },
    { value: 'D', text: 'if (i != 5)' },
  ],
];

export const SubjectName = [
  {
    text: '我们可以在下列哪个 HTML 元素中放置 Javascript 代码？',
    model: 'test1-1',
  },
  { text: '写 “Hello World” 的正确 Javascript 语法是？', model: 'test1-2' },
  { text: 'Javascript 可以插入到以下哪些位置中？', model: 'test1-3' },
  { text: '引用名为 “runoob.js” 的外部脚本的正确语法是？', model: 'test1-4' },
  { text: '外部脚本必须包含 <script> 标签吗？', model: 'test1-5' },
  { text: '如何在警告框中写入 “Hello World”？', model: 'test1-6' },
  { text: '如何创建函数？', model: 'test1-7' },
  { text: '如何调用名为 “runoobFunction” 的函数？', model: 'test1-8' },
  { text: '如何编写当 i 等于 5 时执行一些语句的条件语句？', model: 'test1-9' },
  {
    text: '如何编写当 i 不等于 5 时执行一些语句的条件语句？',
    model: 'test1-10',
  },
];
export const Answer = {
  'test1-1': 'A',
  'test1-2': 'B',
  'test1-3': 'D',
  'test1-4': 'D',
  'test1-5': 'D',
  'test1-6': 'D',
  'test1-7': 'D',
  'test1-8': 'C',
  'test1-9': 'B',
  'test1-10': 'A',
};
// export let Buttonlist = [
//   { type: 'primary', title: '上一个', handle: () => LastOne },
//   { type: 'dashed', title: '下一个', handle: () => NextOne },
//   {
//     type: 'primary',
//     title: '提交',
//     showicon: true,
//     icon: PoweroffOutlined,
//     handle: () => Submit,
//   },
// ];
