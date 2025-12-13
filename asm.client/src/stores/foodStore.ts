import {
  createFood,
  deleteFood,
  getFoods,
  getFood,
  updateFood,
  restoreFood,
  getActiveFoods,
} from "@/api/foodService";
import { defineStore } from "pinia";
import { ref } from "vue";
import { type Food, type FoodCreateOrUpdate } from "@/types/food";

export const useFoodStore = defineStore("food", () => {
  const foods = ref<Food[]>([]);
  const loading = ref(false);

  async function fetchFoods() {
    loading.value = true;
    try {
      foods.value = await getFoods();
    } catch (err) {
      console.error("Lỗi khi tải foods: ", err);
    } finally {
      loading.value = false;
    }
  }

  async function fetchActiveFoods() {
    loading.value = true;
    try {
      foods.value = await getActiveFoods();
    } catch (err) {
      console.error("Lỗi khi tải foods đang hoạt động: ", err);
    } finally {
      loading.value = false;
    }
  }

  async function fetchFood(id: number) {
    loading.value = true;
    try {
      const food = await getFood(id);
      return food;
    } catch (err) {
      console.error("Lỗi khi tải food: ", err);
    } finally {
      loading.value = false;
    }
  }

  async function addFood(foodData: Omit<FoodCreateOrUpdate, "id">) {
    try {
      const newFood = await createFood(foodData);
      foods.value.unshift(newFood);
    } catch (err) {
      console.error("Lỗi khi thêm food: ", err);
      throw err;
    }
  }

  async function editFood(id: number, foodData: FoodCreateOrUpdate) {
    try {
      await updateFood(id, foodData);
      const index = foods.value.findIndex((f) => f.id === id);
      if (index !== -1) {
        const targetFood = foods.value[index];
        Object.assign(targetFood!, foodData);
      }
    } catch (err) {
      console.error(`Lỗi khi sửa food ID ${id}: `, err);
      throw err;
    }
  }

  async function removeFood(id: number) {
    try {
      const deletedTime = await deleteFood(id);

      const food = foods.value.find((c) => c.id === id);
      if (food) {
        food.deletedAt = deletedTime;
      }
    } catch (err) {
      console.error(`Lỗi khi xóa Food ID ${id}: `, err);
      throw err;
    }
  }

  async function restoreFoodById(id: number) {
    try {
      await restoreFood(id);
      const food = foods.value.find((c) => c.id === id);
      if (food) {
        food.deletedAt = null;
      }
    } catch (err) {
      console.error(`Lỗi khi restore Food ID ${id}: `, err);
      throw err;
    }
  }

  return {
    foods,
    loading,
    fetchFoods,
    addFood,
    editFood,
    removeFood,
    fetchFood,
    restoreFoodById,
    fetchActiveFoods,
  };
});
