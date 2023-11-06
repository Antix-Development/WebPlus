using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPlus
{
    internal class FileDialogOptions
    {
        public string title { get; set; }
        public string filter { get; set; }
        public bool multiSelect { get; set; }

        public FileDialogOptions(string title, string filter, bool multiSelect)
        {
            this.title = title;
            this.filter = filter;
            this.multiSelect = multiSelect;
        }
    }
}
