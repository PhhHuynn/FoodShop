import api from "./index";
import { type Combo } from "@/types/combo";

export async function getComboes(): Promise<Combo[]> {
  const res = await api.get<Combo[]>("/Combo");
  return res.data;
}

export async function getCombo(id: number): Promise<Combo> {
  const res = await api.get<Combo>(`/Combo/${id}`);
  return res.data;
}

export async function createCombo(combo: Omit<Combo, "id">): Promise<Combo> {
  const res = await api.post<Combo>("/Combo", combo);
  return res.data;
}

export async function updateCombo(id: number, combo: Combo): Promise<void> {
  await api.put(`/Combo/${id}`, combo);
}

export async function deleteCombo(id: number): Promise<void> {
  await api.delete(`/Combo/${id}`);
}

export async function uploadImageToServer(file: File): Promise<string> {
  const formData = new FormData();
  formData.append("file", file);
  const res = await api.post("/combo/upload", formData, {
    headers: { "Content-Type": "multipart/form-data" },
  });
  return res.data.imageUrl;
}
