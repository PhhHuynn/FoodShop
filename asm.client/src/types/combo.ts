import type { Food } from "./food";

export interface Combo {
  id: number;
  name: string;
  price: number;
  description: string;
  imageUrl: string;
  isAvailable: boolean;
  comboFoods: comboFood[];
}

export interface comboFood {
  id?: number;
  foodId: number;
  comboId: number;
  quantity: number;
  food: Food;
}
