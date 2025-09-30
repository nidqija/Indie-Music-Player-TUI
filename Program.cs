
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


        if (string.IsNullOrEmpty(client))
        {
            Console.WriteLine("JAMENDO_CLIENT_ID is not set in the environment variables.");
        }
        else
        {
            Console.WriteLine($"Your jamendo client id is: {client}");

        }
    }

   
}





// main class

class MusicInput
{
   
    string intro = "Welcome to MusicInput Station!";
    string input;

    static void Main(string[] args)
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
            Console.WriteLine("You chose to search songs.");
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









