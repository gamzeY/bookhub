<template>
  <div class="h-100">
    <!-- Header -->
    <div class="d-flex align-center justify-space-between mb-6">
      <div>
        <h1 class="text-h4 font-weight-bold mb-1">My Books</h1>
        <div class="text-subtitle-1 text-medium-emphasis">
          Manage your book collection and discover new reads.
        </div>
      </div>

      <v-btn
        color="#5C6BC0"
        class="text-none text-subtitle-2 px-6 rounded-lg"
        prepend-icon="mdi-plus"
        elevation="0"
        height="40"
        @click="openAdd()"
      >
        Add Book
      </v-btn>
    </div>

    <!-- Filters -->
    <div class="d-flex align-center gap-4 mb-6">
      <v-text-field
        v-model="search"
        placeholder="Search by title or author..."
        prepend-inner-icon="mdi-magnify"
        variant="solo"
        density="compact"
        hide-details
        class="flex-grow-1 search-input"
        flat
        rounded="lg"
      />

      <div class="d-flex align-center gap-2" style="width: auto; min-width: 260px;">
        <v-select
          v-model="sort"
          :items="sortItems"
          item-title="title"
          item-value="value"
          variant="solo"
          density="compact"
          hide-details
          flat
          rounded="lg"
          class="sort-select"
          menu-icon="mdi-chevron-down"
        />

        <v-btn icon variant="tonal" rounded="lg" color="grey-darken-1" class="ml-2" disabled>
          <v-icon>mdi-view-grid-outline</v-icon>
        </v-btn>

        <v-btn icon variant="flat" rounded="lg" color="#5C6BC0" class="ml-2">
          <v-icon color="white">mdi-format-list-bulleted</v-icon>
        </v-btn>
      </div>
    </div>

    <!-- Errors -->
    <v-alert v-if="store.error" type="error" variant="tonal" class="mb-4">
      {{ store.error }}
    </v-alert>

    <!-- List Header / Count -->
    <div class="text-caption text-medium-emphasis mb-4">
      {{ store.totalCount }} books found
    </div>

    <!-- List -->
    <div>
      <div v-if="store.loading" class="d-flex justify-center pa-6">
        <v-progress-circular indeterminate color="primary" />
      </div>

      <template v-else>
        <BookListItem
          v-for="b in store.items"
          :key="b.id"
          :book="b"
          @view="onView"
          @edit="onEdit"
          @delete="onDelete"
        />

        <div v-if="store.items.length === 0" class="text-center pa-6 text-medium-emphasis">
          No books found. Add one to get started!
        </div>
      </template>
    </div>

    <!-- Pagination -->
    <div class="d-flex justify-end mt-6" v-if="store.totalPages > 1">
      <v-pagination
        v-model="store.page"
        :length="store.totalPages"
        :total-visible="6"
        density="comfortable"
        active-color="#5C6BC0"
      />
    </div>

    <!-- Add Book Modal -->
    <v-dialog v-model="addDialog" max-width="600" persistent>
      <v-card class="rounded-xl">
        <v-card-title class="text-h6 font-weight-bold pa-4">Add New Book</v-card-title>
        <v-divider />

        <v-card-text class="pa-4">
          <v-form ref="addForm" @submit.prevent="submitAdd">
            <v-row dense>
              <v-col cols="12">
                <v-text-field
                  v-model="newBook.title"
                  label="Title"
                  variant="outlined"
                  density="comfortable"
                  :rules="titleRules"
                  required
                />
              </v-col>

              <v-col cols="12" md="6">
                <v-text-field
                  v-model="newBook.author"
                  label="Author"
                  variant="outlined"
                  density="comfortable"
                  :rules="authorRules"
                  required
                />
              </v-col>

              <v-col cols="12" md="6">
                <v-text-field
                  v-model="newBook.isbn"
                  label="ISBN"
                  variant="outlined"
                  density="comfortable"
                  :rules="isbnRules"
                  required
                />
              </v-col>

              <v-col cols="12">
                <div class="text-subtitle-2 mb-1">Rating</div>
                <v-rating
                  v-model="newBook.rating"
                  color="amber-darken-2"
                  hover
                  density="compact"
                />
                <div class="text-caption text-medium-emphasis mt-1">
                  (Optional) If you set a rating, comments are required.
                </div>
              </v-col>

              <v-col cols="12">
                <v-textarea
                  v-model="newBook.comments"
                  label="Comments"
                  variant="outlined"
                  density="comfortable"
                  rows="3"
                  :rules="commentsRules"
                />
              </v-col>

              <v-col cols="12">
                <v-text-field
                  v-model="newBook.coverImageUrl"
                  label="Cover Image URL (optional)"
                  variant="outlined"
                  density="comfortable"
                  :rules="coverRules"
                />
              </v-col>
            </v-row>

            <div v-if="addError" class="text-error text-caption mt-2">
              {{ addError }}
            </div>
          </v-form>
        </v-card-text>

        <v-card-actions class="pa-4 pt-0 justify-end">
          <v-btn variant="text" @click="closeAdd()" :disabled="submitting">Cancel</v-btn>
          <v-btn color="primary" variant="flat" @click="submitAdd" :loading="submitting">
            Save Book
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <BookEditDialog
      v-model="editDialog"
      :book="selectedBook"
      @saved="store.fetch()"
    />

    <DeleteConfirmDialog
      v-model="deleteDialog"
      :book="selectedBook"
      @deleted="store.fetch()"
    />

  </div>
