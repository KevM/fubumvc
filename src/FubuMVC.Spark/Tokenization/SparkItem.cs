﻿using System;
using System.Collections.ObjectModel;
using System.IO;
using FubuCore;

namespace FubuMVC.Spark.Tokenization
{
    public static class Constants
    {
        public const string HostOrigin = "Host";
    }

    public class SparkItem
    {
        public SparkItem(string filePath, string rootPath, string origin)
        {
            FilePath = filePath;
            RootPath = rootPath;
            Origin = origin;
        }

        public string FilePath { get; private set; }
        public string RootPath { get; private set; }
        public string Origin { get; private set; }

        public SparkItem Master { get; set; }
        public Type ViewModelType { get; set; }
        public string Namespace { get; set; }

        public string RelativePath()
        {
            return FilePath.PathRelativeTo(RootPath);
        }

        public string DirectoryPath()
        {
            return Path.GetDirectoryName(FilePath);
        }

        public string RelativeDirectoryPath()
        {
            return DirectoryPath().PathRelativeTo(RootPath);
        }


        public string Name()
        {
            return Path.GetFileNameWithoutExtension(FilePath);
        }

        public bool HasViewModel()
        {
            return ViewModelType != null;
        }

        public override string ToString()
        {
            return FilePath;
        }
    }
    public class SparkItems : Collection<SparkItem> { }
}