using System;
using System.Collections.Generic;

namespace ServiceStudioEnvironmentsSyncApp
{
    public class AppSettings
    {
        public string ServiceStudioPath { get; set; }
        public string OdCStudioPath { get; set; }
        public string ServerUrl { get; set; }
        public int SyncIntervalInMinutes { get; set; } = 5; // Default to 5 minutes
        public bool SyncSensitiveInfo { get; set; } = true; // Default to true
        public int LogRetentionInDays { get; set; } = 30; // Default to 30 days
        public bool AutoStart { get; set; } = false; // Default to not auto-start
        public string ApiKey { get; set; }
    }

    // Structure to hold settings
    public class ServiceStudioServerInfo
    {
        public HostInfoServiceStudio HostInfo { get; set; }
        public string UserName { get; set; }
        public bool DoNotSaveSecret { get; set; }
        public Password Password { get; set; }
        public bool PasswordIsEncryptedForWebServiceRequest { get; set; }
        public DateTime LastLogin { get; set; }
        public string CustomName { get; set; }
        public CompanyInfo CompanyInfo { get; set; }
    }

    // Structure to hold ODCStudio server info
    public class ODCStudioServerInfo
    {
        public string UserName { get; set; }
        public string CustomName { get; set; }
        public DateTime LastLogin { get; set; }
        public HostInfoODC HostInfo { get; set; }
        public TokenInfo TokenInfo { get; set; }
    }

    public class HostInfoServiceStudio
    {
        public string HostName { get; set; }
        public string PortNumber { get; set; } // Nullable if xsi:nil="true"
        public string DisplayName { get; set; }
        public string HostEnvironmentType { get; set; }
    }

    public class HostInfoODC
    {
        public string DisplayName { get; set; }
        public string PortNumber { get; set; } // Nullable if xsi:nil="true"
        public string EnvironmentId { get; set; }
        public string HostName { get; set; }
    }

    public class Password
    {
        public string EncryptedValue { get; set; }
    }

    public class TokenInfo
    {
        public string EncryptedIdToken { get; set; }
        public string EncryptedRefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
    }

    public class CompanyInfo
    {
        public string Name { get; set; }
    }

    public class SyncPayload
    {
        public DateTime Timestamp { get; set; }
        public List<ServiceStudioServerInfo> ServiceStudioServers { get; set; }
        public List<ODCStudioServerInfo> ODCStudioServers { get; set; }
    }
}