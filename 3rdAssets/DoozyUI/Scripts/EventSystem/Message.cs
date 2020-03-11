// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using UnityEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DoozyUI
{
    /// <summary>
    /// A global messaging system that can be used to communicate between classes.
    /// </summary>
    public class Message
    {

        /// <summary>
        /// A delegation type used to callback when messages are sent and received.
        /// </summary>
        /// <param name="callerType">The caller type of the message.</param>
        /// <param name="handlerType">The handler type of the message.</param>
        /// <param name="messageType">The type of the sent message.</param>
        /// <param name="messageName">The name of the sent message.</param>
        /// <param name="handlerMethodName">The name of the method handling the sent message.</param>
        public delegate void OnMessageHandleDelegate(Type callerType, Type handlerType, Type messageType, string messageName, string handlerMethodName);

        /// <summary>
        /// Called when a message is sent and handled. Only works when in the Unity editor.
        /// </summary>
        public static OnMessageHandleDelegate OnMessageHandle;

        private static Dictionary<string, List<Delegate>> handlers = new Dictionary<string, List<Delegate>>();

        /// <summary>
        /// This prefix is added to typeless message names internally to distinct them from the typed messages
        /// </summary>
        private const string TypelessMessagePrefix = "typeless ";

        /// <summary>
        /// Adds a listener that triggers the given callback when the message with the given name is received.
        /// </summary>
        /// <param name="messageName">The message name that will be listened to.</param>
        /// <param name="callback">The callback that will be triggered when the message is received.</param>
        public static void AddListener(string messageName, Action callback)
        {
            RegisterListener(TypelessMessagePrefix + messageName, callback);
        }

        /// <summary>
        /// Adds a listener that triggers the given callback when a message of the given type is received.
        /// </summary>
        /// <typeparam name="T">The message type that will be listened to.</typeparam>
        /// <param name="callback">The callback that will be triggered when the message is received.</param>
        public static void AddListener<T>(Action<T> callback) where T : Message
        {
            RegisterListener(typeof(T).ToString(), callback);
        }

        /// <summary>
        /// Adds a listener that triggers the given callback when a message of the given type and name is received.
        /// </summary>
        /// <typeparam name="T">The message type that will be listened to.</typeparam>
        /// <param name="messageName">The message name that will be listened to.</param>
        /// <param name="callback">The callback that is triggered when the message is received.</param>
        public static void AddListener<T>(string messageName, Action<T> callback) where T : Message
        {
            RegisterListener(typeof(T).ToString() + messageName, callback);
        }

        /// <summary>
        /// Removes a listener that would trigger the given callback when a message with the given name is received.
        /// </summary>
        /// <param name="messageName">The message name that is being listened to.</param>
        /// <param name="callback">The callback that is triggered when the message is received.</param>
        public static void RemoveListener(string messageName, Action callback)
        {
            UnregisterListener(TypelessMessagePrefix + messageName, callback);
        }

        /// <summary>
        /// Removes a listener that would trigger the given callback when a message of the given type is received.
        /// </summary>
        /// <typeparam name="T">The message type that is being listened to.</typeparam>
        /// <param name="callback">The callback that is triggered when the message is received.</param>
        public static void RemoveListener<T>(Action<T> callback) where T : Message
        {
            UnregisterListener(typeof(T).ToString(), callback);
        }

        /// <summary>
        /// Removes a listener that would trigger the given callback when a message of the given type is received.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messageName"></param>
        /// <param name="callback"></param>
        public static void RemoveListener<T>(string messageName, Action<T> callback) where T : Message
        {
            UnregisterListener(typeof(T).ToString() + messageName, callback);
        }

        /// <summary>
        /// Sends a message of the given name.
        /// </summary>
        /// <param name="messageName">The name of the message.</param>
        public static void Send(string messageName)
        {
            SendMessage<Message>(TypelessMessagePrefix + messageName, null);
        }

        /// <summary>
        /// Sends a message of the given type.
        /// </summary>
        /// <typeparam name="T">The type of the message.</typeparam>
        /// <param name="message">The instance of the message.</param>
        public static void Send<T>(T message) where T : Message
        {
            SendMessage<T>(typeof(T).ToString(), message);
        }

        /// <summary>
        /// Sends a message of the given name and type.
        /// </summary>
        /// <typeparam name="T">The type of the message.</typeparam>
        /// <param name="messageName">The name of the message.</param>
        /// <param name="message">The instance of the message.</param>
        public static void Send<T>(string messageName, T message) where T : Message
        {
            SendMessage<T>(typeof(T).ToString() + messageName, message);
        }

        private static void RegisterListener(string messageName, Delegate callback)
        {
            if (callback == null)
            {
                UnityEngine.Debug.LogError("Failed to add Message Listener because the given callback is null!");
                return;
            }
            if (!handlers.ContainsKey(messageName))
            {
                handlers.Add(messageName, new List<Delegate>());
            }
            List<Delegate> messageHandlers = handlers[messageName];
            messageHandlers.Add(callback);
        }

        private static void UnregisterListener(string messageName, Delegate callback)
        {
            if (!handlers.ContainsKey(messageName)) { return; }
            List<Delegate> messageHandlers = handlers[messageName];
            Delegate messageHandler = messageHandlers.Find(x => x.Method == callback.Method && x.Target == callback.Target);
            if (messageHandler == null) { return; }
            messageHandlers.Remove(messageHandler);
        }

        private static void SendMessage<T>(string messageName, T e) where T : Message
        {
            if (!handlers.ContainsKey(messageName)) { return; }

            Type callerType = null;
            if (Application.isEditor)
            {
                StackTrace stackTrace = new StackTrace();
                callerType = stackTrace.GetFrame(2).GetMethod().DeclaringType;
            }

            List<Delegate> messageHandlers = handlers[messageName];
            foreach (Delegate messageHandler in messageHandlers)
            {
                if (messageHandler.GetType() != typeof(Action<T>) && messageHandler.GetType() != typeof(Action)) { continue; }

                if (Application.isEditor && OnMessageHandle != null)
                {
                    string methodName = messageHandler.Method.Name;

                    messageName = messageName.Replace(TypelessMessagePrefix, "");

                    if (typeof(T) != typeof(Message))
                    {
                        messageName = messageName.Replace(typeof(T).ToString(), "");
                    }

                    OnMessageHandle(callerType, messageHandler.Target.GetType(), typeof(T), messageName, methodName);
                }

                if (typeof(T) == typeof(Message))
                {
                    Action action = (Action)messageHandler;
                    action();
                }
                else
                {
                    Action<T> action = (Action<T>)messageHandler;
                    action(e);
                }
            }
        }

        protected Message() { }

    }
}


