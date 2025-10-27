<template>
  <div class="container mt-4">
    <div class="d-flex justify-content-between align-items-center mb-3">
      <h2 class="fw-semibold">Quản lý danh mục</h2>
      <RouterLink to="/admin/categories/add" class="btn btn-warning">+ Thêm danh mục</RouterLink>
    </div>

    <table class="table table-striped table-bordered align-middle">
      <thead class="table-light">
        <tr>
          <th>ID</th>
          <th>Tên danh mục</th>
          <th style="width: 200px">Hành động</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="category in store.categories" :key="category.id">
          <td>{{ category.id }}</td>
          <td>{{ category.name }}</td>
          <td>
            <div class="d-flex gap-2">
              <RouterLink :to="`/admin/categories/${category.id}`" class="btn btn-sm btn-secondary"
                >Xem</RouterLink
              >
              <RouterLink
                :to="`/admin/categories/edit/${category.id}`"
                class="btn btn-sm btn-primary"
                >Sửa</RouterLink
              >
              <button
                class="btn btn-sm btn-danger"
                data-bs-toggle="modal"
                data-bs-target="#deleteConfirm"
                @click="setSelectedCategory(category)"
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
              Bạn có chắc muốn xóa danh mục
              <strong>{{ selectedCategory?.name }}</strong>
              không?
            </p>
          </div>
          <div class="modal-footer justify-content-center">
            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Hủy</button>
            <button
              type="button"
              class="btn btn-danger"
              data-bs-dismiss="modal"
              @click="deleteCategory"
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
import { useCategoryStore } from "@/stores/categoryStore";
import { RouterLink } from "vue-router";
import type { Category } from "@/types/category";

const store = useCategoryStore();
const selectedCategory = ref<Category | null>(null);

onMounted(() => {
  store.fetchCategories();
});

function setSelectedCategory(category: Category) {
  selectedCategory.value = category;
}

async function deleteCategory() {
  if (selectedCategory.value) {
    await store.removeCategory(selectedCategory.value.id);
    selectedCategory.value = null;
  }
}
</script>
