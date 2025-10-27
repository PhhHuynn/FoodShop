<template>
  <div class="container mt-4">
    <h2 class="fw-semibold mb-4">Chi tiết danh mục</h2>

    <div v-if="category" class="card p-3">
      <h5 class="card-title">{{ category.name }}</h5>
      <p class="card-text">ID: {{ category.id }}</p>
    </div>

    <RouterLink to="/admin/categories" class="btn btn-secondary mt-3">Quay lại</RouterLink>
  </div>
</template>

<script setup lang="ts">
import { onMounted, ref } from "vue";
import { useRoute } from "vue-router";
import { useCategoryStore } from "@/stores/categoryStore";
import type { Category } from "@/types/category";

const store = useCategoryStore();
const route = useRoute();
const category = ref<Category | null>(null);

onMounted(async () => {
  if (store.categories.length === 0) await store.fetchCategories();
  category.value = store.categories.find((c) => c.id === Number(route.params.id)) || null;
});
</script>
