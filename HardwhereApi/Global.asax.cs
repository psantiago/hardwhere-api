using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AutoMapper;
using HardwhereApi.Core.Dto;
using HardwhereApi.Core.Models;
using HardwhereApi.Infrastructure;
using ValueType = HardwhereApi.Core.Models.ValueType;

namespace HardwhereApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Database.SetInitializer(new HardwhereApiInitializer());

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Mapper.CreateMap<TypeProperty, TypePropertyDto>();
            Mapper.CreateMap<AssetType, AssetTypeDto>();
            Mapper.CreateMap<GoodAsset, GoodAssetDto>();
            Mapper.CreateMap<User, UserDto>();
            Mapper.CreateMap<Section, SectionDto>();
            Mapper.CreateMap<ValueType, ValueTypeDto>();
        }
    }
}
