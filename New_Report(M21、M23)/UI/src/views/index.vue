<template>
  <div class="wrapper">
    <Answer></Answer>
    <div class="content-box">
      <div class="content">
        <router-view #="{ Component }">
          <keep-alive>
            <component :is="Component" />
          </keep-alive>
        </router-view>
      </div>
    </div>
    <img className="bg" src="http://10.189.127.62:2022/start//pool/cover_2.jpg" alt="bg"  />
  </div>
</template>

<script setup>
import Answer from "@/components/Answer/index.vue";
</script>

<style scoped>
.nav {
  text-align: center;
}
.bg {
  width: 100%;
  height: 100%;
  position: fixed;
  top: 0;
  left: 0;
  float: left;
  border: 0;
  overflow: hidden;
  z-index: -100;
}

.content-box {
  position: absolute;
  left: 250px;
  right: 0;
  top: 0px;
  bottom: 0;
  padding-bottom: 30px;
  -webkit-transition: left 0.3s ease-in-out;
  transition: left 0.3s ease-in-out;
  background: #f0f0f0;
}

.content {
  width: auto;
  height: 100%;
  padding: 10px;
  overflow-y: scroll;
  box-sizing: border-box;
}

.content-collapse {
  left: 64px;
  /* left: 0px; */
}
.wrapper {
  width: 100%;
  height: 100%;
  overflow: hidden;
  user-select: none; /*不允许复制 */
}
</style>
