/*
 * Copyright (c) 2018 Samsung Electronics Co., Ltd All Rights Reserved
 *
 * Licensed under the Apache License, Version 2.0 (the License);
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an AS IS BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Tizen.Network.WiFi
{
    /// <summary>
    /// A class for managing the WiFiManager handle.
    /// </summary>
    /// <since_tizen> 3 </since_tizen>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public sealed class SafeWiFiManagerHandle : SafeHandle
    {
        private int _tid;

        internal SafeWiFiManagerHandle() : base(IntPtr.Zero, true)
        {
        }

        /// <summary>
        /// Checks the validity of the handle.
        /// </summary>
        /// <value>Represents the validity of the handle.</value>
        /// <since_tizen> 3 </since_tizen>
        public override bool IsInvalid
        {
            get
            {
                return this.handle == IntPtr.Zero;
            }
        }

        /// <summary>
        /// Release the handle
        /// </summary>
        protected override bool ReleaseHandle()
        {
            Interop.WiFi.Deinitialize(_tid, this.handle);
            this.SetHandle(IntPtr.Zero);
            return true;
        }

        internal void SetTID(int id)
        {
            _tid = id;
            Log.Info(Globals.LogTag, "New Handle for Thread " + _tid);
        }
    }

    /// <summary>
    /// A manager class which allows applications to connect to a Wireless Local Area Network (WLAN) and transfer data over the network.
    /// The Wi-Fi Manager enables your application to activate and deactivate a local Wi-Fi device, and to connect to a WLAN network in the infrastructure mode.
    /// </summary>
    /// <since_tizen> 3 </since_tizen>
    static public class WiFiManager
    {
        /// <summary>
        /// The local MAC address.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        /// <value>Represents the MAC address of the Wi-Fi.</value>
        /// <privilege>http://tizen.org/privilege/network.get</privilege>
        static public string MacAddress
        {
            get
            {
                return WiFiManagerImpl.Instance.MacAddress;
            }
        }

        /// <summary>
        /// The name of the network interface.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        /// <value>Interface name of the Wi-Fi.</value>
        /// <privilege>http://tizen.org/privilege/network.get</privilege>
        static public string InterfaceName
        {
            get
            {
                return WiFiManagerImpl.Instance.InterfaceName;
            }
        }

        /// <summary>
        /// The network connection state.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        /// <value>Represents the connection state of the Wi-Fi.</value>
        /// <privilege>http://tizen.org/privilege/network.get</privilege>
        static public WiFiConnectionState ConnectionState
        {
            get
            {
                return WiFiManagerImpl.Instance.ConnectionState;
            }
        }

        /// <summary>
        /// A property to check whether Wi-Fi is activated.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        /// <value>Boolean value to check whether Wi-Fi is activated or not.</value>
        /// <privilege>http://tizen.org/privilege/network.get</privilege>
        static public bool IsActive
        {
            get
            {
                return WiFiManagerImpl.Instance.IsActive;
            }
        }

        /// <summary>
        /// The Wi-Fi scan state.
        /// </summary>
        /// <since_tizen> 6 </since_tizen>
        /// <value>Represents the scan state of the Wi-Fi.</value>
        static public WiFiScanState ScanState
        {
            get
            {
                return WiFiManagerImpl.Instance.ScanState;
            }
        }

        /// <summary>
        /// DeviceStateChanged is raised when the device state is changed.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        /// <privilege>http://tizen.org/privilege/network.get</privilege>
        /// <feature>http://tizen.org/feature/network.wifi</feature>
        static public event EventHandler<DeviceStateChangedEventArgs> DeviceStateChanged
        {
            add
            {
                WiFiManagerImpl.Instance.DeviceStateChanged += value;
            }
            remove
            {
                WiFiManagerImpl.Instance.DeviceStateChanged -= value;
            }
        }

        /// <summary>
        /// ConnectionStateChanged is raised when the connection state is changed.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        /// <privilege>http://tizen.org/privilege/network.get</privilege>
        /// <feature>http://tizen.org/feature/network.wifi</feature>
        static public event EventHandler<ConnectionStateChangedEventArgs> ConnectionStateChanged
        {
            add
            {
                WiFiManagerImpl.Instance.ConnectionStateChanged += value;
            }
            remove
            {
                WiFiManagerImpl.Instance.ConnectionStateChanged -= value;
            }
        }

        /// <summary>
        /// RssiLevelChanged is raised when the RSSI of the connected Wi-Fi is changed.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        /// <privilege>http://tizen.org/privilege/network.get</privilege>
        /// <feature>http://tizen.org/feature/network.wifi</feature>
        static public event EventHandler<RssiLevelChangedEventArgs> RssiLevelChanged
        {
            add
            {
                WiFiManagerImpl.Instance.RssiLevelChanged += value;
            }
            remove
            {
                WiFiManagerImpl.Instance.RssiLevelChanged -= value;
            }
        }

        /// <summary>
        /// BackgroundScanFinished is raised when the background scan is finished.
        /// The background scan starts automatically when Wi-Fi is activated. The callback will be invoked periodically.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        /// <privilege>http://tizen.org/privilege/network.get</privilege>
        /// <feature>http://tizen.org/feature/network.wifi</feature>
        static public event EventHandler BackgroundScanFinished
        {
            add
            {
                WiFiManagerImpl.Instance.BackgroundScanFinished += value;
            }
            remove
            {
                WiFiManagerImpl.Instance.BackgroundScanFinished -= value;
            }
        }

        /// <summary>
        /// ScanStateChanged is raised when the scan state is changed.
        /// </summary>
        /// <since_tizen> 6 </since_tizen>
        /// <feature>http://tizen.org/feature/network.wifi</feature>
        static public event EventHandler<ScanStateChangedEventArgs> ScanStateChanged
        {
            add
            {
                WiFiManagerImpl.Instance.ScanStateChanged += value;
            }
            remove
            {
                WiFiManagerImpl.Instance.ScanStateChanged -= value;
            }
        }

        /// <summary>
        /// Retrieves the Wi-Fi safe handle, which is used to manage Wi-Fi operations securely.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        /// <returns>The instance of the SafeWiFiManagerHandle.</returns>
        /// <feature>http://tizen.org/feature/network.wifi</feature>
        /// <privilege>http://tizen.org/privilege/network.get</privilege>
        /// <exception cref="NotSupportedException">Thrown when the Wi-Fi is not supported.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when the permission is denied.</exception>
        /// <exception cref="OutOfMemoryException">Thrown when the system is out of memory.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the method failed due to an invalid operation.</exception>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static SafeWiFiManagerHandle GetWiFiHandle()
        {
            return WiFiManagerImpl.Instance.GetSafeHandle();
        }

        /// <summary>
        /// Retrieves the results of the Wi-Fi scan, providing a list of discovered Wi-Fi access points (APs).
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        /// <returns>A list of the WiFiAP objects.</returns>
        /// <feature>http://tizen.org/feature/network.wifi</feature>
        /// <privilege>http://tizen.org/privilege/network.get</privilege>
        /// <exception cref="NotSupportedException">Thrown when the Wi-Fi is not supported.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when the permission is denied.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the method failed due to an invalid operation.</exception>
        static public IEnumerable<WiFiAP> GetFoundAPs()
        {
            return WiFiManagerImpl.Instance.GetFoundAPs();
        }

        /// <summary>
        /// Retrieves the results of the scan specific to a particular Wi-Fi access point (AP), providing a list of discovered APs matching the specified ESSID.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        /// <returns>A list containing the WiFiAP objects.</returns>
        /// <feature>http://tizen.org/feature/network.wifi</feature>
        /// <privilege>http://tizen.org/privilege/network.get</privilege>
        /// <exception cref="NotSupportedException">Thrown when the Wi-Fi is not supported.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when the permission is denied.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the method failed due to an invalid operation.</exception>
        static public IEnumerable<WiFiAP> GetFoundSpecificAPs()
        {
            return WiFiManagerImpl.Instance.GetFoundSpecificAPs();
        }

        /// <summary>
        /// Retrieves the results of the BSSID scan, providing a list of discovered Wi-Fi access points (APs) based on their BSSID (Basic Service Set Identifier).
        /// </summary>
        /// <since_tizen> 5 </since_tizen>
        /// <returns>A list of the WiFiAP objects.</returns>
        /// <feature>http://tizen.org/feature/network.wifi</feature>
        /// <privilege>http://tizen.org/privilege/network.get</privilege>
        /// <exception cref="NotSupportedException">Thrown when the Wi-Fi is not supported.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when the permission is denied.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the method failed due to an invalid operation.</exception>
        static public IEnumerable<WiFiAP> GetFoundBssids()
        {
            return WiFiManagerImpl.Instance.GetFoundBssids();
        }

        /// <summary>
        /// Retrieves the list of Wi-Fi configurations stored on the device.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        /// <returns>A list containing the WiFiConfiguration objects.</returns>
        /// <feature>http://tizen.org/feature/network.wifi</feature>
        /// <privilege>http://tizen.org/privilege/network.profile</privilege>
        /// <exception cref="NotSupportedException">Thrown when the Wi-Fi is not supported.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when the permission is denied.</exception>
        /// <exception cref="OutOfMemoryException">Thrown when system is out of memory.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the method failed due to an invalid operation.</exception>
        static public IEnumerable<WiFiConfiguration> GetWiFiConfigurations()
        {
            return WiFiManagerImpl.Instance.GetWiFiConfigurations();
        }

        /// <summary>
        /// Saves the Wi-Fi configuration of the access point to the device's storage.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        /// <param name="configuration">The configuration to be stored.</param>
        /// <feature>http://tizen.org/feature/network.wifi</feature>
        /// <privilege>http://tizen.org/privilege/network.profile</privilege>
        /// <exception cref="NotSupportedException">Thrown when the Wi-Fi is not supported.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when the permission is denied.</exception>
        /// <exception cref="ArgumentNullException">Thrown when WiFiConfiguration is passed as null.</exception>
        /// <exception cref="ArgumentException">Thrown when the method failed due to an invalid parameter.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the method failed due to an invalid operation.</exception>
        static public void SaveWiFiConfiguration(WiFiConfiguration configuration)
        {
            WiFiManagerImpl.Instance.SaveWiFiNetworkConfiguration(configuration);
        }

        /// <summary>
        /// Retrieves the information of the currently connected Wi-Fi access point (AP).
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        /// <returns>The connected Wi-Fi access point (AP) information.</returns>
        /// <feature>http://tizen.org/feature/network.wifi</feature>
        /// <privilege>http://tizen.org/privilege/network.get</privilege>
        /// <exception cref="NotSupportedException">Thrown when the Wi-Fi is not supported.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when the permission is denied.</exception>
        /// <exception cref="OutOfMemoryException">Thrown when system is out of memory.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the method failed due to an invalid operation.</exception>
        static public WiFiAP GetConnectedAP()
        {
            return WiFiManagerImpl.Instance.GetConnectedAP();
        }

        /// <summary>
        /// Activates the Wi-Fi module asynchronously.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        /// <returns> A task indicating whether the activate method is done or not.</returns>
        /// <feature>http://tizen.org/feature/network.wifi</feature>
        /// <privilege>http://tizen.org/privilege/network.set</privilege>
        /// <privilege>http://tizen.org/privilege/network.get</privilege>
        /// <exception cref="NotSupportedException">Thrown when the Wi-Fi is not supported.</exception>
        /// <exception cref="NowInProgressException">Thrown when the Wi-Fi activation is now in progress.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when the permission is denied.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the method failed due to an invalid operation.</exception>
        static public Task ActivateAsync()
        {
            return WiFiManagerImpl.Instance.ActivateAsync();
        }

        /// <summary>
        /// Activates the Wi-Fi asynchronously and displays the Wi-Fi picker (popup) when the Wi-Fi is not automatically connected.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        /// <returns>A task indicating whether the ActivateWithPicker method is done or not.</returns>
        /// <feature>http://tizen.org/feature/network.wifi</feature>
        /// <privilege>http://tizen.org/privilege/network.set</privilege>
        /// <privilege>http://tizen.org/privilege/network.get</privilege>
        /// <exception cref="NotSupportedException">Thrown when the Wi-Fi is not supported.</exception>
        /// <exception cref="NowInProgressException">Thrown when the Wi-Fi activation is now in progress.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when the permission is denied.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the method failed due to an invalid operation.</exception>
        static public Task ActivateWithPickerAsync()
        {
            return WiFiManagerImpl.Instance.ActivateWithWiFiPickerTestedAsync();
        }

        /// <summary>
        /// Deactivates the Wi-Fi module asynchronously.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        /// <returns>A task indicating whether the deactivate method is done or not.</returns>
        /// <feature>http://tizen.org/feature/network.wifi</feature>
        /// <privilege>http://tizen.org/privilege/network.set</privilege>
        /// <privilege>http://tizen.org/privilege/network.get</privilege>
        /// <exception cref="NotSupportedException">Thrown when the Wi-Fi is not supported.</exception>
        /// <exception cref="NowInProgressException">Thrown when the Wi-Fi deactivation is now in progress.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when the permission is denied.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the method failed due to an invalid operation.</exception>
        static public Task DeactivateAsync()
        {
            return WiFiManagerImpl.Instance.DeactivateAsync();
        }

        /// <summary>
        /// Initiates a Wi-Fi scan asynchronously.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        /// <returns>A task indicating whether the scan method is done or not.</returns>
        /// <feature>http://tizen.org/feature/network.wifi</feature>
        /// <privilege>http://tizen.org/privilege/network.set</privilege>
        /// <privilege>http://tizen.org/privilege/network.get</privilege>
        /// <exception cref="NotSupportedException">Thrown when the Wi-Fi is not supported.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when the permission is denied.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the method failed due to an invalid operation.</exception>
        static public Task ScanAsync()
        {
            return WiFiManagerImpl.Instance.ScanAsync();
        }

        /// <summary>
        /// Starts a specific access point scan asynchronously.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        /// <returns>A task indicating whether the ScanSpecificAP method is done or not.</returns>
        /// <param name="essid">The ESSID of the hidden AP.</param>
        /// <feature>http://tizen.org/feature/network.wifi</feature>
        /// <privilege>http://tizen.org/privilege/network.set</privilege>
        /// <privilege>http://tizen.org/privilege/network.get</privilege>
        /// <exception cref="NotSupportedException">Thrown when the Wi-Fi is not supported.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when the permission is denied.</exception>
        /// <exception cref="ArgumentException">Thrown when the method failed due to an invalid parameter.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the method failed due to an invalid operation.</exception>
        static public Task ScanSpecificAPAsync(string essid)
        {
            return WiFiManagerImpl.Instance.ScanSpecificAPAsync(essid);
        }

        /// <summary>
        /// Initiates a BSSID scan asynchronously.
        /// </summary>
        /// <remarks>
        /// This method must be called from MainThread.
        /// </remarks>
        /// <since_tizen> 5 </since_tizen>
        /// <returns>A task indicating whether the BssidScanAsync method is done or not.</returns>
        /// <feature>http://tizen.org/feature/network.wifi</feature>
        /// <privilege>http://tizen.org/privilege/network.set</privilege>
        /// <privilege>http://tizen.org/privilege/network.get</privilege>
        /// <exception cref="NotSupportedException">Thrown when the Wi-Fi is not supported.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when the permission is denied.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the method failed due to an invalid operation.</exception>
        static public Task BssidScanAsync()
        {
            return WiFiManagerImpl.Instance.BssidScanAsync();
        }

        /// <summary>
        /// Sets the auto scan mode for the Wi-Fi module.
        /// </summary>
        /// <since_tizen> 10 </since_tizen>
        /// <feature>http://tizen.org/feature/network.wifi</feature>
        /// <privilege>http://tizen.org/privilege/network.set</privilege>
        /// <privilege>http://tizen.org/privilege/network.get</privilege>
        /// <exception cref="NotSupportedException">Thrown when the Wi-Fi is not supported.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when the permission is denied.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the method failed due to an invalid operation.</exception>
        [EditorBrowsable(EditorBrowsableState.Never)]
        static public void SetAutoScanMode(WiFiAutoScanMode scanMode)
        {
            WiFiManagerImpl.Instance.SetAutoScanMode((int)scanMode);
        }

        /// <summary>
        /// Connects to a hidden Wi-Fi access point asynchronously.
        /// </summary>
        /// <remarks>
        /// This method must be called from MainThread.
        /// </remarks>
        /// <since_tizen> 10 </since_tizen>
        /// <returns>A task indicating whether the HiddenAPConnectAsync method is done or not.</returns>
        /// <feature>http://tizen.org/feature/network.wifi</feature>
        /// <privilege>http://tizen.org/privilege/network.set</privilege>
        /// <privilege>http://tizen.org/privilege/network.get</privilege>
        /// <exception cref="NotSupportedException">Thrown when the Wi-Fi is not supported.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when the permission is denied.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the method failed due to an invalid operation.</exception>
        [EditorBrowsable(EditorBrowsableState.Never)]
        static public Task HiddenAPConnectAsync(string essid, int secType, string password)
        {
            return WiFiManagerImpl.Instance.HiddenAPConnectAsync(essid, secType, password);
        }

        /// <summary>
        /// Creates a specific scan handle for the Wi-Fi module.
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        /// <feature>http://tizen.org/feature/network.wifi</feature>
        /// <privilege>http://tizen.org/privilege/network.profile</privilege>
        /// <exception cref="NotSupportedException">Thrown when the Wi-Fi is not supported.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when the permission is denied.</exception>
        /// <exception cref="OutOfMemoryException">Thrown when system is out of memory.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the method failed due to an invalid operation.</exception>
        [EditorBrowsable(EditorBrowsableState.Never)]
        static public void CreateSpecificHandle()
        {
            WiFiManagerImpl.Instance.CreateSpecificScanHandle();
        }

        /// <summary>
        /// Destroys the specific scan handle for the Wi-Fi module.
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        /// <feature>http://tizen.org/feature/network.wifi</feature>
        /// <privilege>http://tizen.org/privilege/network.profile</privilege>
        /// <exception cref="NotSupportedException">Thrown when the Wi-Fi is not supported.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when the permission is denied.</exception>
        /// <exception cref="OutOfMemoryException">Thrown when system is out of memory.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the method failed due to an invalid operation.</exception>
        [EditorBrowsable(EditorBrowsableState.Never)]
        static public void DestroySpecificHandle()
        {
            WiFiManagerImpl.Instance.DestroySpecificScanHandle();
        }

        /// <summary>
        /// Starts a multi-frequency scan for available Wi-Fi networks asynchronously.
        /// </summary>
        /// <remarks>
        /// This method must be called from MainThread.
        /// </remarks>
        /// <since_tizen> 9 </since_tizen>
        /// <param name="frequency">Frequency for which MultiScan is to be run.</param>
        /// <returns>A task indicating whether the StartMultiScan  method is done or not.</returns>
        /// <feature>http://tizen.org/feature/network.wifi</feature>
        /// <privilege>http://tizen.org/privilege/network.set</privilege>
        /// <privilege>http://tizen.org/privilege/network.get</privilege>
        /// <exception cref="NotSupportedException">Thrown when the Wi-Fi is not supported.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when the permission is denied.</exception>
        /// <exception cref="OutOfMemoryException">Thrown when system is out of memory.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the method failed due to an invalid operation.</exception>
        [EditorBrowsable(EditorBrowsableState.Never)]
        static public Task StartMultiScan(int frequency)
        {
            WiFiManagerImpl.Instance.SetSpecificScanFreq(frequency);
            return WiFiManagerImpl.Instance.StartMultiScan();
        }
    }
}
