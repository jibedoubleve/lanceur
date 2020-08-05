using AutoMapper;
using Probel.Lanceur.Core.Entities;
using Probel.Lanceur.Core.Entities.Settings;
using Probel.Lanceur.Models;
using Probel.Lanceur.Models.Settings;

namespace Probel.Lanceur.Helpers
{
    public class AutoMapperConfigurator
    {
        #region Fields

        private static IMapper _mapper;

        #endregion Fields

        #region Methods

        private void EntityToModel(IMapperConfigurationExpression e)
        {
            e.CreateMap<Alias, AliasModel>();
            e.CreateMap<AliasName, AliasNameModel>();

            //Settings
            e.CreateMap<AppSettings, AppSettingsModel>();
            e.CreateMap<WindowSettings, WindowSettingsModel>();
            e.CreateMap<PositionSettings, PositionSettingsModel>();
            e.CreateMap<RepositorySettings, RepositorySettingsModel>();
            e.CreateMap<HotKeySettings, HotKeySettingsModel>();
            e.CreateMap<AliasSession, AliasSessionModel>();
            e.CreateMap<AliasSession, AliasSessionModel>();

            //Macros
            e.CreateMap<AliasSession, SwitchSessionResult>()
                .ForMember(src => src.Title, opt => opt.MapFrom(dest => dest.Name))
                .ForMember(src => src.Subtitle, opt => opt.MapFrom(dest => dest.Notes));
        }

        private void ModelToEntity(IMapperConfigurationExpression e)
        {
            e.CreateMap<AliasModel, Alias>();
            e.CreateMap<AliasNameModel, AliasName>();

            //Settings
            e.CreateMap<AppSettingsModel, AppSettings>();
            e.CreateMap<WindowSettingsModel, WindowSettings>();
            e.CreateMap<RepositorySettingsModel, RepositorySettings>();
            e.CreateMap<PositionSettingsModel, PositionSettings>();
            e.CreateMap<HotKeySettingsModel, HotKeySettings>();
            e.CreateMap<AliasSessionModel, AliasSession>();
        }

        public IMapper GetMapper()
        {
            if (_mapper == null)
            {
                var config = new MapperConfiguration(e =>
                {
                    EntityToModel(e);
                    //EntityToEntity(e);
                    ModelToEntity(e);
                });
                _mapper = config.CreateMapper();
            }
            return _mapper;
        }

        #endregion Methods
    }
}