import { defineStore } from "pinia";
import { computed, ref } from "vue";
import type { Cart, CartDetailCreateOrUpdate } from "@/types/cart";
import {
  getCartActive,
  addCartDetail,
  updateCartDetail,
  deleteCart,
  deleteCartDetail,
} from "@/api/cartService";

export const useCartStore = defineStore("cart", () => {
  const cart = ref<Cart | null>(null);
  const cartTotal = computed(
    () => cart.value?.cartDetails.reduce((sum, d) => sum + d.price * d.quantity, 0) ?? 0
  );

  const cartCount = computed(
    () => cart.value?.cartDetails.reduce((sum, i) => sum + i.quantity, 0) ?? 0
  );

  const fetchCart = async () => {
    try {
      cart.value = await getCartActive();
    } catch (error) {
      console.error("Lấy cart thất bại:", error);
    }
  };

  const addItem = async (cartDetail: CartDetailCreateOrUpdate) => {
    try {
      const newDetail = await addCartDetail(cartDetail);
      if (cart.value) {
        cart.value.cartDetails.push(newDetail);
      }
    } catch (error) {
      console.error("Thêm item thất bại:", error);
    }
  };

  const updateItem = async (cartDetail: CartDetailCreateOrUpdate) => {
    try {
      await updateCartDetail(cartDetail);

      if (!cart.value) return;

      const item = cart.value.cartDetails.find((d) => d.productId === cartDetail.productId);

      if (item) {
        item.quantity = cartDetail.quantity;
      }
    } catch (error) {
      console.error("Cập nhật item thất bại:", error);
    }
  };

  const removeItem = async (productId: number) => {
    if (!cart.value) return;
    try {
      await deleteCartDetail(productId);
      cart.value.cartDetails = cart.value.cartDetails.filter((d) => d.productId !== productId);
    } catch (error) {
      console.error("Xóa item thất bại:", error);
    }
  };

  const checkOutCart = async () => {
    if (!cart.value) return;
    try {
      await deleteCart();
      cart.value = null;
    } catch (error) {
      console.error("Xóa cart thất bại:", error);
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
    cartTotal,
  };
});
