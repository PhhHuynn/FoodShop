<template>
  <div class="container mt-4">
    <h2 class="fw-semibold mb-4">Chi tiết combo</h2>

    <div v-if="combo" class="card p-3">
      <div class="row g-3">
        <!-- Cột hình ảnh -->
        <div class="col-md-5">
          <img
            :src="`https://localhost:7108/${combo.imageUrl}`"
            class="img-fluid rounded"
            :alt="combo.name"
            style="object-fit: cover; height: 330px; width: 100%"
          />
        </div>

        <!-- Cột thông tin combo -->
        <div class="col-md-7 d-flex flex-column justify-content-center">
          <h3 class="card-title mb-3">{{ combo.name }}</h3>
          <p class="card-text text-muted">{{ combo.description }}</p>

          <hr />

          <h5 class="fw-bold mb-3">Thành phần combo:</h5>
          <ul class="list-group list-group-flush mb-4">
            <li
              v-for="comboFood in combo.comboFoods"
              :key="comboFood.foodId"
              class="list-group-item d-flex justify-content-between align-items-center"
            >
              <span class="fw-medium">{{ comboFood.food.name }}</span>
              <span class="badge bg-primary rounded-pill">x {{ comboFood.quantity }}</span>
            </li>
            <li v-if="combo.comboFoods.length === 0" class="list-group-item text-muted">
              Combo này hiện chưa có món ăn nào.
            </li>
          </ul>

          <hr />

          <p class="card-text fw-bold fs-4 text-danger">Giá: {{ combo.price.toLocaleString() }}₫</p>
        </div>
      </div>
    </div>

    <div v-else class="alert alert-warning">Không tìm thấy chi tiết combo.</div>

    <RouterLink to="/admin/comboes" class="btn btn-secondary mt-3">Quay lại</RouterLink>
  </div>
</template>

<script setup lang="ts">
import { onMounted, ref } from "vue";
import { useRoute } from "vue-router";
import { useComboStore } from "@/stores/comboStore";
import { type Combo } from "@/types/combo";

const store = useComboStore();
const route = useRoute();
const combo = ref<Combo | undefined>(undefined);

onMounted(async () => {
  const fetchData: Combo | undefined = await store.fetchCombo(Number(route.params.id));
  if (fetchData) {
    combo.value = fetchData;
    console.log(combo.value);
  } else {
    console.warn(`Không tìm thấy combo với ID: ${route.params.id}`);
  }
});
</script>
