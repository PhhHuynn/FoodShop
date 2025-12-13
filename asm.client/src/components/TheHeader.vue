<template>
  <nav
    class="navbar navbar-expand-lg border-bottom position-fixed top-0 start-0 end-0 bg-white"
    style="height: 70px; z-index: 100"
  >
    <div class="container">
      <a class="navbar-brand d-flex align-items-center" href="/">
        <img src="/img/logo.png" alt="logo cook food" style="width: 90px" class="me-2" />
      </a>

      <div class="d-flex align-items-center gap-3">
        <template v-if="auth.token">
          <button
            class="btn position-relative me-2"
            type="button"
            data-bs-toggle="offcanvas"
            data-bs-target="#cartOffcanvas"
            aria-controls="cartOffcanvas"
          >
            <lord-icon
              src="https://cdn.lordicon.com/euduggnx.json"
              trigger="hover"
              target=".btn"
              style="width: 25px; height: 25px"
            >
            </lord-icon>
            <span
              v-if="cartStore.cart?.cartDetails.length"
              style="top: 10px"
              class="position-absolute start-100 translate-middle badge rounded-pill bg-danger"
            >
              {{ cartStore.cart?.cartDetails.length }}
            </span>
          </button>

          <RouterLink to="/admin/dashboard" v-if="auth.isAdmin" class="btn btn-warning">
            Quản lý
          </RouterLink>

          <div class="dropdown">
            <button
              style="width: 40px; height: 40px"
              class="btn p-0 border-0 d-flex justify-content-center align-items-center bg-body-secondary border-secondary rounded-circle btn-size-custom"
              type="button"
              data-bs-toggle="dropdown"
              aria-expanded="false"
            >
              <i class="fa-solid fa-user"></i>
            </button>
            <ul class="dropdown-menu dropdown-menu-end shadow-sm">
              <li>
                <router-link to="/dashboard" class="dropdown-item">My Profile</router-link>
              </li>
              <li>
                <router-link to="/orders" class="dropdown-item">Đơn hàng</router-link>
              </li>
              <li><a class="dropdown-item" @click="logout">Logout</a></li>
            </ul>
          </div>
        </template>

        <template v-else>
          <router-link to="/login" class="btn btn-outline-warning me-2">Đăng nhập</router-link>
          <router-link to="/signup" class="btn btn-warning me-2">Đăng ký</router-link>
        </template>
      </div>
    </div>
  </nav>

  <div
    class="offcanvas offcanvas-end"
    tabindex="-1"
    id="cartOffcanvas"
    aria-labelledby="cartOffcanvasLabel"
  >
    <div class="offcanvas-header">
      <h5 class="offcanvas-title">Giỏ hàng</h5>
      <button type="button" class="btn-close" data-bs-dismiss="offcanvas"></button>
    </div>

    <div class="offcanvas-body d-flex flex-column">
      <div
        v-if="!cartStore.cart || cartStore.cart.cartDetails.length === 0"
        class="text-center text-muted"
      >
        Giỏ hàng trống
      </div>

      <ul v-else class="list-group mb-3">
        <li
          v-for="item in cartStore.cart.cartDetails"
          :key="item.id"
          class="list-group-item d-flex justify-content-between align-items-center"
        >
          <span style="flex: 1">
            {{ item.productName }}
          </span>

          <span class="me-2">
            {{ formatCurrency(item.price * item.quantity) }}
          </span>

          <input
            type="number"
            min="1"
            v-model.number="item.quantity"
            @change="updateQuantity(item)"
            class="form-control form-control-sm w-25 me-2"
          />

          <button class="btn btn-sm btn-danger" @click="removeItem(item)">X</button>
        </li>
      </ul>

      <div v-if="cartStore.cart?.cartDetails.length" class="mb-3">
        <h6>Tổng tiền: {{ formatCurrency(cartStore.cartTotal) }}</h6>
      </div>

      <div class="mb-3">
        <label class="form-label">Địa chỉ nhận hàng</label>
        <input
          v-model="address"
          type="text"
          class="form-control"
          placeholder="Nhập địa chỉ giao hàng"
        />
      </div>

      <div class="mb-3">
        <label class="form-label">Phương thức thanh toán</label>
        <input class="form-control" readonly value="Thanh toán khi nhận hàng (COD)" />
      </div>

      <button class="btn btn-warning mt-auto" @click="checkout">Thanh toán</button>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from "vue";
import { useRouter } from "vue-router";
import { useAuthStore } from "@/stores/authStore";
import { useCartStore } from "@/stores/cartStore";
import { useOrderStore } from "@/stores/orderStore";
import type { CartDetail } from "@/types/cart";
import type { Order, OrderDetail } from "@/types/order";
import { OrderStatus, PaymentMethod } from "@/types/order";

const auth = useAuthStore();
const cartStore = useCartStore();
const orderStore = useOrderStore();

const address = ref("");

onMounted(async () => {
  if (auth.user) {
    await cartStore.fetchCart();
  }
  console.log(cartStore.cart);
});

const router = useRouter();
const logout = () => {
  auth.logout();
  router.push("/");
};

async function checkout() {
  if (!address.value) {
    alert("Vui lòng nhập địa chỉ giao hàng!");
    return;
  }
  if (!auth.user || !cartStore.cart) return;

  const orderDetails: OrderDetail[] = cartStore.cart.cartDetails.map((item: CartDetail) => ({
    quantity: item.quantity,
    unitPrice: item.price,
    productId: item.productId,
  }));

  const newOrder: Order = {
    id: 0,
    shippingAddress: address.value,
    status: OrderStatus.Pending,
    totalAmount: cartStore.cartTotal,
    discountAmount: "0",
    paymentMethod: PaymentMethod.COD,
    userId: auth.user.id,
    orderDetails,
  };

  try {
    await orderStore.addOrder(newOrder);
    await cartStore.checkOutCart();

    await cartStore.fetchCart();

    alert("Đặt hàng thành công!");
    address.value = "";
  } catch (err) {
    console.error(err);
    alert("Có lỗi khi đặt hàng.");
  }
}

async function removeItem(item: CartDetail) {
  await cartStore.removeItem(item.productId);
}

async function updateQuantity(item: CartDetail) {
  if (item.quantity < 1) item.quantity = 1;

  const dto = {
    quantity: item.quantity,
    productId: item.productId,
  };

  await cartStore.updateItem(dto);
}

function formatCurrency(amount: number) {
  return amount.toLocaleString("vi-VN", { style: "currency", currency: "VND" });
}
</script>
