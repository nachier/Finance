using System.ComponentModel.DataAnnotations;

namespace FinanceSystem.Models
{
    public class MessageViewModel
    {
        [Required]
        [Display(Name="消息内容")]
        public string Content { get; set; }
    }
}