using SummitDiary.Core.Models.Common;

namespace SummitDiary.Core.Services.Interfaces;

public interface IGpxAnalyzer
{
    AnalysisResult AnalyzePath(List<Waypoint> waypoints);
    AnalysisResult AnalyzeGpx(Stream gpxFile, int compressValue = 1);
}
