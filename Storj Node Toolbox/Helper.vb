Imports System.IO
Imports System.Net
Imports System.ServiceProcess
Imports System.Text.RegularExpressions

Public Class Helper

    Public Shared Function GetExternalIp() As String
        Try
            Dim ExternalIP As String
            ExternalIP = (New WebClient()).DownloadString("http://checkip.dyndns.org/")
            ExternalIP = (New Regex("\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}")) _
                     .Matches(ExternalIP)(0).ToString()
            Return ExternalIP
        Catch
            Return Nothing
        End Try
    End Function
    Public Shared Function IsServiceRuning(ByVal serviceName As String) As Boolean
        Dim services As ServiceController() = ServiceController.GetServices()

        For Each s As ServiceController In ServiceController.GetServices()
            If s.ServiceName = serviceName AndAlso s.Status = ServiceControllerStatus.Running Then
                Return True

            End If
        Next

        Return False
    End Function
    Public Shared Function GetIPFromDomain(name As String) As String

        Dim myIPHostEntry As IPHostEntry = Dns.Resolve(name)
        Dim myIPAddresses() As IPAddress = myIPHostEntry.AddressList

        Return myIPAddresses.First.ToString
    End Function
    Public Shared Function CheckWinFirewallOpened(port As Integer) As Boolean
        Dim objFirewall = CreateObject("HNetCfg.FwMgr")
        Dim objPolicy = objFirewall.LocalPolicy.CurrentProfile
        If objFirewall.LocalPolicy.CurrentProfile.FirewallEnabled Then
            Dim colPorts = objPolicy.GloballyOpenPorts
            For Each objPort In colPorts
                If objPort.port = port Then

                    Return False

                End If
            Next
            Return True
        Else
            Return False
        End If
        'Dim CreatePort80 = CreateObject("HNetCfg.FwOpenPort")

        'CreatePort80.port = 80

        'CreatePort80.name = "HTTP"

        'CreatePort80.Enabled = False

        'CreatePort80.Protocol = 6

        'colPorts.add(CreatePort80)
    End Function
    Public Shared Function GetFileCountMatch(path As String) As Boolean
        Dim files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)
        If files IsNot Nothing Then
            If files.Count > 4 Then
                Return False
            Else
                Return True
            End If
        Else
            Return True
        End If
    End Function
End Class
