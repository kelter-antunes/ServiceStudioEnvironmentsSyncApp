using System;
using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Reflection;
using System.Timers;
using System.Linq;

namespace ServiceStudioEnvironmentsSyncApp
{
    public class TrayApplicationContext : ApplicationContext
    {
        private NotifyIcon _notifyIcon;
        private ContextMenuStrip _contextMenu;
        private System.Timers.Timer _syncTimer;
        private Logger _logger;
        private AppSettings _settings;
        private string _latestSyncTime = "Never";
        private string _latestError = "None";
        private Version _appVersion;
        private string _assemblyCopyright;
        private bool _backupPerformed = false;

        public TrayApplicationContext()
        {
            InitializeApplication();
        }

        private void InitializeApplication()
        {
            // Initialize Logger with default retention
            _logger = new Logger(30);
            _logger.Log("Application started.");

            // Load settings
            if (!LoadSettings())
            {
                _logger.Log("Failed to load settings. Application will exit.", LogLevel.Error);
                MessageBox.Show("Failed to load settings. Application will exit.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ExitThread();
                return;
            }

            // Perform first-time backup if not already done
            PerformFirstTimeBackup();

            // Reinitialize Logger with actual log retention
            _logger = new Logger(_settings.LogRetentionInDays);
            _logger.Log("Logger reinitialized with specified log retention.");

            // Set auto-start based on settings
            SetAutoStart(_settings.AutoStart);

            // Retrieve application version
            _appVersion = Assembly.GetExecutingAssembly().GetName().Version;

            // Retrieve AssemblyCopyright
            var copyrightAttr = (AssemblyCopyrightAttribute)Assembly.GetExecutingAssembly()
                                        .GetCustomAttribute(typeof(AssemblyCopyrightAttribute));
            string defaultCopyright = "Copyright © 2024 Miguel 'Kelter' Antunes";
            if (copyrightAttr != null)
            {
                _assemblyCopyright = copyrightAttr.Copyright;
            }
            else
            {
                _assemblyCopyright = defaultCopyright;
            }

            // Initialize NotifyIcon
            InitializeTrayIcon();

            // Perform initial sync
            PerformSync();

            // Setup Timer
            _syncTimer = new System.Timers.Timer(_settings.SyncIntervalInMinutes * 60 * 1000);
            _syncTimer.Elapsed += async (sender, e) => await PerformSyncAsync();
            _syncTimer.AutoReset = true;
            _syncTimer.Start();
            _logger.Log($"Sync timer started with interval {_settings.SyncIntervalInMinutes} minutes.");
        }

        private bool LoadSettings()
        {
            try
            {
                string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Settings.json");
                if (!File.Exists(configPath))
                {
                    _logger.Log("Settings.json not found.", LogLevel.Error);
                    return false;
                }

                string json = File.ReadAllText(configPath);
                _settings = JsonConvert.DeserializeObject<AppSettings>(json);

                // Validate paths
                if (!Directory.Exists(_settings.ServiceStudioPath) || !Directory.Exists(_settings.OdCStudioPath))
                {
                    _logger.Log("One or both specified directories do not exist.", LogLevel.Error);
                    return false;
                }

                // Validate ServerUrl
                if (string.IsNullOrWhiteSpace(_settings.ServerUrl))
                {
                    _logger.Log("ServerUrl is not set in Settings.json.", LogLevel.Error);
                    return false;
                }


                // Validate ApiKey
                if (string.IsNullOrWhiteSpace(_settings.ApiKey))
                {
                    _logger.Log("ApiKey is not set in Settings.json.", LogLevel.Error);
                    return false;
                }


                _logger.Log("Settings loaded successfully.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Log($"Error loading settings: {ex.Message}", LogLevel.Error);
                return false;
            }
        }

        private void PerformFirstTimeBackup()
        {
            try
            {
                string backupsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "backups");
                if (!Directory.Exists(backupsDir))
                {
                    Directory.CreateDirectory(backupsDir);
                    _logger.Log("Backups directory created.");

                    // Define the paths to original Settings.xml files
                    string serviceStudioSettingsPath = Path.Combine(_settings.ServiceStudioPath, "Settings.xml");
                    string odcStudioSettingsPath = Path.Combine(_settings.OdCStudioPath, "Settings.xml");

                    // Check if the Settings.xml files exist before backing up
                    if (File.Exists(serviceStudioSettingsPath))
                    {
                        string backupServicePath = Path.Combine(backupsDir, $"ServiceStudio_Settings_{DateTime.Now:yyyyMMdd_HHmmss}.xml");
                        File.Copy(serviceStudioSettingsPath, backupServicePath);
                        _logger.Log($"ServiceStudio Settings.xml backed up to {backupServicePath}");
                    }
                    else
                    {
                        _logger.Log($"ServiceStudio Settings.xml not found at {serviceStudioSettingsPath}", LogLevel.Warning);
                    }

                    if (File.Exists(odcStudioSettingsPath))
                    {
                        string backupOdcPath = Path.Combine(backupsDir, $"ODCStudio_Settings_{DateTime.Now:yyyyMMdd_HHmmss}.xml");
                        File.Copy(odcStudioSettingsPath, backupOdcPath);
                        _logger.Log($"ODCStudio Settings.xml backed up to {backupOdcPath}");
                    }
                    else
                    {
                        _logger.Log($"ODCStudio Settings.xml not found at {odcStudioSettingsPath}", LogLevel.Warning);
                    }

                    _backupPerformed = true;
                }
                else
                {
                    _logger.Log("Backups already exist. Skipping backup.");
                }
            }
            catch (Exception ex)
            {
                _logger.Log($"Error performing backup: {ex.Message}", LogLevel.Error);
            }
        }

        private void InitializeTrayIcon()
        {
            Icon appIcon = ServiceStudioEnvironmentsSyncApp.Properties.Resources.app_icon;

            if (appIcon == null)
            {
                // Create a default icon if the specified icon is not found
                Bitmap bmp = new Bitmap(16, 16);
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.Transparent);
                    g.DrawRectangle(Pens.Red, 0, 0, 15, 15);
                }
                Icon defaultIcon = Icon.FromHandle(bmp.GetHicon());
                _notifyIcon = new NotifyIcon
                {
                    Icon = defaultIcon,
                    Visible = true,
                    Text = "ServiceStudio Environments Sync App"
                };
            }
            else
            {
                _notifyIcon = new NotifyIcon
                {
                    Icon = appIcon,
                    Visible = true,
                    Text = "ServiceStudio Environments Sync App"
                };
            }

