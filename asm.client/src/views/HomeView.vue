<template>
  <div class="bg-light py-5">
    <div class="container">
      <div class="row align-items-center">
        <div class="col-md-6 text-center text-md-start mb-4 mb-md-0">
          <h1 class="display-4 fw-bold mb-3">
            Chào mừng đến với <span class="text-warning">CookStore</span>
          </h1>
          <p class="lead mb-4 text-secondary">
            Khám phá các món ăn ngon, tươi mới và được chế biến với tâm huyết. Đừng bỏ lỡ trải
            nghiệm ẩm thực tuyệt vời ngay hôm nay!
          </p>
          <div class="d-flex flex-column flex-sm-row gap-3">
            <div
              class="stat-box border rounded-4 p-3 d-flex flex-column align-items-center shadow-sm animate"
            >
              <h3 class="fw-bold text-warning mb-0">10.000+</h3>
              <p class="text-secondary mb-0">Khách hàng hài lòng</p>
            </div>

            <div
              class="stat-box border rounded-4 p-3 d-flex flex-column align-items-center shadow-sm animate"
            >
              <h3 class="fw-bold text-warning mb-0">100%</h3>
              <p class="text-secondary mb-0">Nguyên liệu tươi sạch</p>
            </div>

            <div
              class="stat-box border rounded-4 p-3 d-flex flex-column align-items-center shadow-sm animate"
            >
              <h3 class="fw-bold text-warning mb-0">5★</h3>
              <p class="text-secondary mb-0">Cam kết chất lượng</p>
            </div>
          </div>
        </div>

        <div class="col-md-6 text-center">
          <img style="height: 400px; width: auto" src="/img/hamburger.png" />
        </div>
      </div>
    </div>
  </div>

  <div class="container my-5">
    <div class="d-flex justify-content-center align-items-center mb-4">
      <ul class="nav nav-underline mb-3">
        <li class="nav-item">
          <a class="text-dark nav-link" href="#" @click.prevent="selectCategory('all')"> All </a>
        </li>

        <li class="nav-item">
          <a class="text-dark nav-link" href="#" @click.prevent="selectCategory('combo')">
            Combo
          </a>
        </li>
        <li class="nav-item" v-for="cat in categories" :key="cat.id">
          <a class="text-dark nav-link" href="#" @click.prevent="selectCategory(cat.id)">
            {{ cat.name }}
          </a>
        </li>
      </ul>
    </div>

    <div
      v-if="showFilter"
      class="border rounded p-3 mb-4 shadow-sm bg-white"
      style="position: absolute; z-index: 50"
    >
      <div class="mb-2">
        <label>Tên:</label>
        <input
          v-model="filterName"
          class="form-control form-control-sm"
          placeholder="Tìm theo tên"
        />
      </div>
      <div class="mb-2">
        <label>Giá tối đa:</label>
        <input
          type="number"
          v-model.number="filterPrice"
          class="form-control form-control-sm"
          placeholder="Nhập giá tối đa"
        />
      </div>
      <button class="btn btn-warning btn-sm mt-2" @click="applyFilter">Áp dụng</button>
    </div>

    <div class="row mt-4">
      <div v-for="item in filteredItems" :key="item.id" class="col-6 col-md-3 mb-4">
        <CardProduct :item="item" />
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from "vue";
import CardProduct from "@/components/CardProduct.vue";
import { useFoodStore } from "@/stores/foodStore";
import { useCategoryStore } from "@/stores/categoryStore";
import type { Category } from "@/types/category";
import { useComboStore } from "@/stores/comboStore";

const foodStore = useFoodStore();
const comboStore = useComboStore();
const categoryStore = useCategoryStore();

const selectedCategory = ref<number | "all" | "combo">("all");
const showFilter = ref(false);
const filterName = ref("");
const filterPrice = ref<number | null>(null);
const categories = ref<Category[]>([]);

onMounted(async () => {
  await categoryStore.fetchCategories();
  categories.value = categoryStore.categories;

  await foodStore.fetchFoods();
  await comboStore.fetchComboes();
});

const selectCategory = (cat: number | "all" | "combo") => {
  selectedCategory.value = cat;
};

const filteredItems = computed(() => {
  if (selectedCategory.value === "all") {
    return [...foodStore.foods, ...comboStore.comboes];
  } else if (selectedCategory.value === "combo") {
    return comboStore.comboes.filter((c) => c.isAvailable);
  } else if (typeof selectedCategory.value === "number") {
    return foodStore.foods.filter((f) => f.categoryId === selectedCategory.value);
  }
  return [];
});

const applyFilter = () => {
  showFilter.value = false;
};
</script>
