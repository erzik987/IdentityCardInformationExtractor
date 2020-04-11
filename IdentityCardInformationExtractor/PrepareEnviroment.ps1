 New-Item -Path ($PSScriptRoot + './bin/Debug/netcoreapp3.0') -Name "tessdata" -ItemType "directory"


$url = "https://api.github.com/repos/tesseract-ocr/tessdata/git/blobs/d8384a707dac23ef68011499c476b6deda41ec33"
$output = ($PSScriptRoot + './bin/Debug/netcoreapp3.0/tessdata')
$start_time = Get-Date

Import-Module BitsTransfer
Start-BitsTransfer -Source $url -Destination $output
#OR
Start-BitsTransfer -Source $url -Destination $output -Asynchronous

Write-Output "Time taken: $((Get-Date).Subtract($start_time).Seconds) second(s)"

# function DownloadFilesFromRepo {
#     Param(
#         [string]$Owner,
#         [string]$Repository,
#         [string]$Path,
#         [string]$DestinationPath
#         )
        
#         $baseUri = "https://api.github.com/"
#         $args = "repos/$Owner/$Repository/$Path"
#         $wr = Invoke-WebRequest -Uri $($baseuri + $args)
#         $objects = $wr.Content | ConvertFrom-Json
#         $files = $objects | where { $_.type -eq "file" } | Select -exp download_url
#         $directories = $objects | where { $_.type -eq "dir" }
        
#         $directories | ForEach-Object { 
#             DownloadFilesFromRepo -Owner $Owner -Repository $Repository -Path $_.path -DestinationPath $($DestinationPath + $_.name)
#         }
        
        
#         if (-not (Test-Path $DestinationPath)) {
#             # Destination path does not exist, let's create it
#             try {
#                 New-Item -Path $DestinationPath -ItemType Directory -ErrorAction Stop
#             }
#             catch {
#                 throw "Could not create path '$DestinationPath'!"
#             }
#         }
        
#         foreach ($file in $files) {
#             $fileDestination = Join-Path $DestinationPath (Split-Path $file -Leaf)
#             try {
#                 Invoke-WebRequest -Uri $file -OutFile $fileDestination -ErrorAction Stop -Verbose
#                 "Grabbed '$($file)' to '$fileDestination'"
#             }
#             catch {
#                 throw "Unable to download '$($file.path)'"
#             }
#         }
        
#     }
    
#     # https://api.github.com/repos/tesseract-ocr/tessdata/git/blobs/d8384a707dac23ef68011499c476b6deda41ec33
#     DownloadFilesFromRepo -Owner "tesseract-ocr" -Repository "tessdata" -Path "git/blobs/d8384a707dac23ef68011499c476b6deda41ec33" -DestinationPath ($PSScriptRoot + './bin/Debug/netcoreapp3.0/tessdata')
    
Get-Content -Path ($PSScriptRoot + './bin/Debug/netcoreapp3.0/tessdata/d8384a707dac23ef68011499c476b6deda41ec33')