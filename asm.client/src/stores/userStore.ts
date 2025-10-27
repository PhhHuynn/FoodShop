import { createUser, deleteUser, getUsers, getUser, updateUser } from "@/api/userService";
import { defineStore } from "pinia";
import { ref } from "vue";
import type { User, UserUpdate } from "@/types/user";

export const useUserStore = defineStore("user", () => {
  const users = ref<User[]>([]);
  const loading = ref(false);

  // Lấy danh sách tất cả user
  async function fetchUsers() {
    loading.value = true;
    try {
      users.value = await getUsers();
    } catch (err) {
      console.error("Lỗi khi tải users: ", err);
    } finally {
      loading.value = false;
    }
  }

  // Lấy chi tiết 1 user theo id
  async function fetchUser(id: string) {
    loading.value = true;
    try {
      const user = await getUser(id);
      return user;
    } catch (err) {
      console.error(`Lỗi khi tải user ID ${id}: `, err);
    } finally {
      loading.value = false;
    }
  }

  // Thêm user mới
  async function addUser(userData: Omit<User, "id">) {
    try {
      const newUser = await createUser(userData);
      users.value.unshift(newUser);
    } catch (err) {
      console.error("Lỗi khi thêm user: ", err);
      throw err;
    }
  }

  // Cập nhật user
  async function editUser(id: string, userData: UserUpdate) {
    try {
      await updateUser(id, userData);
      const index = users.value.findIndex((u) => u.id === id);
      if (index !== -1) {
        const targetUser = users.value[index];
        Object.assign(targetUser!, userData);
      }
    } catch (err) {
      console.error(`Lỗi khi sửa user ID ${id}: `, err);
      throw err;
    }
  }

  // Xóa user
  async function removeUser(id: string) {
    try {
      await deleteUser(id);
      users.value = users.value.filter((u) => u.id !== id);
    } catch (err) {
      console.error(`Lỗi khi xóa user ID ${id}: `, err);
      throw err;
    }
  }

  return {
    users,
    loading,
    fetchUsers,
    fetchUser,
    addUser,
    editUser,
    removeUser,
  };
});
