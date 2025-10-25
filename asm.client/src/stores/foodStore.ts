import { getFoods, type Food } from "@/api/foodService";
import { defineStore } from "pinia";
import { ref } from "vue";

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
  return { foods, loading, fetchFoods };
});
