# archives 目录结构<br>
  * archives.gateway
    * 这是网关服务，使用ocelot作为核心组件，通过集成identity4中间件进行身份认证，转发上游请求到下游业务服务(archives.service)。
  * archives.identityserver
    * 这是认证服务,使用identityserver4作为核心组件，实现oAuth2的功能。
  * archives.service.api
    * 这是业务功能服务，接收上游网关的请求，通过identity4中间件获取用户标识，并实现业务功能。
