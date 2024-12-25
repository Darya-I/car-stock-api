namespace CarStockMAP.DTO
{
    public class GetUsersDTO
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public List<string> Roles { get; set; }
    }
}
