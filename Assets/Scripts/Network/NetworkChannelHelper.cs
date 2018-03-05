using ProtoBuf.Meta;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using ZFramework;
using ZFramework.Event;
using ZFramework.Network;

namespace GGame.NetWork
{
    public class NetworkChannelHelper : INetworkChannelHelper
    {
        private readonly Dictionary<int, Type> m_ServerToClientPacketTypes = new Dictionary<int, Type>();
        private INetworkChannel m_NetworkChannel = null;
        private SCPacketHeader m_CachedPacketHeader = null;
        private bool isFirst = true;

        /// <summary>
        /// 获取消息包头长度。
        /// </summary>
        int INetworkChannelHelper.PacketHeaderLength
        {
            get
            {
                return sizeof(int) + sizeof(int);
            }
        }

        /// <summary>
        /// 初始化网络频道辅助器。 ---自动调用
        /// </summary>
        /// <param name="networkChannel">网络频道。</param>
        void INetworkChannelHelper.Initialize(INetworkChannel networkChannel)
        {
            m_NetworkChannel = networkChannel;
             
            // 反射注册包和包处理函数。
            Type packetBaseType = typeof(ServerToClientPacketBase);
            Type packetHandlerBaseType = typeof(PacketHandlerBase);
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type[] types = assembly.GetTypes();
            for (int i = 0; i < types.Length; i++)
            {
                if (!types[i].IsClass || types[i].IsAbstract)
                {
                    continue;
                }

                if (types[i].BaseType == packetBaseType)
                {
                    PacketBase packetBase = (PacketBase)Activator.CreateInstance(types[i]);
                    Type packetType = GetServerToClientPacketType(packetBase.Id);
                    if (packetType != null)
                    {
                        Log.Warning("Already exist packet type '{0}', check '{1}' or '{2}'?.", packetBase.Id.ToString(), packetType.Name, packetBase.GetType().Name);
                        continue;
                    }

                    m_ServerToClientPacketTypes.Add(packetBase.Id, types[i]);
                }
                else if (types[i].BaseType == packetHandlerBaseType)
                {
                    IPacketHandler packetHandler = (IPacketHandler)Activator.CreateInstance(types[i]);
                    m_NetworkChannel.RegisterHandler(packetHandler);
                }
            }

            //m_NetworkChannel.NetworkChannelConnected += OnNetworkConnected;
            //m_NetworkChannel.NetworkChannelClosed += OnNetworkClosed;
            //m_NetworkChannel.NetworkChannelSended += OnNetworkSendPacket;
            //m_NetworkChannel.NetworkChannelMissHeartBeat += OnNetworkMissHeartBeat;
            //m_NetworkChannel.NetworkChannelError += OnNetworkError;
            //m_NetworkChannel.NetworkChannelCustomError += OnNetworkCustomError;

            GameEntry.Event.Subscribe(ZFramework.Runtime.NetworkConnectedEventArgs.EventId, OnNetworkConnected);
            GameEntry.Event.Subscribe(ZFramework.Runtime.NetworkClosedEventArgs.EventId, OnNetworkClosed);
            GameEntry.Event.Subscribe(ZFramework.Runtime.NetworkSendPacketEventArgs.EventId, OnNetworkSendPacket);
            GameEntry.Event.Subscribe(ZFramework.Runtime.NetworkMissHeartBeatEventArgs.EventId, OnNetworkMissHeartBeat);
            GameEntry.Event.Subscribe(ZFramework.Runtime.NetworkErrorEventArgs.EventId, OnNetworkError);
            GameEntry.Event.Subscribe(ZFramework.Runtime.NetworkCustomErrorEventArgs.EventId, OnNetworkCustomError);
             
        }

        /// <summary>
        /// 关闭并清理网络频道辅助器。 ---自动调用
        /// </summary>
        void INetworkChannelHelper.Shutdown()
        {
            //m_NetworkChannel.NetworkChannelConnected -= OnNetworkConnected;
            //m_NetworkChannel.NetworkChannelClosed -= OnNetworkClosed;
            //m_NetworkChannel.NetworkChannelSended -= OnNetworkSendPacket;
            //m_NetworkChannel.NetworkChannelMissHeartBeat -= OnNetworkMissHeartBeat;
            //m_NetworkChannel.NetworkChannelError -= OnNetworkError;
            //m_NetworkChannel.NetworkChannelCustomError -= OnNetworkCustomError;

            GameEntry.Event.Unsubscribe(ZFramework.Runtime.NetworkConnectedEventArgs.EventId, OnNetworkConnected);
            GameEntry.Event.Unsubscribe(ZFramework.Runtime.NetworkClosedEventArgs.EventId, OnNetworkClosed);
            GameEntry.Event.Unsubscribe(ZFramework.Runtime.NetworkSendPacketEventArgs.EventId, OnNetworkSendPacket);
            GameEntry.Event.Unsubscribe(ZFramework.Runtime.NetworkMissHeartBeatEventArgs.EventId, OnNetworkMissHeartBeat);
            GameEntry.Event.Unsubscribe(ZFramework.Runtime.NetworkErrorEventArgs.EventId, OnNetworkError);
            GameEntry.Event.Unsubscribe(ZFramework.Runtime.NetworkCustomErrorEventArgs.EventId, OnNetworkCustomError);

            m_NetworkChannel = null;
            m_CachedPacketHeader = null;
        }

