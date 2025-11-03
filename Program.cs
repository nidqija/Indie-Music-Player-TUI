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
using Data;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Marshalling;
using Microsoft.EntityFrameworkCore.Storage.Json;
using MusicPlayer;



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

        var app = new AppDbContext();
        var fetchPlaylist = new List<Collection>();
        string[] playlistMenu = { "1. Create New Collection", "2. View Existing Collections", "3. Return to Main Menu" };

        string playlistChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .AddChoices(playlistMenu));

        switch (playlistChoice)
        {
            case "1. Create New Collection":
                Console.WriteLine("Enter your Collection name");
                string collectionName = Console.ReadLine();


                // ============================= checks empty collection name ( input ) ==============================//

                if (string.IsNullOrEmpty(collectionName))
                {
                    Console.WriteLine("Collection name cannot be empty. Please try again.");
                    return;
                }


                else
                // ============================== connect to database and create new collection =============================//
                {
                    var newCollection = new Collection
                    {
                        CollectionName = collectionName
                    };

                    using (var db = new AppDbContext())
                    {
                        db.Collections.Add(newCollection);
                        db.SaveChanges();

                        Console.WriteLine($"Collection {newCollection.CollectionName} created successfully!");
                        Console.WriteLine("\n");


                    }
                    Console.WriteLine("Your Collections: ");
                    Console.WriteLine("========================= | ===========================");

                    var allCollections = app.Collections.ToList();

                    foreach (var db in allCollections)
                    {
                        Console.WriteLine($"{db.CollectionId} | {db.CollectionName}");
                    }


                    break;
                }

            case "2. View Existing Collections":


                var collections = app.Collections
                    .Include(c => c.Songs)
                    .ToList();

                var table = new Table();

                table.AddColumn(new TableColumn("Collection ID").Centered());
                table.AddColumn(new TableColumn("Collection Name").Centered());
                table.AddColumn(new TableColumn("Number of Songs").Centered());

                foreach (var db in app.Collections)
                {
                    var songCounts = db.Songs.Count();

                    table.AddRow(new Text(db.CollectionId.ToString()), new Text(db.CollectionName), new Text(songCounts.ToString()));

                }

                AnsiConsole.Write(table);



                string viewCollection = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("[yellow] Select a collection to view or add songs:[/]")
                    .AddChoices(collections.Select(c => c.CollectionName).ToArray())
                    );

                var selectedCollection = collections.FirstOrDefault(c => c.CollectionName == viewCollection);

                if (selectedCollection != null)
                {
                    if (selectedCollection.Songs.Count == 0)
                    {
                        Console.WriteLine("No songs in this collection yet. ");
                        Console.WriteLine("Do you want to add songs to this collection?");
                    }
                    else
                    {
                        AnsiConsole.Write(new Markup($"[underline yellow] {selectedCollection.CollectionName}[/]"));
                        Console.WriteLine("\n");

                        foreach (var song in selectedCollection.Songs)
                        {
                            Console.WriteLine(song.SongId + " - " + song.songTitle + " by " + song.songArtist);
                        }

                        var songChoicefromCollection = selectedCollection.Songs
                            .Select(s => $"{s.songTitle} by {s.songArtist}").ToList();

                        string[] songplayFromCollection = { "1. Choose a song to play ", "2. Play all songs in collection", "3. Delete songs" , "4. Return to Main Menu" };

                        string playsongsfromCollection = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                            .Title("[yellow] Select your collection options:[/]")
                            .AddChoices(songplayFromCollection));


                        if (playsongsfromCollection == "1. Choose a song to play ")
                        {
                            string chooseASongPlay = AnsiConsole.Prompt(
                                new SelectionPrompt<string>()
                                .Title("[yellow]Select a song to play:[/]")
                                .AddChoices(songChoicefromCollection));


                            PlaySongs player = new PlaySongs();


                            // play the selected song from collection
                            // this code splits the selected string to get song title and artist
                            // the url is fetched from the database based on the song artist and title

                            player.playSong(new Songs
                            {
                                Name = chooseASongPlay.Split(" by ")[0],
                                Artist = chooseASongPlay.Split(" by ")[1],
                                Url = selectedCollection.Songs.First(s => s.songTitle == chooseASongPlay.Split(" by ")[0]
                                && s.songArtist == chooseASongPlay.Split(" by ")[1]).songUrl,
                            });



                        }
                        else if (playsongsfromCollection == "2. Play all songs in collection")
                        {
                            PlaySongs player = new PlaySongs();
                            for (var i = 0; i < selectedCollection.Songs.Count; i++)
                            {
                                Console.WriteLine($"Playing {selectedCollection.Songs[i].songTitle} by {selectedCollection.Songs[i].songArtist}");
                                player.playSong(new Songs
                                {
                                    Name = selectedCollection.Songs[i].songTitle,
                                    Artist = selectedCollection.Songs[i].songArtist,
                                    Url = selectedCollection.Songs[i].songUrl
                                });

                                if (i + 1 >= selectedCollection.Songs.Count)
                                {
                                    Console.WriteLine("End of collection reached.");
                                    break;
                                }

                                string continueChoice = AnsiConsole.Prompt(
                                    new SelectionPrompt<string>()
                                    .Title("[green]Do you want to skip this song?[/]")
                                    .AddChoices("Yes", "No")
                                    );

                                if (continueChoice == "Yes")
                                {
                                    if (i + 1 < selectedCollection.Songs.Count)
                                    {
                                        Console.WriteLine("Skipping to the next song...");
                                        continue;

                                    }
                                }

                            }
                        }

                        else if (playsongsfromCollection == "3. Delete songs")
                        {
                            string songToDelete = AnsiConsole.Prompt(
                                new SelectionPrompt<string>()
                                .Title("[yellow]Select a song to delete from this collection:[/]")
                                .AddChoices(songChoicefromCollection)
                                );




                            using (var db = new AppDbContext())
                            {

                                // load collection with songs
                                var collectionsWithSongs = db.Collections.Include(c => c.Songs).FirstOrDefault(c => c.CollectionId == selectedCollection.CollectionId);

                                // find the song to delete
                                var deleteSong = collectionsWithSongs.Songs.FirstOrDefault(s => s.songTitle == songToDelete.Split(" by ")[0]);

                                if (deleteSong != null)
                                {
                                    // remove the song from the collection
                                    collectionsWithSongs.Songs.Remove(deleteSong);

                                    // save changes to database
                                    db.SaveChanges();
                                    Console.WriteLine($"{deleteSong.songTitle} has been removed from {selectedCollection.CollectionName}.");
                                    break;
                                }

                                else
                                {
                                    Console.WriteLine("Song not found in the collection!.");
                                    return;
                                }
                            }



                        }
                        else if (playsongsfromCollection == "4. Return to Main Menu")
                        {
                            return;
                        }

                       




                    }

                }
                else
                {
                    Console.WriteLine("Collection not found.");
                    return;
                }

                break;


            case "3. Return to Main Menu":
                return;



            default:
                Console.WriteLine("Invalid Choice! Enter a valid choice ");
                break;


        }



    }




    // Add song into playlist
    public void AddSong(Songs song)
    {
        playlistSongs.Add(song);
        Console.WriteLine($" Added {song.Name} by {song.Artist} to your playlist!");
        Console.WriteLine($"Your playlist now has {playlistSongs.Count} songs");
    }

    public async Task InsertSongfromArtistSearch(string artistName)
    {
        Env.Load(@"C:\Users\User\source\repos\MusicPlayer\MusicPlayer\.env");
        string client = Environment.GetEnvironmentVariable("JAMENDO_CLIENT_ID");
        using var httpClient = new HttpClient();
        string url = $"https://api.jamendo.com/v3.0/tracks/?client_id={client}&format=json&limit=10&artist_name={artistName}";
        string response = await httpClient.GetStringAsync(url);

        using JsonDocument doc = JsonDocument.Parse(response);
        JsonElement root = doc.RootElement.GetProperty("results");

        if (root.GetArrayLength() == 0)
        {
            Console.WriteLine($"No songs found for {artistName}");
            return;
        }

        List<Songs> artistResult = new List<Songs>();

        foreach (JsonElement track in root.EnumerateArray())
        {
            artistResult.Add(new Songs
            {
                Name = track.GetProperty("name").GetString(),
                Artist = track.GetProperty("artist_name").GetString(),
                Url = track.GetProperty("audio").GetString()


            });
        }

        Songs chosenArtistSongs = AnsiConsole.Prompt(
            new SelectionPrompt<Songs>()
            .Title("[yellow]Choose a song to add from " + artistName + ":[/]")
            .UseConverter(s => $"{s.Name} by {s.Artist}")
            .AddChoices(artistResult));


        PlaySongs player = new PlaySongs();

        player.playSong(chosenArtistSongs);









    }
    // Insert via search (Jamendo + user pick)
    public async Task InsertSongFromSearch(string searchQuery)
    {
        Env.Load(@"C:\Users\User\source\repos\MusicPlayer\MusicPlayer\.env");
        string client = Environment.GetEnvironmentVariable("JAMENDO_CLIENT_ID");

        using var httpClient = new HttpClient();
        string url = $"https://api.jamendo.com/v3.0/tracks/?client_id={client}&format=json&limit=10&search={searchQuery}";
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
    private List<string> songSettings = new List<string> {
        "1. Save Song" ,
        "2. Pause" ,
        "3. Resume" ,
        "4. Download song" ,
        "5. Return to Main Menu"
    };


    public async Task playSong(Songs song)
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
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine($"[green] >> Now Playing:[/] [bold yellow]{song.Name}[/]");
            AnsiConsole.MarkupLine($"[green] >> Artist: [/] [bold yellow]{song.Artist}[/] ");

           

            var app = new AppDbContext();


            /*   var totalTime = audioFile.TotalTime;

             // Start progress bar in a background task
           var progressTask = Task.Run(async () => {
                 while (outputDevice.PlaybackState == NAudio.Wave.PlaybackState.Playing)
                 {
                     AnsiConsole.Clear();
                     var currentTime = audioFile.CurrentTime;
                     double progress = currentTime.TotalSeconds / totalTime.TotalSeconds;
                     int barLength = 20;
                     int filled = (int)(progress * barLength);
                     string bar = new string('█', filled) + new string('-', barLength - filled);
                     Console.SetCursorPosition(0, Console.CursorTop);
                     Console.WriteLine($"▶️ {song.Name} [{bar}] {progress * 100:F0}% ({currentTime:mm\\:ss}/{totalTime:mm\\:ss})");

                     Thread.Sleep(1000);
                 }
             }); */

           







            if (app.Songs.Any(s => s.songTitle == song.Name && s.songArtist == song.Artist))
            {

                var matchedSong = app.Songs.FirstOrDefault(s =>
            s.songTitle == song.Name && s.songArtist == song.Artist);

                if (matchedSong != null)
                {
                    app.PlayHistories.Add(new PlayHistory
                    {
                        SongId = matchedSong.SongId,
                        playedAt = DateTime.Now
                    });
                    app.SaveChanges();
                    Console.WriteLine($"✅ Play history added for {song.Name}");
                }
                else
                {
                    Console.WriteLine("⚠️ No matching song found in database. Skipping history log.");
                }

                while (true)
                {

                    // Clear current console line and reprint progress

                   


                    Console.WriteLine("\n");
                    string settingChoice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("[yellow]Settings:[/]")
                    .AddChoices("1. Add to Playlist", "2. Delete from savelist", "3. Pause", "4. Resume", "5. Download song", "6. Return to Main Menu"));

                    if (settingChoice == "1. Add to Playlist")
                    {
                        var playlists = app.Collections.Include(c=> c.Songs).ToList();

                        Console.WriteLine("Select a playlist to save your song!");
                        string playlistname = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                            .AddChoices(playlists.Select(p => p.CollectionName)));


                        // matching the playlist name to collection name from db //
                        var selectedPlaylist = playlists.FirstOrDefault(p => p.CollectionName == playlistname);

                        // matching the song displayed and the artist to the data from db //
                        var currentSong = app.Songs.FirstOrDefault(s => s.songTitle == song.Name && s.songArtist == song.Artist);

                        //  checks if the playlist and current song is empty before
                        if (selectedPlaylist != null && currentSong != null)
                        {
                            if (!selectedPlaylist.Songs.Any(s => s.SongId == currentSong.SongId))
                            {
                                selectedPlaylist.Songs.Add(currentSong);

                                app.SaveChanges();
                                AnsiConsole.MarkupLine($"[green]✅ '{currentSong.songTitle}' added to '{selectedPlaylist.CollectionName}'![/]");
                                var playlistSongs = app.Collections
                                    .Include(c => c.Songs)
                                    .FirstOrDefault(c => c.CollectionId == selectedPlaylist.CollectionId);

                                Console.WriteLine($"{playlistname}");
                                foreach (var s in playlistSongs.Songs)
                                {
                                    Console.WriteLine($"{s.SongId}- {s.songTitle} by {s.songArtist}");

                                }

                            }
                            else
                            {
                                AnsiConsole.MarkupLine($"[yellow]⚠️ Song already exists in '{selectedPlaylist.CollectionName}'.[/]");
                            }
                        }





                    }
                    else if (settingChoice == "2. Delete from savelist")
                    {
                        var songToDelete = app.Songs.FirstOrDefault(s => s.songTitle == song.Name && s.songArtist == song.Artist && s.songUrl == song.Url);
                        if (songToDelete != null)
                        {
                            app.Songs.Remove(songToDelete);
                            app.SaveChanges();
                            Console.WriteLine($"{songToDelete.songTitle} is deleted from Saved List!");

                        }


                    }
                    else if (settingChoice == "3. Pause")
                    {
                        outputDevice.Pause();
                        Console.WriteLine("Song is paused. Press any key to resume...");
                    }

                    else if (settingChoice == "4. Resume")
                    {
                        outputDevice.Play();
                        Console.WriteLine("Song is resumed. Press any key to pause...");
                    }

                    else if (settingChoice == "5. Download song")
                    {
                        DownloadManager download = new DownloadManager();
                        download.DownloadSong(song).Wait();

                    }

                    else if (settingChoice == "6. Return to Main Menu")
                    {
                        Console.WriteLine("Returning to main menu...");
                        return;
                    }

                    Console.WriteLine("Press 'S'  open settings or choose again...");
                    if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                    {
                        break;
                    }
                }
            }
            else
            {
                while (true)
                {
                    string settingChoice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("[yellow]Settings:[/]")
                    .AddChoices(songSettings));

                    if (settingChoice == "1. Save Song")
                    {
                        Console.WriteLine("Saving song to database...");

                        var newSong = new Song
                        {
                            songTitle = song.Name,
                            songArtist = song.Artist,
                            songUrl = song.Url

                        };




                        using (var db = new AppDbContext())

                        {
                            db.Songs.Add(newSong);
                            db.SaveChanges();

                        }

                        Console.WriteLine($" Song '{newSong.songTitle}' added to the database!");
                        Console.WriteLine("\n");
                        Console.WriteLine("Your saved songs: ");
                        foreach (var s in app.Songs)
                        {
                            Console.WriteLine($"- {s.songTitle} by {s.songArtist}");
                        }


                    }
                    else if (settingChoice == "2. Pause")
                    {
                        outputDevice.Pause();
                        Console.WriteLine("Song is paused. Press any key to resume...");
                    }
                    else if (settingChoice == "3. Resume")
                    {
                        outputDevice.Play();
                        Console.WriteLine("Song is resumed. Press any key to pause...");
                    }
                    else if (settingChoice == "4. Download song")
                    {
                        DownloadManager download = new DownloadManager();
                        download.DownloadSong(song).Wait();
                    }
                    else if (settingChoice == "5. Return to Main Menu")
                    {
                        Console.WriteLine("Returning to main menu...");
                        return;
                    }

                    Console.WriteLine("Press 'S'  open settings or choose again...");
                    if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                    {
                        break;
                    }


                }

            }


            outputDevice.Stop();
            File.Delete(tempFile);
        }
    }
}


