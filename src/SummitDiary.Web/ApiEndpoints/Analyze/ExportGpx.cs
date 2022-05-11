using System.Collections.Immutable;
using System.Text;
using System.Xml;
using NetTopologySuite.IO;
using SummitDiary.Core.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace SummitDiary.Web.ApiEndpoints.Analyze;

public class ExportGpx : EndpointBaseAsync
    .WithRequest<ExportGpxRequest>
    .WithoutResult
{
    private readonly IElevationService _elevationService;

    public ExportGpx(IElevationService elevationService)
    {
        _elevationService = elevationService;
    }
    
    [HttpPost("/api/gpx/generateGpx")]
    [SwaggerOperation(
        Summary = "Create a gpx file from a path",
        Description = "Create a gpx file from a path",
        OperationId = "Gpx.ExportGpx",
        Tags = new[]{"GpxEndpoints"})]
    public override async Task<ActionResult> HandleAsync(ExportGpxRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
        var gpxWaypoints = new List<GpxWaypoint>();

        foreach (var waypoint in request.Points)
        {
            var elevation = await _elevationService.GetElevation(waypoint.Latitude, waypoint.Longitude, cancellationToken);
                
            gpxWaypoints.Add(new GpxWaypoint(new GpxLongitude(waypoint.Longitude),
                new GpxLatitude(waypoint.Latitude), elevation));
        }
            
        var file = new GpxFile
        {
            Metadata = BuildMetadata()
        };
        file.Tracks.AddRange(BuildTracks(gpxWaypoints));

        var memoryStream = new MemoryStream();
        await using (var xmlWriter = new XmlTextWriter(memoryStream, Encoding.UTF8))
        {
            file.WriteTo(xmlWriter, new GpxWriterSettings
            {
                TimeZoneInfo = TimeZoneInfo.Local
            });
        }
        var bytes = memoryStream.ToArray();
        return File(bytes, "application/xml+gpx", "Tour.gpx");

    }
    
    private List<GpxTrack> BuildTracks(IEnumerable<GpxWaypoint> gpxWaypoints)
    {
        var track = new GpxTrack("", "", "", "", ImmutableArray<GpxWebLink>.Empty, 1, "", null,
            BuildSegments(gpxWaypoints));

        return new List<GpxTrack>
        {
            track
        };
    }

    private ImmutableArray<GpxTrackSegment> BuildSegments(IEnumerable<GpxWaypoint> gpxWaypoints)
    {
        var segment = new GpxTrackSegment(new ImmutableGpxWaypointTable(gpxWaypoints), null);
        return ImmutableArray.Create(segment);
    }

    private GpxMetadata BuildMetadata()
    {
        var name = "GipfelStürmer";
        return new GpxMetadata(name, "Tour", "", null, new GpxCopyright(name),
            ImmutableArray<GpxWebLink>.Empty, DateTime.Now.ToUniversalTime(), "gpx", null, null);
    }
}