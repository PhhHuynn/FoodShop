<template>
  <div
    class="login-page d-flex justify-content-center align-items-center vh-100 position-relative bg-light"
  >
    <!-- Decorative plates -->
    <img src="/img/login-1.png" alt="food plate" class="plate plate-left position-absolute" />
    <img src="/img/login-2.png" alt="food plate" class="plate plate-center position-absolute" />
    <img src="/img/login-3.png" alt="food plate" class="plate plate-right position-absolute" />
    <img src="/img/login-4.png" alt="lemon" class="plate plate-lemon position-absolute" />

    <div class="card shadow p-4 rounded-4">
      <h3 class="text-center mb-4">Đăng nhập</h3>
      <form @submit.prevent="onSubmit" style="width: 400px">
        <div class="mb-3">
          <label for="email" class="form-label">Email</label>
          <input
            v-model="email"
            type="email"
            id="email"
            class="form-control"
            required
            placeholder="you@example.com"
          />
        </div>
        <div class="mb-3">
          <label for="password" class="form-label">Mật khẩu</label>
          <input
            v-model="password"
            type="password"
            id="password"
            class="form-control"
            required
            placeholder="••••••••"
          />
        </div>
        <button type="submit" class="btn btn-primary w-100 mb-3">Đăng nhập</button>
        <div class="text-center text-muted mb-3">Hoặc</div>
        <GoogleLogin :callback="onGoogle" class="w-100" />
      </form>
      <p class="text-center mt-3 mb-0 text-muted">Không có tài khoản? <a href="#">Đăng ký</a></p>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useAuthStore } from "@/stores/authStore";
import { GoogleLogin } from "vue3-google-login";
import { ref } from "vue";
import { useRouter } from "vue-router";

const router = useRouter();
const authStore = useAuthStore();

const email = ref("");
const password = ref("");

async function onSubmit() {
  try {
    await authStore.loginUser({ email: email.value, password: password.value });
    email.value = "";
    password.value = "";
    router.push("/home");
  } catch (error) {
    console.error("Login failed:", error);
  }
}

async function onGoogle(response: { credential: string; clientId?: string }) {
  const idToken = response.credential;
  try {
    await authStore.loginGoogle(idToken);
    router.push("/");
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
