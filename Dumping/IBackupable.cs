using TypeDB.Interfaces;

namespace TypeDB.Dumping
{
    public interface IBackupable
    {
        string Save();
        void SaveAs(string filename);

        void Restore();
        void RestoreFromFile(string filename);
    }
}