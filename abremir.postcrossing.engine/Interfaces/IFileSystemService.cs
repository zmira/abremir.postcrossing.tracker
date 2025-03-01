using System.IO;

namespace abremir.postcrossing.engine.Interfaces
{
    public interface IFileSystemService
    {
        void EnsureLocalDataFolder();
        Stream GetStreamForDbFile(string file);
    }
}
