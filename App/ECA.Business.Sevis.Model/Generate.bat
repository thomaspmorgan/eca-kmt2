SET exchangeVisitorVersion=6.27
SET transactionLogVersion=6.23

"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\x64\xsd.exe" ".\Schemas\%exchangeVisitorVersion%\Common.xsd" ".\Schemas\%exchangeVisitorVersion%\sevistable.xsd" ".\Schemas\%exchangeVisitorVersion%\create-updateexchangevisitor.xsd" /c /out:"." /namespace:"ECA.Business.Sevis.Model"
"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\x64\xsd.exe" ".\Schemas\%transactionLogVersion%\Common.xsd" ".\Schemas\%transactionLogVersion%\sevistable.xsd" ".\Schemas\%transactionLogVersion%\sevistranslog.xsd" /c /out:"." /namespace:"ECA.Business.Sevis.Model.TransLog"