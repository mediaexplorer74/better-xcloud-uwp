using System;
using System.Threading.Tasks;
using Microsoft.Xbox.Services;
using Microsoft.Xbox.Services.System;
using Windows.UI.Core;

namespace BetterXCloudUWP.Services
{
    public class XboxAuthService
    {
        public XboxLiveUser User { get; private set; }
        public bool IsSignedIn => User != null && User.IsSignedIn;

        public event Action<string> AuthStatusChanged;

        public async Task<bool> SignInAsync(CoreDispatcher coreDispatcher)
        {
            User = new XboxLiveUser();
            try
            {
                var result = await User.SignInAsync(coreDispatcher);
                AuthStatusChanged?.Invoke(result.Status.ToString());
                return result.Status == SignInStatus.Success;
            }
            catch (Exception ex)
            {
                AuthStatusChanged?.Invoke($"Ошибка авторизации: {ex.Message}");
                return false;
            }
        }
    }
}
