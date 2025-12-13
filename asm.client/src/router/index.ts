import { useAuthStore } from "@/stores/authStore";
import ChatAdmin from "@/views/admin/ChatAdmin.vue";
import DashboardAdminView from "@/views/admin/DashboardAdminView.vue";
import DashboardView from "@/views/DashboardView.vue";
import HomeView from "@/views/HomeView.vue";
import LogIn from "@/views/LogIn.vue";
import OrdersView from "@/views/OrdersView.vue";
import SignUp from "@/views/SignUp.vue";
import { createRouter, createWebHistory } from "vue-router";
import "vue-router";

declare module "vue-router" {
  interface RouteMeta {
    requiresAuth?: boolean;
    roles?: string[];
  }
}

const routes = [
  {
    path: "/",
    name: "home",
    component: HomeView,
  },
  {
    path: "/login",
    name: "login",
    component: LogIn,
  },
  {
    path: "/signup",
    name: "signup",
    component: SignUp,
  },
  {
    path: "/dashboard",
    name: "dashboard",
    component: DashboardView,
    meta: {
      requiresAuth: true,
    },
  },
  {
    path: "/orders",
    name: "orders",
    component: OrdersView,
    meta: {
      requiresAuth: true,
    },
  },
  {
    path: "/admin/dashboard",
    name: "dashboardAdmin",
    component: DashboardAdminView,
    meta: {
      requiresAuth: true,
      roles: ["Admin"],
    },
  },
  {
    path: "/admin/chat",
    name: "chatAdmin",
    component: ChatAdmin,
    meta: {
      requiresAuth: true,
      roles: ["Admin"],
    },
  },
  {
    path: "/admin/foods",
    children: [
      { path: "", component: () => import("@/views/admin/foods/FoodList.vue") },
      { path: "add", component: () => import("@/views/admin/foods/FoodForm.vue") },
      { path: "edit/:id", component: () => import("@/views/admin/foods/FoodForm.vue") },
      { path: ":id", component: () => import("@/views/admin/foods/FoodDetail.vue") },
    ],
    meta: { requiresAuth: true, roles: ["Admin"] },
  },
  {
    path: "/admin/orders",
    children: [
      { path: "", component: () => import("@/views/admin/orders/OrderList.vue") },
      { path: "edit/:id", component: () => import("@/views/admin/orders/OrderForm.vue") },
      { path: ":id", component: () => import("@/views/admin/orders/OrderDetail.vue") },
    ],
    meta: { requiresAuth: true, roles: ["Admin"] },
  },
  {
    path: "/admin/comboes",
    children: [
      { path: "", component: () => import("@/views/admin/comboes/ComboList.vue") },
      { path: "add", component: () => import("@/views/admin/comboes/ComboForm.vue") },
      { path: "edit/:id", component: () => import("@/views/admin/comboes/ComboForm.vue") },
      { path: ":id", component: () => import("@/views/admin/comboes/ComboDetail.vue") },
    ],
    meta: { requiresAuth: true, roles: ["Admin"] },
  },
  {
    path: "/admin/categories",
    children: [
      { path: "", component: () => import("@/views/admin/categories/CategoryList.vue") },
      { path: "add", component: () => import("@/views/admin/categories/CategoryForm.vue") },
      { path: "edit/:id", component: () => import("@/views/admin/categories/CategoryForm.vue") },
      { path: ":id", component: () => import("@/views/admin/categories/CategoryDetail.vue") },
    ],
    meta: { requiresAuth: true, roles: ["Admin"] },
  },
  {
    path: "/admin/users",
    children: [
      { path: "", component: () => import("@/views/admin/users/UserList.vue") },
      { path: "add", component: () => import("@/views/admin/users/UserForm.vue") },
      { path: "edit/:id", component: () => import("@/views/admin/users/UserForm.vue") },
      { path: ":id", component: () => import("@/views/admin/users/UserDetail.vue") },
    ],
    meta: { requiresAuth: true, roles: ["Admin"] },
  },
];

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
});

router.beforeEach((to, from, next) => {
  const store = useAuthStore();
  if (to.meta.requiresAuth && !store.token) {
    return next("/login");
  }

  if (to.meta.roles && store.user?.role && !to.meta.roles.includes(store.user?.role)) {
    return next("/403");
  }

  next();
});

export default router;
