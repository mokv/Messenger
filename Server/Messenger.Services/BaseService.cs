using Messenger.Data;

namespace Messenger.Services
{
    public abstract class BaseService
    {
        protected MessengerEntities context;

        public BaseService()
        {
            this.context = new MessengerEntities();
        }
    }
}
