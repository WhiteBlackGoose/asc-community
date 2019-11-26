using ascsite;
using ascsite.Core;
using Processor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ascsite.Core.PyInterface
{
    public class PyInterface : ProgramInterface
    {
        public static void RunPyProcess(string pyPath)
        {
            var prompt = "\"" + Const.PATH_PYTHON + "\"" + " " + "\"" + pyPath + "\"";
            System.Diagnostics.Process.Start("CMD.exe", prompt);
        }

        public PyInterface()
        {
            ProcessInit();
        }

        public static List<string> FromPyList(string pylist, char del=',')
        {
            return pylist.Split(del).Select(x => x.Trim()).ToList();
        }

        private Socket socket;
        public void ProcessInit()
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(Const.PYINTERFACE_ADDRESS), Const.PYINTERFACE_PORT);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(ipPoint);
        }
        public string Run(string code)
        {
            if (string.IsNullOrEmpty(code))
                return "";
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(code);
                socket.Send(data);
                data = new byte[Const.LIMIT_PYMAXRESPONSESIZE];
                string resp = Encoding.UTF8.GetString(data, 0, socket.Receive(data, data.Length, 0));
                if (resp[0] == Const.PYINTERFACE_ERROR)
                    throw new Exception(Const.ERMSG_PYINTERFACE_RESP);
                return resp.Substring(1, resp.Length - 1);
            }
            catch(Exception e)
            {
                ProcessStop();
                throw e;
            }
        }

        public void ProcessStop()
        {
            socket.Close();
        }
    }
}