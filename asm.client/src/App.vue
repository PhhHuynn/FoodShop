<script setup lang="ts">
// import ChatWidget from "./components/ChatWidget.vue";
import TheHeader from "./components/TheHeader.vue";
</script>

<template>
  <TheHeader />
  <div style="height: 70px" class="mb-5"></div>
  <div style="min-height: calc(100vh - 70px)">
    <router-view />
  </div>
  <!-- <ChatWidget /> -->
  <div class="bg-dark text-white text-center py-4 mt-5">
    &copy; 2025 CookStore. All Rights Reserved.
  </div>
</template>
