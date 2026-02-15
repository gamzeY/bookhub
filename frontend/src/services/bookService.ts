import axios from 'axios';

const API_URL = 'http://localhost:5205/api';

export interface Book {
    id: string;
    title: string;
    author: string;
    isbn: string;
    rating?: number | null;
    comments?: string | null;
    coverImageUrl?: string | null;
}

export interface PagedResult<T> {
    items: T[];
    page: number;
    pageSize: number;
    totalCount: number;
    totalPages: number;
}

export interface CreateBookRequest {
    title: string;
    author: string;
    isbn: string;
    rating?: number | null;
    comments?: string | null;
    coverImageUrl?: string | null;
}

export interface UpdateBookRequest {
    rating?: number | null;
    comments?: string | null;
}

const apiClient = axios.create({
    baseURL: API_URL,
    headers: {
        'Content-Type': 'application/json',
    },
});

export const bookService = {
    async getAll(page = 1, pageSize = 10, search = '', sort = 'title'): Promise<PagedResult<Book>> {
        const response = await apiClient.get<PagedResult<Book>>('/books', {
            params: { page, pageSize, search, sort },
        });
        return response.data;
    },

    async getById(id: string): Promise<Book> {
        const response = await apiClient.get<Book>(`/books/${id}`);
        return response.data;
    },

    async create(book: CreateBookRequest): Promise<Book> {
        const response = await apiClient.post<Book>('/books', book);
        return response.data;
    },

    async update(id: string, update: UpdateBookRequest): Promise<Book> {
        const response = await apiClient.put<Book>(`/books/${id}`, update);
        return response.data;
    },

    async delete(id: string): Promise<void> {
        await apiClient.delete(`/books/${id}`);
    },
};
