
export const Subject = [
    [
      { value: 'A', text: '三种。for 循环、do...while 循环和 while 循环。' },
      { value: 'B', text: '四种。for 循环、while 循环、do...while 循环以及 loop...until 循环。' },
      { value: 'C', text: '一种。for 循环。' },
    ],
    [
      { value: 'A', text: 'for (i <= 5; i++)' },
      { value: 'B', text: 'for (i = 0; i <= 5; i++)' },
      { value: 'C', text: 'for (i = 0; i <= 5)' },
      { value: 'D', text: 'for i = 1 to 5' },
    ],
    [
      { value: 'A', text:  "'这是一个注释" },
      { value: 'B', text: '<!--这是一个注释-->' },
      { value: 'C', text: '//这是一个注释' },
    ],
    [
      { value: 'A', text: '/*这是多行注释*/' },
      { value: 'B', text: '//这是多行注释//' },
      { value: 'C', text: '<!--这是多行注释-->' },
    ],
    [
      { value: 'A', text: ' var txt = new Array="runoob","google","taobao"' },
      { value: 'B', text: 'var txt = new Array(1:"runoob",2:"google",3:"taobao")' },
      { value: 'C', text: 'var txt = new Array("runoob","google","taobao")' },
      { value: 'D', text: 'var txt = new Array:1=("runoob")2=("google")3=("taobao")' },
    ],
    [
      { value: 'A', text: ' round(7.25)' },
      { value: 'B', text: ' rnd(7.25)' },
      { value: 'C', text: 'Math.rnd(7.25)' },
      { value: 'D', text: ' Math.round(7.25)' },
    ],
    [
      { value: 'A', text: 'Math.ceil(2,4)' },
      { value: 'B', text: 'Math.max(2,4)' },
      { value: 'C', text: 'ceil(2,4)' },
      { value: 'D', text: 'top(2,4)' },
    ],
    [
      { value: 'A', text: 'open.new("https://www.runoob.com","window2")' },
      { value: 'B', text: ' new.window("https://www.runoob.com","window2")' },
      { value: 'C', text: 'new("https://www.runoob.com","window2")' },
      { value: 'D', text: ' window.open("https://www.runoob.com","window2")' },
    ],
    [
      { value: 'A', text: 'statusbar = "put your message here"' },
      { value: 'B', text: 'window.status = "put your message here"' },
      { value: 'C', text: 'window.status("put your message here")' },
      { value: 'D', text: 'status("put your message here")' },
    ],
    [
      { value: 'A', text: 'client.navName' },
      { value: 'B', text: 'navigator.appName' },
      { value: 'C', text: 'browser.name' },
    ],
  ];
  
  export const SubjectName = [
    {
      text: '在 JavaScript 中，有多少种不同类型的循环？',
      model: 'test1-1',
    },
    { text: 'for 循环如何开始？', model: 'test1-2' },
    { text: '如何在 JavaScript 中添加注释？', model: 'test1-3' },
    { text: '可插入多行注释的 JavaScript 语法是？', model: 'test1-4' },
    { text: '定义 JavaScript 数组的正确方法是？', model: 'test1-5' },
    { text: '如何把 7.25 四舍五入为最接近的整数？', model: 'test1-6' },
    { text: '如何求得 2 和 4 中最大的数？', model: 'test1-7' },
    { text: '打开名为 “window2” 的新窗口的 JavaScript 语法是？', model: 'test1-8' },
    { text: '如何在浏览器的状态栏放入一条消息？', model: 'test1-9' },
    {
      text: '如何获得客户端浏览器的名称？',
      model: 'test1-10',
    },
  ];
  export const Answer = {
    'test1-1': 'A',
    'test1-2': 'B',
    'test1-3': 'C',
    'test1-4': 'A',
    'test1-5': 'C',
    'test1-6': 'D',
    'test1-7': 'B',
    'test1-8': 'D',
    'test1-9': 'B',
    'test1-10': 'B',
  };
  export const DesCription = "JavaScript 测试二"
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
  