namespace CarStockDAL.Data
{
    public abstract class PostgreBaseRepository 
    {
        protected readonly AppDbContext _dbContext;
        
        protected PostgreBaseRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync(); 
        }
    }
}
