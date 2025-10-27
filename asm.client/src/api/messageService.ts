import type { Message, MessageCreate } from "@/types/chat";
import api from ".";

export async function getMessages(conversationId: number): Promise<Message[]> {
  const res = await api.get<Message[]>(`/Messages/conversation${conversationId}`);
  return res.data;
}

export async function saveMessage(message: MessageCreate): Promise<void> {
  await api.post<Message>(`/Messages/send/`, message);
}
