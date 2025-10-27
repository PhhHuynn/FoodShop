<template>
  <div
    style="height: calc(100vh - 50px); margin-top: -70px"
    class="login-page d-flex justify-content-center align-items-center position-relative bg-light"
  >
    <!-- Decorative plates -->
    <img src="/img/login-1.png" alt="food plate" class="plate plate-left position-absolute" />
    <img src="/img/login-2.png" alt="food plate" class="plate plate-center position-absolute" />
    <img src="/img/login-3.png" alt="food plate" class="plate plate-right position-absolute" />
    <img src="/img/login-4.png" alt="lemon" class="plate plate-lemon position-absolute" />

    <div class="card shadow p-4 rounded-4" style="width: 400px">
      <h3 class="text-center mb-3">Đăng ký</h3>

      <p class="text-center text-muted mb-4">
        Sử dụng tài khoản Google của bạn để đăng ký nhanh chóng.
      </p>

      <GoogleLogin :callback="onGoogle" class="w-100" />

      <p class="text-center mt-4 mb-0 text-muted">
        Đã có tài khoản? <RouterLink to="/login">Đăng nhập</RouterLink>
      </p>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useAuthStore } from "@/stores/authStore";
import { GoogleLogin } from "vue3-google-login";
import { useRouter } from "vue-router";

const router = useRouter();
const authStore = useAuthStore();

async function onGoogle(response: { credential: string; clientId?: string }) {
  const idToken = response.credential;
  try {
    await authStore.loginGoogle(idToken);
    router.push(`/dashboard/`);
  } catch (error) {
    console.error("Lỗi đăng nhập Google:", error);
  }
}
</script>

<style scoped>
.login-page {
  background-color: #faf9f6;
  overflow: hidden;
}

.plate {
  width: 300px;
  height: 300px;
  object-fit: contain;
  opacity: 0.9;
}

.plate-left {
  left: -200px;
  top: 40%;
  animation: enter-left 1.2s cubic-bezier(0.2, 0.8, 0.2, 1) forwards;
}

.plate-center {
  top: 25%;
  right: -200px;
  animation: enter-center 1.3s cubic-bezier(0.2, 0.8, 0.2, 1) 0.15s forwards;
}

.plate-right {
  right: -200px;
  top: 55%;
  animation: enter-right 1.2s cubic-bezier(0.2, 0.8, 0.2, 1) 0.1s forwards;
}

.plate-lemon {
  width: 150px;
  height: 150px;
  object-fit: contain;
  opacity: 0.9;
  left: -200px;
  top: 80%;
  animation: enter-lemon 1.2s cubic-bezier(0.2, 0.8, 0.2, 1) forwards;
}

@keyframes enter-lemon {
  0% {
    transform: rotate(-20deg) scale(0.8);
  }
  70% {
    transform: translateX(340px) rotate(5deg) scale(1.05);
  }
  100% {
    transform: translateX(300px) rotate(0deg) scale(1);
    left: 0%;
    top: 65%;
  }
}

@keyframes enter-left {
  0% {
    transform: rotate(-15deg) scale(0.8);
  }
  70% {
    transform: translateX(220px) rotate(6deg) scale(1.05);
  }
  100% {
    transform: translateX(100px) rotate(0deg) scale(1);
    left: 0%;
    top: 20%;
  }
}

@keyframes enter-right {
  0% {
    transform: rotate(15deg) scale(0.8);
  }
  70% {
    transform: translateX(-220px) rotate(-6deg) scale(1.05);
  }
  100% {
    transform: translateX(-130px) rotate(0deg) scale(1);
    right: 5%;
    top: 57%;
  }
}

@keyframes enter-center {
  0% {
    transform: rotate(-10deg) scale(0.8);
  }
  70% {
    transform: translateX(-180px) rotate(5deg) scale(1.05);
  }
  100% {
    transform: translateX(-90px) rotate(0deg) scale(1);
    top: 10%;
    right: 0%;
  }
}
</style>
