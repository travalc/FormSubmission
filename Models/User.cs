using System.ComponentModel.DataAnnotations;

namespace FormSubmission.Models
{
    public class User
    {
        [Required]
        [MinLength(4)]
        public string firstName {get; set;}
        [Required]
        [MinLength(4)]
        public string lastName {get; set;}

        [Required]
        [Range(0, 120)]
        public int age {get; set;}

        [Required]
        [EmailAddress]
        public string email {get; set;}

        [Required]
        [DataType(DataType.Password)]
        [MinLength(8)]
        public string password {get; set;}

    }
}