namespace ApiTalking.DTOs.common;
public class ResponseDTO
{
    public bool sucess { get; set; } = false;
    public string message { get; set; } = "";

    public object data { get; set; }
}
