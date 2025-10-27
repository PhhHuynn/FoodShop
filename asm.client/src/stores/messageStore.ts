import { defineStore } from "pinia";
import { ref } from "vue";
import hubConnection from "../signalr/hubConnection";
import { getMessages, saveMessage } from "../api/messageService";
import type { Message, MessageCreate } from "@/types/chat";

export const useMessageStore = defineStore("message", () => {
  const messages = ref<Message[]>([]);
  const currentConversation = ref<number | null>(null);

  async function loadMessages(conversationId: number) {
    currentConversation.value = conversationId;
    try {
      messages.value = await getMessages(conversationId);
    } catch (err) {
      messages.value = [];
      console.error(err);
    }
  }

  async function sendMessage(senderId: string, receiverId: string, content: string) {
    const message: MessageCreate = {
      conversationId: currentConversation.value!,
      senderId,
      content,
    };

    await saveMessage(message);
    await hubConnection.invoke("SendMessage", senderId, receiverId, content);
  }

  function connectSignalR(userId: string) {
    hubConnection.start().then(() => {
      hubConnection.on("ReceiveMessage", (senderId, message) => {
        messages.value.push({
          id: 0,
          senderId,
          content: message,
          conversationId: currentConversation.value ?? 0,
        });
      });
      hubConnection.invoke("AddUserConnection", userId).catch(console.error);
    });
  }

  return { messages, loadMessages, sendMessage, connectSignalR };
});
