# 🚀 ChatStream — Real-Time Chat System

**ChatStream** is a high-performance, real-time messaging application built on **.NET 9**, leveraging **SignalR** for instantaneous data transmission and following **Clean Architecture**.

---

## 📸 Screenshots

### 🔑 Authentication
> *User registration and secure login interface.*
<img width="1913" height="856" alt="LoginPage" src="https://github.com/user-attachments/assets/b532b786-c0d9-40d6-bbdb-e5f1e011d7bc" />


### 💬 Chat Interface
> *Main dashboard showing room list and active chat.*

<img width="1917" height="851" alt="ChatLobby" src="https://github.com/user-attachments/assets/1c8fee87-5681-40f6-96c1-50f59f39cdf2" />


### ⚡ Real-Time Interaction
> *Instant message broadcasting in action.*
<img width="1918" height="896" alt="ChatScreen" src="https://github.com/user-attachments/assets/c779bc35-61c6-4d58-aa2a-318f8dcac3e2" />

---

## ✨ Core Features
*   **Real-time Messaging:** Instant message delivery using WebSockets via ASP.NET Core SignalR.
*   **Secure Authentication:** User registration and login protected by JWT Bearer Tokens and BCrypt.
*   **Room Management:** Dynamic creation and joining of distinct chat rooms.
*   **Persistent History:** Automatic storage and retrieval of message history from SQL Server.
*   **Auto-Migrations:** Database schema is automatically initialized on startup via EF Core.

---

## 🛠 Tech Stack

| Feature | Technology |
| :--- | :--- |
| **Real-time messaging** | ASP.NET Core SignalR (WebSockets) |
| **REST API** | ASP.NET Core Web API |
| **Authentication** | JWT Bearer Tokens + BCrypt |
| **Database** | SQL Server (LocalDB for dev) |
| **ORM** | Entity Framework Core 9 (auto-migrate) |
| **Frontend** | HTML + Vanilla JavaScript |
| **Logging** | Serilog → Console |

---

---
## How to Run

```powershell
cd d:\chatrealtimesystem
dotnet run --project src/ChatApp.API/ChatApp.API.csproj
```

Then open **http://localhost:5000** in your browser.

> [!NOTE]
> The database schema is **auto-created on startup** via EF Core migrations. No manual setup needed.

> [!IMPORTANT]
> Before deploying to production, change the `Jwt.Key` in `appsettings.json` to a strong, random secret.

---

## API Endpoints

| Method | Route | Auth | Description |
|---|---|---|---|
| POST | `/api/auth/register` | No | Create account |
| POST | `/api/auth/login` | No | Login → JWT |
| GET | `/api/rooms` | JWT | List all rooms |
| POST | `/api/rooms` | JWT | Create a room |
| GET | `/api/rooms/{id}/messages` | JWT | Paginated history |
| WS | `/hubs/chat` | JWT | SignalR real-time hub |

## SignalR Hub Methods

| Direction | Method | Description |
|---|---|---|
| Client → Server | `JoinRoom(roomId)` | Add to room group |
| Client → Server | `LeaveRoom(roomId)` | Remove from group |
| Client → Server | `SendMessage(roomId, content)` | Persist + broadcast |
| Server → Client | `ReceiveMessage(MessageDto)` | Incoming message |
| Server → Client | `UserJoined(username)` | Join notification |
| Server → Client | `UserLeft(username)` | Leave notification |
---
## 📂 Project Structure

```text
ChatRealtimeSystem/
├── src/
│   ├── ChatApp.API/           ← Web server, SignalR Hub, controllers
│   │   ├── Controllers/       AuthController, RoomsController
│   │   ├── Hubs/              ChatHub.cs (SignalR)[cite: 1]
│   │   ├── Services/          JwtService.cs[cite: 1]
│   │   ├── wwwroot/           index.html (frontend)[cite: 1]
│   │   └── Program.cs[cite: 1]
│   ├── ChatApp.Core/          ← Entities + interfaces (no dependencies)[cite: 1]
│   ├── ChatApp.Infrastructure/← EF Core DbContext + repositories[cite: 1]
│   └── ChatApp.Shared/        ← DTOs shared across layers[cite: 1]
---

