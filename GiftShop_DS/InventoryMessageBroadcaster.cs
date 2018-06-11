using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftShop_DS
{

    internal static class InventoryMessageBroadcaster
    {

        private static readonly Dictionary<MessageType, List<Subscription>> Actions;

        static InventoryMessageBroadcaster() => Actions = new Dictionary<MessageType, List<Subscription>>();

        

        public static void SubscribeToQuantityOverhead(Action<Package> action)
        {
            if (Actions.TryGetValue(MessageType.QuantityOverhead, out List<Subscription> actionList))
            {
                actionList.Add(new Subscription()
                {                    
                    Action = action
                });
            }
            else
            {
                actionList = new List<Subscription>
                {
                    new Subscription()
                    {
                        Action = action,                       
                    }
                };
                Actions.Add(MessageType.QuantityOverhead, actionList);
            }
        }

        public static void SubscribeToQuantityToLow(Action<Package> action)
        {
            if (Actions.TryGetValue(MessageType.PackageQuanityLow, out List<Subscription> actionList))
            {
                actionList.Add(new Subscription()
                {
                    Action = action,                    
                });
            }
            else
            {
                actionList = new List<Subscription>
                {
                    new Subscription()
                    {
                        Action = action,                        
                    }
                };
                Actions.Add(MessageType.PackageQuanityLow, actionList);
            }
        }

        public static void Publish(MessageType messageType, Package package, int numOffPackagesTooMuch = 0)
        {
            if (Actions.ContainsKey(messageType))
            {
                foreach (Subscription subscription in Actions[messageType])
                {                    
                    var copy = new Package()
                    {
                        Width = package.Width,
                        Height = package.Height,
                        Count = numOffPackagesTooMuch
                    };
                    subscription.Action.Invoke(copy);
                }
            }
        }
        internal enum MessageType
        {
            QuantityOverhead, PackageQuanityLow
        }

        internal class Subscription
        {
            public Action<Package> Action { get; set; }
            public string Message { get; set; }
        }
    }
}
