export default function QQ({ className }: { className?: string }) {
  return (
    <svg
      className={className}
      aria-describedby="desc"
      aria-labelledby="title"
      role="img"
      viewBox="0 0 64 64"
      xmlns="http://www.w3.org/2000/svg"
      xmlnsXlink="http://www.w3.org/1999/xlink"
    >
      <title>Tencent QQ</title>
      <desc>A colored QQ icon from Orion Icon Library.</desc>
      <defs>
        {/* QQ 经典渐变色 */}
        <linearGradient id="qqGradient" x1="0%" y1="0%" x2="100%" y2="100%">
          <stop offset="0%" stopColor="#00D4FF" />
          <stop offset="50%" stopColor="#1890FF" />
          <stop offset="100%" stopColor="#0050B3" />
        </linearGradient>

        {/* 阴影效果 */}
        <filter id="shadow" x="-20%" y="-20%" width="140%" height="140%">
          <feDropShadow dx="2" dy="2" stdDeviation="3" floodColor="#0050B3" floodOpacity="0.3" />
        </filter>
      </defs>

      <path
        d="M51.035 27.026a21.136 21.136 0 0 0 .75-5.437 19.5 19.5 0 1 0-39 0 20.583 20.583 0 0 0 .75 5.438c-2.438 1.969-12 10.5-8.062 21.656 0 0 3.281-.281 6.094-5.344a21.167 21.167 0 0 0 4.5 8.625c-3.094.938-5.156 2.625-5.156 4.688 0 2.906 4.5 5.344 9.937 5.344 3.75 0 6.937-1.125 8.719-2.719.938.094 1.875.188 2.813.188s1.875-.094 2.813-.188c1.687 1.594 4.969 2.719 8.719 2.719 5.531 0 9.938-2.437 9.938-5.344 0-2.063-2.063-3.75-5.156-4.687a22.366 22.366 0 0 0 4.5-8.625c2.719 5.063 6.094 5.344 6.094 5.344 3.747-11.158-5.816-19.69-8.253-21.658z"
        data-name="layer1"
        fill="url(#qqGradient)"
        stroke="#0050B3"
        strokeLinecap="round"
        strokeLinejoin="round"
        strokeMiterlimit="10"
        strokeWidth={2}
        filter="url(#shadow)"
      />
    </svg>
  );
}
