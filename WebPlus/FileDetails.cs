using System.IO;
using System.Runtime.InteropServices;

namespace WebPlus
{
    //[ClassInterface(ClassInterfaceType.AutoDual)]
    [ComVisible(true)]
    public class FileDetails
    {
        public string name { get; set; }
        public string extension { get; set; }
        public string path { get; set; }
        public string fullPath { get; set; }
        public string type { get; set; }
        public long size { get; set; }

        public FileDetails(string _path)
        {
            fullPath = _path;
            name = Path.GetFileName(_path);
            extension = Path.GetExtension(_path);
            path = Path.GetDirectoryName(_path);

            if (File.GetAttributes(_path).HasFlag(FileAttributes.Directory) )
            {
                type = "DIRECTORY";
            }
            else
            {
                type = "FILE";
                size = new FileInfo(_path).Length;
            }
        }

        public override string ToString()
        {
            return $"name:{name}, extension:{extension}, type:{type}, path:{path}, fullPath:{fullPath}, size:{size:N0}";
        }
    }
}
