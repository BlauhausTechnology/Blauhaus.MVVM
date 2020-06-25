using System;
using Blauhaus.TestHelpers.MockBuilders;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Blauhaus.MVVM.TestHelpers.MockBuilders.Services
{
    public class ServiceProviderMockBuilder : BaseMockBuilder<ServiceProviderMockBuilder, IServiceProvider>
    {
        public ServiceProviderMockBuilder Where_GetService_returns<T>(T value)
        {
            Mock.Setup(x => x.GetService<T>()).Returns(value);
            return this;
        }

        public ServiceProviderMockBuilder Where_GetRequiredService_returns<T>(T value)
        {
            Mock.Setup(x => x.GetRequiredService<T>()).Returns(value);
            return this;
        }

        public ServiceProviderMockBuilder Where_GetService_returns(object value, Type type = null)
        {
            if (type == null)
            {
                Mock.Setup(x => x.GetService(It.IsAny<Type>())).Returns(value);
            }
            else
            {
                Mock.Setup(x => x.GetService(type)).Returns(value);
            }
            return this;
        }

        public ServiceProviderMockBuilder Where_GetRequiredService_returns(object value, Type type = null)
        {
            if (type == null)
            {
                Mock.Setup(x => x.GetRequiredService(It.IsAny<Type>())).Returns(value);
            }
            else
            {
                Mock.Setup(x => x.GetRequiredService(type)).Returns(value);
            }
            return this;
        }


        public void Verify_GetService_was_called_with_Type(Type type)
        {
            Mock.Verify(x => x.GetService(type));
        }

    }
}