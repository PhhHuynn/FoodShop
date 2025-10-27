<template>
  <div class="container mt-4">
    <div class="d-flex justify-content-between align-items-center mb-3">
      <h2 class="fw-semibold">Quản lý Đơn hàng</h2>
    </div>

    <div v-if="store.loading" class="text-center my-5">
      <div class="spinner-border text-primary" role="status">
        <span class="visually-hidden">Đang tải...</span>
      </div>
      <p class="mt-2">Đang tải danh sách đơn hàng...</p>
    </div>

    <table v-else class="table table-striped table-bordered align-middle">
      <thead class="table-light">
        <tr>
          <th style="width: 80px">ID</th>
          <th style="width: 150px">Mã Người đặt</th>
          <th>Ngày đặt</th>
          <th>Trạng thái</th>
          <th style="width: 250px">Hành động</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="order in store.orders" :key="order.id">
          <td>{{ order.id }}</td>
          <td>{{ order.userId }}</td>
          <td>{{ formatDate(order.createdAt) }}</td>
          <td>
            <span :class="getStatusClass(order.status)">
              {{ getStatusText(order.status) }}
            </span>
          </td>
          <td>
            <div class="d-flex gap-2">
              <RouterLink :to="`/admin/orders/${order.id}`" class="btn btn-sm btn-secondary">
                Xem chi tiết
              </RouterLink>

              <RouterLink :to="`/admin/orders/edit/${order.id}`" class="btn btn-sm btn-primary">
                Sửa trạng thái
              </RouterLink>
            </div>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script setup lang="ts">
import { onMounted } from "vue";
import { RouterLink } from "vue-router";
import { useOrderStore } from "@/stores/orderStore";
import { OrderStatus } from "@/types/order";

const store = useOrderStore();

const getStatusClass = (status: OrderStatus): string => {
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

function getStatusText(status: OrderStatus | number | undefined): string {
  if (status === undefined || status === null) {
    return "Chưa có trạng thái";
  }

  switch (status) {
    case OrderStatus.Pending:
    case 1:
      return "Đang làm món";
    case OrderStatus.Shipping:
    case 2:
      return "Đang giao hàng";
    case OrderStatus.Delivered:
    case 3:
      return "Đã giao";
    default:
      return "Trạng thái không xác định";
  }
}

function formatDate(dateString: string | undefined): string {
  if (!dateString) return "";
  return new Date(dateString).toLocaleDateString("vi-VN");
}

onMounted(async () => {
  await store.fetchOrders();
});
</script>
