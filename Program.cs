
// this is a music input program
using System;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Text.Json;
using System.Threading.Tasks;
using DotNetEnv;
using Spectre.Console;
using NAudio;
using System.Net;

// class to display menu choices
class MusicChoices
{
    private string[] menuChoices = { "1. Search Songsss", "2. Playlists", "3. Search by Artists" };
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


    public void getChoice()
    {
        Console.WriteLine("Enter your choice: ");
        choice = Console.ReadLine();
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

        string  response = await httpClient.GetStringAsync(url);

        using JsonDocument doc = JsonDocument.Parse(response);

        JsonElement root = doc.RootElement.GetProperty("results");


        if (root.GetArrayLength() == 0)
        {
            Console.WriteLine("No results found for the song: " + getSongName());
        }

        else
        {
            Console.WriteLine("Top 5 results: ");
            int index = 1;
            List<Songs> songlists = new List<Songs>();

            foreach (JsonElement track in root.EnumerateArray())
            {
                string name = track.GetProperty("name").GetString();
                string artist = track.GetProperty("artist_name").GetString();
                string audioUrl = track.GetProperty("audio").GetString(); 

                Console.WriteLine("================================");
                Console.WriteLine($"{name}" + " by: " + $"{artist}");
                songlists.Add(new Songs { Name = name, Artist = artist , Url=audioUrl});
                index++;






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

class PlaySongs
{

    private List<Songs> songs;
    
    public PlaySongs(List<Songs> songLists)
    {
        songs = songLists;
    }

    public void playSong(Songs song)
    {
        using (var webClient = new WebClient())
        {`
            string tempFile = Path.GetTempFileName() + ".mp3";
            webClient.DownloadFile(song.Url, tempFile);

            using var audioFile = new NAudio.Wave.AudioFileReader(tempFile);
            using var outputDevice = new NAudio.Wave.WaveOutEvent();
            outputDevice.Init(audioFile);
            outputDevice.Play();

            Console.WriteLine("Now playing: " + song.Name + " by " + song.Artist);
            Console.WriteLine("Press any key to stop playback...");
            Console.ReadLine();
            outputDevice.Stop();

            File.Delete(tempFile);
            
        }
    }

    


}

class EnvFile
{
    public static void doEnvOps(string[] args)
    {
 // ================= telling the env variable where is the exact path of the .env file =======================//
 // do this if the .env file is not detected automatically

        Env.Load(@"C:\Users\User\source\repos\MusicPlayer\MusicPlayer\.env");

        string client = Environment.GetEnvironmentVariable("JAMENDO_CLIENT_ID");


        
    }

   
}





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

        AnsiConsole.Write(
            new FigletText(font,"MusicInput Station!")
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
                Console.WriteLine("Enter your song name: ");
                string songInput = Console.ReadLine();
                Songs songs = new Songs();
                songs.setSongName(songInput);
                Console.WriteLine("Submitted! Wait for a while...");
                await songs.searchSong();

                PlaySongs playsong = new PlaySongs(null);
                playsong.playSong(songs.returnPlaySongs());


                    break;

            case "2. Playlists":
                Console.WriteLine("You chose to view playlists");
                break;

            case "3. Search by Artists":
                Console.WriteLine("You chose to search by artists");
                break;

            default:
                Console.WriteLine("Invalid choice. Please try again.");
                break;



        }




    }

    
}









