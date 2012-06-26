using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using xNet.Text;

namespace xNet.Net
{
    /// <summary>
    /// ������������ ������ ��� HTTP ������-�������.
    /// </summary>
    public class HttpProxyClient : ProxyClient
    {
        #region ��������� (��������)

        private const int BufferSize = 50;
        private const int DefaultPort = 8080;

        #endregion


        #region ������������ (��������)

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="HttpProxyClient"/>.
        /// </summary>
        public HttpProxyClient()
            : this(null) { }

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="HttpProxyClient"/> �������� ������ ������-�������, � ������������� ���� ������ - 8080.
        /// </summary>
        /// <param name="host">���� ������-�������.</param>
        public HttpProxyClient(string host)
            : this(host, DefaultPort) { }

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="HttpProxyClient"/> ��������� ������� � ������-�������.
        /// </summary>
        /// <param name="host">���� ������-�������.</param>
        /// <param name="port">���� ������-�������.</param>
        public HttpProxyClient(string host, int port)
            : this(host, port, string.Empty, string.Empty) { }

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="HttpProxyClient"/> ��������� ������� � ������-�������.
        /// </summary>
        /// <param name="host">���� ������-�������.</param>
        /// <param name="port">���� ������-�������.</param>
        /// <param name="username">��� ������������ ��� ����������� �� ������-�������.</param>
        /// <param name="password">������ ��� ����������� �� ������-�������.</param>
        public HttpProxyClient(string host, int port, string username, string password)
            : base(ProxyType.Http, host, port, username, password) { }

        #endregion


        #region ����������� ������ (��������)

        /// <summary>
        /// ����������� ������ � ��������� ������ <see cref="HttpProxyClient"/>.
        /// </summary>
        /// <param name="proxyAddress">������ ���� - ����:����:���_������������:������. ��� ��������� ��������� �������� ���������������.</param>
        /// <param name="proxyType">��� ������-�������.</param>
        /// <returns>��������� ������ <see cref="HttpProxyClient"/>.</returns>
        /// <exception cref="System.ArgumentNullException">�������� ��������� <paramref name="proxyAddress"/> ����� <see langword="null"/>.</exception>
        /// <exception cref="System.ArgumentException">�������� ��������� <paramref name="proxyAddress"/> �������� ������ �������.</exception>
        /// <exception cref="System.FormatException">������ ����� �������� ������������.</exception>
        /// <exception cref="System.InvalidOperationException">������� ���������������� ��� ������-�������.</exception>
        public static HttpProxyClient Parse(string proxyAddress)
        {
            return ProxyClient.Parse(ProxyType.Http, proxyAddress) as HttpProxyClient;
        }

