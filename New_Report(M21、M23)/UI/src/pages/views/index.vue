<template>
    <a-card class="routerpage">
        <ul>
            <li v-for="item in menusData" :key="item.name">
                <a @click="clickMenuItem(item.path)">{{ item.meta.title }}</a>
            </li>
        </ul>
        <!-- <router-view></router-view> -->
    </a-card>
</template>

<script lang="ts" setup>
// @ts-nocheck
import { useRoute, useRouter } from 'vue-router';
import { watch, ref, reactive, onMounted } from 'vue';
import { useUserStore } from '@/store/module/user';
// import { GetCommon } from '@/api/pages/common';
// import { Storage } from '@/utils/Storage';
import type { RouteRecordRaw } from 'vue-router';
// import { cloneDeep } from 'lodash-es';

const route = useRoute();
const router = useRouter();
let roData: any = ref([]);
let useUserParams = useUserStore();
let menusData: any = reactive({ children: [] });

const loadTableData = async (params: string) => {
    let Data = useUserParams.routerPage;
    getRouter(Data, params);
};
function getRouter(Data: any[], params: string) {
    Data.forEach((item) => {
        if (item.path != params && item.children) {
            getRouter(item.children, params);
        } else {
            if (item.path == params) {
                menusData = item.children;
            }
        }
    });
}


const clickMenuItem = (key: string) => {
    if (key === route.path) return;
    if (/http(s)?:/.test(key)) {
        window.open(key);
    } else {
        router.push(key);
    }
};


watch(
    () => route?.fullPath,
    async (newValue) => {
        roData.value = await loadTableData(newValue);
    },
    {
        immediate: true, //立即執行一次
        deep: true
    }
);

</script>
<script lang="ts">
export default {
    name: 'MFGROUTERPAGE'
};
</script>
<style scoped>
.routerpage{
    color: rgb(92, 91, 184);
    padding: 20px;
    margin-top: 1%;
    display: flex;
    justify-content: space-between;
    font-size: 20px;
}

.buttonarray {
    width: 100px;
    border-radius: 5%;
    margin-left: 1px;
    background-color: #f6f6f6;
    position: relative;
    font-size: 15px;
}

.buttonarray:hover {
    z-index: 100;
    transform: scale(1.1);
    box-shadow: 0px 7px 21px 0 rgba(0, 0, 0, 0.1);
}
</style>
