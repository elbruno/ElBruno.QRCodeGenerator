using System.CommandLine;
using ElBruno.QRCodeGenerator.Tool;

var rootCommand = GenerateCommand.Create();
return await rootCommand.InvokeAsync(args);
