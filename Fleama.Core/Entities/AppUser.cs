namespace Fleama.Core.Entities
{
    public class AppUser : BaseEntity
    {        
        public string Name { get; set; }
        public string Surname { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public Guid? UserGuid { get; set; } = Guid.NewGuid();          
    }
}