            // Context Menu
            _contextMenu = new ContextMenuStrip();

            // Status menu item
            var statusItem = new ToolStripMenuItem("Status");
            statusItem.Click += StatusItem_Click;

            // Separator
            var separator = new ToolStripSeparator();

            // Exit menu item
            var exitItem = new ToolStripMenuItem("Exit");
            exitItem.Click += ExitItem_Click;

            _contextMenu.Items.Add(statusItem);
            _contextMenu.Items.Add(separator);
            _contextMenu.Items.Add(exitItem);

            _notifyIcon.ContextMenuStrip = _contextMenu;

            // Double-click to show detailed status
            _notifyIcon.DoubleClick += (s, e) =>
            {
                ShowDetailedStatus();
            };

            _logger.Log("Tray icon initialized.");
        }

        private void StatusItem_Click(object sender, EventArgs e)
        {
            ShowDetailedStatus();
        }

        private void ShowDetailedStatus()
        {
            var statusForm = new StatusForm(
                _latestSyncTime,
                _latestError,
                _appVersion
            );
            _logger.Log("User viewed detailed status.");
            statusForm.ShowDialog();
        }

        private void ExitItem_Click(object sender, EventArgs e)
        {
            _logger.Log("Exiting application.");
            _notifyIcon.Visible = false;
            _notifyIcon.Dispose();
            _syncTimer?.Stop();
            _syncTimer?.Dispose();
            ExitThread();
        }

