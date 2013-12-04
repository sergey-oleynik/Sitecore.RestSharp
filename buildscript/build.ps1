
if (-not (test-path "$env:ProgramFiles\7-Zip\7z.exe")) {throw "$env:ProgramFiles\7-Zip\7z.exe needed"} 
set-alias sz "$env:ProgramFiles\7-Zip\7z.exe"

properties {
	#Incoming
	#$buildKey = "Empty_Build_Key"
	$assemblyVersion = $null
	$distributivePath = $null
	
	#Internal
	$localStorage = "C:\LocalStorage"
	
	$buildFolder = Resolve-Path .. 
	
	$referencesFolder = "$buildFolder\References"
}

task Init {
	Assert ($distributivePath -ne $null) "distributivePath should not be null"  
	Assert ($distributivePath -ne "") "distributivePath should not be empty"  

	$distributiveName = [System.IO.Path]::GetFileNameWithoutExtension($distributivePath)
	
    if (-not (Test-Path $localStorage)) {
        New-Item $localStorage -type directory  -Verbose
    }

	$zipFile = "$localStorage\$distributiveName.zip"
	
    if (-not (Test-Path $zipFile)) {
        Copy-Item $distributivePath $zipFile -Verbose
    }
    
    if (-not (Test-Path $localStorage\$distributiveName)) {
        sz x -y  "-o$localStorage" $zipFile "$distributiveName/Website"
        sz x -y  "-o$localStorage" $zipFile "$distributiveName/Data"
		sz x -y  "-o$localStorage" $zipFile "$distributiveName/Databases"
    }
}

task InitBuild -depends Init { 
	Assert ($distributivePath -ne $null) "distributivePath should not be null"  
	Assert ($distributivePath -ne "") "distributivePath should not be empty" 

	if (-not (Test-Path $buildFolder\Website\bin)) {
        New-Item $buildFolder\Website\bin -type directory  -Verbose
    }
	
	$distributiveName = [System.IO.Path]::GetFileNameWithoutExtension($distributivePath)
	
	robocopy $localStorage\$distributiveName\Website\bin $referencesFolder /E /XC /XN /XO | Out-Null
}