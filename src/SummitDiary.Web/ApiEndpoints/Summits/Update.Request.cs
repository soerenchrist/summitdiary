namespace SummitDiary.Web.ApiEndpoints.Summits;

public class UpdateSummitRequest
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Height { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int CountryId { get; set; }
    public int RegionId { get; set; }
}