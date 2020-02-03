Imports System.Runtime.CompilerServices

Public Module MyExtensions
    <Extension()>
    Public Sub AddItemToArray(Of T)(ByRef arr As T(), item As T)
        If arr IsNot Nothing Then
            Array.Resize(arr, arr.Length + 1)
            arr(arr.Length - 1) = item
        Else
            Dim newarr(0) As T
            newarr(0) = item
            arr = newarr
        End If
    End Sub
End Module
