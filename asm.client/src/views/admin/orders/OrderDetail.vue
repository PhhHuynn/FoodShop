<template>
  <div class="container mt-4">
    <h2 class="fw-semibold mb-4">Chi tiết Đơn hàng</h2>

    <div v-if="order" class="card p-4">
      <div class="row g-3">
        <div class="col-md-12">
          <h5 class="card-title mb-3">Đơn hàng #{{ order.id }}</h5>
          <p class="card-text">
            <span class="fw-bold">Mã Người dùng (User ID):</span>
            {{ order.userId }}
          </p>
          <p class="card-text">
            <span class="fw-bold">Trạng thái:</span>
            <span :class="getStatusClass(order.status)">
              {{ getStatusText(order.status) }}
            </span>
          </p>
        </div>
      </div>
    </div>
    <div v-else-if="!loading && !order" class="alert alert-warning">
      Không tìm thấy đơn hàng này.
    </div>
    <div v-else-if="loading" class="text-center">Đang tải chi tiết đơn hàng...</div>

    <RouterLink to="/admin/orders" class="btn btn-secondary mt-3">Quay lại</RouterLink>
  </div>
</template>

<script setup lang="ts">
import { onMounted, ref } from "vue";
import { useRoute } from "vue-router";
import { useOrderStore } from "@/stores/orderStore";
import { OrderStatus, type Order } from "@/types/order";

const store = useOrderStore();
const route = useRoute();
const order = ref<Order | null>(null);
const loading = ref(false);

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

onMounted(async () => {
  const id = Number(route.params.id);
  if (isNaN(id)) {
    console.error("ID đơn hàng không hợp lệ.");
    return;
  }

  loading.value = true;
  try {
    const fetchedOrder = await store.fetchOrder(id);
    order.value = fetchedOrder || null;
  } catch (error) {
    order.value = null;
    console.error("Lấy dữ liệu order lỗi: " + error);
  } finally {
    loading.value = false;
  }
});
</script>
