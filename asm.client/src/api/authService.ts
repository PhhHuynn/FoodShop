import api from ".";
import type { AccountUpdate, AuthResponse, LoginUser } from "@/types/user";

export async function login(user: LoginUser): Promise<AuthResponse> {
  const res = await api.post("/account/login", user);
  return res.data;
}

export async function loginWithGoogle(id_token: string): Promise<AuthResponse> {
  const res = await api.post("/account/google-login", { idToken: id_token });
  return res.data;
}

export async function updateAccount(id: string, user: AccountUpdate): Promise<void> {
  await api.patch(`/account/${id}`, user);
}
