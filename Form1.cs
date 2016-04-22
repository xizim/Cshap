using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Collections;
using System.Net;

namespace ipquery
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        ArrayList DIYIP = new ArrayList();//自定义的IP地址
        ArrayList Belonging = new ArrayList();//自定义的IP的归属地
        //创建一个委托，是为访问TextBox控件服务的。
        public delegate void UpdateTxt(string msg);
        //定义一个委托变量
        public UpdateTxt updateTxt;
        //修改TextBox值的方法。
        Process p = null;
        StreamReader reader = null;
        string[] commandlist;
        public void UpdateTxtMethod(string msg)
        {
            textBox2.AppendText(msg + "\r\n");
            textBox2.ScrollToCaret();
        }
        public void DosUpdate()
        {
            textBox2.AppendText("\r\nxiz>.<:");
            textBox2.ScrollToCaret();
        }
        private void button1_Click(object sender, EventArgs e)
        {

                DIYIP.Clear(); Belonging.Clear();
                bool tmp = true;
                for (int i = 0; i < comboBox2.Items.Count; i++)
                {
                    if (comboBox2.Items[i].ToString() == comboBox2.Text)
                    {
                        tmp = false;
                    }
                }
                if (tmp)
                    comboBox2.Items.Add(comboBox2.Text);
                try
                {
                    //从www.75cdn.org上获取360网站卫士IP段
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.75cdn.org/data/attachment/wzb/qhip.wzb");
                    //声明一个HttpWebRequest请求
                    request.Timeout = 3000000;
                    //设置连接超时时间
                    request.Headers.Set("Pragma", "no-cache");
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    if (response.ToString() != "")
                    {
                        Stream streamReceive = response.GetResponseStream();
                        Encoding encoding = Encoding.Default;
                        StreamReader streamReader = new StreamReader(streamReceive, encoding);
                        string strResult = "";
                        while (strResult != null)
                        {
                            strResult = streamReader.ReadLine();
                            if (strResult != null)
                            {
                                //上面一行一行读。然后在里面就看你自己怎么处理了。下面是假设。
                                DIYIP.Add(strResult.Substring(0, strResult.LastIndexOf("@")));//IP地址
                                Belonging.Add(strResult.Substring(strResult.LastIndexOf("@") + 1));//归属地
                            }
                        }
                    }


                    Encoding code = Encoding.Default;
                    StreamReader objReader = new StreamReader("DIYIP.txt", code);
                    string sLine = "";
                    while (sLine != null)
                    {
                        sLine = objReader.ReadLine();
                        if (sLine != null)
                        {
                            for (int i = 0; i < DIYIP.Count; i++)
                            {
                                string ip = sLine.Substring(0, sLine.LastIndexOf("@"));
                                if (DIYIP[i].ToString() != ip)
                                {
                                    DIYIP.Add(ip);//IP地址
                                    Belonging.Add(sLine.Substring(sLine.LastIndexOf("@") + 1));//归属地
                                }
                            }
                        }
                    }
                    objReader.Close();
                }
                catch (Exception a)
                {

                }
                if (button1.Text == "开始执行")
                {
                    button1.Text = "停止执行";
                    commandlist = comboBox2.Text.Split(' ');
                    IP.EnableFileWatch = true; // 默认值为：false，如果为true将会检查ip库文件的变化自动reload数据

                    IP.Load("17monipdb.bat");

                    //  textBox2.Text=   IP.Find(textBox1.Text)[0] + IP.Find(textBox1.Text)[1] + IP.Find(textBox1.Text)[2] ; //返回字符串数组["GOOGLE","GOOGLE"]
                    textBox2.Text = "";
                    Thread objThread = new Thread(new ThreadStart(delegate
                    {
                        dos();
                    }));
                    objThread.Start();
                }
                else
                {
                if(commandlist[0]!="host")
                {
                    button1.Text = "开始执行";
                    p.Close();//关闭进程
                    reader.Close();//关闭流
                    lblstate.Text = "状态:正在与你对望(￣.￣)";
                }
                else
                {
                    MessageBox.Show("host命令暂不支持停止执行！");
                }
                }
            }

    /// <summary>
    /// 装逼模式执行
    /// </summary>
        private void DosZhixing()
        {
            DIYIP.Clear(); Belonging.Clear();
            try
            {
                //从www.75cdn.org上获取360网站卫士IP段
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.75cdn.org/data/attachment/wzb/qhip.wzb");
                //声明一个HttpWebRequest请求
                request.Timeout = 3000000;
                //设置连接超时时间
                request.Headers.Set("Pragma", "no-cache");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.ToString() != "")
                {
                    Stream streamReceive = response.GetResponseStream();
                    Encoding encoding = Encoding.Default;
                    StreamReader streamReader = new StreamReader(streamReceive, encoding);
                    string strResult = "";
                    while (strResult != null)
                    {
                        strResult = streamReader.ReadLine();
                        if (strResult != null)
                        {
                            //上面一行一行读。然后在里面就看你自己怎么处理了。下面是假设。
                            DIYIP.Add(strResult.Substring(0, strResult.LastIndexOf("@")));//IP地址
                            Belonging.Add(strResult.Substring(strResult.LastIndexOf("@") + 1));//归属地
                        }
                    }
                }
                Encoding code = Encoding.Default;
                StreamReader objReader = new StreamReader("DIYIP.txt", code);
                string sLine = "";
                while (sLine != null)
                {
                    sLine = objReader.ReadLine();
                    if (sLine != null)
                    {
                        for (int i = 0; i < DIYIP.Count; i++)
                        {
                            string ip = sLine.Substring(0, sLine.LastIndexOf("@"));
                            if (DIYIP[i].ToString() != ip)
                            {
                                DIYIP.Add(ip);//IP地址
                                Belonging.Add(sLine.Substring(sLine.LastIndexOf("@") + 1));//归属地
                            }
                        }
                    }
                }
                objReader.Close();
            }
            catch (Exception a)
            {

            }
                commandlist = textBox2.Text.Substring(textBox2.Text.LastIndexOf(":") + 1).Split(' ');
                IP.EnableFileWatch = true; // 默认值为：false，如果为true将会检查ip库文件的变化自动reload数据

                IP.Load("17monipdb.bat");

                //  textBox2.Text=   IP.Find(textBox1.Text)[0] + IP.Find(textBox1.Text)[1] + IP.Find(textBox1.Text)[2] ; //返回字符串数组["GOOGLE","GOOGLE"]
           //     textBox2.Text = "";
                Thread objThread = new Thread(new ThreadStart(delegate
                {
                    dos();
                }));
                objThread.Start();
            }
        private void timer1_Tick(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 执行DOS命令
        /// </summary>
        private void dos()
        {
            lblstate.Text = "正在执行" + commandlist[0] + "～(￣▽￣～)(～￣▽￣)～";

            if (commandlist[0] != "host")
            {
                string command = string.Empty;
                for (int i = 1; i < commandlist.Length; i++)
                {
                    command += commandlist[i] + " ";
                }
                ProcessStartInfo start = new ProcessStartInfo(commandlist[0] + ".exe");//设置运行的命令行文件问ping.exe文件，这个文件系统会自己找到
                                                                                       //如果是其它exe文件，则有可能需要指定详细路径，如运行winRar.exe
                start.Arguments = command;//设置命令参数
                start.CreateNoWindow = true;//不显示dos命令行窗口
                start.RedirectStandardOutput = true;//
                start.RedirectStandardInput = true;//
                start.UseShellExecute = false;//是否指定操作系统外壳进程启动程序

                try
                {
                    p = Process.Start(start);
                    reader = p.StandardOutput;//截取输出流
                    string line = reader.ReadLine();//每次读取一行
                    while (!reader.EndOfStream)
                    {

                        this.Invoke(new Action(() =>
                        {
                            bool tmp = true; string stringtmp = ""; int n; string aaa = "";
                            if (line != "")
                            {
                                aaa = line.Substring(0, 1);
                            }
                            if (!int.TryParse(aaa, out n))//如果第一个不是数字的话 那我就反转
                            {
                                for (int i = line.Length - 1; i > 0; i--)
                                {
                                    stringtmp += line[i].ToString();
                                }
                            }
                            else
                            {
                                stringtmp = line;
                            }
                            Match m = Regex.Match(stringtmp, @"\d{1,3}.\d{1,3}.\d{1,3}.\d{1,3}");
                            if (m.Success)
                            {
                                if (!int.TryParse(aaa, out n))//如果第一个不是数字的话 那我就反转
                                {
                                    char[] arr = m.Value.ToCharArray();
                                    Array.Reverse(arr);
                                    stringtmp = new string(arr);
                                }
                                string pattrn = @"(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])";
                                if (System.Text.RegularExpressions.Regex.IsMatch(stringtmp, pattrn))
                                {
                                    for (int i = 0; i < DIYIP.Count; i++)
                                    {
                                        if (stringtmp.IndexOf(DIYIP[i].ToString()) != -1)//如果截取的IP地址在DIYIP.txt中存在那么就输出归属地
                                        {
                                            if (Belonging[i].ToString() == "")//如果归属地为空的话
                                            {
                                                Belonging[i] = "未知";
                                                updateTxt(line + " 归属地:" + Belonging[i].ToString());
                                                tmp = false;//不再调用API来查询归属地
                                            }
                                            else
                                            {
                                                updateTxt(line + " 归属地:" + Belonging[i].ToString());
                                                tmp = false;//不再调用API来查询归属地
                                            }
                                        }

                                    }

                                    if (tmp)//如果DIYIP.txt没查到IP的haunted
                                    {
                                        if (IP.Find(stringtmp)[2] == IP.Find(stringtmp)[1])
                                            updateTxt(line + " 归属地:" + IP.Find(stringtmp)[0] + IP.Find(stringtmp)[1]);
                                        if (IP.Find(stringtmp)[1] == IP.Find(stringtmp)[0])
                                            updateTxt(line + " 归属地:" + IP.Find(stringtmp)[0]);
                                        if (IP.Find(stringtmp)[0] != IP.Find(stringtmp)[1] && IP.Find(stringtmp)[1] != IP.Find(stringtmp)[2])
                                            updateTxt(line + " 归属地:" + IP.Find(stringtmp)[0] + IP.Find(stringtmp)[1] + IP.Find(stringtmp)[2]);
                                    }
                                }
                                else
                                {
                                    updateTxt(line);
                                }
                            }
                            else
                            {
                                updateTxt(line);
                            }
                        }));


                        line = reader.ReadLine();
                    }
                    //p.WaitForExit();//等待程序执行完退出进程
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    try
                    {
                        p.Close();//关闭进程
                        reader.Close();//关闭流
                        lblstate.Text = "状态:无动作_(:3」∠)_";
                        button1.Text = "开始执行";
                        DosUpdate();
                    }
                    catch { }
                       

                }
            }
            else
            {
                string command = string.Empty;
                for (int i = 1; i < commandlist.Length; i++)
                {
                    command += commandlist[i] + " ";
                }

                String URL = commandlist[1]; //定义要获取http头的网址
                string statusCode;
                try
                {
                    HttpWebRequest req = (HttpWebRequest)WebRequest.CreateDefault(new Uri("http://" + URL));
                    req.Method = "HEAD";//设置请求方式为请求头，这样就不需要把整个网页下载下来 
                    req.Proxy = new WebProxy(commandlist[2]);
                    //req.Timeout = 2000; //这里设置超时时间，如果不设置，默认为10000 
                    req.Timeout = 10000; //这里设置超时时间，如果不设置，默认为10000 
                    HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                    statusCode = "状态:" + res.StatusCode.ToString() + "\r\n" + res.Headers.ToString(); ;
                    //   textBox2.Text = statusCode + "\r\n" + WebsCan.GetHtml(URL);
                    this.Invoke(new Action(() =>
                    {
                        UpdateTxtMethod(statusCode + "\r\n" + WebsCan.GetHtml(URL));
                        Setextbox();
                    }));
                }
                catch (WebException a)//使用try catch方式，如果正常，则返回OK，不正常就返回对应的错误。 
                {
                    this.Invoke(new Action(() =>
                    {
                        UpdateTxtMethod(a.Message.ToString());
                    }));
                }
                finally
                {

                        lblstate.Text = "状态:无动作_(:3」∠)_";
                        button1.Text = "开始执行";
                        GetLine(textBox2);
                        DosUpdate();

                } 
            }
        }
        /// <summary>
        /// 设置textbox2的光标到第一行
        /// </summary>
        private void Setextbox()
        {
                        this.textBox2.Focus();
                        //设置光标的位置到文本尾
                        this.textBox2.Select(0, 0);
                        //滚动到控件光标处
                        this.textBox2.ScrollToCaret();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            updateTxt = new UpdateTxt(UpdateTxtMethod);
            try
            {
                Encoding code = Encoding.Default;
                StreamReader objReader = new StreamReader("ini.txt", code);
                string sLine = "";
                while (sLine != null)
                {
                    sLine = objReader.ReadLine();
                    if (sLine != null)
                    {
                        comboBox2.Items.Add(sLine);
                    }
                }
            }
            catch(Exception a)
            {

            }
        }

        private void 更新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // System.Diagnostics.Process.Start("http://www.xiz.im/post-42.html");

            MessageBox.Show(WebsCan.GetWebContent("https://bbs.mydzbbs.com/dos.xiz"));
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            comboBox2.Text = comboBox1.Text;
            comboBox1.Text = "";
        }

        private void 自定义IP库ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //导入IP库
            if (MessageBox.Show("你的操作会导致原来自定义的IP库被替换，建议先备份软件目录下的DIYIP.txt文件再进行操作！", "注意！", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = "c:\\";//注意这里写路径时要用c:\\而不是c:\
                openFileDialog.Filter = "txt文件|*.txt|所有文件|*.*";
                openFileDialog.RestoreDirectory = true;
                openFileDialog.FilterIndex = 1;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                         //获得文件路径
                            string localFilePath = openFileDialog.FileName.ToString();
                            //获取文件名，不带路径
                            string fileNameExt = localFilePath.Substring(localFilePath.LastIndexOf("\\") + 1);
                            string str = System.Environment.CurrentDirectory;
                    /*          Dos.cmd("copy " + localFilePath + " " + str + "");
                           Dos.cmd("del DIYIP.txt");
                           Dos.cmd("ren " + fileNameExt + " DIYIP.txt");*/
                    System.IO.File.Copy(localFilePath,"DIYIP.txt",true);
                    MessageBox.Show("导入成功");
                }
            }

        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.xiz.im/post-42.html");
        }

        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="srcPath"></param>
        /// <param name="aimPath"></param>
        private void CopyDir(string srcPath, string aimPath)
       {
            try
            {
                // 检查目标目录是否以目录分割字符结束如果不是则添加
                if (aimPath[aimPath.Length - 1] != System.IO.Path.DirectorySeparatorChar)
                {
                    aimPath += System.IO.Path.DirectorySeparatorChar;
                }
                // 判断目标目录是否存在如果不存在则新建
                if (!System.IO.Directory.Exists(aimPath))
               {
                    System.IO.Directory.CreateDirectory(aimPath);
               }
                // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组
                // 如果你指向copy目标文件下面的文件而不包含目录请使用下面的方法
                // string[] fileList = Directory.GetFiles（srcPath）；
                string[] fileList = System.IO.Directory.GetFileSystemEntries(srcPath);
                // 遍历所有的文件和目录
                foreach(string file in fileList)
                {
                    // 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                    if (System.IO.Directory.Exists(file))
                  {
                        CopyDir(file, aimPath + System.IO.Path.GetFileName(file));
                  }
                  // 否则直接Copy文件
                   else
                   {
                        System.IO.File.Copy(file, aimPath + System.IO.Path.GetFileName(file),true);
                    }
                }
            }
            catch (Exception a)
            {

            }
       }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
  
        }

        private void 清理缓存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string sLine = "";
            using (StreamWriter sw = new StreamWriter("ini.txt"))
            {
                sw.Write(sLine);
                sw.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.textBox2.Focus();
            //设置光标的位置到文本尾
            this.textBox2.Select(0, 0);
            //滚动到控件光标处
            this.textBox2.ScrollToCaret();


        }

        private void GetLine(TextBox txtCmdInput)//取控件里鼠标所在行的命令发送后提到最前
        {
            //取光标所在行的字符串包括末尾的换行回车符"\r\n"
            string strCmdText = txtCmdInput.Text;
            int curInx = txtCmdInput.SelectionStart;       //光标所在位置索引
            string tmp = strCmdText.Substring(0, curInx);  //开始到光标处的子串
            int start = tmp.LastIndexOf('\n');             //找光标所在行的开头索引start + 1
            tmp = strCmdText.Substring(curInx);//当前光标所在位置到最后的子串
            int end = tmp.IndexOf('\n'); //找该行的末尾索引包括"\r\n"
            string curRowText = null;
            if (end > 0)
            {
                curRowText = strCmdText.Substring(start + 1, curInx - start + end);
           }
           else
           {
                 curRowText = strCmdText.Substring(start + 1);
           }
            //把光标所在行的命令提到第一行的下一行
           String strLeft = strCmdText.Remove(start + 1, curRowText.Length);
            
            //处理剩下的字符串，注意把开头结尾的"\r\n"找到删掉
            if (strLeft != "")
              {
                while (strLeft[strLeft.Length - 1] == '\r' || strLeft[strLeft.Length - 1] == '\n')
                {
                    strLeft = strLeft.Remove(strLeft.Length - 1, 1);
              }
              }
              if (strLeft != "")
                {
                       while (strLeft[0] == '\r')
                {
                          strLeft = strLeft.Remove(0, 2);
                }
                }
                //处理你取出的当前行的字符串若有"\r\n"注意把它去掉
            if (curRowText != "" && curRowText.Length > 0)
                            {
                              while (curRowText[curRowText.Length - 1] == '\r' || curRowText[curRowText.Length - 1] == '\n')
                                   {
                                       curRowText = curRowText.Remove(curRowText.Length - 1, 1);
                                   }
                            }
                        String strNew = curRowText + "\r\n" + strLeft;
                       //最后前面留一行空格且把鼠标定位到此
            txtCmdInput.Text = "\r\n" + strNew;
        }

        private void 帮助ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            help help = new help();
            help.Show();
        }

        private void moshi_Click(object sender, EventArgs e)
        {
            if(moshi.Text=="强行装逼模式")
            {
         //       this.WindowState = FormWindowState.Maximized;
                textBox2.Dock = DockStyle.Fill;
                menuStrip1.Visible = false;
                textBox2.BringToFront();
                MessageBox.Show("按ECS退出装逼模式!");
                textBox2.Text = "Xiz>.<:";
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if ( e.KeyCode == Keys.Escape)
            {
                this.WindowState = FormWindowState.Normal;
                textBox2.Dock = DockStyle.None;
                menuStrip1.Visible = true;
                textBox2.SendToBack();
            }
            if (e.KeyCode == Keys.Enter)
            {
                DosZhixing();
            }
        }
    }
}
