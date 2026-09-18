$asm = [Reflection.Assembly]::LoadFrom("C:\Users\micha\.nuget\packages\communitytoolkit.maui\9.0.1\lib\net8.0-android34.0\CommunityToolkit.Maui.dll")
$type = $asm.GetTypes() | Where-Object Name -eq 'IPopupService'
$type.GetMethods() | ForEach-Object {
    $paramList = $_.GetParameters() | ForEach-Object { $_.ParameterType.Name + ' ' + $_.Name }
    $paramStr = [string]::Join(', ', $paramList)
    Write-Output ($_.Name + '(' + $paramStr + ')')
}
