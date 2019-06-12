using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// AccountSettings
/// 
/// Replace this class with an interface to your own applications account settings. 
/// 
/// Each account should as a minimum have the following:
///  - A URL pointing to the identity provider for sending Auth Requests
///  - A X.509 certificate for validating the SAML Responses from the identity provider
/// 
/// These should be retrieved from the account store/database on each request in the authentication flow.
/// </summary>
public class AccountSettings
{
   // public string certificate = "-----BEGIN CERTIFICATE-----\nMIIBrTCCAaGgAwIBAgIBATADBgEAMGcxCzAJBgNVBAYTAlVTMRMwEQYDVQQIDApD\nYWxpZm9ybmlhMRUwEwYDVQQHDAxTYW50YSBNb25pY2ExETAPBgNVBAoMCE9uZUxv\nZ2luMRkwFwYDVQQDDBBhcHAub25lbG9naW4uY29tMB4XDTEwMDMwOTA5NTgzNFoX\nDTE1MDMwOTA5NTgzNFowZzELMAkGA1UEBhMCVVMxEzARBgNVBAgMCkNhbGlmb3Ju\naWExFTATBgNVBAcMDFNhbnRhIE1vbmljYTERMA8GA1UECgwIT25lTG9naW4xGTAX\nBgNVBAMMEGFwcC5vbmVsb2dpbi5jb20wgZ8wDQYJKoZIhvcNAQEBBQADgY0AMIGJ\nAoGBANtmwriqGBbZy5Dwy2CmJEtHEENVPoATCZP3UDESRDQmXy9Q0Kq1lBt+KyV4\nkJNHYAAQ9egLGWQ8/1atkPBye5s9fxROtf8VO3uk/x/X5VSRODIrhFISGmKUnVXa\nUhLFIXkGSCAIVfoR5S2ggdfpINKUWGsWS/lEzLNYMBkURXuVAgMBAAEwAwYBAAMB\nAA==\n-----END CERTIFICATE-----";
    public string certificate = @"MIIEBTCCAu2gAwIBAgIUOaQErsft4OyTq/l2Hnl0Lp7pgwwwDQYJKoZIhvcNAQEF
BQAwUjEdMBsGA1UECgwUQ2Fycm9sbCBPcmdhbml6YXRpb24xFTATBgNVBAsMDE9u
ZUxvZ2luIElkUDEaMBgGA1UEAwwRT25lTG9naW4gQWNjb3VudCAwHhcNMTkwMzA2
MTQ1ODMxWhcNMjQwMzA2MTQ1ODMxWjBSMR0wGwYDVQQKDBRDYXJyb2xsIE9yZ2Fu
aXphdGlvbjEVMBMGA1UECwwMT25lTG9naW4gSWRQMRowGAYDVQQDDBFPbmVMb2dp
biBBY2NvdW50IDCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBAMFZHbBb
2gYwbTrx+Q++q+/J4aKBS8qt7oEyZgRcTgV+6V+cOWqjpaf7HgNuptuu/Boa0kLL
QY2l86pNSW9taFxxcJgDC7dRzzOwOCB46+dn1w5+qXvUxnYwe4zHEROBBSeQUtRw
77RUMEPa22l/hGz/LssiYv4zKhF24x9YGx7TUwRlrFAIGVngzxIwb4ai9xhHhWvR
mtQtaXXL34KTVbKrfBVhmdCYpEP0l4eUSX0T3nZJv1xDRIh/JO7lU0s2zhb78WU9
n3dXBLHLqU8Xc6fWslnBUjFe6aZRYa23Rvrw8kEZwvMyFsL2/wnbZAdgNCrFzCwL
a/K/b5zWRQUrx+8CAwEAAaOB0jCBzzAMBgNVHRMBAf8EAjAAMB0GA1UdDgQWBBSs
4s5SRRN7IuZqpSYAt2SCcEosBjCBjwYDVR0jBIGHMIGEgBSs4s5SRRN7IuZqpSYA
t2SCcEosBqFWpFQwUjEdMBsGA1UECgwUQ2Fycm9sbCBPcmdhbml6YXRpb24xFTAT
BgNVBAsMDE9uZUxvZ2luIElkUDEaMBgGA1UEAwwRT25lTG9naW4gQWNjb3VudCCC
FDmkBK7H7eDsk6v5dh55dC6e6YMMMA4GA1UdDwEB/wQEAwIHgDANBgkqhkiG9w0B
AQUFAAOCAQEAf8YPuwlxd9siONLUFbYdz0e00lx6qC39xFXUbwjFoZjdzuOc88EU
TbJ3Ma+wfsSaaaiLO6/hpWAaAbvkZF9jRc37iRSmr2wqu3paX6ymfJW7ToJ7Rnqp
KLR4s+8nt2nsoww3+/UshEMmB/9LH6EbWtaoNg+1VV+uzjVbumuP0Z+72ZCjc38X
cLLVa3OPbt4+l4h9RBjra8T3uMU1unBi35VX8l9pHAzeJaHmtxv4jIufZMp2pAD/
daqZbmdcR3dPEc/BlfWTZ19prSeVk+TrXCGxAViu8JSbhRTDHzT3ClCnvlfX4aVv
PvJVxENXdwpIlI6YC/8E5KDC8SlwhF9TcA==";
    public string idp_sso_target_url = "https://carrollorg.onelogin.com/trust/saml2/http-redirect/sso/914533";
    //https://carrollorg.onelogin.com/saml/signon/20219
}
