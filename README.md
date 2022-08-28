# Genshin Proxy Server

![](https://img.shields.io/badge/Language-C%23-blue)

**原神祈愿记录代理服务器**

*跟原神私服没多大关系*

## 目录 / Table of Contents

- 背景 / Background
- 用法 / Usage
- 目标 / Goal
- 已知Bug

## 背景 / Background

*该项目目前的作用是当作[我的另一个项目](https://github.com/AuroraZiling/genshin-pray-export)的模块*

由于原神3.0更新，善良的MiHoyo将访问祈愿历史记录的"马脚"收住了，但代理还是逃不过，故有了本项目

## 用法 / Usage

### 普通用法 / Common

1. 打开`GenshinProxyServer.exe`
2. 若默认端口(8088)与已有端口冲突，请切换 *(不切换也没关系，会自动崩溃的)*
3. 点击`开始`按钮后，代理服务器会启动
4. 此时回到原神的祈愿界面，打开`历史记录`后，返回到本程序
5. 如果不出意外，`输出结果`框内会显示一大串地址
6. 这个地址即为原神访问祈愿历史记录的URL

### Python 交互用法 / Python Interact

*严格来说，不是Python也能用*

1. 在软件同目录下，创建`python_interact`文件 **没有后缀名**
2. 打开`GenshinProxyServer.exe`
3. 此时回到原神的祈愿界面，打开`历史记录`后，返回到本程序
4. 如果不出意外，软件会闪退
5. 同目录下会出现`requestUrl.txt`，内容即是原神访问祈愿历史记录的URL

为了防止出现被调用启动后关闭崩溃的问题，建议使用`subprocess.Popen()`

然后用`.wait()`判断是否被关闭

Tips: 如果你是想要将本软件用在你的程序中，**建议按照 Python 交互用法写相关模块**

## 已知Bug

1. 在端口冲突的情况下，会崩溃 (后续考虑报错/自动寻找可用端口)
2. 如果有其他软件使用代理(VPN等)，会导致无法正常启动代理服务器
