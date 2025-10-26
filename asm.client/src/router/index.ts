import { useAuthStore } from "@/stores/authStore";
import HomeView from "@/views/HomeView.vue";
import LogIn from "@/views/LogIn.vue";
import { createRouter, createWebHistory } from "vue-router";

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
];

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
});

router.beforeEach((to, from, next) => {
  const store = useAuthStore();
  if (to.meta.requiresAuth && !store.token) {
    next("/login");
  } else {
    next();
  }
});

export default router;
