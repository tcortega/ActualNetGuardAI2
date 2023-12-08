using NetGuardAI.Masscan;
using NetGuardAI.Nmap;

// var nmap = await NmapWrapper.CreateAsync();
// var result = await nmap.ScanAsync("192.168.100.113", 5173);
// Console.WriteLine(result.Success);

var masscanWrapper = new MasscanWrapper();
var result = await masscanWrapper.ScanAsync(OnResult, "192.168.100.0/24", 5173, 5173, 100);

Console.WriteLine(result.Success);
return;

Task OnResult(MasscanServer server)
{
    Console.WriteLine(server.Ip);
    return Task.CompletedTask;
}