
// this is a music input program



class MusicChoices
{
    string[] menuChoices = { "1. Search Songs", "2. Playlists", "3. Search by Artists" };
    
    
   public void getChoices()
    {
        for (int i = 0; i < menuChoices.Length; i++)
        {
            Console.WriteLine(menuChoices[i]);
        }
    }
    
}
class MusicInput
{
    string input = "";
    string intro = "Welcome to MusicInput Station!";


    static void Main(string[] args)
    {
        MusicInput musicInput = new MusicInput();
        musicInput.input = "Hello from MusicInput";
        Console.WriteLine(musicInput.intro);
        Console.WriteLine(musicInput.input);

        // display choices
        MusicChoices musicChoices = new MusicChoices();
        musicChoices.getChoices();


    }
}
