<template>
  <div class="bg-light py-5" style="margin-top: -70px">
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
      <ul class="nav nav-underline me-4">
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

      <button class="btn btn-sm btn-outline-secondary" @click="showFilter = !showFilter">
        {{ showFilter ? "Ẩn bộ lọc" : "Bộ Lọc" }}
      </button>
    </div>

    <div
      v-if="showFilter"
      class="border rounded p-3 mb-4 shadow-sm bg-white d-flex flex-wrap gap-3 align-items-end mx-auto"
      style="max-width: 800px"
    >
      <div class="flex-grow-1" style="min-width: 150px">
        <label for="filterNameInput" class="form-label mb-1">Tên:</label>
        <input
          id="filterNameInput"
          v-model="filterName"
          class="form-control form-control-sm"
          placeholder="Tìm theo tên"
        />
      </div>

      <div class="flex-grow-1" style="min-width: 150px">
        <label for="filterPriceInput" class="form-label mb-1">Giá tối đa:</label>
        <input
          id="filterPriceInput"
          type="number"
          v-model.number="filterPrice"
          class="form-control form-control-sm"
          placeholder="Nhập giá tối đa"
        />
      </div>

      <button class="btn btn-warning btn-sm" @click="applyFilter">Áp dụng</button>

      <button class="btn btn-outline-secondary btn-sm" @click="clearFilter">Xóa Lọc</button>
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
import type { Food } from "@/types/food";
import type { Combo } from "@/types/combo";

interface Item extends Food, Combo {
  id: number;
  name: string;
  price: number;
  isAvailable: boolean;
}

const foodStore = useFoodStore();
const comboStore = useComboStore();
const categoryStore = useCategoryStore();

const selectedCategory = ref<number | "all" | "combo">("all");
const showFilter = ref(false);

const filterName = ref("");
const filterPrice = ref<number | null>(null);

const appliedFilterName = ref("");
const appliedFilterPrice = ref<number | null>(null);

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

const applyFilter = () => {
  appliedFilterName.value = filterName.value;
  appliedFilterPrice.value = filterPrice.value;
};

const clearFilter = () => {
  filterName.value = "";
  filterPrice.value = null;
  applyFilter();
};

const filteredItems = computed<Item[]>(() => {
  let items: Item[] = [];

  if (selectedCategory.value === "all") {
    items = [...foodStore.foods, ...comboStore.comboes] as Item[];
  } else if (selectedCategory.value === "combo") {
    items = comboStore.comboes as Item[];
  } else if (typeof selectedCategory.value === "number") {
    items = foodStore.foods.filter((f) => f.categoryId === selectedCategory.value) as Item[];
  }

  if (appliedFilterName.value) {
    const nameFilter = appliedFilterName.value.toLowerCase();
    items = items.filter((item) => item.name.toLowerCase().includes(nameFilter));
  }

  if (appliedFilterPrice.value !== null && appliedFilterPrice.value >= 0) {
    items = items.filter((item) => item.price <= appliedFilterPrice.value!);
  }

  return items.filter((item) => item.isAvailable);
});
</script>
