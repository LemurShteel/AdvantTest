using System.Collections.Generic;

namespace Providers
{
    public interface IDescriptionsProvider
    {
        Dictionary<int, BusinessDescription> GetBusinessDescriptions();
    }
}
