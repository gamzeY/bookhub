import { createRouter, createWebHistory } from "vue-router";
import AppLayout from "@/components/layout/AppLayout.vue";

const MyBooksPage = () => import("@/pages/MyBooksPage.vue");
const BookDetailsPage = () => import("@/pages/BookDetailsPage.vue");
const AnalyticsPage = () => import("@/pages/AnalyticsPage.vue");
const SettingsPage = () => import("@/pages/SettingsPage.vue");

const DashboardPage = () => import("@/pages/DashboardPage.vue");

export const router = createRouter({
    history: createWebHistory(),
    routes: [
        {
            path: "/",
            component: AppLayout,
            children: [
                { path: "", redirect: "/dashboard" },
                { path: "dashboard", component: DashboardPage },
                { path: "books", component: MyBooksPage },
                { path: "books/:id", component: BookDetailsPage },
                { path: "analytics", component: AnalyticsPage },
                { path: "settings", component: SettingsPage },
            ],
        },
    ],
});
