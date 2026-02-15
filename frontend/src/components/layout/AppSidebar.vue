<template>
  <v-navigation-drawer
    :model-value="drawer"
    @update:model-value="$emit('update:drawer', $event)"
    color="#2563EB"
    width="260"
    permanent
    class="text-white"
  >
    <div class="px-4 py-4 d-flex align-center ga-2">
      <v-icon icon="mdi-book-open-page-variant" color="amber-accent-2" />
      <div class="text-h6 font-weight-semibold">BookHub</div>
    </div>

    <v-list nav density="compact" class="text-white">
      <v-list-item
        title="Dashboard"
        prepend-icon="mdi-view-dashboard"
        to="/dashboard"
      />
      <v-list-item
        title="My Books"
        prepend-icon="mdi-book"
        to="/books"
      />
      <v-list-item
        title="Analytics"
        prepend-icon="mdi-chart-box"
        to="/analytics"
      />
      <v-list-item
        title="Settings"
        prepend-icon="mdi-cog"
        to="/settings"
      />
    </v-list>

    <template #append>
      <div class="pa-4 border-t border-opacity-10">
        <div class="d-flex align-center ga-3">
          <v-avatar color="blue-lighten-4" size="40">
            <v-icon icon="mdi-account" color="#2563EB" />
          </v-avatar>
          <div>
            <div class="text-subtitle-2 font-weight-bold">Book Lover</div>
            <div class="text-caption opacity-80">Reader</div>
          </div>
        </div>
      </div>
    </template>
  </v-navigation-drawer>
</template>

<script setup lang="ts">
defineProps<{ drawer: boolean }>();
defineEmits<{ (e: "update:drawer", v: boolean): void }>();
</script>

<style scoped>
:deep(.v-list-item--active) {
  background: rgba(255, 255, 255, 0.15) !important;
  color: white !important;
}
.border-t {
  border-top: 1px solid rgba(255, 255, 255, 0.1);
}
</style>