        private void UpdateTrayText()
        {
            //// TODO: Temporarily disabled status on tooltip. Maybe this should be dropped and just show the app name on hover.

            /// Note: NotifyIcon.Text has a maximum length of 63 characters.
            //string tooltip = $"ServiceStudio Environments Sync App\nLast Sync: {_latestSyncTime}\nLast Error: {_latestError}";
            //if (tooltip.Length > 63)
            //{
            //    tooltip = $"ServiceStudio Environments Sync App\nLast Sync: {_latestSyncTime}";
            //    if (tooltip.Length > 63)
            //    {
            //        tooltip = $"ServiceStudio Environments Sync App...";
            //    }
            //}
            //_notifyIcon.Text = tooltip;
        }

        private async Task PerformSyncAsync()
        {
            _logger.Log("Starting synchronization.");
            try
            {
                var serviceStudioServers = new List<ServiceStudioServerInfo>();
                var odcStudioServers = new List<ODCStudioServerInfo>();

                // Extract ServiceStudio servers
                string serviceStudioSettings = Path.Combine(_settings.ServiceStudioPath, "Settings.xml");
                if (File.Exists(serviceStudioSettings))
                {
                    var extractedServiceStudio = ExtractServiceStudioServers(serviceStudioSettings);
                    serviceStudioServers.AddRange(extractedServiceStudio);
                    _logger.Log($"Extracted {extractedServiceStudio.Count} ServiceStudio servers.");
                }
                else
                {
                    _latestError = $"Settings.xml not found in {_settings.ServiceStudioPath}";
                    _logger.Log(_latestError, LogLevel.Error);
                }

                // Extract ODCStudio servers
                string odcStudioSettings = Path.Combine(_settings.OdCStudioPath, "Settings.xml");
                if (File.Exists(odcStudioSettings))
                {
                    var extractedODCStudio = ExtractODCStudioServers(odcStudioSettings);
                    odcStudioServers.AddRange(extractedODCStudio);
                    _logger.Log($"Extracted {extractedODCStudio.Count} ODCStudio servers.");
                }
                else
                {
                    _latestError = $"Settings.xml not found in {_settings.OdCStudioPath}";
                    _logger.Log(_latestError, LogLevel.Error);
                }

                if (serviceStudioServers.Count > 0 || odcStudioServers.Count > 0)
                {
                    // If SyncSensitiveInfo is disabled, omit Password and TokenInfo
                    if (!_settings.SyncSensitiveInfo)
                    {
                        foreach (var s in serviceStudioServers)
                        {
                            s.Password = null;
                        }
                        foreach (var o in odcStudioServers)
                        {
                            o.TokenInfo = null;
                        }
                        _logger.Log("Sensitive information synchronization is disabled.");
                    }

                    // Prepare JSON payload with settings to ignore null values
                    var payload = new SyncPayload
                    {
                        Timestamp = DateTime.UtcNow,
                        ServiceStudioServers = serviceStudioServers,
                        ODCStudioServers = odcStudioServers
                    };

                    string jsonPayload = JsonConvert.SerializeObject(payload, Formatting.Indented, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });



                    // Send to Server
                    using (HttpClient client = new HttpClient())
                    {
                        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                        // Add the API key to the request headers
                        client.DefaultRequestHeaders.Add("apikey", _settings.ApiKey);

                        HttpResponseMessage response = await client.PostAsync(_settings.ServerUrl, content);
                        _logger.Log($"Sent synchronization data to server. Status Code: {response.StatusCode}");

                        if (response.IsSuccessStatusCode)
                        {
                            // Receive the server's list of servers
                            string responseContent = await response.Content.ReadAsStringAsync();
                            var serverPayload = JsonConvert.DeserializeObject<SyncPayload>(responseContent);
                            _logger.Log("Received synchronization data from server.");

                            // If SyncSensitiveInfo is disabled, omit Password and TokenInfo from received data
                            if (!_settings.SyncSensitiveInfo)
                            {
                                foreach (var s in serverPayload.ServiceStudioServers)
                                {
                                    s.Password = null;
                                }
                                foreach (var o in serverPayload.ODCStudioServers)
                                {
                                    o.TokenInfo = null;
                                }
                                _logger.Log("Sensitive information received from server is ignored based on settings.");
                            }

                            // Process received ServiceStudioServers
                            int addedService = 0;
                            if (serverPayload.ServiceStudioServers != null && serverPayload.ServiceStudioServers.Any())
                            {
                                foreach (var server in serverPayload.ServiceStudioServers)
                                {
                                    if (!serviceStudioServers.Exists(s => s.HostInfo.HostName.Equals(server.HostInfo.HostName, StringComparison.OrdinalIgnoreCase)))
                                    {
                                        AddServiceStudioServerToXml(serviceStudioSettings, server);
                                        addedService++;
                                    }
                                }
                            }

                            // Process received ODCStudioServers
                            int addedODC = 0;
                            if (serverPayload.ODCStudioServers != null && serverPayload.ODCStudioServers.Any())
                            {
                                foreach (var server in serverPayload.ODCStudioServers)
                                {
                                    if (!odcStudioServers.Exists(s => s.HostInfo.HostName.Equals(server.HostInfo.HostName, StringComparison.OrdinalIgnoreCase)))
                                    {
                                        AddODCStudioServerToXml(odcStudioSettings, server);
                                        addedODC++;
                                    }
                                }
                            }

                            _latestSyncTime = DateTime.Now.ToString("g");
                            _latestError = "None";
                            _logger.Log($"Synchronization successful. Added {addedService} ServiceStudio and {addedODC} ODCStudio servers.");
                        }
                        else
                        {
                            _latestError = $"Server responded with status code {response.StatusCode}";
                            _logger.Log(_latestError, LogLevel.Error);
                        }
                    }
                }
                else
                {
                    _latestError = "No server information extracted.";
                    _logger.Log(_latestError, LogLevel.Warning);
                }
            }
            catch (Exception ex)
            {
                _latestError = ex.Message;
                _logger.Log($"Synchronization error: {ex.Message}", LogLevel.Error);
            }
            finally
            {
                UpdateTrayText();
            }
        }

        private void PerformSync()
        {
            Task.Run(async () => await PerformSyncAsync());
        }

        // Extraction method for ServiceStudio servers
        private List<ServiceStudioServerInfo> ExtractServiceStudioServers(string xmlPath)
        {
            var servers = new List<ServiceStudioServerInfo>();
            try
            {
                XDocument doc = XDocument.Load(xmlPath);
                foreach (var element in doc.Descendants("MruPasswordBasedServers"))
                {
                    var hostInfoElement = element.Element("HostInfo");
                    if (hostInfoElement != null)
                    {
                        var hostInfo = new HostInfoServiceStudio
                        {
                            HostName = hostInfoElement.Element("HostName")?.Value,
                            PortNumber = hostInfoElement.Element("PortNumber")?.Attribute(XNamespace.Xml + "nil")?.Value == "true" ? null : hostInfoElement.Element("PortNumber")?.Value,
                            DisplayName = hostInfoElement.Element("DisplayName")?.Value,
                            HostEnvironmentType = hostInfoElement.Element("HostEnvironmentType")?.Value
                        };

                        var passwordElement = element.Element("Password");
                        var password = new Password
                        {
                            EncryptedValue = passwordElement?.Element("EncryptedValue")?.Value
                        };

                        var companyInfoElement = element.Element("CompanyInfo");
                        var companyInfo = new CompanyInfo
                        {
                            Name = companyInfoElement?.Element("Name")?.Value
                        };

                        bool doNotSaveSecret = false;
                        bool.TryParse(element.Element("DoNotSaveSecret")?.Value, out doNotSaveSecret);

                        bool isEncrypted = false;
                        bool.TryParse(element.Element("PasswordIsEncryptedForWebServiceRequest")?.Value, out isEncrypted);

                        DateTime lastLogin = DateTime.MinValue;
                        DateTime.TryParse(element.Element("LastLogin")?.Value, out lastLogin);

                        var serverInfo = new ServiceStudioServerInfo
                        {
                            HostInfo = hostInfo,
                            UserName = element.Element("UserName")?.Value,
                            DoNotSaveSecret = doNotSaveSecret,
                            Password = password,
                            PasswordIsEncryptedForWebServiceRequest = isEncrypted,
                            LastLogin = lastLogin,
                            CustomName = element.Element("CustomName")?.Value,
                            CompanyInfo = companyInfo
                        };

                        servers.Add(serverInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                _latestError = $"Error extracting ServiceStudio servers from XML: {ex.Message}";
                _logger.Log(_latestError, LogLevel.Error);
            }
            return servers;
        }

        // Extraction method for ODCStudio servers
        private List<ODCStudioServerInfo> ExtractODCStudioServers(string xmlPath)
        {
            var servers = new List<ODCStudioServerInfo>();
            try
            {
                XDocument doc = XDocument.Load(xmlPath);
                foreach (var element in doc.Descendants("MruServers"))
                {
                    var hostInfoElement = element.Element("HostInfo");
                    if (hostInfoElement != null)
                    {
                        var hostInfo = new HostInfoODC
                        {
                            HostName = hostInfoElement.Element("Hostname")?.Value, // Note: 'Hostname' with lowercase 'n' as per original code
                            PortNumber = hostInfoElement.Element("PortNumber")?.Attribute(XNamespace.Xml + "nil")?.Value == "true" ? null : hostInfoElement.Element("PortNumber")?.Value,
                            DisplayName = hostInfoElement.Element("DisplayName")?.Value,
                            EnvironmentId = hostInfoElement.Element("EnvironmentId")?.Value
                        };

                        var tokenInfoElement = element.Element("TokenInfo");
                        DateTime refreshTokenExpiration = DateTime.MinValue;
                        DateTime.TryParse(tokenInfoElement.Element("RefreshTokenExpiration")?.Value, out refreshTokenExpiration);

                        var tokenInfo = new TokenInfo
                        {
                            EncryptedIdToken = tokenInfoElement?.Element("EncryptedIdToken")?.Value,
                            EncryptedRefreshToken = tokenInfoElement?.Element("EncryptedRefreshToken")?.Value,
                            RefreshTokenExpiration = refreshTokenExpiration
                        };

                        DateTime lastLogin = DateTime.MinValue;
                        DateTime.TryParse(element.Element("LastLogin")?.Value, out lastLogin);

                        var serverInfo = new ODCStudioServerInfo
                        {
                            UserName = element.Element("Username")?.Value,
                            CustomName = element.Element("CustomName")?.Value,
                            LastLogin = lastLogin,
                            HostInfo = hostInfo,
                            TokenInfo = tokenInfo
                        };

                        servers.Add(serverInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                _latestError = $"Error extracting ODCStudio servers from XML: {ex.Message}";
                _logger.Log(_latestError, LogLevel.Error);
            }
            return servers;
        }

        // Method to add ServiceStudio server to Settings.xml
        private void AddServiceStudioServerToXml(string xmlPath, ServiceStudioServerInfo server)
        {
            try
            {
                XDocument doc = XDocument.Load(xmlPath);
                XElement root = doc.Root;

                // Create the new MruPasswordBasedServers element
                XElement newServer = new XElement("MruPasswordBasedServers",
                    new XElement("HostInfo",
                        new XElement("HostName", server.HostInfo.HostName),
                        // Adjusted condition for PortNumber: empty, null, or "0"
                        string.IsNullOrWhiteSpace(server.HostInfo.PortNumber) || server.HostInfo.PortNumber == "0"
                            ? new XElement("PortNumber", null, new XAttribute(XNamespace.Xml + "nil", "true"))
                            : new XElement("PortNumber", server.HostInfo.PortNumber),
                        new XElement("DisplayName", server.HostInfo.DisplayName),
                        new XElement("HostEnvironmentType", server.HostInfo.HostEnvironmentType)
                    ),
                    new XElement("UserName", server.UserName),
                    new XElement("DoNotSaveSecret", server.DoNotSaveSecret.ToString().ToLower()),
                    _settings.SyncSensitiveInfo && server.Password != null && !string.IsNullOrWhiteSpace(server.Password.EncryptedValue)
                        ? new XElement("Password",
                            new XElement("EncryptedValue", server.Password.EncryptedValue))
                        : new XElement("Password", null, new XAttribute(XNamespace.Xml + "nil", "true")),
                    new XElement("PasswordIsEncryptedForWebServiceRequest", server.PasswordIsEncryptedForWebServiceRequest.ToString().ToLower()),
                    new XElement("LastLogin", server.LastLogin.ToString("o")),
                    new XElement("CustomName", server.CustomName ?? string.Empty),
                    new XElement("CompanyInfo",
                        new XElement("Name", server.CompanyInfo?.Name ?? string.Empty))
                );

                // Append the new server
                root.Add(newServer);

                // Save the updated XML
                doc.Save(xmlPath);

                _latestSyncTime = DateTime.Now.ToString("g");
                _logger.Log($"Added new ServiceStudio server: {server.HostInfo.HostName}");
            }
            catch (Exception ex)
            {
                _latestError = $"Error adding ServiceStudio server to XML: {ex.Message}";
                _logger.Log(_latestError, LogLevel.Error);
            }
        }


        // Method to add ODCStudio server to Settings.xml
        private void AddODCStudioServerToXml(string xmlPath, ODCStudioServerInfo server)
        {
            try
            {
                XDocument doc = XDocument.Load(xmlPath);
                XElement root = doc.Root;

                // Create the new MruServers element
                XElement newServer = new XElement("MruServers",
                    new XElement("UserName", server.UserName),
                    new XElement("CustomName", server.CustomName ?? string.Empty),
                    new XElement("LastLogin", server.LastLogin.ToString("o")),
                    new XElement("HostInfo",
                        new XElement("Hostname", server.HostInfo.HostName),
                        // Adjusted condition for PortNumber: empty, null, or "0"
                        string.IsNullOrWhiteSpace(server.HostInfo.PortNumber) || server.HostInfo.PortNumber == "0"
                            ? new XElement("PortNumber", null, new XAttribute(XNamespace.Xml + "nil", "true"))
                            : new XElement("PortNumber", server.HostInfo.PortNumber),
                        new XElement("DisplayName", server.HostInfo.DisplayName),
                        new XElement("EnvironmentId", server.HostInfo.EnvironmentId)
                    ),
                    _settings.SyncSensitiveInfo && server.TokenInfo != null && (!string.IsNullOrWhiteSpace(server.TokenInfo.EncryptedIdToken) || !string.IsNullOrWhiteSpace(server.TokenInfo.EncryptedRefreshToken))
                        ? new XElement("TokenInfo",
                            new XElement("EncryptedIdToken", server.TokenInfo.EncryptedIdToken),
                            new XElement("EncryptedRefreshToken", server.TokenInfo.EncryptedRefreshToken),
                            new XElement("RefreshTokenExpiration", server.TokenInfo.RefreshTokenExpiration.ToString("o")))
                        : new XElement("TokenInfo", null, new XAttribute(XNamespace.Xml + "nil", "true"))
                );

                // Append the new server
                root.Add(newServer);

                // Save the updated XML
                doc.Save(xmlPath);

                _latestSyncTime = DateTime.Now.ToString("g");
                _logger.Log($"Added new ODCStudio server: {server.HostInfo.HostName}");
            }
            catch (Exception ex)
            {
                _latestError = $"Error adding ODCStudio server to XML: {ex.Message}";
                _logger.Log(_latestError, LogLevel.Error);
            }
        }


        // --- Auto-Start Methods ---

        private void SetAutoStart(bool enable)
        {
            try
            {
                string appName = "ServiceStudioEnvironmentsSyncApp"; // Name of your application
                string exePath = Assembly.GetExecutingAssembly().Location;

                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true))
                {
                    if (enable)
                    {
                        key.SetValue(appName, $"\"{exePath}\"");
                        _logger.Log("Auto-start enabled.");
                    }
                    else
                    {
                        if (key.GetValue(appName) != null)
                        {
                            key.DeleteValue(appName);
                            _logger.Log("Auto-start disabled.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log($"Error setting auto-start: {ex.Message}", LogLevel.Error);
            }
        }

    }
}