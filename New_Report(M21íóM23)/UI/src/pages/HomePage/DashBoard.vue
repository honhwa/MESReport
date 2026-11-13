<template>
    <div>
        <a-card class="routerpage">
            <div class="flip-card-inner">
                <div class="flip-card-front">
                    <p class="title">Favorite Page</p>
                </div>
                <div class="flip-card-back">
                    <p class="title">右鍵可取消收藏</p>
                </div>
            </div>
            <ol>
                <li v-for="item in FavoriteData" :key="item['ID']" class="pagename">
                    <a @click="clickMenuItem(item['ID'])" @contextmenu="handleContext($event, item)">
                        {{ item['ROUTE'] }}-{{ item['REP_NAME'] }}</a
                    >
                </li>
            </ol>
        </a-card>
    </div>
</template>

<script lang="ts">
export default {
    name: 'DashBoard'
};
</script>
<script lang="ts" setup>
import { useRoute, useRouter } from 'vue-router';
import { Storage } from '@/utils/Storage';
import { GetCommon } from '@/api/pages/common';
import { ref, watch } from 'vue';
import { useContextMenu } from '@/utils/helper/ContextMenuHelper';
import { message, Alert } from 'ant-design-vue';

const route = useRoute();
const router = useRouter();
const [createContextMenu] = useContextMenu();
let FavoriteData = ref([]);
const UserParams = Storage.get('UserMessage');
/**获取我的最爱明细 */
const GetFavorite = async () => {
    let Params = { apiName: 'Favorite_Show', EMP_ID: UserParams['MemID'] };
    let { api01 } = await GetCommon(Params);
    return api01['Data']['Table'];
};
/**删除我的最爱 */
const DeleteFavorite = async (item: any) => {
    let Params = {
        EMP_ID: UserParams['MemID'],
        EMP_NAME: UserParams['MemName'],
        ROUTE: item.ROUTE ?? '',
        ID: item.ID ?? '',
        REP_NAME: item.REP_NAME ?? '',
        SYSMODULE: item.SYSMODULE ?? '',
        apiName: 'Favorite_Delete'
    };
    let { api01 } = await GetCommon(Params);
    if (api01 == 'Success!') message.success('取消收藏成功!');
    else message.warning('取消收藏失敗!');
};
/**路由跳转 */
const clickMenuItem = (key: string) => {
    if (key === route.name) return;
    else router.push({ name: key });
};
/**右键点击触发 */
function handleContext(e: MouseEvent, item) {
    createContextMenu({
        event: e,
        items: [
            {
                label: '取消收藏',
                icon: 'juxing-cuowu',
                handler: async () => {
                    await DeleteFavorite(item);
                    FavoriteData.value = await GetFavorite();
                }
            }
        ]
    });
}

watch(
    () => route?.fullPath,
    async (newValue) => {
        let Data = await GetFavorite();
        FavoriteData.value = Data.map((item) => {
            item['ROUTE'] = `${item['ROUTE'].split('/')[2]}-${item['ROUTE'].split('/')[3]}`;
            return item;
        });
        // FavoriteData.value = await GetFavorite();
    },
    {
        immediate: true, //立即執行一次
        deep: true
    }
);
</script>

<style scoped lang="less">
.routerpage {
    color: rgb(92, 91, 184);
    /* padding: 20px; */
    margin-top: 1%;
    justify-content: space-between;
    font-size: 20px;
    border-radius: 20px;
    background: #f5f5f5;
    position: relative;
    padding: 1.8rem;
    border: 2px solid #c3c6ce;
    transition: 0.5s ease-out;
    overflow: visible;
    height: 78vh;

    &:hover {
        border-color: #008bf8;
        box-shadow: 0 4px 18px 0 rgba(0, 0, 0, 0.25);
    }
    ol {
        margin-top: 2%;
    }

    .title {
        font-size: 1.5em;
        font-weight: 900;
        text-align: center;
        margin: 0;
    }

    .flip-card-inner {
        position: relative;
        width: 100%;
        height: 100%;
        text-align: center;
        transition: transform 0.8s;
        transform-style: preserve-3d;
    }

    .flip-card-inner:hover {
        transform: rotateY(180deg);
    }

    .flip-card-front,
    .flip-card-back {
        box-shadow: 0 8px 14px 0 rgba(0, 0, 0, 0.2);
        position: absolute;
        display: flex;
        flex-direction: column;
        justify-content: center;
        width: 100%;
        height: 100%;
        -webkit-backface-visibility: hidden;
        backface-visibility: hidden;
        border-radius: 1rem;
    }

    .flip-card-front {
        background: linear-gradient(
            120deg,
            bisque 60%,
            rgb(255, 231, 222) 88%,
            rgb(255, 211, 195) 40%,
            rgba(255, 127, 80, 0.603) 48%
        );
        color: coral;
    }

    .flip-card-back {
        background: linear-gradient(120deg, rgb(255, 174, 145) 30%, coral 88%, bisque 40%, rgb(255, 185, 160) 78%);
        color: coral;
        transform: rotateY(180deg);
    }
    .pagename a {
        text-decoration: none;
        line-height: 34px;
        position: relative;
        display: inline-block;
    }
    .pagename a::before {
        content: '';
        display: block;
        position: absolute;
        height: 2px;
        background: rgb(92, 91, 184);
        width: 0;
        left: auto; /* left 和 right 的值是关键 */
        right: 0;
        bottom: 0;
        transition: all 0.5s;
    }
    .pagename a:hover::before {
        width: 100%;
        left: 0; /* left 和 right 的值是关键 */
        right: auto;
    }
}
</style>
