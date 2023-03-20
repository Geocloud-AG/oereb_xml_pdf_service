:: xsd.exe /c /language:CS "Versioning.xsd"
:: pause
xsd.exe Versioning.xsd Extract.xsd ExtractData.xsd geometry.xsd xmldsig-core-schema.xsd /c /language:CS
pause
:: xsd.exe /c /language:CS "ExtractData.xsd"
:: pause