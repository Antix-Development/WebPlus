﻿using System;
using System.IO;
using System.Runtime.InteropServices;

namespace WebPlus
{
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

            try
            {
                if (File.GetAttributes(_path).HasFlag(FileAttributes.Directory))
                {
                    type = "DIRECTORY";
                }
                else
                {
                    type = "FILE";
                    size = new FileInfo(_path).Length;
                }

            }
            catch (Exception)
            {
                // Cover corner case where `saveFileDialog` has been called and a non existant filename has been entered, 
                // as is the case when the user enters a name for a new file that they want to create.

                type = "UNKNOWN";
                size = 0;
            }
        }

        public override string ToString()
        {
            return $"name:{name}, extension:{extension.ToLower()}, type:{type}, path:{path}, fullPath:{fullPath}, size:{size:N0}";
        }
    }
}
