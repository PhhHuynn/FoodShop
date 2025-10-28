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
          class="list-group-item list-group-item-action"
          :class="{ active: messageStore.currentConversation === c.id }"
          @click="selectConversation(c)"
        >
          <div class="fw-semibold">{{ c.customer?.fullName || "Khách mới" }}</div>
          <small class="text-muted">
            {{
              c.messages?.length ? c.messages[c.messages.length - 1]?.content : "Chưa có tin nhắn"
            }}
          </small>
        </li>
      </ul>
    </div>

    <div class="flex-grow-1 d-flex flex-column">
      <div class="p-3 border-bottom bg-white fw-bold">{{ selectedCustomerName }}</div>

      <div class="flex-grow-1 p-3 overflow-auto" id="chat-box">
        <div v-if="!messageStore.currentConversation" class="text-center text-muted mt-5">
          Chọn một cuộc trò chuyện để bắt đầu
        </div>

        <div v-else>
          <div
            v-for="m in messageStore.messages"
            :key="m.id"
            class="mb-3 d-flex"
            :class="{
              'justify-content-end': m.senderId === adminId,
              'justify-content-start': m.senderId !== adminId,
            }"
          >
            <div
              class="p-2 rounded-3"
              :class="m.senderId === adminId ? 'bg-warning text-white' : 'bg-body-secondary border'"
            >
              {{ m.content }}
            </div>
          </div>
        </div>
      </div>

      <div v-if="messageStore.currentConversation" class="p-3 border-top bg-white">
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
import type { Conversation } from "@/types/chat";

const conversationStore = useConversationStore();
const messageStore = useMessageStore();
const authStore = useAuthStore();

const adminId = authStore.user!.id;
const messageText = ref("");

const selectedCustomerName = computed(() => {
  const c = conversationStore.conversations.find((x) => x.id === messageStore.currentConversation);
  return c?.customer?.fullName || "Chưa chọn cuộc trò chuyện";
});

async function selectConversation(conversation: Conversation) {
  await messageStore.loadMessages(conversation.id);
  messageStore.connectSignalR(conversation.id);
}

async function sendMessage() {
  if (!messageText.value.trim() || !messageStore.currentConversation) return;

  await messageStore.sendMessage(adminId, messageText.value);

  messageText.value = "";
}

onMounted(async () => {
  await conversationStore.fetchConversations();
});
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
