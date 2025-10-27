<template>
  <!-- Floating button -->
  <button
    type="button"
    class="btn btn-primary rounded-circle shadow position-fixed"
    style="bottom: 20px; right: 20px; width: 60px; height: 60px"
    @click="toggleChat"
  >
    <i class="bi bi-chat-dots fs-4"></i>
  </button>

  <!-- Chat box -->
  <div
    v-if="open"
    class="card position-fixed shadow-lg"
    style="bottom: 100px; right: 20px; width: 360px; max-height: 500px"
  >
    <!-- Header -->
    <div
      class="card-header bg-primary text-white d-flex justify-content-between align-items-center"
    >
      <span><i class="bi bi-headset me-2"></i> Hỗ trợ khách hàng</span>
      <button class="btn-close btn-close-white" @click="toggleChat"></button>
    </div>

    <!-- Body -->
    <div ref="scrollContainer" class="card-body overflow-auto" style="max-height: 400px">
      <div v-for="(msg, i) in messages" :key="i" class="mb-3">
        <!-- Tin của người khác -->
        <div v-if="msg.senderId !== userId" class="d-flex align-items-start">
          <img
            src="https://cdn-icons-png.flaticon.com/512/149/149071.png"
            class="rounded-circle me-2"
            width="36"
          />
          <div class="bg-light p-2 rounded-3 shadow-sm">
            <p class="mb-1">{{ msg.content }}</p>
            <small class="text-muted">{{ formatTime(msg.createAt) }}</small>
          </div>
        </div>

        <!-- Tin của mình -->
        <div v-else class="d-flex justify-content-end align-items-start">
          <div class="bg-primary text-white p-2 rounded-3 shadow-sm text-end">
            <p class="mb-1">{{ msg.content }}</p>
            <small class="text-light opacity-75">{{ formatTime(msg.createAt) }}</small>
          </div>
          <img
            src="https://cdn-icons-png.flaticon.com/512/847/847969.png"
            class="rounded-circle ms-2"
            width="36"
          />
        </div>
      </div>
    </div>

    <!-- Input -->
    <div class="card-footer bg-light d-flex align-items-center gap-2">
      <input
        type="text"
        class="form-control rounded-pill"
        placeholder="Nhập tin nhắn..."
        v-model="text"
        @keyup.enter="send"
      />
      <button
        class="btn btn-primary rounded-circle"
        style="width: 40px; height: 40px"
        @click="send"
      >
        <i class="bi bi-send"></i>
      </button>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, nextTick, watch } from "vue";
import { useMessageStore } from "@/stores/messageStore";

const store = useMessageStore();
const open = ref(false);
const text = ref("");
const userId = "user1";
const receiverId = "user2";
const scrollContainer = ref<HTMLElement | null>(null);

onMounted(() => {
  store.connectSignalR(userId);
  store.loadMessages(1);
});

watch(
  () => store.messages.length,
  async () => {
    await nextTick();
    scrollToBottom();
  }
);

const messages = store.messages;

function toggleChat() {
  open.value = !open.value;
}

async function send() {
  if (!text.value.trim()) return;
  await store.sendMessage(userId, receiverId, text.value);
  text.value = "";
}

function scrollToBottom() {
  if (scrollContainer.value) scrollContainer.value.scrollTop = scrollContainer.value.scrollHeight;
}

function formatTime(dateStr?: string) {
  if (!dateStr) return "";
  const date = new Date(dateStr);
  return `${date.getHours().toString().padStart(2, "0")}:${date
    .getMinutes()
    .toString()
    .padStart(2, "0")}`;
}
</script>

<style scoped>
.card-body::-webkit-scrollbar {
  width: 6px;
}
.card-body::-webkit-scrollbar-thumb {
  background-color: rgba(0, 0, 0, 0.2);
  border-radius: 3px;
}
</style>
