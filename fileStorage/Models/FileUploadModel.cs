using Microsoft.AspNetCore.Mvc;

public class FileUploadModel
{
    [FromForm(Name = "file")]
    public IFormFile File { get; set; }
}
