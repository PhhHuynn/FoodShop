import type { Combo } from "./combo";
import type { Food } from "./food";

export interface Order {
  id: number;
  shippingAddress: string;
  status: OrderStatus;
  totalAmount: number;
  createdAt: string;
  userId: string;
  orderDetails?: OrderDetail[] | null;
}

export interface OrderDetail {
  id: number;
  quantity: number;
  unitPrice: number;
  orderId: number;
  foodId?: number | null;
  comboId?: number | null;
  food?: Food | null;
  combo?: Combo | null;
}

export enum OrderStatus {
  Pending = 1, // làm món
  Shipping = 2, // đang giao hàng
  Delivered = 3, // đã giao
}
