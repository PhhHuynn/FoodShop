// stores/dashboard.ts
import { defineStore } from "pinia";
import { ref } from "vue";
import { GetDashboardStats } from "@/api/dashboardAdminService";
import type { Dashboard } from "@/types/dashboardAdmin";

export const useDashboardStore = defineStore("dashboard", () => {
  const stats = ref<Dashboard | null>(null);
  const loading = ref(false);

  async function fetchDashboard() {
    loading.value = true;
    try {
      stats.value = await GetDashboardStats();
    } catch (err) {
      console.error("Lỗi khi tải Combos: ", err);
    } finally {
      loading.value = false;
    }
  }

  return { stats, loading, fetchDashboard };
});
