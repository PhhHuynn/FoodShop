<template>
  <h1 class="my-5 fw-bold text-center">Admin Dashboard</h1>

  <div class="container py-3 my-3" id="dashboard">
    <div class="row g-4 justify-content-center">
      <!-- Hàng 1 -->
      <div class="col-12 col-lg-3">
        <div class="dashboard-card card-blue">
          <h6 class="title">Tổng số tài khoản</h6>
          <p class="display-4 fw-bold mb-0">{{ dashboardData.totalUsers }}</p>
          <router-link to="/admin/users" class="dashboard-btn">
            Quản lý tài khoản
            <lord-icon
              target=".dashboard-btn"
              src="https://cdn.lordicon.com/dygfbwwx.json"
              trigger="hover"
            ></lord-icon>
          </router-link>
        </div>
      </div>

      <div class="col-12 col-lg-3">
        <div class="dashboard-card card-orange">
          <h6 class="title">Tổng số loại món ăn</h6>
          <p class="display-4 fw-bold mb-0">{{ dashboardData.totalFoods }}</p>
          <router-link to="/admin/foods" class="dashboard-btn">
            Quản lý món ăn nhanh
            <lord-icon
              target=".dashboard-btn"
              src="https://cdn.lordicon.com/hlejxoqz.json"
              trigger="hover"
            ></lord-icon>
          </router-link>
        </div>
      </div>

      <div class="col-12 col-lg-3">
        <div class="dashboard-card card-pink">
          <h6 class="title">Tổng số combo</h6>
          <p class="display-4 fw-bold mb-0">{{ dashboardData.totalCombos }}</p>
          <router-link to="/admin/comboes" class="dashboard-btn">
            Quản lý combo
            <lord-icon
              target=".dashboard-btn"
              src="https://cdn.lordicon.com/efxgwrkc.json"
              trigger="hover"
            ></lord-icon>
          </router-link>
        </div>
      </div>

      <div class="col-12 col-lg-3">
        <div class="dashboard-card card-green">
          <h6 class="title">Tổng số loại sản phẩm</h6>
          <p class="display-4 fw-bold mb-0">{{ dashboardData.totalCategories }}</p>
          <router-link to="/admin/categories" class="dashboard-btn">
            Quản lý loại sản phẩm
            <lord-icon
              target=".dashboard-btn"
              src="https://cdn.lordicon.com/dutqakce.json"
              trigger="hover"
            ></lord-icon>
          </router-link>
        </div>
      </div>

      <div class="col-12 col-lg-6">
        <div class="dashboard-card card-lightgreen">
          <h6 class="title">Đơn hàng</h6>
          <div class="d-flex flex-column gap-2 mb-3">
            <div class="order-item">
              <div class="text-muted">Chưa giao</div>
              <div class="fw-bold fs-4">{{ dashboardData.orders.pending }}</div>
            </div>
            <div class="order-item">
              <div class="text-muted">Đang giao</div>
              <div class="fw-bold fs-4">{{ dashboardData.orders.shipping }}</div>
            </div>
            <div class="order-item">
              <div class="text-muted">Đã giao</div>
              <div class="fw-bold fs-4">{{ dashboardData.orders.delivered }}</div>
            </div>
          </div>
          <router-link to="/admin/orders" class="dashboard-btn w-100">
            Xem tất cả đơn hàng
            <lord-icon src="https://cdn.lordicon.com/euduggnx.json" trigger="hover"></lord-icon>
          </router-link>
        </div>
      </div>

      <!-- Mục Chat Support -->
      <div class="col-12 col-lg-6">
        <div class="dashboard-card card-purple">
          <h6 class="title">Hỗ trợ khách hàng</h6>
          <p class="fs-5 mb-3">Trò chuyện trực tiếp với khách hàng đang cần hỗ trợ</p>
          <router-link to="/admin/chat" class="dashboard-btn w-100">
            Tới trang chat hỗ trợ
            <lord-icon
              target=".dashboard-btn"
              src="https://cdn.lordicon.com/ayhtotha.json"
              trigger="hover"
            ></lord-icon>
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
  orders: { pending: 0, shipping: 0, delivered: 0 },
});

const fetchDashboardData = async () => {
  await dashboardStore.fetchDashboard();
  if (dashboardStore.stats) {
    dashboardData.value = dashboardStore.stats;
  }
};

onMounted(fetchDashboardData);
</script>

<style scoped>
.dashboard-card {
  padding: 1.8rem;
  border-radius: 16px;
  color: #fff;
  box-shadow: 0 8px 20px rgba(0, 0, 0, 0.15);
  height: 100%;
}
.title {
  text-transform: uppercase;
  font-weight: 600;
  letter-spacing: 1.2px;
  margin-bottom: 0.5rem;
}
.dashboard-btn {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  gap: 3px;
  margin-top: 1rem;
  font-weight: 600;
  font-size: 0.9rem;
  background: #fff;
  color: #222;
  border-radius: 10px;
  padding: 4px 10px;
  text-decoration: none;
  transition: all 0.2s ease;
}
.dashboard-btn:hover {
  background: #f8f8f8;
  transform: translateY(-2px);
}
.dashboard-btn lord-icon {
  width: 25px;
  height: 25px;
}
.order-item {
  background: rgba(255, 255, 255, 0.7);
  border-radius: 10px;
  padding: 10px 14px;
}

.card-blue {
  background: linear-gradient(135deg, #667eea, #764ba2);
}
.card-orange {
  background: linear-gradient(135deg, #f6d365, #fda085);
}
.card-pink {
  background: linear-gradient(135deg, #ff758c, #ff7eb3);
}
.card-green {
  background: linear-gradient(135deg, #43cea2, #185a9d);
}
.card-lightgreen {
  background: linear-gradient(135deg, #86b490, #4f7c54);
  color: #2c5f53;
}
.card-purple {
  background: linear-gradient(135deg, #9b7dbd, #6c4791);
}
</style>
