<template>
  <div class="card-food card h-100 border rounded-4 shadow-sm">
    <img
      :src="`https://localhost:7119${item.imageUrl}`"
      class="card-img-top object-fit-cover"
      :alt="item.name"
      style="height: 250px"
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
import { type Food } from "@/types/food";
import type { Combo } from "@/types/combo";
import { useCartStore } from "@/stores/cartStore";
import { useAuthStore } from "@/stores/authStore";
import { useRouter } from "vue-router";
import type { CartDetailCreateOrUpdate } from "@/types/cart";

const cartStore = useCartStore();
const authStore = useAuthStore();
const router = useRouter();

defineProps<{ item: Food | Combo }>();

async function addToCart(item: Food | Combo) {
  if (authStore.user) {
    await cartStore.fetchCart();
    if (cartStore.cart) {
      const cartDetail: CartDetailCreateOrUpdate = {
        quantity: 1,
        productId: item.id,
      };
      await cartStore.addItem(cartDetail);
      await cartStore.fetchCart();
      alert("Thêm thành công");
    }
  } else {
    router.push("/login");
  }
}
</script>
