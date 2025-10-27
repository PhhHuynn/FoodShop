<template>
  <div class="container mt-4">
    <div class="d-flex justify-content-between align-items-center mb-3">
      <h2 class="fw-semibold">Quản lý món ăn</h2>
      <RouterLink to="/admin/foods/add" class="btn btn-warning">+ Thêm món ăn</RouterLink>
    </div>

    <table class="table table-striped table-bordered align-middle">
      <thead class="table-light">
        <tr>
          <th>Ảnh</th>
          <th>Tên món</th>
          <th>Mô tả</th>
          <th>Giá (₫)</th>
          <th style="width: 200px">Hành động</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="food in store.foods" :key="food.id">
          <td class="text-center">
            <img
              :src="`https://localhost:7108/${food.imageUrl}`"
              alt="food image"
              class="rounded"
              style="width: 60px; height: 60px; object-fit: cover"
            />
          </td>
          <td>{{ food.name }}</td>
          <td>{{ food.description }}</td>
          <td>{{ food.price.toLocaleString() }}</td>
          <td>
            <div class="d-flex gap-2">
              <RouterLink :to="`/admin/foods/${food.id}`" class="btn btn-sm btn-secondary"
                >Xem</RouterLink
              >
              <RouterLink :to="`/admin/foods/edit/${food.id}`" class="btn btn-sm btn-primary"
                >Sửa</RouterLink
              >
              <button
                class="btn btn-sm btn-danger"
                data-bs-toggle="modal"
                data-bs-target="#deleteConfirm"
                @click="setSelectedFood(food)"
              >
                Xóa
              </button>
            </div>
          </td>
        </tr>
      </tbody>
    </table>

    <!-- Modal xác nhận xóa -->
    <div
      class="modal fade"
      id="deleteConfirm"
      tabindex="-1"
      aria-labelledby="deleteConfirmLabel"
      aria-hidden="true"
    >
      <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title" id="deleteConfirmLabel">Xác nhận xóa</h5>
            <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
          </div>
          <div class="modal-body text-center">
            <p>
              Bạn có chắc muốn xóa món
              <strong>{{ selectedFood?.name }}</strong>
              không?
            </p>
          </div>
          <div class="modal-footer justify-content-center">
            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Hủy</button>
            <button
              type="button"
              class="btn btn-danger"
              data-bs-dismiss="modal"
              @click="deleteFood"
            >
              Xóa
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from "vue";
import { useFoodStore } from "@/stores/foodStore";
import { RouterLink } from "vue-router";
import type { Food } from "@/types/food";

const store = useFoodStore();
const selectedFood = ref<Food | null>(null);

onMounted(() => {
  store.fetchFoods();
});

function setSelectedFood(food: Food) {
  selectedFood.value = food;
}

async function deleteFood() {
  if (selectedFood.value) {
    await store.removeFood(selectedFood.value.id);
    selectedFood.value = null;
  }
}
</script>
