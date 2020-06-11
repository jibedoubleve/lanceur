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

        public static IEnumerable<AliasName> AsEntity(this IEnumerable<AliasNameModel> src) => AsEntity<AliasNameModel, AliasName>(src);

        public static Alias AsEntity(this AliasModel src) => Mapper.Map<AliasModel, Alias>(src);

        public static AppSettings AsEntity(this AppSettingsModel src) => Mapper.Map<AppSettingsModel, AppSettings>(src);

        public static IEnumerable<Alias> AsEntity(this IEnumerable<AliasModel> src) => AsEntity<AliasModel, Alias>(src);

        public static IEnumerable<AppSettings> AsEntity(this IEnumerable<AppSettingsModel> src) => AsEntity<AppSettingsModel, AppSettings>(src);

        public static AliasSession AsEntity(this AliasSessionModel src) => Mapper.Map<AliasSessionModel, AliasSession>(src);

        public static AppSettingsModel AsModel(this AppSettings src) => Mapper.Map<AppSettings, AppSettingsModel>(src);

        public static IEnumerable<AliasSessionModel> AsModel(this IEnumerable<AliasSession> src) => AsModel<AliasSession, AliasSessionModel>(src);

        public static AliasSessionModel AsModel(this AliasSession src) => Mapper.Map<AliasSession, AliasSessionModel>(src);

        public static AliasModel AsModel(this Alias src) => Mapper.Map<Alias, AliasModel>(src);

        public static AliasNameModel AsModel(this AliasName src) => Mapper.Map<AliasName, AliasNameModel>(src);

        public static IEnumerable<AliasNameModel> AsModel(this IEnumerable<AliasName> src) => AsModel<AliasName, AliasNameModel>(src);

        public static IEnumerable<AliasModel> AsModel(this IEnumerable<Alias> src) => AsModel<Alias, AliasModel>(src);

        public static IEnumerable<AppSettingsModel> AsModel(this IEnumerable<AppSettings> src) => AsModel<AppSettings, AppSettingsModel>(src);

        public static IEnumerable<SwitchSessionResult> AsSwitchSessionResult(this IEnumerable<AliasSession> src) => Mapper.Map<IEnumerable<AliasSession>, IEnumerable<SwitchSessionResult>>(src);

        private static IEnumerable<TEntity> AsEntity<TModel, TEntity>(this IEnumerable<TModel> src) => Mapper.Map<IEnumerable<TModel>, IEnumerable<TEntity>>(src);

        private static IEnumerable<TModel> AsModel<TEntity, TModel>(this IEnumerable<TEntity> src) => Mapper.Map<IEnumerable<TEntity>, IEnumerable<TModel>>(src);

        #endregion Methods
    }
}