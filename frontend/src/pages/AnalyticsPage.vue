<template>
  <div>
    <div class="text-h5 font-weight-bold mb-4">Analytics</div>

    <v-row class="mb-6">
      <v-col cols="12" md="4">
        <v-card class="pa-4 rounded-xl">
          <div class="text-caption text-medium-emphasis">Total Books</div>
          <div class="text-h4 font-weight-bold">{{ total }}</div>
        </v-card>
      </v-col>

      <v-col cols="12" md="4">
        <v-card class="pa-4 rounded-xl">
          <div class="text-caption text-medium-emphasis">Average Rating</div>
          <div class="text-h4 font-weight-bold">{{ avgRating }}</div>
        </v-card>
      </v-col>

      <v-col cols="12" md="4">
        <v-card class="pa-4 rounded-xl">
          <div class="text-caption text-medium-emphasis">Books With Notes</div>
          <div class="text-h4 font-weight-bold">{{ notesCount }}</div>
        </v-card>
      </v-col>
    </v-row>

    <v-card class="pa-4 rounded-xl">
      <div class="text-subtitle-1 font-weight-semibold mb-4">Rating Distribution</div>
      <v-chart class="chart" :option="chartOption" autoresize />
    </v-card>
  </div>
</template>

<script setup lang="ts">
import { computed, onMounted } from "vue";
import { useBooksStore } from "@/stores/books.store";
import VChart from "vue-echarts";

const store = useBooksStore();

onMounted(() => store.fetch());

const total = computed(() => store.totalCount);

const ratedBooks = computed(() => store.items.filter(b => b.rating));

const avgRating = computed(() => {
  if (!ratedBooks.value.length) return "-";
  const sum = ratedBooks.value.reduce((a,b)=>a+(b.rating||0),0);
  return (sum / ratedBooks.value.length).toFixed(1);
});

const notesCount = computed(() =>
  store.items.filter(b => b.comments && b.comments.trim().length > 0).length
);

const distribution = computed(() => {
  const arr = [0,0,0,0,0];
  store.items.forEach(b => {
    if (b.rating) arr[b.rating-1]++;
  });
  return arr;
});

const chartOption = computed(() => ({
  xAxis: { type: "category", data: ["1★","2★","3★","4★","5★"] },
  yAxis: { type: "value" },
  series: [
    {
      data: distribution.value,
      type: "bar",
      smooth: true
    }
  ]
}));
</script>

<style scoped>
.chart { height: 300px; }
</style>
