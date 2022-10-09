namespace SD_340_W22SD_Final_Project_Group6.DAL
{
    public interface IRepository<T> where T : class
    {
        // CREATE
        void Add(T entity);

        // READ
        T Get(int id);
        T Get(Func<T, bool> predicate);
        ICollection<T> GetAll();
        ICollection<T> GetList(Func<T, bool> predicate);

        // UPDATE
        T Update(T entity);

        // DELETE
        void Delete(T entity);

        void Save();
    }
}