</template>

<script setup lang="ts">
import { onMounted, ref, watch } from "vue";
import { useRouter } from "vue-router";
import BookListItem from "@/components/books/BookListItem.vue";
import { useBooksStore } from "@/stores/books.store";
import { createBook } from "@/api/booksApi";
import type { Book } from "@/api/booksApi";
import BookEditDialog from "@/components/books/BookEditDialog.vue";
import DeleteConfirmDialog from "@/components/books/DeleteConfirmDialog.vue";

const router = useRouter();
const store = useBooksStore();

const addDialog = ref(false);
const addForm = ref<any>(null);

const editDialog = ref(false);
const deleteDialog = ref(false);
const selectedBook = ref<Book | null>(null);


const search = ref("");
const sort = ref("title");
const sortItems = [{ title: "Sort by Title", value: "title" }];

const submitting = ref(false);
const addError = ref("");

type NewBook = {
  title: string;
  author: string;
  isbn: string;
  rating: number;
  comments: string;
  coverImageUrl: string;
};

const newBook = ref<NewBook>({
  title: "",
  author: "",
  isbn: "",
  rating: 0,
  comments: "",
  coverImageUrl: "",
});

const titleRules = [
  (v: string) => (!!v && v.trim().length > 0) || "Title is required.",
  (v: string) => (v?.length ?? 0) <= 120 || "Title must be at most 120 characters.",
];

const authorRules = [
  (v: string) => (!!v && v.trim().length > 0) || "Author is required.",
  (v: string) => (v?.length ?? 0) <= 80 || "Author must be at most 80 characters.",
];

const isbnRules = [
  (v: string) => (!!v && v.trim().length > 0) || "ISBN is required.",
  (v: string) => (v?.length ?? 0) <= 20 || "ISBN must be at most 20 characters.",
];

const coverRules = [
  (v: string) => (v?.length ?? 0) <= 300 || "Cover URL must be at most 300 characters.",
];

const commentsRules = [
  (v: string) => (v?.length ?? 0) <= 500 || "Comments must be at most 500 characters.",
  (v: string) => !v || !v.toLowerCase().includes("horrible") || "The word 'horrible' is not allowed.",
  // rating set => comments required
  () => (newBook.value.rating === 0 || newBook.value.comments.trim().length > 0) || "Comments are required when rating is provided.",
];

function resetAddForm() {
  newBook.value = { title: "", author: "", isbn: "", rating: 0, comments: "", coverImageUrl: "" };
  addError.value = "";
  addForm.value?.resetValidation?.();
}

function openAdd() {
  resetAddForm();
  addDialog.value = true;
}

function closeAdd() {
  addDialog.value = false;
}

async function submitAdd() {
  const res = await addForm.value?.validate?.();
  if (!res?.valid) return;

  submitting.value = true;
  addError.value = "";

  try {
    const payload = {
      title: newBook.value.title.trim(),
      author: newBook.value.author.trim(),
      isbn: newBook.value.isbn.trim(),
      rating: newBook.value.rating === 0 ? null : newBook.value.rating,
      comments: newBook.value.rating === 0 ? (newBook.value.comments.trim() || null) : newBook.value.comments.trim(),
      coverImageUrl: newBook.value.coverImageUrl.trim() || null,
    };

    await createBook(payload);
    await store.fetch();
    closeAdd();
  } catch (e: any) {
    const msg =
      e?.response?.data?.message ??
      (e?.response?.data?.errors
        ? Object.values(e.response.data.errors).flat().join(" ")
        : null) ??
      "Failed to create book. Please try again.";
    addError.value = msg;
  } finally {
    submitting.value = false;
  }
}

// --- Fetch list ---
onMounted(async () => {
  search.value = store.search;
  sort.value = store.sort;
  await store.fetch();
});

// Debounced search
let t: number | undefined;
watch(search, (v) => {
  window.clearTimeout(t);
  t = window.setTimeout(async () => {
    store.setSearch(v);
    await store.fetch();
  }, 300);
});

watch(sort, async (v) => {
  store.setSort(v);
  await store.fetch();
});

watch(
  () => store.page,
  async () => {
    await store.fetch();
  }
);

function onView(id: string) {
  router.push(`/books/${id}`);
}
function onEdit(id: string) {
  selectedBook.value = store.items.find((b) => b.id === id) ?? null;
  editDialog.value = true;
}

function onDelete(id: string) {
  selectedBook.value = store.items.find((b) => b.id === id) ?? null;
  deleteDialog.value = true;
}
</script>

<style scoped>
.search-input :deep(.v-field) {
  border-radius: 8px;
  border: 1px solid rgba(var(--v-border-color), var(--v-border-opacity));
  box-shadow: none !important;
}

.sort-select :deep(.v-field) {
  border-radius: 8px;
  border: 1px solid rgba(var(--v-border-color), var(--v-border-opacity));
  box-shadow: none !important;
}
</style>
