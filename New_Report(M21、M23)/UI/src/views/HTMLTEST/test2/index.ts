export const Subject = [
    [
        { value: 'A', text: '<a url="http://www.w3school.com.cn">W3School.com.cn</a' },
        { value: 'B', text: '<a href="http://www.w3school.com.cn">W3School</a>' },
        { value: 'C', text: '<a>http://www.w3school.com.cn</a>' },
        { value: 'D', text: '<a name="http://www.w3school.com.cn">W3School.com.cn</a>' },
    ],
    [
        { value: 'A', text: '<a href="xxx@yyy">' },
        { value: 'B', text: '<mail href="xxx@yyy">' },
        { value: 'C', text: '<a href="mailto:xxx@yyy">' },
        { value: 'D', text: '<mail>xxx@yyy</mail>' },
    ],
    [
        { value: 'A', text: '<table><head><tfoot>' },
        { value: 'B', text: '<table><tr><td>' },
        { value: 'C', text: '<table><tr><tt>' },
        { value: 'D', text: '<thead><body><tr>' },
    ], [
        { value: 'A', text: '<td align="left">' },
        { value: 'B', text: '<td valign="left">' },
        { value: 'C', text: '<td leftalign>' },
        { value: 'D', text: '<tdleft>' },
    ], [
        { value: 'A', text: '<ul>' },
        { value: 'B', text: '<dl>' },
        { value: 'C', text: '<ol>' },
        { value: 'D', text: '<list>' },
    ], [
        { value: 'A', text: '<input type="check">' },
        { value: 'B', text: '<checkbox>' },
        { value: 'C', text: '<input type="checkbox">' },
        { value: 'D', text: '<check>' },
    ], [
        { value: 'A', text: '<input type="textfield">' },
        { value: 'B', text: '<textinput type="text">' },
        { value: 'C', text: '<input type="text">' },
        { value: 'D', text: '<textfield>' },
    ], [

        { value: 'A', text: '<list>' },
        { value: 'B', text: '<input type="list">' },
        { value: 'C', text: '<input type="dropdown">' },
        { value: 'D', text: '<select>' },
    ], [

        { value: 'A', text: '<textarea>' },
        { value: 'B', text: '<input type="textarea">' },
        { value: 'C', text: '<input type="textbox">' },
    ], [
        { value: 'A', text: '<img href="image.gif">' },
        { value: 'B', text: '<image src="image.gif">' },
        { value: 'C', text: '<img src="image.gif">' },
        { value: 'D', text: '<img>image.gif</img>' },
    ]
];

export const SubjectName = [
    {
        text: '在下列的 HTML 中，哪个可以产生超链接？',
        model: 'test1-1',
    },
    { text: '如何制作电子邮件链接？', model: 'test1-2' },
    { text: '以下选项中，哪个全部都是表格标签？', model: 'test1-3' },
    { text: '请选择可以使单元格中的内容进行左对齐的正确 HTML 标签', model: 'test1-4' },
    { text: '如何产生带有数字列表符号的列表？', model: 'test1-5' },
    { text: '在下列的 HTML 中，哪个可以产生复选框？', model: 'test1-6' },
    { text: '在下列的 HTML 中，哪个可以产生文本框？', model: 'test1-7' },
    { text: '在下列的 HTML 中，哪个可以产生下拉列表？', model: 'test1-8' },
    { text: '在下列的 HTML 中，哪个可以产生文本区（textarea）', model: 'test1-9' },
    { text: '在下列的 HTML 中，哪个可以插入背景图像？', model: 'test1-10' }

];
export const Answer = {
    "test1-1": "B",
    "test1-2": "C",
    "test1-3": "B",
    "test1-4": "A",
    "test1-5": "C",
    "test1-6": "C",
    "test1-7": "C",
    "test1-8": "D",
    "test1-9": "A",
    "test1-10": "C",
};
export const DesCription = "HTML 测试二"
