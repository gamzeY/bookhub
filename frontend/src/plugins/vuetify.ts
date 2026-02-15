import "vuetify/styles";
import { createVuetify } from "vuetify";
import { md3 } from "vuetify/blueprints";
import * as components from "vuetify/components";
import * as directives from "vuetify/directives";

export const vuetify = createVuetify({
    blueprint: md3,
    components,
    directives,
    theme: {
        defaultTheme: "light",
        themes: {
            light: {
                colors: {
                    primary: "#4F46E5", // indigo
                    secondary: "#2563EB",
                },
            },
        },
    },
});
