using System.Threading.Tasks;

namespace NASADailyWallpaper.Client
{
    public interface INasaApodClient
    {
        Task<ApodResponse> GetLatestImage();
    }
}