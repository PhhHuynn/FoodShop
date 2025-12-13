import api from "./index";
import { type Food, type FoodCreateOrUpdate } from "@/types/food";

export async function getFoods(): Promise<Food[]> {
  const res = await api.get<Food[]>("/food");
  return res.data;
}
export async function getActiveFoods(): Promise<Food[]> {
  const res = await api.get<Food[]>("/food/active");
  return res.data;
}

export async function getFood(id: number): Promise<Food> {
  const res = await api.get<Food>(`/food/${id}`);
  return res.data;
}

export async function createFood(food: Omit<FoodCreateOrUpdate, "id">): Promise<Food> {
  const formData = new FormData();

  formData.append("name", food.name);
  formData.append("description", food.description);
  formData.append("price", food.price.toString());
  formData.append("isAvailable", food.isAvailable ? "true" : "false");
  formData.append("categoryId", food.categoryId.toString());

  if (food.fImageFile) {
    formData.append("fImageFile", food.fImageFile);
  }

  const res = await api.post<Food>("/food", formData, {
    headers: {
      "Content-Type": "multipart/form-data",
    },
  });

  return res.data;
}

export async function updateFood(id: number, food: FoodCreateOrUpdate): Promise<void> {
  const formData = new FormData();

  formData.append("name", food.name);
  formData.append("description", food.description);
  formData.append("categoryId", food.categoryId.toString());
  formData.append("price", food.price.toString());
  formData.append("isAvailable", food.isAvailable ? "true" : "false");

  if (food.fImageFile) {
    formData.append("fImageFile", food.fImageFile);
  }

  await api.put(`/food/${id}`, formData, {
    headers: {
      "Content-Type": "multipart/form-data",
    },
  });
}

export async function deleteFood(id: number): Promise<string> {
  return await api.delete(`/food/${id}`).then((res) => res.data);
}

export async function restoreFood(id: number): Promise<void> {
  return await api.patch(`/food/${id}/restore`).then((res) => res.data);
}
