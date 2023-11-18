using Autofac;
using S3.WEB.Models;

namespace S3.WEB
{
    public class WebModule : Autofac.Module
    {
        public WebModule()
        { }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<S3BucketItemListModel>().AsSelf()
            .InstancePerLifetimeScope();
            builder.RegisterType<S3BucketItemCreateModel>().AsSelf()
  .InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}
