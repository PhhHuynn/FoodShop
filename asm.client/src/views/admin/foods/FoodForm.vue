<template>
  <div class="food-form container">
    <h2>{{ isEdit ? "Cập nhật món ăn" : "Thêm món ăn" }}</h2>

    <form @submit.prevent="handleSubmit">
      <div class="mb-3">
        <label>Tên món</label>
        <input v-model="form.name" class="form-control" required />
      </div>

      <div class="mb-3">
        <label>Giá</label>
        <input type="number" v-model.number="form.price" min="0" class="form-control" required />
      </div>

      <div class="mb-3">
        <label>Mô tả</label>
        <textarea v-model="form.description" class="form-control"></textarea>
      </div>

      <div class="mb-3">
        <label>Danh mục</label>
        <select v-model.number="form.categoryId" class="form-control" required>
          <option value="" disabled>Chọn danh mục</option>
          <option v-for="cat in categoryStore.categories" :key="cat.id" :value="cat.id">
            {{ cat.name }}
          </option>
        </select>
      </div>

      <div class="mb-3">
        <label>Ảnh</label>
        <input type="file" @change="onFileChange" class="form-control" accept="image/*" />
      </div>

      <div class="mb-4">
        <div class="form-check d-flex align-items-center">
          <input
            class="form-check-input checkbox-lg"
            type="checkbox"
            id="isAvailableCheckbox"
            v-model="form.isAvailable"
          />
          <label class="form-check-label ms-2 fw-medium" for="isAvailableCheckbox">
            Còn hàng
          </label>
        </div>
      </div>

      <button type="submit" class="btn btn-primary" :disabled="store.loading">
        {{ isEdit ? "Lưu thay đổi" : "Thêm mới" }}
      </button>
    </form>
  </div>
</template>

<script lang="ts" setup>
import { ref, computed, onMounted } from "vue";
import { useRoute } from "vue-router";
import { useFoodStore } from "@/stores/foodStore";
import { useCategoryStore } from "@/stores/categoryStore";
import { useRouter } from "vue-router";

const router = useRouter();

const store = useFoodStore();
const categoryStore = useCategoryStore();
const route = useRoute();
const isEdit = computed(() => !!Number(route.params.id));

const form = ref({
  id: 0,
  name: "",
  price: 0,
  description: "",
  imageUrl: "",
  categoryId: 0,
  isAvailable: true,
  fImageFile: null,
});

onMounted(async () => {
  await categoryStore.fetchCategories();

  if (isEdit.value) {
    const foodId = Number(route.params.id);
    const food = await store.fetchFood(foodId);
    if (food) {
      form.value = { ...food, fImageFile: null };
    }
  }
});

function onFileChange(event: Event) {
  const target = event.target as HTMLInputElement;
  if (target.files && target.files[0]) {
    form.value.fImageFile = target.files[0];
  }
}

const handleSubmit = async () => {
  try {
    if (isEdit.value) {
      await store.editFood(form.value.id, { ...form.value });
      alert("Cập nhật món ăn thành công!");
    } else {
      await store.addFood(form.value);
      alert("Thêm món ăn thành công!");
      form.value = {
        id: 0,
        name: "",
        price: 0,
        description: "",
        imageUrl: "",
        categoryId: 0,
        isAvailable: true,
        fImageFile: null,
      };
    }

    router.push("/admin/foods/");
  } catch (err) {
    console.error("Lỗi khi xử lý form:", err);
    alert("Có lỗi xảy ra: " + (err instanceof Error ? err.message : String(err)));
  }
};
</script>
