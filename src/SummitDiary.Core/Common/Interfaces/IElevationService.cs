using System.Threading.Tasks;

namespace SummitDiary.Core.Common.Interfaces
{
    public interface IElevationService
    {
        Task<double?> GetElevation(double latitude, double longitude);
    }
}