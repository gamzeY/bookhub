<template>
  <div class="d-flex align-center justify-space-between mb-6">
    <div>
      <div class="text-h5 font-weight-bold">Book Details</div>
      <div class="text-body-2 text-medium-emphasis">View full information and notes.</div>
    </div>

    <div class="d-flex ga-2">
      <v-btn variant="text" prepend-icon="mdi-arrow-left" @click="goBack">Back</v-btn>
      <v-btn
        v-if="book"
        color="#5C6BC0"
        class="text-none rounded-lg"
        prepend-icon="mdi-pencil-outline"
        elevation="0"
        @click="openEdit"
      >
        Edit
      </v-btn>
      <v-btn
        v-if="book"
        color="error"
        class="text-none rounded-lg"
        prepend-icon="mdi-trash-can-outline"
        elevation="0"
        @click="openDelete"
      >
        Delete
      </v-btn>
    </div>
  </div>

  <v-alert v-if="error" type="error" variant="tonal" class="mb-4">
    {{ error }}
  </v-alert>

  <v-card class="rounded-xl" flat>
    <v-card-text class="pa-6">
      <div v-if="loading" class="d-flex justify-center pa-10">
        <v-progress-circular indeterminate color="primary" />
      </div>

      <div v-else-if="!book" class="text-center pa-10 text-medium-emphasis">
        Book not found.
      </div>

      <template v-else>
        <div class="d-flex flex-wrap ga-6">
          <!-- Cover -->
          <div class="cover">
            <img v-if="book.coverImageUrl" :src="book.coverImageUrl" alt="Cover" />
            <div v-else class="cover-fallback">
              <v-icon icon="mdi-book-open-page-variant" size="32" />
            </div>
          </div>

          <!-- Main info -->
          <div class="flex-grow-1" style="min-width: 280px;">
            <div class="text-h5 font-weight-bold">{{ book.title }}</div>
            <div class="text-subtitle-1 text-medium-emphasis mt-1">{{ book.author }}</div>

            <div class="mt-3 d-flex flex-wrap ga-2 align-center">
              <v-chip size="small" variant="tonal" color="primary">
                ISBN: {{ book.isbn }}
              </v-chip>

              <v-chip v-if="hasNotes" size="small" variant="tonal" color="success">
                Has notes
              </v-chip>
            </div>

            <div class="mt-4 d-flex align-center ga-3">
              <v-rating :model-value="book.rating ?? 0" readonly density="compact" size="18" />
              <div class="text-body-2 text-medium-emphasis">
                <span v-if="book.rating">{{ book.rating }}/5</span>
                <span v-else>No rating</span>
              </div>
            </div>

            <div class="mt-6">
              <div class="text-subtitle-2 font-weight-semibold mb-2">Comments</div>
              <v-card variant="outlined" class="rounded-lg">
                <v-card-text class="py-4">
                  <div v-if="book.comments && book.comments.trim().length > 0" class="text-body-2">
                    {{ book.comments }}
                  </div>
                  <div v-else class="text-body-2 text-medium-emphasis">
                    No comments yet.
                  </div>
                </v-card-text>
              </v-card>
            </div>
          </div>
        </div>
      </template>
    </v-card-text>
  </v-card>

  <BookEditDialog v-model="editDialog" :book="book" @saved="reload" />
  <DeleteConfirmDialog v-model="deleteDialog" :book="book" @deleted="afterDelete" />
</template>

<script setup lang="ts">
import { computed, onMounted, ref, watch } from "vue";
import { useRoute, useRouter } from "vue-router";
import { getBook, type Book } from "@/api/booksApi";
import BookEditDialog from "@/components/books/BookEditDialog.vue";
import DeleteConfirmDialog from "@/components/books/DeleteConfirmDialog.vue";

const route = useRoute();
const router = useRouter();

const loading = ref(false);
const error = ref<string | null>(null);

const book = ref<Book | null>(null);
const editDialog = ref(false);
const deleteDialog = ref(false);

const hasNotes = computed(() => !!book.value?.comments?.trim());

function goBack() {
  router.push("/books");
}

function openEdit() {
  editDialog.value = true;
}

function openDelete() {
  deleteDialog.value = true;
}

async function reload() {
  const id = String(route.params.id);
  loading.value = true;
  error.value = null;

  try {
    book.value = await getBook(id);
  } catch (e: any) {
    error.value = e?.response?.data?.message ?? "Failed to load book.";
    book.value = null;
  } finally {
    loading.value = false;
  }
}

async function afterDelete() {
  router.push("/books");
}

onMounted(reload);
watch(() => route.params.id, reload);
</script>

<style scoped>
.cover {
  width: 140px;
  height: 180px;
  border-radius: 14px;
  overflow: hidden;
  border: 1px solid #e8ecf5;
  background: #fff;
}
.cover img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}
.cover-fallback {
  width: 100%;
  height: 100%;
  display: grid;
  place-items: center;
  background: rgba(79, 70, 229, 0.12);
  color: #4f46e5;
}
</style>
