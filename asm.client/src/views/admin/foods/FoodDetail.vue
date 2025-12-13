<template>
  <div class="container mt-4">
    <h2 class="fw-semibold mb-4">Chi tiết món ăn</h2>

    <div v-if="food" class="card p-3">
      <div class="row g-3">
        <div class="col-md-5">
          <img
            :src="`https://localhost:7119${food.imageUrl}`"
            class="img-fluid rounded"
            alt="food image"
            style="object-fit: cover; height: 250px; width: 100%"
          />
        </div>

        <div class="col-md-7 d-flex flex-column justify-content-center">
          <h5 class="card-title">{{ food.name }}</h5>
          <p class="card-text">{{ food.description }}</p>
          <p class="card-text fw-bold">Giá: {{ food.price.toLocaleString() }}₫</p>
        </div>
      </div>
    </div>

    <RouterLink to="/admin/foods" class="btn btn-secondary mt-3">Quay lại</RouterLink>
  </div>
</template>

<script setup lang="ts">
import { onMounted, ref } from "vue";
import { useRoute } from "vue-router";
import { useFoodStore } from "@/stores/foodStore";
import type { Food } from "@/types/food";

const store = useFoodStore();
const route = useRoute();
const food = ref<Food | null>(null);

onMounted(async () => {
  if (store.foods.length === 0) await store.fetchFoods();
  food.value = store.foods.find((f) => f.id === Number(route.params.id)) || null;
});
</script>
