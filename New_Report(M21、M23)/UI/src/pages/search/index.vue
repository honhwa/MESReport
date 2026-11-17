<script type="text/jsx">
import { Avatar, Col, Layout, Menu, Row, Input } from 'ant-design-vue';
import { useRoute } from 'vue-router';
// import {useLocation} from "@reach/router";
import { createFromIconfontCN } from '@ant-design/icons-vue';
import logo from '@/assets/images/logo.png';
import { WEB_HOST } from '../HomePage/constant';
import { defineComponent, isRef, ref, reactive } from '@vue/runtime-core';
const { Search } = Input;
import '../HomePage/index.css';
import defaultSearch,{iframeSearch} from '../HomePage/constant/defaultPage';

const IconFont = createFromIconfontCN({
    scriptUrl: '/iconfont.js'
});

export default defineComponent(() => {
    const route = useRoute();
    let question = route.params.q ? route.params.q : 'Vue';
    const handleClick = (e) => {
        setCurrent(e.key);
    };
    let current = reactive({ value: 'bing' });
    const setCurrent = (value) => {
        current.value = value;
    };
    let searchText = reactive({ value: question }); //reactive要包装复杂数据类型,如{},[]
    const setSearchText = (value) => {
        searchText.value = value;
    };
    // const searchParams = parse(localStorage.getItem())

    const toNav = () => {
        window.open(WEB_HOST);
    };
    const defaultSearchPage = defaultSearch.map((item, index) => {
        return (
            <Menu.Item key={item.station}>
                <IconFont type={item.type} />
                {item.title}
            </Menu.Item>
        );
    });
    const render = () => {
        return (
            <div>
                <Layout>
                    <Layout.Header style={{ zIndex: 10 }} classname="ant-design-headerss">
                        <Row align="middle" justify="center">
                            <Col xl={1} lg={2} md={2} sm={3} xs={4}>
                                <div onClick={toNav} style={{ cursor: 'pointer' }}>
                                    <Avatar shape="square" src={logo} />
                                </div>
                            </Col>
                            <Col xl={7} lg={8} md={10} sm={11} xs={20}>
                                <Search enterButton style={{ display: 'block' }} size="large" defaultValue={searchText.value} onSearch={(value) => setSearchText(value)} />
                            </Col>
                            <Col xl={16} lg={14} md={12} sm={10} xs={24}>
                                <Menu theme="dark" mode="horizontal" selectedKeys={[current.value]} onClick={handleClick} style={{ float: 'right' }}>
                                    {defaultSearchPage}
                                </Menu>
                            </Col>
                        </Row>
                    </Layout.Header>
                    {current.value === 'bing' && (
                        <iframe
                            src={`https://cn.bing.com/search?q=${searchText.value}`}
                            style={{ border: 'none', width: '100%', height: '100vh', position: 'fixed', top: '-30px', zIndex: 1 }}
                        />
                    )}
                    {current.value === '360' && (
                        <iframe
                            src={`https://www.so.com/s?q=${searchText.value}`}
                            style={{ border: 'none', width: '100%', height: '100vh', position: 'fixed', top: '-30px', zIndex: 1 }}
                        />
                    )}
                    {current.value === 'sogou' && (
                        <iframe
                            src={`https://www.sogou.com/web?query=${searchText.value}`}
                            style={{ border: 'none', width: '100%', height: '100vh', position: 'fixed', top: -30, zIndex: 1 }}
                        />
                    )}
                    {current.value === 'bilibili' && (
                        <iframe
                            src={`https://search.bilibili.com/all?keyword=${searchText.value}`}
                            style={{ border: 'none', width: '100%', height: '100vh', position: 'fixed', top: '-80px', zIndex: 1 }}
                        />
                    )}
                    {current.value === 'gitee' && (
                        <iframe
                            src={`https://search.gitee.com/?q=${searchText.value}`}
                            style={{ border: 'none', width: '100%', height: '100vh', position: 'fixed', top: '-50px', zIndex: 1 }}
                        />
                    )}
                </Layout>
            </div>
        );
    };
    return render;
});
</script>