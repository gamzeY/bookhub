# --- Stage 1: Frontend Build ---
FROM node:20 AS frontend-build
WORKDIR /app/frontend
COPY frontend/package*.json ./
RUN npm install
COPY frontend/ ./
# Backend url as "/" because they will be on the same domain
ENV VITE_API_BASE_URL=/api
RUN npm run build

# --- Stage 2: Backend Build ---
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS backend-build
WORKDIR /app/backend
COPY backend/*.csproj ./
RUN dotnet restore
COPY backend/ ./
RUN dotnet publish -c Release -o /out

# --- Stage 3: Final Image ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=backend-build /out .
# Copy frontend 'dist' into 'wwwroot' for the backend to serve
COPY --from=frontend-build /app/frontend/dist ./wwwroot

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "backend.dll"]
