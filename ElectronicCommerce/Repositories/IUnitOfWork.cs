namespace ElectronicCommerce.Repositories
{
    public interface IUnitOfWork
    {
        IBaseRepository<T> GenericResponsitories<T>() where T : class;
        void Save();
    }
}
