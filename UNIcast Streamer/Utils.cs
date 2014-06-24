using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace UNIcast_Streamer
{
    public static class Utils
    {
        public static void WriteMetaXml(UNIcastMeta meta, string path)
        {
            XElement tagsElement = new XElement("Tags");
            foreach (string tag in meta.Tags)
            {
                tagsElement.Add(new XElement("Tag", tag));
            }

            XElement xml =
               new XElement("UNIcastMeta",
                   new XElement("DateTime", meta.DateTime.ToString("s")),
                   new XElement("Lecturer", meta.Lecturer),
                   new XElement("Subject", meta.Subject),
                   new XElement("Title", meta.Title),
                   new XElement("Description", meta.Description),
                   tagsElement,
                   new XElement("Privacy", meta.Privacy),
                   new XElement("Media",
                       new XElement("FileName", meta.Media.FileName),
                       new XElement("Format", meta.Media.Format),
                       new XElement("Quality", meta.Media.Quality)
                       ));

            Debug.WriteLine(xml);

            try
            {
                xml.Save(path);
            }
            catch (Exception)
            {
                //
            }
        }
    }
}
