namespace Web.DotW.API.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;

    public class ImageHelpers
    {
        public static Stream Base64ToStream(string base64String, string path)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);

            Stream stream = new MemoryStream(imageBytes);

            return stream;
        }
    }
}