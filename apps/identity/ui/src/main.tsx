import ReactDOM from 'react-dom/client'
import { RouterProvider, createRouter } from '@tanstack/react-router'
import { QueryClient, QueryClientProvider } from '@tanstack/react-query'
import { routeTree } from './routeTree.gen'
import '@/theme/global.scss'
import reportWebVitals from '@/libs/reportWebVitals.ts'
import { ReactQueryDevtoolsHelper } from '@/helper/ReactQueryDevtoolsHelper.tsx'
import { StrictMode } from 'react'

const router = createRouter({
  routeTree,
  context: {},
  defaultPreload: 'intent',
  scrollRestoration: true,
  defaultStructuralSharing: true,
  defaultPreloadStaleTime: 0,
})

declare module '@tanstack/react-router' {
  interface Register {
    router: typeof router
  }
} 
const queryClient = new QueryClient()

const rootElement = document.getElementById('app')
if (rootElement && !rootElement.innerHTML) {
  const root = ReactDOM.createRoot(rootElement)
  root.render(
    <StrictMode>
      <QueryClientProvider client={queryClient}>
        <ReactQueryDevtoolsHelper />
        <RouterProvider router={router} />
      </QueryClientProvider>
    </StrictMode>,
  )
}
reportWebVitals()
