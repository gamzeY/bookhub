<template>
  <v-card class="mb-3 rounded-lg border-opacity-100" elevation="0" border color="white">
    <div class="d-flex align-center pa-4">
      <div class="me-4">
        <v-sheet
          color="#5C6BC0"
          width="48"
          height="48"
          class="d-flex align-center justify-center rounded"
        >
          <v-icon icon="mdi-book" color="white" size="24" />
        </v-sheet>
      </div>

      <div class="flex-grow-1">
        <div class="text-subtitle-1 font-weight-bold text-truncate">{{ book.title }}</div>
        <div class="text-caption text-medium-emphasis">{{ book.author }}</div>
        
        <div class="d-flex align-center mt-1 ga-3">
          <div class="text-caption text-disabled" v-if="book.isbn">ISBN: {{ book.isbn }}</div>
          
          <div v-if="book.comments" class="d-flex align-center text-caption text-success font-weight-medium">
            <v-icon icon="mdi-check-circle" size="14" class="me-1" />
            Has notes
          </div>
        </div>
      </div>

      <div class="d-flex align-center ga-4">
        <div class="d-flex align-center">
          <v-rating
            v-if="book.rating"
            :model-value="book.rating"
            color="amber-darken-2"
            active-color="amber-darken-2"
            density="compact"
            size="small"
            readonly
            half-increments
          />
          <span class="text-caption ms-2 text-medium-emphasis" v-if="book.rating">{{ book.rating }}/5</span>
        </div>

        <div class="d-flex align-center ga-2">
          <v-btn
            icon
            variant="text"
            density="comfortable"
            size="small"
            color="grey-darken-1"
            class="rounded border"
            @click="$emit('edit', book.id)"
          >
            <v-icon icon="mdi-pencil-outline" size="18" />
          </v-btn>

          <v-btn
            icon
            variant="text"
            density="comfortable"
            size="small"
            color="grey-darken-1"
            class="rounded border"
            @click="$emit('view', book.id)"
          >
            <v-icon icon="mdi-eye-outline" size="18" />
          </v-btn>

          <v-btn
            icon
            variant="text"
            density="comfortable"
            size="small"
            color="grey-darken-1"
            class="rounded border"
            @click="$emit('delete', book.id)"
          >
            <v-icon icon="mdi-delete-outline" size="18" />
          </v-btn>
        </div>
      </div>
    </div>
  </v-card>
</template>

<script setup lang="ts">
import { PropType } from 'vue';

type Book = {
  id: string;
  title: string;
  author: string;
  isbn: string;
  rating?: number | null;
  comments?: string | null;
  coverImageUrl?: string | null;
};

defineProps({
  book: {
    type: Object as PropType<Book>,
    required: true
  }
});

defineEmits<{
  (e: 'view', id: string): void;
  (e: 'edit', id: string): void;
  (e: 'delete', id: string): void;
}>();
</script>

<style scoped>
.v-btn.border {
  border-color: #E0E0E0 !important;
}
</style>
