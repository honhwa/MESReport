export const Subject = [
    [
        { value: 'A', text: '<group>' },
        { value: 'B', text: '<header>' },
        { value: 'C', text: '<headings>' },
        { value: 'D', text: '<hgroup>' },
    ],
    [
        { value: 'A', text: '<h6>' },
        { value: 'B', text: '<head>' },
        { value: 'C', text: '<heading>' },
        { value: 'D', text: '<h1>' },
    ],
    [
        { value: 'A', text: '<br>' },
        { value: 'B', text: '<lb>' },
        { value: 'C', text: '<break>' },
    ], [
        { value: 'A', text: 'HTML 元素' },
        { value: 'B', text: '样式属性' },
        { value: 'C', text: '事件属性' },
    ], [
        { value: 'A', text: ' <movie>' },
        { value: 'B', text: ' <media>' },
        { value: 'C', text: ' <video>' },
    ], [
        { value: 'A', text: '<mp3>' },
        { value: 'B', text: '<audio>' },
        { value: 'C', text: '<sound>' },
    ], [
        { value: 'A', text: '<body color="yellow">' },
        { value: 'B', text: '<background>yellow</background>' },
        { value: 'C', text: '<body bgcolor="yellow">' },
    ], [
        { value: 'A', text: 'getPosition()' },
        { value: 'B', text: 'getCurrentPosition()' },
        { value: 'C', text: 'getUserPosition()' },
    ],[
        { value: 'A', text: '<a href="reight.html" target= "_blank">节目</a>' },
        { value: 'B', text: '<a href="reight.html" target="_parent">节目</a>' },
        { value: 'C', text: '<a href="reight.html" target="_top">节目</a>' },
        { value: 'D', text: 'a href="reight.html" target="_self">节目</a>' },
    ],[
        { value: 'A', text: '<br/> ' },
        { value: 'B', text: '<br></br>  ' },
        { value: 'C', text: '<hr/>' },
        { value: 'D', text: '<hr></hr>' },
    ]
];

export const SubjectName = [
    {
        text: '在 HTML中，哪个元素用于组合标题元素？',
        model: 'test1-1',
    },
    { text: '在下列的 HTML 中，哪个是最大的标题？', model: 'test1-2' },
    {text:'在下列的 HTML 中，哪个可以插入折行？',model: 'test1-3'},
    { text: '在 HTML中，onblur 和 onfocus 是：', model: 'test1-4' },
    { text: '用于播放 HTML 视频文件的正确 HTML 元素是：', model: 'test1-5' },
    { text: '用于播放 HTML 音频文件的正确 HTML 元素是：', model: 'test1-6' },
    { text: '在下列的 HTML 中，哪个可以添加背景颜色？', model: 'test1-7' },
    { text: '在 HTML 中，哪个方法用于获得用户的当前位置？', model: 'test1-8' },
    {text:'在HTML中，若实现点击超链接时，弹出一个新的网页窗口，下面（ ）选项符合要求',model:'test1-9'},
    {text:'在HTML中，使用（ ）标签可以实现在页面上添加一条水平线',model:'test1-10'}

];
export const Answer = {
    "test1-1": "D",
    "test1-2": "D",
    "test1-3": "A",
    "test1-4": "C",
    "test1-5": "C",
    "test1-6": "B",
    "test1-7": "C",
    "test1-8": "B",
    "test1-9": "A",
    "test1-10": "C",
};
export const DesCription = "HTML 测试一"
