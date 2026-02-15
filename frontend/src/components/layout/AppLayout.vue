<template>
  <v-app>
    <AppSidebar v-model:drawer="drawer" />

    <v-main class="bg-white">
      <v-container class="py-8 px-6" style="max-width: 1200px;">
        <RouterView />
      </v-container>
    </v-main>
  </v-app>
</template>

<script setup lang="ts">
import { ref } from "vue";
import AppSidebar from "./AppSidebar.vue";

const drawer = ref(true);
</script>
