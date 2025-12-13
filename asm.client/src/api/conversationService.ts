import api from "./index";
import { type Conversation, type ConversationCreate, type ConversationUpdate } from "@/types/chat";

export async function getConversations(): Promise<Conversation[]> {
  console.log("đang get");
  const res = await api.get<Conversation[]>("/Conversation");
  return res.data;
}

export async function getConversation(id: number): Promise<Conversation> {
  const res = await api.get<Conversation>(`/Conversation/${id}`);
  return res.data;
}

export async function createConversation(food: ConversationCreate): Promise<Conversation> {
  console.log("đang tạo");
  const res = await api.post<Conversation>("/Conversation", food);
  return res.data;
}

export async function updateConversation(id: number, food: ConversationUpdate): Promise<void> {
  await api.put(`/Conversation/${id}`, food);
}

export async function deleteConversation(id: number): Promise<void> {
  await api.delete(`/Conversation/${id}`);
}
