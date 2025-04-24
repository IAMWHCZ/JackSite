// 基础类型定义
export type ClusterConfig = {
  clusterId: string;
  loadBalancingPolicy?: 'PowerOfTwoChoices' | 'FirstAlphabetical' | 'Random' | 'RoundRobin' | 'LeastRequests';
  sessionAffinity?: SessionAffinityConfig;
  healthCheck?: HealthCheckConfig;
  httpClient?: HttpClientConfig;
  httpRequest?: ForwarderRequestConfig;
  destinations?: Record<string, DestinationConfig>;
  metadata?: Record<string, string>;
}

export type RouteConfig = {
  routeId: string;
  match: RouteMatch;
  order?: number;
  clusterId?: string;
  authorizationPolicy?: 'Anonymous' | 'Default' | string;
  rateLimiterPolicy?: string;
  outputCachePolicy?: string;
  timeoutPolicy?: string;
  timeout?: string;
  corsPolicy?: 'Default' | 'Disable' | string;
  maxRequestBodySize?: number;
  metadata?: Record<string, string>;
  transforms?: Transform[];
  sessionAffinity?: SessionAffinityConfig;
}

export type RouteMatch = {
  path?: string;
  hosts?: string[];
  methods?: string[];
  headers?: HeaderMatch[];
  queryParameters?: QueryParameterMatch[];
}

export type HeaderMatch = {
  name: string;
  values: string[];
  mode: 'ExactHeader' | 'HeaderPrefix' | 'Exists' | 'Contains' | 'NotContains' | 'NotExists';
  isCaseSensitive: boolean;
}

export type QueryParameterMatch = {
  name: string;
  values: string[];
  mode: 'Exact' | 'Prefix' | 'Exists' | 'Contains' | 'NotContains';
  isCaseSensitive: boolean;
}

export type Transform = {
  requestHeader?: string;
  set?: string;
  pathPattern?: string;
  pathRemovePrefix?: string;
  requestHeadersCopy?: boolean;
  requestHeadersRemove?: string[];
  responseHeadersCopy?: boolean;
  responseHeadersRemove?: string[];
}

export type SessionAffinityConfig = {
  enabled: boolean;
  policy?: 'Cookie' | 'CustomHeader';
  affinityKeyName?: string;
  failurePolicy?: 'Redistribute' | 'Return503Error';
  settings?: {
    customHeaderName?: string;
  };
  cookie?: {
    path?: string;
    domain?: string;
    httpOnly?: boolean;
    secure?: boolean;
    sameSite?: 'Lax' | 'Strict' | 'None';
    maxAge?: string;
    expiration?: string;
  };
}

export type HealthCheckConfig = {
  passive?: {
    enabled: boolean;
    policy?: string;
    reactivationPeriod?: string;
  };
  active?: {
    enabled: boolean;
    interval?: string;
    timeout?: string;
    policy?: string;
    path?: string;
    query?: string;
  };
}

export type HttpClientConfig = {
  ssl?: {
    allowInvalidCertificates?: boolean;
    certificateFile?: string;
    certificatePassword?: string;
    sslProtocols?: string[];
  };
  maxConnectionsPerServer?: number;
  webProxy?: {
    address?: string;
    bypassOnLocal?: boolean;
    useDefaultCredentials?: boolean;
  };
  requestHeaderEncoding?: string;
  responseHeaderEncoding?: string;
  enableMultipleHttp2Connections?: boolean;
  dangerousAcceptAnyServerCertificate?: boolean;
}

export type ForwarderRequestConfig = {
  activityTimeout?: string;
  version?: string;
  versionPolicy?: 'RequestVersionExact' | 'RequestVersionOrLower' | 'RequestVersionOrHigher';
  allowResponseBuffering?: boolean;
}

export type DestinationConfig = {
  address: string;
  health?: string;
  metadata?: Record<string, string>;
}

// YARP 配置根类型
export interface YarpConfiguration {
  urls: string;
  logging: LoggingConfiguration;
  reverseProxy: ReverseProxyConfiguration;
}

export interface LoggingConfiguration {
  logLevel: {
    default: string;
    'Microsoft.Hosting.Lifetime'?: string;
    microsoft?: string;
    yarp?: string;
  };
}

export interface ReverseProxyConfiguration {
  routes: Record<string, RouteConfig>;
  clusters: Record<string, ClusterConfig>;
}

// 请求日志类型
export type RequestLog = {
  id: number;
  path: string;
  method: string;
  queryString: string;
  requestBody: string;
  requestHeaders: string | null;
  responseHeaders: string | null;
  responseBody: string | null;
  statusCode: number;
  errorMessage: string | null;
  stackTrace: string | null;
  clientIp: string | null;
  userAgent: string | null;
  requestTime: string;  
  responseTime: string; 
  executionTime: number;
  targetService: string;
}

export type CreateClusterRequest = {
  clusterId: string;
  clusterConfig: ClusterConfig;
}