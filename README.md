# 📚 BookHub – Full Stack Developer Assessment

A simple full-stack book tracking application.

Users can add, review, search and manage their personal book collection, while also viewing statistics about their reading habits.

---

## 🧱 Tech Stack

### Frontend
- Vue 3 + Vite
- TypeScript
- Vuetify (UI)
- Pinia (State Management)
- Vue Router
- ECharts (Analytics)
- Vitest + Vue Test Utils

### Backend
- .NET Web API (C#)
- In-Memory Storage
- FluentValidation
- Swagger
- xUnit (Unit Testing)

---

## ✨ Features

### 📖 My Books
- List books with:
  - Title, Author, ISBN
  - Notes indicator
  - Rating stars
  - Actions (View / Edit / Delete)
- Add book (modal form)
- Edit book (rating & comments only)
- Delete confirmation dialog
- View book details page
- Search by title or author
- Sort by title
- Pagination (10 per page)

### 📊 Analytics
- Total books count
- Average rating
- Books with notes
- Rating distribution chart

### ⚙️ Settings
- Light / Dark theme toggle

---

## 🔒 Validation Rules
Enforced both in backend and UI:

- Maximum **25 books**
- Rating must be between **1 – 5**
- If rating is given → comments required
- Comments must NOT contain the word **"horrible"**
- Reasonable string length limits

---

## 🚀 Getting Started

### Prerequisites
- Node.js 18+
- .NET SDK 8+

---

## 🖥️ Backend (API)

```bash
cd backend
dotnet restore
dotnet run
```

**Swagger UI:**
```
http://localhost:5205/swagger
```

**Run backend tests:**
```bash
dotnet test
```

---

## 🎨 Frontend (UI)

```bash
cd frontend
npm install
```

**Create `frontend/.env.local`:**
```env
VITE_API_BASE_URL=http://localhost:5205/api
```

**Run app:**
```bash
npm run dev
```

**Run frontend tests:**
```bash
npm run test
```

**Build production:**
```bash
npm run build
```

---

## 📝 Notes

- Backend uses **in-memory storage**
- Restarting backend **clears all books**
- Add sample books via Swagger or UI after restart

---

## 📁 Project Structure

```
bookhub/
 ├── backend/
 │    ├── Controllers
 │    ├── Models
 │    ├── Services
 │    ├── Validation
 │    └── Tests
 │
 └── frontend/
      ├── pages
      ├── components
      ├── stores
      ├── api
      └── tests
```

---

## 🧪 Testing

**Backend:**
```bash
dotnet test
```

**Frontend:**
```bash
npm run test
```