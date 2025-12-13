import api from "./index";
import type { Cart, CartDetail, CartDetailCreateOrUpdate } from "@/types/cart";

export async function getCartActive(): Promise<Cart> {
  const res = await api.get<Cart>(`/Cart`);
  return res.data;
}
export const addCartDetail = async (cartDetail: CartDetailCreateOrUpdate): Promise<CartDetail> => {
  console.log(cartDetail);

  const response = await api.post(`cart/product/`, cartDetail);
  return response.data;
};

export const updateCartDetail = async (cartDetail: CartDetailCreateOrUpdate): Promise<void> => {
  await api.post(`cart/product/`, cartDetail);
};

export const deleteCartDetail = async (productId: number): Promise<void> => {
  await api.delete(`cart/product/${productId}`);
};

export const deleteCart = async (): Promise<void> => {
  await api.delete(`Cart`);
};
