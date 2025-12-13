import type { Product } from "./product";

export interface Food extends Product {
  categoryId: number;
  categoryName: string;
}

export type FoodCreateOrUpdate = Omit<
  Food,
  "createdAt" | "updatedAt" | "deletedAt" | "averageRating" | "imageUrl" | "categoryName"
>;
