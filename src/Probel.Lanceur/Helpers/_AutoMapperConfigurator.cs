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
            e.CreateMap<Shortcut, ShortcutModel>();
            e.CreateMap<ShortcutName, ShortcutNameModel>();

            //Settings
            e.CreateMap<AppSettings, AppSettingsModel>();
            e.CreateMap<WindowSettings, WindowSettingsModel>()
                .ForMember(src => src.ColourRed, opt => opt.Ignore())
                .ForMember(src => src.ColourGreen, opt => opt.Ignore())
                .ForMember(src => src.ColourBlue, opt => opt.Ignore());
            e.CreateMap<PositionSettings, PositionSettingsModel>();
            e.CreateMap<HotKeySettings, HotKeySettingsModel>();
            e.CreateMap<ShortcutSession, ShortcutSessionModel>();
            e.CreateMap<ShortcutSession, ShortcutSessionModel>();
        }

        private void ModelToEntity(IMapperConfigurationExpression e)
        {
            e.CreateMap<ShortcutModel, Shortcut>();
            e.CreateMap<ShortcutNameModel, ShortcutName>();

            //Settings
            e.CreateMap<AppSettingsModel, AppSettings>();
            e.CreateMap<WindowSettingsModel, WindowSettings>();
            e.CreateMap<PositionSettingsModel, PositionSettings>();
            e.CreateMap<HotKeySettingsModel, HotKeySettings>();
            e.CreateMap<ShortcutSessionModel, ShortcutSession>();
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