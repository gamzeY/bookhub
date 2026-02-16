<template>
  <v-dialog :model-value="modelValue" @update:model-value="emit('update:modelValue', $event)" max-width="520">
    <v-card class="rounded-xl">
      <v-card-title class="text-h6 font-weight-bold pa-4">Delete Book</v-card-title>
      <v-divider />

      <v-card-text class="pa-4">
        <div class="text-body-2">
          Are you sure you want to delete a book/review?
        </div>

        <div v-if="book" class="mt-3 text-body-2 text-medium-emphasis">
          <div><strong>{{ book.title }}</strong></div>
          <div>{{ book.author }}</div>
        </div>

        <div v-if="error" class="text-error text-caption mt-3">
          {{ error }}
        </div>
      </v-card-text>

      <v-card-actions class="pa-4 pt-0 justify-end">
        <v-btn variant="text" @click="close" :disabled="submitting">Cancel</v-btn>
        <v-btn color="error" variant="flat" @click="confirm" :loading="submitting" :disabled="!book">
          Delete
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { ref } from "vue";
import type { Book } from "@/api/booksApi";
import { deleteBook } from "@/api/booksApi";

const props = defineProps<{
  modelValue: boolean;
  book: Book | null;
}>();

const emit = defineEmits<{
  (e: "update:modelValue", v: boolean): void;
  (e: "deleted"): void;
}>();

const submitting = ref(false);
const error = ref("");

function close() {
  error.value = "";
  emit("update:modelValue", false);
}

async function confirm() {
  if (!props.book) return;

  submitting.value = true;
  error.value = "";

  try {
    await deleteBook(props.book.id);
    emit("deleted");
    close();
  } catch (e: any) {
    error.value = e?.response?.data?.message ?? "Failed to delete book.";
  } finally {
    submitting.value = false;
  }
}
</script>
