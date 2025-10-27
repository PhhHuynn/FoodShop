import api from "./index";
import { type Combo } from "@/types/combo";

export async function getComboes(): Promise<Combo[]> {
  const res = await api.get<Combo[]>("/Comboes");
  return res.data;
}

export async function getCombo(id: number): Promise<Combo> {
  const res = await api.get<Combo>(`/Comboes/${id}`);
  return res.data;
}

export async function createCombo(combo: Omit<Combo, "id">): Promise<Combo> {
  const res = await api.post<Combo>("/Comboes", combo);
  return res.data;
}

export async function updateCombo(id: number, combo: Combo): Promise<void> {
  await api.put(`/Comboes/${id}`, combo);
}

export async function deleteCombo(id: number): Promise<void> {
  await api.delete(`/Comboes/${id}`);
}

export async function uploadImageToServer(file: File): Promise<string> {
  const formData = new FormData();
  formData.append("file", file);
  const res = await api.post("/comboes/upload", formData, {
    headers: { "Content-Type": "multipart/form-data" },
  });
  return res.data.imageUrl;
}
