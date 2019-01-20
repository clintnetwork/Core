using TypeDB.Interfaces;

namespace TypeDB.Dumping
{
    public interface IBackupable
    {
        string Dump();
        void Restore();

        void DumpToFile(string filename);
        void RestoreFromFile(string filename);
    }
}