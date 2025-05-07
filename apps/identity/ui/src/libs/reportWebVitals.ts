/**
 * 中文映射表
 */
const nameMap: Record<string, string> = {
  FCP: '首次内容绘制',
  LCP: '最大内容绘制',
  CLS: '累计布局偏移',
  FID: '首次输入延迟',
  TTFB: '首字节时间',
  INP: '互动响应时间',
}

const ratingMap: Record<string, string> = {
  good: '良好',
  needsImprovement: '需要优化',
  poor: '较差',
}

const navTypeMap: Record<string, string> = {
  reload: '刷新加载',
  navigate: '首次访问',
  back_forward: '前进/后退',
}

/**
 * 格式化并打印性能指标
 */
function logPerformanceMetric(metric: any): void {
  const { name, value, delta, rating, navigationType } = metric

  console.log(`
性能指标更新：
名称：${nameMap[name] || name}
数值：${value.toFixed(2)} ms
变化量：+${delta.toFixed(2)} ms
评分：${ratingMap[rating] || rating}
导航类型：${navTypeMap[navigationType] || navigationType}
`)
}

/**
 * 注册 Web Vitals 监听器
 */
const reportWebVitals = (onPerfEntry?: (metric: any) => void): void => {
  if (onPerfEntry && onPerfEntry instanceof Function) {
    import('web-vitals').then(({ onCLS, onINP, onFCP, onLCP, onTTFB }) => {
      ;[onCLS, onINP, onFCP, onLCP, onTTFB].forEach((cb) => cb(onPerfEntry))
    })
  } else {
    // 如果没有传入自定义处理器，默认使用 logPerformanceMetric
    import('web-vitals').then(({ onCLS, onINP, onFCP, onLCP, onTTFB }) => {
      ;[onCLS, onINP, onFCP, onLCP, onTTFB].forEach((cb) =>
        cb(logPerformanceMetric),
      )
    })
  }
}

export default reportWebVitals
