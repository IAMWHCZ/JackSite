import React from 'react';

// 这个组件只需要在应用程序中渲染一次，通常在根组件中
export const IconSprite: React.FC = () => {
  return (
    <svg
      xmlns="http://www.w3.org/2000/svg"
      style={{
        position: 'absolute',
        width: 0,
        height: 0,
        overflow: 'hidden'
      }}
      aria-hidden="true"
    >
      <defs>
        {/* Logo 图标 */}
        <symbol id="icon-logo" viewBox="0 0 512 512">
          {/* 压缩后的 logo SVG 路径 */}
          <path d="M256 8C119 8 8 119 8 256s111 248 248 248 248-111 248-248S393 8 256 8zm0 448c-110.5 0-200-89.5-200-200S145.5 56 256 56s200 89.5 200 200-89.5 200-200 200z" />
        </symbol>
        
        {/* 可以添加更多图标 */}
      </defs>
    </svg>
  );
};

// 使用图标的组件
interface IconProps {
  name: string;
  width?: number | string;
  height?: number | string;
  color?: string;
  className?: string;
}

export const Icon: React.FC<IconProps> = ({
  name,
  width = 24,
  height = 24,
  color = 'currentColor',
  className = '',
}) => {
  return (
    <svg
      width={width}
      height={height}
      className={className}
      fill={color}
      aria-hidden="true"
    >
      <use xlinkHref={`#icon-${name}`} />
    </svg>
  );
};