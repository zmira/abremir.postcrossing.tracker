using System;
using System.IO;
using abremir.postcrossing.engine.Assets;
using abremir.postcrossing.engine.Interfaces;

namespace abremir.postcrossing.engine.Services
{
    public class FileSystemService : IFileSystemService
    {
        private static readonly string PostcrossingTrackerDbFolder = Path.Combine(Path.GetDirectoryName(AppContext.BaseDirectory), PostcrossingTrackerConstants.PostcrossingTrackerDatabaseFolder);

        public void EnsureLocalDataFolder()
        {
            if (!Directory.Exists(PostcrossingTrackerDbFolder))
            {
                Directory.CreateDirectory(PostcrossingTrackerDbFolder);
            }
        }

        private static string GetPathToDbFile(string file) => Path.Combine(PostcrossingTrackerDbFolder, file);

        public Stream GetStreamForDbFile(string file)
        {
            return new FileStream(GetPathToDbFile(file), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
        }
    }
}
