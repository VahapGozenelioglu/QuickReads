namespace QuickReads.Entities;

public class ApiResponseModel<T>
{
    public bool Error { get; set; }
    public string Message { get; set; } 
    public int StatusCode { get; set; }
    public int ErrorCode { get; set; } = 0;
    public T? Result { get; set; }
}