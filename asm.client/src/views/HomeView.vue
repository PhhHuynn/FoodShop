<template>
  <div class="bg-light py-5">
    <div class="container">
      <div class="row align-items-center">
        <div class="col-md-6 text-center text-md-start mb-4 mb-md-0">
          <h1 class="display-4 fw-bold mb-3">
            Chào mừng đến với <span class="text-primary">CookStore</span>
          </h1>
          <p class="lead mb-4 text-secondary">
            Khám phá các món ăn ngon, tươi mới và được chế biến với tâm huyết. Đừng bỏ lỡ trải
            nghiệm ẩm thực tuyệt vời ngay hôm nay!
          </p>
          <RouterLink to="/menu" class="btn btn-primary btn-lg"> Xem menu </RouterLink>
        </div>
        <div class="col-md-6 text-center">
          <img style="height: 400px; width: auto" src="/img/hamburger.png" />
        </div>
      </div>
    </div>
  </div>
  <div class="container my-5">
    <div class="d-flex justify-content-between align-items-center mb-4">
      <h2 class="text-primary-custom mb-0">Special Combos</h2>
      <RouterLink to="/menu?isCombo=true" class="text-decoration-none text-primary fw-semibold">
        Xem thêm →
      </RouterLink>
      <div class="row col-4"></div>
    </div>
    <div class="container my-5">
      <div class="d-flex justify-content-between align-items-center mb-4">
        <h2 class="text-primary-custom mb-0">Our Products</h2>
        <RouterLink to="/menu" class="text-decoration-none text-primary fw-semibold">
          Xem thêm →
        </RouterLink>
      </div>
      <div class="row">
        <div class="row">
          <div v-for="food in foods" :key="food.id" class="col-6 col-md-3 mb-4">
            <CardProduct :food="food" />
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import CardProduct from "@/components/CardProduct.vue";
import { useFoodStore } from "@/stores/foodStore";
import { onMounted, computed } from "vue";

const foodStore = useFoodStore();

const foods = computed(() => foodStore.foods);

onMounted(async () => {
  foodStore.fetchFoods();
});
</script>
