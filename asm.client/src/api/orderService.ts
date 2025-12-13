import api from "./index";
import { OrderStatus, type Order } from "@/types/order";

export async function getOrders(): Promise<Order[]> {
  const res = await api.get<Order[]>("/Order");
  return res.data;
}

export async function getOrder(id: number): Promise<Order> {
  const res = await api.get<Order>(`/Order/${id}`);
  return res.data;
}

export async function createOrder(food: Omit<Order, "id">): Promise<Order> {
  const res = await api.post<Order>("/Order", food);
  return res.data;
}

export async function UpdateOrderStatus(id: number, status: OrderStatus): Promise<void> {
  console.log(status);
  await api.put(`/Order/${id}`, { status });
}

export async function getOrdersByUserId(): Promise<Order[]> {
  const response = await api.get<Order[]>(`/Order/me`);
  return response.data;
}
