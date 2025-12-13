<template>
  <div v-if="!authStore.isAdmin" class="position-fixed bottom-0 end-0 m-4" style="z-index: 2000">
    <button
      v-if="!isOpen"
      class="btn btn-warning rounded-circle shadow-lg text-white chat-toggle-btn"
      @click="toggleChat"
      style="width: 60px; height: 60px"
      title="Mở chat hỗ trợ"
    >
      <i class="fa-solid fa-comments fs-4"></i>
    </button>

    <div
      v-else
      class="chat-box shadow-2xl rounded-4 bg-white border d-flex flex-column"
      style="width: 350px; height: 500px"
    >
      <div
        class="chat-header p-3 bg-warning text-white fw-bold d-flex justify-content-between align-items-center rounded-top-4"
      >
        <span>Hỗ trợ khách hàng</span>
        <button
          class="btn btn-sm fs-4 text-white opacity-75 hover:opacity-100"
          @click="toggleChat"
          title="Đóng chat"
        >
          <i class="fa-solid fa-circle-xmark"></i>
        </button>
      </div>

      <div class="flex-grow-1 p-3 overflow-auto chat-messages-container" id="chat-scroll">
        <div v-if="loading" class="text-center text-muted mt-4">
          <div class="spinner-border text-warning spinner-border-sm me-2" role="status">
            <span class="visually-hidden">Loading...</span>
          </div>
          Đang tải...
        </div>
        <div v-else>
          <div
            v-for="m in messageStore.messages"
            :key="m.id"
            class="mb-3 d-flex"
            :class="{
              'justify-content-end': m.senderId === customerId,
              'justify-content-start': m.senderId !== customerId,
            }"
          >
            <div
              class="message-bubble p-2 rounded-3 text-break"
              :class="
                m.senderId === customerId
                  ? 'bg-warning text-white user-bubble'
                  : 'bg-white border text-dark agent-bubble'
              "
              style="max-width: 80%"
            >
              {{ m.content }}
            </div>
          </div>
        </div>
      </div>

      <div class="border-top p-2 bg-white chat-input-area">
        <div class="input-group">
          <input
            type="text"
            class="form-control"
            placeholder="Nhập tin nhắn..."
            v-model="messageText"
            @keyup.enter="sendMessageToAdmin"
            :disabled="!messageStore.currentConversationId"
          />
          <button
            class="btn btn-warning text-white px-2"
            @click="sendMessageToAdmin"
            :disabled="!messageText.trim() || !messageStore.currentConversationId"
          >
            <i class="fa-solid fa-paper-plane"></i>
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from "vue";
import { useMessageStore } from "@/stores/messageStore";
import { useConversationStore } from "@/stores/conversationStore";
import { createConversation } from "@/api/conversationService";
import { useAuthStore } from "@/stores/authStore";

const messageStore = useMessageStore();
const conversationStore = useConversationStore();
const authStore = useAuthStore();

const customerId = authStore.user!.id;
const messageText = ref("");
const isOpen = ref(false);
const loading = ref(false);

async function toggleChat() {
  isOpen.value = !isOpen.value;
  if (isOpen.value) initConversation();
}

async function initConversation() {
  loading.value = true;
  await conversationStore.fetchConversations();

  let conv = conversationStore.conversations.find((c) => c.customerId === customerId);

  if (!conv) {
    conv = await createConversation({
      customerId,
    });
    conversationStore.conversations.push(conv);
  }

  await messageStore.loadMessages(conv.id);
  messageStore.connectSignalR(conv.id);
  messageStore.currentConversationId = conv.id;
  loading.value = false;
}

async function sendMessageToAdmin() {
  console.log("Mới nhận gửi");
  if (!messageText.value.trim() || !messageStore.currentConversationId) return;
  console.log("đang nhận gửi");

  const newMessage = {
    senderId: customerId,
    content: messageText.value,
    senderType: "Customer",
    conversationId: messageStore.currentConversationId,
  };
  await messageStore.sendMessage(newMessage);

  messageText.value = "";
  console.log("đã nhận gửi");
}
</script>
<style scoped>
.chat-box {
  z-index: 2000;
  border-radius: 1rem !important;
  box-shadow: 0 10px 15px -3px rgba(0, 0, 0, 0.1), 0 4px 6px -2px rgba(0, 0, 0, 0.05);
}

.chat-header {
  border-top-left-radius: 1rem !important;
  border-top-right-radius: 1rem !important;
}

.chat-toggle-btn {
  transition: transform 0.2s ease-in-out;
}

.chat-toggle-btn:hover {
  transform: scale(1.05);
}

.message-bubble {
  padding: 0.5rem 0.75rem !important;
  line-height: 1.4;
  font-size: 0.95rem;
}

.user-bubble {
  border-bottom-right-radius: 0.2rem !important;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
}

.agent-bubble {
  border-bottom-left-radius: 0.2rem !important;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.05);
}

.chat-messages-container {
  background-color: #f7f7f7 !important;
}

.btn.text-white {
  --bs-btn-padding-y: 0;
  --bs-btn-padding-x: 0.25rem;
  --bs-btn-font-size: 1rem;
  line-height: 1;
}

.spinner-border {
  vertical-align: middle;
}
</style>
