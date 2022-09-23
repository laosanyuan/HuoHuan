<p align="center">
    <img src="src/HuoHuan/Resources/HuoHuan.png" width=150/>
</p>
<h2 align="center">火浣 - 微信群聊耙子</h2>
<div align="center">
火浣是一款微信群爬虫工具，用于获取网络中他人公开并且有效的微信群聊二维码图片。
</div>

<p align="center">
	<a href="https://github.com/laosanyuan/HuoHuan/blob/master/LICENSE">
		<img alt="GitHub license" src="https://img.shields.io/github/license/laosanyuan/HuoHuan">
	</a>
    <a href="https://github.com/laosanyuan/HuoHuan/stargazers">
        <img alt="GitHub stars" src="https://img.shields.io/github/stars/laosanyuan/HuoHuan">
    </a>
    <a href="https://github.com/laosanyuan/HuoHuan/network">
        <img alt="GitHub forks" src="https://img.shields.io/github/forks/laosanyuan/HuoHuan">
    </a>
    <a>
        <img alt="Actions" src="https://github.com/laosanyuan/HuoHuan/actions/workflows/HuoHuan - CI.yml/badge.svg?event=push">
    </a>
</p>

## 简介

基于微信的功能，如有人在网络上分享某一微信群二维码，他人即可在有效期内通过此二维码扫码加群。火浣就是通过爬取目的源的页面，解析图片内容，实现获取到有效的微信群二维码。

通过加载插件的方式，将添加的各个源进行分分别执行爬取、结果统一管理，可以很方便快速的加入新的插件。如发现比较好的数据来源，可在**HuoHuan.Plugins**中分别继承IPlugin、ISpider(BaseSpider)两个接口，仅需要实现获取图片链接的相关的方法后，即可在编译后直接使用。

|                    项目地址                     |                          最新安装包                          | 版本检测、自动升级 |
| :---------------------------------------------: | :----------------------------------------------------------: | :----------------: |
| [Github](https://github.com/laosanyuan/HuoHuan) | [release-1.1.0](https://github.com/laosanyuan/HuoHuan/releases/download/1.1.0/Setup.exe) |   :pushpin: 支持   |
|   [Gitee](https://gitee.com/ylaosan/huo-huan)   | [release-1.1.0](https://gitee.com/ylaosan/huo-huan/releases/download/1.1.0/Setup.exe) |   :pushpin: 支持   |

## 使用效果

![Home Page](/images/home_page.gif)

![View Page](/images/view_page.gif)

![View Page](/images/plugins_page.gif)

## 获取群聊步骤

1. 根据插件设置爬取源获取图片链接
2. 识别图片是否为二维码
3. 判断图片是否为微信群二维码
4. OCR识别文字内容，判断群二维码是否尚在有效期内

## 现有功能

* 识别、获取有效微信群聊
* 通过添加插件的方式快捷增加爬取源
* 已识别Url过滤
* 保存信息到本地数据库
* 下载相关群二维码图片至本地
* 浏览查看爬取结果
* 插件增删、重新排序
* 根据特有配置执行插件（尚不能软件内自定义）
* 检测最新版本，下载安装包并自动启动安装
* 爬取完成音效提醒

TODO

- [ ] 尝试其他方案，提升OCR识别准确率
- [ ] 支持Arm（当前PaddleOcr仅支持Amd64下使用）
- [ ] 增加像素灰度均值判断护眼模式，取反解析内容，增加识别范围
- [ ] 增加配置代理、支持多线程爬取
- [ ] 支持爬取结果一键导出文件压缩包
- [ ] 支持插件卡片空间内修改插件配置
- [ ] 扩充插件库数量
- [ ] 更新UI

##  注意

**本项目开发初衷仅为学习及技术交流，切勿将本其用于任何非法用途，否则一切后果自负，与本项目作者无关。**



