import {
  createConversation,
  deleteConversation,
  getConversations,
  getConversation,
  updateConversation,
} from "@/api/conversationService";
import { defineStore } from "pinia";
import { ref } from "vue";
import { type Conversation, type ConversationUpdate } from "@/types/chat";

export const useConversationStore = defineStore("conversation", () => {
  const conversations = ref<Conversation[]>([]);
  const loading = ref(false);

  async function fetchConversations() {
    loading.value = true;
    try {
      conversations.value = await getConversations();
    } catch (err) {
      console.error("Lỗi khi tải conversations: ", err);
    } finally {
      loading.value = false;
    }
  }

  async function fetchConversation(id: number) {
    loading.value = true;
    try {
      const conversation = await getConversation(id);
      return conversation;
    } catch (err) {
      console.error("Lỗi khi tải conversation: ", err);
    } finally {
      loading.value = false;
    }
  }

  async function addConversation(conversation: Omit<Conversation, "id">) {
    try {
      const newCategory = await createConversation(conversation);
      conversations.value.unshift(newCategory);
    } catch (err) {
      console.error("Lỗi khi thêm conversation: ", err);
      throw err;
    }
  }

  async function editConversation(id: number, conversationData: ConversationUpdate) {
    try {
      console.log(conversationData);
      await updateConversation(id, conversationData);
      const index = conversations.value.findIndex((f) => f.id === id);
      if (index !== -1) {
        const targetFood = conversations.value[index];
        Object.assign(targetFood!, conversationData);
      }
    } catch (err) {
      console.error(`Lỗi khi sửa conversation ID ${id}: `, err);
      throw err;
    }
  }

  async function removeConversation(id: number) {
    try {
      await deleteConversation(id);
      conversations.value = conversations.value.filter((c) => c.id !== id);
    } catch (err) {
      console.error(`Lỗi khi xóa conversation ID ${id}: `, err);
      throw err;
    }
  }

  return {
    conversations,
    loading,
    fetchConversation,
    addConversation,
    editConversation,
    removeConversation,
    fetchConversations,
  };
});
