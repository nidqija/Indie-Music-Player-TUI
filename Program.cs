// this is a music input program
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using DotNetEnv;
using Spectre.Console;
using NAudio;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Npgsql;
using Models;





// class to display menu choices
class MusicChoices
{
    private string[] menuChoices = { "1. Search Songs", "2. Playlists", "3. Search by Artists" };
    private string choice;

    public void printChoices()
    {
        // using Spectre.Console to display menu choices
        choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("[yellow]Please select an option:[/]")
            .AddChoices(menuChoices)
        );
    }

    public string returnChoice()
    {
        return choice;
    }
}

class Songs
{
    private string songName;
    private Songs songNameChoice;

    public void setSongName(string song)
    {
        songName = song;
    }

    public string getSongName()
    {
        return songName;
    }

    public string Name { get; set; }
    public string Artist { get; set; }
    public string Url { get; set; }

    public async Task searchSong()
    {
        Console.WriteLine("Searching for the song: " + getSongName());
        Env.Load(@"C:\Users\User\source\repos\MusicPlayer\MusicPlayer\.env");
        string client = Environment.GetEnvironmentVariable("JAMENDO_CLIENT_ID");

        using var httpClient = new HttpClient();
        string url = $"https://api.jamendo.com/v3.0/tracks/?client_id={client}&format=json&limit=5&search={getSongName()}";

        string response = await httpClient.GetStringAsync(url);
        using JsonDocument doc = JsonDocument.Parse(response);
        JsonElement root = doc.RootElement.GetProperty("results");

        if (root.GetArrayLength() == 0)
        {
            Console.WriteLine("No results found for the song: " + getSongName());
        }
        else
        {
            AnsiConsole.Clear();
            Console.WriteLine("Top 5 results: ");
            List<Songs> songlists = new List<Songs>();

            foreach (JsonElement track in root.EnumerateArray())
            {
                string name = track.GetProperty("name").GetString();
                string artist = track.GetProperty("artist_name").GetString();
                string audioUrl = track.GetProperty("audio").GetString();

                Console.WriteLine("================================");
                Console.WriteLine($"{name} by {artist}");
                songlists.Add(new Songs { Name = name, Artist = artist, Url = audioUrl });
            }

            Console.WriteLine("\n");
            songNameChoice = AnsiConsole.Prompt(
                new SelectionPrompt<Songs>().Title("[yellow]Enter your choice of song to play:[/]")
                 .UseConverter(songname => $"{songname.Name} by {songname.Artist}")
                 .AddChoices(songlists)
             );
        }
    }

    public Songs returnPlaySongs()
    {
        return songNameChoice;
    }
}

class Playlist
{
    private List<Songs> playlistSongs = new List<Songs>();

    public Playlist() { }

    // Show how many songs are in playlist
    public void ShowPlaylist()
    {
        if (playlistSongs.Count == 0)
        {
            Console.WriteLine("Your playlist is empty.");
        }
        else
        {
            Console.WriteLine($"\n Your Playlist has {playlistSongs.Count} songs");
            foreach (var song in playlistSongs)
            {
                Console.WriteLine($"- {song.Name} by {song.Artist}");
            }
        }
    }

    // Add song into playlist
    public void AddSong(Songs song)
    {
        playlistSongs.Add(song);
        Console.WriteLine($" Added {song.Name} by {song.Artist} to your playlist!");
        Console.WriteLine($"Your playlist now has {playlistSongs.Count} songs");
    }

    // Insert via search (Jamendo + user pick)
    public async Task InsertSongFromSearch(string searchQuery)
    {
        Env.Load(@"C:\Users\User\source\repos\MusicPlayer\MusicPlayer\.env");
        string client = Environment.GetEnvironmentVariable("JAMENDO_CLIENT_ID");

        using var httpClient = new HttpClient();
        string url = $"https://api.jamendo.com/v3.0/tracks/?client_id={client}&format=json&limit=5&search={searchQuery}";
        string response = await httpClient.GetStringAsync(url);

        using JsonDocument doc = JsonDocument.Parse(response);
        JsonElement root = doc.RootElement.GetProperty("results");

        if (root.GetArrayLength() == 0)
        {
            Console.WriteLine(" No results found.");
            return;
        }

        List<Songs> searchResults = new List<Songs>();
        foreach (JsonElement track in root.EnumerateArray())
        {
            searchResults.Add(new Songs
            {
                Name = track.GetProperty("name").GetString(),
                Artist = track.GetProperty("artist_name").GetString(),
                Url = track.GetProperty("audio").GetString()
            });
        }

        Songs chosenSong = AnsiConsole.Prompt(
            new SelectionPrompt<Songs>()
                .Title("[yellow]Choose a song to add:[/]")
                .UseConverter(s => $"{s.Name} by {s.Artist}")
                .AddChoices(searchResults)
        );

        AddSong(chosenSong);
    }

