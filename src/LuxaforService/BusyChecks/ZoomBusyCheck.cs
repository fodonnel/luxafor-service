using Microsoft.Win32;

namespace LuxaforService.BusyChecks
{
    public class ZoomBusyCheck : IBusyCheck
    {
        private const string WebcamKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\webcam\NonPackaged\C:#Program Files (x86)#Zoom#bin#Zoom.exe";
        private const string MicKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\microphone\NonPackaged\C:#Program Files (x86)#Zoom#bin#Zoom.exe";

        public bool IsBusy() => CapabilityInUse(WebcamKey) || CapabilityInUse(MicKey);

        private static bool CapabilityInUse(string key)
        { 
            using var registryKey = Registry.LocalMachine.OpenSubKey(key);
            if (registryKey == null)
            {
                return false;
            }

            var lastUsedStart = (long?) registryKey.GetValue("LastUsedTimeStart") ?? 0;
            var lastUsedStop = (long?) registryKey.GetValue("LastUsedTimeStop") ?? 0;
            return lastUsedStart != 0 && lastUsedStop == 0;
        }
    }
}
