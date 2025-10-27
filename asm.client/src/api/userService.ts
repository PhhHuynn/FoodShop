import api from "./index";
import { type User, type UserUpdate } from "@/types/user";

export async function getUsers(): Promise<User[]> {
  const res = await api.get<User[]>("/users");
  return res.data;
}

export async function getUser(id: string): Promise<User> {
  const res = await api.get<User>(`/users/${id}`);
  return res.data;
}

export async function createUser(user: Omit<User, "id">): Promise<User> {
  const res = await api.post<User>("/users", user);
  return res.data;
}

export async function updateUser(id: string, user: UserUpdate): Promise<void> {
  await api.patch(`/users/${id}`, user);
}

export async function deleteUser(id: string): Promise<void> {
  await api.delete(`/users/${id}`);
}
