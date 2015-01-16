using System;
using System.Web.Optimization;

[assembly: WebActivatorEx.PostApplicationStartMethod(
    typeof(DurandalTest.App_Start.DurandalConfig), "PreStart")]

namespace DurandalTest.App_Start
{
    public static class DurandalConfig
    {
        public static void PreStart()
        {
            // Add your start logic here
            DurandalBundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}