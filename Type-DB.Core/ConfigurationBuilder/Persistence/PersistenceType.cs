namespace TypeDB
{
    public enum PersistenceType
    {
        /// <summary>
        /// No Persistence, configured by default
        /// </summary>
        None,

        /// <summary>
        /// Persistence is executed when TypeDB.Persistence.Invoke() is invoked
        /// </summary>
        /// See <see cref="TypeDB.Persistence.Invoke()" />
        OnlyInvoke,

        /// <summary>
        /// Persistence is executed every n interval
        /// See <see cref="TypeDB.Persistence.Interval"/> to specify the interval
        /// </summary>
        Snapshot,

        /// <summary>
        /// Persistence is executed every each operations
        /// </summary>
        Iteration
    }
}