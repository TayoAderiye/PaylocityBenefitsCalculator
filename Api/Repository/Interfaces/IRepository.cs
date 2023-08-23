namespace Api.Repository.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<List<T>> GetAllAysnc();
        Task<T> AddAysnc(T entity);
        Task<T> UpdateAysnc(T entity);
        void DeleteAysnc(T entity);
        IQueryable<T> Query();
    }
}