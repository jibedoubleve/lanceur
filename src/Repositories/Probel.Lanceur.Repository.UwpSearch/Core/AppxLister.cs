using Probel.Lanceur.Infrastructure;
using Probel.Lanceur.Infrastructure.Helpers;
using Probel.Lanceur.Infrastructure.PackagedApp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using Windows.Management.Deployment;

namespace Probel.Lanceur.Repository.UwpSearch.Core
{
    public class AppxLister
    {

        #region Properties

        public ILogService Log { get; internal set; }

        #endregion Properties

        #region Methods

        public IEnumerable<UwpApp> All()
        {
            var u = WindowsIdentity.GetCurrent().User;

            if (u != null)
            {
                var id = u.Value;
                var m = new PackageManager();
                var ps = m.FindPackagesForUser(id);
                ps = ps.Where(p =>
                {
                    bool valid;
                    try
                    {
                        var f = p.IsFramework;
                        var path = p.InstalledLocation.Path;
                        valid = !f && !string.IsNullOrEmpty(path);
                    }
                    catch (Exception e)
                    {
                        Trace.WriteLine($"An unexpected error occurred and unable to verify if package is valid [{e}]");
                        return false;
                    }

                    return valid;
                });

                var factory = new UwpAppFactory(Log ?? new TraceLog());
                if (_cache == null)
                {
                    _cache = (from s in ps.Select(e => factory.Create(e))
                              where s.IsEmpty == false
                              select s).ToList();
                }
                return _cache;
            }
            else
            {
                return new UwpApp[] { };
            }
        }

        private IEnumerable<UwpApp> _cache;

        #endregion Methods
    }
}