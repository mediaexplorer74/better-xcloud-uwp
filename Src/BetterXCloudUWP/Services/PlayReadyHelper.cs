using System;
using Windows.Media.Protection;
using Windows.Media.Protection.PlayReady;
using Windows.UI.Xaml.Controls;

namespace BetterXCloudUWP.Services
{
    public static class PlayReadyHelper
    {
        public static void EnablePlayReady(MediaElement mediaElement, string licenseAcquisitionUrl, string customData = null)
        {
            var protectionManager = new MediaProtectionManager();
            protectionManager.Properties["Windows.Media.Protection.MediaProtectionSystemId"] = PlayReadyStatics.ProtectionSystemId; // Corrected property name
            protectionManager.Properties["Windows.Media.Protection.MediaProtectionSystemIdMapping"] = new Windows.Foundation.Collections.PropertySet
                {
                    { PlayReadyStatics.ProtectionSystemId.ToString(), "Windows.Media.Protection.PlayReady.PlayReadyWinRTTrustedInput" }
                };
            protectionManager.Properties["Windows.Media.Protection.MediaProtectionSystemIdMapping"] = PlayReadyStatics.ProtectionSystemId.ToString(); // Corrected property name
            protectionManager.Properties["Windows.Media.Protection.MediaProtectionContainerGuid"] = PlayReadyStatics.ProtectionSystemId; // Corrected property name

            if (!string.IsNullOrEmpty(licenseAcquisitionUrl))
            {
                var serviceRequest = new PlayReadyLicenseAcquisitionServiceRequest();
                serviceRequest.Uri = new Uri(licenseAcquisitionUrl);
                if (!string.IsNullOrEmpty(customData))
                {
                    serviceRequest.ChallengeCustomData = customData;
                }
            }

            mediaElement.ProtectionManager = protectionManager;
        }
    }
}
