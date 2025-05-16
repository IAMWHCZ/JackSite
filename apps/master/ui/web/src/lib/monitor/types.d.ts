export interface MonitorConfig {
  appId: string;
  reportUrl: string;
  debug?: boolean;
  sampleRate?: number; // 采样率 0-1
  ignoreUrls?: string[]; // 忽略的URL
  maxBreadcrumbs?: number; // 用户行为追踪的最大记录数
}

export interface MonitorData {
  type: MonitorType;
  subType: string;
  data: any;
}

export enum MonitorType {
  ERROR = 'error',
  PERFORMANCE = 'performance',
  BEHAVIOR = 'behavior',
  RESOURCE = 'resource',
  HTTP = 'http',
}