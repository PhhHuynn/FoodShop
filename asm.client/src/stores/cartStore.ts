import { defineStore } from "pinia";
import { computed, ref } from "vue";
import type { Cart, CartDetail, CartDetailCreate } from "@/types/cart";
import { CartStatus } from "@/types/cart";
import {
  getCartActive,
  addCartDetail,
  updateCartDetail,
  deleteCartDetail,
  updateCartStatus,
} from "@/api/cartService";

export const useCartStore = defineStore("cart", () => {
  const cart = ref<Cart | null>(null);

  const cartCount = computed(
    () => cart.value?.cartDetails.reduce((sum, i) => sum + i.quantity, 0) ?? 0
  );

  const fetchCart = async (userId: string) => {
    try {
      cart.value = await getCartActive(userId);
    } catch (error) {
      console.error("Lấy cart thất bại:", error);
    }
  };

  const addItem = async (cartDetail: CartDetailCreate) => {
    try {
      const newDetail = await addCartDetail(cartDetail);
      if (cart.value) {
        cart.value.cartDetails.push(newDetail);
      }
    } catch (error) {
      console.error("Thêm item thất bại:", error);
    }
  };

  const updateItem = async (id: number, quantity: number) => {
    if (!cart.value) return;
    const index = cart.value.cartDetails.findIndex((d) => d.id === id);
    if (index === -1) return;

    const updatedDetail: CartDetail = {
      ...cart.value.cartDetails[index],
      quantity,
    };

    try {
      await updateCartDetail(id, updatedDetail);
      cart.value.cartDetails.splice(index, 1, updatedDetail);
    } catch (error) {
      console.error("Cập nhật item thất bại:", error);
    }
  };

  const removeItem = async (id: number) => {
    if (!cart.value) return;
    try {
      await deleteCartDetail(id);
      cart.value.cartDetails = cart.value.cartDetails.filter((d) => d.id !== id);
    } catch (error) {
      console.error("Xóa item thất bại:", error);
    }
  };

  const checkOutCart = async () => {
    if (!cart.value) return;
    try {
      await updateCartStatus(cart.value.id, {
        ...cart.value,
        status: CartStatus.CheckedOut,
      });
      cart.value.status = CartStatus.CheckedOut;
    } catch (error) {
      console.error("Cập nhật trạng thái cart thất bại:", error);
    }
  };

  const clear = () => {
    cart.value = null;
  };

  return {
    cart,
    cartCount,
    fetchCart,
    addItem,
    updateItem,
    removeItem,
    checkOutCart,
    clear,
  };
});
