import api from "./index";
import { type Food } from "@/types/food";

export async function getFoods(): Promise<Food[]> {
  const res = await api.get<Food[]>("/food");
  return res.data;
}

export async function getFood(id: number): Promise<Food> {
  const res = await api.get<Food>(`/food/${id}`);
  return res.data;
}

export async function createFood(food: Omit<Food, "id">): Promise<Food> {
  const res = await api.post<Food>("/food", food);
  return res.data;
}

export async function updateFood(id: number, food: Food): Promise<void> {
  await api.put(`/food/${id}`, food);
}

export async function deleteFood(id: number): Promise<void> {
  await api.delete(`/food/${id}`);
}

export async function uploadImageToServer(file: File): Promise<string> {
  const formData = new FormData();
  formData.append("file", file);
  const res = await api.post("/food/upload", formData, {
    headers: { "Content-Type": "multipart/form-data" },
  });
  return res.data.imageUrl;
}
