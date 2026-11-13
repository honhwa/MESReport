const datalist = [{
    station:'bing',
    title:'必应',
    type:'icon-bing'
},
{
    station:'360',
    title:'360',
    type:'icon-360logo'
},
{
    station:'sogou',
    title:'搜狗',
    type:'icon-sogou'
},
{
    station:'bilibili',
    title:'b站',
    type:'icon-bilibili-line'
},
{
    station:'gitee',
    title:'Gitee',
    type:'icon-gitee-fill-round'
}]

export const iframeSearch = [{
    station:'https://cn.bing.com/search?q=',
    top:'-30px',
    title:'bing'
},{
    station:'https://www.so.com/s?q=',
    top:'-30px',
    title:'360'
},{
    station:'https://www.sogou.com/web?query=',
    top:'-30px',
    title:'sogou'
},{
    station:'https://search.bilibili.com/all?keyword=',
    top:'-80px',
    title:'bilibili'
},{
    station:'https://search.gitee.com/?q=',
    top:'-50px',
    title:'gitee'
},]
export default datalist

// const iframepage = iframeSearch.map((item, index) => {
//     if (item.title === current.value) {
//         return <iframe src={`${item.station}${searchText.value}`} style={{ border: 'none', width: '100%', height: '100vh', position: 'fixed', top: item.top, zIndex: 1 }} />
//     }
// });