        /// <summary>
        /// 发送心跳消息包。 ---自动调用
        /// </summary>
        /// <returns>是否发送心跳消息包成功。</returns>
        bool INetworkChannelHelper.SendHeartBeat()
        {
            //CSHeartBeat packet = new CSHeartBeat();
            //m_NetworkChannel.Send(packet);

            //CSLogin msg = new CSLogin();
            //msg.Password = "Password Password Password Password Password Password Password Password ";
            //msg.Password3 = "Password3 Password3 Password3 Password3 Password3 Password3 Password3 Password3 Password3 Password3";
            //msg.PacketType = 4;

            //m_NetworkChannel.Send(msg);

            return true;
        }

        /// <summary>
        /// 序列化消息包。
        /// </summary>
        /// <typeparam name="T">消息包类型。</typeparam>
        /// <param name="packet">要序列化的消息包。</param>
        /// <returns>序列化后的消息包字节流。</returns>
        byte[] INetworkChannelHelper.Serialize<T>(T packet)
        {
            // 恐怖的 GCAlloc，这里是例子，不做优化
            using (MemoryStream memoryStream = new MemoryStream())
            {
                RuntimeTypeModel.Default.Serialize(memoryStream, packet);
                Byte[] data = memoryStream.ToArray();
                //Array.Reverse(data);  //反转数组转成大端。
                Byte[] id = Utility.Converter.GetBytes(packet.Id);
                Array.Reverse(id);  //反转数组转成大端。
                Byte[] length = Utility.Converter.GetBytes(data.Length);
                Array.Reverse(length);  //反转数组转成大端。
                Byte[] all = new Byte[data.Length + length.Length + id.Length];
                length.CopyTo(all, 0);
                id.CopyTo(all, length.Length);
                data.CopyTo(all, length.Length + id.Length);

                //CSPacketHeader packetHeader = new CSPacketHeader(packetImpl.PacketId);
                //Serializer.Serialize(memoryStream, packetHeader);
                //Serializer.SerializeWithLengthPrefix(memoryStream, packet, PrefixStyle.Fixed32);
                //return memoryStream.ToArray();
                
                return all;
            }
        }

        /// <summary>
        /// 反序列消息包头。
        /// </summary>
        /// <param name="source">要反序列化的来源流。</param>
        /// <param name="customErrorData">用户自定义错误数据。</param>
        /// <returns></returns>
        IPacketHeader INetworkChannelHelper.DeserializePacketHeader(Stream source, out object customErrorData)
        {
            MemoryStream ms = source as MemoryStream;

            // 注意：此函数并不在主线程调用！
            Byte[] data = ms.ToArray();

            Byte[] length = new Byte[4];
            source.Read(length, 0, 4);
            Array.Reverse(length);  //反转数组转成小端

            Byte[] id = new Byte[4];
            source.Read(id, 0, 4);
            Array.Reverse(id);  //反转数组转成小端

            m_CachedPacketHeader = new SCPacketHeader(Utility.Converter.GetInt32(id));
            m_CachedPacketHeader.PacketLength = Utility.Converter.GetInt32(length);
             
            customErrorData = null;
            //m_CachedPacketHeader = Serializer.Deserialize<SCPacketHeader>(source);
            return m_CachedPacketHeader;
        }

        /// <summary>
        /// 反序列化消息包。
        /// </summary>
        /// <param name="source">要反序列化的来源流。</param>
        /// <param name="customErrorData">用户自定义错误数据。</param>
        /// <returns>反序列化后的消息包。</returns>
        Packet INetworkChannelHelper.DeserializePacket(Stream source, out object customErrorData)
        {
            // 注意：此函数并不在主线程调用！
            customErrorData = null;
            Type packetType = GetServerToClientPacketType(m_CachedPacketHeader.Id);
            if (packetType == null)
            {
                PacketType pt = PacketType.Undefined;
                int pid = 0;
                //GameEntry.Network.ParseOpCode(m_CachedPacketHeader.Id, out pt, out pid);
                Log.Error(string.Format("Can not deserialize packet for packet type '{0}', packet id '{1}'.", pt.ToString(), pid.ToString()));
            }

            return (PacketBase)RuntimeTypeModel.Default.Deserialize(source, null, packetType);
        }

        private Type GetServerToClientPacketType(int opCode)
        {
            Type packetType = null;
            if (m_ServerToClientPacketTypes.TryGetValue(opCode, out packetType))
            {
                return packetType;
            }

            return null;
        }

