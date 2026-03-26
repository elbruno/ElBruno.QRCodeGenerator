using ElBruno.QRCodeGenerator.CLI;
using ElBruno.QRCodeGenerator.Payloads;

Console.WriteLine("=== QR Code Payloads Demo ===");
Console.WriteLine();

// WiFi payload
Console.WriteLine("WiFi Network QR Code:");
var wifi = PayloadBuilder.Wifi("MyHomeNetwork", "SuperSecret123")
    .WithHidden(false);
Console.WriteLine($"Payload: {wifi.GetPayloadString()}");
QRCode.Print(wifi.GetPayloadString());

Console.WriteLine();

// vCard payload
Console.WriteLine("vCard Contact QR Code:");
var vcard = PayloadBuilder.VCard("Bruno Capuano")
    .WithPhone("+1234567890", VCardPhoneType.Mobile)
    .WithEmail("bruno@example.com", VCardEmailType.Work)
    .WithOrganization("Contoso")
    .WithUrl("https://inthelabs.dev");
Console.WriteLine($"Payload:\n{vcard.GetPayloadString()}");
QRCode.Print(vcard.GetPayloadString());

Console.WriteLine();

// Email payload
Console.WriteLine("Email QR Code:");
var email = PayloadBuilder.Email("hello@example.com")
    .WithSubject("Meeting Tomorrow")
    .WithBody("Let's meet at 10 AM.");
Console.WriteLine($"Payload: {email.GetPayloadString()}");
QRCode.Print(email.GetPayloadString());

Console.WriteLine();
Console.WriteLine("Demo complete!");
