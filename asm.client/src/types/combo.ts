import type { Product } from "./product";

export interface Combo extends Product {
  comboFoods: comboFood[];
}

export interface comboFood {
  id?: number;
  foodId: number;
  name?: string;
  quantity: number;
}

export type ComboCreateOrUpdateDto = Omit<
  Combo,
  "id" | "createdAt" | "updatedAt" | "deletedAt" | "averageRating" | "imageUrl"
>;