        private void OnNetworkConnected(object sender, GameEventArgs e)
        {
            ZFramework.Runtime.NetworkConnectedEventArgs ne = (ZFramework.Runtime.NetworkConnectedEventArgs)e;
            if (ne.NetworkChannel != m_NetworkChannel)
            {
                return;
            }

            Log.Info("Network channel '{0}' connected, local address '{1}:{2}', remote address '{3}:{4}'.", ne.NetworkChannel.Name, ne.NetworkChannel.LocalIPAddress, ne.NetworkChannel.LocalPort.ToString(), ne.NetworkChannel.RemoteIPAddress, ne.NetworkChannel.RemotePort.ToString());
        }

        private void OnNetworkClosed(object sender, GameEventArgs e)
        {
            ZFramework.Runtime.NetworkClosedEventArgs ne = (ZFramework.Runtime.NetworkClosedEventArgs)e;
            if (ne.NetworkChannel != m_NetworkChannel)
            {
                return;
            }

            Log.Info("Network channel '{0}' closed.", ne.NetworkChannel.Name);

            //GameEntry.Network.DestroyNetworkChannel("Battle");
            //GameEntry.Network.EventComponent = GameEntry.Event;

            //INetworkChannelHelper channelHelper01 = new NetworkChannelHelper();
            //INetworkChannel channel01 = GameEntry.Network.CreateNetworkChannel("Battle", channelHelper01);
            //channel01.HeartBeatInterval = 100f;
            ////channel01.Connect(System.Net.IPAddress.Parse("10.246.52.157"), 9099);
            //channel01.Connect(System.Net.IPAddress.Parse("115.159.68.24"), 9099);

        }

        private void OnNetworkSendPacket(object sender, GameEventArgs e)
        {
            ZFramework.Runtime.NetworkSendPacketEventArgs ne = (ZFramework.Runtime.NetworkSendPacketEventArgs)e;
            if (ne.NetworkChannel != m_NetworkChannel)
            {
                return;
            }
        }

        private void OnNetworkMissHeartBeat(object sender, GameEventArgs e)
        {
            ZFramework.Runtime.NetworkMissHeartBeatEventArgs ne = (ZFramework.Runtime.NetworkMissHeartBeatEventArgs)e;
            if (ne.NetworkChannel != m_NetworkChannel)
            {
                return;
            }

            Log.Info("Network channel '{0}' miss heart beat '{1}' times.", ne.NetworkChannel.Name, ne.MissCount.ToString());

            if (ne.MissCount < 2)
            {
                return;
            }

            ne.NetworkChannel.Close();
        }

        private void OnNetworkError(object sender, GameEventArgs e)
        {
            ZFramework.Runtime.NetworkErrorEventArgs ne = (ZFramework.Runtime.NetworkErrorEventArgs)e;
            if (ne.NetworkChannel != m_NetworkChannel)
            {
                return;
            }

            Log.Info("Network channel '{0}' error, error code is '{1}', error message is '{2}'.", ne.NetworkChannel.Name, ne.ErrorCode.ToString(), ne.ErrorMessage);

            ne.NetworkChannel.Close();
        }

        private void OnNetworkCustomError(object sender, GameEventArgs e)
        {
            ZFramework.Runtime.NetworkCustomErrorEventArgs ne = (ZFramework.Runtime.NetworkCustomErrorEventArgs)e;
            if (ne.NetworkChannel != m_NetworkChannel)
            {
                return;
            }
        }

        //private void OnNetworkConnected(INetworkChannel sender, object e)
        //{
        //    ZFramework.Runtime.NetworkConnectedEventArgs ne = (ZFramework.Runtime.NetworkConnectedEventArgs)e;
        //    if (ne.NetworkChannel != m_NetworkChannel)
        //    {
        //        return;
        //    }

        //    Log.Info("Network channel '{0}' connected, local address '{1}:{2}', remote address '{3}:{4}'.", ne.NetworkChannel.Name, ne.NetworkChannel.LocalIPAddress, ne.NetworkChannel.LocalPort.ToString(), ne.NetworkChannel.RemoteIPAddress, ne.NetworkChannel.RemotePort.ToString());
        //}

        //private void OnNetworkClosed(INetworkChannel sender)
        //{
        //    Log.Info("Network channel '{0}' closed.", sender.Name);
        //}

        //private void OnNetworkSendPacket(INetworkChannel sender, int num,  object e)
        //{
        //    Log.Info(sender.Name + "  发送 " + num + "字节! ____" + e.ToString());
        //}

        //private void OnNetworkMissHeartBeat(INetworkChannel sender, int num)
        //{
        //    Log.Info("Network channel '{0}' miss heart beat '{1}' times.", sender.Name, sender.ToString());

        //    //sender.Close();
        //}

        //private void OnNetworkError(INetworkChannel sender, NetworkErrorCode code, string obj)
        //{
        //    Log.Info("Network channel '{0}' error, error code is '{1}', error message is '{2}'.", sender.Name, code.ToString(), obj);

        //    sender.Close();
        //}

        //private void OnNetworkCustomError(INetworkChannel sender, object e)
        //{
        //    if (sender != m_NetworkChannel)
        //    {
        //        return;
        //    }
        //}
    }
}