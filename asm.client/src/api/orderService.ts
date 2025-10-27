import api from "./index";
import { OrderStatus, type Order } from "@/types/order";

export async function getOrders(): Promise<Order[]> {
  const res = await api.get<Order[]>("/Orders");
  return res.data;
}

export async function getOrder(id: number): Promise<Order> {
  const res = await api.get<Order>(`/Orders/${id}`);
  return res.data;
}

export async function createOrder(food: Omit<Order, "id">): Promise<Order> {
  const res = await api.post<Order>("/Orders", food);
  return res.data;
}

export async function UpdateOrderStatus(id: number, status: OrderStatus): Promise<void> {
  console.log(status);
  await api.put(`/Orders/${id}`, status, {
    headers: {
      "Content-Type": "application/json",
    },
  });
}

export async function deleteOrder(id: number): Promise<void> {
  await api.delete(`/Orders/${id}`);
}

export async function getOrdersByUserId(userId: string): Promise<Order[]> {
  const response = await api.get<Order[]>(`/Orders/user/${userId}`);
  return response.data;
}
