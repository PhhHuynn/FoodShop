<template>
  <div class="user-form container">
    <h2>{{ isEdit ? "Cập nhật người dùng" : "Thêm người dùng" }}</h2>

    <form @submit.prevent="handleSubmit">
      <div class="mb-3">
        <label>Họ và tên</label>
        <input v-model="form.fullName" class="form-control" required />
      </div>

      <div class="mb-3">
        <label>Email</label>
        <input v-model="form.email" type="email" class="form-control" required />
      </div>

      <div class="mb-3">
        <label>Địa chỉ</label>
        <input v-model="form.address" class="form-control" />
      </div>

      <div class="mb-3">
        <label>Trạng thái</label>
        <select v-model="form.status" class="form-control">
          <option :value="UserStatus.Active">Active</option>
          <option :value="UserStatus.Inactive">Inactive</option>
          <option :value="UserStatus.Banned">Banned</option>
          <option :value="UserStatus.Pending">Pending</option>
        </select>
      </div>

      <div class="mb-3">
        <label>Role</label>
        <select v-model="form.role" class="form-control">
          <option :value="UserRole.Admin">Admin</option>
          <option :value="UserRole.User">User</option>
        </select>
      </div>

      <div v-if="!isEdit" class="mb-3">
        <label>Mật khẩu</label>
        <input v-model="form.password" type="password" class="form-control" required />
      </div>

      <button type="submit" class="btn btn-primary" :disabled="store.loading">
        {{ isEdit ? "Lưu thay đổi" : "Thêm mới" }}
      </button>
    </form>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from "vue";
import { useRoute, useRouter } from "vue-router";
import { useUserStore } from "@/stores/userStore";
import type { User, UserUpdate } from "@/types/user";
import { UserStatus } from "@/types/user";
import { UserRole } from "@/types/user";

const store = useUserStore();
const route = useRoute();
const router = useRouter();

const isEdit = computed(() => !!route.params.id);

const form = ref<User & { password?: string }>({
  id: "",
  fullName: "",
  email: "",
  address: "",
  status: UserStatus.Active,
  role: UserRole.User,
  password: "",
});

onMounted(async () => {
  if (isEdit.value) {
    const id = String(route.params.id);
    const user = await store.fetchUser(id);
    if (user) {
      form.value = { ...user };
    }
  }
});

const handleSubmit = async () => {
  try {
    if (isEdit.value) {
      const updateData: UserUpdate = {
        id: form.value.id,
        fullName: form.value.fullName,
        email: form.value.email,
        address: form.value.address,
        status: form.value.status,
        role: form.value.role,
      };
      console.log(updateData);

      await store.editUser(form.value.id, updateData);
      alert("Cập nhật người dùng thành công!");
    } else {
      const createData = { ...form.value };
      await store.addUser(createData);
      alert("Thêm người dùng thành công!");
      form.value.fullName = "";
      form.value.email = "";
      form.value.address = "";
      form.value.status = UserStatus.Active;
      form.value.role = UserRole.User;
      form.value.password = "";
    }

    router.push("/admin/users");
  } catch (err) {
    console.error("Lỗi khi xử lý form:", err);
    alert("Có lỗi xảy ra: " + (err instanceof Error ? err.message : String(err)));
  }
};
</script>
