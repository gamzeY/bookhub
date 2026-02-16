import { createApp } from "vue";
import "@mdi/font/css/materialdesignicons.css";
import App from "./App.vue";
import { createPinia } from "pinia";
import { router } from "./router";
import { vuetify } from "./plugins/vuetify";

//ECHARTS
import VueECharts from "vue-echarts";
import { use } from "echarts/core";
import { CanvasRenderer } from "echarts/renderers";
import { BarChart } from "echarts/charts";
import {
    GridComponent,
    TooltipComponent,
    TitleComponent,
    LegendComponent
} from "echarts/components";

use([
    CanvasRenderer,
    BarChart,
    GridComponent,
    TooltipComponent,
    TitleComponent,
    LegendComponent
]);

const app = createApp(App);

app.use(createPinia());
app.use(router);
app.use(vuetify);

app.component("v-chart", VueECharts);

app.mount("#app");
