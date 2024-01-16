namespace Fanex.Athena.ExtService.Core.Models
{
    public class BrowserInfo
    {
        public BrowserInfo(bool isPhone, bool isTablet)
        {
            IsPhone = isPhone;
            IsTablet = isTablet;
        }

        public bool IsMobile => IsPhone || IsTablet;

        public bool IsPhone { get; }

        public bool IsTablet { get; }
    }
}