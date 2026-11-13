// 先使用jsx的寫法，ts實在是hold不住，報錯太多了
// 後續這裏改成<script lang="tsx"> 或者文件名改成.tsx
import { useRouter } from 'vue-router';
import defaultTabPane, { DEFAULT_COVER } from './constant/defaultPane';
import { defineComponent, reactive, isRef, ref, defineProps } from '@vue/runtime-core';
// const cover = DEFAULT_COVER;
import { Avatar, Card, List, Input, message, Drawer, Button, Tabs, Tooltip, Image, Switch } from 'ant-design-vue';
import { CURRENT_COVER, GHOST_OPEN, PROJECT_GITHUB, SEARCH_ALL_OPEN, WEB_HOST } from './constant';
import logo from '@/assets/images/logo.png';
import baidu from '@/assets/images/baidu.png';
import google from '@/assets/images/google.png';
import './index.css';
import './ghost.css';
import template from './constant/defaultHome';
import { PictureOutlined, SearchOutlined, GithubOutlined, SendOutlined } from '@ant-design/icons-vue';
import axios from 'axios';
import Component from '@/locales/lang/en-US/component';

const { Search } = Input;

const listGrid = {
    gutter: 28,
    xxl: 4,
    xl: 4,
    lg: 4,
    md: 3,
    sm: 2,
    xs: 1
};

const gridStyle = {
    width: '25%',
    textAlign: 'center',
    fontSize: '13px',
    color: '#555'
};

const localStorage = window.localStorage;

