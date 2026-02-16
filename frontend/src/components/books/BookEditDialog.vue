<template>
  <v-dialog :model-value="modelValue" @update:model-value="emit('update:modelValue', $event)" max-width="600" persistent>
    <v-card class="rounded-xl">
      <v-card-title class="text-h6 font-weight-bold pa-4">Edit Review</v-card-title>
      <v-divider />

      <v-card-text class="pa-4">
        <div v-if="!book" class="text-medium-emphasis">No book selected.</div>

        <template v-else>
          <!-- Read-only info -->
          <v-row dense class="mb-2">
            <v-col cols="12">
              <v-text-field :model-value="book.title" label="Title" variant="outlined" density="comfortable" readonly />
            </v-col>
            <v-col cols="12" md="6">
              <v-text-field :model-value="book.author" label="Author" variant="outlined" density="comfortable" readonly />
            </v-col>
            <v-col cols="12" md="6">
              <v-text-field :model-value="book.isbn" label="ISBN" variant="outlined" density="comfortable" readonly />
            </v-col>
          </v-row>

          <v-form ref="form" @submit.prevent="submit">
            <v-row dense>
              <v-col cols="12">
                <div class="text-subtitle-2 mb-1">Rating</div>
                <v-rating v-model="rating" color="amber-darken-2" hover density="compact" />
                <div class="text-caption text-medium-emphasis mt-1">
                  If you set a rating, comments are required.
                </div>
              </v-col>

              <v-col cols="12">
                <v-textarea
                  v-model="comments"
                  label="Comments"
                  variant="outlined"
                  density="comfortable"
                  rows="3"
                  :rules="commentsRules"
                />
              </v-col>
            </v-row>

            <div v-if="error" class="text-error text-caption mt-2">
              {{ error }}
            </div>
          </v-form>
        </template>
      </v-card-text>

      <v-card-actions class="pa-4 pt-0 justify-end">
        <v-btn variant="text" @click="close" :disabled="submitting">Cancel</v-btn>
        <v-btn color="primary" variant="flat" @click="submit" :loading="submitting" :disabled="!book">
          Save
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { computed, ref, watch } from "vue";
import type { Book } from "@/api/booksApi";
import { updateBook } from "@/api/booksApi";

const props = defineProps<{
  modelValue: boolean;
  book: Book | null;
}>();

const emit = defineEmits<{
  (e: "update:modelValue", v: boolean): void;
  (e: "saved"): void;
}>();

const form = ref<any>(null);
const submitting = ref(false);
const error = ref("");

const rating = ref<number>(0);
const comments = ref<string>("");

watch(
  () => props.book,
  (b) => {
    rating.value = b?.rating ?? 0;
    comments.value = b?.comments ?? "";
    error.value = "";
    form.value?.resetValidation?.();
  },
  { immediate: true }
);

const commentsRules = computed(() => [
  (v: string) => (v?.length ?? 0) <= 500 || "Comments must be at most 500 characters.",
  (v: string) => !v || !v.toLowerCase().includes("horrible") || "The word 'horrible' is not allowed.",
  () => (rating.value === 0 || comments.value.trim().length > 0) || "Comments are required when rating is provided.",
]);

function close() {
  emit("update:modelValue", false);
}

async function submit() {
  const res = await form.value?.validate?.();
  if (!res?.valid) return;
  if (!props.book) return;

  submitting.value = true;
  error.value = "";

  try {
    await updateBook(props.book.id, {
      rating: rating.value === 0 ? null : rating.value,
      comments: rating.value === 0 ? (comments.value.trim() || null) : comments.value.trim(),
    });

    emit("saved");
    close();
  } catch (e: any) {
    const msg =
      e?.response?.data?.message ??
      (e?.response?.data?.errors ? Object.values(e.response.data.errors).flat().join(" ") : null) ??
      "Failed to update book.";
    error.value = msg;
  } finally {
    submitting.value = false;
  }
}
</script>
