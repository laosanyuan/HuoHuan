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
    <a>
        <img alt="Actions" src="https://img.shields.io/github/downloads/laosanyuan/HuoHuan/total">
    </a>
</p>

## 简介

基于微信的群二维码分享功能，微信用户在网络上分享微信群聊二维码，他人即可在有效期内通过此二维码扫码加群。火浣可通过爬取特定网页页面，解析图片内容进行判断，识别、筛选、获取到尚在有效期内的微信群聊二维码并集中展示。

火浣通过加载插件的方式，将添加到其中的各个爬取源统一管理调度。插件相对独立，又可以很方便快速的加入新的插件。如用户发现比较好的数据来源，可在**HuoHuan.Plugins**工程中分别继承IPlugin、ISpider(BaseSpider)两个接口，仅需要实现获取图片链接的相关的方法后，即可在编译后直接使用。

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
* 浏览过程中删除已失效内容

TODO

- [ ] 尝试其他方案，提升OCR识别准确率
- [ ] 支持Arm（当前PaddleOcr仅支持Amd64下使用）
- [ ] 增加像素灰度均值判断护眼模式，取反解析内容，增加识别范围
- [ ] 增加配置代理、支持多线程爬取
- [ ] 支持爬取结果一键导出文件压缩包
- [ ] 支持插件卡片空间内修改插件配置
- [ ] 扩充插件库数量

## 使用注意

二维码图片来自网络，所以并不能保证准确率达到100%。爬取结果中可能会存在以下几种失效情况：

* 群二维码来源网站对二维码做了二次转换，不包含失效信息（此时以上传时间为开始时间，计算出失效时间），转换时间不准确，群二维码已失效。
* 群二维码分享后，被分享群已解散、分享人退群、入群规则修改、被投诉异常等原因，导致无法加群。
* 分享群时间不在当年，但日期恰好重合（2022年10月1日爬取到2019年10月3日失效群）。
* OCR识别解析时间错误。

对于已经失效的群聊，可以鼠标悬停至浏览区顶部点击删除按钮进行剔除。后续会增加按日期筛选机制，筛选失效时间距离当前更久的群（失效概率相对较小）。

## JetBrains开源许可

本项目重度依赖于JetBrains™ ReSharper，感谢JetBrains为本项目提供[开源许可证](https://www.jetbrains.com/community/opensource/#support)。

<figure >
    <img src="https://resources.jetbrains.com/storage/products/company/brand/logos/ReSharper_icon.png" width="200" height="200"/>
    <figcaption>Copyright © 2023 JetBrains s.r.o. </br>ReSharper and the ReSharper logo are registered trademarks of JetBrains s.r.o.</figcaption>
</figure>

##  免责声明

**本项目开发初衷仅为学习及技术交流，切勿将本其用于任何非法用途，否则一切后果自负，与本项目及作者无关。**