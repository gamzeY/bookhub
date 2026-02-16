While implementing the UI I noticed that theme handling (especially dark mode)
affects perceived quality more than additional features.

Instead of adding more components, I focused on making the existing interface
consistent, readable and visually stable across themes.

The notes below summarize both fixes I implemented and improvements I would
apply if this were a production product.


# 🎨 Visual Improvements

This document tracks the visual enhancements and theme-related fixes made to the BookHub application.

---

## ✅ Completed Improvements

### 🌙 Dark Mode & Theme Awareness
- **AppLayout Background Fix**: Removed hardcoded `bg-white` from `v-main` in `AppLayout.vue`. The application now correctly uses the theme's background color in both Light and Dark modes.
- **Book List Border Visibility**:
    - Removed hardcoded `color="white"` and `border-opacity-100` from `BookListItem.vue` cards.
    - Replaced hardcoded `#e0e0e0` borders with theme-aware CSS variables: `rgba(var(--v-border-color), var(--v-border-opacity))`.
- **Search & Filter Refinement**: 
    - Removed hardcoded `bg-color="white"` from search inputs and select fields in `MyBooksPage.vue`.
    - Updated scoped styles to use theme-compliant borders, ensuring input fields are visible and look premium in Dark Mode.

### 🍱 UI Refinement
- **Button Borders**: Updated action buttons to use standard Vuetify border variables instead of static hex codes.
- **Icon Consistency**: Ensured icons have proper contrast against variable backgrounds.

---

## 🚀 Proposed Next Steps

### 🔍 Micro-interactions
- [ ] **Hover Effects**: Add subtle elevation or scale transitions to `BookListItem` cards.
- [ ] **Loading States**: Implement skeleton loaders for the book list and detail pages to improve perceived performance.

### 📊 Data Visualization
- [ ] **ECharts Theme Sync**: Dynamically update ECharts themes (Light/Dark) in `AnalyticsPage.vue` based on the active Vuetify theme.
- [ ] **Transitions**: Add smooth entry animations for dashboard charts.

### 🎨 Design Polish
- [ ] **Glassmorphism**: Apply subtle backdrop-filters to the `AppSidebar` for a more modern, premium feel.
- [ ] **Typography**: Audit and standardize font weights across headers and body text for better hierarchy.
