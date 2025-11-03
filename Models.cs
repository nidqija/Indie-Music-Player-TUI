namespace Models
{
    public class Song
    {
        public int SongId { get; set; }
        public string? songTitle { get; set; }
        public string? songArtist { get; set; }
        public string? songUrl { get; set; }

        public bool hasDownloaded { get; set; }

        public string? downloadfilePath { get; set; }


        public List<Collection> Collections { get; set; } = new();

    }

    public class Collection
    {
        public int CollectionId { get; set; }
        public string? CollectionName { get; set; }
        public List<Song> Songs { get; set; } = new();

    }

    public class PlayHistory
    {
        public int playHistoryId { get; set; }
        public string songTitle { get; set; }

        public string songArtist { get; set; }

        public string songUrl { get; set; }

        public DateTime playedAt { get; set; }
    }


    

   

}

