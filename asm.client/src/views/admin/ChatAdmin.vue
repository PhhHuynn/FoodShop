<template>
  <div class="d-flex bg-light" style="height: calc(100vh - 70px); margin-top: -50px">
    <div class="border-end bg-white" style="width: 25%">
      <div class="p-3 border-bottom fw-bold d-flex justify-content-between align-items-center">
        <span>Danh sách chat</span>
        <button class="btn btn-sm bg-transparent" @click="conversationStore.fetchConversations()">
          <lord-icon
            src="https://cdn.lordicon.com/valwmkhs.json"
            trigger="hover"
            target=".btn"
            style="width: 25px; height: 25px"
          >
          </lord-icon>
        </button>
      </div>

      <ul class="list-group list-group-flush">
        <li
          v-for="c in conversationStore.conversations"
          :key="c.id"
          class="list-group-item list-group-item-action d-flex justify-content-between align-items-start"
          :class="{ active: messageStore.currentConversationId === c.id }"
          @click="selectConversation(c)"
        >
          <div>
            <div class="fw-semibold">{{ c.customerName || "Khách mới" }}</div>
            <small class="text-muted">
              {{
                c.messages?.length ? c.messages[c.messages.length - 1]?.content : "Chưa có tin nhắn"
              }}
            </small>
          </div>

          <span
            class="badge rounded-pill"
            :class="{
              'bg-success': c.status === ConversationStatus.Closed,
              'bg-danger': c.status === ConversationStatus.Pending,
              'bg-primary': c.status === ConversationStatus.Open,
              'bg-secondary': c.status === ConversationStatus.Archived,
            }"
          >
            {{
              c.status === 1
                ? "Open"
                : c.status === 2
                ? "Closed"
                : c.status === 3
                ? "Pending"
                : c.status === 4
                ? "Archived"
                : "Unknown"
            }}
          </span>
        </li>
      </ul>
    </div>

    <div class="flex-grow-1 d-flex flex-column">
      <div class="p-3 border-bottom bg-white fw-bold d-flex justify-content-between">
        <span>
          {{ selectedConversation.customerName }}
        </span>
        <div v-if="selectedConversation.id" class="dropdown">
          <button
            class="btn btn-sm btn-outline-secondary dropdown-toggle"
            type="button"
            data-bs-toggle="dropdown"
            aria-expanded="false"
            style="min-width: 120px"
          >
            {{ ConversationStatus[selectedConversation.status] }}
          </button>

          <ul class="dropdown-menu">
            <li>
              <button class="dropdown-item" @click="onStatusClick(ConversationStatus.Open)">
                Open
              </button>
            </li>
            <li>
              <button class="dropdown-item" @click="onStatusClick(ConversationStatus.Closed)">
                Closed
              </button>
            </li>
            <li>
              <button class="dropdown-item" @click="onStatusClick(ConversationStatus.Pending)">
                Pending
              </button>
            </li>
            <li>
              <button class="dropdown-item" @click="onStatusClick(ConversationStatus.Archived)">
                Archived
              </button>
            </li>
          </ul>
        </div>
      </div>

      <div class="flex-grow-1 p-3 overflow-auto" id="chat-box">
        <div v-if="!messageStore.currentConversationId" class="text-center text-muted mt-5">
          Chọn một cuộc trò chuyện để bắt đầu
        </div>

        <div v-else>
          <div
            v-for="m in messageStore.messages"
            :key="m.id"
            class="mb-3 d-flex"
            :class="{
              'justify-content-end': m.senderType === 'Admin',
              'justify-content-start': m.senderType !== 'Admin',
            }"
          >
            <div
              class="d-flex flex-column align-items-start"
              :class="m.senderType === 'Admin' ? 'align-items-end' : 'align-items-start'"
            >
              <div
                class="p-2 rounded-3"
                :class="
                  m.senderType === 'Admin' ? 'bg-warning text-white' : 'bg-body-secondary border'
                "
              >
                {{ m.content }}
              </div>
              <small
                v-if="m.senderType === 'Admin' && m.senderName"
                class="text-muted mt-1"
                style="font-size: 0.7rem"
              >
                Người gửi: {{ m.senderName }}
              </small>
            </div>
          </div>
        </div>
      </div>

      <div v-if="messageStore.currentConversationId" class="p-3 border-top bg-white">
        <div class="input-group">
          <input
            type="text"
            v-model="messageText"
            class="form-control"
            placeholder="Nhập tin nhắn..."
            @keyup.enter="sendMessage"
          />
          <button class="btn btn-primary" @click="sendMessage">Gửi</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from "vue";
import { useConversationStore } from "@/stores/conversationStore";
import { useMessageStore } from "@/stores/messageStore";
import { useAuthStore } from "@/stores/authStore";
import { ConversationStatus, type Conversation, type MessageCreate } from "@/types/chat";

const conversationStore = useConversationStore();
const messageStore = useMessageStore();
const authStore = useAuthStore();

const adminId = authStore.user!.id;
const messageText = ref("");

const selectedConversation = computed(() => {
  const c = conversationStore.conversations.find(
    (x) => x.id === messageStore.currentConversationId
  );

  if (!c) {
    return {
      customerName: "Chưa chọn cuộc trò chuyện",
      status: null,
      id: null,
    };
  }

  return {
    customerName: c.customerName,
    status: c.status,
    id: messageStore.currentConversationId,
  };
});

async function selectConversation(conversation: Conversation) {
  await messageStore.loadMessages(conversation.id);
  messageStore.connectSignalR(conversation.id);
  messageStore.currentConversationId = conversation.id;
  console.log(messageStore.messages);
}

async function sendMessage() {
  if (!messageText.value.trim() || !messageStore.currentConversationId) return;
  const newMessage: MessageCreate = {
    content: messageText.value,
    conversationId: messageStore.currentConversationId,
    senderId: adminId,
    senderType: "Admin",
  };
  await messageStore.sendMessage(newMessage);
  messageText.value = "";
}

onMounted(async () => {
  await conversationStore.fetchConversations();
});

async function onStatusClick(status: ConversationStatus) {
  await conversationStore.editConversation(selectedConversation.value.id, {
    id: selectedConversation.value.id,
    status,
  });
}
</script>

<style scoped>
#chat-box {
  background-color: #f8f9fa;
}
.list-group-item.active {
  background-color: #e0a800 !important;
  color: white !important;
  border-color: #7c600b;
}
</style>
