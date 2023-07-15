namespace ASD.Dto;

public class RequestBaseModel
{
    public string Url { get; set; }
    public string Method { get; set; }
    public string ApiKey { get; set; }
    public string Data { get; set; }
}