for ($i = 0; $i -lt 1000; $i++) {
    Get-AzCommandNotFound
}

ipmo "C:\Users\yeliu\code\test-psmodule\bin\Debug\netstandard2.0\psmodule.dll"
[psmodule.Utilities.NotFoundAction]::RegisterCommandNotFoundAction($ExecutionContext.InvokeCommand)

for ($i = 0; $i -lt 1000; $i++) {
    Get-AzCommandNotFound
}
