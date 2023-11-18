using Autofac;
using S3.Application.Features.Services;
using S3.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3.Infrastructure
{
    public class InfrastructureModule : Module
    {
        public InfrastructureModule()
        {
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<S3Service>().As<IS3BucketService>()
                .InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}
