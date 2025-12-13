import { defineStore } from "pinia";
import { ref } from "vue";
import hubConnection from "../signalr/hubConnection";
import { getMessages } from "../api/messageService";
import type { Message, MessageCreate } from "@/types/chat";
import { HubConnectionState } from "@microsoft/signalr";

export const useMessageStore = defineStore("message", () => {
  const messages = ref<Message[]>([]);
  const currentConversationId = ref<number | null>(null);

  async function loadMessages(conversationId: number) {
    currentConversationId.value = conversationId;
    console.log("Đang load message");
    try {
      messages.value = await getMessages(conversationId);
    } catch (err) {
      messages.value = [];
      console.error(err);
    }
  }

  async function sendMessage(message: MessageCreate) {
    try {
      if (hubConnection.state !== HubConnectionState.Connected) {
        await hubConnection.start();
      }

      await hubConnection.invoke("SendMessage", message);
      messages.value = await getMessages(currentConversationId.value!);
    } catch (err) {
      console.error("Lỗi khi gửi message:", err);
    }
  }

  function connectSignalR(conversationId: number) {
    if (!hubConnection) {
      console.error("SignalR HubConnection object is null or undefined.");
      return;
    }

    if (hubConnection.state !== HubConnectionState.Disconnected) {
      console.log(`SignalR is already in state: ${hubConnection.state}. Skipping start.`);
      return;
    }

    if (hubConnection.state === HubConnectionState.Disconnected) {
      hubConnection
        .start()
        .then(async () => {
          await hubConnection.invoke("JoinConversation", conversationId.toString());

          hubConnection.on("ReceiveMessage", (m) => {
            messages.value.push({
              id: m.id,
              content: m.content,
              conversationId: m.conversationId,
              senderId: m.senderId,
              senderType: m.senderType,
              senderName: m.senderName,
            });
          });
        })
        .catch((err) => console.error("Error starting existing connection: ", err));

      return;
    }
  }

  return { messages, loadMessages, sendMessage, connectSignalR, currentConversationId };
});
