<template>
  <nav
    class="navbar navbar-expand-lg border-bottom position-fixed top-0 start-0 end-0 z-2 bg-white"
    style="height: 70px"
  >
    <div class="container d-flex justify-content-between align-items-center">
      <a class="navbar-brand d-flex align-items-center" href="/">
        <img src="/img/logo.png" alt="logo cook food" style="width: 90px" class="me-2" />
      </a>

      <div class="d-flex align-items-center gap-3">
        <template v-if="auth.token">
          <button v-if="userRole != null && userRole == 'Admin'" class="btn btn-warning">
            Quản lý
          </button>

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
                <router-link to="/dashboard" class="dropdown-item" href="#">My Profile</router-link>
              </li>
              <li><a class="dropdown-item" href="#" @click="logout">Logout</a></li>
            </ul>
          </div>
        </template>
        <template v-else>
          <router-link to="/login" class="btn btn-outline-a me-2">Đăng nhập</router-link>
          <button class="btn btn-warning">Đăng ký</button>
        </template>
      </div>
    </div>
  </nav>
</template>

<script setup lang="ts">
import { useAuthStore } from "@/stores/authStore";
import { jwtDecode } from "jwt-decode";
import { useRouter } from "vue-router";
import { computed } from "vue";

interface TokenPayload {
  "http://schemas.microsoft.com/ws/2008/06/identity/claims/role": string;
}

const auth = useAuthStore();
const router = useRouter();
const userRole = computed<string | null>(() => {
  const token = auth.token;
  if (!token) {
    return null;
  }

  try {
    const payload: TokenPayload = jwtDecode(token);

    return payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
  } catch (error) {
    console.error("Failed to decode token:", error);
    return null;
  }
});

const logout = () => {
  auth.logout();
  router.push("/");
};
</script>
