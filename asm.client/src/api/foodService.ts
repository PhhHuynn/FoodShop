import api from "./index";
import { type Food } from "@/types/food";

export async function getFoods(): Promise<Food[]> {
  const res = await api.get<Food[]>("/foods");
  return res.data;
}

export async function createFood(food: Food): Promise<Food[]> {
  const res = await api.post<Food[]>("/foods", food);
  return res.data;
}

export async function updateFood(id: number, food: Food): Promise<void> {
  await api.put(`/foods/${id}`, food);
}

export async function deleteFood(id: number): Promise<void> {
  await api.delete(`/foods/${id}`);
}