        /// <summary>
        /// ����������� ������ � ��������� ������ <see cref="HttpProxyClient"/>. ���������� ��������, �����������, ������� �� ��������� ��������������.
        /// </summary>
        /// <param name="proxyAddress">������ ���� - ����:����:���_������������:������. ��� ��������� ��������� �������� ���������������.</param>
        /// <param name="proxyType">��� ������-�������.</param>
        /// <param name="result">���� �������������� ��������� �������, �� �������� ��������� ������ <see cref="HttpProxyClient"/>, ����� <see langword="null"/>.</param>
        /// <returns>�������� <see langword="true"/>, ���� �������� <paramref name="proxyAddress"/> ������������ �������, ����� <see langword="false"/>.</returns>
        public static bool TryParse(string s, out HttpProxyClient result)
        {
            ProxyClient proxy;

            if (ProxyClient.TryParse(ProxyType.Http, s, out proxy))
            {
                result = proxy as HttpProxyClient;
                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }

        #endregion


        /// <summary>
        /// ������ ���������� � ������-��������.
        /// </summary>
        /// <param name="destinationHost">���� ������ ����������, � ������� ����� ��������� ����� ������-������.</param>
        /// <param name="destinationPort">���� ������ ����������, � ������� ����� ��������� ����� ������-������.</param>
        /// <returns>���������� � ������-��������.</returns>
        /// <exception cref="System.InvalidOperationException">�������� �������� <see cref="xNet.Net.ProxyClient.Host"/> ����� <see langword="null"/> ��� ����� ������� �����.</exception>
        /// <exception cref="System.InvalidOperationException">�������� �������� <see cref="xNet.Net.ProxyClient.Port"/> ������ 1 ��� ������ 65535.</exception>
        /// <exception cref="System.InvalidOperationException">�������� �������� <see cref="xNet.Net.ProxyClient.Username"/> ����� ����� ����� 255 ��������.</exception>
        /// <exception cref="System.InvalidOperationException">�������� �������� <see cref="xNet.Net.ProxyClient.Password"/> ����� ����� ����� 255 ��������.</exception>
        /// <exception cref="System.ArgumentNullException">�������� ��������� <paramref name="destinationHost"/> ����� <see langword="null"/>.</exception>
        /// <exception cref="System.ArgumentException">�������� ��������� <paramref name="destinationHost"/> �������� ������ �������.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">�������� ��������� <paramref name="destinationPort"/> ������ 1 ��� ������ 65535.</exception>
        /// <exception cref="xNet.Net.ProxyException">������ ��� ������ � ������-��������.</exception>
        /// <remarks>���� ���� ������ ���������� ������� 80, �� ��� ����������� ������������ ����� 'CONNECT'.</remarks>
        public override TcpClient CreateConnection(string destinationHost, int destinationPort)
        {
            CheckState();

            #region �������� ����������

            if (destinationHost == null)
            {
                throw new ArgumentNullException("destinationHost");
            }

            if (destinationHost.Length == 0)
            {
                throw ExceptionHelper.EmptyString("destinationHost");
            }

            if (!ExceptionHelper.ValidateTcpPort(destinationPort))
            {
                throw ExceptionHelper.WrongTcpPort("destinationPort");
            }

            #endregion

            TcpClient tcpClient = CreateConnectionWithProxy();

            if (destinationPort != 80)
            {
                HttpStatusCode statusCode = HttpStatusCode.OK;

                try
                {
                    NetworkStream nStream = tcpClient.GetStream();

                    SendConnectionCommand(nStream, destinationHost, destinationPort);
                    statusCode = ReceiveResponse(nStream);
                }
                catch (Exception ex)
                {
                    tcpClient.Close();

                    if (ex is IOException || ex is SocketException)
                    {
                        throw NewProxyException(Resources.ProxyException_Error, ex);
                    }

                    throw;
                }

                if (statusCode != HttpStatusCode.OK)
                {
                    tcpClient.Close();

                    throw new ProxyException(string.Format(
                        Resources.ProxyException_ReceivedWrongStatusCode, statusCode, ToString()), this);
                }
            }

            return tcpClient;
        }


        #region ������ (��������)

        private void SendConnectionCommand(NetworkStream nStream, string destinationHost, int destinationPort)
        {
            var commandBuilder = new StringBuilder();

            commandBuilder.AppendFormat("CONNECT {0}:{1} HTTP/1.1\r\n", destinationHost, destinationPort);
            commandBuilder.AppendFormat(GenerateAuthorizationHeader());
            commandBuilder.AppendLine();

            byte[] buffer = Encoding.ASCII.GetBytes(commandBuilder.ToString());

            nStream.Write(buffer, 0, buffer.Length);
        }

        private HttpStatusCode ReceiveResponse(NetworkStream nStream)
        {
            byte[] buffer = new byte[BufferSize];
            var responseBuilder = new StringBuilder();

            WaitData(nStream);

            do
            {
                int bytesRead = nStream.Read(buffer, 0, BufferSize);
                responseBuilder.Append(Encoding.ASCII.GetString(buffer, 0, bytesRead));
            } while (nStream.DataAvailable);

            string response = responseBuilder.ToString();

            if (response.Length == 0)
            {
                throw NewProxyException(Resources.ProxyException_ReceivedEmptyResponse);
            }

            // �������� ������ �������. ������: HTTP/1.1 200 OK\r\n
            string strStatus = response.Substring(" ", HttpHelper.NewLine);

            int simPos = strStatus.IndexOf(' ');

            if (simPos == -1)
            {
                throw NewProxyException(Resources.ProxyException_ReceivedWrongResponse);
            }

            string statusLine = strStatus.Substring(0, simPos);

            if (statusLine.Length == 0)
            {
                throw NewProxyException(Resources.ProxyException_ReceivedWrongResponse);
            }

            HttpStatusCode statusCode = (HttpStatusCode)Enum.Parse(
                typeof(HttpStatusCode), statusLine);

            return statusCode;
        }

        private void WaitData(NetworkStream nStream)
        {
            int sleepTime = 0;
            int delay = (nStream.ReadTimeout < 10) ?
                10 : nStream.ReadTimeout;

            while (!nStream.DataAvailable)
            {
                if (sleepTime >= delay)
                {
                    throw NewProxyException(Resources.ProxyException_WaitDataTimeout);
                }

                sleepTime += 10;
                Thread.Sleep(10);
            }
        }

        #endregion


        internal string GenerateAuthorizationHeader()
        {
            if (!string.IsNullOrEmpty(_username) || !string.IsNullOrEmpty(_password))
            {
                string str = Convert.ToBase64String(Encoding.UTF8.GetBytes(
                    string.Format("{0}:{1}", _username, _password)));

                return string.Format("Proxy-Authorization: Basic {0}\r\n", str);
            }
            else
            {
                return string.Empty;
            }
        }
    }
}