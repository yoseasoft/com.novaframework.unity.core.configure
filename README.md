﻿## NovaFramework - Unity 核心库

NovaFramework的配置库，提供程序启动相关的配置管理。

## 使用文档

## 注意事项

使用方式(任选其一)

1. 直接在 `manifest.json` 的文件中的 `dependencies` 节点下添加以下内容：
    ```json
        {"com.novaframework.unity.configure": "https://github.com/yoseasoft/com.novaframework.unity.configure.git"}
    ```

2. 在Unity 的`Packages Manager` 中使用`Git URL` 的方式添加库,地址为：
https://github.com/yoseasoft/com.novaframework.unity.configure.git

3. 直接下载仓库放置到Unity 项目的`Packages` 目录下，会自动加载识别。
