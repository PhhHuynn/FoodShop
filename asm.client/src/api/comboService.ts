import api from "./index";
import type { Combo, ComboCreateOrUpdateDto } from "@/types/combo";

export async function getComboes(): Promise<Combo[]> {
  const res = await api.get<Combo[]>("/Combo");
  return res.data;
}

export async function getCombo(id: number): Promise<Combo> {
  const res = await api.get<Combo>(`/Combo/${id}`);
  return res.data;
}

export async function getActiveCombos(): Promise<Combo[]> {
  const res = await api.get<Combo[]>("/Combo/active");
  return res.data;
}

export async function createCombo(combo: ComboCreateOrUpdateDto): Promise<Combo> {
  const formData = new FormData();
  formData.append("name", combo.name);
  formData.append("description", combo.description);
  formData.append("price", combo.price.toString());
  formData.append("isAvailable", combo.isAvailable ? "true" : "false");

  if (combo.fImageFile) {
    formData.append("fImageFile", combo.fImageFile);
  }

  if (combo.comboFoods) {
    formData.append("comboFoods", JSON.stringify(combo.comboFoods));
  }

  const res = await api.post<Combo>("/Combo", formData, {
    headers: { "Content-Type": "multipart/form-data" },
  });

  return res.data;
}

export async function updateCombo(id: number, combo: ComboCreateOrUpdateDto): Promise<void> {
  const formData = new FormData();
  formData.append("name", combo.name);
  formData.append("description", combo.description);
  formData.append("price", combo.price.toString());
  formData.append("isAvailable", combo.isAvailable ? "true" : "false");

  if (combo.fImageFile) {
    formData.append("fImageFile", combo.fImageFile);
  }

  combo.comboFoods.forEach((cf, index) => {
    formData.append(`comboFoods[${index}].foodId`, cf.foodId.toString());
    formData.append(`comboFoods[${index}].quantity`, cf.quantity.toString());
  });

  await api.put(`/Combo/${id}`, formData, {
    headers: { "Content-Type": "multipart/form-data" },
  });
}

export async function deleteCombo(id: number): Promise<string> {
  return await api.delete(`/Combo/${id}`).then((res) => res.data);
}

export async function restoreCombo(id: number): Promise<void> {
  await api.patch(`/Combo/${id}/restore`);
}
