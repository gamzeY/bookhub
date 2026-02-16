import { http } from "./http";

export type Book = {
    id: string;
    title: string;
    author: string;
    isbn: string;
    rating?: number | null;
    comments?: string | null;
    coverImageUrl?: string | null;
    hasNotes?: boolean;
};

export type PagedResult<T> = {
    items: T[];
    page: number;
    pageSize: number;
    totalCount: number;
    totalPages: number;
};

export async function getBooks(params: {
    page: number;
    pageSize: number;
    search?: string;
    sort?: string;
}) {
    const { data } = await http.get<PagedResult<Book>>("/books", { params });
    return data;
}

export async function getBook(id: string) {
    const { data } = await http.get<Book>(`/books/${id}`);
    return data;
}

export async function createBook(payload: {
    title: string;
    author: string;
    isbn: string;
    rating?: number | null;
    comments?: string | null;
    coverImageUrl?: string | null;
}) {
    const { data } = await http.post<Book>("/books", payload);
    return data;
}

export async function updateBook(id: string, payload: { rating?: number | null; comments?: string | null }) {
    const { data } = await http.put<Book>(`/books/${id}`, payload);
    return data;
}

export async function deleteBook(id: string) {
    await http.delete(`/books/${id}`);
}
