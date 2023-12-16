namespace n5.WebApi.Model;

public class RequestPermissionResult
{
    public bool Access { get; set; }
    public ResultData ResultData { get; set; } = new ResultData();
}
