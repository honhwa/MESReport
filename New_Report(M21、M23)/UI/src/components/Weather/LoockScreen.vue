<template>
    <div class="LockScreen">
        <img class="bgimage" :src="WeatherParams.bgurl" @load="isLoading = false" />
        <div class="WeatherWrapper">
            <div id="loading" :hidden="!isLoading"></div>
            <div class="cont">
                <div class="one">
                    <h1>🌦️ Weather.io ☔️</h1>
                </div>
            </div>
            <div class="two">
                <div class="card">
                    <div class="search">
                        <input type="text" class="search-bar" placeholder="Search" v-model="WeatherParams.city" />
                        <button @click="getWeatherData(WeatherParams.city)">
                            <svg
                                stroke="currentColor"
                                fill="currentColor"
                                stroke-width="0"
                                viewBox="0 0 16 16"
                                height="1em"
                                width="1.5em"
                                xmlns="http://www.w3.org/2000/svg"
                            >
                                <path
                                    fill-rule="evenodd"
                                    d="M10.442 10.442a1 1 0 011.415 0l3.85 3.85a1 1 0 01-1.414 1.415l-3.85-3.85a1 1 0 010-1.415z"
                                    clip-rule="evenodd"
                                ></path>
                                <path
                                    fill-rule="evenodd"
                                    d="M6.5 12a5.5 5.5 0 100-11 5.5 5.5 0 000 11zM13 6.5a6.5 6.5 0 11-13 0 6.5 6.5 0 0113 0z"
                                    clip-rule="evenodd"
                                ></path>
                            </svg>
                        </button>
                    </div>
                    <div class="weather" :class="{ loading: isLoading }">
                        <!-- <div class="other">注意：请输入城市名拼音，首字母大写</div> -->
                        <h2 class="city">Weather in {{ WeatherParams.city }}</h2>
                        <h1 class="temp">{{ WeatherParams.temp }}°C</h1>
                        <div class="flex">
                            <img :src="WeatherParams.icon" alt="" class="icon" />
                            <div class="description">{{ WeatherParams.description }}</div>
                        </div>
                        <div class="other">
                            <div class="humidity">湿度: {{ WeatherParams.humidity }}%</div>
                            <div class="wind">风速: {{ WeatherParams.speed }} km/h</div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="three">
                <div class="name" @click="lockscreenStore.setLock(false)">点击解除锁定</div>
            </div>
        </div>
    </div>
</template>
<!--<script lang="ts">
export default {
    name: 'Weather'
};
</script>-->
<script setup lang="ts">
import { reactive, ref } from '@vue/reactivity';
import { onMounted } from '@vue/runtime-core';
import axios from 'axios';
import { useLockscreenStore } from '@/store/module/LockScreen';

const lockscreenStore = useLockscreenStore();
const WeatherParams = reactive({
    city: 'Foshan',
    temp: '',
    humidity: '',
    speed: '',
    apiKey: '13e786b2255800d48474a6fdbcd071dc', //抄的别人的key,不知道后面会不会被禁掉。。
    description: '',
    icon: '',
    bgurl: ''
});
const isLoading = ref<boolean>(true);
const getWeatherData = async (city: string) => {
    return await new Promise((resolve, reject) => {
        axios
            .get(`https://api.openweathermap.org/data/2.5/weather?q=${city}&units=metric&appid=${WeatherParams.apiKey}`)
            .then((data) => resolve(data.data))
            .catch((error) => {
                reject(error);
            });
    });
};

const displayWeather = (data: any) => {
    //赋值天气数据
    const { icon, description } = data.weather[0];
    WeatherParams.speed = data.wind.speed;
    WeatherParams.temp = data.main.temp;
    WeatherParams.humidity = data.main.humidity;
    WeatherParams.description = description;
    WeatherParams.city = data.name; //城市名
    WeatherParams.icon = `https://openweathermap.org/img/wn/${icon}.png`; //天气的接口
    WeatherParams.bgurl = `https://source.unsplash.com/1600x900/?city=${WeatherParams.city} `; //获取城市图片的接口
    // setInterval(() => (isLoading.value = false), 2000);
    // isLoading.value = false;
};
onMounted(async () => {
    let WeatherData = await getWeatherData(WeatherParams.city);
    displayWeather(WeatherData);
    // setInterval(() => isLoading.value = false, 2000);
});
</script>
<style src="./index.css" scoped></style>
