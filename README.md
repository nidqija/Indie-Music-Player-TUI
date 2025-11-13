# 🎵 MusicInput Station

**MusicInput Station** is an open-source **C# console application** built with **.NET**, **Spectre.Console**, and **Entity Framework Core**.  
It lets you **search, play, and manage songs** directly from your terminal — integrated with the **Jamendo Music API**.  
The app features a rich interactive interface, song logs, artist search, and playlist management — all designed for music enthusiasts and developers alike.

---

## 🚀 Features

- 🎧 **Search & Play Songs**  
  Browse music from the Jamendo API by title or artist name.  

- 🧩 **Playlist Management**  
  Save your favorite songs and collections locally.  

- 📜 **Play History Logs**  
  Keep a record of all the songs you’ve played.  

- 🔊 **Audio Playback Support**  
  Uses `NAudio` to stream and play audio files directly in your console.  

- 💾 **Local Database (EF Core)**  
  Stores song data, collections, and play history in an SQLite database through **Entity Framework Core**.  

- 🖼️ **Modern Console UI**  
  Built using **Spectre.Console** with colorful menus, prompts, and FIGlet banners.  

- 🐳 **Containerized with Podman**  
  Easily build and run the app in an isolated container environment for consistency and portability.  

---

## 🛠️ Tech Stack

| Component | Technology |
|------------|-------------|
| Language | C# (.NET 8 / .NET 6) |
| API | [Jamendo Music API](https://developer.jamendo.com/v3.0) |
| Database | postgreSQL with Entity Framework Core |
| Console UI | Spectre.Console |
| Audio Playback | NAudio |
| Configuration | DotNetEnv |
| Containerization | Podman |

---

## ⚙️ Installation (Local Setup)

1. **Clone this repository**
   ```bash
   git clone https://github.com/your-username/musicinput-station.git
   cd musicinput-station

2. **🧩 Step 2 — Create Environment File**
   Create a `.env` file in the root directory with the following content:
   ```bash
   JAMENDO_CLIENT_ID=your_jamendo_api_client_id

3. **🧩 Step 3 — Install Dependencies**
   Restore dependencies using:
   ```bash
   dotnet restore

4. **🧩 Step 4 — Run the Application**
   Run the project:
   ```bash
   dotnet run

5. You’ll now see the main interactive console menu appear.

---

## 🐳 Containerization (Using Podman)
You can run MusicInput Station in a container to avoid installing dependencies locally. This is recommended for contributors or developers who want a hassle-free setup.


1. **Build the Container Image**
   ```bash
   podman build -t musicinput-station .

2. **Run the Container**
   ```bash
   podman run -it --env-file .env musicinput-station


### 📝 Notes

- Uses a **Linux-based container**, works on WSL, Linux, and macOS.  
- No need to install **.NET**, **NAudio**, or **EF Core** on your host machine.  
- Container keeps your environment **consistent for all contributors**.  
- To rebuild after changes:
  ```bash
  podman build --no-cache -t musicinput-station .


---

## 🤝 Contributing

This project is **open-source** and welcomes contributions!  

- Feel free to **fork**, **create issues**, or **submit pull requests**.  
- You can add new features, improve the **console UI**, or enhance **containerization**.  
- Make sure to **test changes locally** or inside the **Podman container**.  

Enjoy building and sharing music with **MusicInput Station**! 🎶




