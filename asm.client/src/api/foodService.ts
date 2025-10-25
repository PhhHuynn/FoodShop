import api from "./index";

export interface Food {
  id: number;
  name: string;
  price: number;
  description: string;
  imageUrl: string;
}

export async function getFoods(): Promise<Food[]> {
  const res = await api.get<Food[]>("/foods");
  return res.data;
}