    // Get full playlist
    public List<Songs> GetSongs()
    {
        return playlistSongs;
    }
}

class PlaySongs
{
    public void playSong(Songs song)
    {
        if (song == null || string.IsNullOrEmpty(song.Url))
        {
            Console.WriteLine("❌ No song selected or URL is invalid.");
            return;
        }

        using (var webClient = new WebClient())
        {
            string tempFile = Path.GetTempFileName() + ".mp3";
            webClient.DownloadFile(song.Url, tempFile);

            using var audioFile = new NAudio.Wave.AudioFileReader(tempFile);
            using var outputDevice = new NAudio.Wave.WaveOutEvent();
            outputDevice.Init(audioFile);
            outputDevice.Play();

            Console.WriteLine("🎵 Now playing: " + song.Name + " by " + song.Artist);
            Console.WriteLine("Press any key to stop playback...");
            
            Console.ReadKey();

            outputDevice.Stop();
            File.Delete(tempFile);
        }
    }
}

class EnvFile
{
    public static void doEnvOps(string[] args)
    {
        Env.Load(@"C:\Users\User\source\repos\MusicPlayer\MusicPlayer\.env");
        string client = Environment.GetEnvironmentVariable("JAMENDO_CLIENT_ID");
    }
};




// main class
class MusicInput
{
    string input;
    string intro = "Welcome to MusicInput Station!";

    static async Task Main(string[] args)

    {





        //====================== using Spectre.Console to display title ===========================//
        var fontPath = Path.Combine("fonts", "alligator2.flf");
        var font = FigletFont.Load(fontPath);

      
      

    while (true)
        {
            AnsiConsole.Write(
          new FigletText(font, "MusicInput Station!")
          .Centered().Color(Color.Yellow)
      );
            MusicInput musicInput = new MusicInput();
            Console.WriteLine("\n");
            musicInput.input = "Choose your options!";
            Console.WriteLine(musicInput.input);

            // display choices
            MusicChoices musicChoices = new MusicChoices();
            musicChoices.printChoices();

            // handle env file operations
            EnvFile.doEnvOps(args);

            //============================ get user choice and handles it =================================//
            var choice = musicChoices.returnChoice();
            switch (choice)
            {
                case "1. Search Songs":
                    AnsiConsole.Clear();
                    AnsiConsole.Write(new FigletText(font, "Search Songs")
                        .Centered().Color(Color.Yellow));
                    Console.WriteLine("\n");
                    Console.WriteLine("Enter your song name: ");
                    string songInput = Console.ReadLine();
                    Songs songs = new Songs();
                    songs.setSongName(songInput);
                    AnsiConsole.Clear();
                    Console.WriteLine("Submitted! Wait for a while...");
                    await songs.searchSong();

                    PlaySongs player = new PlaySongs();
                    player.playSong(songs.returnPlaySongs());
                    
                    break;

                case "2. Playlists":
                    AnsiConsole.Clear();
                    AnsiConsole.Write(new FigletText(font, "Playlist")
                     .Centered().Color(Color.Purple));
                    Console.WriteLine("\n");
                    Playlist playlist = new Playlist();

                    // Show playlist
                    playlist.ShowPlaylist();

                    // Ask user if they want to add
                    string addSong = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("[green]Do you want to add a song?[/]")
                            .AddChoices("Yes", "No")
                    );

                    if (addSong == "Yes")
                    {
                        Console.Write("Enter search keyword: ");
                        string keyword = Console.ReadLine();
                        await playlist.InsertSongFromSearch(keyword);

                        // Show playlist again
                        playlist.ShowPlaylist();
                    }
                    break;

                case "3. Search by Artists":
                    Console.WriteLine("You chose to search by artists");
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please try again.");

                    break;






            }
            string restartProgram = AnsiConsole.Prompt(
      new SelectionPrompt<string>()
      .Title("[green]Return to main menu?[/]")
      .AddChoices("Yes" , "No"));

            if (restartProgram == "No")
            {
                Console.WriteLine("Exiting program. Goodbye!");
                break;
            } else if (restartProgram == "Yes")
            {
                AnsiConsole.Clear();
                continue;
            }


        }




    }
}
