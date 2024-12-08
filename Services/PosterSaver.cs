namespace BookHive.Services;

public static class PosterSaver
{
    public static async Task<string> SavePoster(IFormFile file)
    {
        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        var localFilePath = Path.Join("\\images", fileName);
        var filePath = Path.Join(Directory.GetCurrentDirectory(), "wwwroot", localFilePath);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        return localFilePath;
    }
}