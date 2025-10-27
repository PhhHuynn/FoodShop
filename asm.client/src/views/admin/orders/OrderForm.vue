<template>
  <div class="order-status-form container">
    <h2>Cập nhật Trạng thái Đơn hàng #{{ form.id }}</h2>

    <div v-if="store.loading" class="alert alert-info text-center">
      Đang tải thông tin đơn hàng...
    </div>
    <div v-else-if="!form.id && isEdit" class="alert alert-danger text-center">
      Không tìm thấy đơn hàng cần cập nhật.
    </div>

    <form v-else @submit.prevent="handleSubmit">
      <div class="mb-3">
        <label class="form-label fw-bold">Trạng thái hiện tại</label>
        <input
          :value="form.status"
          class="form-control"
          readonly
          :class="getStatusClass(form.status)"
        />
      </div>

      <div class="mb-3">
        <label class="form-label">Mã Người dùng (User ID)</label>
        <input v-model="form.userId" class="form-control" readonly />
      </div>

      <div class="mb-4">
        <label class="form-label fw-bold">Chọn Trạng thái mới</label>
        <select v-model="form.status" class="form-select" required>
          <option :value="OrderStatus.Pending">Đang chờ xử lý</option>
          <option :value="OrderStatus.Shipping">Đang giao hàng</option>
          <option :value="OrderStatus.Delivered">Đã giao hàng</option>
        </select>
      </div>

      <div class="d-flex gap-2">
        <button type="submit" class="btn btn-primary" :disabled="store.loading">
          Lưu thay đổi
        </button>
        <RouterLink to="/admin/orders" class="btn btn-secondary"> Hủy </RouterLink>
      </div>
    </form>
  </div>
</template>

<script lang="ts" setup>
import { ref, computed, onMounted } from "vue";
import { useRoute, useRouter, RouterLink } from "vue-router";
import { useOrderStore } from "@/stores/orderStore";
import type { Order } from "@/types/order";
import { OrderStatus } from "@/types/order";

const router = useRouter();
const store = useOrderStore();
const route = useRoute();

const isEdit = computed(() => !!Number(route.params.id));

const form = ref<Partial<Order>>({
  id: 0,
  userId: "",
  status: OrderStatus.Pending,
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

onMounted(async () => {
  if (isEdit.value) {
    const orderId = Number(route.params.id);
    const orderData = await store.fetchOrder(orderId);

    if (orderData) {
      form.value.id = orderData.id;
      form.value.userId = orderData.userId;
      form.value.status = orderData.status;
    } else {
      form.value.id = 0;
    }
  }
});

const handleSubmit = async () => {
  if (!form.value.id || !form.value.status) return;

  try {
    await store.editOrder(form.value.id, form.value.status);

    alert(`Cập nhật trạng thái đơn hàng #${form.value.id} thành công!`);

    router.push("/admin/orders/");
  } catch (err) {
    console.error("Lỗi khi cập nhật trạng thái đơn hàng:", err);
    alert("Có lỗi xảy ra khi cập nhật trạng thái: " + (err instanceof Error ? err.message : ""));
  }
};
</script>
