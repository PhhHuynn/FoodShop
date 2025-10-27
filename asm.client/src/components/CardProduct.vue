<template>
  <div class="card-food card h-100 border rounded-4 shadow-sm">
    <img
      :src="`https://localhost:7108/${item.imageUrl}`"
      class="card-img-top object-fit-cover"
      :alt="item.name"
      style="height: 300px"
    />
    <div class="card-body text-center d-flex flex-column justify-content-between gap-4">
      <div>
        <h5 class="card-title">{{ item.name }}</h5>
        <p class="card-text fw-bold text-danger">{{ item.price }}₫</p>
        <p class="card-text">{{ item.description }}</p>
      </div>
      <button @click="addToCart(item)" class="btn btn-outline-warning">Thêm vào giỏ hàng</button>
    </div>
  </div>
</template>

<script setup lang="ts">
import { defineProps } from "vue";
import { type Food } from "@/types/food";
import type { Combo } from "@/types/combo";
import type { CartDetailCreate } from "@/types/cart";
import { useCartStore } from "@/stores/cartStore";
import { useAuthStore } from "@/stores/authStore";
import { useRouter } from "vue-router";

const cartStore = useCartStore();
const authStore = useAuthStore();
const router = useRouter();

defineProps<{ item: Food | Combo }>();

async function addToCart(item: Food | Combo) {
  if (authStore.user) {
    await cartStore.fetchCart(authStore.user?.id);
    if (cartStore.cart) {
      if ("comboFoods" in item) {
        const cartDetail: CartDetailCreate = {
          quantity: 1,
          comboId: item.id,
          cartId: cartStore.cart.id,
        };
        cartStore.addItem(cartDetail);
        alert("Thêm thành công");
      } else {
        const cartDetail: CartDetailCreate = {
          quantity: 1,
          foodId: item.id,
          cartId: cartStore.cart.id,
        };
        cartStore.addItem(cartDetail);
        alert("Thêm thành công");
        await cartStore.fetchCart(authStore.user.id);
      }
    }
  } else {
    router.push("/login");
  }
}
</script>
