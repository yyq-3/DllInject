# DllInject
PC微信Hook防撤回(注入器)

# 客户端
  客户端主要使用C++编写DLL,注入该DLL之后微信撤回的消息内容会发送到服务端.

  [客户端源码地址](https://github.com/yyq-3/MyWXDll.git)

# 服务端
  服务端使的是Java,接受到客户端发送的POST请求后将撤回消息存入Redis中.

  [ 服务端源码地址](https://github.com/yyq-3/WeChatRevokeMsgWeb.git)

# 注入器
  注入器采用C#编写,主要用于将DLL进行注入.

  [注入器源码地址](https://github.com/yyq-3/DllInject.git)
