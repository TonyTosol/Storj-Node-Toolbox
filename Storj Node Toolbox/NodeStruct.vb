Imports Newtonsoft.Json
Public Class NodeStruct
    <JsonProperty("Nodes")>
    Public Property Nodes As NodeProp()
End Class
Public Class NodeProp
    <JsonProperty("Name")>
    Public Property Name As String
    <JsonProperty("Ip")>
    Public Property IP As String
    <JsonProperty("Port")>
    Public Property Port As String
    <JsonProperty("Path")>
    Public Property Path As String
    <JsonProperty("ServiceName")>
    Public Property ServiceName As String
    <JsonProperty("MainNode")>
    Public Property MainNode As Boolean
    <JsonProperty("ServiceStatus")>
    Public Property ServiceStatus As Boolean
End Class