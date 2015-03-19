using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using Autofac.Core;
using Bookmarky.DAL.EntityModels;
using Bookmarky.DAL.Service;
using Bookmarky.DAL.ServiceImplementations;

namespace Bookmarky.WebAPI.App_Start.Modules
{
    public class BookmarkDataModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BookmarkyContext>().As<IBookmarkyContext>();
            builder.RegisterType<EFBookmarkDataService>().As<IBookmarkDataService>();
        }
    }
}