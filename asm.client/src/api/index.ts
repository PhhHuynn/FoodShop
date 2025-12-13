import router from "@/router";
import axios from "axios";
const api = axios.create({
  baseURL: "https://localhost:7119/api",
  timeout: 5000,
  headers: {
    "Content-Type": "application/json",
  },
});

api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem("token");
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }

    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      alert("Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại.");

      localStorage.removeItem("accessToken");
      localStorage.removeItem("user");

      router.push("/login");
    }

    return Promise.reject(error);
  }
);

export default api;
