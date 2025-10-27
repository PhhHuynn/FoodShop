import {
  createFood,
  deleteFood,
  getFoods,
  getFood,
  updateFood,
  uploadImageToServer,
} from "@/api/foodService";
import { defineStore } from "pinia";
import { ref } from "vue";
import { type Food } from "@/types/food";

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

  async function uploadImage(file: File): Promise<string> {
    try {
      const imageUrl = await uploadImageToServer(file);
      return imageUrl;
    } catch (err) {
      console.error("Lỗi khi upload ảnh: ", err);
      throw err;
    }
  }

  async function addFood(foodData: Omit<Food, "id">) {
    try {
      const newFood = await createFood(foodData);
      foods.value.unshift(newFood);
    } catch (err) {
      console.error("Lỗi khi thêm food: ", err);
      throw err;
    }
  }

  async function editFood(id: number, foodData: Food) {
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
      await deleteFood(id);
      foods.value = foods.value.filter((f) => f.id !== id);
    } catch (err) {
      console.error(`Lỗi khi xóa food ID ${id}: `, err);
      throw err;
    }
  }

  return { foods, loading, fetchFoods, addFood, editFood, removeFood, uploadImage, fetchFood };
});
