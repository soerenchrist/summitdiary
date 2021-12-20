﻿namespace SummitDiary.Core.Common.Interfaces
{
    public interface IElevationService
    {
        Task<double?> GetElevation(double latitude, double longitude, CancellationToken cancellationToken = default);
    }
}