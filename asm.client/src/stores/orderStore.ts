import {
  createOrder,
  getOrders,
  getOrder,
  UpdateOrderStatus,
  getOrdersByUserId,
} from "@/api/orderService";
import { defineStore } from "pinia";
import { ref } from "vue";
import { OrderStatus, type Order } from "@/types/order";

export const useOrderStore = defineStore("Order", () => {
  const orders = ref<Order[]>([]);
  const loading = ref(false);
  async function fetchOrders() {
    loading.value = true;
    try {
      orders.value = await getOrders();
    } catch (err) {
      console.error("Lỗi khi tải orders: ", err);
    } finally {
      loading.value = false;
    }
  }

  async function fetchOrdersByUserId() {
    loading.value = true;
    try {
      orders.value = await getOrdersByUserId();
    } catch (err) {
      console.error("Lỗi khi tải orders: ", err);
    } finally {
      loading.value = false;
    }
  }

  async function fetchOrder(id: number) {
    loading.value = true;
    try {
      const Order = await getOrder(id);
      return Order;
    } catch (err) {
      console.error("Lỗi khi tải Order: ", err);
    } finally {
      loading.value = false;
    }
  }

  async function addOrder(Order: Omit<Order, "id">) {
    try {
      const newCategory = await createOrder(Order);
      orders.value.unshift(newCategory);
    } catch (err) {
      console.error("Lỗi khi thêm Order: ", err);
      throw err;
    }
  }

  async function editOrder(id: number, status: OrderStatus) {
    try {
      await UpdateOrderStatus(id, status);
      const index = orders.value.findIndex((f) => f.id === id);
      if (index !== -1) {
        const targetOrder = orders.value[index];
        Object.assign(targetOrder!, { ...targetOrder, status });
      }
    } catch (err) {
      console.error(`Lỗi khi sửa Order ID ${id}: `, err);
      throw err;
    }
  }

  return {
    orders,
    loading,
    fetchOrder,
    addOrder,
    editOrder,
    fetchOrders,
    fetchOrdersByUserId,
  };
});
