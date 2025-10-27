<template>
  <div class="container mt-4">
    <h2 class="fw-semibold mb-4">Chi tiết người dùng</h2>

    <div v-if="user" class="card p-3">
      <h5 class="card-title">{{ user.fullName }}</h5>
      <p class="card-text">ID: {{ user.id }}</p>
      <p class="card-text">Email: {{ user.email }}</p>
      <p class="card-text">Address: {{ user.address || "-" }}</p>
      <p class="card-text">Status: {{ user.status ? "Active" : "Inactive" }}</p>
      <p class="card-text">Role: {{ user.role || "-" }}</p>
    </div>

    <RouterLink to="/admin/users" class="btn btn-secondary mt-3">Quay lại</RouterLink>
  </div>
</template>

<script setup lang="ts">
import { onMounted, ref } from "vue";
import { useRoute } from "vue-router";
import { useUserStore } from "@/stores/userStore";
import type { User } from "@/types/user";

const store = useUserStore();
const route = useRoute();
const user = ref<User | null>(null);

onMounted(async () => {
  if (store.users.length === 0) await store.fetchUsers();
  user.value = store.users.find((u) => u.id === String(route.params.id)) || null;
});
</script>
