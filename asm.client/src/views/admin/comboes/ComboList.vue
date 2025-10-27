<template>
  <div class="container mt-4">
    <div class="d-flex justify-content-between align-items-center mb-3">
      <h2 class="fw-semibold">Quản lý combo</h2>
      <RouterLink to="/admin/comboes/add" class="btn btn-warning">+ Thêm combo</RouterLink>
    </div>

    <table class="table table-striped table-bordered align-middle">
      <thead class="table-light">
        <tr>
          <th>Ảnh</th>
          <th>Tên combo</th>
          <th>Mô tả</th>
          <th>Giá (₫)</th>
          <th>Đang bán</th>
          <th style="width: 200px">Hành động</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="Combo in store.comboes" :key="Combo.id">
          <td class="text-center">
            <img
              :src="`https://localhost:7108/${Combo.imageUrl}`"
              alt="Combo image"
              class="rounded"
              style="width: 60px; height: 60px; object-fit: cover"
            />
          </td>
          <td>{{ Combo.name }}</td>
          <td>{{ Combo.description }}</td>
          <td>{{ Combo.price.toLocaleString() }}</td>
          <td>{{ Combo.isAvailable }}</td>
          <td>
            <div class="d-flex gap-2">
              <RouterLink :to="`/admin/comboes/${Combo.id}`" class="btn btn-sm btn-secondary"
                >Xem</RouterLink
              >
              <RouterLink :to="`/admin/comboes/edit/${Combo.id}`" class="btn btn-sm btn-primary"
                >Sửa</RouterLink
              >
              <button
                class="btn btn-sm btn-danger"
                data-bs-toggle="modal"
                data-bs-target="#deleteConfirm"
                @click="setSelectedFood(Combo)"
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
import { useComboStore } from "@/stores/comboStore";
import { RouterLink } from "vue-router";
import type { Combo } from "@/types/combo";

const store = useComboStore();
const selectedFood = ref<Combo | null>(null);

onMounted(() => {
  store.fetchComboes();
});

function setSelectedFood(Combo: Combo) {
  selectedFood.value = Combo;
}

async function deleteFood() {
  if (selectedFood.value) {
    await store.removeCombo(selectedFood.value.id);
    selectedFood.value = null;
  }
}
</script>
