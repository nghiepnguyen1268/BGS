namespace Application.DTOs.Response
{
    public record GeneralResponse(bool Success=false, int Status=200, string Message=null!);
}