import { ReactQueryDevtools } from '@tanstack/react-query-devtools'
import { lazy, Suspense, useEffect, useState } from 'react'

export const ReactQueryDevtoolsHelper = () => {
  const [showDevtools, setShowDevtools] = useState(false)
  useEffect(() => {
    // @ts-expect-error
    window.toggleDevtools = () => setShowDevtools((old) => !old)
  }, [])
  const ReactQueryDevtoolsProduction = lazy(() =>
    import('@tanstack/react-query-devtools/build/modern/production.js').then(
      (d) => ({
        default: d.ReactQueryDevtools,
      }),
    ),
  )
  return (
    <>
      <ReactQueryDevtools initialIsOpen />
      {showDevtools && (
        <Suspense fallback={null}>
          <ReactQueryDevtoolsProduction />
        </Suspense>
      )}
    </>
  )
}
