namespace Entities.General
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class AllowedExtensions
    {
        public List<string> ImageExtensions { get; private set; }

        public AllowedExtensions()
        {
            ImageExtensions = new List<string>() { "jpeg", "bmp", "png", "gif", "jpg" };
        }
    }
}