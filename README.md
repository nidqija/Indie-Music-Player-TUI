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

