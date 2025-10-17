namespace Models
{
    public class Song
    {
        public int SongId { get; set; }
        public string songTitle { get; set; }
        public string songArtist { get; set; }
        public string songUrl { get; set; }

        public int? CollectionId { get; set; }
        public Collection Collection { get; set; }
    }

    public class Collection
    {
        public int CollectionId { get; set; }
        public string CollectionName { get; set; }
        public List<Song> Songs { get; set; } = new();
    }

   

}

