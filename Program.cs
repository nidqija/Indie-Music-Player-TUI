
// this is a music input program
using System;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Text.Json;
using System.Threading.Tasks;
using DotNetEnv;


// class to display menu choices
class MusicChoices
{
    private string[] menuChoices = { "1. Search Songs", "2. Playlists", "3. Search by Artists" };
    private string choice;
    
    
   public void printChoices()
    {
        for (int i = 0; i < menuChoices.Length; i++)
        {
            Console.WriteLine(menuChoices[i]);
        }
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

    public void setSongName(string song)
    {
        songName = song;
    }

    public string getSongName()
    {
        return songName;
    }

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

        Console.WriteLine("Top 5 results: ");
        foreach (JsonElement track in root.EnumerateArray())
        {   
            string name = track.GetProperty("name").GetString();
            string artist = track.GetProperty("artist_name").GetString();
            Console.WriteLine("================================");
            Console.WriteLine($"{ name}" + " by: " + $"{ artist}");

        } if (root.GetArrayLength() == 0)
        {
            Console.WriteLine("No results found for the song: " + getSongName());
        }





    }

   




}

// class to handle environment variables
class EnvFile
{
    public static void doEnvOps(string[] args)
    {
        // telling the env variable where is the exact path of the .env file
        // do this if the .env file is not detected automatically

        Env.Load(@"C:\Users\User\source\repos\MusicPlayer\MusicPlayer\.env");

        string client = Environment.GetEnvironmentVariable("JAMENDO_CLIENT_ID");


        
    }

   
}





// main class

class MusicInput
{
   
    string intro = "Welcome to MusicInput Station!";
    string input;

    static async Task Main(string[] args)
    {
        MusicInput musicInput = new MusicInput();
        musicInput.input = "Hello from MusicInput";
        Console.WriteLine(musicInput.intro);
        Console.WriteLine(musicInput.input);

        // display choices
        MusicChoices musicChoices = new MusicChoices();
        musicChoices.printChoices();

        // handle env file operations
        EnvFile.doEnvOps(args);

        musicChoices.getChoice();
        
        if(musicChoices.returnChoice() == "1")
        {
            Console.WriteLine("Enter your song name: ");
            string songInput = Console.ReadLine();
            Songs songs = new Songs();
            songs.setSongName(songInput);
            Console.WriteLine("Submitted! Wait for a while...");
            await songs.searchSong();

        }
        else if (musicChoices.returnChoice() == "2")
        {
            Console.WriteLine("You chose to view playlists");
        }
        else if (musicChoices.returnChoice() == "3")
        {
            Console.WriteLine("You chose to search by artists");
        }

        else
        {
            Console.WriteLine("Invalid choice. Please try again.");
        }


       



    }
}









