namespace TypeDB
{
    public enum PersistenceType
    {
        /// <summary>
        /// No Persistence, configured by default
        /// </summary>
        None,

        /// <summary>
        /// Persistence is executed when Configuration.Persistence.Execute() is invoked
        /// </summary>
        /// See <see cref="Configuration.Persistence.Execute()" />
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