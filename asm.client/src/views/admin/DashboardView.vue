<template>
  <h1 class="my-5 fw-bold text-center">Admin Dashboard</h1>

  <div class="container py-3 my-3" id="dashboard">
    <div class="row g-4">
      <div class="col-12 col-lg-8 d-flex flex-wrap gap-4 justify-content-center">
        <div
          class="dashboard-card p-4"
          style="
            flex: 1 1 320px;
            background: linear-gradient(135deg, #667eea, #764ba2);
            color: #fff;
            border-radius: 16px;
            box-shadow: 0 8px 20px rgba(102, 126, 234, 0.3);
          "
        >
          <h6 class="text-uppercase fw-semibold mb-2" style="letter-spacing: 1.2px">
            Tổng số tài khoản người dùng
          </h6>
          <p class="display-4 fw-bold mb-0">{{ dashboardData.totalUsers }}</p>
          <router-link
            to="/admin/users"
            class="mt-3 btn btn-light btn-sm fw-semibold d-inline-flex align-items-center gap-2"
            style="border-radius: 12px; color: #222"
          >
            Quản lý tài khoản
            <lord-icon
              src="https://cdn.lordicon.com/dygfbwwx.json"
              target=".btn"
              trigger="hover"
              style="width: 20px; height: 20px; --lord-icon-primary: #667eea"
            >
            </lord-icon>
          </router-link>
        </div>

        <div
          class="dashboard-card p-4"
          style="
            flex: 1 1 320px;
            background: linear-gradient(135deg, #f6d365, #fda085);
            color: #fff;
            border-radius: 16px;
            box-shadow: 0 8px 20px rgba(253, 160, 133, 0.3);
          "
        >
          <h6 class="text-uppercase fw-semibold mb-2" style="letter-spacing: 1.2px">
            Tổng số loại món ăn nhanh
          </h6>
          <p class="display-4 fw-bold mb-0">{{ dashboardData.totalFoods }}</p>
          <router-link
            to="/admin/foods"
            class="mt-3 btn btn-light btn-sm fw-semibold d-inline-flex align-items-center gap-2"
            style="border-radius: 12px; color: #222"
          >
            Quản lý món ăn nhanh
            <lord-icon
              src="https://cdn.lordicon.com/hlejxoqz.json"
              target=".btn"
              trigger="hover"
              style="width: 20px; height: 20px; --lord-icon-primary: #f6d365"
            >
            </lord-icon>
          </router-link>
        </div>

        <div
          class="dashboard-card p-4"
          style="
            flex: 1 1 320px;
            background: linear-gradient(135deg, #ff758c, #ff7eb3);
            color: #fff;
            border-radius: 16px;
            box-shadow: 0 8px 20px rgba(24, 90, 157, 0.3);
          "
        >
          <h6 class="text-uppercase fw-semibold mb-2" style="letter-spacing: 1.2px">
            Tổng số combo
          </h6>
          <p class="display-4 fw-bold mb-0">{{ dashboardData.totalCombos }}</p>
          <router-link
            to="/admin/comboes"
            class="mt-3 btn btn-light btn-sm fw-semibold d-inline-flex align-items-center gap-2"
            style="border-radius: 12px; color: #222"
          >
            Quản lý combo
            <lord-icon
              src="https://cdn.lordicon.com/efxgwrkc.json"
              target=".btn"
              trigger="hover"
              style="width: 20px; height: 20px; --lord-icon-primary: #ff758c"
            >
            </lord-icon>
          </router-link>
        </div>

        <div
          class="dashboard-card p-4"
          style="
            flex: 1 1 320px;
            background: linear-gradient(135deg, #43cea2, #185a9d);
            color: #fff;
            border-radius: 16px;
            box-shadow: 0 8px 20px rgba(24, 90, 157, 0.3);
          "
        >
          <h6 class="text-uppercase fw-semibold mb-2" style="letter-spacing: 1.2px">
            Tổng số loại sản phẩm
          </h6>
          <p class="display-4 fw-bold mb-0">{{ dashboardData.totalCategories }}</p>
          <router-link
            to="/admin/categories"
            class="mt-3 btn btn-light btn-sm fw-semibold d-inline-flex align-items-center gap-2"
            style="border-radius: 12px; color: #222"
          >
            Quản lý loại sản phẩm
            <lord-icon
              src="https://cdn.lordicon.com/dutqakce.json"
              target=".btn"
              trigger="hover"
              style="width: 20px; height: 20px; --lord-icon-primary: #43cea2"
            >
            </lord-icon>
          </router-link>
        </div>
      </div>

      <div class="col-12 col-lg-4 d-flex align-items-start justify-content-center">
        <div
          class="dashboard-card p-4 w-100"
          style="
            background: #e6f4ea;
            color: #326a35;
            border-radius: 16px;
            box-shadow: 0 8px 15px rgba(50, 168, 82, 0.3);
          "
        >
          <h6 class="text-uppercase fw-semibold mb-3" style="letter-spacing: 1.2px">Đơn hàng</h6>

          <div class="d-flex flex-column gap-2 mb-3">
            <div class="p-3" style="background: rgba(255, 255, 255, 0.6); border-radius: 10px">
              <div class="text-muted">Chưa giao</div>
              <div class="fw-bold fs-4">{{ dashboardData.orders.pending }}</div>
            </div>
            <div class="p-3" style="background: rgba(255, 255, 255, 0.6); border-radius: 10px">
              <div class="text-muted">Đang giao</div>
              <div class="fw-bold fs-4">{{ dashboardData.orders.shipping }}</div>
            </div>
            <div class="p-3" style="background: rgba(255, 255, 255, 0.6); border-radius: 10px">
              <div class="text-muted">Đã giao</div>
              <div class="fw-bold fs-4">{{ dashboardData.orders.delivered }}</div>
            </div>
          </div>

          <router-link
            to="/admin/orders"
            class="mt-2 btn btn-light btn-sm fw-semibold d-inline-flex align-items-center justify-content-center gap-2 w-100"
            style="border-radius: 12px; color: #2a5934"
          >
            Xem tất cả đơn hàng
            <lord-icon
              src="https://cdn.lordicon.com/euduggnx.json"
              trigger="hover"
              target=".btn"
              style="width: 20px; height: 20px; --lord-icon-primary: #326a35"
            >
            </lord-icon>
          </router-link>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useDashboardStore } from "@/stores/dashboardAdminStore";
import type { Dashboard } from "@/types/dashboardAdmin";
import { onMounted, ref } from "vue";

const dashboardStore = useDashboardStore();

const dashboardData = ref<Dashboard>({
  totalUsers: 0,
  totalFoods: 0,
  totalCombos: 0,
  totalCategories: 0,
  orders: {
    pending: 0,
    shipping: 0,
    delivered: 0,
  },
});

const fetchDashboardData = async () => {
  await dashboardStore.fetchDashboard();
  if (dashboardStore.stats) {
    dashboardData.value = dashboardStore.stats;
  }
};

onMounted(() => {
  fetchDashboardData();
});
</script>

<style>
#dashboard .btn:hover {
  background-color: white;
}

lord-icon {
  display: inline-block;
}
</style>
