<template>
  <div class="combo-form container">
    <h2>{{ isEdit ? "Cập nhật combo" : "Thêm combo" }}</h2>

    <form @submit.prevent="handleSubmit">
      <!-- Tên combo -->
      <div class="mb-3">
        <label>Tên combo</label>
        <input v-model="form.name" class="form-control" required />
      </div>

      <!-- Giá combo -->
      <div class="mb-3">
        <label>Giá combo</label>
        <input type="number" v-model.number="form.price" class="form-control" required />
      </div>

      <!-- Mô tả combo -->
      <div class="mb-3">
        <label>Mô tả</label>
        <textarea v-model="form.description" class="form-control"></textarea>
      </div>

      <!-- Chọn món ăn trong combo -->
      <div class="mb-3">
        <label>Món ăn trong combo</label>
        <div class="row g-3">
          <div v-for="food in foodStore.foods" :key="food.id" class="col-12 col-sm-6 col-lg-4">
            <template v-if="comboFoodSelection[food.id]">
              <div class="p-2 border rounded-3 d-flex align-items-center justify-content-between">
                <div class="d-flex align-items-center flex-grow-1">
                  <input
                    class="form-check-input me-2"
                    type="checkbox"
                    :id="'combo-' + food.id"
                    v-model="comboFoodSelection[food.id]!.selected"
                  />
                  <label
                    :for="'combo-' + food.id"
                    class="form-check-label fw-medium text-break me-3"
                  >
                    {{ food.name }}
                  </label>
                </div>

                <div class="d-flex align-items-center ms-2">
                  <input
                    type="number"
                    v-model.number="comboFoodSelection[food.id]!.quantity"
                    class="form-control form-control-sm text-center"
                    :disabled="!comboFoodSelection[food.id]!.selected"
                    min="1"
                    style="width: 60px"
                  />
                </div>
              </div>
            </template>
          </div>
        </div>
      </div>
      <!-- Ảnh combo -->
      <div class="mb-3">
        <label>Ảnh</label>
        <input type="file" @change="onFileChange" class="form-control" accept="image/*" />
      </div>

      <div class="mb-4">
        <div class="form-check d-flex align-items-center">
          <input
            class="form-check-input checkbox-lg"
            type="checkbox"
            id="isAvailableCheckbox"
            v-model="form.isAvailable"
          />
          <label class="form-check-label ms-2 fw-medium" for="isAvailableCheckbox">
            Còn hàng
          </label>
        </div>
      </div>

      <!-- Submit -->
      <button type="submit" class="btn btn-primary" :disabled="comboStore.loading">
        {{ isEdit ? "Lưu thay đổi" : "Thêm mới" }}
      </button>
    </form>
  </div>
</template>

<script lang="ts" setup>
import { ref, onMounted, computed } from "vue";
import { useRouter, useRoute } from "vue-router";
import { useFoodStore } from "@/stores/foodStore";
import { useComboStore } from "@/stores/comboStore";

const foodStore = useFoodStore();
const comboStore = useComboStore();
const router = useRouter();
const route = useRoute();
const isEdit = computed(() => !!Number(route.params.id));

const imageFile = ref<File | null>(null);

const form = ref({
  id: 0,
  name: "",
  price: 0,
  description: "",
  imageUrl: "",
  isAvailable: true,
});

interface ComboSelection {
  selected: boolean;
  quantity: number;
}
const comboFoodSelection = ref<Record<number, ComboSelection>>({});

onMounted(async () => {
  await foodStore.fetchFoods();
  foodStore.foods.forEach((f) => {
    comboFoodSelection.value[f.id] = { selected: false, quantity: 1 };
  });

  if (isEdit.value) {
    const comboId = Number(route.params.id);
    const combo = await comboStore.fetchCombo(comboId);
    if (combo) {
      form.value = {
        id: combo.id,
        name: combo.name,
        price: combo.price,
        description: combo.description,
        imageUrl: combo.imageUrl,
        isAvailable: combo.isAvailable,
      };
      combo.comboFoods?.forEach((cf) => {
        comboFoodSelection.value[cf.foodId] = { selected: true, quantity: cf.quantity };
      });
    }
  }
});

function onFileChange(event: Event) {
  const target = event.target as HTMLInputElement;
  if (target.files && target.files[0]) {
    imageFile.value = target.files[0];
  }
}

const handleSubmit = async () => {
  try {
    if (imageFile.value) {
      form.value.imageUrl = await comboStore.uploadImage(imageFile.value);
    }

    const comboFoodsPayload = Object.entries(comboFoodSelection.value)
      .filter(([_, val]) => val.selected)
      .map(([foodId, val]) => ({
        foodId: Number(foodId),
        comboId: form.value.id,
        quantity: val.quantity,
      }));

    const payload = {
      ...form.value,
      comboFoods: comboFoodsPayload,
    };
    console.log(payload);

    if (isEdit.value) {
      await comboStore.editCombo(form.value.id, payload);
      alert("Cập nhật combo thành công!");
    } else {
      await comboStore.addCombo(payload);
      alert("Thêm combo thành công!");
      form.value = { id: 0, name: "", price: 0, description: "", imageUrl: "", isAvailable: true };
      Object.keys(comboFoodSelection.value).forEach((k) => {
        comboFoodSelection.value[Number(k)] = { selected: false, quantity: 1 };
      });
      imageFile.value = null;
    }

    router.push("/admin/comboes");
  } catch (err) {
    console.error("Lỗi khi xử lý combo:", err);
    alert("Có lỗi xảy ra: " + (err instanceof Error ? err.message : String(err)));
  }
};
</script>
