echo "Fetching Dependencies"

Try
{
    $targetFolder = Join-Path $pwd "Media"
    $target = Join-Path $pwd "Media.zip"
    (New-Object System.Net.WebClient).DownloadFile("https://github.com/cookgreen/AMGE/releases/download/alpha-0.1.1/Media.zip", $target)
    if(!(Test-Path $targetFolder))
    {
        mkdir $targetFolder
    }
    $shell = New-Object -ComObject Shell.Application
    $zipFile = $shell.NameSpace($target).Items()
    $shell.NameSpace($targetFolder).CopyHere($zipFile)
}
Catch [System.Exception]
{
    echo $Error[0];
}
Catch
{
    echo "fetch error";
}
pause

