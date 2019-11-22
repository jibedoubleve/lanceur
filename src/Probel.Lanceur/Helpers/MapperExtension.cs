using AutoMapper;
using Probel.Lanceur.Core.Entities;
using Probel.Lanceur.Core.Entities.Settings;
using Probel.Lanceur.Models;
using Probel.Lanceur.Models.Settings;
using System.Collections.Generic;

namespace Probel.Lanceur.Helpers
{
    public static class MapperExtension
    {
        #region Properties

        private static IMapper Mapper => new AutoMapperConfigurator().GetMapper();

        #endregion Properties

        #region Methods

        private static IEnumerable<TEntity> AsEntity<TModel, TEntity>(this IEnumerable<TModel> src) => Mapper.Map<IEnumerable<TModel>, IEnumerable<TEntity>>(src);

        private static IEnumerable<TModel> AsModel<TEntity, TModel>(this IEnumerable<TEntity> src) => Mapper.Map<IEnumerable<TEntity>, IEnumerable<TModel>>(src);

        public static IEnumerable<ShortcutName> AsEntity(this IEnumerable<ShortcutNameModel> src) => AsEntity<ShortcutNameModel, ShortcutName>(src);

        public static Shortcut AsEntity(this ShortcutModel src) => Mapper.Map<ShortcutModel, Shortcut>(src);

        public static AppSettings AsEntity(this AppSettingsModel src) => Mapper.Map<AppSettingsModel, AppSettings>(src);

        public static IEnumerable<Shortcut> AsEntity(this IEnumerable<ShortcutModel> src) => AsEntity<ShortcutModel, Shortcut>(src);

        public static IEnumerable<AppSettings> AsEntity(this IEnumerable<AppSettingsModel> src) => AsEntity<AppSettingsModel, AppSettings>(src);

        public static ShortcutSession AsEntity(this ShortcutSessionModel src) => Mapper.Map<ShortcutSessionModel, ShortcutSession>(src);

        public static AppSettingsModel AsModel(this AppSettings src) => Mapper.Map<AppSettings, AppSettingsModel>(src);

        public static IEnumerable<ShortcutSessionModel> AsModel(this IEnumerable<ShortcutSession> src) => AsModel<ShortcutSession, ShortcutSessionModel>(src);

        public static ShortcutSessionModel AsModel(this ShortcutSession src) => Mapper.Map<ShortcutSession, ShortcutSessionModel>(src);

        public static ShortcutModel AsModel(this Shortcut src) => Mapper.Map<Shortcut, ShortcutModel>(src);

        public static ShortcutNameModel AsModel(this ShortcutName src) => Mapper.Map<ShortcutName, ShortcutNameModel>(src);

        public static IEnumerable<ShortcutNameModel> AsModel(this IEnumerable<ShortcutName> src) => AsModel<ShortcutName, ShortcutNameModel>(src);

        public static IEnumerable<ShortcutModel> AsModel(this IEnumerable<Shortcut> src) => AsModel<Shortcut, ShortcutModel>(src);

        public static IEnumerable<AppSettingsModel> AsModel(this IEnumerable<AppSettings> src) => AsModel<AppSettings, AppSettingsModel>(src);

        #endregion Methods
    }
}