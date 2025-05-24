import { create } from 'zustand'

interface Props {
    isSideBarCollapsed: boolean
    setSideBarCollapsed: (isCollapsed: boolean) => void,
    breadcrumb: string
    setBreadcrumb: (breadcrumb: string) => void
}
export const useSystemStore = create<Props>((set) => ({
    isSideBarCollapsed: false,
    setSideBarCollapsed: (isCollapsed) => set({ isSideBarCollapsed: isCollapsed }),
    breadcrumb: 'title',
    setBreadcrumb: (breadcrumb) => set({ breadcrumb })
}))

