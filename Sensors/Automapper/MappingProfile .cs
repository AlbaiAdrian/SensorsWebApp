using AutoMapper;
using Entities;
using Sensors.Models.Groups;
using Sensors.Models.Sensors;
using Sensors.Models.Zones;

namespace Sensors.Automapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateGroupDTO, Group>();
        CreateMap<Group, DefaultInfoGroupDTO>();
        
        CreateMap<CreateZoneDTO, Zone>();
        CreateMap<Zone, DefaultInfoZoneDTO>();
        CreateMap<Zone, EditZoneDTO>();

        CreateMap<CreateSensorDTO, Sensor>();
        CreateMap<Sensor, DefaultInfoSensorDTO>();
        CreateMap<Sensor, EditSensorDTO>();
    }
}
