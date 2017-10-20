using System.ComponentModel.DataAnnotations;

namespace Finance.Web.ViewModels
{
    public class MessageViewModel
    {
        [Required]
        [Display(Name="消息内容")]
        public string Content { get; set; }
    }
}