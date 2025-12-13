import type { Message } from "@/types/chat";
import api from ".";

export async function getMessages(conversationId: number): Promise<Message[]> {
  const res = await api.get<Message[]>(`/Messages/conversation/${conversationId}`);
  return res.data;
}
