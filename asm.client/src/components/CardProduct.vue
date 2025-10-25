<template>
  <div class="card-food card h-100 border rounded-4 shadow-sm">
    <img
      :src="food.imageUrl"
      class="card-img-top object-fit-cover"
      :alt="food.name"
      style="height: 300px"
    />
    <div class="card-body text-center d-flex flex-column justify-content-between gap-4">
      <div>
        <h5 class="card-title">{{ food.name }}</h5>
        <p class="card-text fw-bold text-danger">{{ food.price }}₫</p>
        <p class="card-text">{{ food.description }}</p>
      </div>
      <button @click="addToCart(food, 'food')" class="btn btn-outline-primary">
        Thêm vào giỏ hàng
      </button>
    </div>
  </div>
</template>

<script setup lang="ts">
import { defineProps } from "vue";
import { type Food } from "@/api/foodService";

defineProps<{ food: Food }>();

function addToCart(item: Food, type: string) {
  console.log("Add", type, item.name);
}
</script>
