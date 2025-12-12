import { defineStore } from "pinia";
import { ref } from "vue";
import hubConnection from "../signalr/hubConnection";
import { getMessages, saveMessage } from "../api/messageService";
import type { Message, MessageCreate } from "@/types/chat";
import { HubConnectionState } from "@microsoft/signalr";

export const useMessageStore = defineStore("message", () => {
  const messages = ref<Message[]>([]);
  const currentConversation = ref<number | null>(null);

  async function loadMessages(conversationId: number) {
    currentConversation.value = conversationId;
    console.log("Đang load message");
    try {
      messages.value = await getMessages(conversationId);
    } catch (err) {
      messages.value = [];
      console.error(err);
    }
  }

  async function sendMessage(senderId: string, content: string) {
    console.log("đang gửi");
    try {
      const message: MessageCreate = {
        conversationId: currentConversation.value!,
        senderId,
        content,
      };

      if (hubConnection.state !== HubConnectionState.Connected) {
        await hubConnection.start();
      }

      await saveMessage(message);
      await hubConnection.invoke(
        "SendMessage",
        message.conversationId.toString(),
        senderId,
        content
      );
      messages.value = await getMessages(currentConversation.value!);
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

          hubConnection.on("ReceiveMessage", (senderId, message) => {
            messages.value.push({
              id: 0,
              senderId,
              content: message,
              conversationId,
            });
          });
        })
        .catch((err) => console.error("Error starting existing connection: ", err));

      return;
    }
  }

  return { messages, loadMessages, sendMessage, connectSignalR, currentConversation };
});
