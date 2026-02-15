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
        @click="addDialog = true"
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
        bg-color="white"
        flat
        rounded="lg"
      />

      <div class="d-flex align-center gap-2" style="width: auto; min-width: 200px;">
        <v-select
          v-model="sort"
          :items="sortItems"
          variant="solo"
          density="compact"
          hide-details
          bg-color="white"
          flat
          rounded="lg"
          class="sort-select"
          menu-icon="mdi-chevron-down"
        />
        
        <v-btn icon variant="tonal" rounded="lg" color="grey-darken-1" class="bg-white ml-2">
          <v-icon>mdi-view-grid-outline</v-icon>
        </v-btn>
        
        <v-btn icon variant="flat" rounded="lg" color="#5C6BC0" class="ml-2">
          <v-icon color="white">mdi-format-list-bulleted</v-icon>
        </v-btn>
      </div>
    </div>

    <!-- List Header / Count -->
    <div class="text-caption text-medium-emphasis mb-4">
      {{ filtered.length }} of {{ books.length }} books
    </div>

    <!-- List -->
    <div>
      <BookListItem
        v-for="b in paged"
        :key="b.id"
        :book="b"
        @view="onView"
        @edit="onEdit"
        @delete="onDelete"
      />
    </div>

    <!-- Pagination -->
    <div class="d-flex justify-end mt-6" v-if="totalPages > 1">
      <v-pagination
        v-model="page"
        :length="totalPages"
        :total-visible="6"
        density="comfortable"
        active-color="#5C6BC0"
      />
    </div>

    <v-dialog v-model="addDialog" max-width="520">
      <v-card class="rounded-xl">
        <v-card-title>Add Book</v-card-title>
        <v-card-text class="text-body-2 text-medium-emphasis">
          (Next step) Here we’ll add the real form + API integration.
        </v-card-text>
        <v-card-actions class="justify-end">
          <v-btn variant="text" @click="addDialog = false">Close</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>

<script setup lang="ts">
import { computed, ref } from "vue";
import { useRouter } from "vue-router";
import BookListItem from "@/components/books/BookListItem.vue";

type Book = {
  id: string;
  title: string;
  author: string;
  isbn: string;
  rating?: number | null;
  comments?: string | null;
  coverImageUrl?: string | null;
};

const router = useRouter();

const addDialog = ref(false);
const search = ref("");
const sort = ref<"title">("title");
const sortItems = [{ title: "Sort by Title", value: "title" }];

const page = ref(1);
const pageSize = 10;

// mock data
const books = ref<Book[]>([
  { id: "1", title: "1984", author: "George Orwell", isbn: "978-0451524935", rating: 4.5, comments: "Haunting." },
  { id: "2", title: "Pride and Prejudice", author: "Jane Austen", isbn: "978-0141439518", rating: 5, comments: "Classic." },
  { id: "3", title: "The Great Gatsby", author: "F. Scott Fitzgerald", isbn: "978-0743273565", rating: 4.5, comments: "Great read." },
  { id: "4", title: "The Hobbit", author: "J.R.R. Tolkien", isbn: "978-0547928227", rating: 4.5, comments: "Fun adventure." },
  { id: "5", title: "To Kill a Mockingbird", author: "Harper Lee", isbn: "978-0061120084", rating: 5, comments: "Powerful." },
]);

const filtered = computed(() => {
  const q = search.value.trim().toLowerCase();
  const list = !q
    ? books.value
    : books.value.filter(
        (b) =>
          b.title.toLowerCase().includes(q) ||
          b.author.toLowerCase().includes(q)
      );

  return [...list].sort((a, b) => a.title.localeCompare(b.title));
});

const totalPages = computed(() =>
  Math.max(1, Math.ceil(filtered.value.length / pageSize))
);

const paged = computed(() => {
  const start = (page.value - 1) * pageSize;
  return filtered.value.slice(start, start + pageSize);
});

function onView(id: string) {
  router.push(`/books/${id}`);
}
function onEdit(id: string) {
  console.log("edit", id);
}
function onDelete(id: string) {
  console.log("delete", id);
}
</script>

<style scoped>
.search-input :deep(.v-field) {
  border-radius: 8px;
  border: 1px solid #E0E0E0;
  box-shadow: none !important;
}

.sort-select :deep(.v-field) {
  border-radius: 8px;
  border: 1px solid #E0E0E0;
  box-shadow: none !important;
}
</style>
