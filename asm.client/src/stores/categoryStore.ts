import {
  createCategory,
  deleteCategory,
  getCategories,
  getCategory,
  updateCategory,
} from "@/api/categoryService";
import { defineStore } from "pinia";
import { ref } from "vue";
import { type Category } from "@/types/category";

export const useCategoryStore = defineStore("category", () => {
  const categories = ref<Category[]>([]);
  const loading = ref(false);

  async function fetchCategories() {
    loading.value = true;
    try {
      categories.value = await getCategories();
    } catch (err) {
      console.error("Lỗi khi tải categories: ", err);
    } finally {
      loading.value = false;
    }
  }

  async function fetchCategory(id: number) {
    loading.value = true;
    try {
      const category = await getCategory(id);
      return category;
    } catch (err) {
      console.error("Lỗi khi tải category: ", err);
    } finally {
      loading.value = false;
    }
  }

  async function addCategory(category: Omit<Category, "id">) {
    try {
      const newCategory = await createCategory(category);
      categories.value.unshift(newCategory);
    } catch (err) {
      console.error("Lỗi khi thêm category: ", err);
      throw err;
    }
  }

  async function editCategory(id: number, categoryData: Category) {
    try {
      await updateCategory(id, categoryData);
      const index = categories.value.findIndex((f) => f.id === id);
      if (index !== -1) {
        const targetFood = categories.value[index];
        Object.assign(targetFood!, categoryData);
      }
    } catch (err) {
      console.error(`Lỗi khi sửa category ID ${id}: `, err);
      throw err;
    }
  }

  async function removeCategory(id: number) {
    try {
      await deleteCategory(id);
      categories.value = categories.value.filter((c) => c.id !== id);
    } catch (err) {
      console.error(`Lỗi khi xóa category ID ${id}: `, err);
      throw err;
    }
  }

  return {
    categories,
    loading,
    fetchCategories,
    addCategory,
    editCategory,
    removeCategory,
    fetchCategory,
  };
});
