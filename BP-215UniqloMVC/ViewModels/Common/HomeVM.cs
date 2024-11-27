using BP_215UniqloMVC.ViewModels.Product;
using BP_215UniqloMVC.ViewModels.Slider;

namespace BP_215UniqloMVC.ViewModels.Common
{
    public class HomeVM
    {
        public IEnumerable<SliderItemVM> Sliders { get; set; }
        public IEnumerable<ProductItemVM> Products { get; set; }
    }
}
