import api from "./index";
import type { Cart, CartDetail, CartDetailCreate } from "@/types/cart";

export async function getCartActive(userId: string): Promise<Cart> {
  const res = await api.get<Cart>(`/Carts/active/${userId}`);
  return res.data;
}
export const addCartDetail = async (cartDetail: CartDetailCreate): Promise<CartDetail> => {
  const response = await api.post(`CartDetails/`, cartDetail);
  return response.data;
};

// Cập nhật item trong cart
export const updateCartDetail = async (id: number, cartDetail: CartDetail): Promise<void> => {
  await api.put(`CartDetails/${id}`, cartDetail);
};

// Xóa item khỏi cart
export const deleteCartDetail = async (id: number): Promise<void> => {
  await api.delete(`CartDetails/${id}`);
};

// Cập nhật trạng thái cart
export const updateCartStatus = async (id: number, cart: Cart): Promise<void> => {
  await api.put(`Carts/${id}`, cart);
};
