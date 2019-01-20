using System;
using System.Collections.Generic;
using TypeDB.Dumping;

namespace TypeDB.Interfaces
{
    public interface IDatabase : IBackupable
    {
        Type GetType(string key);
        List<Object> GetAll();
        Meta GetMeta(string key);

        bool Exist(string key);
        string Print(string key);
        string PrintAll();

        void Set<T>(string key, T value, bool createIfNotExist = true);
        //T Get<T>(Guid guid);
        T Get<T>(string key);

        int Update<T>(string key, T value);

        void Increment<T>(string key, T value);
        void Decrement<T>(string key, T value);

        void Expire(string key, TimeSpan ttl);
        void Lock(string key);
        void Unlock(string key);

        void Drop(string key);
        void Drop(Predicate<Object> keys);
    }
}