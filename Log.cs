namespace Hv.Sos100.Logger;

internal class Log
{
    internal int Id { get; set; }
    internal DateTime? TimeStamp { get; set; }
    internal string? SourceSystem { get; set; }
    internal string? Message { get; set; }
}