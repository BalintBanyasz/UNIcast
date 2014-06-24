using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace UNIcast_Streamer
{
    public class UNIcastMeta
    {
        public DateTime DateTime { get; set; }

        public string Lecturer { get; set; }

        public string Subject { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public List<String> Tags { get; set; }

        public Privacy Privacy { get; set; }

        public Media Media { get; set; }
    }

    public class Media
    {
        public Media(string fileName, string format, int quality)
        {
            this.FileName = fileName;
            this.Format = format;
            this.Quality = quality;
        }

        public string FileName { get; set; }

        public string Format { get; set; }

        public int Quality { get; set; }
    }

    public enum Privacy
    {
        Public,
        Unlisted,
        Private
    }
}
