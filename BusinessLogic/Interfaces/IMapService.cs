﻿namespace BusinessLogic.Interfaces;

public interface IMapService
{
    Task<List<Location>> GetLocationsAsync();
    
    Task<BaseResponse<Location>> AddNewLocationAsync(Location location);

}