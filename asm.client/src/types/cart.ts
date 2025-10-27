import type { Combo } from "./combo";
import type { Food } from "./food";

export interface Cart {
  id: number;
  status: CartStatus;
  userId: string;
  cartDetails: CartDetail[];
}

export interface CartDetail {
  id: number;
  quantity: number;
  cartId: number;
  foodId?: number;
  comboId?: number;
  food?: Food;
  combo?: Combo;
}

export interface CartDetailCreate {
  quantity: number;
  cartId: number;
  foodId?: number;
  comboId?: number;
}

export enum CartStatus {
  Active = 1, // đang hoạt động
  CheckedOut = 2, // đã thanh toán
}
