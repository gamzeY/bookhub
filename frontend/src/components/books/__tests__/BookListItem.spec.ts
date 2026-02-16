import { mount } from "@vue/test-utils";
import { test, expect } from "vitest";
import BookListItem from "../BookListItem.vue";

test("shows notes badge when comments exist", () => {
    const wrapper = mount(BookListItem, {
        props: {
            book: {
                id: "1",
                title: "Test",
                author: "A",
                isbn: "123",
                rating: 4,
                comments: "hello"
            }
        }
    });

    expect(wrapper.text()).toContain("Has notes");
});