export default defineComponent((props, context) => {
    // 开启万能搜索
    const currentSearchAll = localStorage.getItem(SEARCH_ALL_OPEN)
        ? JSON.parse(localStorage.getItem(SEARCH_ALL_OPEN))
        : true;
    // 当前封面
    const currentCover = localStorage.getItem(CURRENT_COVER)
        ? JSON.parse(localStorage.getItem(CURRENT_COVER))
        : DEFAULT_COVER;
    // 透明模式
    const currentGhostClose = localStorage.getItem(GHOST_OPEN) ? JSON.parse(localStorage.getItem(GHOST_OPEN)) : false;
    const router = useRouter();
    const tabKey = 'myLike';
    let maskOpened = ref(false);
    // function UseState(status, functioname) {
    //     function Setfunctioname() {
    //         functioname.value = status;
    //     }
    //     return [functioname, Setfunctioname];
    // }
    let drawerVisible = ref(false);
    let isBaidu = ref(true);
    let searchAll = ref(currentSearchAll);
    const ghostClose = ref(currentGhostClose); //添加響應性,問題是怎么優雅的寫出類似react中usestate的寫法
    function UseState(status, functioname) {
        functioname.value = status;
    }
    let coverPage = currentCover;
    function setCover(status) {
        coverPage = status;
    }

    const doSearch = (value) => {
        console.log(props, context.attrs.color, props.color);
        if (value.length < 1) {
            message.error('请输入搜索内容');
            return;
        }
        if (searchAll.value) {
            router.push({
                //vue3的路由，如果路由少的话不要分很多文件夹
                name: 'search',
                params: {
                    q: value
                }
            });
            // router.push({ name: 'search', params: { q: value } });
            // navigate(`/search?q=${value}`);
            return;
        }
        if (isBaidu.value) {
            window.open(`https://www.baidu.com/s?wd=${value}`);
        } else {
            window.open(`https://www.google.com/search?q=${value}`);
        }
    };

    const changeCover = async (cover) => {
        if (cover.type === 'api') {
            const hide = message.loading('获取壁纸中..', 0); //在这里已经触发此动作 获取壁纸中
            const res = await axios.get(`https://bird.ioliu.cn/v1/?url=${cover.api}`).finally(() => hide()); //在此调用hide()移除loading动作
            //finally无论成功与否都会执行
            if (res.data && res.data.imgurl) {
                cover.src = res.data.imgurl;
                cover.preview = res.data.imgurl;
                message.success('切换成功');
            } else {
                message.error('请求异常，请刷新重试');
                return;
            }
        } else {
            UseState(false, drawerVisible);
        }
        const newCover = { ...cover }; //...是rest 参数或者叫扩展运算符
        setCover(newCover);
        localStorage.setItem(CURRENT_COVER, JSON.stringify(newCover));
    };

    const toggleSearchAll = (e) => {
        if (searchAll.value) {
            message.success('已关闭万能搜索');
        } else {
            message.success('已开启万能搜索');
        }
        localStorage.setItem(SEARCH_ALL_OPEN, JSON.stringify(!searchAll.value)); //localStorage会自动转换SEARCH_ALL_OPEN为searchAllOpen
        UseState(!searchAll.value, searchAll);
    };

    const toggleGhost = (checked, e) => {
        localStorage.setItem(GHOST_OPEN, JSON.stringify(checked));
        UseState(checked, ghostClose);
    };

    const onClose = (e) => {
        UseState(false, drawerVisible);
    };

    const renderViewByTabKey = template.keyContentMap[tabKey].map((resource, index) => (
        <a key={index} target="_blank" href={resource.link}>
            <Card.Grid style={gridStyle}>
                <Avatar shape="square" src={resource.icon} />
                <div className="resource-name">{resource.name}</div>
            </Card.Grid>
        </a>
    ));
    const toTestPage = (route) => {
        router.push(route);
    };

    // const changeCover = async (cover) => {
    //   console.log(cover);
    //   const newCover = { ...cover }; //...是rest 参数或者叫扩展运算符
    //   localStorage.setItem("CURRENTCOVER", JSON.stringify(newCover));
    // };
    const drawerTitle = (
        <div className="drawer-title">
            <a href={WEB_HOST} target="_blank">
                <Avatar src={logo} shape="square" style={{ marginRight: '12px' }} />
                <span className="site-title">CUP</span>
            </a>
        </div>
    );
    const footer = [
        { title: '切换封面', Component: PictureOutlined, function: () => UseState(true, drawerVisible) },
        { title: '项目详情', Component: GithubOutlined, function: () => window.open(PROJECT_GITHUB) },
        {
            title: '进入平台',
            Component: SendOutlined,
            function: () =>
                router.push({
                    name: 'DashBoard'
                })
        }
    ];
    const footrender = footer.map((item, index) => (
        <>
            <Tooltip title={item.title}>
                <Button
                    type={ghostClose.value ? 'primary' : 'ghost'}
                    size="small"
                    shape="round"
                    icon={<item.Component />} //jsx灵活性的体现
                    style={{ marginLeft: '8px' }}
                    onClick={item.function}
                ></Button>
            </Tooltip>
        </>
    ));

    function DrawerTabPanes(defaultTabPane) {
        return defaultTabPane.map((item, index) => {
            return (
                <Tabs.TabPane tab={item.name} key={item.key}>
                    <List
                        rowKey="id"
                        grid={listGrid}
                        dataSource={item.list}
                        renderItem={(cover, index) => {
                            return (
                                <List.Item key={index} style={{ marginBottom: 28 }}>
                                    <Card
                                        hoverable
                                        key={index}
                                        cover={
                                            <Image
                                                alt={cover.item.name}
                                                preview={false}
                                                placeholder
                                                // style={{height:'300px'}}
                                                src={cover.item.preview}
                                            />
                                        }
                                        bodyStyle={{ padding: 12, textAlign: 'center' }}
                                        onClick={() => changeCover(cover.item)}
                                    >
                                        {cover.item.name}
                                    </Card>
                                </List.Item>
                            );
                        }}
                    />
                </Tabs.TabPane>
            );
        });
    }
    const render = () => {
        return (
            <div className={`index${ghostClose.value ? '' : ' ghost'}`}>
                <div style={{ textAlign: 'center', margin: '104px 0 34px' }}>
                    <img
                        src={isBaidu.value ? baidu : google}
                        alt={isBaidu.value ? '百度' : '谷歌'}
                        className="search-logo"
                        onClick={() => UseState(!isBaidu.value, isBaidu)}
                    />
                </div>
                <div className="search-wrapper">
                    <Search
                        enterButton={
                            <Button type={ghostClose.value ? 'primary' : 'ghost'}>
                                <SearchOutlined />
                            </Button>
                        }
                        size="large"
                        className="search"
                        suffix={
                            <Tooltip title={`${searchAll.value ? '关闭' : '开启'}万能搜索`} placement="left">
                                <img
                                    className={`search-all-switch${searchAll ? '' : ' close'}`}
                                    src={logo}
                                    width={28}
                                    alt="万能搜索"
                                    onClick={toggleSearchAll}
                                />
                                {/*为什么toggleSearchAll这里不用写成箭头函数？因为这里没带参数*/}
                            </Tooltip>
                        }
                        onSearch={doSearch}
                        onFocus={() => UseState(true, maskOpened)}
                        onBlur={() => UseState(false, maskOpened)}
                    />
                </div>
                <div className={'card-wrapper' + (maskOpened.value ? ' hidden' : '')}>
                    {template.keyList.length > 0 && (
                        <Card className="card ant-card" bordered={false}>
                            {renderViewByTabKey}
                        </Card>
                    )}
                </div>
                <div className="fix-group">
                    <Switch
                        checkedChildren="白底"
                        unCheckedChildren="透明"
                        checked={ghostClose.value}
                        onChange={toggleGhost}
                    />
                    {footrender}
                </div>
                <Drawer
                    title={drawerTitle}
                    placement="top"
                    closable={true}
                    height={document.body.clientHeight}
                    headerStyle={{ padding: 16, background: 'rgba(0, 0, 0, 0.5)' }}
                    bodyStyle={{ background: '#f5f5f5' }}
                    visible={drawerVisible.value}
                    onClose={onClose}
                >
                    <div className="drawer-wrapper">
                        <Tabs defaultActiveKey="1" type="card" centered>
                            {DrawerTabPanes(defaultTabPane)}
                        </Tabs>
                    </div>
                </Drawer>
                <div className={'mask' + (maskOpened.value ? '' : ' hidden')} />
                {coverPage.type === 'iframe' ? (
                    <iframe className="bg" title="myIframe" src={coverPage.src} marginWidth={0} marginHeight={0} />
                ) : (
                    <img className="bg" src={coverPage.src} alt="bg" />
                )}
            </div>
        );
    };
    return render;
});
