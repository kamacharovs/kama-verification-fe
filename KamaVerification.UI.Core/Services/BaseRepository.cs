namespace KamaVerification.UI.Core.Services
{
    public abstract class BaseRepository
    {
        public BaseRepository()
        {
        }

        public async Task<T> PostAsync<T, TDto>(TDto dto)
            where T : class
            where TDto : class
        {
            return null;
        }
    }
}
