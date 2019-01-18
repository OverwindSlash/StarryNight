using System.ComponentModel.DataAnnotations;

namespace StarryNight.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}