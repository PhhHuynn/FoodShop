import api from "./index";
import { type Category } from "@/types/category";

export async function getCategories(): Promise<Category[]> {
  const res = await api.get<Category[]>("/Categories");
  return res.data;
}

export async function getCategory(id: number): Promise<Category> {
  const res = await api.get<Category>(`/Categories/${id}`);
  return res.data;
}

export async function createCategory(food: Omit<Category, "id">): Promise<Category> {
  const res = await api.post<Category>("/Categories", food);
  return res.data;
}

export async function updateCategory(id: number, food: Category): Promise<void> {
  await api.put(`/Categories/${id}`, food);
}

export async function deleteCategory(id: number): Promise<void> {
  await api.delete(`/Categories/${id}`);
}
