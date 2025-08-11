namespace MyBackend.Models
{
    public class FormModel
    {
        
         public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName  { get; set; } = string.Empty;
        public string Email     { get; set; } = string.Empty;
        public string Phone     { get; set; } = string.Empty;
        public string Address   { get; set; } = string.Empty;
        public string City      { get; set; } = string.Empty;
        public string State     { get; set; } = string.Empty;
        public string Zip       { get; set; } = string.Empty;

        public DateTime? Dob { get; set; }

        public string Gender  { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
