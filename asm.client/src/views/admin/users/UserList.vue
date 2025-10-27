<template>
  <div class="container mt-4">
    <div class="d-flex justify-content-between align-items-center mb-3">
      <h2 class="fw-semibold">Quản lý người dùng</h2>
      <RouterLink to="/admin/users/add" class="btn btn-warning">+ Thêm người dùng</RouterLink>
    </div>

    <table class="table table-striped table-bordered align-middle">
      <thead class="table-light">
        <tr>
          <th>ID</th>
          <th>Họ và tên</th>
          <th>Email</th>
          <th>Adress</th>
          <th>Role</th>
          <th>Status</th>
          <th style="width: 200px">Hành động</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="user in store.users" :key="user.id">
          <td>{{ user.id }}</td>
          <td>{{ user.fullName }}</td>
          <td>{{ user.email }}</td>
          <td>{{ user.address || "-" }}</td>
          <td>{{ user.role || "-" }}</td>
          <td>{{ user.status ? "Active" : "Inactive" }}</td>
          <td>
            <div class="d-flex gap-2">
              <RouterLink :to="`/admin/users/${user.id}`" class="btn btn-sm btn-secondary"
                >Xem</RouterLink
              >
              <RouterLink :to="`/admin/users/edit/${user.id}`" class="btn btn-sm btn-primary"
                >Sửa</RouterLink
              >
              <button
                class="btn btn-sm btn-danger"
                data-bs-toggle="modal"
                data-bs-target="#deleteConfirm"
                @click="setSelectedUser(user)"
              >
                Xóa
              </button>
            </div>
          </td>
        </tr>
      </tbody>
    </table>

    <!-- Modal xác nhận xóa -->
    <div
      class="modal fade"
      id="deleteConfirm"
      tabindex="-1"
      aria-labelledby="deleteConfirmLabel"
      aria-hidden="true"
    >
      <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title" id="deleteConfirmLabel">Xác nhận xóa</h5>
            <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
          </div>
          <div class="modal-body text-center">
            <p>
              Bạn có chắc muốn xóa người dùng
              <strong>{{ selectedUser?.fullName }}</strong>
              không?
            </p>
          </div>
          <div class="modal-footer justify-content-center">
            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Hủy</button>
            <button
              type="button"
              class="btn btn-danger"
              data-bs-dismiss="modal"
              @click="deleteUser"
            >
              Xóa
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { onMounted, ref } from "vue";
import { useUserStore } from "@/stores/userStore";
import { RouterLink } from "vue-router";
import type { User } from "@/types/user";
import { useAuthStore } from "@/stores/authStore";

const store = useUserStore();
const authStore = useAuthStore();
const selectedUser = ref<User | null>(null);

onMounted(() => {
  store.fetchUsers();
});

function setSelectedUser(user: User) {
  selectedUser.value = user;
}

async function deleteUser() {
  if (selectedUser.value) {
    if (selectedUser.value.id === authStore.user?.id) {
      alert("Bạn không thể xóa tài khoản đang sử dụng");
      return;
    }
    await store.removeUser(selectedUser.value.id);
    selectedUser.value = null;
  }
}
</script>
