import {
  createCombo,
  deleteCombo,
  getComboes,
  getCombo,
  updateCombo,
  uploadImageToServer,
} from "@/api/comboService";
import { defineStore } from "pinia";
import { ref } from "vue";
import { type Combo } from "@/types/combo";

export const useComboStore = defineStore("Combo", () => {
  const comboes = ref<Combo[]>([]);
  const loading = ref(false);

  async function fetchComboes() {
    loading.value = true;
    try {
      comboes.value = await getComboes();
    } catch (err) {
      console.error("Lỗi khi tải Combos: ", err);
    } finally {
      loading.value = false;
    }
  }

  async function fetchCombo(id: number) {
    loading.value = true;
    try {
      const Combo = await getCombo(id);
      return Combo;
    } catch (err) {
      console.error("Lỗi khi tải Combo: ", err);
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

  async function addCombo(comboData: Omit<Combo, "id">) {
    try {
      const newCombo = await createCombo(comboData);
      comboes.value.unshift(newCombo);
    } catch (err) {
      console.error("Lỗi khi thêm Combo: ", err);
      throw err;
    }
  }

  async function editCombo(id: number, comboData: Combo) {
    try {
      await updateCombo(id, comboData);
      const index = comboes.value.findIndex((f) => f.id === id);
      if (index !== -1) {
        const targetCombo = comboes.value[index];
        Object.assign(targetCombo!, comboData);
      }
    } catch (err) {
      console.error(`Lỗi khi sửa Combo ID ${id}: `, err);
      throw err;
    }
  }

  async function removeCombo(id: number) {
    try {
      await deleteCombo(id);
      comboes.value = comboes.value.filter((c) => c.id !== id);
    } catch (err) {
      console.error(`Lỗi khi xóa Combo ID ${id}: `, err);
      throw err;
    }
  }

  return {
    comboes,
    loading,
    fetchComboes,
    addCombo,
    editCombo,
    removeCombo,
    uploadImage,
    fetchCombo,
  };
});
