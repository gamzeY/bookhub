<template>
  <div>
    <div class="text-h5 font-weight-bold mb-4">Settings</div>

    <v-card class="pa-4 rounded-xl">
      <div class="d-flex align-center justify-space-between">
        <div>
          <div class="text-subtitle-1 font-weight-semibold">Dark Mode</div>
          <div class="text-body-2 text-medium-emphasis">
            Toggle application theme
          </div>
        </div>

        <v-switch v-model="dark" inset color="primary" />
      </div>
    </v-card>
  </div>
</template>

<script setup lang="ts">
import { ref, watch } from "vue";
import { useTheme } from "vuetify";

const theme = useTheme();
const dark = ref(theme.global.name.value === "dark");

watch(dark, (v) => {
  theme.global.name.value = v ? "dark" : "light";
});
</script>
