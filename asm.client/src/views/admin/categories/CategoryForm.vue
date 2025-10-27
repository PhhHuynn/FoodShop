<template>
  <div class="category-form container">
    <h2>{{ isEdit ? "Cập nhật danh mục" : "Thêm danh mục" }}</h2>

    <form @submit.prevent="handleSubmit">
      <div class="mb-3">
        <label>Tên danh mục</label>
        <input v-model="form.name" class="form-control" required />
      </div>

      <button type="submit" class="btn btn-primary" :disabled="store.loading">
        {{ isEdit ? "Lưu thay đổi" : "Thêm mới" }}
      </button>
    </form>
  </div>
</template>

<script lang="ts" setup>
import { ref, computed, onMounted } from "vue";
import { useRoute, useRouter } from "vue-router";
import { useCategoryStore } from "@/stores/categoryStore";
import type { Category } from "@/types/category";

const store = useCategoryStore();
const route = useRoute();
const router = useRouter();

const isEdit = computed(() => !!Number(route.params.id));

const form = ref<Category>({
  id: 0,
  name: "",
});

onMounted(async () => {
  if (isEdit.value) {
    const id = Number(route.params.id);
    const category = await store.fetchCategory(id);
    if (category) {
      form.value = { ...category };
    }
  }
});

const handleSubmit = async () => {
  try {
    console.log(form.value);
    if (isEdit.value) {
      await store.editCategory(form.value.id, { ...form.value });
      alert("Cập nhật danh mục thành công!");
    } else {
      await store.addCategory({ name: form.value.name });
      alert("Thêm danh mục thành công!");
      form.value.name = "";
    }

    router.push("/admin/categories");
  } catch (err) {
    console.error("Lỗi khi xử lý form:", err);
    alert("Có lỗi xảy ra: " + (err instanceof Error ? err.message : String(err)));
  }
};
</script>
