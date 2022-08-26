using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using Titanium.Web.Proxy;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Models;

namespace GenshinProxyServer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private ProxyServer proxyServer = new ProxyServer();
        public bool interactMode = false;
        public string statusLog = "", urlLog = "";
        public MainWindow()
        {
            InitializeComponent();

            if (File.Exists("python_interact"))
            {
                interactMode = true;
            }

            if (interactMode)
            {
                LogUpdate("status", "已启用Python扩展模式");
                Dispatcher.BeginInvoke(
                    new Action(
                        delegate {
                            portLabel.Visibility = Visibility.Hidden;
                            portTextBox.Visibility = Visibility.Hidden;
                            startBtn.Visibility = Visibility.Hidden;
                            stopBtn.Visibility = Visibility.Hidden;
                            urlTextBox.Visibility = Visibility.Hidden;
                            this.Height = 200;
                            stsTextbox.Margin = new Thickness(0, 0, 0, 0);
                        }
                    )
                );
                startProxyServer();
                LogUpdate("status", "[提示] 请返回原神祈愿界面，点击历史记录");
            }
        }
        public void LogUpdate(string target, string updLog)
        {
            Dispatcher.BeginInvoke(
                    new Action(
                        delegate {
                            if (target == "status")
                            {
                                statusLog += updLog + "\n";
                                stsTextbox.Text = statusLog;
                            }
                            else if (target == "url")
                            {
                                urlLog = "";
                                urlLog += updLog + "\n";
                                urlTextBox.Text = urlLog;
                            }
                        }
                    )
                );
        }
        public async Task OnRequest(object sender, SessionEventArgs e)
        {
            await Task.Run(() =>
            {
                var method = e.HttpClient.Request.Method.ToUpper();
                if (method == "GET")
                {
                    var requestUrl = e.HttpClient.Request.Url;
                    var targetUrl = "/event/gacha_info/api/getGachaLog";
                    if (requestUrl.Contains(targetUrl))
                    {
                        LogUpdate("status", "已截获");
                        stopProxyServer();
                        LogUpdate("url", requestUrl);
                        if (interactMode)
                        {
                            File.WriteAllText("requestUrl.txt", requestUrl);
                            stopProxyServer();
                            Environment.Exit(0);
                        }
                    }

                }
            });

        }

        private void startProxyServer()
        {
            Dispatcher.BeginInvoke(
                new Action(
                    delegate {
                        portTextBox.IsEnabled = false;
                        stopBtn.IsEnabled = true;
                        startBtn.IsEnabled = false;
                        LogUpdate("url", "输出结果");
                    }
                )
            );
            proxyServer.BeforeRequest += OnRequest;
            int port = 0;
            try
            {
                port = int.Parse(portTextBox.Text);
                if (port < 0 || port > 65535)
                {
                    MessageBox.Show("端口范围错误");
                    return;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("端口输入错误");
                return;
            }
            var explicitEndPoint = new ExplicitProxyEndPoint(IPAddress.Any, port, true) { };
            LogUpdate("status", "代理服务器已启动");
            proxyServer.AddEndPoint(explicitEndPoint);
            proxyServer.Start();
            proxyServer.SetAsSystemHttpsProxy(explicitEndPoint);

        }
        private void stopProxyServer()
        {
            Dispatcher.BeginInvoke(
                new Action(
                    delegate {
                        portTextBox.IsEnabled = true;
                        startBtn.IsEnabled = true;
                        stopBtn.IsEnabled = false;
                    }
                )
            );
            proxyServer.BeforeRequest -= OnRequest;
            if (proxyServer.ProxyRunning)
            {
                proxyServer.Stop();
            }
            LogUpdate("status", "代理服务器已停止");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            stopProxyServer();
            e.Cancel = false;
        }

        private void stopBtn_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(
                new Action(
                    delegate {
                        stopBtn.IsEnabled = false;
                    }
                )
            );
            stopProxyServer();
        }

        private void startBtn_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(
                new Action(
                    delegate {
                        startBtn.IsEnabled = false;
                    }
                )
            );
            startProxyServer();
        }
    }
}
