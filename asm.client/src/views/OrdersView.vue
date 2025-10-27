<template>
  <div class="container mt-4">
    <h2 class="mb-4">Đơn hàng của tôi</h2>

    <div v-if="loading" class="text-center">Đang tải dữ liệu...</div>
    <div v-else-if="orders.length === 0" class="text-center text-muted">
      Bạn chưa có đơn hàng nào.
    </div>

    <div v-else class="row gap-3">
      <div v-for="order in orders" :key="order.id" class="col-4 border shadow-sm rounded-5">
        <div class="py-4 px-3">
          <div class="d-flex justify-content-between align-items-center mb-2">
            <h5>Đơn hàng #{{ order.id }}</h5>
            <span :class="getStatusClass(order.status)">
              {{ getStatusText(order.status) }}
            </span>
          </div>

          <p class="mb-1"><strong>Địa chỉ giao:</strong> {{ order.shippingAddress }}</p>
          <p class="mb-1"><strong>Tổng tiền:</strong> {{ formatCurrency(order.totalAmount) }}</p>
          <p class="text-muted mb-2">
            <small>Ngày tạo: {{ formatDate(order.createdAt) }}</small>
          </p>

          <ul class="list-group small">
            <li
              v-for="detail in order.orderDetails"
              :key="detail.id"
              class="list-group-item d-flex justify-content-between align-items-center"
            >
              <span>
                {{ detail.food?.name || detail.combo?.name }}
                <small class="text-muted">(x{{ detail.quantity }})</small>
              </span>
              <span>{{ formatCurrency(detail.unitPrice) }}</span>
            </li>
          </ul>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from "vue";
import { getOrdersByUserId } from "@/api/orderService";
import type { Order } from "@/types/order";
import { OrderStatus } from "@/types/order";
import { useAuthStore } from "@/stores/authStore";

const authStore = useAuthStore();
const orders = ref<Order[]>([]);
const loading = ref(true);

onMounted(async () => {
  try {
    const userId = authStore.user?.id;
    if (!userId) return;
    orders.value = await getOrdersByUserId(userId);
  } catch (error) {
    console.error("Lỗi khi tải đơn hàng:", error);
  } finally {
    loading.value = false;
  }
});

const getStatusClass = (status: OrderStatus | undefined): string => {
  if (!status) return "badge text-bg-secondary";
  switch (status) {
    case OrderStatus.Pending:
      return "badge text-bg-warning";
    case OrderStatus.Shipping:
      return "badge text-bg-primary";
    case OrderStatus.Delivered:
      return "badge text-bg-success";
    default:
      return "badge text-bg-secondary";
  }
};

function getStatusText(status: number): string {
  switch (status) {
    case 1:
      return "Đang làm món";
    case 2:
      return "Đang giao hàng";
    case 3:
      return "Đã giao";
    default:
      return "Không xác định";
  }
}

function formatCurrency(value: number): string {
  return value.toLocaleString("vi-VN", { style: "currency", currency: "VND" });
}

function formatDate(date: string): string {
  return new Date(date).toLocaleString("vi-VN");
}
</script>

<style scoped>
.list-group-item {
  background: #fff;
}
</style>
