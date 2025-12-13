<script setup lang="ts">
import ChatWidget from "./components/ChatWidget.vue";
import TheHeader from "./components/TheHeader.vue";
import { useAuthStore } from "./stores/authStore";
const auth = useAuthStore();
</script>

<template>
  <TheHeader />
  <div style="height: 70px" class="mb-5"></div>
  <div style="min-height: calc(100vh - 70px)">
    <router-view />
  </div>
  <div v-if="auth.token && auth.user && auth.user.role !== 'Admin'">
    <ChatWidget />
  </div>
  <div class="bg-dark text-white text-center py-4 mt-5">
    &copy; 2025 CookStore. All Rights Reserved.
  </div>
</template>
