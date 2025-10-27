import api from "./index";
import { type Dashboard } from "@/types/dashboardAdmin";

export async function GetDashboardStats(): Promise<Dashboard> {
  const res = await api.get<Dashboard>("/dashboardAdmin");
  return res.data;
}
