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
        <tr v-for="Combo in store.comboes" :key="Combo.id" :class="{ italic: Combo.deletedAt }">
          <td class="text-center">
            <img
              :class="{ 'img-gray': Combo.deletedAt !== null }"
              :src="`https://localhost:7119/${Combo.imageUrl}`"
              alt="Combo image"
              class="rounded"
              style="width: 60px; height: 60px; object-fit: cover"
            />
          </td>
          <td>{{ Combo.name }} <span v-if="Combo.deletedAt">[Deleted]</span></td>
          <td>{{ Combo.description }}</td>
          <td>{{ Combo.price.toLocaleString() }}</td>
          <td>
            <span class="badge" :class="Combo.isAvailable ? 'bg-success' : 'bg-secondary'">
              {{ Combo.isAvailable ? "Còn hàng" : "Hết hàng" }}
            </span>
          </td>

          <td>
            <div class="d-flex gap-2">
              <RouterLink :to="`/admin/comboes/${Combo.id}`" class="btn btn-sm btn-secondary"
                >Xem</RouterLink
              >
              <template v-if="Combo.deletedAt == null">
                <RouterLink :to="`/admin/comboes/edit/${Combo.id}`" class="btn btn-sm btn-primary"
                  >Sửa</RouterLink
                >
                <button
                  class="btn btn-sm btn-danger"
                  data-bs-toggle="modal"
                  data-bs-target="#deleteConfirm"
                  @click="setSelectedCombo(Combo)"
                >
                  Xóa
                </button>
              </template>
              <button
                v-else
                class="btn btn-sm btn-success"
                data-bs-toggle="modal"
                data-bs-target="#restoreConfirm"
                @click="setSelectedCombo(Combo)"
              >
                Khôi phục
              </button>
            </div>
          </td>
        </tr>
      </tbody>
    </table>

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
              @click="deleteCombo"
            >
              Xóa
            </button>
          </div>
        </div>
      </div>
    </div>

    <!-- Modal khôi phục -->
    <div
      class="modal fade"
      id="restoreConfirm"
      tabindex="-1"
      aria-labelledby="restoreConfirmLabel"
      aria-hidden="true"
    >
      <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title" id="restoreConfirmLabel">Xác nhận khôi phục</h5>
            <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
          </div>
          <div class="modal-body text-center">
            <p>
              Bạn có chắc muốn khôi phục món
              <strong>{{ selectedFood?.name }}</strong>
              không?
            </p>
          </div>
          <div class="modal-footer justify-content-center">
            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Hủy</button>
            <button
              type="button"
              class="btn btn-success"
              data-bs-dismiss="modal"
              @click="restoreCombo"
            >
              Khôi phục
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

function setSelectedCombo(Combo: Combo) {
  selectedFood.value = Combo;
}

async function deleteCombo() {
  if (selectedFood.value) {
    await store.removeCombo(selectedFood.value.id);
    selectedFood.value = null;
  }
}

async function restoreCombo() {
  if (selectedFood.value) {
    await store.restoreComboById(selectedFood.value.id);
    selectedFood.value = null;
  }
}
</script>