class DownloadManager
{
    private readonly SongProcess songProcess = new SongProcess();

    public async Task DownloadSong(Songs song)
    {
        if (song == null || string.IsNullOrEmpty(song.Url))
        {
            Console.WriteLine("No songs selected or URL is invalid.");
            return;
        }
        else
        {
            string filePath = songProcess.GetLocalPath(song);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            Console.WriteLine($"Downloading {song.Name} by {song.Artist}");

            using var httpClient = new HttpClient();
            using var response = await httpClient.GetAsync(song.Url, HttpCompletionOption.ResponseHeadersRead);

            response.EnsureSuccessStatusCode();

            var totalBytes = response.Content.Headers.ContentLength ?? -1L;
            var canReportProgress = totalBytes != -1;

            using var contentStream = await response.Content.ReadAsStreamAsync();
            using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true);
            await contentStream.CopyToAsync(fileStream);
            fileStream.Close();

            Console.WriteLine($"Download completed! File saved to: {filePath}");

            using (var db = new AppDbContext())
            {
                var existing = db.Songs.FirstOrDefault(s => s.songTitle == song.Name && s.songArtist == song.Artist);

                if (existing != null)
                {
                    existing.hasDownloaded = true;
                    existing.downloadfilePath = filePath;
                    db.SaveChanges();
                    Console.WriteLine($"Song '{existing.songTitle}' download status updated in database.");
                }

            }



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

    static async Task Main(string[] args)

    {


        //====================== using Spectre.Console to display title ===========================//
        var fontPath = Path.Combine("fonts", "alligator2.flf");
        var font = FigletFont.Load(fontPath);
        string[] songSearchChoice = { "1. Browse", "2. Search from saved list", "3. Return to Main Menu" };



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
                    string songsInput = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                        .Title("[green] Select an option to search songs[/]")
                        .AddChoices(songSearchChoice));

                    if (songsInput == "1. Browse")
                    {
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
                    }
                    else if (songsInput == "2. Search from saved list")
                    {
                        AnsiConsole.Clear();
                        AnsiConsole.Write(new FigletText(font, "Search Songs")
                            .Centered().Color(Color.Yellow));

                        // fetch songs from database

                        var fetchSong = new List<Song>();

                        using (var db = new AppDbContext())
                        {

                            // fetched song list from database
                            fetchSong = db.Songs.ToList();

                            Console.WriteLine("Songs in your saved list: ");
                            Console.WriteLine("\n");

                            // translate the list of songs from db to array to display as choice //
                            string[] songList = fetchSong.Select(s =>
                            $"{s.songTitle} by {s.songArtist}")
                                .ToArray();

                            // display fetched songs as choice selection 

                            string dbSongChoice = AnsiConsole.Prompt(
                                new SelectionPrompt<string>()
                                .Title("[green]Select a song to play from your saved list:[/]")
                                .AddChoices(songList));

                            PlaySongs player = new PlaySongs();

                            player.playSong(new Songs
                            {
                                Name = dbSongChoice.Split(" by ")[0],
                                Artist = dbSongChoice.Split(" by ")[1],
                                Url = fetchSong.First(s => s.songTitle == dbSongChoice.Split(" by ")[0]
                                && s.songArtist == dbSongChoice.Split(" by ")[1]).songUrl

                            });

                            Console.WriteLine("\n");

                        }


                        break;

                    }
                    else if (songsInput == "3. Return to Main Menu")
                    {
                        break;
                    }
                    break;



                case "2. Playlists":
                    AnsiConsole.Clear();
                    AnsiConsole.Write(new FigletText(font, "Playlist")
                     .Centered().Color(Color.Purple));
                    Console.WriteLine("\n");
                    Playlist playlist = new Playlist();

                    // Show playlist
                    playlist.ShowPlaylist();


                    break;

                case "3. Search by Artists":
                    AnsiConsole.Clear();
                    AnsiConsole.Write(new FigletText(font, "Search by Artists")
                   .Centered().Color(Color.Blue1));

                    Console.WriteLine("\n");
                    while (true)
                    {
                        AnsiConsole.WriteLine("Enter artist name to search: ");
                        string artistInput = Console.ReadLine();
                        Playlist artistplay = new Playlist();
                        await artistplay.InsertSongfromArtistSearch(artistInput);
                        string continueChoice = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                            .Title("[green]Do you want to search another artist?[/]")
                            .AddChoices("Yes", "No"));

                        if (continueChoice == "No")
                        {
                            break;
                        }
                        else if (continueChoice == "Yes")
                        {
                            AnsiConsole.Clear();
                            continue;
                        }
                    }


                    break;


                default:
                    Console.WriteLine("Invalid choice. Please try again.");

                    break;

            }

            // restart or exit program //
            string restartProgram = AnsiConsole.Prompt(
      new SelectionPrompt<string>()
      .Title("[green]Return to main menu?[/]")
      .AddChoices("Yes", "No"));

            if (restartProgram == "No")
            {
                Console.WriteLine("Exiting program. Goodbye!");
                break;
            }
            else if (restartProgram == "Yes")
            {
                AnsiConsole.Clear();
                continue;
            }


        }

    }
}
