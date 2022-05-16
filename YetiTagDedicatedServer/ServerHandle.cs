using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace YetiTagDedicatedServer
{
    class ServerHandle
    {
        public static void WelcomeReceived(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            string _username = _packet.ReadString();

            Console.WriteLine($"{_username} has connected. ({Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint})");
            if(_fromClient != _clientIdCheck)
            {
                Console.WriteLine($"Player \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
            }
        }

        public static void EmailReceived(int _fromClient, Packet _packet)
        {
            string _email = _packet.ReadString();
            string _username = _packet.ReadString();

            OTP.Instance.SendOTP(_email, _username);
        }

        public static void OTPReceived(int _fromClient, Packet _packet)
        {
            int _otp = _packet.ReadInt();
            string _username = _packet.ReadString();

            if(OTP.Instance.CheckLogin(_username, _otp))
            {
                ServerSend.Login(_fromClient,1);
            }
            else
            {
                ServerSend.Login(_fromClient, 0);
            }
        }

        public static void MessageReceived(int _fromClient, Packet _packet)
        {
            string _msg = _packet.ReadString();
            string _username = _packet.ReadString();

            ServerSend.SendMessage(_fromClient, _msg, _username);
        }

        public static void PlayerMovement(int _fromClient, Packet _packet)
        {
            bool[] _inputs = new bool[_packet.ReadInt()];
            for(int i = 0; i < _inputs.Length; i++)
            {
                _inputs[i] = _packet.ReadBool();
            }
            Quaternion _rotation = _packet.ReadQuaternion();

            Server.clients[_fromClient].player.SetInput(_inputs,_rotation);
        }
    }
}
