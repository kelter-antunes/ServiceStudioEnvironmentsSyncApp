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

    public class ServiceStudioServerInfo
    {
        public HostInfoServiceStudio HostInfo { get; set; }
        public string UserName { get; set; }
        public bool DoNotSaveSecret { get; set; }
        public Password Password { get; set; }
        public bool PasswordIsEncryptedForWebServiceRequest { get; set; }

        // Default LastLogin to 1900-01-01
        private DateTime _lastLogin = new DateTime(1900, 1, 1);
        public DateTime LastLogin
        {
            get { return _lastLogin; }
            set { _lastLogin = value == default ? new DateTime(1900, 1, 1) : value; }
        }

        public string CustomName { get; set; }
        public CompanyInfo CompanyInfo { get; set; }
    }

    public class ODCStudioServerInfo
    {
        public string UserName { get; set; }
        public string CustomName { get; set; }

        // Nullable DateTime for LastLogin to allow null, default to 1900-01-01
        private DateTime? _lastLogin = new DateTime(1900, 1, 1);
        public DateTime? LastLogin
        {
            get { return _lastLogin ?? new DateTime(1900, 1, 1); }
            set { _lastLogin = value ?? new DateTime(1900, 1, 1); }
        }

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

        // Default RefreshTokenExpiration to 1900-01-01
        private DateTime _refreshTokenExpiration = new DateTime(1900, 1, 1);
        public DateTime RefreshTokenExpiration
        {
            get { return _refreshTokenExpiration; }
            set { _refreshTokenExpiration = value == default ? new DateTime(1900, 1, 1) : value; }
        }
    }

    public class CompanyInfo
    {
        public string Name { get; set; }
    }

    public class SyncPayload
    {
        public DateTime Timestamp { get; set; } = new DateTime(1900, 1, 1); // Default to 1900-01-01
        public List<ServiceStudioServerInfo> ServiceStudioServers { get; set; }
        public List<ODCStudioServerInfo> ODCStudioServers { get; set; }
    }
}
