import {
  createCombo,
  updateCombo,
  deleteCombo,
  getComboes,
  getCombo,
  getActiveCombos,
  restoreCombo,
} from "@/api/comboService";
import { defineStore } from "pinia";
import { ref } from "vue";
import type { Combo, ComboCreateOrUpdateDto } from "@/types/combo";

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

  async function fetchCombo(id: number): Promise<Combo | undefined> {
    loading.value = true;
    try {
      const combo = await getCombo(id);
      return combo;
    } catch (err) {
      console.error("Lỗi khi tải Combo: ", err);
    } finally {
      loading.value = false;
    }
  }

  async function fetchActiveCombos() {
    loading.value = true;
    try {
      comboes.value = await getActiveCombos();
    } catch (err) {
      console.error("Lỗi khi tải combo active: ", err);
    } finally {
      loading.value = false;
    }
  }

  async function addCombo(comboData: ComboCreateOrUpdateDto) {
    try {
      const newCombo = await createCombo(comboData);
      comboes.value.unshift(newCombo);
    } catch (err) {
      console.error("Lỗi khi thêm Combo: ", err);
      throw err;
    }
  }

  async function editCombo(id: number, comboData: ComboCreateOrUpdateDto) {
    try {
      await updateCombo(id, comboData);
      const updated = await getCombo(id);
      if (updated) {
        const index = comboes.value.findIndex((f) => f.id === id);
        if (index !== -1) comboes.value[index] = updated;
      }
    } catch (err) {
      console.error(`Lỗi khi sửa Combo ID ${id}: `, err);
      throw err;
    }
  }

  async function removeCombo(id: number) {
    try {
      const deletedTime = await deleteCombo(id);

      const combo = comboes.value.find((c) => c.id === id);
      if (combo) {
        combo.deletedAt = deletedTime;
      }
    } catch (err) {
      console.error(`Lỗi khi xóa Combo ID ${id}: `, err);
      throw err;
    }
  }

  async function restoreComboById(id: number) {
    try {
      await restoreCombo(id);
      const combo = comboes.value.find((c) => c.id === id);
      if (combo) {
        combo.deletedAt = null;
      }
    } catch (err) {
      console.error(`Lỗi khi restore Combo ID ${id}: `, err);
      throw err;
    }
  }

  return {
    comboes,
    loading,
    fetchComboes,
    fetchActiveCombos,
    fetchCombo,
    addCombo,
    editCombo,
    removeCombo,
    restoreComboById,
  };
});
