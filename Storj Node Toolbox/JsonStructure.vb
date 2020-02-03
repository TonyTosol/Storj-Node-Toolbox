
Imports Newtonsoft.Json


Public Class Nodes
    <JsonProperty("UserID")>
    Public Property UserID As String
    <JsonProperty("LiveNodes")>
    Public Property LiveNodeCount As Integer
    <JsonProperty("UnicID")>
    Public Property UnicID As String
    <JsonProperty("Nodes")>
    Public Property Nodes As Node()



End Class
Public Class Node
    <JsonProperty("Name")>
    Public Property Name As String
    <JsonProperty("Status")>
    Public Property Status As String
    <JsonProperty("TotalBandwidth")>
    Public Property TotalBandwidth As String
    <JsonProperty("EgressBandwidth")>
    Public Property EgressBandwidth As String
    <JsonProperty("IngressBandwidth")>
    Public Property IngressBandwidth As String
    <JsonProperty("storageDaily")>
    Public Property storageDaily As String
End Class