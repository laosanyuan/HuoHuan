<p align="center">
    <img src="src/HuoHuan/Resources/HuoHuan.png" width=150/>
</p>
<h1 align="center">火浣 - 微信群聊耙子</h1>
<div align="center">
火浣是一款快捷爬虫工具，用于主动获取网络他人公开并且有效的微信群聊二维码图片。
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

## 使用效果

![Home Page](/Images/home_page.gif)

![View Page](/Images/view_page.gif)

## 简介

基于微信的功能，如有人在网络上分享某一微信群二维码，他人即可在有效期内通过此二维码扫码加群。火浣就是通过爬取目的源的页面，解析图片内容，实现获取到有效的微信群二维码。

通过加载插件的方式，将添加的各个源进行分分别执行爬取、结果统一管理，可以很方便快速的加入新的插件。如发现比较好的数据来源，可在**HuoHuan.Plugins**中分别继承IPlugin、ISpider(BaseSpider)两个接口，仅需要实现筛选图片的相关的方法后，即可在编译后直接使用。

[发布版本](https://github.com/laosanyuan/HuoHuan/releases)

## 获取群聊逻辑

1. 根据设置爬取源获取图片信息
2. 识别图片是否为二维码
3. 识别图片是否为微信群二维码
4. OCR识别文字内容，判断是否在有效期内

## 现有功能

* 支持识别、获取有效微信群聊
* 支持通过添加插件的方式快捷增加爬取源
* 支持已识别Url过滤
* 支持保存信息到本地数据库
* 支持下载相关群二维码图片至本地
* 支持浏览查看爬取结果
* 支持根据特有配置执行插件

## TODO

- [ ] 尝试其他方案，提升OCR识别准确率
- [ ] 支持爬取结果一键导出
- [ ] 增加配置代理、支持多线程爬取
- [ ] 支持插件卡片内修改插件配置
- [ ] 支持界面内操作插件的导入、卸载、排序
- [ ] 更新UI
- [ ] 增加音效提醒

## 注意

本项目仅供学习及技术交流使用，切勿将本项目用于非法用途，否则后果自负。



