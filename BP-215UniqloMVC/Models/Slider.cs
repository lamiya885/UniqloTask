using System.ComponentModel.DataAnnotations;
using BP_215UniqloMVC.ViewModels.Slider;

namespace BP_215UniqloMVC.Models
{
    public class Slider:BaseEntity
    {
        public string Title { get; set; } = null!;
        public string Subtitle { get; set; } = null!;
        public string? Link { get; set; }
        public string ImageUrl { get; set; } = null!;

        public static implicit operator Slider(SliderCreateVM vm)
        {
            return new Slider
            {
                Title = vm.Title,
                Subtitle = vm.Subtitle,
                Link = vm.Link,
           };

        }
    }
}
