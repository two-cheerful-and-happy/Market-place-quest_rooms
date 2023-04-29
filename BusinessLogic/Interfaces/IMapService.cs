namespace BusinessLogic.Interfaces;

public interface IMapService
{
    Task<List<Location>> GetLocationsAsync();
    

}
