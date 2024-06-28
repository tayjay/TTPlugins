using System.Collections.Generic;
using Exiled.API.Features;
using TTAdmin.Data.Events;
using TTAdmin.WebSocketServer;

namespace TTAdmin.Handlers;

public class SubscriptionHandler
{
    public Dictionary<Client,List<string>> Subscriptions = new Dictionary<Client, List<string>>();
    
    public void Subscribe(Client client, string subscription)
    {
        if (!Subscriptions.ContainsKey(client))
        {
            Subscriptions.Add(client, new List<string>());
        }
        Subscriptions[client].Add(subscription);
    }
    
    public void Subscribe(Client client, List<string> subscriptions)
    {
        if (!Subscriptions.ContainsKey(client))
        {
            Subscriptions.Add(client, new List<string>());
        }
        Subscriptions[client].AddRange(subscriptions);
    }
    
    public void Subscribe(Client client, List<object> subscriptions)
    {
        if (!Subscriptions.ContainsKey(client))
        {
            Subscriptions.Add(client, new List<string>());
        }
        foreach (var subscription in subscriptions)
        {
            Log.Debug($"{client.ToString()} subscribed to {subscription.ToString()}");
            Subscriptions[client].Add(subscription.ToString());
        }
    }
    
    public void Unsubscribe(Client client, string subscription)
    {
        if (Subscriptions.ContainsKey(client))
        {
            Subscriptions[client].Remove(subscription);
        }
    }
    
    public void Unsubscribe(Client client, List<string> subscriptions)
    {
        if (Subscriptions.ContainsKey(client))
        {
            foreach (var subscription in subscriptions)
            {
                Subscriptions[client].Remove(subscription);
            }
        }
    }
    
    public void Unsubscribe(Client client, List<object> subscriptions)
    {
        if (Subscriptions.ContainsKey(client))
        {
            foreach (var subscription in subscriptions)
            {
                Subscriptions[client].Remove(subscription.ToString());
            }
        }
    }
    
    public List<string> GetSubscriptions(Client client)
    {
        if (Subscriptions.ContainsKey(client))
        {
            return Subscriptions[client];
        }
        return new List<string>();
    }
    
    public void ClearSubscriptions(Client client)
    {
        if (Subscriptions.ContainsKey(client))
        {
            Subscriptions[client].Clear();
        }
    }
    
    public bool HasSubscription(Client client, string subscription)
    {
        if (Subscriptions.ContainsKey(client))
        {
            return Subscriptions[client].Contains(subscription) || Subscriptions[client].Contains("*");
        }
        return false;
    }
    
    public List<Client> GetClientsWithSubscription(string subscription)
    {
        List<Client> clients = new List<Client>();
        foreach (var client in Subscriptions.Keys)
        {
            if (HasSubscription(client, subscription))
            {
                clients.Add(client);
            }
        }
        return clients;
    }

    public List<Client> GetClientsWithSubscription(EventData eventData)
    {
        return GetClientsWithSubscription(eventData.EventName);
    }
    
}