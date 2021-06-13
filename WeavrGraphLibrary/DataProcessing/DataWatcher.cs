using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace WeavrGraphLibrary.DataProcessing
{
    public class DataWatcher
    {
        class AvailabilityRequest
        {
            public UIElement Requester { get; set; }
            public object[] Tokens { get; set; }
            public Action Action { get; set; }
        }

        class DataSubscription
        {
            public UIElement Subscriber { get; set; }
            public object Data { get; set; }
            public Action Action { get; set; }
        }

        static Dictionary<object, bool> availableDataTokens = new Dictionary<object, bool>();
        static Dictionary<object, List<DataSubscription>> subscriptions = new Dictionary<object, List<DataSubscription>>();
        static List<AvailabilityRequest> availabilityRequests = new List<AvailabilityRequest>();

        public static void Subscribe(UIElement subscriber, object data, Action action)
        {
            DataSubscription subscription = new DataSubscription() { Subscriber = subscriber, Data = data, Action = action };
            if (!subscriptions.ContainsKey(data))
            {
                subscriptions.Add(data, new List<DataSubscription>());
            }
            subscriptions[data].Add(subscription);
        }

        public static void Trigger(object data)     
        {
            if (subscriptions.ContainsKey(data))
            {
                foreach (DataSubscription subscription in subscriptions[data])
                {
                    subscription.Subscriber.Dispatcher.Invoke(subscription.Action);
                }
            }
        }

        public static void RequestAvailabilityNotification(UIElement requester, object[] dataAvailabilityTokens, Action action)
        {
            AvailabilityRequest request = new AvailabilityRequest() { Requester = requester, Tokens = dataAvailabilityTokens, Action = action };
            if (!CheckAndTriggerAvailibility(request))
            {
                availabilityRequests.Add(request);
            }
        }

        public static void NotifyAvailability(object dataToken)
        {
            if (!availableDataTokens.ContainsKey(dataToken))
            {
                availableDataTokens.Add(dataToken, true);
            }
            List<AvailabilityRequest> requestsToBeDeleted = new List<AvailabilityRequest>();
            for (int i = 0; i < availabilityRequests.Count; i++)
            {
                if (CheckAndTriggerAvailibility(availabilityRequests[i]))
                {
                    requestsToBeDeleted.Add(availabilityRequests[i]);
                }
            }
            foreach (AvailabilityRequest requestToBeDeleted in requestsToBeDeleted)
            {
                availabilityRequests.Remove(requestToBeDeleted);
            }
        }

        static bool CheckAndTriggerAvailibility(AvailabilityRequest request)
        {
            bool allDataTokensAvailable = request.Tokens.Length > 0; // set to fals if token list is empty, otherwise to true
            for (int j = 0; j < request.Tokens.Length; j++)
            {
                if (!availableDataTokens.ContainsKey(request.Tokens[j]))
                {
                    allDataTokensAvailable = false;
                }
            }
            if (allDataTokensAvailable)
            {
                request.Requester.Dispatcher.Invoke(request.Action);
            }
            return allDataTokensAvailable;
        }
    }
}
