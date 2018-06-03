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

        private const string OverheadMessage = "After adding the provided packages we were left with {0} packages too much.\nPackage dimentions:{1}x{2}";
        private const string TooLowMessage = "";

        public static void SubscribeToQuantityOverhead(Action<string, Package> action)
        {
            if (Actions.TryGetValue(MessageType.QuantityOverhead, out List<Subscription> actionList))
            {
                actionList.Add(new Subscription()
                {
                    Action = action,
                    Message = OverheadMessage
                });
            }
            else
            {
                actionList = new List<Subscription>
                {
                    new Subscription()
                    {
                        Action = action,
                        Message = OverheadMessage
                    }
                };
                Actions.Add(MessageType.QuantityOverhead, actionList);
            }
        }

        public static void SubscribeToQuantityToLow(Action<string, Package> action)
        {
            if (Actions.TryGetValue(MessageType.PackageQuanityLow, out List<Subscription> actionList))
            {
                actionList.Add(new Subscription()
                {
                    Action = action,
                    Message = OverheadMessage
                });
            }
            else
            {
                actionList = new List<Subscription>
                {
                    new Subscription()
                    {
                        Action = action,
                        Message = OverheadMessage
                    }
                };
                Actions.Add(MessageType.QuantityOverhead, actionList);
            }
        }

        public static void Publish(MessageType messageType, Package package, int numOffPackagesTooMuch = 0)
        {
            if (Actions.ContainsKey(messageType))
            {
                foreach (Subscription subscription in Actions[messageType])
                {
                    var msg = subscription.Message;
                    if (numOffPackagesTooMuch > 0)
                    {
                        msg = string.Format(msg, numOffPackagesTooMuch, package.Width, package.Height);
                    }
                    subscription.Action.Invoke(msg, package);
                }
            }
        }
        internal enum MessageType
        {
            QuantityOverhead, PackageQuanityLow
        }

        internal class Subscription
        {
            public Action<string, Package> Action { get; set; }
            public string Message { get; set; }
        }
    }
}
