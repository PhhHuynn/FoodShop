import { login, loginWithGoogle } from "@/api/authService";
import { type AuthResponse, type LoginUser } from "@/types/user";
import { defineStore } from "pinia";
import { ref } from "vue";

export const useAuthStore = defineStore("auth", () => {
  const token = ref<string | null>(localStorage.getItem("token"));
  const user = ref<AuthResponse["user"] | null>(
    localStorage.getItem("user") ? JSON.parse(localStorage.getItem("user")!) : null
  );

  function setSession(data: AuthResponse) {
    token.value = data.token;
    user.value = data.user;

    localStorage.setItem("token", data.token);
    localStorage.setItem("user", JSON.stringify(data.user));
  }

  function clearSession() {
    token.value = null;
    user.value = null;
    localStorage.removeItem("token");
    localStorage.removeItem("user");
  }

  async function loginUser(credentials: LoginUser): Promise<AuthResponse> {
    const res = await login(credentials);
    setSession(res);
    return res;
  }

  async function loginGoogle(idToken: string): Promise<AuthResponse> {
    const res = await loginWithGoogle(idToken);
    setSession(res);
    return res;
  }

  function logout() {
    clearSession();
  }

  return {
    token,
    user,
    loginUser,
    loginGoogle,
    logout,
  };
});
