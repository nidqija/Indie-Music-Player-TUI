
namespace MusicPlayer
{
     class SongProcess
    {
        private string basePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic), "MusicConsole");

        public string GetLocalPath(Songs song)
        {
            
            string safeTitle = string.Join("_", song.Name.Split(Path.GetInvalidFileNameChars()));
            string safeArtist = string.Join("_", song.Artist.Split(Path.GetInvalidFileNameChars()));
            return Path.Combine(basePath, $"{safeArtist}-{safeTitle}.mp3");
        }

        public bool Exists(Songs song)
        {
            return File.Exists(GetLocalPath(song));
        }
    }
}
