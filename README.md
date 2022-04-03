<p align="center">
    <img src="/HuoHuan/Resources/HuoHuan.png"/>
</p>
<h1 align="center">火浣 - 微信群耙子</h1>
<div align="center">
火浣是一款主动获取网络中已公开有效微信群聊二维码图片的工具。
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
</p>

![Home Page=](/Images/HomePage.jpg)

![View Page](/Images/ViewPage.jpg)

## 现有功能

* 支持根据特定目标爬取图片(目前仅支持百度贴吧)
* 判断图片是否为微信群二维码及是否尚在可扫描有效期内
* 浏览爬取结果

## 使用方法

### 设置爬取目标

配置文件：/Data/tieba_spider_key.yaml

```yaml
- Key: "王者荣耀微信群"
  Page: 1
```

依此格式添加爬取贴吧，Key为贴吧名称，Page为爬取深度。如Page设置为1，则仅爬取贴吧首页，可根据贴吧活跃程度及爬取价值自行调整深度。

## TODO

- [ ] 爬取数据导出
- [ ] 支持更多目的站点（如豆瓣、微博等）
- [ ] 支持多线程爬取
- [ ] 增加更多软件内配置

## 注意

本项目仅供学习及技术交流使用，切勿将本项目用于非法用途，否则后果自负。



