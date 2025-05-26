public class MinioOption
{
    public string Endpoint { get; set; } = string.Empty;
    public string AccessKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public string BucketName { get; set; } = string.Empty;
    public string ObjectName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string DownloadFilePath { get; set; } = string.Empty;

    public bool UseSSL { get; set; }
}