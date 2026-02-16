import { defineStore } from "pinia";
import { getBooks, type Book } from "@/api/booksApi";

export const useBooksStore = defineStore("books", {
    state: () => ({
        items: [] as Book[],
        page: 1,
        pageSize: 10,
        totalCount: 0,
        totalPages: 1,
        search: "",
        sort: "title",
        loading: false,
        error: "" as string | null,
    }),

    actions: {
        async fetch() {
            this.loading = true;
            this.error = null;

            try {
                const res = await getBooks({
                    page: this.page,
                    pageSize: this.pageSize,
                    search: this.search || undefined,
                    sort: this.sort,
                });

                this.items = res.items;
                this.totalCount = res.totalCount;
                this.totalPages = Math.max(1, res.totalPages);
            } catch (e: any) {
                this.error = e?.response?.data?.message ?? "Failed to load books.";
            } finally {
                this.loading = false;
            }
        },

        setSearch(value: string) {
            this.search = value;
            this.page = 1;
        },

        setSort(value: string) {
            this.sort = value;
            this.page = 1;
        },

        setPage(value: number) {
            this.page = value;
        },
    },
});
