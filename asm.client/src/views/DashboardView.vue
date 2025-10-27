<template>
  <div class="account-update-form container">
    <h2>Cập nhật thông tin tài khoản</h2>

    <form @submit.prevent="handleSubmit">
      <div class="mb-3">
        <label for="fullName" class="form-label">Họ và tên</label>
        <input id="fullName" v-model="form.fullName" class="form-control" required />
      </div>

      <div class="mb-3">
        <label for="email" class="form-label">Email</label>
        <input
          id="email"
          v-model="form.email"
          type="email"
          class="form-control"
          required
          disabled
        />
        <div class="form-text">Email không thể thay đổi.</div>
      </div>

      <div class="mb-4">
        <label for="address" class="form-label">Địa chỉ (Tùy chọn)</label>
        <input id="address" v-model="form.address" class="form-control" />
      </div>
      <hr class="my-4" />

      <h4 class="mb-3">
        {{ hasLocalPassword ? "Thay đổi mật khẩu" : "Thiết lập mật khẩu" }}
      </h4>

      <template v-if="!hasLocalPassword">
        <div class="alert alert-warning small" role="alert">
          Tài khoản của bạn chưa có mật khẩu. Bạn có thể thiết lập mật khẩu tại đây để đăng nhập
          bằng Email và mật khẩu trong tương lai.
        </div>

        <div class="mb-3">
          <label for="newPassword" class="form-label">Mật khẩu mới</label>
          <input
            id="newPassword"
            v-model="form.newPassword"
            type="password"
            class="form-control"
            placeholder="Nhập mật khẩu bạn muốn thiết lập"
            required
          />
        </div>

        <div class="mb-4">
          <label for="confirmPassword" class="form-label">Xác nhận mật khẩu</label>
          <input
            id="confirmPassword"
            v-model="confirmPassword"
            type="password"
            class="form-control"
            required
          />
        </div>
      </template>

      <template v-else>
        <div class="mb-3">
          <label for="oldPassword" class="form-label">Mật khẩu cũ</label>
          <input
            id="oldPassword"
            v-model="form.oldPassword"
            type="password"
            class="form-control"
            placeholder="Chỉ nhập nếu bạn muốn thay đổi mật khẩu"
            :required="!!form.newPassword"
          />
        </div>

        <div class="mb-4">
          <label for="newPassword" class="form-label">Mật khẩu mới</label>
          <input
            id="newPassword"
            v-model="form.newPassword"
            type="password"
            class="form-control"
            placeholder="Để trống nếu không muốn thay đổi"
          />
        </div>
      </template>

      <button type="submit" class="btn btn-primary">Lưu thay đổi</button>
    </form>
  </div>
</template>
<script setup lang="ts">
import { ref, onMounted } from "vue";
import { useRouter } from "vue-router";
import { useAuthStore } from "@/stores/authStore";
import type { AccountUpdate, User } from "@/types/user";
import { UserStatus } from "@/types/user";
import { useUserStore } from "@/stores/userStore";

const authStore = useAuthStore();
const userStore = useUserStore();
const router = useRouter();

interface UserWithHash extends User {
  passwordHash?: string | null;
}

const hasLocalPassword = ref<boolean>(true);
const confirmPassword = ref<string>("");

const form = ref<AccountUpdate>({
  id: "",
  email: "",
  status: UserStatus.Active as UserStatus,
  fullName: "",
  address: undefined,
  newPassword: "",
  oldPassword: "",
});

onMounted(async () => {
  if (authStore.user) {
    const userAccount = (await userStore.fetchUser(authStore.user.id)) as UserWithHash | undefined;

    if (userAccount) {
      form.value.id = userAccount.id;
      form.value.email = userAccount.email;
      form.value.fullName = userAccount.fullName;
      form.value.address = userAccount.address;
      form.value.status = userAccount.status;

      form.value.newPassword = "";
      form.value.oldPassword = "";

      hasLocalPassword.value = !!userAccount.passwordHash;
    }
  } else {
    alert("Vui lòng đăng nhập để cập nhật thông tin.");
    router.push("/login");
  }
});

const handleSubmit = async () => {
  try {
    if (!hasLocalPassword.value) {
      if (form.value.newPassword !== confirmPassword.value) {
        alert("Mật khẩu mới và xác nhận mật khẩu không khớp.");
        return;
      }
      if (!form.value.newPassword) {
        form.value.oldPassword = "";
      } else {
        form.value.oldPassword = "";
      }
    } else {
      if (form.value.newPassword && !form.value.oldPassword) {
        alert("Vui lòng nhập Mật khẩu cũ để xác nhận thay đổi mật khẩu.");
        return;
      }
    }

    const updateData: AccountUpdate = {
      id: form.value.id,
      fullName: form.value.fullName,
      email: form.value.email,
      address: form.value.address || undefined,
      status: form.value.status,
      newPassword: form.value.newPassword,
      oldPassword: form.value.oldPassword,
    };

    if (!updateData.newPassword) {
      delete updateData.newPassword;
      delete updateData.oldPassword;
    }

    await authStore.editAccount(updateData);

    alert("Cập nhật tài khoản thành công!");

    if (!hasLocalPassword.value && form.value.newPassword) {
      hasLocalPassword.value = true;
    }

    form.value.newPassword = "";
    form.value.oldPassword = "";
    confirmPassword.value = "";
  } catch (err) {
    console.error("Lỗi khi cập nhật tài khoản:", err);
    alert("Cập nhật thất bại: " + (err instanceof Error ? err.message : String(err)));
  }
};
</script>
