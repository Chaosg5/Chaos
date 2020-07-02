//-----------------------------------------------------------------------
// <copyright file="BundleConfig.cs" company="EVRY">
//   Copyright (c) EVRY. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Wedding
{
    using System.Web.Optimization;

    /// <summary>Class configuring bundles</summary>
    public class BundleConfig
    {
        /// <summary>Register bundles</summary>
        /// <param name="bundles">The <see cref="BundleCollection"/> of bundles to register</param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js", "~/Scripts/bootstrap-notify.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/fontawesome.css",
                      "~/Content/brands.css",
                      "~/Content/regular.css",
                      "~/Content/solid.css",
                      "~/Content/svg-with-js.css",
                      "~/Content/v4-shims.css"));
        }
    }
}
