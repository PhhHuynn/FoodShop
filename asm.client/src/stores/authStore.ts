import { login, loginWithGoogle } from "@/api/authService";
import { type AuthResponse, type LoginUser } from "@/types/user";
import { jwtDecode } from "jwt-decode";
import { defineStore } from "pinia";
import { computed, ref } from "vue";

interface TokenPayload {
  "http://schemas.microsoft.com/ws/2008/06/identity/claims/role": string;
}

export const useAuthStore = defineStore("auth", () => {
  const token = ref<string | null>(localStorage.getItem("token"));
  const user = ref<AuthResponse["user"] | null>(
    localStorage.getItem("user") ? JSON.parse(localStorage.getItem("user")!) : null
  );

  const isLoggedIn = computed(() => !!token.value);
  const userRole = computed(() => {
    if (!token.value) {
      return null;
    }

    try {
      const payload: TokenPayload = jwtDecode(token.value);

      return payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
    } catch (error) {
      console.error("Failed to decode token:", error);
      return null;
    }
  });

  const isAdmin = computed(() => userRole.value == "Admin");
  const isSale = computed(() => userRole.value == "Sale");

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
    isLoggedIn,
    isAdmin,
    isSale,
    userRole,
  };
});
